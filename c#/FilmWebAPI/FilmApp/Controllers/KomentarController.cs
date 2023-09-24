using FilmRecenzijaApp.Data;
using FilmRecenzijaApp.Models.DTO;
using FilmRecenzijaApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FilmRecenzijaApp.Controllers
{
    /// <summary>
    /// Namijenjeno za CRUD operacije na entitetom komentar u bazi
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class KomentarController : ControllerBase
    {
        private readonly FilmRecenzijaContext _context;
        public KomentarController(FilmRecenzijaContext context)
        {
            _context = context;
        }


        /// <summary> Dohvaćanje svih komentara filma</summary>
        /// <remarks> **Primjer upita:** ```GET api/v1/film/1/komentari``` </remarks>
        /// <param name="sifra">Šifra filma za kojeg se dohvaćaju komentari</param>  
        /// <response code="200">Lista komentara ostavljenih na film</response>
        /// <response code="204">Nema u bazi filma za kojeg želimo dohvatiti komentare ili film nema komentara</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpGet]
        [Route("{sifra:int}/komentari")]
        public IActionResult GetKomentari(int sifra)
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
                    .Include(f => f.Komentari)
                    .ThenInclude(k => k.Korisnik)
                    .FirstOrDefault(f => f.Sifra == sifra);

                if (film == null)
                {
                    return NoContent();
                }

                if (film.Komentari == null || film.Komentari.Count == 0)
                {
                    return NoContent();
                }

                List<KomentarDTO> vrati = new();

                film.Komentari.ForEach(k =>
                {
                    vrati.Add(new KomentarDTO()
                    {
                        Sifra = k.Sifra,
                        Korisnik = k.Korisnik.KorisnickoIme,
                        Sadrzaj = k.Sadrzaj,
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


        /// <summary> Dodaje komentar na film </summary>
        /// <remarks>
        /// **Primjer upita:**
        ///```
        ///POST api/v1/Film/1/dodajKomentar
        ///{
        /// "korisnik" : 1,
        /// "sadržaj" : "proizvoljni tekst komentara"
        /// }
        /// ```
        /// </remarks>
        /// <param name="sifra"> Šifra filma koji se želi komentirati</param>
        ///  <param name="komentarDTO"> Objekt sa korisničkim imenom i sadržajem komentara</param>
        /// <response code="200">Komentar usješno dodan</response>
        /// <response code="204">Film ili korisnik ne postoji</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpPost]
        [Route("{sifra:int}/dodajKomentar")]
        public IActionResult DodajKomentar(int sifra, KomentarDTO komentarDTO)
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
                    .Include(f => f.Komentari)
                    .FirstOrDefault(f => f.Sifra == sifra);

                var korisnik = _context.Korisnik
                    .FirstOrDefault(korisnik => korisnik.KorisnickoIme == komentarDTO.Korisnik);

                if (film == null || korisnik == null)
                {
                    return NoContent();
                }

                Komentar noviKomentar = new Komentar()
                {
                    Film = film,
                    Korisnik = korisnik,
                    Sadrzaj = komentarDTO.Sadrzaj,
                };

                _context.Komentar.Update(noviKomentar);
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

        /// <summary> Briše komentar s filma </summary>
        /// <remarks> **Primjer upita:** ``` DELETE api/v1/film/1/obrisiKomentar/1 ``` </remarks>
        /// <param name="komentarSifra">Šifra komentara koji se želi obrisati</param>  
        /// <response code="200">Komentar uspješno obrisan</response>
        /// <response code="204">Komentar sa predanom šifrom ne postoji</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpDelete]
        [Route("/obrisiKomentar")]
        public IActionResult DeleteKomentar(int komentarSifra)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (komentarSifra <= 0)
            {
                return BadRequest();
            }

            try
            {

                var komentar = _context.Komentar.Find(komentarSifra);

                if (komentar == null)
                {
                    return NoContent();
                }

                _context.Komentar.Remove(komentar);
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

        //TODO: omogućiti izmjenu postojećeg komentara
        //hint: PUT request
        //PAZI !!! samo korisnik koji je kreirao komentar može ga ažurirati
    }
}
