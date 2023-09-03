using FilmApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmApp.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FilmController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get ()
        {
            var lista = new List<Film>()
            {
                new(){Naziv="Prvi"},
                new(){Naziv="Drugi"}
            };
            return new JsonResult(lista);
        }

        [HttpPost] 
        public IActionResult Post(Film film)
        {
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
