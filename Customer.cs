using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JesseAdminApp
{
    public class Customer
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Naam { get; set; } = string.Empty;
        public string Achternaam { get; set; } = string.Empty;
        public int Klantnummer { get; set; }
        public string Adres { get; set; } = string.Empty;
        public string Postcode { get; set; } = string.Empty;
        public DateTime DatumVanStart { get; set; }
    }
    public class DisplayCustomer
    {
        public string DisplayName { get; set; }
        public string Klantnummer { get; set; }  // New property for Klantnummer
    }


}
