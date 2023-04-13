namespace ET.Server
{
    public class AccountDBDestroySystem:DestroySystem<AccountDB>
    {
        protected override void Destroy(AccountDB self)
        {
            self.Account = null;
            self.Password = null;
        }
    }
};

