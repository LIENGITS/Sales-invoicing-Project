using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VidlyWithLIEN.Models;
using System.Data.Entity;
using VidlyWithLIEN.ViewModels;

namespace VidlyWithLIEN.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private ApplicationDbContext _context;

        public CustomerController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Customer
        public ViewResult Index()
        {
            var customers = _context.Customers.Include(c => c.MemberShipType).ToList();
            return View(customers);
        }

        public ActionResult New()
        {
            var memberShipTypes = _context.MemberShipTypes.ToList();
            CustomerFormViewModel viewModel = new CustomerFormViewModel
            {
                MemberShipTypes = memberShipTypes,
                Customer = new Customer()
            };

            return View("CustomerForm", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                CustomerFormViewModel viewModel = new CustomerFormViewModel
                {
                    MemberShipTypes = _context.MemberShipTypes.ToList(),
                    Customer = customer
                };
                return View("CustomerForm", viewModel);
            }
            else
            {
                if (customer.Id == 0)
                {
                    _context.Customers.Add(customer);
                }
                else
                {
                    var customerInDB = _context.Customers.SingleOrDefault(c => c.Id == customer.Id);
                    customerInDB.Name = customer.Name;
                    customerInDB.Birthdate = customer.Birthdate;
                    customerInDB.MemberShipTypeId = customer.MemberShipTypeId;
                    customerInDB.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
                }
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

        }

        public ActionResult Edit(int id)
        {
            var customers = _context.Customers.SingleOrDefault(c => c.Id == id);

            CustomerFormViewModel viewModel = new CustomerFormViewModel
            {
                Customer = customers,
                MemberShipTypes = _context.MemberShipTypes.ToList()
            };
            return View("CustomerForm", viewModel);
        }

        public ActionResult Delete(int id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}