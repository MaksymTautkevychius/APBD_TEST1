namespace TEST1;




public class books_editions
{
    public int id { get; set; }
    public publishing_houses2 publishing_housesDB{ get; set; }
    public BooksDto2 BookDB { get; set; }
    public DateTime  release_date{ get; set; }  
    public DateTime  edition_title{ get; set; } 
}

public class BooksDto2
{
    public int id { get; set; }
    public String title { get; set; }
}

public class publishing_houses2 {
    public int id { get; set; }
    public String name { get; set; }
    public String owner_first_na { get; set; }
    public String owner_last_na { get; set; }
    
}
