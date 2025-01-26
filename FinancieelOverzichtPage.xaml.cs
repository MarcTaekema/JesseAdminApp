using Microsoft.Maui.Controls;

namespace JesseAdminApp
{
    public partial class FinancieelOverzichtPage : ContentPage
    {
        public FinancieelOverzichtPage()
        {
            InitializeComponent();
        }

        private void OnJaarrapportClicked(object sender, EventArgs e)
        {
            if (YearPicker.SelectedItem is int selectedYear)
            {
                DisplayAlert("Jaarrapport", $"Jaarrapport voor het jaar {selectedYear} wordt gegenereerd.", "OK");
                // Add logic here to generate and display the year report
            }
            else
            {
                DisplayAlert("Fout", "Selecteer alstublieft een jaar.", "OK");
            }
        }
    }
}
