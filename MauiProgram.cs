using Microsoft.Extensions.Logging;
using Syncfusion.Licensing;

namespace JesseAdminApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            SyncfusionLicenseProvider.RegisterLicense("MzU4ODYwOUAzMjM3MmUzMDJlMzBpcTZxTDB3U1pEL2RNazVBbjBBMjFyMUdNekRPc2pCMHhvck16a1hKVDFFPQ==");


            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
