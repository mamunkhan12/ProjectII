using ProjectII.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Microsoft.Ajax.Utilities;
using System.Data;

namespace ProjectII.Controllers
{
    public class HomeController : Controller
    {


        private MyDatabaseEntities db = new MyDatabaseEntities();


        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }

            return RedirectToAction("Login");

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
     

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(User Tb)
        {
            if (ModelState.IsValid)
            {
                using (MyDatabaseEntities db = new MyDatabaseEntities())
                {

                    db.Users.Add(Tb);
                    db.SaveChanges();


                    ModelState.Clear();
                    Tb = null;
                    ViewBag.Message = "Registration Successful";
                }


            }

            return View();
        }

        public ActionResult Member(User tb)
        {
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {

                return View(db.Users.ToList());
            }


            return View();
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user,Payment pay)
        {
            try
            {
                Session["UserId"] = null;
                using (MyDatabaseEntities dc = new MyDatabaseEntities())
                {

                    var usr = dc.Users.Single(u => u.Email == user.Email && u.Password == user.Password);


                    if (usr != null)
                    {
                        Session["UserId"] = usr.Id.ToString();
                        Session["Email"] = usr.Email.ToString();
                        Session["UserName"] = usr.UserName.ToString();
                        if (Session["UserName"].Equals("Admin"))
                        {
                            Session["Admin"] = usr.UserName.ToString();

                        }

                        Session["FullName"] = usr.FullName.ToString();
                        Session["Address"] = usr.Address.ToString();
                        Session["Phone"] = usr.Phone.ToString();

                        return RedirectToAction("Index");
                    }

                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Email or Password is wrong";

            }






            return View();
        }

        public ActionResult VicReg()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VicReg(Victim vt)
        {
            if (ModelState.IsValid)
            {
                using (MyDatabaseEntities db = new MyDatabaseEntities())
                {

                    db.Victims.Add(vt);

                    db.SaveChanges();


                    ModelState.Clear();
                    vt = null;
                    ViewBag.Message = "Registration Successful";
                }


            }

            return View();
        }


        public ActionResult Children(Victim vt)
        {


            List<Victim> Lst = new List<Victim>();
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                Lst = db.Victims.SqlQuery("select * from Victim where VictimType='Children'").ToList();

                return View(Lst);
            }


        }

        public ActionResult Poor(Victim vt)
        {
            List<Victim> Lst = new List<Victim>();
            
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                Lst = db.Victims.SqlQuery("select * from Victim where VictimType='Poor'").ToList();

                return View(Lst);
            }

            

        }

        public ActionResult Adult(Victim vt)
        {
            List<Victim> Lst = new List<Victim>();
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                Lst = db.Victims.SqlQuery("select * from Victim where VictimType='Adult'").ToList();
                 return View(Lst);
            }

        }

        public JsonResult IsUserExists(string UserName)
        {
            return Json(!db.Users.Any(user => user.UserName == UserName), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsUserExists2(string UserName)
        {
            return Json(!db.Victims.Any(user => user.UserName == UserName), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsNIDExists(decimal NID)
        {
            return Json(!db.Victims.Any(user => user.NID == NID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsEmailExists(string Email)
        {
            return Json(!db.Users.Any(user => user.Email == Email), JsonRequestBehavior.AllowGet);
        }

        public ActionResult LogOut()
        {
            
            Session["UserId"] = null;
            Session["Email"] = null;
            Session["UserName"] = null;
            Session["FullName"] = null;
            Session["Admin"] = null;
            return RedirectToAction("Login");
        }

        public ActionResult Payment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Payment(Payment pay)
        {

            if (ModelState.IsValid)
            {
                using (MyDatabaseEntities db = new MyDatabaseEntities())
                {
                    db.Payments.Add(pay);

                    db.SaveChanges();
                    ModelState.Clear();
                    pay = null;
                }

            }

            return View();
        }

        public ActionResult History()
        {
            return View();
        }

        [HttpPost]
        public ActionResult History(History history)
        {
            if (ModelState.IsValid)
            {
                using (MyDatabaseEntities db = new MyDatabaseEntities())
                {

                    db.Histories.Add(history);

                    db.SaveChanges();


                    ModelState.Clear();
                    history = null;
                    ViewBag.Message = "Registration Successful";

                }


            }

            return View();
        }

       
       

        public ActionResult Pay(int id,string fullName,string type,string nid,string address)
        {
            Session["Id"] = id;
            Session["FullNameVictim"] = fullName;
            Session["Type"] = type;
            Session["NID"] = nid;
            Session["VictimAddress"] = address;
           
            return RedirectToAction("History");


        }

        public ActionResult PaymentHistory(Payment tb)
        {
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {

                return View(db.Payments.ToList());
            }


            return View();
        }

        public ActionResult PoorPayment(History vt,int id)
        {
            
            List<History> Lst = new List<History>();

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                Lst = db.Histories.SqlQuery("select * from History where VictimId=5").ToList();

                return View(Lst);
            }
        }
        public ActionResult VictimPaymentHistory(History vt, int id)
        {

            List<History> Lst = new List<History>();

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                Lst = db.Histories.SqlQuery("select * from History where VictimId=id").ToList();

                return View(Lst);
            }
        }
        //public ActionResult VictimPaymentHistory2(Payment pay)
        //{
        //    double sum=0;
        //    List<Payment> Lst = new List<Payment>();

        //    using (MyDatabaseEntities db = new MyDatabaseEntities())
        //    {
        //        Lst = db.Payments.SqlQuery("select Amount from Payment ").ToList();
        //        sum = sum + Convert.ToDouble(Lst);
        //        Session["Total"] = sum;
                
        //        return View(Lst);
        //    }
        //}



    }
}