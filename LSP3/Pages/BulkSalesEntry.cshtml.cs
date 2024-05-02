using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

using System.Globalization;
using System.Text;

namespace LSP3.Pages;

public class BulkSalesEntryModel : PageModel
{

    private readonly IWebHostEnvironment _environment;

    private readonly AppSettings _appSettings;

    public BulkSalesEntryModel(IOptions<AppSettings> appSettings, IWebHostEnvironment environment)
    {
        _appSettings = appSettings.Value;
        _environment = environment;
    }

    [BindProperty]
    public IFormFile? Upload { get; set; }

    [BindProperty]
    public string? Status { get; set; }


    public async Task OnPostAsync()
    {
        HttpHelper helper = new();

        var file = Path.Combine(_environment.ContentRootPath, "data", Upload.FileName);
        using (var fileStream = new FileStream(file, FileMode.Create))
        {
            await Upload.CopyToAsync(fileStream);
        }
        decimal royalties = 0.0M, salestodate = 0.0M, salesthisperiod = 0.00M;
        int booktype = 0, bookid = 0, units = 0, unitstodate = 0;


        string strbooktype = "";
        string? input = "";
        DateTime inputdate = DateTime.MinValue;

        StringBuilder sb = new();

        try
        {
            using (StreamReader sr = new(file))
            {
                input = sr.ReadLine();


                sb.AppendLine(@"<div class='container-fluid'>");
                sb.AppendLine(@"<div class='content'>");
                sb.AppendLine(@"<div class='row'>");
				sb.AppendLine(@"<div class='col-sm-1 text-sm-end'>Book ID</div>");
				sb.AppendLine(@"<div class='col-sm-1 text-sm-end'>Sales Date</div>");
				sb.AppendLine(@"<div class='col-sm-1 text-sm-end'>Book Type</div>");
				sb.AppendLine(@"<div class='col-sm-1 text-sm-end'>Units Sold</div>");
				sb.AppendLine(@"<div class='col-sm-1 text-sm-end'>Units To Date</div>");
				sb.AppendLine(@"<div class='col-sm-1 text-sm-end'>Royalty</div>");
				sb.AppendLine(@"<div class='col-sm-1 text-sm-end'>Sales This Period</div>");
				sb.AppendLine(@"<div class='col-sm-1 text-sm-end'>Sales To Date</div>");
                sb.AppendLine(@"<div class='col-sm-4'>&nbsp;</div>");
                sb.AppendLine(@"</div>");



                int count = 1;

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


                    if (booktype == 5)
                        strbooktype = "EBook";
                    else
                        strbooktype = "Paperback";

                    string row = "";

                    if (count % 2 == 0)
                        row = "<div class='row color-blue'>";
                    else
                        row = "<div class='row color-lightblue'>";

                    string strinputdate = inputdate.ToShortDateString();

                    string strunit = units.ToString().PadLeft(10, ' '); 
                    string strunitstodate = unitstodate.ToString().PadLeft(10, ' '); 

                    string strroyalty = royalties.ToString("C2").PadLeft(10, ' '); 
                    string strsalesthisperiod = salesthisperiod.ToString("C2").PadLeft(10, ' '); 
                    string strsalestodate = salestodate.ToString("C2").PadLeft(10, ' '); 

                    sb.AppendFormat($@"{row}<div class='col-sm-1 text-sm-end'>{bookid}</div><div class='col-sm-1 text-sm-end'>{strinputdate}</div><div class='col-sm-1 text-sm-end'>{strbooktype}</div><div class='col-sm-1 text-sm-end'>{strunit}</div><div class='col-sm-1  text-sm-end'>{strunitstodate}</div><div class='col-sm-1 text-sm-end'>{strroyalty}</div><div class='col-sm-1 text-sm-end'>{strsalesthisperiod}</div><div class='col-sm-1 text-sm-end'>{strsalestodate}</div><div class='col-sm-4'>&nbsp;</div></div>");

                    royalties = 0.0M;
                    booktype = 0;
                    bookid = 0;
                    units = 0;
                    unitstodate = 0;
                    strbooktype = "";
                    input = "";

                    // apiResponse = await helper.Get(_appSettings.HostUrl + $"author/{authorid}");
                    count++;
				}
            }
            sb.AppendLine("</div></div>");
            Status = sb.ToString();
        }
        catch (Exception ex)
        {
            sb.Clear();
            sb.Append("ERROR: ");
            sb.Append(ex.Message);
            sb.Append("<BR>");
            sb.Append(ex.InnerException);
            sb.Append("<BR>");
            sb.Append(ex.StackTrace);
            Status = sb.ToString();
        }
    }


    private static void MyParse(string p1, ref int qty, string p2)
    {
        bool result = int.TryParse(p1, out qty);
        if (!result)
            throw new InvalidDataException("Error parsing " + p2);
    }
    private static void MyParse(string p1, ref decimal qty, string p2)
    {
        bool result = decimal.TryParse(p1, out qty);
        if (!result)
            throw new InvalidDataException("Error parsing " + p2);
    }
    private static void MyParse(string p1, ref DateTime inputdate, string p2)
    {
        bool result = DateTime.TryParse(p1, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out inputdate);
        if (!result)
            throw new InvalidDataException("Error parsing " + p2);

    }

}
