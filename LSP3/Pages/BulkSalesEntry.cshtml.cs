using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using System.Globalization;

namespace LSP3.Pages
{
    public class BulkSalesEntryModel : PageModel
    {

        private IWebHostEnvironment _environment;
        public BulkSalesEntryModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [BindProperty]
        public IFormFile Upload { get; set; }

        [BindProperty]
        public string Status { get; set; }

        public void OnGet()
        {
        }

        public async Task OnPostAsync()
        {
            HttpHelper helper = new HttpHelper();

            var file = Path.Combine(_environment.ContentRootPath, "SalesData", Upload.FileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
            }
            decimal royalties = 0.0M, comp = 0.0M, salestodate = 0.0M, salesthisperiod = 0.00M;
            int booktype = 0, bookid = 0, units = 0, unitstodate = 0;
            int authorid = 0;

            string strbooktype = "";
            string input = "";
            DateTime inputdate = DateTime.MinValue;

            StringBuilder sb = new StringBuilder();

            try
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    input = sr.ReadLine();
                    sb.AppendLine(@"<table border='1'>");
                    sb.AppendLine("<tr><td>Book ID</td><td>Sales<br/>Date</td><td>Book<br />Type</td><td>Units<br />Sold</td><td>Units<br />Sold<br /> To<br /> Date</td><td>Royalty</td><td>Sales<br /> This<br /> Period</td><td>Sales<br /> To<br /> Date</td></tr>");

                    while ((input = sr.ReadLine()) != null)
                    {
                        string[] vals = input.Split(',');

                        MyParse(vals[0], ref booktype, "booktype");
                        MyParse(vals[1], ref inputdate, "input date");
                        MyParse(vals[2], ref bookid, "bookid");
                        MyParse(vals[3], ref units, "units");
                        MyParse(vals[4], ref unitstodate, "unitstodate");
                        MyParse(vals[5], ref royalties, "royalties");
                        MyParse(vals[6], ref salestodate, "salestodate");
                        MyParse(vals[7], ref salesthisperiod, "salesthisperiod");


                        //clsSales i = new clsSales
                        //{
                        //    VendorID = booktype,
                        //    SalesDate = inputdate,
                        //    BookID = bookid,
                        //    UnitsSold = units,
                        //    UnitsToDate = unitstodate,
                        //    Royalty = royalties,
                        //    SalesToDate = salestodate,
                        //    SalesThisPeriod = salesthisperiod,
                        //    DateCreated = DateTime.Now.ToString()
                        //};

                        // check to see if entry already there
                        //int unitscheck = sales.BooksSoldByDate(bookid, inputdate, booktype);

                        //if (unitscheck > 0)
                        //    sales.DeleteSalesRange(bookid, inputdate, booktype);

                        //sales.InsertSale(i);

                        if (booktype == 5)
                            strbooktype = "EBook";
                        else
                            strbooktype = "Paperback";

                        sb.AppendFormat("<tr><td>{0}</td><td>{1}<td>{2}</td><td>{3}</td><td>{4}</td><td>{5:C}</td><td>{6:C}</td><td>{7:C}</td></tr>",
                            bookid, inputdate.ToShortDateString(), strbooktype, units, unitstodate, royalties, salesthisperiod, salestodate);

                        royalties = 0.0M;
                        booktype = 0;
                        bookid = 0;
                        comp = 0.0M;
                        units = 0;
                        unitstodate = 0;
                        strbooktype = "";
                        input = "";

                    }
                }
                sb.AppendLine("</table>");
                Status = sb.ToString();
            }
            catch (Exception ex)
            {
                sb.Clear();
                sb.Append("<div>ERROR: ");
                sb.Append(ex.Message);
                sb.Append("<BR>");
                sb.Append(ex.InnerException);
                sb.Append("<BR>");
                sb.Append(ex.StackTrace);
                Status = sb.ToString();
            }
        }


        private void MyParse(string p1, ref int qty, string p2)
        {
            bool result = int.TryParse(p1, out qty);
            if (!result)
                throw new InvalidDataException("Error parsing " + p2);
        }
        private void MyParse(string p1, ref decimal qty, string p2)
        {
            bool result = decimal.TryParse(p1, out qty);
            if (!result)
                throw new InvalidDataException("Error parsing " + p2);
        }
        private void MyParse(string p1, ref DateTime inputdate, string p2)
        {
            bool result = DateTime.TryParse(p1, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out inputdate);
            if (!result)
                throw new InvalidDataException("Error parsing " + p2);

        }

    }
}
