using FilmRecenzijaApp.Data;
using FilmRecenzijaApp.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FilmRecenzijaApp.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GlumacController : ControllerBase
    {
        private readonly FilmRecenzijaContext _context;
        public GlumacController(FilmRecenzijaContext context )
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

            var glumci = _context.Glumac.ToList();
            if(glumci == null || glumci.Count == 0)
            {
                return new EmptyResult();
            }

            List<GlumacDTO> vrati = new();

            glumci.ForEach(g =>
            {
                var gdto = new GlumacDTO() {
                Sifra = g.Sifra,
                Ime = g.Ime,
                Prezime = g.Prezime,
                Drzavljanstvo = g.Drzavljanstvo
                };

                vrati.Add(gdto);

            });

            return Ok(vrati);
        }
    }
}
