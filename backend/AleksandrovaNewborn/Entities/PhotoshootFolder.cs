namespace AleksandrovaNewborn.Entities;

public class PhotoshootFolder
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public IList<Photo> Photos { get; set; }
}