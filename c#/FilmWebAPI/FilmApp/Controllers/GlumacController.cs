using FilmRecenzijaApp.Data;
using FilmRecenzijaApp.Models;
using FilmRecenzijaApp.Models.DTO;
using Microsoft.AspNetCore.Mvc;

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


        /// <summary>
        /// Dohvaća sve glumce iz baze
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    GET api/v1/Glumac
        ///
        /// </remarks>
        /// <returns>Glumci u bazi</returns>
        /// <response code="200">Sve je u redu</response>
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
                return new EmptyResult();
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


        /// <summary>
        /// Dodaje glumca u bazu
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    POST api/v1/Glumac
        ///    {Ime:"", Prezime:""}
        ///
        /// </remarks>
        /// <returns>Kreirani glumac u bazi sa svim podacima</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
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


        /// <summary>
        /// Mijenja podatke postojećeg glumca u bazi
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    PUT api/v1/glumac/1
        ///
        /// {
        ///  "sifra": 0,
        ///  "ime": "string",
        ///  "prezime": "string",
        ///  "drzavljanstvo": "string"
        /// }
        ///
        /// </remarks>
        /// <param name="sifra">Šifra glumca koji se mijenja</param>  
        /// <returns>Svi poslani podaci od glumca</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="204">Nema u bazi glumca kojeg želimo promijeniti</response>
        /// <response code="415">Nismo poslali JSON</response> 
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
                    return BadRequest();
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


        /// <summary>
        /// Briše glumca iz baze
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    DELETE api/v1/glumac/1
        ///    
        /// </remarks>
        /// <param name="sifra">Šifra glumca koji se briše</param>  
        /// <returns>Odgovor da li je obrisano ili ne</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="204">Nema u bazi glumca kojeg želimo obrisati</response>
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

            var glumacBaza = _context.Glumac.Find(sifra);
            if (glumacBaza == null)
            {
                return BadRequest();
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
    }
}
