using Airline.Inventory.Controllers;
using Airline.Inventory.DBContext;
using Airline.Inventory.Repository;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Airline.Inventory.Consumer
{
    public class QueueConsumer : IHostedService
    {
      
        private readonly IModel channel;
        private readonly IConnectionFactory _factory;
        private readonly IServiceProvider _serviceProvider;
        private readonly IInventoryRepository _Irepository;
        private readonly InventoryDbContext _inventoryDbContext;

        public QueueConsumer(IServiceProvider serviceProvider)
        {
            _factory = new ConnectionFactory { Uri = new Uri("amqp://guest:guest@localhost:5672") };
            //  _Irepository = irepository;
            _serviceProvider = serviceProvider;
              channel = _factory.CreateConnection().CreateModel();
        }

        public void Consume()
        {

            string[] msg;

            channel.QueueDeclare("demo-queue2",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();

                var message = Encoding.UTF8.GetString( body);
                msg = message.Split(',');
                int SeatType = Convert.ToInt32(msg[0].Substring(1));
                string Flightid = msg[1];
                string FrmPlace = msg[2];
                string Toplace=msg[3];
                int SeatCount = Convert.ToInt32(msg[4]);
                int TicketType = Convert.ToInt32(msg[5].Substring(0, msg[5].Length - 1));
              
                new Update(_serviceProvider , _inventoryDbContext).updateseats(SeatType, Flightid, FrmPlace, Toplace, SeatCount, TicketType);
            };

           
            channel.BasicConsume("demo-queue2", true, consumer);


        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Consume();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Consume();
            return Task.CompletedTask;
        }
    }
}
