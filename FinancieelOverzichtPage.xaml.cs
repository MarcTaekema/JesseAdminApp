using Microsoft.Maui.Controls;

namespace JesseAdminApp
{
    public partial class FinancieelOverzichtPage : ContentPage
    {
        private FinancialDatabase _FinanceDB;           //Database instatnie maken
        public FinancieelOverzichtPage()
        {
            InitializeComponent();
            _FinanceDB = new FinancialDatabase();       //Database koppelen aan pagina
        }

        private async void OnJaarrapportClicked(object sender, EventArgs e)
        {
            // Controleer of een jaar geselecteerd is
            if (YearPicker.SelectedItem is int selectedYear)
            {
                // Haal alle facturen op voor het geselecteerde jaar
                var invoices = await _FinanceDB.GetAllInvoicesAsync();

                // Filter facturen op geselecteerd jaar
                var yearInvoices = invoices.Where(invoice =>
                    invoice.InvoiceNumber.StartsWith(selectedYear.ToString())).ToList();

                // Controleer of er facturen zijn voor het jaar
                if (yearInvoices.Any())
                {
                    // Bereken totaalbedragen inclusief en exclusief BTW
                    decimal totalAmountInclBTW = yearInvoices.Sum(invoice => invoice.AmountInclBTW);
                    decimal totalAmountExclBTW = yearInvoices.Sum(invoice => invoice.AmountExclBTW);

                    // Toon rapportbericht met de totaalbedragen
                    string reportMessage = $"Totaal bedrag incl. BTW: {totalAmountInclBTW:C}\nTotaal bedrag excl. BTW: {totalAmountExclBTW:C}";
                    DisplayAlert("Jaarrapport", reportMessage, "OK");
                }
                else
                {
                    // Toon foutmelding als geen facturen gevonden
                    DisplayAlert("Fout", "Geen facturen gevonden.", "OK");
                }
            }
            else
            {
                // Toon foutmelding als geen jaar geselecteerd is
                DisplayAlert("Fout", "Selecteer een jaar.", "OK");
            }
        }

        private async void OnKwartaalOverzichtClicked(object sender, EventArgs e)
        {
            // Haal jaar en kwartaal op
            var year = YearPicker.SelectedItem as int?;
            var kwartaal = KwartaalPicker.SelectedItem as int?;

            // Controleer of jaar en kwartaal geselecteerd zijn
            if (year.HasValue && kwartaal.HasValue)
            {
                // Haal alle facturen op voor het geselecteerde jaar
                var invoices = await _FinanceDB.GetAllInvoicesAsync();

                // Filter facturen op geselecteerd jaar en kwartaal
                var kwartaalInvoices = invoices.Where(invoice =>
                    invoice.InvoiceNumber.StartsWith(year.Value.ToString()) &&
                    invoice.Kwartaalnummer == kwartaal.Value).ToList();

                // Controleer of er facturen zijn voor jaar en kwartaal
                if (kwartaalInvoices.Any())
                {
                    // Bereken totaalbedragen inclusief en exclusief BTW voor kwartaal
                    decimal totalAmountInclBTW = kwartaalInvoices.Sum(invoice => invoice.AmountInclBTW);
                    decimal totalAmountExclBTW = kwartaalInvoices.Sum(invoice => invoice.AmountExclBTW);

                    // Toon rapportbericht met kwartaal totaalbedragen
                    string reportMessage = $"Totaal bedrag incl. BTW: {totalAmountInclBTW:C}\nTotaal bedrag excl. BTW: {totalAmountExclBTW:C}";
                    DisplayAlert("Kwartaaloverzicht", reportMessage, "OK");
                }
                else
                {
                    // Toon foutmelding als geen facturen gevonden
                    DisplayAlert("Fout", "Geen facturen gevonden.", "OK");
                }
            }
            else
            {
                // Toon foutmelding als jaar en kwartaal niet geselecteerd zijn
                DisplayAlert("Fout", "Selecteer een jaar en kwartaal.", "OK");
            }
        }


    }
}
