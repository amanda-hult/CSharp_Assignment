using System.ComponentModel.DataAnnotations;
namespace Business.Models;

[Serializable]
public class StoredContact : BaseContact
{
    [Key]
    public string ContactId { get; set; } = null!;

    //partial validation for updating a contact

    [MinLength(2, ErrorMessage = "First name must contain at least two characters")]
    public override string FirstName { get; set; } = null!;


    [MinLength(2, ErrorMessage = "Last name must contain at least two characters")]
    public override string LastName { get; set; } = null!;


    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$", ErrorMessage = "Invalid email address")]
    public override string Email { get; set; } = null!;

    [RegularExpression(@"^\+?[0-9]{1,4}?[-.\s]?(\(?[0-9]{1,3}?\)?[-.\s]?)*[0-9]{3,}$", ErrorMessage = "Invalid phone number")]
    public override string Phone { get; set; } = null!;

    [MaxLength(100, ErrorMessage = "Address cannot be longer than 100 characters")]
    public override string Address { get; set; } = null!;

    [RegularExpression(@"^\d{3}\s?\d{2}$", ErrorMessage = "Postcode must be 5 digits long")]
    public override string Postcode { get; set; } = null!;

    [MaxLength(50, ErrorMessage = "City name cannot be longer than 50 characters")]
    public override string City { get; set; } = null!;



    //empty constructor to make serialization possible
    public StoredContact() { }
}




