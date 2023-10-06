using FilmRecenzijaApp.Data;
using FilmRecenzijaApp.Models;
using FilmRecenzijaApp.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmRecenzijaApp.Controllers
{
    /// <summary>
    /// Namijenjeno za CRUD operacije na entitetom glumac u bazi
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GlumacController : ControllerBase
    {
        private readonly FilmRecenzijaContext _context;
        public GlumacController(FilmRecenzijaContext context)
        {
            _context = context;
        }


        /// <summary> Dohvaća sve glumce iz baze </summary>
        /// <remarks> **Primjer upita:** ```GET api/v1/Glumac``` </remarks>
        /// <response code="200">Lista glumaca</response>
        /// <response code="204">Ne postoji niti jedan glumac u bazi</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpGet]
        public IActionResult Get()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var glumci = _context.Glumac.ToList();
            if (glumci == null || glumci.Count == 0)
            {
                return NoContent();
            }

            List<GlumacDTO> vrati = new();

            glumci.ForEach(g =>
            {
                var gdto = new GlumacDTO()
                {
                    Sifra = g.Sifra,
                    Ime = g.Ime,
                    Prezime = g.Prezime,
                    Drzavljanstvo = g.Drzavljanstvo
                };

                vrati.Add(gdto);

            });

            return Ok(vrati);
        }


        /// <summary> Dodaje glumca u bazu </summary>
        /// <remarks> 
        ///**Primjer upita:**
        ///```
        ///POST api/v1/Glumac
        ///{
        /// Ime:"Johnny", 
        /// Prezime:"Depp"
        ///}
        ///```
        /// </remarks>
        /// <response code="200"> Glumac koji je kreiran </response>
        /// <response code="400"> Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpPost]
        public IActionResult Post(GlumacDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Glumac g = new Glumac()
                {
                    Ime = dto.Ime,
                    Prezime = dto.Prezime,
                    Drzavljanstvo = dto.Drzavljanstvo
                };

                _context.Glumac.Add(g);
                _context.SaveChanges();
                dto.Sifra = g.Sifra;
                return Ok(dto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }


        /// <summary> Mijenja podatke postojećeg glumca u bazi </summary>
        /// <remarks>
        /// **Primjer upita:**
        ///```
        ///    PUT api/v1/Glumac/1
        ///
        /// {
        ///  "ime": "string",
        ///  "prezime": "string",
        ///  "drzavljanstvo": "string"
        /// }
        ///```
        /// </remarks>
        /// <param name="sifra">Šifra glumca koji se mijenja</param>  
        /// <response code="200">Objekt izmjenjenog glumca</response>
        /// <response code="204">Nema u bazi glumca kojeg želimo promijeniti</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response> 
        [HttpPut]
        [Route("{sifra:int}")]
        public IActionResult Put(int sifra, GlumacDTO gdto)
        {

            if (sifra <= 0 || gdto == null)
            {
                return BadRequest();
            }

            try
            {
                var glumacBaza = _context.Glumac.Find(sifra);
                if (glumacBaza == null)
                {
                    return NoContent();
                }
                glumacBaza.Ime = gdto.Ime;
                glumacBaza.Prezime = gdto.Prezime;
                glumacBaza.Drzavljanstvo = gdto.Drzavljanstvo;

                _context.Glumac.Update(glumacBaza);
                _context.SaveChanges();
                gdto.Sifra = glumacBaza.Sifra;
                return StatusCode(StatusCodes.Status200OK, gdto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex);
            }
        }


        /// <summary> Briše glumca iz baze </summary>
        /// <remarks> **Primjer upita:** ```DELETE api/v1/Glumac/1``` </remarks>
        /// <param name="sifra">Šifra glumca koji se briše</param>  
        /// <response code="200">Glumac uspjeršno obirsan</response>
        /// <response code="204">Nema u bazi glumca kojeg želimo obrisati</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpDelete]
        [Route("{sifra:int}")]
        public IActionResult Delete(int sifra)
        {
            if (sifra <= 0)
            {
                return BadRequest();
            }

            var glumacBaza = _context.Glumac.Find(sifra);
            if (glumacBaza == null)
            {
                return NoContent();
            }

            try
            {
                _context.Glumac.Remove(glumacBaza);
                _context.SaveChanges();

                return new JsonResult("{\"poruka\":\"Obrisano\"}");
            }
            catch (Exception ex)
            {
                return new JsonResult("{\"poruka\":\"Ne može se obrisati\"}");
            }
        }

        /// <summary>Dohvaćanje svih filmova u kojima je glumac</summary>
        /// <remarks> **Primjer upita:** ```GET api/v1/Glumac/1``` </remarks>
        /// <param name="sifra">Šifra glumca za kojeg se dohvaćaju filmovi</param>  
        /// <response code="200">Lista filmova u kojima je glumac</response>
        /// <response code="204">Nema u bazi glumca za kojeg želimo dohvatiti filmove ili glumac ne glumi u tom filmu</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpGet]
        [Route("{sifra:int}/filmovi")]
        public IActionResult getFilmovi(int sifra)
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

                var glumac = _context.Glumac
                    .Include(g => g.Filmovi)
                    .FirstOrDefault(g => g.Sifra == sifra);

                if (glumac == null)
                {
                    return NoContent();
                }

                if (glumac.Filmovi == null || glumac.Filmovi.Count == 0)
                {
                    return NoContent();
                }

                List<FilmDTO> vrati = new();

               vrati = glumac.Filmovi.Select(f =>
                    new FilmDTO()
                    {
                        Sifra = f.Sifra,
                        Naziv = f.Naziv,
                        Godina = f.Godina,
                        Redatelj = f.Redatelj,
                        Zanr = f.Zanr,
                    }
                ).ToList();

                return Ok(vrati);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                    ex.Message);
            }
        }

        /// <summary> Dohvaćanje glumca po šifri </summary>
        /// <remarks>**Primjer upita:** ```GET /api/v1/Glumac/1```</remarks>
        /// <param name="sifra">Šifra glumca za kojeg želimo dohvatiti detalje</param>
        /// <response code="200">Objekt GlumacDetaljiDTO</response>
        /// <response code="204">Glumac sa traženom šifrom ne postoji</response>
        /// <response code="400">Zahtjev nije ispravan (BadRequest)</response>
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpGet]
        [Route("{sifra:int}")]
        public IActionResult getGlumacPoSifri(int sifra) {

            if (!ModelState.IsValid) {
                return BadRequest();
            }

            if (sifra <= 0) {
                return BadRequest();
            }

            try
            {
                var glumac = _context.Glumac
                    .Include(g => g.Filmovi)
                    .FirstOrDefault(g => g.Sifra == sifra);

                if (glumac == null)
                {
                    return NoContent();
                }

                List<FilmDTO> privFilmovi = new();

                foreach(Film f in glumac.Filmovi) {
                    privFilmovi.Add(new FilmDTO()
                    {
                        Sifra = f.Sifra,
                        Naziv = f.Naziv,
                        Godina = f.Godina,
                        Redatelj = f.Redatelj,
                        Zanr = f.Zanr
                    });
                }

                GlumacDetaljiDTO vrati = new GlumacDetaljiDTO()
                {
                    Sifra = glumac.Sifra,
                    Ime = glumac.Ime,
                    Prezime = glumac.Prezime,
                    Drzavljanstvo = glumac.Drzavljanstvo,
                    Filmovi = privFilmovi,
                };

                return Ok(vrati);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                   ex.Message);
            }

        }
    }
}
