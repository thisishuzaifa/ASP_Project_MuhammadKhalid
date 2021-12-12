using ASP_Project_MuhammadKhalid.Data;
using ASP_Project_MuhammadKhalid.Models;
using ASP_Project_MuhammadKhalid.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Project_MuhammadKhalid.Repositories
{
    public class AccRepo
    {
        private readonly ApplicationDbContext _context;

        public AccRepo(ApplicationDbContext context)
        {
            _context = context;
        }



        public IQueryable getListInfo(string email)
        {
            var listQuery = from c in _context.Clients
                            from nav in c.ClientAccount
                            from b in _context.BankAccounts
                            from nav2 in b.ClientAccount
                            where (c.email == email && c.clientID == nav.clientID && nav.accountNum == b.accountNum)
                            select new AccountVM()
                            {
                                clientID = c.clientID,
                                lastName = (c.lastName != null) ? c.lastName : "",
                                firstName = (c.firstName != null) ? c.firstName : "",
                                email = (c.email != null) ? c.email : "",
                                accountNum = b.accountNum,
                                accountType = (b.accountType != null) ? b.accountType : "",
                                balance = b.balance,
                            };
            return listQuery;


        }

        public bool Update(AccountVM accountVM)
        {
            var updateQuery = (from c in _context.Clients
                               where accountVM.clientID == c.clientID
                               select c).FirstOrDefault();

            var updateQuery2 = (from b in _context.BankAccounts
                                where accountVM.accountNum == b.accountNum
                                select b).FirstOrDefault();

            updateQuery.lastName = accountVM.lastName;
            updateQuery.firstName = accountVM.firstName;
            updateQuery2.balance = accountVM.balance;
            _context.SaveChanges();

            return true;

        }


        public AccountVM getEdit(int? id, int? accountNum)
        {
            var editQuery = (from c in _context.Clients
                             from b in _context.BankAccounts
                             where (id == c.clientID && accountNum == b.accountNum)
                             select new AccountVM()
                             {
                                 clientID = c.clientID,
                                 lastName = (c.lastName != null) ? c.lastName : "",
                                 firstName = (c.firstName != null) ? c.firstName : "",
                                 email = (c.email != null) ? c.email : "",
                                 accountNum = b.accountNum,
                                 accountType = (b.accountType != null) ? b.accountType : "",
                                 balance = b.balance,
                             }).FirstOrDefault();
            return editQuery;
        }

        public AccountVM getDetailInfo(int? id, int accountNum)
        {
            var detailQuery = (from c in _context.Clients
                               from b in _context.BankAccounts
                               where (id == c.clientID && accountNum == b.accountNum)
                               select new AccountVM()
                               {
                                   clientID = c.clientID,
                                   lastName = (c.lastName != null) ? c.lastName : "",
                                   firstName = (c.firstName != null) ? c.firstName : "",
                                   email = (c.email != null) ? c.email : "",
                                   accountNum = b.accountNum,
                                   accountType = (b.accountType != null) ? b.accountType : "",
                                   balance = b.balance,
                               }).FirstOrDefault();
            return detailQuery;
        }

        public bool Create(AccountVM accountVM, string email)
        {
            var query = (from c in _context.Clients
                         where (c.email == email)
                         select c).FirstOrDefault();
            BankAccount bankAccount = new BankAccount()
            {
                accountType = accountVM.accountType,
                balance = accountVM.balance
            };
            _context.Add(bankAccount);
            _context.SaveChanges();
            ClientAccount clientAccount = new ClientAccount()
            {
                accountNum = bankAccount.accountNum,
                clientID = query.clientID
            };
            _context.Add(clientAccount);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int? clientID, int? accountNum)
        {
            string deleteMessage = "Product Id: " + clientID
                     + " deleted successfully";
            try
            {
                var clientAccount = (from ca in _context.ClientAccounts
                                     where ca.clientID == clientID && ca.accountNum == accountNum
                                     select ca).FirstOrDefault();

                var bAccount = (from b in _context.BankAccounts
                                where b.accountNum == accountNum
                                select b).FirstOrDefault();

                _context.Remove(clientAccount);
                _context.Remove(bAccount);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                deleteMessage = e.Message + " "
                + "The product may not exist or "
                + "there could be a foreign key restriction.";
            }
            return true;
        }
    }
}
