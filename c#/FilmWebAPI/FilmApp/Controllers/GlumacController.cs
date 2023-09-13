using FilmRecenzijaApp.Data;
using FilmRecenzijaApp.Models;
using FilmRecenzijaApp.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FilmRecenzijaApp.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GlumacController : ControllerBase
    {
        private readonly FilmRecenzijaContext _context;
        public GlumacController(FilmRecenzijaContext context)
        {
            _context = context;
        }

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
