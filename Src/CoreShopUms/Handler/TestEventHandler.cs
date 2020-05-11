using DotNetCore.CAP;

namespace CoreShopUms.Handler
{

    public class TestEventHandler : ICapSubscribe
    {

        [CapSubscribe("a")]
        public void AReceivedMessage(string msg)
        {


        }
    }
}
