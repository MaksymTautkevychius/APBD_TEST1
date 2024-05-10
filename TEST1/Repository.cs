using Microsoft.Data.SqlClient;

namespace TEST1;

public class Repository
{
    private IConfiguration _configuration;

    public Repository(IConfiguration _configuration)
    {
        this._configuration = _configuration;
    }

    public books_editions Getter(int id)
    {
        
        var query = @"SELECT 
                    books_editions.ID AS  books_editionsID,
                    books_editions.release_date AS release_date,
                    books_editions.edition_title AS edition_title,
                    book.ID as BookID,
                    title,
                    publishing_houses.ID AS publishing_houses,
			        Name
                    FROM books_editions
                    JOIN books ON books.ID = books_editions.book
		            JOIN publishing_houses ON publishing_houses.ID = books_editions.publishing_houses
                    WHERE books.ID = @ID;";
        SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        SqlCommand command = new SqlCommand(query, connection); 
        command.Parameters.AddWithValue("@id", id);

        connection.Open();
        publishing_houses publishingHouses = new publishing_houses();
        var books_editions1 = new books_editions();
        var read = command.ExecuteReader();
        command.ExecuteReader();
        while (read.Read())
        {
            books_editions1 = new books_editions
            {
                id = read.GetInt32(read.GetOrdinal("books_editionsID")),
                release_date = read.GetDateTime(read.GetOrdinal("release_date")),
                edition_title = read.GetDateTime(read.GetOrdinal("edition_title")),
                BookDB = new BooksDto2
                {
                    title = read.GetString(read.GetOrdinal("title"))
                },
                publishing_housesDB = new publishing_houses2
                {
                    name = read.GetString(read.GetOrdinal("name"))
                }
            };
        }
        connection.Close();
        return books_editions1;
    }

    
    
    
    public void AddBook(BooksDto booksDto)
    {
        SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        var query = @"insert into books(title)    
                                    values(@title); SELECT @@IDENTITY AS ID;";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@title", booksDto.title);
        connection.Open();
        var id = command.ExecuteScalar();
        var query2 = "INSERT INTO books_editions VALUES(@publishing_houses_id, @book_id, @edition_title,@release_date)";
        foreach (var proc in booksDto.PublishingHousesList)
        {
            command.Parameters.Clear();
            command.CommandText = query2;
            command.Parameters.AddWithValue("@book_id", id);
            command.Parameters.AddWithValue("@publishing_houses_id", proc.id);
            command.Parameters.AddWithValue("@edition_title", proc.edition_title);
            command.Parameters.AddWithValue("@release_date", proc.release_date);
            command.ExecuteNonQuery();


        }
    }
}