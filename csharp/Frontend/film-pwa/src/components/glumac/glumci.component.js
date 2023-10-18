import React, { Component } from "react";
import GlumacDataService from "../../services/glumac.service";
import { Button, Card, CardBody, Col, Container, Row } from "react-bootstrap";
import { Link } from "react-router-dom";
import { FaEdit } from "react-icons/fa";
import { FaTrash } from "react-icons/fa";
import { Modal } from "react-bootstrap";

import noimage from "../../images/no-image-found-360x250.png";
import http from "../../http-common";

export default class Glumci extends Component {
  constructor(props) {
    super(props);
    this.dohvatiGlumce = this.dohvatiGlumce.bind(this);

    this.state = {
      glumci: [],
      slike: [],
      prikaziModal: false,
      modal: {
        id: null,
        ime: "",
        prezime: "",
      },
    };
  }

  otvoriModal = (g) =>
    this.setState({
      prikaziModal: true,
      modal: { id: g.sifra, ime: g.ime, prezime: g.prezime },
    });
  zatvoriModal = () => this.setState({ prikaziModal: false });

  componentDidMount() {
    this.dohvatiGlumce();
    this.dohvatiSlikeGlumaca();
  }

  dohvatiGlumce() {
    GlumacDataService.getAll()
      .then((response) => {
        console.log(response.data);
        this.setState({
          glumci: response.data,
        });
      })
      .catch((e) => {
        console.log(e);
      });
  }

  dohvatiSlikeGlumaca() {
    GlumacDataService.getSlikeGlumaca()
      .then((response) => {
        this.setState({
          slike: response.data,
        });
      })
      .catch((e) => {
        console.log(e);
      });
  }

  nadjiSlikuGlumca = (sifra) => {
    const slika = this.state.slike.filter(
      (slika) => slika.sifraVeze === sifra
    )[0];
    if (slika) {
      return (
        <Card.Img
          src={"data:image/png;base64," + slika.bitovi}
          variant="top"
          className="imgKontejner"
          height={256}
        />
      );
    }
    return <Card.Img src={noimage} />;
  };

  async obrisiGlumac(sifra) {
    console.log("brišem glumca sa šifrom: ", sifra);
    const odgovor = await GlumacDataService.delete(sifra);
    if (odgovor.ok) {
      this.dohvatiGlumce();
    } else {
      console.log("dogodila se greška");
    }
    this.zatvoriModal();
  }

  // async dodajSliku() {
  //   const file = document.getElementById("file").files[0];
  //   const formData = new FormData();
  //   formData.append("file", file);
  //   formData.append("Vrsta", 2);
  //   formData.append("SifraVeze", 5);
  //   const config = {
  //     headers: {
  //       "Content-Type": "multipart/form-data",
  //     },
  //   };
  //   const odgovor = await http.post("/Slika", formData, config);
  //   if(odgovor.status === 200){
  //     alert("slika uspješno spremljena, osvježite");
  //   }
  // }

  render() {
    const { glumci } = this.state;
    const { modal } = this.state;
    return (
      <Container>
        <a href="/glumci/dodaj" className="btn btn-success gumb">
          Dodaj novog glumca
        </a>
        <Row>
          {glumci &&
            glumci.map((g) => (
              <Col key={g.sifra} sm={12} lg={4} md={6}>
                <Card style={{ width: "18rem" }}>
                  {this.nadjiSlikuGlumca(g.sifra)}
                  <Card.Body>
                    <Card.Title>
                      {g.ime} {g.prezime}
                    </Card.Title>
                    <Card.Text>{g.drzavljanstvo}</Card.Text>
                    <Row>
                      <Col>
                        <Link
                          className="btn btn-primary gumb"
                          to={`/glumci/${g.sifra}`}
                        >
                          <FaEdit />
                        </Link>
                      </Col>
                      <Col>
                        <Button
                          variant="danger"
                          className="gumb"
                          onClick={() => this.otvoriModal(g)}
                        >
                          <FaTrash />
                        </Button>
                      </Col>
                    </Row>
                  </Card.Body>
                </Card>
              </Col>
            ))}
        </Row>

        <input type="file" id="file" />
        {/* <Button onClick={this.dodajSliku}>Spremit sliku</Button> */}

        <Modal show={this.state.prikaziModal} onHide={this.zatvoriModal}>
          <Modal.Header closeButton>
            <Modal.Title>
              {modal.ime} {modal.prezime} - brisanje
            </Modal.Title>
          </Modal.Header>
          <Modal.Body>
            Brisanjem glumca, on će se također ukloniti sa svih filmova. Jeste
            li sigurni da želite obrisati glumca?
          </Modal.Body>
          <Modal.Footer>
            <Button
              variant="danger"
              onClick={() => this.obrisiGlumac(modal.id)}
            >
              Obriši
            </Button>
            <Button variant="secondary" onClick={this.zatvoriModal}>
              Zatvori
            </Button>
          </Modal.Footer>
        </Modal>
      </Container>
    );
  }
}
