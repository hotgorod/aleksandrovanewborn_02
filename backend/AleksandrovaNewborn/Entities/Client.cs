namespace AleksandrovaNewborn.Entities;

public class Client
{
    public int Id { get; set; }
    
    public string ChildName { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public string? EmailAddress { get; set; }

    public Client(string childName)
    {
        ChildName = childName;
    }
}