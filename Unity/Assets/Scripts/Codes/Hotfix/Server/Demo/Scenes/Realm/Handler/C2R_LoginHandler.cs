using System;
using System.Net;

namespace ET.Server
{
    [MessageHandler(SceneType.Realm)]
    [FriendOf(typeof (AccountDB))]
    public class C2R_LoginHandler: AMRpcHandler<C2R_Login, R2C_Login>
    {
        protected override async ETTask Run(Session session, C2R_Login request, R2C_Login response)
        {
            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            if (string.IsNullOrEmpty(request.Account) || string.IsNullOrEmpty(request.Password))
            {
                response.Error = ErrorCode.ERR_AccontOrPasswardErr;
                response.Message = "账号密码不能为空!";
                session.Disconnect().Coroutine();
                return;
            }

            string account = request.Account;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.AccountLogin, account.GetHashCode()))
            {
                AccountDB accountDB = null;
                var list = await session.GetDirectDB().Query<AccountDB>(db => db.Account == account);
                if (list.Count > 0)
                {
                    accountDB = list[0];
                }

                if (accountDB == null)
                {
                    accountDB = session.AddChild<AccountDB>();
                    accountDB.Account = account;
                    accountDB.Password = request.Password;
                    await session.GetDirectDB().Save(accountDB);
                }
                else
                {
                    if (accountDB.Password != request.Password)
                    {
                        response.Error = ErrorCode.ERR_AccontOrPasswardErr;
                        response.Message = "账号密码错误!";
                        return;
                    }
                }
            }

            // 随机分配一个Gate
            StartSceneConfig config = RealmGateAddressHelper.GetGate(session.DomainZone());
            Log.Debug($"gate address: {MongoHelper.ToJson(config)}");

            // 向gate请求一个key,客户端可以拿着这个key连接gate
            G2R_GetLoginKey g2RGetLoginKey = (G2R_GetLoginKey)await ActorMessageSenderComponent.Instance.Call(
                config.InstanceId, new R2G_GetLoginKey() { Account = request.Account });

            response.Address = config.InnerIPOutPort.ToString();
            response.Key = g2RGetLoginKey.Key;
            response.GateId = g2RGetLoginKey.GateId;
        }
    }
}