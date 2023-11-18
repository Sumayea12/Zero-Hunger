using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.DTOs;
using ZeroHunger.EF;

namespace ZeroHunger.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CollectionRequests()
        {
            int? restaurantOwnerId = null;

            var cookie = HttpContext.Request.Cookies["UserInfo"];

            if (cookie != null && !string.IsNullOrEmpty(cookie["Id"]))
            {
                int parsedId;
                if (int.TryParse(cookie["Id"], out parsedId))
                {
                    restaurantOwnerId = parsedId;
                }
            }

            var db = new ZeroHungerEntities();

            var collectionRequests = db.CollectionRequests.Where(c => c.Status == "Not Collected" && c.employeeId == restaurantOwnerId).ToList();
            if(collectionRequests.Any())
            {
                var config1 = new MapperConfiguration(cfg => {
                    cfg.CreateMap<CollectionRequest, CollectionRequestDTO>();
                });
                var mapper1 = new Mapper(config1);
                var collectionRequestDTOs = mapper1.Map<List<CollectionRequestDTO>>(collectionRequests);

               
                return View(collectionRequestDTOs);
            }
            else
            {
                ViewBag.Message = "No Requests found.";
                return View();
            }
        }

        [HttpPost]
        public ActionResult CollectionRequests(List<CollectionRequestDTO> collectionRequests, int selectedRow)
        {
            if (ModelState.IsValid)
            {
                var selectedRowData = collectionRequests[selectedRow];

                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<CollectionRequestDTO, CollectionRequest>();
                });
                var mapper = new Mapper(config);
                var collectionRequestEntity = mapper.Map<CollectionRequest>(selectedRowData);

                collectionRequestEntity.Status = "Collected";

                using (var db = new ZeroHungerEntities())
                {
                    var existingEntity = db.CollectionRequests.Find(collectionRequestEntity.Id);

                    if (existingEntity != null)
                    {
                        existingEntity.CollectTime = collectionRequestEntity.CollectTime;
                        existingEntity.Status = collectionRequestEntity.Status;
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("CollectionRequests");
            }

            ViewBag.ErrorMessage = "Invalid form submission.";
            return View("CollectionRequests");
        }




        [HttpGet]
        public ActionResult DistributionRequests()
        {
            return View();
        }



        [HttpGet]
        public ActionResult SubmitCollectedFood()
        {
            int? id = null;

            var cookie = HttpContext.Request.Cookies["UserInfo"];

            if (cookie != null && !string.IsNullOrEmpty(cookie["Id"]))
            {
                int parsedId;
                if (int.TryParse(cookie["Id"], out parsedId))
                {
                    id = parsedId;
                }
            }
            var db = new ZeroHungerEntities();
            var collectionRequests = db.CollectionRequests.Where(c => c.employeeId == id).ToList();
            var collectedItems = db.CollectedItems.ToList();


            if (collectionRequests.Any())
            {
                var config1 = new MapperConfiguration(cfg => {
                    cfg.CreateMap<CollectionRequest, CollectionRequestDTO>();
                });
                var mapper1 = new Mapper(config1);
                var collectionRequestDTOs = mapper1.Map<List<CollectionRequestDTO>>(collectionRequests);

                var config2 = new MapperConfiguration(cfg => {
                    cfg.CreateMap<CollectedItem, CollectedItemDTO>();
                });
                var mapper2 = new Mapper(config2);
                var collectedItemsDTOs = mapper2.Map<List<CollectedItemDTO>>(collectedItems);

                var viewModel = new CompositeDTO2
                {
                    CollectionRequests = collectionRequestDTOs,
                    CollectedItems = collectedItemsDTOs
                };

                return View(viewModel);
            }
            else
            {
                ViewBag.Message = "No restaurants found.";
                return View();
            }
        }

        [HttpPost]
        public ActionResult SubmitCollectedFood(int id)
        {
            return View();
        }
    }
}