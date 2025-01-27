namespace JesseAdminApp;
using Syncfusion.Licensing;
public partial class App : Application
{
    public App()
    {
        //SyncfusionLicenseProvider.RegisterLicense("MzY5Mjg0NUAzMjM3MmUzMDJlMzBpakRBc3QzWEN2Q2c2TjlEUnRkc1pQbkR6Z0QybkFxWWsvQXhZZFk5VTJJPQ==");     //Syncfusion license key version 27.x.x

        InitializeComponent();

        // Wrap MainPage in a NavigationPage, zal wel t werkt. 
        MainPage = new NavigationPage(new MainPage());
    }
}
