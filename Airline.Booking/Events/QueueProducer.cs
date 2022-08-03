using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
namespace Airline.Booking.Events
{
    public static class QueueProducer
    {
        public static void Publish(IModel channel, int Seattype  , string Flightid,string FrmPlace,string ToPlace,int SeatCount,int TicketType)
        {
            channel.QueueDeclare("demo-queue2",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );
           
           
               // var message = new { SeatType= Seattype,FlightID=Flightid,FromPlace=FrmPlace,Toplace=ToPlace,SeatCount=SeatCount,TicketType= TicketType };
            var message = Seattype + "," + Flightid + "," + FrmPlace+","+ ToPlace+","+ SeatCount+","+ TicketType;
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.BasicPublish("", "demo-queue2", null, body);
            
           
        }

    }
}
