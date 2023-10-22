using FilmRecenzijaApp.Data;
using FilmRecenzijaApp.Models.DTO;
using FilmRecenzijaApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace FilmRecenzijaApp.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SlikaController : ControllerBase {
        private readonly FilmRecenzijaContext _context;
        private readonly ILogger<SlikaController> _logger;  

        public SlikaController(FilmRecenzijaContext context, ILogger<SlikaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get(int sifra, int vrsta) {
            
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            try
            {
                
                var slika = _context.Slika
                    .FirstOrDefault(s => s.SifraVeze == sifra && s.Vrsta == vrsta);

                if (slika == null) {
                    return BadRequest("Slika ne postoji");
                }

                SlikaDTO vrati = new SlikaDTO
                {
                    Sifra = slika.Sifra,
                    Vrsta = slika.Vrsta,
                    SifraVeze = slika.SifraVeze,
                    Bitovi = slika.Bitovi
                };

                return Ok(vrati);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        [HttpGet]
        [Route("glumci")]
        public IActionResult GetSlikeGlumaca() {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try
            {
                var slikeGlumaca = _context.Slika.
                    Where(s => s.Vrsta == 2).ToList();


                if (slikeGlumaca == null) {
                    return NoContent();
                }

                List<SlikaDTO> vrati= new();

                slikeGlumaca.ForEach(s =>
                {
                      var sdto = new SlikaDTO()
                      {
                          Sifra = s.Sifra,
                          Vrsta = s.Vrsta,
                          SifraVeze = s.SifraVeze,
                          Bitovi = s.Bitovi
                      };
                      vrati.Add(sdto);
                });

                return Ok(vrati);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        [HttpPost]
        public  IActionResult Post([FromForm] IFormFile file, [FromForm] int Vrsta, [FromForm] int SifraVeze) {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var slika = _context.Slika
                   .FirstOrDefault(s => s.SifraVeze == SifraVeze && s.Vrsta == Vrsta);

                using (var memoryStream = new MemoryStream())
                { 
                    file.CopyTo(memoryStream);
                    byte[] bitovi = memoryStream.ToArray();
                    Slika s = new Slika() {
                        Vrsta = Vrsta,
                        SifraVeze = SifraVeze,
                        Bitovi = bitovi,
                    };
                    if (slika == null)
                    {
                        _logger.LogInformation("dodajem");
                        _context.Slika.Add(s);
                    }
                    else {
                        slika.Bitovi = bitovi;
                        _logger.LogInformation("updateam");
                        _context.Slika.Update(slika);
                    }
                        _context.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }
    }
}
