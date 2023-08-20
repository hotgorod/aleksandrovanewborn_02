namespace AleksandrovaNewborn.Entities;

public class Photoshoot
{
    public int Id { get; set; }
    
    public Client Client { get; set; }
    
    public string Name { get; set; }
    
    public DateTime PlannedDate { get; set; }
    
    public DateTime PhotoshootDate { get; set; }
    
    public bool ClientCanDownload { get; set; }
    
    public IList<PhotoshootFolder> Folders { get; set; }
}