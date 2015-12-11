using BoxedIce.ServerDensity.Agent.PluginSupport;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;

namespace Msmqooz
{
    public class MsmqoozCheck : ICheck
    {
        private const String QueuesKey = "Msmqooz-Queues";
        private readonly static ILog Log = LogManager.GetLogger(typeof(PluginMetadata));

        public string Key
        {
            get { return "Msmqooz"; }
        }

        public object DoCheck()
        {
            IDictionary<string, object> values = new Dictionary<string, object>();

            List<String> queuesInConfig = GetQueueNamesFromConfig(ConfigurationManager.AppSettings[QueuesKey]);
            List<String> queuesInMachine = GetQueueNamesByMachine();

            if(queuesInConfig.Any())
            {
                foreach (String queue in queuesInConfig)
                {
                    String fullQueueName = queuesInMachine.Where(a => a.Trim().ToLower().Contains(queue.Trim().ToLower())).FirstOrDefault();

                    if (String.IsNullOrEmpty(fullQueueName))
                    {
                        values.Add(queue, 0);
                    }
                    else
                    {
                        int count = GetQueueMessageCount(fullQueueName);
                        values.Add(queue, count);
                    }
                }

                if(values.Any())
                    values.Add("total_message_count", values.Sum(a => (int)a.Value));
            }

            return values;
        }

        private List<String> GetQueueNamesByMachine()
        {
            List<String> names = new List<String>();

            MessageQueue[] queues = MessageQueue.GetPrivateQueuesByMachine(Environment.MachineName);

            if (queues.Any())
                names.AddRange(queues.Select(a => a.MachineName + Path.DirectorySeparatorChar +  a.QueueName).ToList());

            return names;
        }

        private int GetQueueMessageCount(String queuePath)
        {
            int i = 0;

            try
            {
                MessageQueue queue = new MessageQueue(queuePath);
                MessageEnumerator messageEnumerator = queue.GetMessageEnumerator2();

                while (messageEnumerator.MoveNext())
                    i++;
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message, ex);
            }

            return i;
        }

        private List<String> GetQueueNamesFromConfig(String queues)
        {
            List<String> names = new List<String>(); 

            if (!String.IsNullOrEmpty(queues))
            {
                String[] splits = queues.Split(',');

                if (splits.Any())
                    names = splits.ToList();
            }

            return names;
        }

    }
}