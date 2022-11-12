using BooksAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BooksAPIDBContext dbContext;
        public BooksController(BooksAPIDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult> GetBooks()
        {
            try
            {
                return Ok(await dbContext.tbl_Books.ToListAsync());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ProducesResponseType(typeof(Books), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Books>> GetBook([FromRoute] Guid id)
        {
            try
            {
                var result = await dbContext.tbl_Books.FindAsync(id);
                if (result == null) return NotFound();
                return Ok(result);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostBook(AddBook model)
        {
            try
            {
                var book = new Books()
                {
                    Id = Guid.NewGuid(),
                    TitleName = model.TitleName,
                    Price = model.Price,
                    AuthorName = model.AuthorName,
                    AddedDate = DateTime.Now,
                    UpdatedDate = null
                };
                await dbContext.tbl_Books.AddAsync(book);
                await dbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, model);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new book record");
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutBook([FromRoute] Guid id, UpdateBook model)
        {
            var result = await dbContext.tbl_Books.FindAsync(id);
            if(result == null) return NotFound();
            result.TitleName = model.TitleName;
            result.Price = model.Price;
            result.AuthorName = model.AuthorName;
            result.UpdatedDate = DateTime.Now;
            result.AddedDate = result.AddedDate;
            await dbContext.SaveChangesAsync();
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBook([FromRoute] Guid id)
        {
            var bookToDelete = await dbContext.tbl_Books.FindAsync(id);
            if (bookToDelete == null) return NotFound();

            dbContext.tbl_Books.Remove(bookToDelete);
            await dbContext.SaveChangesAsync();

            return NoContent(); ;
        }


    }
}
