using FilmRecenzijaApp.Data;
using FilmRecenzijaApp.Models;
using FilmRecenzijaApp.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FilmRecenzijaApp.Controllers
{

    /// <summary>
    /// Namijenjeno za CRUD operacije na entitetom korisnik u bazi
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class KorisnikController : ControllerBase
    {
        private readonly FilmRecenzijaContext _context;
        private readonly ILogger<KorisnikController> _logger;
        public KorisnikController(FilmRecenzijaContext context, ILogger<KorisnikController> logger)
        {
            _context = context;
            _logger = logger;
        }



        /// <summary> Dohvaća sve korisnike iz baze </summary>
        /// <remarks> **Primjer upita:** ```GET api/v1/Korisnik``` </remarks>
        /// <response code="200">Lista korisnika</response>
        /// <response code="204">Ne postoji niti jedan korisnik u bazi</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpGet]
        public IActionResult Get()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var korisnici = _context.Korisnik.ToList();
            if (korisnici == null || korisnici.Count == 0)
            {
                return NoContent();
            }

            List<KorisnikDTO> vrati = new();
           
            korisnici.ForEach(k =>
            {
                var kdto = new KorisnikDTO()
                {
                    Sifra = k.Sifra,
                    KorisnickoIme = k.KorisnickoIme,
                    //Lozinka = k.Lozinka
                };

                vrati.Add(kdto);

            });

            return Ok(vrati);
        }


        /// <summary> Registracija korisnika/dodavanje korisnika u bazu </summary>
        /// <remarks> 
        ///**Primjer upita:**
        ///```
        ///POST api/v1/Korisnik
        ///{
        /// Korisničko ime:"dominik96", 
        /// Lozinka:"hr4dt5"
        ///}
        ///```
        /// </remarks>
        /// <response code="200"> Korisnik koji je kreiran </response>
        /// <response code="400"> Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpPost]
        [Route("registracija")]
        public IActionResult Post(KorisnikDTO kdto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var privKorisnik = _context.Korisnik.
                    FirstOrDefault(kor => kor.KorisnickoIme == kdto.KorisnickoIme);

                if (privKorisnik == null)
                {
                    Korisnik k = new Korisnik()
                    {
                        KorisnickoIme = kdto.KorisnickoIme,
                        Lozinka = kdto.Lozinka
                    };

                    _context.Korisnik.Add(k);
                    _context.SaveChanges();
                    return Ok(k);

                }
                return BadRequest("Korisnik već postiji");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }


        /// <summary> Prijava korisnika u aplikaciju </summary>
        /// <remarks> 
        ///**Primjer upita:**
        ///```
        ///POST api/v1/Korisnik/Login
        ///{
        /// Korisničko ime:"dominik96", 
        /// Lozinka:"hr4dt5"
        ///}
        ///```
        /// </remarks>
        /// <response code="200"> Korisnik koji je kreiran </response>
        /// <response code="400"> Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpPost]
        [Route("prijava")]
        public IActionResult PostLogin(KorisnikDTO kdto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var privKorisnik = _context.Korisnik.
                    FirstOrDefault(kor => kor.KorisnickoIme == kdto.KorisnickoIme);

                if (privKorisnik == null)
                {
                    return BadRequest("Korisnim pod ovim korisničkim imenom ne postoji, registrirajte se.");

                }
                else
                {
                    if (privKorisnik.Lozinka == kdto.Lozinka)
                    {
                        return Ok(true);
                    }
                    return Ok(false);
                }
                

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }
    }
}
