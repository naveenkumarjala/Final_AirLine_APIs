using Airline.Booking.Events;
using Airline.Booking.Models;
using Airline.Booking.Services;
using Airline.Booking.ViewModels;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Transactions;

namespace Airline.Booking.Controllers
{
   // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        public readonly IBookingRepository _userRepository;
        //private ITopicProducer<BookingEvent> _topicProducer;
        public BookingsController(IBookingRepository userRepository /*, ITopicProducer<BookingEvent> topicProducer*/)
        {
            _userRepository = userRepository;
           // _topicProducer = topicProducer;
        }

       
        [HttpGet]
        [Route("Gell-All-Flights")]
        public IActionResult GetallInventory()
        {
            try
            {
                var users = _userRepository.GetInventorys().ToList();
                return new OkObjectResult(users);
            }
            catch
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// 

       
        [HttpPost]
        [Route("insert-booking-details")/*, Authorize(Roles = "1")*/]
        public async Task<IActionResult> InsertUserDetails([FromBody] BookingViewModel[] bookingViewModel)
        {
            try
            {
                
                IEnumerable<UsersViewModel> _usersViewModels = bookingViewModel[0].BookingUsers;
                int setCount = _usersViewModels.Count();
                Inventorys _inventorys = null;
                Bookings bookings;
                if (Convert.ToInt32(bookingViewModel[0].Seattype) == 1)
                {
                    _inventorys = _userRepository.GetInventorys().ToList()
                        .Where(o => o.FlightNumber == bookingViewModel[0].FlightNumber && o.BclassAvailCount >= 1 && o.StartDate >= bookingViewModel[0].DateOfJourney).FirstOrDefault();
                }
                else if (Convert.ToInt32(bookingViewModel[0].Seattype) == 2)
                {
                    _inventorys = _userRepository.GetInventorys().ToList()
                        .Where(o => o.FlightNumber == bookingViewModel[0].FlightNumber && o.NBclassAvailableCount >= 1 && o.StartDate >= bookingViewModel[0].DateOfJourney).FirstOrDefault();
                }

                if (_inventorys == null)
                {
                    return BadRequest("Invalid Flight Number or Seats not Available");
                }
                string BookingId = GenerateBookingID();
                string flightNumber = bookingViewModel[0].FlightNumber;
                DateTime DateOfJourney = bookingViewModel[0].DateOfJourney;
                string FromPlace = bookingViewModel[0].FromPlace;
                string ToPlace = bookingViewModel[0].ToPlace;
                string BoardingTime = bookingViewModel[0].BoardingTime;
                string CreatedBy = bookingViewModel[0].CreatedBy;
                string EmailID = bookingViewModel[0].EmailID;
                Decimal TicketCost= bookingViewModel[0].TicketCost;
                Decimal Discount = bookingViewModel[0].Discount;
                int seatNumber = 0;
                if (Convert.ToInt32(bookingViewModel[0].Seattype) == 1)
                {
                    seatNumber = (int)(_inventorys.BClassCount - _inventorys.BclassAvailCount) + 1;
                }
                else if (Convert.ToInt32(bookingViewModel[0].Seattype) == 2)
                {
                    seatNumber = (int)(_inventorys.NBclassAvailableCount - _inventorys.NBclassAvailableCount) + 1;
                }
                #region
                foreach (UsersViewModel usersViewModel in _usersViewModels)
                {
                    bookings = new Bookings();
                    bookings.TicketID = GenerateticketID();
                    bookings.BookingID = BookingId;
                    bookings.FlightNumber = flightNumber;
                    bookings.DateOfJourney = DateOfJourney;
                    bookings.FromPlace = FromPlace;
                    bookings.ToPlace = ToPlace;
                    bookings.BoardingTime = BoardingTime;
                    bookings.EmailID = EmailID;
                    bookings.UserName = usersViewModel.UserName;
                    bookings.passportNumber = usersViewModel.passportNumber;
                    bookings.Age = usersViewModel.Age;
                    bookings.SeatNumber = seatNumber;
                    bookings.Status = 0;
                    bookings.Statusstr = "Ticket Booked";
                    bookings.TicketCost = TicketCost;
                    bookings.Discount = Discount;
                    bookings.CreatedBy = CreatedBy;
                    bookings.CreatedDate = DateTime.Now;
                    bookings.Seattype = bookingViewModel[0].Seattype;
                    bookings.Updatedby = EmailID;
                    bookings.UpdatedDate = DateTime.Now;
                    using (var scope = new TransactionScope())
                    {
                        _userRepository.Insert(bookings);
                        scope.Complete();
                    }
                    seatNumber++;
                }
                #endregion


                //await _topicProducer.Produce(new BookingEvent
                //{
                //    FlightNumber = flightNumber,
                //    FromPlace = FromPlace,
                //    ToPlace = ToPlace,
                //    StartDate = DateOfJourney,
                //    startTime = BoardingTime,
                //    NumberOfTickets = 1,
                //    Settype = (int)(Seattype)bookingViewModel[0].Seattype,
                //    tickettype = 0
                //});


                var factory = new ConnectionFactory
                {
                    Uri = new Uri("amqp://guest@localhost:5672")
                };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                QueueProducer.Publish(channel, Convert.ToInt32(bookingViewModel[0].Seattype), flightNumber, FromPlace, ToPlace, setCount, 0);


                return Ok(new  { PNR = BookingId });
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Generate UniQue TiketID
        /// </summary>
        /// <returns></returns>
        private string GenerateticketID()
        {
            int count = _userRepository.GetBookings().ToList().Count();
            string strSecretCode = string.Empty;
            string strguid = string.Empty;
            string strYearCode = string.Empty;
            string TicketID = string.Empty;
            try
            {
                System.Guid guid = System.Guid.NewGuid();
                strguid = guid.ToString();
                strSecretCode = strguid.Substring(strguid.LastIndexOf("-") + 1);
                strSecretCode = strSecretCode.ToUpper().Replace('O', 'W').Replace('0', '4');
                strSecretCode = strSecretCode.Substring(0, 6);

                TicketID = "TIC" + strSecretCode.ToUpper()+ count;

                return TicketID;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return TicketID;
            }
           
        }
        /// <summary>
        /// Genarate unique Booking iD
        /// </summary>
        /// <returns></returns>
        private string GenerateBookingID()
        {
            int count = _userRepository.GetBookings().ToList().Count();
            string strSecretCode = string.Empty;
            string strguid = string.Empty;
            string strYearCode = string.Empty;
            string TicketID = string.Empty;
            try
            {
                System.Guid guid = System.Guid.NewGuid();
                strguid = guid.ToString();
                strSecretCode = strguid.Substring(strguid.LastIndexOf("-") + 1);
                strSecretCode = strSecretCode.ToUpper().Replace('O', 'W').Replace('0', '4');
                strSecretCode = strSecretCode.Substring(0, 6);

                TicketID = "BOK" + strSecretCode.ToUpper() + count;

                return TicketID;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return TicketID;
            }

        }

        /// <summary>
        /// Get All User Tickets
        /// </summary>
        /// <returns></returns>
        
        [HttpGet]
        [Route("get-all-Tickets"), Authorize(Roles = "1")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _userRepository.GetBookings().ToList();
                return new OkObjectResult(users);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Cancel Ticket
        /// </summary>
        /// <param name="TicketID"></param>
        /// <returns></returns>
        
        [HttpGet]
        [Route("cancel-ticket/{TicketID}")/*, Authorize(Roles = "0")*/]
        public async Task<IActionResult> CancelTicket(string TicketID)
        {
            try
            {
                Bookings bookings = _userRepository.GetBookings().ToList().Where(o => o.TicketID == TicketID ).FirstOrDefault();
                #region
                //foreach (Bookings booking in bookings)
                //{
                //    booking.Status = 1;
                //    booking.Statusstr = "Canceled Ticket";
                //    using (var scope = new TransactionScope())
                //    {
                //        _userRepository.UpdateBooking(booking);


                //        await _topicProducer.Produce(new BookingEvent
                //        {
                //            FlightNumber = booking.FlightNumber,
                //            FromPlace = booking.FromPlace,
                //            ToPlace = booking.ToPlace,
                //            StartDate = booking.DateOfJourney,
                //            startTime = booking.BoardingTime,
                //            NumberOfTickets = 1,
                //            Settype = (int)(Seattype)booking.Seattype,
                //            tickettype=1,
                //        });
                //        scope.Complete();
                //    }

                //}
                #endregion
                using (var scope = new TransactionScope())
                {
                    bookings.Status = 1;
                    bookings.Statusstr = "Canceled Ticket";
                    _userRepository.UpdateBooking(bookings); 
                    scope.Complete();
                }
                //await _topicProducer.Produce(new BookingEvent
                //{
                //    FlightNumber = bookings.FlightNumber,
                //    FromPlace = bookings.FromPlace,
                //    ToPlace = bookings.ToPlace,
                //    StartDate = bookings.DateOfJourney,
                //    startTime = bookings.BoardingTime,
                //    NumberOfTickets = 1,
                //    Settype = (int)(Seattype)bookings.Seattype,
                //    tickettype = 1,
                //});
                var factory = new ConnectionFactory
                {
                    Uri = new Uri("amqp://guest@localhost:5672")
                };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                QueueProducer.Publish(channel, (int)(Seattype)bookings.Seattype, bookings.FlightNumber, bookings.FromPlace, bookings.ToPlace, 1, 1);

                return Ok(new Status {TicketID=TicketID ,CancelStatus = "cancel"});
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// PNR STATUS Check
        /// </summary>
        /// <param name="TicketID"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("pnr-ticket/{TicketID}")]
        public IActionResult GetpnrTicket(string TicketID)
        {
            try
            {
                IEnumerable<Bookings> bookings = _userRepository.GetBookings().ToList()
                                                .Where(o => o.TicketID.ToUpper()== TicketID.ToUpper() 
                                                || o.BookingID.ToUpper()==TicketID.ToUpper() || o.EmailID == TicketID);
                var flights = (from p in bookings

                              // where (p.TicketID == TicketID || p.BookingID==TicketID || p.EmailID == TicketID)
                               select new
                               {
                                   p.TicketID,
                                   p.BookingID,
                                   p.FlightNumber,
                                   p.DateOfJourney,
                                   p.BoardingTime,
                                   p.EmailID,
                                   p.FromPlace,
                                   p.ToPlace,
                                   p.UserName,
                                   p.passportNumber,
                                   p.Age,
                                   TicketType = Enum.GetName(typeof(Seattype), p.Seattype),
                                   p.Statusstr,
                                   p.SeatNumber,p.Status
                                   
                               }).ToList();
                return new OkObjectResult(flights);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Get All Inventories
        /// </summary>
        /// <returns></returns>
       

    }
}
