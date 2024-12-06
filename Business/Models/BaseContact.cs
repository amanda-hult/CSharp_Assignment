namespace Business.Models;

public abstract class BaseContact
{
    public virtual string FirstName { get; set; } = null!;
    public virtual string LastName { get; set; } = null!;
    public virtual string Email { get; set; } = null!;
    public virtual string Phone { get; set; } = null!;
    public virtual string Address { get; set; } = null!;
    public virtual string Postcode { get; set; } = null!;
    public virtual string City { get; set; } = null!;
}
