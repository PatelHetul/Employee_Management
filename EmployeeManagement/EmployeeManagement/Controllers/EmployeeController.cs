using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployeeManagement;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private EmployeeMgEntities db = new EmployeeMgEntities();

        // GET: Employee
        public ActionResult Index()
        {
            //using Storeprocedure
            //var employeeMasters = db.EmployeeLists();

            //Using Linq
            var employeeMasters = db.EmployeeMasters.Where(e=>e.IsDelete==0  && e.DepartmentMaster.IsDelete==0);
            return View(employeeMasters);
        }

        public ActionResult RetrieveImage(int id)
        {
            byte[] cover = GetImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public byte[] GetImageFromDataBase(int Id)
        {
            var q = from temp in db.EmployeeMasters where temp.Employee_Id == Id select temp.Image;
            byte[] cover = q.First();
            return cover;
        }

        // GET: Employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeMaster employeeMaster = db.EmployeeMasters.Find(id);
            if (employeeMaster == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = id;
            return View(employeeMaster);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            var DepartmentLists = db.DepartmentMasters.Where(s => s.IsDelete == 0).ToList();
            ViewBag.Department = new SelectList(DepartmentLists, "Department_Id", "Department_Name");
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeMaster employeeMaster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (db.EmployeeMasters.Any(name => name.Email.Equals(employeeMaster.Email)))
                    {
                        ModelState.AddModelError(string.Empty, "Employee is already exists");
                    }
                    else
                    {
                        //ImgUpload code
                        HttpPostedFileBase file = Request.Files["ImageData"];
                        if (file != null)
                        {
                            var supportedTypes = new[] { "jpg", "jpeg", "png", "gif", "bmp" };
                            var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                            if (!supportedTypes.Contains(fileExt))
                            {
                                string ErrorMessage = "File Extension Is InValid - Only Upload Image File";
                                ModelState.AddModelError(string.Empty, ErrorMessage);
                                var DepartmentList = db.DepartmentMasters.Where(s => s.IsDelete == 0).ToList();
                                ViewBag.Department = new SelectList(DepartmentList, "Department_Id", "Department_Name", employeeMaster.Department);
                                return View(employeeMaster);
                            }
                            employeeMaster.Image = ConvertToBytes(file);
                        }
                        employeeMaster.IsDelete = 0;
                        db.EmployeeMasters.Add(employeeMaster);
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
            var DepartmentLists = db.DepartmentMasters.Where(s => s.IsDelete == 0).ToList();
            ViewBag.Department = new SelectList(DepartmentLists, "Department_Id", "Department_Name", employeeMaster.Department);
            return View(employeeMaster);
        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeMaster employeeMaster = db.EmployeeMasters.Find(id);
            if (employeeMaster == null)
            {
                return HttpNotFound();
            }
           // ViewBag.id = id;
            var DepartmentLists = db.DepartmentMasters.Where(s => s.IsDelete == 0).ToList();
            ViewBag.Department = new SelectList(DepartmentLists, "Department_Id", "Department_Name", employeeMaster.Department);
            return View(employeeMaster);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( EmployeeMaster employeeMaster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (db.EmployeeMasters.Any(name => name.Email.Equals(employeeMaster.Email) && name.Employee_Id != employeeMaster.Employee_Id))
                    {
                        ModelState.AddModelError(string.Empty, "Employee is already exists");
                    }
                    else
                    {
                        HttpPostedFileBase file = Request.Files["ImageData"];
                        if (file != null)
                        {
                            var supportedTypes = new[] { "jpg", "jpeg", "png", "gif","bmp" };
                            var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                            if (!supportedTypes.Contains(fileExt))
                            {
                                string ErrorMessage = "File Extension Is InValid - Only Upload Image File";
                                ModelState.AddModelError(string.Empty, ErrorMessage);
                                var DepartmentList = db.DepartmentMasters.Where(s => s.IsDelete == 0).ToList();
                                ViewBag.Department = new SelectList(DepartmentList, "Department_Id", "Department_Name", employeeMaster.Department);
                                return View(employeeMaster);
                            }

                            employeeMaster.Image = ConvertToBytes(file);
                        }
                        employeeMaster.IsDelete = 0;
                        db.Entry(employeeMaster).State = EntityState.Modified;
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
            var DepartmentLists = db.DepartmentMasters.Where(s => s.IsDelete == 0).ToList();
            ViewBag.Department = new SelectList(DepartmentLists, "Department_Id", "Department_Name", employeeMaster.Department);
            return View(employeeMaster);
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeMaster employeeMaster = db.EmployeeMasters.Find(id);
            if (employeeMaster == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = id;
            return View(employeeMaster);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                EmployeeMaster employeeMaster = db.EmployeeMasters.Find(id);
                employeeMaster.IsDelete = 1;
                db.Entry(employeeMaster).State = EntityState.Modified;
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
