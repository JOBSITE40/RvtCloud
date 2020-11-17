using ForgeAppTesting.Models;
using ForgeAppTesting.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ForgeAppTesting.Controllers
{
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<dynamic> Get() =>
            _bookService.Get();

        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public async Task<Book> Get(string id)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return null;
            }

            return book;
        }

        [HttpPost]
        public async Task<Book> Create(Book book)
        {
            _bookService.Create(book);

            //return CreatedAtRoute("GetBook", new { id = book.Id.ToString() }, book);
            return book;
        }

        [HttpPut("{id:length(24)}")]
        public async Task<dynamic> Update(string id, Book bookIn)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return null;
            }

            _bookService.Update(id, bookIn);

            return bookIn;
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<dynamic> Delete(string id)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return null;
            }

            _bookService.Remove(book.Id);

            return id;
        }
    }
}