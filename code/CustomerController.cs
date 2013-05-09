using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SullysStraps.Data.IRepos;
using SullysStraps.Model;

namespace SullysStraps.Web.Controllers
{
    public class CustomerController : ApiControllerBase
    {
        public CustomerController(ISullysStrapsUow uow)
        {
            Uow = uow;
        }
        // GET api/customer
        public IEnumerable<Customer> Get()
        {
            return Uow.Customers.GetAll().OrderBy(c => c.LName);
        }

        // GET api/customer/5
        public Customer Get(int id)
        {
            var customer = Uow.Customers.GetById(id);
            if (customer != null) return customer;
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
        }

        // POST api/customer
        public HttpResponseMessage Post(Customer customer)
        {
            Uow.Customers.Add(customer);
            Uow.Commit();
            var response = Request.CreateResponse(HttpStatusCode.Created, customer);
            response.Headers.Location = new Uri(Url.Link(RouteConfig.ControllerAndId, new { id = customer.CustomerId }));
            return response;
        }

        // PUT api/customer
        public HttpResponseMessage Put(Customer customer)
        {
            Uow.Customers.Update(customer);
            Uow.Commit();
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        // DELETE api/customer/5
        public HttpResponseMessage Delete(int id)
        {
            Uow.Customers.Delete(id);
            Uow.Commit();
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}
