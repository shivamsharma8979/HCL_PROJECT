using HCL_PROJECT.db_context;
using HCL_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HCL_PROJECT.Controllers
{
    public class HomeController : Controller
    {
       

        [HttpGet]
        public ActionResult Index()
        {  
           
            return View();
        }
        [HttpPost]
        public ActionResult Index(logout mod)
        {
            hclEntities1 ent = new hclEntities1();
            var use = ent.logins.Where(m => m.email == mod.email).FirstOrDefault();
            if (use== null)
            {
                TempData["invalid"] = "Email not found invalid email ";
            }
            else
            {
                if (use.email == mod.email && use.password == mod.password)
                {

                    FormsAuthentication.SetAuthCookie(use.email, false);
                    Session["username"] = use.name;
                    return RedirectToAction("indexdeshboard");

                }
                else
                {
                    TempData["wrong"] = "Wrong password";
                  

                }
            }

            return View();
        }
        [HttpGet]
        public ActionResult registration()
        {


            return View();
        }
        [HttpPost]
        public ActionResult registration(logout mod)
        {
            hclEntities1 ent = new hclEntities1();
            login tb = new login();
            tb.id = mod.id;
            tb.name = mod.name;
            tb.email = mod.email;
            tb.password = mod.password;
            ent.logins.Add(tb);
            ent.SaveChanges();


            return View("registration");
        }
        [Authorize]
        public ActionResult About()
        {
            return View(); 
        }
        [HttpGet]
        public ActionResult Emp_Form()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult Emp_Form(EMPLOYEE obj2)
        {
            hclEntities1 obj = new hclEntities1();
            hcl_emp obj1 = new hcl_emp();
            obj1.id = obj2.id;
            obj1.name = obj2.name;
            obj1.address = obj2.address;
            obj1.email = obj2.email;
            obj1.salary = obj2.salary;
            obj1.department = obj2.department;

            if (obj2.id == 0)
            {
                obj.hcl_emp.Add(obj1);
                obj.SaveChanges();
            }
            else
            {
                obj.Entry(obj1).State = System.Data.Entity.EntityState.Modified;
                obj.SaveChanges();
            }

            return RedirectToAction("Table","Home");
        }
        [Authorize]
        public ActionResult delete(int id)
        {
            hclEntities1 ent = new hclEntities1();
            var dlt = ent.hcl_emp.Where(m => m.id == id).First();
            ent.hcl_emp.Remove(dlt);
            ent.SaveChanges();

            return RedirectToAction("Table","Home");
        }
        [Authorize]
        public ActionResult edit( int ID)
        {
            EMPLOYEE obj = new EMPLOYEE();
            hclEntities1 obj1 = new hclEntities1();
            var edt = obj1.hcl_emp.Where(m => m.id == ID).First();
            obj.id = edt.id;
            obj.name = edt.name;
            obj.address = edt.address;
            obj.email = edt.email;
            obj.salary = edt.salary;
            obj.department = edt.department;

            return View("Emp_Form",obj);
        }
        [Authorize]
        public ActionResult indexdeshboard()
        {
           

            return View();
        }
        [Authorize]
        public ActionResult Table()
        {
            hclEntities1 obj = new hclEntities1();
            var ress = obj.hcl_emp.ToList();
            return View(ress);
        }
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}