namespace JesseAdminApp;
using Syncfusion.Licensing; 
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        //SyncfusionLicenseProvider.RegisterLicense("MzY5Mjg0NUAzMjM3MmUzMDJlMzBpakRBc3QzWEN2Q2c2TjlEUnRkc1pQbkR6Z0QybkFxWWsvQXhZZFk5VTJJPQ==");     //Syncfusion license key version 27.x.x

        InitializeComponent();
    }

    private async void OnFinancieelOverzichtClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FinancieelOverzichtPage());
    }

    private async void OnKlantinformatieClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new KlantinformatiePage());
    }

    private void SendToPathFacturen(object sender, EventArgs e)
    {
        try
        {
            // Get the path to the AppDataDirectory
            var appDataPath = FileSystem.Current.AppDataDirectory;

            // Update the Entry with the path and make it visible
            PathDisplayEntry.Text = appDataPath;
            PathDisplayEntry.IsVisible = true;
        }
        catch (Exception ex)
        {
            // Handle errors
            Application.Current.MainPage.DisplayAlert("Error", $"Could not retrieve the file path: {ex.Message}", "OK");
        }
    }

    // Method to display the path on the page
    private void DisplayPathOnPage(string path)
    {
        // Assuming you have an Entry or Label named `PathDisplayEntry` on your XAML page
        PathDisplayEntry.Text = path;
    }
}
