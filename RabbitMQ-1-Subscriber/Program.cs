using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQ_1_Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();

            factory.Uri = new Uri("amqps://uaujyisy:H3i_uCahwKuNuShYsQa6TdDHfY4Cr6rF@tiger.rmq.cloudamqp.com/uaujyisy");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            //channel.QueueDeclare("hello-queue", true, false, false);

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume("hello-queue", false, consumer); //false=> Benim ack etmemi bekle.
            
            consumer.Received += (object sender, BasicDeliverEventArgs e)=>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                channel.BasicAck(e.DeliveryTag, false);
                Thread.Sleep(1000);
                Console.WriteLine("Coming Message: " + message);
            };

            Console.ReadLine();
        }
    }
}
