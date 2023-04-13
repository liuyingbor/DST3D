namespace ET.Server
{
    [ChildOf(typeof(Session))]
    public class AccountDB: Entity, IAwake, IDestroy
    {
        public string Account = default;
        public string Password = default;
    }
}