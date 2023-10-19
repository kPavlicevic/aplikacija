import React, { Component } from "react";
import { Container, Row } from "react-bootstrap";
import { Link } from "react-router-dom";

export default class Pocetna extends Component {

  render() {
    return (
      <Container className="center_vertical">
          <p>
            Dobrodošli na vrlo kreativno nazvanu aplikaciju recenzija na film
            app!
          </p>
          <p>
            Ova aplikacija služi za recenziranje i komentiranje fimova. Možete
            pregledati, uređivati i brisati filmove, te vidjeti ocjene ili
            komentare kojim su ostali korisnici ostavili. <br /> Isto tako
            možete pregledavati, dodavati, uređivati i brisati glumce. Ako pak
            želite sami ocjeniti ili komentari neki film, to možete učiniti
            nakon što se prijavite, ili ako nemate račun nakon što ga kreirate.
          </p>

          <p>
            Trenutno je svima dopušteno brisanje i uređivanje filmova, tako da
            to molimo radite svjesno te da ne brišete bez razloga filmove koje
            su neki drugi korisnici dodali. Ako se ove mogućnosti budu
            zloupotrebljavale, u kasnijim verzijama biti će onemogućene. Hvala
            na razumijevanju.
          </p>
          <div className="naslovna_linkovi">
          <Link to="/prijava" className="btn btn-primary"> 
            Prijava
          </Link>
          <Link to="/registracija" className="btn btn-primary">
            Registracija
          </Link>
          </div>
      </Container>
    );
  }
}
