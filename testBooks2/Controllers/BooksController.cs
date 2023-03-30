using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using testBooks2.Models;

namespace testBooks2.Controllers
{
    public class BooksController : Controller
    {
        private BooksDBEntities db = new BooksDBEntities();

        // GET: Books
        //to filter data
        public ActionResult Index(string searchString)
        {
            var data = db.Books.ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(d => d.name.ToLower().Contains(searchString.ToLower())).ToList();
            }

            return View(data);
        }

        // GET: Books/Details/5
        //modified to give better view -- VJ
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            string sts = book.status.ToString();
            bool isRes = false;

            if (sts == "RS")
            {
                sts = "Reserved";
                isRes = true;
            }
            else
            {
                sts = "Not Reserved";
            }
            ViewBag.sts = sts;
            ViewBag.isRes = isRes;
            staticClassForBook.selBookID = book.id.ToString();
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/BookRes/{selcted book}
        // to update book status -- VJ
        [HttpPost, ActionName("BookRes")]
        [ValidateAntiForgeryToken]
        public ActionResult BookRes()
        {
            Console.WriteLine("" + staticClassForBook.selBookID);
            Book book = db.Books.Find(""+staticClassForBook.selBookID); ;
            if (ModelState.IsValid)
            {
                book.status = "RS";
                book.booking_num = "" + book.id.ToString() + "C001";
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //return View(book);
            return RedirectToAction("Index");
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,author,price,status,booking_num")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,author,price,status,booking_num")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
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
