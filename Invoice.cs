using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JesseAdminApp
{
    public class Invoice
    {
        [PrimaryKey]
        public string InvoiceNumber { get; set; } = string.Empty; // Format: YYYYDDDDDKKK

        public decimal AmountInclBTW { get; set; } // Amount including BTW
        public decimal AmountExclBTW { get; set; } // Amount excluding BTW
        public byte[] InvoicePDF { get; set; } // PDF stored as a byte array

        public int CustomerNumber { get; set; } // Customer number (KKK)
    }
}
