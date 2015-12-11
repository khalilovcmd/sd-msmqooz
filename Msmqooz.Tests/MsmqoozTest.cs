using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Messaging;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.IO;

namespace Msmqooz.Tests
{
    [TestClass]
    public class MsmqoozTest
    {
        public String QueueName;
        public String FullQueueName;

        [TestInitialize]
        public void Initialize()
        {
            QueueName = "test_queue_msmqooz";
            FullQueueName = Environment.MachineName + @"\private$\" + QueueName;

            MessageQueue[] queues = MessageQueue.GetPrivateQueuesByMachine(Environment.MachineName);

            if (!queues.Any(q => q.QueueName.Contains(QueueName)))
                MessageQueue.Create(FullQueueName);
            else
                MessageQueue.Delete(FullQueueName);

            MessageQueue queue = new MessageQueue(FullQueueName);

            for (int i = 0; i < 10; i++)
                queue.Send("This is a sample message: " + i);
        }

        [TestMethod]
        public void CanGetMessageNamesFromConfig_DefaultValues_ShouldReturnFourNames()
        {
            PrivateObject test = new PrivateObject(new MsmqoozCheck());
            List<String> queues = test.Invoke("GetQueueNamesFromConfig", new Object[] { "queueA,queueB,queueC,queueD" }) as List<String>;

            Assert.IsTrue(queues.Count == 4);
        }

        [TestMethod]
        public void CanGetMessageNamesFromConfig_NullValue_ShouldReturnFourNames()
        {
            PrivateObject test = new PrivateObject(new MsmqoozCheck());
            List<String> queues = test.Invoke("GetQueueNamesFromConfig", new Object[] { null}) as List<String>;

            Assert.IsTrue(queues.Count == 0);
        }

        [TestMethod]
        public void CanGetQueueNamesByMachine_DefaultValues_ShouldReturnAtleastOnePrivateQueue()
        {
            PrivateObject test = new PrivateObject(new MsmqoozCheck());
            List<String> queues = test.Invoke("GetQueueNamesByMachine") as List<String>;

            Assert.IsTrue(queues.Count >= 1);
        }

        [TestMethod]
        public void CanGetQueueMessageCount_DefaultValues_ShouldReturn10Messages()
        {
            PrivateObject test = new PrivateObject(new MsmqoozCheck());
            int count = (int)test.Invoke("GetQueueMessageCount", new Object[] { FullQueueName });

            Assert.IsTrue(count == 10);
        }

        [TestMethod]
        public void CanDoCheck_DefaultValues_ShouldReturnOneQueueWith10MessagesCount()
        {
            PrivateObject test = new PrivateObject(new MsmqoozCheck());
            IDictionary<string, object> count = test.Invoke("DoCheck") as IDictionary<string, object>;

            Assert.IsTrue(count.Any(a => a.Key == QueueName && (int)a.Value == 10));
        }

        [TestCleanup]
        public void Cleanup()
        {
            MessageQueue.Delete(FullQueueName);
        }
    }
}