using EventBusEvent;
using EventBusRabbitMQ;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Bus
{
    public class EventBusRabbitMQConsumer
    {
        private readonly IRabbitMQConnection _connection;

        public EventBusRabbitMQConsumer(IRabbitMQConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public void Consume()
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: EventBusConstansts.RegisterOrderQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += ReceivedEvent;

            channel.BasicConsume(queue: EventBusConstansts.RegisterOrderQueue, autoAck: true, consumer: consumer);

        }

        public async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if(e.RoutingKey == EventBusConstansts.RegisterOrderQueue)
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var registerOrderEvent = JsonConvert.DeserializeObject<RegisterOrderEvent>(message);

                Console.WriteLine($"Id = {registerOrderEvent.Id} , OrderDate = {registerOrderEvent.OrderDate}");
            }
        }

        public void Disconnect()
        {
            _connection.Dispose();
        }
    }
}
