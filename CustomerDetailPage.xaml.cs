using Microsoft.Maui.Controls;
using Microsoft.VisualBasic;
using System.Linq;

namespace JesseAdminApp
{
    public partial class CustomerDetailPage : ContentPage
    {
        private readonly CustomerDatabase _database;
        private readonly FinancialDatabase _FinanceDB;
        public CustomerDetailPage(string klantnummer)
        {
            InitializeComponent();
            _database = new CustomerDatabase();
            _FinanceDB = new FinancialDatabase();
            LoadCustomerDetails(klantnummer);
        }

        private async void LoadCustomerDetails(string klantnummer)
        {
            // Get the customer based on the klantnummer
            var customer = (await _database.GetCustomers())
                            .FirstOrDefault(c => c.Klantnummer == int.Parse(klantnummer));

            if (customer != null)
            {
                NameLabel.Text = $"Naam: {customer.Naam}";
                SurnameLabel.Text = $"Achternaam: {customer.Achternaam}";      
                AdresLabel.Text = $"Straat + Huisnummer: {customer.Adres}";
                PostcodeLabel.Text = $"Postcode + Plaats: {customer.Postcode}";
                KlantnummerLabel.Text = $"Klantnummer: {customer.Klantnummer}";
                StartDateLabel.Text = $"Start Datum: {customer.DatumVanStart.ToShortDateString()}";
            }
            else
            {
                await DisplayAlert("Error", "Customer not found.", "OK");
            }
        }
        private async void OnFinancieleInformatieClicked(object sender, EventArgs e)
        {
            var klantnummerString = KlantnummerLabel.Text?.Split(":").Last().Trim();

            // Attempt to parse the string to an integer
            if (int.TryParse(klantnummerString, out int klantnummer))
            {
                // Fetch invoices by customer number
                var invoices = await _FinanceDB.GetInvoicesByCustomerAsync(klantnummer);

                // Check if there are invoices for the customer
                if (invoices != null && invoices.Any())
                {
                    // Sum the amounts (including and excluding BTW)
                    decimal totalAmountInclBTW = invoices.Sum(invoice => invoice.AmountInclBTW);
                    decimal totalAmountExclBTW = invoices.Sum(invoice => invoice.AmountExclBTW);

                    // Display the totals
                    string reportMessage = $"Totaal betaald incl. BTW: {totalAmountInclBTW:C}\nTotaal betaald excl. BTW: {totalAmountExclBTW:C}";
                    await DisplayAlert("Financiële Informatie", reportMessage, "OK");
                }
                else
                {
                    await DisplayAlert("Fout", "Geen facturen gevonden.", "OK");
                }
            }
            else
            {
                // Handle the case where the parsing fails
                await DisplayAlert("Fout", "Ongeldig klantnummer.", "OK");
            }
        }

        // Delete customer event handler
        private async void OnDeleteCustomerClicked(object sender, EventArgs e)
        {
            var klantnummer = KlantnummerLabel.Text?.Split(":").Last().Trim(); // Assuming the format is "Klantnummer: X"

            if (!string.IsNullOrEmpty(klantnummer))
            {
                // Confirm before deletion
                bool confirmDelete = await DisplayAlert("Confirm", "Are you sure you want to delete this customer?", "Yes", "No");

                if (confirmDelete)
                {
                    var customer = (await _database.GetCustomers())
                                   .FirstOrDefault(c => c.Klantnummer == int.Parse(klantnummer));

                    if (customer != null)
                    {
                        // Call DeleteCustomerAsync to delete the customer
                        var result = await _database.DeleteCustomerAsync(customer);
                        if (result > 0)  // Assuming DeleteAsync returns the number of affected rows
                        {
                            await DisplayAlert("Success", "Customer deleted successfully.", "OK");

                            // Optionally, navigate back or clear details
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await DisplayAlert("Error", "Failed to delete customer.", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "Customer not found.", "OK");
                    }
                }
            }
        }
        private async void OnNavigateToFactuurPageClicked(object sender, EventArgs e)
        {
            // Extract values from labels, ensuring they're not null
            string Klantnaam = $"{NameLabel.Text?.Split(':').Last().Trim()} {SurnameLabel.Text?.Split(':').Last().Trim()}";
            string Addres = AdresLabel.Text?.Split(':').Last().Trim();
            string Postcode = PostcodeLabel.Text?.Split(':').Last().Trim();
            string klantnummer = KlantnummerLabel.Text?.Split(':').Last().Trim();

            // If any value is null or empty, show an error message
            if (string.IsNullOrWhiteSpace(Klantnaam) || string.IsNullOrWhiteSpace(Addres) || string.IsNullOrWhiteSpace(Postcode) || string.IsNullOrWhiteSpace(klantnummer))
            {
                await DisplayAlert("Error", "Please ensure all customer details are filled in correctly.", "OK");
                return;
            }

            // Step 1: Get the current year
            int currentYear = DateTime.Now.Year;
            string yearPart = currentYear.ToString("D4");

            // Step 2: Query the database to find the next available invoice number
            var invoices = await _FinanceDB.GetAllInvoicesAsync();
            int nextInvoiceNumber = 1; // Start with 00001 if no invoices exist

            if (invoices.Any())
            {
                // Extract the DDDDD part from existing invoice numbers and find the next available
                var existingNumbers = invoices
                    .Select(i => int.Parse(i.InvoiceNumber.Substring(4, 5))) // Extract DDDDD part
                    .OrderBy(n => n);

                // Find the next unused number
                foreach (int number in existingNumbers)
                {
                    if (number == nextInvoiceNumber)
                        nextInvoiceNumber++;
                    else
                        break;
                }
            }

            string invoiceNumberPart = nextInvoiceNumber.ToString("D5");

            // Step 3: Append the customer number
            string customerNumberPart = int.Parse(klantnummer).ToString("D3");

            // Combine to form the final InvoiceNumber
            string InvoiceNumber = $"{yearPart}{invoiceNumberPart}{customerNumberPart}";

            // Navigate to FactuurPage and pass the details
            await Navigation.PushAsync(new FactuurPage(Klantnaam, Addres, Postcode, InvoiceNumber));
        }

    }
}
