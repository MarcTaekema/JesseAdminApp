using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JesseAdminApp
{
    public static class Constants
    {
        // Get the path for local application data
        public static string LocalFolder => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        // Customer Database Filename
        public const string CustomerDatabaseFilename = "CustomerDatabase.db3";

        // Financial Database Filename
        public const string FinancialDatabaseFilename = "FinanceDatabase.db3";

        // SQLite Flags
        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |  // open the database in read/write mode
            SQLite.SQLiteOpenFlags.Create |     // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.SharedCache; // enable multi-threaded database access

        // Get the full path for the Financial Database
        public static string FinancialDatabasePath =>
           Path.Combine(LocalFolder, FinancialDatabaseFilename);

        // Get the full path for the Customer Database
        public static string CustomerDatabasePath =>
            Path.Combine(LocalFolder, CustomerDatabaseFilename);
        //Local Database Folder
        //C:\Users\marct\AppData\Local\Packages\com.companyname.jesseadminapp_9zz4h110yvjzm\LocalCache\Local        dis is the goede
        //C:\Users\marct\AppData\Local\Packages\com.companyname.jesseadminapp_9zz4h110yvjzm\LocalState              dis is not de goede, hier worden de PDFs opgeslagen. 
    }
}

