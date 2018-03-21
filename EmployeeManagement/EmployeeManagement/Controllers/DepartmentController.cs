using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployeeManagement;

namespace EmployeeManagement.Controllers
{
    public class DepartmentController : Controller
    {
        private EmployeeMgEntities db = new EmployeeMgEntities();

        // GET: Department
        public ActionResult Index()
        {
            var DepartmentLists = db.DepartmentMasters.Where(s => s.IsDelete == 0).ToList();
            return View(DepartmentLists);
        }

        // GET: Department/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepartmentMaster departmentMaster = db.DepartmentMasters.Find(id);
            if (departmentMaster == null)
            {
                return HttpNotFound();
            }
            return View(departmentMaster);

        }

        // GET: Department/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Department_Id,Department_Name")] DepartmentMaster departmentMaster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (db.DepartmentMasters.Any(name => name.Department_Name.Equals(departmentMaster.Department_Name)))
                    {
                        ModelState.AddModelError(string.Empty, "Department is already exists");
                    }
                    else
                    {
                        departmentMaster.IsDelete = 0;
                        db.DepartmentMasters.Add(departmentMaster);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                }
            }
            catch (Exception ex)
            {
                string message = string.Format("<b>Message:</b> {0}<br /><br />", ex.Message);
                message += string.Format("<b>StackTrace:</b> {0}<br /><br />", ex.StackTrace.Replace(Environment.NewLine, string.Empty));
                message += string.Format("<b>Source:</b> {0}<br /><br />", ex.Source.Replace(Environment.NewLine, string.Empty));
                message += string.Format("<b>TargetSite:</b> {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, string.Empty));
                ModelState.AddModelError(string.Empty, message);
            }
            return View(departmentMaster);
        }

        // GET: Department/Edit/id
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepartmentMaster departmentMaster = db.DepartmentMasters.Find(id);
            if (departmentMaster == null)
            {
                return HttpNotFound();
            }
            return View(departmentMaster);
        }

        // POST: Department/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Department_Id,Department_Name")] DepartmentMaster departmentMaster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (db.DepartmentMasters.Any(name => name.Department_Name.Equals(departmentMaster.Department_Name) && name.Department_Id != departmentMaster.Department_Id))
                    {
                        ModelState.AddModelError(string.Empty, "Department is already exists");
                    }
                    else
                    {
                        departmentMaster.IsDelete = 0;
                        db.Entry(departmentMaster).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("<b>Message:</b> {0}<br /><br />", ex.Message);
                message += string.Format("<b>StackTrace:</b> {0}<br /><br />", ex.StackTrace.Replace(Environment.NewLine, string.Empty));
                message += string.Format("<b>Source:</b> {0}<br /><br />", ex.Source.Replace(Environment.NewLine, string.Empty));
                message += string.Format("<b>TargetSite:</b> {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, string.Empty));
                ModelState.AddModelError(string.Empty, message);
            }
            return View(departmentMaster);
        }

        // GET: Department/Delete/id
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepartmentMaster departmentMaster = db.DepartmentMasters.Find(id);
            if (departmentMaster == null)
            {
                return HttpNotFound();
            }
            return View(departmentMaster);
        }

        // POST: Department/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                DepartmentMaster departmentMaster = db.DepartmentMasters.Find(id);
                departmentMaster.IsDelete = 1;
                db.Entry(departmentMaster).State = EntityState.Modified;
                //db.DepartmentMasters.Remove(departmentMaster);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string message = string.Format("<b>Message:</b> {0}<br /><br />", ex.Message);
                message += string.Format("<b>StackTrace:</b> {0}<br /><br />", ex.StackTrace.Replace(Environment.NewLine, string.Empty));
                message += string.Format("<b>Source:</b> {0}<br /><br />", ex.Source.Replace(Environment.NewLine, string.Empty));
                message += string.Format("<b>TargetSite:</b> {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, string.Empty));
                ModelState.AddModelError(string.Empty, message);
            }
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
