using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;

namespace JesseAdminApp
{
    public class CustomerDatabase
    {
        private SQLiteAsyncConnection customerDatabase;

        // Initialize the database connection
        async Task Init()
        {
            if (customerDatabase is not null)
                return;

            customerDatabase = new SQLiteAsyncConnection(Constants.CustomerDatabasePath, Constants.Flags);
            var result = await customerDatabase.CreateTableAsync<Customer>();
        }

        // Get all customers from the database
        public async Task<List<Customer>> GetCustomers()
        {
            await Init();
            return await customerDatabase.Table<Customer>().ToListAsync();
        }

        // Get the latest Klantnummer from the database
        public async Task<int> GetLatestKlantnummerAsync()
        {
            await Init();
            var latestCustomer = await customerDatabase.Table<Customer>()
                                                       .OrderByDescending(c => c.Klantnummer)
                                                       .FirstOrDefaultAsync();
            // Return the latest Klantnummer or 0 if no customers exist
            return latestCustomer?.Klantnummer ?? 0;
        }

        // Save or update a customer in the database
        public async Task<int> SaveCustomerAsync(Customer customer)
        {
            await Init();
            if (customer.Id != 0)
                return await customerDatabase.UpdateAsync(customer);
            else
                return await customerDatabase.InsertAsync(customer);
        }

        // Delete a customer from the database
        public async Task<int> DeleteCustomerAsync(Customer customer)
        {
            await Init();
            return await customerDatabase.DeleteAsync(customer);
        }
    }
}
