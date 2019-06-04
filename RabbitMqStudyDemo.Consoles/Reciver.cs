using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMqStudyDemo.Consoles
{
    public class Reciver
    {
        public static void Recive()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                //Port = 15672,
                UserName = "guest",
                Password = "123456"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "wei_logs",   //交换机名
                    type: ExchangeType.Fanout);    //交换类型

                //创建队列
                var queueName = "queue_test_wei";// channel.QueueDeclare().QueueName;
                Console.WriteLine("queueName--" + queueName);
                channel.QueueBind(queue: queueName,
                                  exchange: "wei_logs",
                                  routingKey: "");

                Console.WriteLine(" [*] Waiting for logs.");
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("收到的消息-- [x] {0}", message);
                    Thread.Sleep( 1000);

                    Console.WriteLine(" [x] Done");

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }

}
