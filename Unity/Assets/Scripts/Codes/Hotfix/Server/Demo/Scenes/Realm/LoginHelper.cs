namespace ET.Server
{
    public static class LoginHelper
    {
        public static async ETTask Disconnect(this Session self)
        {
            if (self == null)
            {
                return;
            }

            long instanceId = self.InstanceId;

            await TimerComponent.Instance.WaitAsync(1000);
            
            if (instanceId != self.InstanceId)
            {
                return;
            }

            self.Dispose();
        }
    }
}