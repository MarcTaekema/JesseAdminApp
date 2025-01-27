using Microsoft.Extensions.Logging;
using Syncfusion.Licensing;

namespace JesseAdminApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            SyncfusionLicenseProvider.RegisterLicense("MzY5Mjg0NUAzMjM3MmUzMDJlMzBpakRBc3QzWEN2Q2c2TjlEUnRkc1pQbkR6Z0QybkFxWWsvQXhZZFk5VTJJPQ==");     //Syncfusion license key version 27.x.x
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");               //Beetje fonts toevoegen 
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
