using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Photos")]
public class Photo : BaseEntity
{
    public string Url { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; }

    public int ApartmentId { get; set; }
    public Apartment Apartment { get; set; }
}