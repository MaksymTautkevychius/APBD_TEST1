namespace TEST1;

public class BooksDto
{
    public String title { get; set; }
    public List<publishing_houses> PublishingHousesList { get; set; } = new List<publishing_houses>();
}

public class publishing_houses {
    public int id { get; set; }
    public DateTime  release_date{ get; set; }  
    public DateTime  edition_title{ get; set; } 
    
    
}