namespace JesseAdminApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Wrap MainPage in a NavigationPage
        MainPage = new NavigationPage(new MainPage());
    }
}
