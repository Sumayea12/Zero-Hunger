using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZeroHunger.DTOs;
using ZeroHunger.EF;

namespace ZeroHunger.Controllers
{
    public class RestaurantController : Controller
    {
        // GET: Restaurant
        public ActionResult Index(int resturantownerId)
        {
            var db = new ZeroHungerEntities();

            var restaurants = db.RegisteredRestaurants
                .Where(r => r.userId == resturantownerId)
                .ToList();

            if (restaurants.Any())
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<RegisteredRestaurant, RegisteredRestaurantDTO>();
                });
                var mapper = new Mapper(config);
                var data2 = mapper.Map<List<RegisteredRestaurantDTO>>(restaurants);

                return View("MyRestaurants", data2);
            }
            else
            {
                return RedirectToAction("Register_Restaurant");
            }
        }

        public ActionResult MyRestaurants(List<RegisteredRestaurantDTO> data2)
        {
            return View(data2);
        }

        [HttpGet]
        public ActionResult Donate(int restaurantId)
        {

            ViewBag.RestaurantId = restaurantId;
            return View();
    
        }

        [HttpPost]
        public ActionResult Donate(CollectionRequestDTO request)
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
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<CollectionRequestDTO, CollectionRequest>();
            });

            var mapper = new Mapper(config);
            var data = mapper.Map<CollectionRequest>(request);
            data.Status = "Not Collected";
            data.userid = restaurantOwnerId.GetValueOrDefault();
            if (restaurantOwnerId.HasValue)
            {
                using (var db = new ZeroHungerEntities())
                {
                    try
                    {
                        db.CollectionRequests.Add(data);
                        db.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Error", "Home");
                    }
                }
                return RedirectToAction("Index", "Restaurant", new { resturantownerId = restaurantOwnerId.Value });
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpGet]
        public ActionResult Register_Restaurant()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register_Restaurant(RegisteredRestaurantDTO restaurant)
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
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<RegisteredRestaurantDTO, RegisteredRestaurant>();
            });

            var mapper = new Mapper(config);
            var data = mapper.Map<RegisteredRestaurant>(restaurant);
            data.userId = restaurantOwnerId.GetValueOrDefault();
            using (var db = new ZeroHungerEntities())
            {
                try
                {
                    db.RegisteredRestaurants.Add(data);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            
            return RedirectToAction("Index", new { resturantownerId = restaurantOwnerId.Value });

        }

        public ActionResult My_Donation()
        {
            var cookie = HttpContext.Request.Cookies["UserInfo"];
            int? restaurantOwnerId = null;

            if (cookie != null && !string.IsNullOrEmpty(cookie["Id"]))
            {
                int parsedId;
                if (int.TryParse(cookie["Id"], out parsedId))
                {
                    restaurantOwnerId = parsedId;
                }
            }

            var db = new ZeroHungerEntities();

            var donations = db.CollectionRequests
                .Where(c => c.userid == restaurantOwnerId)
                .ToList();

            if (donations.Any())
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<CollectionRequest, CollectionRequestDTO>();
                });
                var mapper = new Mapper(config);
                var donationData = mapper.Map<List<CollectionRequestDTO>>(donations);

                return View(donationData);
            }
            else
            {
                ViewBag.Message = "You have not made any donations yet.";
                return View();
            }
        }


    }
}