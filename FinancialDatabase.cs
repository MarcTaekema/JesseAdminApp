using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite;

namespace JesseAdminApp
{
    // Invoice model representing the financial data


    public class FinancialDatabase
    {
        private SQLiteAsyncConnection _FinanceDB;

        //public FinancialDatabase(string dbPath)
        //{
        //    _FinanceDB = new SQLiteAsyncConnection(dbPath);
        //}

        // Initialize the database
         async Task InitializeAsync()
        {

            try
            {
                if (_FinanceDB != null) return;
                _FinanceDB = new SQLiteAsyncConnection(Constants.FinancialDatabasePath, Constants.Flags);
                var result = await _FinanceDB.CreateTableAsync<Invoice>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization failed: {ex.Message}");
            }

            // Create the Invoice table if it doesn't exist

        }

        // Add a new invoice
        public async Task AddInvoiceAsync(Invoice invoice)
        {
            await InitializeAsync();
            await _FinanceDB.InsertAsync(invoice);
        }
        public async Task<Invoice> GetInvoiceAsync(string invoiceNumber)
        {
            await InitializeAsync();
            var invoice = await _FinanceDB.FindAsync<Invoice>(invoiceNumber);

            // Check if the invoice is found, return null if not
            if (invoice == null)
            {
                // Display an alert if invoice is not found
                await Application.Current.MainPage.DisplayAlert("Error", "Invoice not found.", "OK");
                return null;
            }

            return invoice;
        }

        // Get all invoices for a specific customer
        public async Task<List<Invoice>> GetInvoicesByCustomerAsync(int customerNumber)
        {
            await InitializeAsync();

            var invoices = await _FinanceDB.Table<Invoice>().Where(i => i.CustomerNumber == customerNumber).ToListAsync();

            // Check if the list is empty and handle it
            if (invoices == null || invoices.Count == 0)
            {
                // Display an alert if no invoices are found
                await Application.Current.MainPage.DisplayAlert("Error", $"No invoices found for customer {customerNumber}.", "OK");
                return new List<Invoice>(); // Return an empty list if no invoices found
            }

            return invoices;
        }

        // Get all invoices
        public async Task<List<Invoice>> GetAllInvoicesAsync()
        {
            await InitializeAsync();
            var invoices = await _FinanceDB.Table<Invoice>().ToListAsync();

            // Check if the list is empty
            if (invoices == null || invoices.Count == 0)
            {
                // Display an alert if no invoices are found
                //await Application.Current.MainPage.DisplayAlert("Error", "No invoices found.", "OK");
                return new List<Invoice>(); // Return an empty list if no invoices found
            }

            return invoices;
        }


        // Delete an invoice by its number
        public async Task DeleteInvoiceAsync(string invoiceNumber)
        {
            await InitializeAsync();

            var invoice = await GetInvoiceAsync(invoiceNumber);
            if (invoice != null)
            {
                await _FinanceDB.DeleteAsync(invoice);
            }
        }
    }

}
