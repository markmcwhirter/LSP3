
namespace LSP3.Model;
public partial class LogDto
{

    public int Id { get; set; }
	public string? Date { get; set; }
	public string? Thread { get; set; }
	public string? Level { get; set; }
	public string? Logger { get; set; }
	public string? Message { get; set; }
	public string? Exception { get; set; }
}
