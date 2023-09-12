using FilmRecenzijaApp.Data;
using FilmRecenzijaApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var filmovi = _context.Film.ToList();
                if(filmovi==null || filmovi.Count==0)
                {
                    return new EmptyResult();
                }
                return new JsonResult(_context.Film.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode (StatusCodes.Status503ServiceUnavailable, ex.Message);
            }

        }

        [HttpPost] 
        public IActionResult Post(Film film)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Film.Add(film);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status201Created, film);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
            

        }

        [HttpPut]
        [Route("{sifra:int}")]
        public IActionResult Put(int sifra, Film film) 
        {

            if (sifra<=0 || film ==  null)
            {
                return BadRequest();
            }

            try
            {
                var filmBaza = _context.Film.Find(sifra);
                if (filmBaza == null)
                {
                    return BadRequest();
                }
                filmBaza.Naziv = film.Naziv;
                filmBaza.Godina = film.Godina;
                filmBaza.Redatelj = film.Redatelj;
                filmBaza.Zanr = film.Zanr;

                _context.Film.Update(filmBaza);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, film);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex);
            }
            
        }

        [HttpDelete]
        [Route("{sifra:int}")]
        public IActionResult Delete(int sifra)
        {
            if (sifra <= 0)
            {
                return BadRequest();
            }

            var filmBaza = _context.Film.Find(sifra);
            if (filmBaza == null)
            {
                return BadRequest();
            }

            try
            {
                _context.Film.Remove(filmBaza);
                _context.SaveChanges();

                return new JsonResult("{\"poruka\":\"Obrisano\"}");
            }
            catch (Exception ex)
            {
                return new JsonResult("{\"poruka\":\"Ne može se obrisati\"}");
            }
        }

    }
}
