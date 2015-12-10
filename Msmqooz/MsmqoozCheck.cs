using BoxedIce.ServerDensity.Agent.PluginSupport;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.Text;

namespace Msmqooz
{
    public class MsmqoozCheck : ICheck
    {
        
        public string Key
        {
            get { return "plugin1"; }
        }

        public object DoCheck()
        {
            PerformanceCounterCategory myCat = new PerformanceCounterCategory("MSMQ Queue");
            PerformanceCounter cntr = new PerformanceCounter();

            cntr.CategoryName = "MSMQ Queue";
            cntr.CounterName = "Messages in Queue";

            foreach (string inst in myCat.GetInstanceNames())
            {
                cntr.InstanceName = inst;
                Console.Write(inst + " = ");
                Console.WriteLine(cntr.NextValue().ToString());
            }

            MessageQueue[] queues = MessageQueue.GetPrivateQueuesByMachine(Environment.MachineName);
            
                var counter = 
                new PerformanceCounter("MSMQ Queue",  "Messages in Queue", @"os:private$\openbidder.impressions.queue");

            Console.WriteLine("Queue contains {0} messages", counter.NextValue().ToString());

            IDictionary<string, object> values = new Dictionary<string, object>();
            values.Add("hats", 5);
            values.Add("Dinosaur Rex", 25.4);
            return values;
        }

        private List<String> GetQueueNames()
        {
            List<String> names = new List<String>();
            MessageQueue[] queues = MessageQueue.GetPrivateQueuesByMachine(Environment.MachineName);

            if (queues.Any())
                names = queues.Select(a => a.QueueName).ToList();

            return names;
        }

        private int GetQueueMessageCount(String queuePath)
        {
            return 0;
        }


        private Dictionary<String, String> GetConfigs()
        {
            //ConfigurationManager

            return null;
        }



    }
}
