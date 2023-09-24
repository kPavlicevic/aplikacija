using FilmRecenzijaApp.Data;
using FilmRecenzijaApp.Models.DTO;
using FilmRecenzijaApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmRecenzijaApp.Controllers
{
    /// <summary>
    /// Namijenjeno za CRUD operacije na entitetom ocjena u bazi
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OcjenaController : ControllerBase
    {
        private readonly FilmRecenzijaContext _context;
        public OcjenaController(FilmRecenzijaContext context)
        {
            _context = context;
        }


        /// <summary> Dohvaćanje svih ocjena filma</summary>
        /// <remarks> **Primjer upita:** ``` GET api/v1/film/1/ocjene ``` </remarks>
        /// <param name="sifra">Šifra filma za kojeg se dohvaćaju ocjene</param>  
        /// <response code="200">Lista ocjena ostavljenih na traženi film</response>
        /// <response code="204">Nema u bazi filma za kojeg želimo dohvatiti ocjene ili film nije ocjenjen</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpGet]
        [Route("{sifra:int}/ocjene")]
        public IActionResult GetOcjene(int sifra)
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
                    .Include(f => f.Ocjene)
                    .ThenInclude(o => o.Korisnik)
                    .FirstOrDefault(f => f.Sifra == sifra);

                if (film == null)
                {
                    return NoContent();
                }

                if (film.Ocjene == null || film.Ocjene.Count == 0)
                {
                    return NoContent();
                }

                List<OcjenaDTO> vrati = new();

                film.Ocjene.ForEach(o =>
                {
                    vrati.Add(new OcjenaDTO()
                    {
                        Sifra = o.Sifra,
                        Korisnik = o.Korisnik.KorisnickoIme,
                        Vrijednost = o.Vrijednost,
                        Film = o.Film.Naziv,
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

        /// <summary >Dodaje ocjenu na film </summary>
        /// <remarks>
        /// **Primjer upita:**
        ///```
        ///    POST api/v1/Film/1/dodajOcjenu
        ///    {
        ///     "korisnik" : "dominik96",
        ///     "vrijednost": 5
        ///    }
        ///```
        /// </remarks>
        /// <param name="sifra">šifra filma koji se ocjenjuje</param>
        /// <param name="ocjenaDTO">Korisničko ime i ocjena filma</param>
        /// <response code="200">Ocjena uspješno dodana</response>
        /// <response code="204">Film ne postoji ili je korisnik već ocijenio ovaj film</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpPost]
        [Route("{sifra:int}/dodajOcjenu")]
        public IActionResult DodajOcjenu(int sifra, OcjenaDTO ocjenaDTO)
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
                    .Include(f => f.Ocjene)
                    .FirstOrDefault(f => f.Sifra == sifra);

                var korisnik = _context.Korisnik
                    .FirstOrDefault(korisnik => korisnik.KorisnickoIme == ocjenaDTO.Korisnik);

                if (film == null || korisnik == null)
                {
                    return NoContent();
                }


                //TODO: ako je korisnik već ostavio ocjenu vratiti NoContent
                //jer ne želimo da korisnik može ostaviti više ocjena kako bi sabotirao
                //prosječnu ocjenu fillma
                Ocjena novaOcjena = new Ocjena()
                {
                    Film = film,
                    Korisnik = korisnik,
                    Vrijednost = ocjenaDTO.Vrijednost,
                };

                _context.Ocjena.Update(novaOcjena);
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



        /// <summary>Briše ocjenu s filma </summary>
        /// <remarks> **Primjer upita:** ``` DELETE api/v1/film/1/obrisiOcjenu/1 ``` </remarks>
        /// <param name="ocjenaSifra">Šifra ocjene koja se želi ukloniti</param>  
        /// <response code="200">Ocjena uspješno uklonjena</response>
        /// <response code="204">Film sa šifrom ne postoji u bazi ili ocjena sa šifromOcjena nije na tom filmu</response> 
        /// <response code="400">Zahtjev nije isprava (BadRequest)</response>
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpDelete]
        [Route("/obrisiOcjenu")]
        public IActionResult DeleteOcjena(int ocjenaSifra)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (ocjenaSifra <= 0)
            {
                return BadRequest();
            }

            try
            {

                var ocjena = _context.Ocjena.Find(ocjenaSifra);

                if (ocjena == null)
                {
                    return NoContent();
                }

                _context.Ocjena.Remove(ocjena);
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

        //TODO: omogućiti izmjenu prijašnje ostavljene ocjene
        //hint: PUT request; ulaz: sifra ocjene, korisničko ime;
        //PAZI!!! samo korisnik koji je prvotno ostavio ocjenu može ju izmjeniti

        //TODO: omogućiti dohvaćanje prosječne ocjene nekog filma
        //hint: Get request, ulaz: šifra filma, funkcija SUM
    }
}
