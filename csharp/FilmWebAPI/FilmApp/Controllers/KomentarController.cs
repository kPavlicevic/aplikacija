using FilmRecenzijaApp.Data;
using FilmRecenzijaApp.Models.DTO;
using FilmRecenzijaApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<KomentarController> _logger;
        public KomentarController(FilmRecenzijaContext context, ILogger<KomentarController> logger)
        {
            _context = context;
            _logger = logger;
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

                film.Komentari.ForEach(kom =>
                {
                    vrati.Add(new KomentarDTO()
                    {
                        Sifra = kom.Sifra,
                        Korisnik = kom.Korisnik.KorisnickoIme,
                        Sadrzaj = kom.Sadrzaj,
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
        ///POST api/v1/Komentar/1/dodajKomentar
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
                    return BadRequest("Film ne postoji ili niste prijavljeni");
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
        /// <remarks> **Primjer upita:** ``` DELETE api/v1/Komentar/1/obrisiKomentar ``` </remarks>
        /// <param name="komentarSifra">Šifra komentara koji se želi obrisati</param>  
        /// <response code="200">Komentar uspješno obrisan</response>
        /// <response code="204">Komentar sa predanom šifrom ne postoji</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpDelete]
        [Route("{komentarSifra:int}/obrisiKomentar")]
        public IActionResult DeleteKomentar(int komentarSifra, string korisnickoIme)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (komentarSifra <= 0)
            {
                return BadRequest("Sifra je obvezna");
            }

            try
            {

                var komentar = _context.Komentar
                    .Include(komentar => komentar.Korisnik)
                    .FirstOrDefault(komentar => komentar.Sifra == komentarSifra);
                if (komentar == null)
                {
                    return BadRequest("Komentar pod ovom šifrom ne postoji.");
                }else if(komentar.Korisnik.KorisnickoIme != korisnickoIme) {
                    return BadRequest("Samo korisnik koji je kreirao komentar može ga obrisati.");
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

        /// <summary>Izmjeni sadržaj komentara</summary>
        /// <remarks>
        /// **Primjer upita:** 
        /// ```
        /// {
        ///     "sifra":1,
        ///     "korisnik":"dominik96",
        ///     "sadrzaj":"proizvoljni tekst"
        /// }
        /// ```
        /// </remarks>
        /// <param name="sifra">Sifra komentara koji se želi izmjeniti</param>
        /// <param name="komentarDTO">Komentar uspješno izmjenjen</param>
        /// <response code="200">Izmjenjeni komentar</response>
        /// <response code="204">Ne postoji komentar s traženom šifrom</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response>
        /// <response code="401">Nemate pravo na ovu radnju</response>
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpPut]
        [Route("{sifra:int}")]
        public IActionResult Put(int sifra, KomentarDTO komentarDTO)
        {

            if (!ModelState.IsValid) {
                return BadRequest();
            }

            if (sifra <= 0)
            {
                return BadRequest("Komentar pod ovom šifrom ne postoji");
            }

            try
            {
                var komentar = _context.Komentar
                    .Include(komentar => komentar.Korisnik)
                    .FirstOrDefault(komentar => komentar.Sifra == sifra);


                if (komentar.Korisnik.KorisnickoIme == komentarDTO.Korisnik) {
                    komentar.Sadrzaj = komentarDTO.Sadrzaj;
                    _context.Komentar.Update(komentar);
                    _context.SaveChanges();
                    return Ok();
                }
                else {
                    return BadRequest("Samo korsinik koji je kreirao komentar ga može izmjeniti");
                }


            }
            catch (Exception ex) {
                return StatusCode(
                    StatusCodes.Status503ServiceUnavailable,
                    ex.Message
                );
            }
        }

    }
}
