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
        private readonly ILogger<OcjenaController> _logger;
        public OcjenaController(FilmRecenzijaContext context, ILogger<OcjenaController> logger)
        {
            _context = context;
            _logger = logger;

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
        ///    POST api/v1/Ocjena/1/dodajOcjenu
        ///    {
        ///     "korisnik" : "dominik96",
        ///     "vrijednost": 5
        ///    }
        ///```
        /// </remarks>
        /// <param name="sifra">šifra filma koji se ocjenjuje</param>
        /// <param name="ocjenaDTO">Korisničko ime i ocjena filma</param>
        /// <response code="200">Ocjena uspješno dodana</response>
        /// <response code="400">
        ///     Zahtjev nije valjan (BadRequest) 
        ///     Film ne postoji, ili korisnik nije prijavljen, ili je korisnik već ocijenio film
        /// </response> 
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
                    .ThenInclude(o => o.Korisnik)
                    .FirstOrDefault(f => f.Sifra == sifra);

                var korisnik = _context.Korisnik
                    .FirstOrDefault(korisnik => korisnik.KorisnickoIme == ocjenaDTO.Korisnik);

                if (film == null || korisnik == null)
                {
                    return BadRequest("Film ne postoji ili niste prijavljeni!");
                }

                _logger.LogInformation("{}", korisnik.KorisnickoIme);

                foreach (Ocjena o in film.Ocjene) {
                    _logger.LogInformation("ocjena - {}", o.Korisnik.KorisnickoIme);
                    if (o.Korisnik == korisnik) {
                        return BadRequest("Film možete ocijeniti samo jednom!");
                    }
                }

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
        /// <remarks> 
        /// **Primjer upita:** 
        /// ``` 
        /// DELETE api/v1/Ocjena/obrisi
        /// ``` 
        /// </remarks>
        /// <param name="sifraFilma">Šifra filma sa kojeg se želi ukloniti ocjena</param>
        /// <param name="korisnickoIme">Korisnik koji želi ukloniti ocjenu sa nekog filma kojeg je ocijenio</param>
        /// <response code="200">Ocjena uspješno uklonjena</response>
        /// <response code="400"> Korisnik još nije ocijenio film ili nije prijavljen</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpDelete]
        [Route("obrisi")]
        public IActionResult DeleteOcjena(int sifraFilma, string korisnickoIme)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (sifraFilma <= 0)
            {
                return BadRequest();
            }

            try
            {

                var ocjena = _context.Ocjena
                    .Include(o => o.Film)
                    .Include(o => o.Korisnik)
                    .FirstOrDefault(o => o.Film.Sifra == sifraFilma && 
                    o.Korisnik.KorisnickoIme == korisnickoIme);

                if (ocjena == null)
                {
                    return BadRequest("Niste još ocijenili ovaj film ili niste prijavljeni!");
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


        /// <summary>Izmjeni vrijednost ocjene</summary>
        /// <remarks>
        /// **Primjer upita:** 
        /// ```
        /// {
        ///     "sifra":1,
        ///     "korisnik":"dominik96",
        ///     "vrijednost": 4.7
        /// }
        /// ```
        /// </remarks>
        /// <param name="sifra">Sifra ocjene koji se želi izmjeniti</param>
        /// <param name="ocjenaDTO">Ocjena uspješno izmjenjena</param>
        /// <response code="200">Izmjenjena ocjena</response>
        /// <response code="204">Ne postoji ocjena s traženom šifrom</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response>
        /// <response code="401">Nemate pravo na ovu radnju</response>
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpPut]
        [Route("{sifra:int}")]
        public IActionResult Put(int sifra, OcjenaDTO ocjenaDTO)
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
                var ocjena = _context.Ocjena
                    .Include(ocjena => ocjena.Korisnik)
                    .FirstOrDefault(ocjena => ocjena.Sifra == sifra);


                if (ocjena.Korisnik.KorisnickoIme == ocjenaDTO.Korisnik)
                {
                    ocjena.Vrijednost = ocjenaDTO.Vrijednost;
                    _context.Ocjena.Update(ocjena);
                    _context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return Unauthorized("Samo korsinik koji je ostavio ocjenu je može izmjeniti");
                }


            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status503ServiceUnavailable,
                    ex.Message
                );
            }
        }


        //TODO: omogućiti izmjenu prijašnje ostavljene ocjene
        //hint: PUT request; ulaz: sifra ocjene, korisničko ime;
        //PAZI!!! samo korisnik koji je prvotno ostavio ocjenu može ju izmjeniti

        /// <summary> Dohvaćanje prosječne ocjena filma</summary>
        /// <remarks> **Primjer upita:** ``` GET api/v1/Ocjena/1/ocjeneProsjek ``` </remarks>
        /// <param name="sifra">Šifra filma za kojeg se dohvaća prosječna ocjena</param>  
        /// <response code="200">Prosječna ocjena filma</response>
        /// <response code="204">Nema u bazi filma za kojeg želimo dohvatiti ocjene</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpGet]
        [Route("{sifra:int}/ocjeneProsjek")]
        public IActionResult GetOcjeneProsjek(int sifra)
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
                    return Ok(0);
                }

                decimal sum = 0;

                film.Ocjene.ForEach(o =>
                {
                    sum += o.Vrijednost;
                });

                return Ok(sum/film.Ocjene.Count);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                    ex.Message);
            }
        }
    }
}
