using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.DTOs;
using ZeroHunger.EF;
using AutoMapper;
using System.Web.UI.WebControls;


namespace ZeroHunger.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserDTO login)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<UserDTO, User>();
            });
            var mapper = new Mapper(config);
            var data = mapper.Map<User>(login);
            var db = new ZeroHungerEntities();
            var res = db.Users.FirstOrDefault(c => c.email == login.email && c.Password == login.Password);
            if (res != null)
            {
                var cookie = new HttpCookie("UserInfo");
                cookie["Name"] = res.Name;
                cookie["Password"] = res.Password;
                cookie["Id"] = res.Id.ToString();
                cookie.Expires = DateTime.Now.AddMinutes(5);
                Response.Cookies.Add(cookie);

                if (res.Type == "NGO")
                {
                    return RedirectToAction("NGODashboard", "Home");
                }
                else if(res.Type=="Employee")
                {
                    var employee = db.Users.FirstOrDefault(c => c.email == login.email);
                    ViewBag.employeeId = employee.Id;
                    return RedirectToAction("Index", "Employee", new { employeeId = employee.Id });
                }
                else
                {
                    var resturantowner = db.Users.FirstOrDefault(c => c.email == login.email);
                    ViewBag.resturantownerId = resturantowner.Id;
                    return RedirectToAction("Index", "Restaurant", new { resturantownerId = resturantowner.Id });
                }
            }
            else
            {
                TempData["Message"] = "Invalid username or password.";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserDTO user)
        {
            // Create the MapperConfiguration once (preferably at application startup)
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<UserDTO, User>();
            });

            var mapper = new Mapper(config);
            var data = mapper.Map<User>(user);

            using (var db = new ZeroHungerEntities())
            {
                try
                {
                    db.Users.Add(data);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error", "Home");
                }
            }


            return RedirectToAction("Login");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult NGODashboard()
        {

            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        //
        [HttpGet]
        public ActionResult CollectionRequests()
        {
            var db = new ZeroHungerEntities();

            var collectionRequests = db.CollectionRequests.Where(c => c.Status == "Not Collected").ToList();
            var users = db.Users.Where(u => u.Type == "Employee").ToList();


            if (collectionRequests.Any())
            {
                var config1 = new MapperConfiguration(cfg => {
                    cfg.CreateMap<CollectionRequest, CollectionRequestDTO>();
                });
                var mapper1 = new Mapper(config1);
                var collectionRequestDTOs = mapper1.Map<List<CollectionRequestDTO>>(collectionRequests);

                var config2 = new MapperConfiguration(cfg => {
                    cfg.CreateMap<User, UserDTO>();
                });
                var mapper2 = new Mapper(config2);
                var userDTOs = mapper2.Map<List<UserDTO>>(users);

                var viewModel = new CompositeDTO1
                {
                    CollectionRequests = collectionRequestDTOs,
                    Users = userDTOs
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
        public ActionResult CollectionRequests(List<CollectionRequestDTO> collectionRequests)
        {
            if (collectionRequests != null && collectionRequests.Any())
            {
                var config1 = new MapperConfiguration(cfg => {
                    cfg.CreateMap<CollectionRequestDTO, CollectionRequest>();
                });
                var mapper1 = new Mapper(config1);
                var collectionReq = mapper1.Map<List<CollectionRequest>>(collectionRequests);

                using (var db = new ZeroHungerEntities())
                {
                    foreach (var request in collectionReq)
                    {
                        if (request != null && request.employeeId != null)
                        {
                            int id = request.Id;
                            int employeeId = (int)request.employeeId;

                            var collectionRequest = db.CollectionRequests.Find(id);

                            if (collectionRequest != null)
                            {
                                collectionRequest.employeeId = employeeId;
                                db.SaveChanges();
                            }
                        }
                    }
                }

                return RedirectToAction("CollectionRequests");
            }
            else
            {
                ViewBag.Message = "No data submitted.";
                return View();
            }
        }












        public ActionResult Registered_Restaurants()
        {
            var db = new ZeroHungerEntities();

            var registeredestaurants = db.RegisteredRestaurants.ToList();
            var config1 = new MapperConfiguration(cfg => {
                cfg.CreateMap<RegisteredRestaurant, RegisteredRestaurantDTO>();
            });
            var mapper1 = new Mapper(config1);
            var registeredtestaurentstDTOs = mapper1.Map<List<RegisteredRestaurantDTO>>(registeredestaurants);
            return View(registeredtestaurentstDTOs);
        }







        public ActionResult CollectedItems()
        {
            var db = new ZeroHungerEntities();

            var collectedItems = db.CollectedItems.ToList();
            var config1 = new MapperConfiguration(cfg => {
                cfg.CreateMap<CollectedItem, CollectedItemDTO>();
            });
            var mapper1 = new Mapper(config1);
            var collectedItemsDTOs = mapper1.Map<List<CollectedItemDTO>>(collectedItems);

            // Check if there are no collected items
            if (collectedItemsDTOs.Count == 0)
            {
                ViewBag.ErrorMessage = "No collected items found.";
            }

            return View(collectedItemsDTOs);
        }





        public ActionResult Distributed_Foods()
        {
            return View();
        }




    public ActionResult Logout()
        {
            // Clear the "CustomerInfo" cookie
            if (Request.Cookies["UserInfo"] != null)
            {
                var customerCookie = new HttpCookie("UserInfo");
                customerCookie.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(customerCookie);
            }

            return RedirectToAction("Login");
        }
        

    }
}