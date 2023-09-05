using FilmRecenzijaApp.Data;
using FilmRecenzijaApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmRecenzijaApp.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FilmController : ControllerBase
    {
        private readonly FilmRecenzijaContext _context;
        public FilmController (FilmRecenzijaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get ()
        {
            return new JsonResult(_context.Film.ToList());
        }

        [HttpPost] 
        public IActionResult Post(Film film)
        {
            _context.Film.Add(film);
            _context.SaveChanges();

            return Created ("/api/v1/Film",film);
        }

        [HttpPut]
        [Route("{sifra:int}")]
        public IActionResult Put(int sifra, Film film) 
        {
            return StatusCode(StatusCodes.Status200OK, film);
        }

        [HttpDelete]
        [Route("{sifra:int}")]
        public IActionResult Delete(int sifra)
        {
            return StatusCode(StatusCodes.Status200OK, "{\"obrisano\":true}");
        }

    }
}
