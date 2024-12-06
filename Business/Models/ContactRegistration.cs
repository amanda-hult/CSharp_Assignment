using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class ContactRegistration : BaseContact
{
    [Required]
    public override string FirstName { get; set; } = null!;

    [Required]
    public override string LastName { get; set; } = null!;

    [Required]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address.")]
    public override string Email { get; set; } = null!;

    [Phone]
    [RegularExpression(@"^\+?[0-9]{1,4}?[-.\s]?(\(?[0-9]{1,3}?\)?[-.\s]?)*[0-9]{3,}$", ErrorMessage = "Invalid phone number.")]
    public override string Phone { get; set; } = null!;

    [StringLength(100, ErrorMessage = "Address cannot be longer than 100 characters.")]
    public override string Address { get; set; } = null!;

    [RegularExpression(@"^\d{3}\s?\d{2}$", ErrorMessage = "Postcode must be 5 digits long.")]
    public override string Postcode { get; set; } = null!;

    [StringLength(50, ErrorMessage = "City name cannot be longer than 50 characters.")]
    public override string City { get; set; } = null!;
}
