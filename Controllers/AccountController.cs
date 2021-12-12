using ASP_Project_MuhammadKhalid.Data;
using ASP_Project_MuhammadKhalid.Repositories;
using ASP_Project_MuhammadKhalid.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Project_MuhammadKhalid.Controllers
{
    public class AccountController : Controller
    {

        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            AccRepo accRepo = new AccRepo(_context);
            var query = accRepo.getListInfo(User.Identity.Name);

            return View(query);
            
        }

        public IActionResult Details(int clientID, int accountNum)
        {
            AccRepo accRepo = new AccRepo(_context);
            var detailQuery = accRepo.getDetailInfo(clientID, accountNum);
            return View(detailQuery);
        }

        public IActionResult Edit(int? clientID, int accountNum)
        {
            AccRepo accRepo = new AccRepo(_context);
            var editQuery = accRepo.getEdit(clientID, accountNum);

            return View(editQuery);
        }

        [HttpPost]

        public IActionResult Edit(
            [Bind("clientID,lastName,firstName, accountNum, balance")] AccountVM accountVM)
        {
            if (accountVM.firstName != null && accountVM.lastName != null &&
                accountVM.balance != null)
            {
                AccRepo accRepo = new AccRepo(_context);
                accRepo.Update(accountVM);
            }
            return RedirectToAction("Index", "Accounts");
        }

        public IActionResult Delete(int? clientID, int? accountNum)
        {
            AccRepo accRepo = new AccRepo(_context);
            var deleteQuery = accRepo.Delete(clientID, accountNum);
            return RedirectToAction("Index", "Accounts");
        }

        public IActionResult Create()
        {
            ViewData["accountType"] = new SelectList(_context.BankAccounts, "accountType", "accountType");
            return View();
        }

        [HttpPost]
        public IActionResult Create(
            [Bind("balance, accountType")] AccountVM accountDetailsVM)
        {
            if (accountDetailsVM.balance != null && accountDetailsVM.accountType != null)
            {
                AccRepo accRepo = new AccRepo(_context);
                accRepo.Create(accountDetailsVM, User.Identity.Name);
            }
            return RedirectToAction("Index", "Accounts");
        }



    }
}
