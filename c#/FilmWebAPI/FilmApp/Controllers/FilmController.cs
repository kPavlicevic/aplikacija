using FilmRecenzijaApp.Data;
using FilmRecenzijaApp.Models;
using FilmRecenzijaApp.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FilmRecenzijaApp.Controllers
{
    /// <summary>
    /// Namijenjeno za CRUD operacije na entitetom film u bazi
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FilmController : ControllerBase
    {
        private readonly FilmRecenzijaContext _context;
        private readonly ILogger<FilmController> _logger;

        public FilmController(FilmRecenzijaContext context, ILogger<FilmController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Dohvaća sve filmove iz baze
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    GET api/v1/Film
        ///
        /// </remarks>
        /// <returns>Filmovi u bazi</returns>
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

            var filmovi = _context.Film.ToList();
            if (filmovi == null || filmovi.Count == 0)
            {
                return new EmptyResult();
            }


            List<FilmDTO> vrati = new();

            filmovi.ForEach(f =>
            {
                var fdto = new FilmDTO()
                {
                    Sifra = f.Sifra,
                    Naziv = f.Naziv,
                    Godina = f.Godina,
                    Redatelj = f.Redatelj,
                    Zanr = f.Zanr,
                };

                vrati.Add(fdto);

            });

            return Ok(vrati);

        }


        /// <summary>
        /// Dodaje film u bazu
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    POST api/v1/Film
        ///    {
        ///    "naziv": "string",
        ///    "godina": 0,
        ///    "redatelj": "string",
        ///    "zanr": "string",
        ///    }
        ///
        /// </remarks>
        /// <returns>Kreirani film u bazi sa svim podacima</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpPost]
        public IActionResult Post(FilmDTO filmdto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Film f = new Film()
                {
                    Naziv = filmdto.Naziv,
                    Godina = filmdto.Godina,
                    Redatelj = filmdto.Redatelj,
                    Zanr = filmdto.Zanr
                };

                _context.Film.Add(f);
                _context.SaveChanges();
                filmdto.Sifra = f.Sifra;
                return Ok(filmdto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }


        }


        /// <summary>
        /// Mijenja podatke postojećeg filma u bazi
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    PUT api/v1/film/1
        ///
        /// {
        ///  "sifra": 0,
        ///  "naziv": "string",
        ///  "godina": 0,
        ///  "redatelj": "string",
        ///  "zanr": "string"
        /// }
        ///
        /// </remarks>
        /// <param name="sifra">Šifra filma koji se mijenja</param>  
        /// <returns>Svi poslani podaci od filma</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="204">Nema u bazi filma kojeg želimo promijeniti</response>
        /// <response code="415">Nismo poslali JSON</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response> 
        [HttpPut]
        [Route("{sifra:int}")]
        public IActionResult Put(int sifra, FilmDTO fdto)
        {

            if (sifra <= 0 || fdto == null)
            {
                return BadRequest();
            }

            try
            {
                var filmBaza = _context.Film.Find(sifra);
                if (filmBaza == null)
                {
                    return BadRequest();
                }
                filmBaza.Naziv = fdto.Naziv;
                filmBaza.Godina = fdto.Godina;
                filmBaza.Redatelj = fdto.Redatelj;
                filmBaza.Zanr = fdto.Zanr;

                _context.Film.Update(filmBaza);
                _context.SaveChanges();
                fdto.Sifra = filmBaza.Sifra;

                return StatusCode(StatusCodes.Status200OK, fdto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex);
            }

        }


        /// <summary>
        /// Briše film iz baze
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    DELETE api/v1/film/1
        ///    
        /// </remarks>
        /// <param name="sifra">Šifra filma koji se briše</param>  
        /// <returns>Odgovor da li je obrisano ili ne</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="204">Nema u bazi filma kojeg želimo obrisati</response>
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

            var filmBaza = _context.Film.Find(sifra);
            if (filmBaza == null)
            {
                return BadRequest();
            }

            try
            {
                _context.Film.Remove(filmBaza);
                _context.SaveChanges();

                return new JsonResult("{\"poruka\":\"Obrisano\"}");
            }
            catch (Exception ex)
            {
                return new JsonResult("{\"poruka\":\"Ne može se obrisati\"}");
            }
        }

        /// <summary>
        /// Dohvaćanje svih glumaca filma
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    GET api/v1/film/1
        ///    
        /// </remarks>
        /// <param name="sifra">Šifra filma za kojeg se dohvaćaju glumci</param>  
        /// <returns> Sve glumce filma</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="204">Nema u bazi filma za kojeg želimo dohvatiti glumce ili film nema glumaca</response>
        /// <response code="415">Nismo poslali JSON</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpGet]
        [Route("{sifra:int}/glumci")]
        public IActionResult GetGlumci(int sifra)
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
                    .Include(f => f.Glumci)
                    .FirstOrDefault(f => f.Sifra == sifra);

                if (film == null)
                {
                    return BadRequest();
                }

                if (film.Glumci == null || film.Glumci.Count == 0)
                {
                    return new EmptyResult();
                }

                List<GlumacDTO> vrati = new();

                film.Glumci.ForEach(g =>
                {
                    vrati.Add(new GlumacDTO()
                    {
                        Sifra = g.Sifra,
                        Ime = g.Ime,
                        Prezime = g.Prezime,
                        Drzavljanstvo = g.Drzavljanstvo,
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

        /// <summary>
        /// Dodaje glumca na film
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    POST api/v1/Film/1/dodajGlumca
        ///    {
        ///    "sifra": 1,
        ///     "glumacSifra" : 3
        ///    }
        ///
        /// </remarks>
        /// <returns>Kreirani film u bazi sa svim podacima</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="204">Glumac već postoji na tom filmu</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpPost]
        [Route("{sifra:int}/dodajGlumca/{glumacSifra:int}")]
        public IActionResult PostGlumca(int sifra, int glumacSifra)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (sifra <= 0 || glumacSifra <= 0)
            {
                return BadRequest();
            }

            try
            {

                var film = _context.Film
                    .Include(f => f.Glumci)
                    .FirstOrDefault(f => f.Sifra == sifra);

                if (film == null)
                {
                    return BadRequest();
                }

                var glumac = _context.Glumac.Find(glumacSifra);

                if (glumac == null)
                {
                    return BadRequest();
                }

                // napraviti kontrolu da li je taj polaznik već u toj grupi
                if (!film.Glumci.Contains(glumac))
                {
                    film.Glumci.Add(glumac);
                }
                else
                {
                    return NoContent();
                }

                _context.Film.Update(film);
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

        /// <summary>
        /// Briše glumca s filma
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    DELETE api/v1/film/1/obrisiGlumca/1
        ///    
        /// </remarks>
        /// <param name="sifra">Šifra filma sa kojeg se briše glumac</param>  
        /// <returns>Odgovor da li je obrisano ili ne</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="400">Film sa šifrom ne postoji u bazi ili glumac sa šifromGlumac nije na tom filmu</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpDelete]
        [Route("{sifra:int}/obrisiGlumca/{glumacSifra:int}")]
        public IActionResult DeleteGlumca(int sifra, int glumacSifra)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (sifra <= 0 || glumacSifra <= 0)
            {
                return BadRequest();
            }

            try
            {

                var film = _context.Film
                    .Include(f => f.Glumci)
                    .FirstOrDefault(f => f.Sifra == sifra);

                if (film == null)
                {
                    return BadRequest("Film s predanom šifrom ne postoji");
                }

                var glumac = _context.Glumac.Find(glumacSifra);

                if (glumac == null)
                {
                    return BadRequest("Glumac s predanom šifrom ne postoji");
                }

                if (film.Glumci.Contains(glumac))
                {
                    film.Glumci.Remove(glumac);
                    _context.Film.Update(film);
                    _context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return BadRequest("Glumac s predanom šifrom se ne nalazi na ovom filmu");

                }
            }
            catch (Exception ex)
            {
                return StatusCode(
                       StatusCodes.Status503ServiceUnavailable,
                       ex.Message);

            }

        }

        /// <summary>
        /// Dohvaćanje svih komentara filma
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    GET api/v1/film/1/komentari
        ///    
        /// </remarks>
        /// <param name="sifra">Šifra filma za kojeg se dohvaćaju komentari</param>  
        /// <returns> Sve komentare koji su komentirani na filmu</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="204">Nema u bazi filma za kojeg želimo dohvatiti komentare ili film nema komentara</response>
        /// <response code="415">Nismo poslali JSON</response> 
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
                    .ThenInclude(k=>k.Korisnik)
                    .FirstOrDefault(f => f.Sifra == sifra);

                if (film == null)
                {
                    return BadRequest();
                }

                if (film.Komentari == null || film.Komentari.Count == 0)
                {
                    return new EmptyResult();
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


        /// <summary>
        /// Dodaje komentar na film
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    POST api/v1/Film/1/dodajKomentar
        ///    {
        ///    "sifra": 1,
        ///     "komentarSifra" : 3
        ///    }
        ///
        /// </remarks>
        /// <returns>Kreirani film u bazi s svim podacima</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="204">Komentar već postoji na tom filmu</response>
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
                    return BadRequest();
                }


                //TODO: dodati komentar na taj film, odnosno
                //insertatnt novi komentar i povezat ga na psotojeći film
                Komentar noviKomentar = new Komentar() {
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

        /// <summary>
        /// Briše komentar s filma
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    DELETE api/v1/film/1/obrisiKomentar/1
        ///    
        /// </remarks>
        /// <param name="sifra">Šifra filma sa kojeg se briše komentar</param>  
        /// <returns>Odgovor da li je obrisano ili ne</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="400">Film sa šifrom ne postoji u bazi ili komentar sa šifromKomentar nije na tom filmu</response> 
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
                    return BadRequest("Komentar s predanom šifrom ne postoji");
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

        /// <summary>
        /// Dohvaćanje svih ocjena filma
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    GET api/v1/film/1/ocjene
        ///    
        /// </remarks>
        /// <param name="sifra">Šifra filma za kojeg se dohvaćaju ocjene</param>  
        /// <returns> Sve ocjene koji su ocjenjeni na filmu</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="204">Nema u bazi filma za kojeg želimo dohvatiti ocjene ili film nije ocjenjen</response>
        /// <response code="415">Nismo poslali JSON</response> 
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
                    .ThenInclude(o=>o.Korisnik)
                    .FirstOrDefault(f => f.Sifra == sifra);

                if (film == null)
                {
                    return BadRequest();
                }

                if (film.Ocjene == null || film.Ocjene.Count == 0)
                {
                    return new EmptyResult();
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

        /// <summary>
        /// Dodaje ocjenu na film
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    POST api/v1/Film/1/dodajOcjenu
        ///    {
        ///    "sifra": 1,
        ///     "ocjenaSifra" : 3
        ///    }
        ///
        /// </remarks>
        /// <returns>Kreirani film u bazi sa svim podacima</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="204">Ocjena već postoji na tom filmu</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response>
        [HttpPost]
        [Route("{sifra:int}/dodajOcjenu/{ocjenaSifra:int}")]
        public IActionResult DodajOcjenu(int sifra, int ocjenaSifra)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (sifra <= 0 || ocjenaSifra <= 0)
            {
                return BadRequest();
            }

            try
            {

                var film = _context.Film
                    .Include(f => f.Ocjene)
                    .FirstOrDefault(f => f.Sifra == sifra);

                if (film == null)
                {
                    return BadRequest();
                }

                var ocjena = _context.Ocjena.Find(ocjenaSifra);

                if (ocjena == null)
                {
                    return BadRequest();
                }

                // napraviti kontrolu da li je taj polaznik već u toj grupi
                film.Ocjene.Add(ocjena);

                _context.Film.Update(film);
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
    }
}