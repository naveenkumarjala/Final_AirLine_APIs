using Airline.Inventory.Consumer;
using Airline.Inventory.DBContext;
using Airline.Inventory.Models;
using Airline.Inventory.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Airline.Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Update : ControllerBase
    {
        
        IInventoryRepository _inventory;
        public InventoryDbContext _inventoryDbContext;
        //public Update()
        //{


        //    _inventory = _Invetory;
        //}
        public Update(IServiceProvider serviceProvider, InventoryDbContext inventoryDbContext)
        {

            _inventoryDbContext = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<InventoryDbContext>();
            //_inventoryDbContext = inventoryDbContext;
        }
        public string updateseats( int Settype, string FlightNumber, string FromPlace, string ToPlace, int NumberOfTickets, int tickettype)
        {

            try

            {
                Inventorys inventory = _inventoryDbContext.tblInventoy.Where(a =>
                                                                     a.FromPlace.ToLower().Contains(FromPlace.ToLower()) &&
                                                                   a.ToPlace.ToLower().Contains(ToPlace.ToLower()) &&
                                                                    a.FlightNumber == FlightNumber
                                                                 
                                                                  ).FirstOrDefault();
                //_inventory.ShowInventories().Where(a => 
                //                                                 a.FromPlace.ToLower().Contains(FromPlace.ToLower()) &&
                //                                               a.ToPlace.ToLower().Contains(ToPlace.ToLower()) &&
                //                                                a.FlightNumber == FlightNumber
                //                                              // && a.startTime == startTime
                //                                              ).FirstOrDefault();
                if (tickettype == 0)
                {
                    if (Settype == 1)
                    {
                        inventory.BclassAvailCount = inventory.BclassAvailCount - NumberOfTickets;
                        inventory.Updatedby = "user";
                        inventory.UpdatedDate = DateTime.Now;
                    }
                    else if (Settype == 2)
                    {
                        inventory.BclassAvailCount = inventory.NBclassAvailableCount - NumberOfTickets;
                        inventory.Updatedby = "user";
                        inventory.UpdatedDate = DateTime.Now;
                    }
                }
                else if (tickettype == 1)
                {
                    if (Settype == 1)
                    {
                        inventory.BclassAvailCount = inventory.BclassAvailCount + 1;
                        inventory.Updatedby = "user";
                        inventory.UpdatedDate = DateTime.Now;
                    }
                    else if (Settype == 2)
                    {
                        inventory.BclassAvailCount = inventory.NBclassAvailableCount + 1;
                        inventory.Updatedby = "user";
                        inventory.UpdatedDate = DateTime.Now;
                    }
                }
                //_inventory.updateBookingCount(inventory);
                _inventoryDbContext.Entry(inventory).State = EntityState.Modified;
                _inventoryDbContext.SaveChanges();

                return "Updated Succusfully";
            }
            catch (Exception ex)
            {
                return "Somthing went wrong";
            }


        }

       
    }
}
