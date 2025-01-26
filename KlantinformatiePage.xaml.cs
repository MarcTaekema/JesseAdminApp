using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Linq;

namespace JesseAdminApp
{
    public partial class KlantinformatiePage : ContentPage
    {
        private readonly CustomerDatabase _database;
        public ObservableCollection<Customer> Customers { get; set; } = new ObservableCollection<Customer>();


        public KlantinformatiePage()
        {
            InitializeComponent();
            _database = new CustomerDatabase();
            LoadCustomers();
        }

        private async void LoadCustomers()
        {
            var customerList = (await _database.GetCustomers())
                 .OrderBy(c => c.Naam)
                 .Select(c => new DisplayCustomer
                 {
                     DisplayName = $"{c.Naam} {c.Achternaam}", // Remove Klantnummer from DisplayName
                     Klantnummer = c.Klantnummer.ToString()  // Store Klantnummer separately
                 })
                 .ToList();

            var displayCustomers = new ObservableCollection<DisplayCustomer>(customerList);
            CustomerListView.ItemsSource = displayCustomers;
        }
        private async void OnCustomerTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                // Get the selected customer object
                var selectedCustomer = (DisplayCustomer)e.Item;

                // Get the Klantnummer directly from the selected customer object
                var klantnummer = selectedCustomer.Klantnummer;

                // Navigate to the CustomerDetailPage and pass the Klantnummer
                await Navigation.PushAsync(new CustomerDetailPage(klantnummer));
            }
        }
        private async void OnNieuweKlantClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NieuweKlantPage());
        }
    }
}
