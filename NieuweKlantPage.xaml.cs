using Microsoft.Maui.Controls;

namespace JesseAdminApp
{
    public partial class NieuweKlantPage : ContentPage
    {
        private readonly CustomerDatabase _database;

        public NieuweKlantPage()
        {
            InitializeComponent();
            _database = new CustomerDatabase();
        }

        private async void OnOpslaanClicked(object sender, EventArgs e)
        {
            // Check if Naam, Achternaam, and Klantnummer are not empty
            if (!string.IsNullOrEmpty(NaamEntry.Text) &&
                !string.IsNullOrEmpty(AchternaamEntry.Text))
            {
                var latestKlantnummer = await _database.GetLatestKlantnummerAsync();
                var newKlantnummer = (latestKlantnummer + 1).ToString();  // Increment the latest Klantnummer

                var newCustomer = new Customer
                {
                    Naam = NaamEntry.Text,
                    Achternaam = AchternaamEntry.Text,
                    Klantnummer = int.Parse(newKlantnummer),  // Save Klantnummer as a string
                    Adres = AdresEntry.Text,
                    Postcode = PostcodeEntry.Text,
                    DatumVanStart = DatumVanStartPicker.Date
                };

                await _database.SaveCustomerAsync(newCustomer);
                MessagingCenter.Send(this, "RefreshKlantinformatiePage"); // Notify to refresh
                await Navigation.PopAsync(); // Go back to KlantinformatiePage
            }
            else
            {
                // Display an alert if any required field is empty
                var missingFields = new List<string>();

                if (string.IsNullOrEmpty(NaamEntry.Text))
                    missingFields.Add("Naam");

                if (string.IsNullOrEmpty(AchternaamEntry.Text))
                    missingFields.Add("Achternaam");

                var missingFieldsMessage = string.Join(", ", missingFields);

                await DisplayAlert("Invalid Input", $"The following fields are missing: {missingFieldsMessage}", "OK");
            }
        }
    }
}
