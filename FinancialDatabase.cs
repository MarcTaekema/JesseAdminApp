using SQLite;

namespace JesseAdminApp
{
    // Database voor financiële gegevens (facturen)
    public class FinancialDatabase
    {
        private SQLiteAsyncConnection _FinanceDB;

        // Initialiseer de database
        async Task InitializeAsync()
        {
            try
            {
                // Controleer of de database al is geïnitialiseerd
                if (_FinanceDB != null) return;
                // Maak een nieuwe verbinding met de database
                _FinanceDB = new SQLiteAsyncConnection(Constants.FinancialDatabasePath, Constants.Flags);
                var result = await _FinanceDB.CreateTableAsync<Invoice>();
            }
            catch (Exception ex)
            {
                // Toon foutmelding als initialisatie mislukt
                Console.WriteLine($"Database initialisatie mislukt: {ex.Message}");
            }
        }

        // Voeg een nieuwe factuur toe
        public async Task AddInvoiceAsync(Invoice invoice)
        {
            // Initialiseer de database en voeg de factuur toe
            await InitializeAsync();
            await _FinanceDB.InsertAsync(invoice);
        }

        // Haal een specifieke factuur op op basis van factuurnummer
        public async Task<Invoice> GetInvoiceAsync(string invoiceNumber)
        {
            // Initialiseer de database en zoek de factuur
            await InitializeAsync();
            var invoice = await _FinanceDB.FindAsync<Invoice>(invoiceNumber);

            // Controleer of de factuur bestaat
            if (invoice == null)
            {
                // Toon foutmelding als factuur niet gevonden is
                await Application.Current.MainPage.DisplayAlert("Error", "Factuur niet gevonden.", "OK");
                return null;
            }

            return invoice;
        }

        // Haal alle facturen op voor een specifieke klant
        public async Task<List<Invoice>> GetInvoicesByCustomerAsync(int customerNumber)
        {
            // Initialiseer de database en haal facturen op voor de klant
            await InitializeAsync();
            var invoices = await _FinanceDB.Table<Invoice>().Where(i => i.CustomerNumber == customerNumber).ToListAsync();

            // Controleer of de lijst leeg is
            if (invoices == null || invoices.Count == 0)
            {
                return new List<Invoice>(); // Return een lege lijst als geen facturen gevonden
            }

            return invoices;
        }

        // Haal alle facturen op
        public async Task<List<Invoice>> GetAllInvoicesAsync()
        {
            // Initialiseer de database en haal alle facturen op
            await InitializeAsync();
            var invoices = await _FinanceDB.Table<Invoice>().ToListAsync();

            // Controleer of de lijst leeg is
            if (invoices == null || invoices.Count == 0)
            {
                // Retourneer een lege lijst als er geen facturen zijn
                return new List<Invoice>();
            }

            return invoices;
        }

        // Verwijder een factuur op basis van factuurnummer
        public async Task DeleteInvoiceAsync(string invoiceNumber)
        {
            // Initialiseer de database en zoek de factuur
            await InitializeAsync();

            var invoice = await GetInvoiceAsync(invoiceNumber);
            if (invoice != null)
            {
                // Verwijder de factuur uit de database
                await _FinanceDB.DeleteAsync(invoice);
            }
        }
    }
}
