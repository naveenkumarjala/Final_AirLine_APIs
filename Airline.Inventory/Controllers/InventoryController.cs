using Airline.Inventory.Models;
using Airline.Inventory.Repository;
using Airline.Inventory.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace Airline.Inventory.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        public readonly IInventoryRepository _inventory;
        public InventoryController(IInventoryRepository inventory)
        {
            _inventory = inventory;
        }
        /// <summary>
        /// Search Flights user request
        /// </summary>
        /// <param name="serachViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search-Flight")]
        public IActionResult GetAllInventories([FromBody] SerachViewModel serachViewModel)
        {
            try
            {
                // Validate modelState
                if (!ModelState.IsValid)
                {
                    return UnprocessableEntity(ModelState);
                }
                IEnumerable<Inventorys> inventories = _inventory.ShowInventories().ToList();
                IEnumerable<AirLine> Airline = _inventory.GetAirline().ToList();
                var flights = (from p in inventories
                              join e in Airline
                              on p.AirLineId equals e.AirlineId
                              where p.StartDate >= serachViewModel.FromDate && e.ActiveStatus=="Y" &&
                                                               p.FromPlace.ToLower().Contains(serachViewModel.FromPlace.ToLower()) &&
                                                             p.ToPlace.ToLower().Contains(serachViewModel.Toplace.ToLower())
                              select new
                              {
                                  p.FlightNumber,
                                  e.Name,
                                  p.FromPlace,
                                  p.ToPlace,
                                  p.BclassAvailCount,
                                  p.NBclassAvailableCount,
                                  p.TicketCost,
                                  p.startTime,
                                  p.StartDate,
                                  p.EndDate,
                                  p.Discount,
                                  p.TicketCoopun,
                                  Meal = Enum.GetName(typeof(Meals), p.Meal)
                              }).ToList();

                return Ok(flights);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }    
        }
        /// <summary>
        /// Get All inventories base on Login user id
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpGet]
        [Route("get-Flight-byID/{userid}")]
        public IActionResult GetAllInventorie(string userid)
        {
            try
            {
                IEnumerable<Inventorys> inventories = _inventory.ShowInventories().ToList();
                IEnumerable<AirLine> Airline = _inventory.GetAirline().ToList();
                var flights = (from p in inventories
                               join e in Airline
                               on p.AirLineId equals e.AirlineId
                               where p.CreatedBy==userid && p.IsActive=='Y'
                               select new
                               {
                                   p.FlightNumber,
                                   e.Name,
                                   p.FromPlace,
                                   p.ToPlace,
                                   StartDate = String.Format("{0:dd-MM-yyyy}", p.StartDate),
                                   //p.StartDate,
                                   EndDate = String.Format("{0:dd-MM-yyyy}", p.EndDate),
                                   //p.EndDate,
                                   p.startTime,
                                   p.EndTime,
                                   p.ScheduledDays,
                                   p.Instrument,
                                   p.BClassCount,
                                   p.BclassAvailCount,
                                   p.NBClassCount,
                                   p.NBclassAvailableCount,
                                   p.TicketCost,
                                   p.Rows,
                                   Meal = Enum.GetName(typeof(Meals), p.Meal)
                               }).ToList();
            
           

                return Ok(flights);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Plan inventories can add only admin 
        /// </summary>
        /// <param name="inventoryviewModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("Add-Flight")]
        public IActionResult AddNewInventory([FromBody] InventoryViewModel inventoryviewModel)
        {
            try
            {
              IEnumerable< Inventorys> inventorys1=  _inventory.ShowInventories().ToList().Where(a => a.FlightNumber == inventoryviewModel.FlightNumber);
                if(inventorys1.Count()>0)
                {
                    return Accepted(new Status { Message = "Flight Number Already Exist" });
                }
                ////Validate modelState
                if (!ModelState.IsValid)
                {
                    return UnprocessableEntity(ModelState);
                }
                Inventorys inventorys = new Inventorys();
                inventorys.FlightNumber = inventoryviewModel.FlightNumber;
                inventorys.AirLineId = inventoryviewModel.AirLineId;
                inventorys.FromPlace = inventoryviewModel.FromPlace;
                inventorys.ToPlace = inventoryviewModel.ToPlace;
                inventorys.StartDate = inventoryviewModel.StartDate;
                inventorys.EndDate = inventoryviewModel.EndDate;
                inventorys.ScheduledDays =(flightAvailable?) inventoryviewModel.ScheduledDays;
                inventorys.Instrument = inventoryviewModel.Instrument;
                inventorys.BClassCount = inventoryviewModel.BClassCount;
                inventorys.NBClassCount = inventoryviewModel.NBClassCount;
                inventorys.TicketCost = inventoryviewModel.TicketCost;
                inventorys.Discount = inventoryviewModel.Discount;
                inventorys.TicketCoopun = inventoryviewModel.TicketCoopun;
                inventorys.Rows = inventoryviewModel.Rows;
                inventorys.Meal =(Meals?) inventoryviewModel.Meal;
                inventorys.CreatedBy = inventoryviewModel.CreatedBy;
                inventorys.CreatedDate = DateTime.Now;
                inventorys.Updatedby = "Admin";
                inventorys.UpdatedDate = DateTime.Now;

                inventorys.EndTime = inventoryviewModel.EndTime;
                inventorys.startTime = inventoryviewModel.startTime;
                inventorys.BclassAvailCount = inventoryviewModel.BClassCount;
                inventorys.NBclassAvailableCount = inventoryviewModel.NBClassCount;
                inventorys.IsActive = 'Y';
                using (var scope = new TransactionScope())
                {
                    _inventory.PlanInventory(inventorys);
                    scope.Complete();
                    return Ok(new Status { Message = "Flight Details Added Successfully" });
                }
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Cancel Inventories this can access only Admin
        /// </summary>
        /// <param name="cancelViewModel"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpPost]
        [Route("cancel-Flight")]
        public IActionResult CancelInventorie([FromBody]cancelViewModel cancelViewModel)
        {
            try
            {
                Inventorys inventories = _inventory.ShowInventories().ToList().Where(o=>o.FlightNumber==cancelViewModel.FlightNumber).FirstOrDefault();
                if(inventories != null) 
                {
                    inventories.IsActive = 'N';
                    inventories.Updatedby = cancelViewModel.updatedby;
                    inventories.UpdatedDate = DateTime.Now;
                    _inventory.updateBookingCount(inventories);
                    return Ok(new { Message = "Canceled Flight Successfully Flight Number:"+cancelViewModel.FlightNumber });
                }
                
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Get Edit Serch inventory based on Flight Number
        /// </summary>
        /// <param name="flightNumber"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpGet]
        [Route("get-edit-inventories/{flightNumber}")]
        public IActionResult GetEditInventorie(string flightNumber)
        {
            try
            {
                IEnumerable<Inventorys> inventories = _inventory.ShowInventories().ToList();
                IEnumerable<AirLine> Airline = _inventory.GetAirline().ToList();
                var flights = (from p in inventories
                               join e in Airline
                               on p.AirLineId equals e.AirlineId
                               where p.FlightNumber ==flightNumber && p.IsActive == 'Y'
                               select new
                               {
                                   p.FlightNumber,
                                   p.AirLineId,
                                   p.FromPlace,
                                   p.ToPlace,
                                   StartDate = String.Format("{0:yyyy-MM-dd}", p.StartDate),
                                   EndDate = String.Format("{0:yyyy-MM-dd}", p.EndDate),
                                   p.startTime,
                                   p.EndTime,
                                   p.ScheduledDays,
                                   p.Instrument,
                                   p.BClassCount,
                                   p.BclassAvailCount,
                                   p.NBClassCount,
                                   p.NBclassAvailableCount,
                                   p.TicketCost,
                                   p.Rows,
                                   p.Meal,
                                   p.CreatedBy
                               }).ToList().FirstOrDefault();
                return Ok(flights);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Update Inventory details Accessble to Admin
        /// </summary>
        /// <param name="inventoryviewModel"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpPost]
        [Route("EditUpdateInventory")]
        public IActionResult EditUpdateInventory([FromBody] InventoryViewModel inventoryviewModel)
        {
            try
            {
                Inventorys inventorys = _inventory.ShowInventories().ToList().Where(a => a.FlightNumber == inventoryviewModel.FlightNumber).FirstOrDefault();
               
                ////Validate modelState
                if (!ModelState.IsValid)
                {
                    return UnprocessableEntity(ModelState);
                }
                int BclassCount = 0;
                int NclassCount = 0;
                if(inventorys.BClassCount!= inventoryviewModel.BClassCount)
                {
                    BclassCount = inventoryviewModel.BClassCount - inventorys.BClassCount;
                }
                if (inventorys.NBClassCount != inventoryviewModel.NBClassCount)
                {
                    NclassCount = inventoryviewModel.NBClassCount - inventorys.NBClassCount;
                }
                //Inventorys inventorys = new Inventorys();
                inventorys.FlightNumber = inventoryviewModel.FlightNumber;
                inventorys.AirLineId = inventoryviewModel.AirLineId;
                inventorys.FromPlace = inventoryviewModel.FromPlace;
                inventorys.ToPlace = inventoryviewModel.ToPlace;
                inventorys.StartDate = inventoryviewModel.StartDate;
                inventorys.EndDate = inventoryviewModel.EndDate;
                inventorys.ScheduledDays = (flightAvailable?)inventoryviewModel.ScheduledDays;
                inventorys.Instrument = inventoryviewModel.Instrument;
                inventorys.BClassCount = inventoryviewModel.BClassCount;
                inventorys.NBClassCount = inventoryviewModel.NBClassCount;
                inventorys.TicketCost = inventoryviewModel.TicketCost;
                inventorys.Discount = inventoryviewModel.Discount;
                inventorys.TicketCoopun = inventoryviewModel.TicketCoopun;
                inventorys.Rows = inventoryviewModel.Rows;
                inventorys.Meal = (Meals?)inventoryviewModel.Meal;
               
                inventorys.Updatedby = inventoryviewModel.CreatedBy;
                inventorys.UpdatedDate = DateTime.Now;

                inventorys.EndTime = inventoryviewModel.EndTime;
                inventorys.startTime = inventoryviewModel.startTime;
                inventorys.BclassAvailCount = inventoryviewModel.BClassCount+BclassCount;
                inventorys.NBclassAvailableCount = inventoryviewModel.NBClassCount+NclassCount;
                inventorys.IsActive = 'Y';
                using (var scope = new TransactionScope())
                {
                    _inventory.updateBookingCount(inventorys);
                    scope.Complete();
                    return Accepted(new Status { Message = "Flight Details Updated Successfully" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Get All Airline Details 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("get-all-Airlines")]
        public IActionResult GetAllAirline()
        {
            try
            {
                IEnumerable<AirLine> Airline = _inventory.GetAirline().ToList();
                var flights = (from 
                                e in Airline
                               where e.ActiveStatus=="Y"
                               select new
                               {
                                   e.AirlineId,
                                   e.Name,
                                   e.Address,
                                   e.ContactNumber
                               }).ToList();
                return Ok(flights);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// add new Airline
        /// </summary>
        /// <param name="airlineViewModel"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpPost]
        [Route("Add-Airline")]
        public IActionResult AddNewAirline([FromBody] airlineViewModel airlineViewModel)
        {
            try
            {
                IEnumerable<AirLine> airLines = _inventory.GetAirline().ToList().Where(a => a.Name == airlineViewModel.AirlineName);
                if (airLines.Count() > 0)
                {
                    return Accepted(new Status { Message = "AirLine Already Exist" });
                }
                ////Validate modelState
                if (!ModelState.IsValid)
                {
                    return UnprocessableEntity(ModelState);
                }
                AirLine airLine = new AirLine();
                airLine.Name = airlineViewModel.AirlineName;
                airLine.Address = airlineViewModel.Address;
                airLine.ContactNumber = airlineViewModel.Contact;
                airLine.ActiveStatus = "Y";
                airLine.CreatedBy = airlineViewModel.CreatedBy;
                airLine.CreatedDate = DateTime.Now;
                airLine.Updatedby = "";

                using (var scope = new TransactionScope())
                {
                    _inventory.PlanAirline(airLine);
                    scope.Complete();
                    return Accepted(new Status { Message = "Airline Details Added Successfully" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Cancel Wxisting AirLine
        /// </summary>
        /// <param name="cancelAirlineViewModel"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpPost]
        [Route("cancel-Airline")]
        public IActionResult CancelAirline([FromBody] CancelAirlineViewModel cancelAirlineViewModel)
        {
            try
            {
                AirLine airLine = _inventory.GetAirline().ToList().Where(o => o.AirlineId == cancelAirlineViewModel.airlineId).FirstOrDefault();
                if (airLine != null)
                {
                    airLine.ActiveStatus = "N";
                    airLine.Updatedby = cancelAirlineViewModel.updatedBy;
                    airLine.UpdatedDate = DateTime.Now;
                    _inventory.UpdateAirline(airLine);
                    return Ok(new { Message = "Canceled Airline Successfully AirLine Name:" + airLine.Name });
                }
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Get Airline Details By ID
        /// </summary>
        /// <param name="airlineId"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpGet]
        [Route("get-edit-airline/{airlineId}")]
        public IActionResult GetEditAirline(int airlineId)
        {
            try
            {
                IEnumerable<AirLine> Airline = _inventory.GetAirline().ToList();
                var Airlinedetails = (from
                                e in Airline
                                where e.AirlineId==airlineId && e.ActiveStatus=="Y"
                               select new
                               {
                                   e.AirlineId,
                                   e.Name,
                                   e.Address,
                                   e.ContactNumber
                               }).ToList().FirstOrDefault();
                return Ok(Airlinedetails);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Update Airline Details 
        /// </summary>
        /// <param name="updaeAirlineViewModel"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpPost]
        [Route("EditUpdateAirline")]
        public IActionResult EditUpdateInventory([FromBody] updaeAirlineViewModel updaeAirlineViewModel)
        {
            try
            {
                ////Validate modelState
                if (!ModelState.IsValid)
                {
                    return UnprocessableEntity(ModelState);
                }
                AirLine airline = _inventory.GetAirline().ToList().Where(o => o.AirlineId == updaeAirlineViewModel.AirlineId).FirstOrDefault();
                airline.Name = updaeAirlineViewModel.Name;
                airline.Address = updaeAirlineViewModel.Address;
                airline.ContactNumber = updaeAirlineViewModel.ContactNumber;
                airline.Updatedby = updaeAirlineViewModel.UpdatedBy;
                airline.UpdatedDate = DateTime.Now;
                using (var scope = new TransactionScope())
                {
                    _inventory.UpdateAirline(airline);
                    scope.Complete();
                    return Accepted(new Status { Message = "AirLine Details Updated Successfully" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


    }
}
