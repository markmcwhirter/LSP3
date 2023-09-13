using System.ComponentModel.DataAnnotations;

namespace LSP3.Model;
public partial class AuthorDto
{
    [Display(Name = "AuthorID")]
    public int AuthorID { get; set; }

    [Display(Name = "Prefix")]
    [DataType(DataType.Text)]
    public string? Prefix { get; set; }

    [Display(Name = "First Name")]
    [DataType(DataType.Text)]
    public string? FirstName { get; set; }

    [Display(Name = "Middle Name")]
    [DataType(DataType.Text)]
    public string? MiddleName { get; set; }

    [Display(Name = "Last Name")]
    [DataType(DataType.Text)]
    public string? LastName { get; set; }

    [Display(Name = "Suffix")]
    [DataType(DataType.Text)]
    public string? Suffix { get; set; }
	public string? Address1 { get; set; }
	public string? Address2 { get; set; }
	public string? City { get; set; }
	public string? State { get; set; }
	public string? ZIP { get; set; }
	public string? Country { get; set; }
	public string? BusinessPhone { get; set; }
	public string? HomePhone { get; set; }
	public string? CellPhone { get; set; }
	public string? Email { get; set; }
	public string? Password { get; set; }
	public string? DateCreated { get; set; }
	public string? DateUpdated { get; set; }
	public string? Username { get; set; }
	public string? Admin { get; set; }
	public string? Bio { get; set; }
}
