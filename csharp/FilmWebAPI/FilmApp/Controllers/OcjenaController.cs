﻿using FilmRecenzijaApp.Data;
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

                foreach (Ocjena o in film.Ocjene) {
                    if (o.Korisnik == korisnik) {
                        return NoContent();
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
        /// <remarks> **Primjer upita:** ``` GET api/v1/film/1/ocjeneProsjek ``` </remarks>
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