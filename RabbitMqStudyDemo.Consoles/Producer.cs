using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMqStudyDemo.Consoles
{
    public class Producer
    {
        public static void Send(int i)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                //Port = 15672,
                UserName = "guest",
                Password = "123456"
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "wei_logs",   //交换机名
                        type: ExchangeType.Fanout);   //交换类型
                    var queueName = "queue_test_wei";
                    channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    // Guid
                    var message = i.ToString() + " - 随机生成Guid";
                    var body = Encoding.UTF8.GetBytes(message);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;// .SetPersistent .SetPersistent(true);
                    channel.BasicPublish(exchange: "wei_logs",
                                         routingKey: "",
                                         basicProperties: properties,
                                         body: body);

                    Console.WriteLine(" [x] Sent {0}", "发送的消息" +message);
                }

               // Console.WriteLine(" Press [enter] to exit.");
                //Console.ReadLine();
            }
        }
    }
}
