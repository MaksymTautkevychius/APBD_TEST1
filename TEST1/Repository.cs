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
                    be.ID AS books_editionsID,
                    be.release_date AS release_date,
                    be.edition_title AS edition_title,
                    b.ID as BookID,
                    b.title,
                    ph.ID AS publishing_housesID,
                    ph.Name
                    FROM books_editions be
                    JOIN books b ON b.ID = be.FK_book
                    JOIN publishing_houses ph ON ph.ID = be.FK_publishing_house
                    WHERE b.ID = @ID;";

        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);

            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new books_editions
                    {
                        id = reader.GetInt32(reader.GetOrdinal("books_editionsID")),
                        release_date = reader.GetDateTime(reader.GetOrdinal("release_date")),
                        edition_title = reader.GetDateTime(reader.GetOrdinal("edition_title")),
                        BookDB = new BooksDto2
                        {
                            id = reader.GetInt32(reader.GetOrdinal("BookID")),
                            title = reader.GetString(reader.GetOrdinal("title"))
                        },
                        publishing_housesDB = new publishing_houses2
                        {
                            id = reader.GetInt32(reader.GetOrdinal("publishing_housesID")),
                            name = reader.GetString(reader.GetOrdinal("Name"))
                        }
                    };
                }
            }
        }

        return null; 
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