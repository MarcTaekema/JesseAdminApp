namespace JesseAdminApp;

using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Graphics.Fonts;
using System.Collections.ObjectModel;
using Syncfusion.Licensing;

public partial class FactuurPage : ContentPage
{
    private FinancialDatabase _FinanceDB;
    private string _pdfPath = string.Empty;
    public ObservableCollection<DescriptionItem> Descriptions { get; set; }
    public FactuurPage(string Klantnaam, string Adres, string Postcode, string InvoiceNumber)
    {
        InitializeComponent();
        Descriptions = new ObservableCollection<DescriptionItem>();
        DescriptionsCollectionView.ItemsSource = Descriptions;

        _FinanceDB = new FinancialDatabase();

        PostalCodeEntry.Text = Postcode;
        AddressEntry.Text = Adres;
        CustomerNameEntry.Text = Klantnaam;
        InvoiceNumberEntry.Text = InvoiceNumber;
        OnPropertyChanged();
    }

    private void OnAddDescriptionClicked(object sender, EventArgs e)
    {
       Descriptions.Add(new DescriptionItem { Quantity = "", Description = "", Price = "" });
    }

    private void OnRemoveDescriptionClicked(object sender, EventArgs e)
    {
        if (Descriptions.Count > 0) // Ensure there are items to remove Edit voor niks
        {
            Descriptions.RemoveAt(Descriptions.Count - 1); // Remove the last item
        }
    }

    private async void OnGeneratePdfClicked(object sender, EventArgs e)
    {
        try
        {
            // Validate inputs
            if (string.IsNullOrEmpty(CustomerNameEntry.Text) ||
                string.IsNullOrEmpty(AddressEntry.Text) ||
                string.IsNullOrEmpty(PostalCodeEntry.Text) ||
                //string.IsNullOrEmpty(CityEntry.Text) ||
                string.IsNullOrEmpty(InvoiceNumberEntry.Text))
            {
                await DisplayAlert("Error", "Please fill all the fields.", "OK");
                return;
            }

            // Calculate total amount
            decimal totalAmount = 0;
            foreach (var item in Descriptions)
            {
                if (decimal.TryParse(item.Price, out var amount))
                {
                    totalAmount += amount;
                }
            }
            TotalAmountEntry.Text = totalAmount.ToString("C");
            int StandardMargin = 40;                                    //Standard Margin
            int StandardYMargin = 30;

            string BTWNummer = "NL000099998B57";
            string KVKNummer = "93056589";
            string IBANNummer = "NL20INGB0001234567";
            // Create a new PDF document
            PdfDocument pdfDocument = new();                            //Declare a new PDF
                                                                        // PdfTrueTypeFont EuroFont = new PdfTrueTypeFont(new Syncfusion.Drawing.Font("Arial", 12, FontStyle.Regular), true);
            pdfDocument.PageSettings.Margins.All = 0;
            PdfPage page = pdfDocument.Pages.Add();                     //Add a page.
            pdfDocument.PageSettings.Size = PdfPageSize.A4;                //set size to A4
                                                                           //Declaring objects for all the different font sizes used in the PDF 
            FileStream fontStream = new FileStream("C:\\Users\\marct\\source\\repos\\PDFGeneration\\Resources\\Fonts\\OpenSans-Regular.ttf", FileMode.Open, FileAccess.Read);
            PdfFontSettings fontSettings = new PdfFontSettings(12, PdfFontStyle.Regular, true, true, true);
            PdfFont font = new PdfTrueTypeFont(fontStream, fontSettings);
            //PdfStandardFont font = new("OpenSansRegular", 12);
            //font.SetTextEncoding(Encoding.GetEncoding("Latin1"));
            PdfStandardFont boldFont = new(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
            PdfStandardFont Zestien = new(PdfFontFamily.Helvetica, 16);
            PdfStandardFont InfoFont = new(PdfFontFamily.Helvetica, 18);
            PdfStandardFont headerFont = new(PdfFontFamily.Helvetica, 40);
            PdfStandardFont TotalAmountFont = new(PdfFontFamily.Helvetica, 25);
            PdfGraphics graphics = page.Graphics;       //Declare an object for page graphics like a background

            //Add Colors and fields first, then text strings later. 
            PdfBrush Brush = new PdfSolidBrush(Syncfusion.Drawing.Color.FromArgb(144, 238, 144)); // RGB for dark blue

            graphics.DrawRectangle(Brush, new Syncfusion.Drawing.RectangleF(0, 0, page.GetClientSize().Width, StandardMargin + 240));
            // Add a white diagonal cutoff using a polygon shape
            PdfBrush whiteBrush = PdfBrushes.White;
            Syncfusion.Drawing.PointF[] whiteTrianglePoints = {
    new Syncfusion.Drawing.PointF(0, StandardMargin +240),                             // Bottom-left of the rectangle
    new Syncfusion.Drawing.PointF(page.GetClientSize().Width, StandardMargin + 120),                   // Top-right of the rectangle
    new Syncfusion.Drawing.PointF(page.GetClientSize().Width, StandardMargin +250)                  // Bottom-right of the rectangle
};
            float pageHeight = page.GetClientSize().Height; //842

            // Draw the dark blue rectangle at the bottom
            graphics.DrawRectangle(Brush, new Syncfusion.Drawing.RectangleF(0, 850 - 110, page.GetClientSize().Width, StandardMargin + 250));

            // Add a white diagonal cutoff using a polygon shape for the bottom
            PdfBrush whiteBrushBottom = new PdfSolidBrush(Syncfusion.Drawing.Color.FromArgb(144, 238, 144)); // RGB for dark blue
            Syncfusion.Drawing.PointF[] whiteTrianglePointsBottom = {
    new Syncfusion.Drawing.PointF(0, 850-110),              // Bottom-left diagonal point
    new Syncfusion.Drawing.PointF(page.GetClientSize().Width, pageHeight - (StandardMargin + 180)), // Top-right of the rectangle
    new Syncfusion.Drawing.PointF(page.GetClientSize().Width, pageHeight - (StandardMargin + 50))   // Bottom-right of the rectangle
};
            // Draw the white polygon for the diagonal cutoff at the bottom
            graphics.DrawPolygon(whiteBrushBottom, whiteTrianglePointsBottom);
            // Draw the white polygon for the diagonal cutoff
            graphics.DrawPolygon(whiteBrush, whiteTrianglePoints);

            //Add the Logo
            // Load the logo image
            //string logoFilePath = "C:\\Users\\marct\\Desktop\\aibende.png";
            string logoFilePath = "C:\\Users\\marct\\source\\repos\\PDFGeneration\\LogoFitPriority.jpg";
            using (FileStream fs = new FileStream(logoFilePath, FileMode.Open, FileAccess.Read))
            {
                PdfBitmap logo = new PdfBitmap(fs); // Load the image from the stream

                // Set the position and size for the logo
                float logoX = StandardMargin; // X-coordinate (distance from the left)
                float logoY = StandardMargin + 21; // Y-coordinate (distance from the top)
                float logoWidth = 150; // Desired width of the logo
                float logoHeight = 180; // Desired height of the logo

                // Draw the logo on the PDF
                graphics.DrawImage(logo, logoX, logoY, logoWidth, logoHeight);
            }
            int yPosition = 10;
            int TextPosX = 370;
            int TextPosXOverMij = 300;
            // Header
            //page.Graphics.DrawString("Factuur", headerFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + 0, StandardYMargin -20 ));
            // Seller Information
            page.Graphics.DrawString("FitPriority", headerFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + 300, StandardYMargin + yPosition));
            yPosition += 50;
            page.Graphics.DrawString("Over mij: ", boldFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + TextPosXOverMij, StandardYMargin + yPosition));
            yPosition += 20;
            page.Graphics.DrawString("Jesse van Berkum", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + TextPosXOverMij, StandardYMargin + yPosition));
            yPosition += 20;
            page.Graphics.DrawString("Oer de Feart 8", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + TextPosXOverMij, StandardYMargin + yPosition));
            yPosition += 20;
            page.Graphics.DrawString("8637VW Wiuwert", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + TextPosXOverMij, StandardYMargin + yPosition));
            yPosition += 50;
            page.Graphics.DrawString("Contact: ", boldFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + TextPosX, StandardYMargin + yPosition));
            yPosition += 20;
            page.Graphics.DrawString("+31646565417", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + TextPosX, StandardYMargin + yPosition));
            yPosition += 20;
            page.Graphics.DrawString("fit-priority@hotmail.com", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + TextPosX, StandardYMargin + yPosition));
            yPosition += 20;
            page.Graphics.DrawString("fitpriority.nl", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + TextPosX, StandardYMargin + yPosition));
            yPosition += 30;
            //page.Graphics.DrawString("BTW: NL000099998B57" , font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + TextPosX, StandardYMargin + yPosition));
            // Define the line thickness (for example, 2 points thick)

            float lineThickness = 2.0f;
            float lineThicknessThin = 1.0f;

            // Set the starting and ending points of the line (from left to right)
            Syncfusion.Drawing.PointF startPoint = new(StandardMargin + 0, StandardYMargin + 250); // Starting point at x = 0 and y = 250
            Syncfusion.Drawing.PointF endPoint = new(StandardMargin + 515, StandardYMargin + 250);  // Ending point at x = 515 (A4 width) and y = 250
                                                                                                    // Set the line color (black in this case)
            PdfPen pen = new(PdfBrushes.Black, lineThickness);
            // Draw the line on the page
            page.Graphics.DrawLine(pen, startPoint, endPoint);

            // Customer Information and payment information
            int CustomerInfoMargin = 100;
            yPosition = 260;
            page.Graphics.DrawString($"KvK: ", boldFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + 0, StandardYMargin + yPosition));
            page.Graphics.DrawString($"{KVKNummer}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + CustomerInfoMargin, StandardYMargin + yPosition));

            yPosition += 20;
            page.Graphics.DrawString($"Btw: ", boldFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + 0, StandardYMargin + yPosition));
            page.Graphics.DrawString($"{BTWNummer}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + CustomerInfoMargin, StandardYMargin + yPosition));
            yPosition += 20;

            page.Graphics.DrawString($"IBAN: ", boldFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + 0, StandardYMargin + yPosition));
            page.Graphics.DrawString($"{IBANNummer}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + CustomerInfoMargin, StandardYMargin + yPosition));
            yPosition += 35;

            //page.Graphics.DrawString("Factuur Informatie", InfoFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + 0, StandardYMargin + yPosition));
            //page.Graphics.DrawString("Factuur Adres", InfoFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + 300, StandardYMargin + yPosition));
            //yPosition += 35;
            //page.Graphics.DrawString($"Factuurnummer: {InvoiceNumberEntry.Text}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + 0, StandardYMargin + yPosition));
            page.Graphics.DrawString($"Factuur: ", boldFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + 0, StandardYMargin + yPosition));
            page.Graphics.DrawString($"{InvoiceNumberEntry.Text}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + CustomerInfoMargin, StandardYMargin + yPosition));

            page.Graphics.DrawString($"{CustomerNameEntry.Text}", Zestien, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + TextPosX, StandardYMargin + yPosition));
            yPosition += 20;

            string currentDate = DateTime.Now.ToString("dd-MM-yyyy"); // You can adjust the format as needed
            DateTime currentdate = DateTime.Now;
            DateTime dueDate = currentdate.AddDays(28);
            string DueDate = dueDate.ToString("dd-MM-yyyy");

            //page.Graphics.DrawString($"Factuurdatum: {currentDate}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + 0, StandardYMargin + yPosition));
            page.Graphics.DrawString($"Datum: ", boldFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + 0, StandardYMargin + yPosition));
            page.Graphics.DrawString($"{currentDate}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + CustomerInfoMargin, StandardYMargin + yPosition));

            page.Graphics.DrawString($"{AddressEntry.Text}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + TextPosX, StandardYMargin + yPosition));

            yPosition += 20;

            page.Graphics.DrawString($"Betaaltermijn: ", boldFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + 0, StandardYMargin + yPosition));
            page.Graphics.DrawString($"{DueDate}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + CustomerInfoMargin, StandardYMargin + yPosition));

            page.Graphics.DrawString($"{PostalCodeEntry.Text}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + TextPosX, StandardYMargin + yPosition));

            yPosition += 20;
            Syncfusion.Drawing.PointF startPoint2 = new(StandardMargin + 0, StandardYMargin + 400); // Starting point at x = 0 and y = 250
            Syncfusion.Drawing.PointF endPoint2 = new(StandardMargin + 515, StandardYMargin + 400);  // Ending point at x = 515 (A4 width) and y = 250
                                                                                                     // Set the line color (black in this case)
            PdfPen pen2 = new(PdfBrushes.Black, lineThickness);
            // Draw the line on the page
            page.Graphics.DrawLine(pen2, startPoint2, endPoint2);

            //From here 

            // Define starting position for table layout
            yPosition = 420; // Start after the second line

            // Define column widths for the table (adjust as needed for spacing)
            float column1Width = 75; // Quantity
            float column2Width = 250; // Description
            float column3Width = 110;  // Unit Price
            float column4Width = 65;  // Total

            // Add table headers
            page.Graphics.DrawString("Aantal", boldFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + 0, StandardYMargin + yPosition));
            page.Graphics.DrawString("Omschrijving", boldFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + column1Width, StandardYMargin + yPosition));
            page.Graphics.DrawString("Prijs", boldFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + column1Width + column2Width, StandardYMargin + yPosition));
            page.Graphics.DrawString("Totaal", boldFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + column1Width + column2Width + column3Width, StandardYMargin + yPosition));

            yPosition += 25;

            //page.Graphics.DrawString($"City: {CityEntry.Text}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, yPosition));
            float SubTotalPrice = 0;
            foreach (var item in Descriptions)  // This function prints the stuff on the page very good
            {
                float TotalPriceOfServices = 0;
                if (float.TryParse(item.Price, out float Totalprice) &&
                float.TryParse(item.Quantity, out float Totalquantity))
                {
                    TotalPriceOfServices = Totalprice * Totalquantity;
                }
                else
                {
                    // Handle the case where conversion fails
                    Console.WriteLine("Invalid input: Price or Quantity is not a valid number.");
                }

                page.Graphics.DrawString($"{item.Quantity}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + 0, StandardYMargin + yPosition));
                page.Graphics.DrawString($"{item.Description}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + column1Width, StandardYMargin + yPosition));
                page.Graphics.DrawString($"€ {item.Price:C}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + column1Width + column2Width, StandardMargin + yPosition - 10));
                page.Graphics.DrawString($"€ {TotalPriceOfServices}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + column1Width + column2Width + column3Width, StandardYMargin + yPosition));

                yPosition += 25;

                SubTotalPrice = SubTotalPrice + TotalPriceOfServices;   //Count up all the costs of all services to 1 nice variable
            }

            /*               Syncfusion.Drawing.PointF startPoint3 = new(StandardMargin + 0, StandardMargin + 650); // Starting point at x = 0 and y = 250
                           Syncfusion.Drawing.PointF endPoint3 = new(StandardMargin + 515, StandardMargin + 650);  // Ending point at x = 515 (A4 width) and y = 250
                           // Set the line color (black in this case)
                           PdfPen pen3 = new(PdfBrushes.Black, lineThickness);
                           // Draw the line on the page
                           page.Graphics.DrawLine(pen, startPoint3, endPoint3);        */

            // Calculate subtotal, tax, and total amount
            float tax = SubTotalPrice * 0.21f; // 21% btw
            yPosition = 675;
            // Draw subtotal
            page.Graphics.DrawString($"totaal excl. btw:", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + 30 + column2Width, StandardYMargin + yPosition));
            page.Graphics.DrawString($"€ {SubTotalPrice - tax:F2}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + column1Width + column2Width + column3Width, StandardYMargin + yPosition));

            yPosition += 20; // Move to the next row for tax

            // Draw tax
            page.Graphics.DrawString($"totaal btw (21%):", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + column2Width + 30, StandardYMargin + yPosition));
            page.Graphics.DrawString($"€ {tax:F2}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + column1Width + column2Width + column3Width, StandardYMargin + yPosition));
            yPosition += 30; // Move to the next row for total amount

            Syncfusion.Drawing.PointF startPoint4 = new(StandardMargin + column2Width + 30, StandardYMargin + 718); // Starting point at x = 0 and y = 250
            Syncfusion.Drawing.PointF endPoint4 = new(StandardMargin + 485, StandardYMargin + 718);  // Ending point at x = 515 (A4 width) and y = 250
                                                                                                     // Set the line color (black in this case)
            PdfPen pen4 = new(PdfBrushes.Black, lineThicknessThin);
            // Draw the line on the page
            page.Graphics.DrawLine(pen4, startPoint4, endPoint4);

            // Draw total amount
            page.Graphics.DrawString($"Te betalen:", boldFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + column2Width + 30, StandardYMargin + yPosition));
            page.Graphics.DrawString($" \u20AC {SubTotalPrice:F2}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(StandardMargin + column1Width + column2Width + column3Width, StandardYMargin + yPosition));

            int currentMonth = DateTime.Now.Month;
            int Kwartaalnummer = (currentMonth - 1) / 3 + 1; // Quarter is 1-4 based on the month
            // Save the PDF
            string fileName = $"Factuur_{InvoiceNumberEntry.Text}Q{Kwartaalnummer}.pdf";
            _pdfPath = Path.Combine(FileSystem.Current.AppDataDirectory, fileName);

            using (var stream = new FileStream(_pdfPath, FileMode.Create, FileAccess.ReadWrite))
            {
                pdfDocument.Save(stream);
            }

            pdfDocument.Close(true);

            //Write stuff to database
            try // Opslaan naar database
            {
                // Explicitly cast SubTotalPrice and tax to decimal for precision
                var BedragExcl = (decimal)SubTotalPrice - (decimal)tax;
                var BedragIncl = (decimal)SubTotalPrice;

                var Factuurnummer = InvoiceNumberEntry.Text;
                
                // Extract the last three characters as Klantnummer
                var Klantnummer = int.Parse(Factuurnummer.Substring(Factuurnummer.Length - 3));

                int currentMonth2 = DateTime.Now.Month;
                int Kwartaalnummer2 = (currentMonth2 - 1) / 3 + 1; // Quarter is 1-4 based on the month

                // Create a new invoice object
                var newFinanceDBAddition = new Invoice
                {
                    InvoiceNumber = Factuurnummer, // No .Text, as Factuurnummer is already a string
                    AmountInclBTW = BedragIncl,
                    AmountExclBTW = BedragExcl,
                    CustomerNumber = Klantnummer, // Convert string to int
                    Kwartaalnummer = Kwartaalnummer2
                };

                // Save the invoice to the database
                await _FinanceDB.AddInvoiceAsync(newFinanceDBAddition);
            }
            catch
            {
                // Handle errors gracefully
                await DisplayAlert("Error", "Saving to database failed", "OK");
                return;
            }

            // Update UI
            StatusLabel.Text = "Invoice PDF Generated!";
            ViewButton.IsVisible = true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnViewPdfClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(_pdfPath))
        {
            await Launcher.Default.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(_pdfPath)
            });
        }
        else
        {
            await DisplayAlert("Error", "No PDF to display.", "OK");
        }
    }
}
