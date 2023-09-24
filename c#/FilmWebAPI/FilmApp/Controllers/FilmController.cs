using FilmRecenzijaApp.Data;
using FilmRecenzijaApp.Models;
using FilmRecenzijaApp.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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
        private readonly ILogger<FilmController> _logger;

        public FilmController(FilmRecenzijaContext context, ILogger<FilmController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>Dohvaća sve filmove iz baze</summary>
        /// <remarks> **Primjer upita:** ```GET api/v1/Film``` </remarks>
        /// <response code="200"> Lista filmova </response>
        /// <response code="204"> Ne postoji niti jedan film </response>
        /// <response code="400"> Zahtjev nije valjan (BadRequest) </response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpGet]
        public IActionResult Get()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filmovi = _context.Film.ToList();
            if (filmovi == null || filmovi.Count == 0)
            {
                return NoContent();
            }


            List<FilmDTO> vrati = new();

            filmovi.ForEach(f =>
            {
                var fdto = new FilmDTO()
                {
                    Sifra = f.Sifra,
                    Naziv = f.Naziv,
                    Godina = f.Godina,
                    Redatelj = f.Redatelj,
                    Zanr = f.Zanr,
                };

                vrati.Add(fdto);

            });

            return Ok(vrati);

        }


        /// <summary>Dodaje film u bazu</summary>
        /// <remarks>
        ///**Primjer upita:** 
        ///```
        ///POST api/v1/Film 
        ///{
        /// "naziv": "string",
        /// "godina": 0,
        /// "redatelj": "string",
        /// "zanr": "string",
        ///}
        ///```
        /// </remarks>
        /// <response code="200">Kreirani film</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpPost]
        public IActionResult Post(FilmDTO filmdto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Film f = new Film()
                {
                    Naziv = filmdto.Naziv,
                    Godina = filmdto.Godina,
                    Redatelj = filmdto.Redatelj,
                    Zanr = filmdto.Zanr
                };

                _context.Film.Add(f);
                _context.SaveChanges();
                filmdto.Sifra = f.Sifra;
                return Ok(filmdto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }


        }


        /// <summary>Izmjeni podatke postojećeg filma u bazi</summary>
        /// <remarks>
        /// **Primjer upita:** 
        ///```
        ///PUT api/v1/film/1
        ///{
        /// "naziv": "string",
        /// "godina": 0,
        /// "redatelj": "string",
        /// "zanr": "string"
        ///}
        ///```
        /// </remarks>
        /// <param name="sifra">Šifra filma koji se mijenja</param>  
        /// <response code="200">Objekt izmjenjenog filma</response>
        /// <response code="204">Nema u bazi filma kojeg želimo promijeniti</response>
        /// <response code="400">Zahtjev nije valjan(BadRequest)</response>
        /// <response code="503">Na azure treba dodati IP u firewall</response> 
        [HttpPut]
        [Route("{sifra:int}")]
        public IActionResult Put(int sifra, FilmDTO fdto)
        {

            if (sifra <= 0 || fdto == null)
            {
                return BadRequest();
            }

            try
            {
                var filmBaza = _context.Film.Find(sifra);
                if (filmBaza == null)
                {
                    return NoContent();
                }
                filmBaza.Naziv = fdto.Naziv;
                filmBaza.Godina = fdto.Godina;
                filmBaza.Redatelj = fdto.Redatelj;
                filmBaza.Zanr = fdto.Zanr;

                _context.Film.Update(filmBaza);
                _context.SaveChanges();
                fdto.Sifra = filmBaza.Sifra;

                return StatusCode(StatusCodes.Status200OK, fdto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex);
            }

        }


        /// <summary> Briše film iz baze </summary>
        /// <remarks> **Primjer upita:** ``` DELETE api/v1/film/1 ``` </remarks>
        /// <param name="sifra">Šifra filma koji se briše</param>  
        /// <response code="200">Film uspješno obrisan</response>
        /// <response code="204">Nema u bazi filma kojeg želimo obrisati</response>
        /// <response code="400">Zahtjev nije valjan(BadRequest)</response> 
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
                return NoContent();
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

        /// <summary> Dohvaćanje svih glumaca filma </summary>
        /// <remarks> **Primjer upita:** ```GET api/v1/film/1``` </remarks>
        /// <param name="sifra">Šifra filma za kojeg se dohvaćaju glumci</param>  
        /// <response code="200">Lista glumaca traženog filma</response>
        /// <response code="204">Nema u bazi filma za kojeg želimo dohvatiti glumce ili film nema glumaca</response>
        /// <response code="400">Zahtjev nije valjan(BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpGet]
        [Route("{sifra:int}/glumci")]
        public IActionResult GetGlumci(int sifra)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (sifra <= 0)
            {
                return BadRequest();
            }

            try
            {

                var film = _context.Film
                    .Include(f => f.Glumci)
                    .FirstOrDefault(f => f.Sifra == sifra);

                if (film == null)
                {
                    return BadRequest();
                }

                if (film.Glumci == null || film.Glumci.Count == 0)
                {
                    return NoContent();
                }

                List<GlumacDTO> vrati = new();

                film.Glumci.ForEach(g =>
                {
                    vrati.Add(new GlumacDTO()
                    {
                        Sifra = g.Sifra,
                        Ime = g.Ime,
                        Prezime = g.Prezime,
                        Drzavljanstvo = g.Drzavljanstvo,
                    });
                });

                return Ok(vrati);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                    ex.Message);
            }
        }

        /// <summary> Dodaje glumca na film </summary>
        /// <remarks> **Primjer upita:** ``` POST api/v1/Film/1/dodajGlumca/3 ``` </remarks>
        /// <param name="sifra">Šifra filma na kojeg se dodaje glumac</param>  
        /// <param name="glumacSifra">Šifra glumca koji se dodaje</param>  
        /// <response code="200">Glumac uspješno dodan na film</response>
        /// <response code="204">Glumac već postoji na tom filmu</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpPost]
        [Route("{sifra:int}/dodajGlumca/{glumacSifra:int}")]
        public IActionResult PostGlumca(int sifra, int glumacSifra)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (sifra <= 0 || glumacSifra <= 0)
            {
                return BadRequest();
            }

            try
            {

                var film = _context.Film
                    .Include(f => f.Glumci)
                    .FirstOrDefault(f => f.Sifra == sifra);

                if (film == null)
                {
                    return BadRequest();
                }

                var glumac = _context.Glumac.Find(glumacSifra);

                if (glumac == null)
                {
                    return BadRequest();
                }

                // kontrola da li je taj glumac već u tom filmu
                if (!film.Glumci.Contains(glumac))
                {
                    film.Glumci.Add(glumac);
                }
                else
                {
                    return NoContent();
                }

                _context.Film.Update(film);
                _context.SaveChanges();

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(
                       StatusCodes.Status503ServiceUnavailable,
                       ex.Message);
            }
        }

        /// <summary> Briše glumca s filma </summary>
        /// <remarks> **Primjer upita:** ``` DELETE api/v1/film/1/obrisiGlumca/1 ``` </remarks>
        /// <param name="sifra">Šifra filma sa kojeg se briše glumac</param>  
        /// <param name="glumacSifra">Šifra glumca koji se želi obrisati sa filma</param>  
        /// <response code="200">Glumac uspješno obrisan</response>
        /// <response code="204">Film sa šifrom ne postoji u bazi ili glumac sa šifromGlumac nije na tom filmu</response> 
        /// <response code="400">Zahvjet nije valjan (BadRequest)</response>
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpDelete]
        [Route("{sifra:int}/obrisiGlumca/{glumacSifra:int}")]
        public IActionResult DeleteGlumca(int sifra, int glumacSifra)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (sifra <= 0 || glumacSifra <= 0)
            {
                return BadRequest();
            }

            try
            {

                var film = _context.Film
                    .Include(f => f.Glumci)
                    .FirstOrDefault(f => f.Sifra == sifra);

                if (film == null)
                {
                    return BadRequest("Film s predanom šifrom ne postoji");
                }

                var glumac = _context.Glumac.Find(glumacSifra);

                if (glumac == null)
                {
                    return NoContent();
                }

                if (film.Glumci.Contains(glumac))
                {
                    film.Glumci.Remove(glumac);
                    _context.Film.Update(film);
                    _context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NoContent();

                }
            }
            catch (Exception ex)
            {
                return StatusCode(
                       StatusCodes.Status503ServiceUnavailable,
                       ex.Message);

            }

        }

        //TODO: omogućiti dohvaćanje samo jednog filma ovisno o predanoj šifri
        //hint: GET request; ulaz: šifra filma
    }
}

       