using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace TEST1;

[ApiController]
[Route("/api/Books")]
public class Controller: ControllerBase
{
    public Repository _Repository;

    public Controller(Repository _Repository)
    {
        this._Repository = _Repository;
    }

    [HttpGet ("{id}")]
    public IActionResult Get_editors_of_books(int id)
    {
        var Book = _Repository.Getter(id);
        return Ok(Book);
    }
    
    [HttpPost]
    public IActionResult AddBook(BooksDto booksDto)
    {
        _Repository.AddBook(booksDto);
        return Ok();
    }
}