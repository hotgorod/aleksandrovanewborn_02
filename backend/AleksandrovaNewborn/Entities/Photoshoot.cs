namespace AleksandrovaNewborn.Entities;

public class Photoshoot
{
    public int Id { get; set; }
    
    public Client Client { get; set; }
    
    public string Name { get; set; }
    
    public DateTime PhotoshootDate { get; set; }
    
    public DateTime? ExpirationDate { get; set; }
    
    public bool ClientCanDownload { get; set; }
    
    public IList<PhotoshootFolder> Folders { get; set; }

    public Photoshoot()
    {
        
    }
    
    public Photoshoot(Client client, string name, DateTime photoshootDate)
    {
        if (client == null) throw new ArgumentNullException(nameof(client));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

        Client = client;
        Name = name;
        PhotoshootDate = photoshootDate;
    }
    
}