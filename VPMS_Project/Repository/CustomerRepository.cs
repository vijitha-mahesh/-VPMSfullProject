using Microsoft.EntityFrameworkCore;
using VPMS_Project.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPMS_Project.Models;

namespace VPMS_Project.Repository
{
    public class CustomerRepository
    {
        private readonly EmpStoreContext _context = null;

        public CustomerRepository(EmpStoreContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerModel>> GetCustomers()
        {
            return await _context.PreSalesCustomers.Select(x => new CustomerModel()
            {
                Id = x.Id,
                name = x.name,
                emailAddress = x.emailAddress
            }).ToListAsync();
        }


        public async Task<int> AddNewCustomer(CustomerModel model)
        {
            var newCustomer = new PreSalesCustomers()
            {
                name = model.name,
                address = model.address,
                contactNumber = model.contactNumber,
                emailAddress = model.emailAddress
            };

            await _context.PreSalesCustomers.AddAsync(newCustomer);
            await _context.SaveChangesAsync();

            return newCustomer.Id;
        }


        public async Task<List<CustomerModel>> GetAllCustomers()
        {
            var customers = new List<CustomerModel>();
            var allcustomers = await _context.PreSalesCustomers.ToListAsync();
            if (allcustomers?.Any() == true)
            {
                foreach (var customer in allcustomers)
                {
                    customers.Add(new CustomerModel()
                    {
                        Id = customer.Id,
                        name = customer.name,
                        address = customer.address,
                        contactNumber = customer.contactNumber,
                        emailAddress = customer.emailAddress
                    });
                }
            }
            return customers;
        }


        public async Task<CustomerModel> GetCustomerById(int id)
        {
            var customer = await _context.PreSalesCustomers.FindAsync(id);

            if (customer != null)
            {
                var customerDetails = new CustomerModel()
                {
                    Id = customer.Id,
                    name = customer.name,
                    address = customer.address,
                    contactNumber = customer.contactNumber,
                    emailAddress = customer.emailAddress
                };
                return customerDetails;
            }

            return null;
        }

        public async Task<bool> EditCustomers(CustomerModel customer)
        {
            var cus = await _context.PreSalesCustomers.FindAsync(customer.Id);

            cus.name = customer.name;
            cus.address = customer.address;
            cus.contactNumber = customer.contactNumber;
            cus.emailAddress = customer.emailAddress;
            cus.projectCount = customer.projectCount;


            _context.Entry(cus).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;

        }

        //delete part
        public async Task<bool> DeleteCustomer(int id)
        {
            var customers = await _context.PreSalesCustomers.FindAsync(id);

            _context.PreSalesCustomers.Remove(customers);
            await _context.SaveChangesAsync();
            return true;

        }

        //delete the customer after going to add new customer
        public async Task<bool> AddToDeletedCustomers(int id)
        {
            var customers = await _context.PreSalesCustomers.FindAsync(id);

            var NewCustomer = new PreSalesDeletedCustomers
            {
                name = customers.name,
                address = customers.address,
                contactNumber = customers.contactNumber,
                emailAddress = customers.emailAddress,
                projectCount = customers.projectCount,

            };
            await _context.PreSalesDeletedCustomers.AddAsync(NewCustomer);
            await _context.SaveChangesAsync();

            return true;
        }

        //Finish delete part
    }
}
