using FilmRecenzijaApp.Data;
using FilmRecenzijaApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace FilmRecenzijaApp.Controllers
{
    /// <summary>
    /// Namijenjeno za CRUD operacije na entitetom film u bazi
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FilmController : ControllerBase
    {
        private readonly FilmRecenzijaContext _context;
        public FilmController (FilmRecenzijaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Dohvaća sve filmove iz baze
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    GET api/v1/Film
        ///
        /// </remarks>
        /// <returns>Filmovi u bazi</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
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


        /// <summary>
        /// Dodaje film u bazu
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    POST api/v1/Film
        ///    {Naziv:"", Godina:2020}
        ///
        /// </remarks>
        /// <returns>Kreirani film u bazi sa svim podacima</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
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


        /// <summary>
        /// Mijenja podatke postojećeg filma u bazi
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    PUT api/v1/film/1
        ///
        /// {
        ///  "sifra": 0,
        ///  "naziv": "string",
        ///  "godina": 0,
        ///  "redatelj": "string",
        ///  "zanr": "string"
        /// }
        ///
        /// </remarks>
        /// <param name="sifra">Šifra filma koji se mijenja</param>  
        /// <returns>Svi poslani podaci od filma</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="204">Nema u bazi filma kojeg želimo promijeniti</response>
        /// <response code="415">Nismo poslali JSON</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response> 
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

                return StatusCode(StatusCodes.Status200OK, filmBaza);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex);
            }
            
        }


        /// <summary>
        /// Briše film iz baze
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    DELETE api/v1/film/1
        ///    
        /// </remarks>
        /// <param name="sifra">Šifra filma koji se briše</param>  
        /// <returns>Odgovor da li je obrisano ili ne</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="204">Nema u bazi filma kojeg želimo obrisati</response>
        /// <response code="415">Nismo poslali JSON</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
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
