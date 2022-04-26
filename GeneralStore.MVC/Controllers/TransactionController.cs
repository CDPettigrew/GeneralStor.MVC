using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GeneralStore.MVC.Models;

namespace GeneralStore.MVC.Controllers
{
    public class TransactionController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Transaction
        public ActionResult Index()
        {
            return View(_db.Transactions.ToList());
        }
        //GET: Transaction/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(_db.Products, "ProductId", "Name");
            ViewBag.CustomerId = new SelectList(_db.Customers, "CustomerId", "FullName");
            return View();
        }
        //POST: Transaction/Create
        [HttpPost]
        public ActionResult Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                var product = _db.Products.FirstOrDefault(p => p.ProductId == transaction.ProductId);
                if (product == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "product was null");
                }
                var customer = _db.Customers.FirstOrDefault(c => c.CustomerId == transaction.CustomerId);
                if (customer == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "customer was null");
                }
                if (transaction.Quantity > product.Inventory)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "not enough inventory");
                }
                product.Inventory -= transaction.Quantity;
                transaction.DateOfTransaction = DateTime.Now;
                _db.Transactions.Add(transaction);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(_db.Products, "ProductId", "Name");
            ViewBag.CustomerId = new SelectList(_db.Customers, "CustomerId", "FullName");
            return View(transaction);
        }
        //GET: Customer/Delete{id}
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = _db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }
        //GET:Transaction/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Transaction transaction = _db.Transactions.Find(id);
            _db.Transactions.Remove(transaction);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        //GET: Transaction/Edit{id}
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = _db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(_db.Products, "ProductId", "Name");
            ViewBag.CustomerId = new SelectList(_db.Customers, "CustomerId", "FullName");
            return View(transaction);
        }
        //POST: Transaction/Edit{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(transaction).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(_db.Products, "ProductId", "Name");
            ViewBag.CustomerId = new SelectList(_db.Customers, "CustomerId", "FullName");
            return View(transaction);
        }
    }
}