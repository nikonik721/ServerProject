using Newtonsoft.Json;
using PS.Models.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PS.DL.Rabbit
{
    public class RabbitProducer
    {

        public static void Publish(IModel channel)
        {

            channel.QueueDeclare(queue: "Envelope-Queue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            //Tuk 6te se pra6tat pisma
            while (true)
            {
                channel.BasicPublish("", "Envelope-Queue", null, null);
                Thread.Sleep(100);
            }
        }

    }
}
