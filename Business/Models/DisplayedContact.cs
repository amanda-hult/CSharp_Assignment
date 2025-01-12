namespace Business.Models;

public class DisplayedContact : BaseContact
{
    public string ContactId { get; set; } = null!;

    //empty constructor as an alternative to setting the parameters directly in DisplayedContact Create() in ContactFactory
    public DisplayedContact() { }

}
