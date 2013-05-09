using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SullysStraps.Data.IRepos;
using SullysStraps.Model;

namespace SullysStraps.Data.Repos
{
    public class CustomerRepo : GenRepo<Customer>, ICustomerRepo
    {
        public CustomerRepo(DbContext context) : base(context) { }


        public IQueryable<CustomerBrief> GetCustomerBriefs()
        {
            return _dbSet.Select(c => new CustomerBrief
                {
                    CustomerId = c.CustomerId,
                    FName = c.FName,
                    LName = c.LName,
                    Email = c.Email,
                    City = c.City,
                    State = c.State
                });
        }
    }
}
