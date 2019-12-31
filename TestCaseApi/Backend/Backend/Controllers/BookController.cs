using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private BookContext db = new BookContext();

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("bookadd")]
        public async Task<IActionResult> bookadd([FromBody] Book book)
        {
            try
            {
                var data = db.Books.Where(x => x.author == book.author && x.title == book.title).FirstOrDefault();
                if (data == null)
                {
                    var kitap = new Book()
                    {
                        author = book.author,
                        title= book.title,
   
                    };
                    db.Books.Add(kitap);
                    db.SaveChanges();
                    
                    return Ok("Kayit Basarili");
                }
                return Conflict();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPut("putaddbook")]
        public async Task<IActionResult> putaddbook([FromBody] Book book)
        {
            try
            {
                var kitap = db.Books.Where(x => x.author == book.author && x.title == book.title).FirstOrDefault();
                if (kitap == null)
                {
                  
                    var b = new Book()
                    {
                        author = book.author,
                        title = book.title
                    };
                    if (b.author == "" || b.title == "")
                    {
                        return BadRequest("'eror': 'Field' 'author' is required'");
                    }
                    else
                    {
                        db.Books.Add(b);
                        db.SaveChanges();
                        return Ok("Kayıt Başarılı.");
                    }


                }
                else
                {
                    return Ok("Another book with similartitle and author already exists.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("allbook")]
        public async Task<IActionResult> book()
        {
            var data = await Task.Run(() => db.Books.ToList());
            if (data.Count>0)
            {
                return Ok(data);
            }
            else
            {
                return Ok("Case Çalışmamda test işlemine başlarken DB boş olmalıdır dediği için, lütfen önce put Testini çalıştırıp kitap ekleyin :)");
            }
          
        }
        [HttpGet("book")]
        public async Task<IActionResult> getbookid(int book_id)
        {
          
                var data = await Task.Run(() => db.Books.Where(x=>x.id==book_id).FirstOrDefault());
            if (data != null)
            {
                return Ok(data);
            }
            else
            {
                return NotFound("Book not found");
            }
            
        
        }

    }
}