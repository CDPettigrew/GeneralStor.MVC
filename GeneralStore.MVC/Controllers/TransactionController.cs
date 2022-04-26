using System;
using System.Collections.Generic;
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
            return View();
        }
        //POST: Transaction/Create
        [HttpPost]
        public ActionResult Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                var product = _db.Products.FirstOrDefault(p => p.ProductId == transaction.ProductId);
                if(product == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var customer = _db.Customers.FirstOrDefault(c => c.CustomerId == transaction.ProductId);
                if(customer == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if(transaction.Quantity > product.Inventory)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                product.Inventory -= transaction.Quantity;
                transaction.DateOfTransaction = DateTime.Now;
                _db.Transactions.Add(transaction);
                _db.SaveChanges();
            }
            return View(transaction);
        }
    }
}