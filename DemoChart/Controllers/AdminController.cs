using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoChart.Models;

namespace DemoChart.Controllers
{
    public class AdminController : Controller
    {
        private CAFEEntities2 db = new CAFEEntities2();

        // GET: Admin
        public ActionResult Index()
        {
            return View(db.RegisterUsers.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisterUser registerUser = db.RegisterUsers.Find(id);
            if (registerUser == null)
            {
                return HttpNotFound();
            }
            return View(registerUser);
        }

        // GET: Admin/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Admin/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Email,IsActive")] RegisterUser registerUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.RegisterUsers.Add(registerUser);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(registerUser);
        //}

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisterUser registerUser = db.RegisterUsers.Find(id);
            if (registerUser == null)
            {
                return HttpNotFound();
            }
            return View(registerUser);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email,Password,IsActive")] RegisterUser registerUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registerUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(registerUser);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisterUser registerUser = db.RegisterUsers.Find(id);
            if (registerUser == null)
            {
                return HttpNotFound();
            }
            return View(registerUser);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RegisterUser registerUser = db.RegisterUsers.Find(id);
            db.RegisterUsers.Remove(registerUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
