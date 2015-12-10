using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Messaging;
using System.Linq;

namespace Msmqooz.Tests
{
    [TestClass]
    public class MsmqoozTest
    {
        [TestInitialize]
        public void Initialize()
        {
            MessageQueue[] queues = MessageQueue.GetPrivateQueuesByMachine(Environment.MachineName);

            if (MessageQueue.Exists(@"t-k\private$\test_queue"))
                MessageQueue.Create(@"t-k\private$\test_queue");
        }

        [TestMethod]
        public void Test()
        {
            new MsmqoozCheck().DoCheck();
        }
    }
}
