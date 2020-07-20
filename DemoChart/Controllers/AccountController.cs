using DemoChart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DemoChart.Controllers
{
    public class AccountController : Controller
    {
        CAFEEntities2 context = new CAFEEntities2();
        // Return Home page.
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult GaugeChart()
        {
            return View();
        }

        public ActionResult UserScreen()
        {
            return View();
        }

        public ActionResult GetData()
        {

            var userData = context.RegisterUsers.Where(x => x.Email != "" && x.IsActive == true).ToList();
            int uCount = userData.Count();
            Object obj = uCount;

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        //The form's data in Register view is posted to this method. 
        //We have binded the Register View with Register ViewModel, so we can accept object of Register class as parameter.
        //This object contains all the values entered in the form by the user.
        [HttpPost]
        public ActionResult SaveRegisterDetails(Register registerDetails)
        {
            //We check if the model state is valid or not. We have used DataAnnotation attributes.
            //If any form value fails the DataAnnotation validation the model state becomes invalid.
            if (ModelState.IsValid)
            {
                //create database context using Entity framework 
                using (var databaseContext = new CAFEEntities2())
                {
                    //If the model state is valid i.e. the form values passed the validation then we are storing the User's details in DB.
                    RegisterUser reglog = new RegisterUser();

                    //Save all details in RegitserUser object

                    reglog.FirstName = registerDetails.FirstName;
                    reglog.LastName = registerDetails.LastName;
                    reglog.Email = registerDetails.Email;
                    reglog.Password = registerDetails.Password;

                    //Calling the SaveDetails method which saves the details.
                    databaseContext.RegisterUsers.Add(reglog);
                    databaseContext.SaveChanges();
                }

                ViewBag.Message = "User Details Saved";                
                return View("GaugeChart");
            }
            else
            {

                //If the validation fails, we are returning the model object with errors to the view, which will display the error messages.
                return View("Register", registerDetails);
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        //The login form is posted to this method.
        [HttpPost]
        public ActionResult Login(Login model)
        {
            //Checking the state of model passed as parameter.
            if (ModelState.IsValid)
            {
                //Validating the user, whether the user is valid or not.
                var isValidUser = IsValidUser(model);
                var userData = context.RegisterUsers.Where(x => x.Email != "" && x.IsActive == true).ToList();
                int uCount = userData.Count();
                RegisterUser user = context.RegisterUsers.Where(query => query.Email.Equals(model.Email) && query.Password.Equals(model.Password)).SingleOrDefault();

                //If user is valid & present in database, we are redirecting it to Welcome page.

                if (isValidUser != null && user.Email == "Admin@admin.com")
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    if (user.Email == "Admin@admin.com")
                    {
                        user.IsActive = false;
                        context.SaveChanges();
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        ModelState.AddModelError("Failure", "Wrong Username and password combination !");
                        return View();
                    }
                }

                else if (isValidUser != null && user.Email != "Admin@admin.com")
                {
                    if (uCount <= 4)
                    {
                        if (isValidUser != null)
                        {
                            Session["UserID"] = user.Id;
                            user.IsActive = true;
                            user.LoginTime = DateTime.Now;
                            user.LogoutTime = null;
                            context.SaveChanges();
                            FormsAuthentication.SetAuthCookie(model.Email, false);
                            return RedirectToAction("UserScreen");

                        }
                        else
                        {
                            //If the username and password combination is not present in DB then error message is shown.
                            ModelState.AddModelError("Failure", "Wrong Username and password combination !  Please contact Admin !!");
                            return View();
                        }
                    }
                    else
                    {
                        user.IsActive = false;
                        context.SaveChanges();
                        ModelState.AddModelError("Failure", "Cafeteria Occupancy Full !! Please Login after few minutes.");
                                                return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("Failure", "Wrong Username and password combination !");
                    return View();
                }
            }

            else
            {
                //If model state is not valid, the model with error message is returned to the View.
                return View(model);
            }
        }

            //function to check if User is valid or not
        public RegisterUser IsValidUser(Login model)
        {
            using (var dataContext = new CAFEEntities2())
            {
                //Retireving the user details from DB based on username and password enetered by user.
                RegisterUser user = dataContext.RegisterUsers.Where(query => query.Email.Equals(model.Email) && query.Password.Equals(model.Password)).SingleOrDefault();
                //If user not present
                if (user == null)
                    return null;
                //If user is present
                else
                {
                    user.IsActive = true;
                    dataContext.SaveChanges();
                    return user;
                }
            }
        }
        
        public ActionResult Logout()
        {
            int data = Convert.ToInt32(Session["UserID"]);
            using (var datacontext = new CAFEEntities2())
            {
                RegisterUser userDetail = datacontext.RegisterUsers.Where(x => x.Id == data).SingleOrDefault();
                if(userDetail!= null)
                {
                    userDetail.IsActive = false;
                    userDetail.LogoutTime = DateTime.Now;
                    datacontext.SaveChanges();
                }
            }
                FormsAuthentication.SignOut();
                Session.Abandon(); // it will clear the session at the end of request            
                return RedirectToAction("Index");
        }

    }
}