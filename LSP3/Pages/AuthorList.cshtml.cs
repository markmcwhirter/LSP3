using LSP3.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LSP3.Pages;

public class AuthorListModel : MasterModel
{
    [BindProperty]
    public List<AuthorDto> AuthorList { get; set; }

    [BindProperty]
    public List<BookDto> Books { get; set; }

    private readonly ILogger<AuthorListModel> _logger;

    public AuthorListModel(ILogger<AuthorListModel> logger, IHttpContextAccessor httpContextAccessor) : base( httpContextAccessor)
    {
        _logger = logger;
    }

    public async Task<IActionResult> OnGet()
    {
        HttpHelper helper = new HttpHelper();
        Extensions<List<AuthorDto>> extensions = new Extensions<List<AuthorDto>>();

        try
        {

            if (!base.IsAuthenticated)
                return Redirect("/Account/Login");

            string apiResponse = await helper.Get($"http://localhost:5253/api/author");
            AuthorList = extensions.Deserialize(apiResponse);
            foreach( var author in AuthorList )
            {
                author.Email = string.IsNullOrEmpty(author.Email) ? string.Empty : author.Email;
                //author.Email = author.Email.Replace("@", "_");
            }
        }
        catch (Exception ex)
        {
             _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
        }

        return Page();

    }


}

public class OrdersDetails
{
    public OrdersDetails()
    {

    }
    public OrdersDetails(int OrderID, string CustomerId, int EmployeeId, double Freight, bool Verified, DateTime OrderDate, string ShipCity, string ShipName, string ShipCountry, DateTime ShippedDate, string ShipAddress)
    {
        this.OrderID = OrderID;
        this.CustomerID = CustomerId;
        this.EmployeeID = EmployeeId;
        this.Freight = Freight;
        this.ShipCity = ShipCity;
        this.Verified = Verified;
        this.OrderDate = OrderDate;
        this.ShipName = ShipName;
        this.ShipCountry = ShipCountry;
        this.ShippedDate = ShippedDate;
        this.ShipAddress = ShipAddress;
    }
    public int? OrderID { get; set; }
    public string CustomerID { get; set; }
    public int? EmployeeID { get; set; }
    public double? Freight { get; set; }
    public string ShipCity { get; set; }
    public bool Verified { get; set; }
    public DateTime OrderDate { get; set; }
    public string ShipName { get; set; }
    public string ShipCountry { get; set; }
    public DateTime ShippedDate { get; set; }
    public string ShipAddress { get; set; }
}
