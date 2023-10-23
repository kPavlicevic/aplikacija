import React, { Component } from "react";
import FilmDataService from "../../services/film.service";
import Container from "react-bootstrap/Container";
import Button from "react-bootstrap/Button";
import Form from "react-bootstrap/Form";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import { Link } from "react-router-dom";
import { ButtonGroup, Card, ListGroup, Modal, Table } from "react-bootstrap";
import noimage from "../../images/no-image-found-360x250.png";
import OcjenaService from "../../services/ocjena.service";
import KomentarService from "../../services/komentar.service";
import { FaEdit, FaMinus, FaTrash } from "react-icons/fa";
import PoveziGlumca from "./poveziGlumca.component";
import SlikeService from "../../services/slike.service";

export default class PromjeniFilm extends Component {
  constructor(props) {
    super(props);
    this.film = this.dohvatiFilm();
    this.prosjek = this.dohvatiProsjek();
    this.trenutnaSlika = this.dohvatiSlikuFilma();

    this.obrisiGlumca = this.obrisiGlumca.bind(this);

    this.handlePromjena = this.handlePromjena.bind(this);
    this.promjeniFilm = this.promjeniFilm.bind(this);

    this.handleOcijeni = this.handleOcijeni.bind(this);
    this.ocijeniFilm = this.ocijeniFilm.bind(this);
    this.handleObrisiOcjenu = this.handleObrisiOcjenu.bind(this);
    this.obrisiOcjenu = this.obrisiOcjenu.bind(this);

    this.handleKomentiraj = this.handleKomentiraj.bind(this);
    this.komentiraj = this.komentiraj.bind(this);

    this.handleObrisiKomentar = this.handleObrisiKomentar.bind(this);
    this.obrisiKomentar = this.obrisiKomentar.bind(this);

    this.otvoriUrediKomentar = this.otvoriUrediKomentar.bind(this);
    this.handleUrediKomentar = this.handleUrediKomentar.bind(this);

    this.zatvoriModal = this.zatvoriModal.bind(this);
    this.otvoriModal = this.otvoriModal.bind(this);

    this.spremiSliku = this.spremiSliku.bind(this);

    this.state = {
      film: {},
      glumci: [],
      modal: {
        uredi: false,
        ocijeni: false,
        komentiraj: false,
        urediKomentar: false,
      },
      prosjek: 0,
      trenutniKomentar: {},
      trenutnaSlika: "",
    };
  }

  async dohvatiFilm() {
    let href = window.location.href;
    let niz = href.split("/");
    await FilmDataService.getBySifra(niz[niz.length - 1])
      .then((response) => {
        this.setState({
          film: response.data,
        });
        console.log(response.data);
      })
      .catch((e) => {
        console.log(e);
      });
  }

  async dohvatiProsjek() {
    let href = window.location.href;
    let niz = href.split("/");
    let sifraFilma = niz[niz.length - 1];
    await OcjenaService.getProsjek(sifraFilma)
      .then((response) => {
        this.setState({
          prosjek: response.data,
        });
      })
      .catch((e) => {
        console.log(e);
      });
  }

  async dohvatiSlikuFilma() {
    let href = window.location.href;
    let niz = href.split("/");
    let sifraFilma = niz[niz.length - 1];
    const odgovor = await SlikeService.getSlikuFilmaPoSifri(sifraFilma);
    if (odgovor.ok) {
      this.setState({
        trenutnaSlika: "data:image/png;base64," + odgovor.slika,
      });
    } else {
      console.log(odgovor.poruka);
      this.setState({
        trenutnaSlika: noimage,
      });
    }
  }

  spremiSlikuAkcija=() => {
    const { film } = this.state;

    this.spremiSliku(film.sifra);
  }

  async spremiSliku(sifra) {
    const file = document.getElementById("file").files[0];
    const formData = new FormData();
    formData.append("file", file);
    formData.append("Vrsta", 1);
    formData.append("SifraVeze", sifra);
    const odgovor = await SlikeService.postaviSliku(formData);
    if (odgovor.ok) {
      //window.location.href='/polaznici';
      this.dohvatiSlikuFilma();
    } else {
      // pokaži grešku
      alert(odgovor.poruka);
      console.log(odgovor);
    }
  }

  async dohvatiGlumci() {
    let href = window.location.href;
    let niz = href.split("/");
    await FilmDataService.getGlumci(niz[niz.length - 1])
      .then((response) => {
        this.setState({
          glumci: response.data,
        });

        // console.log(response.data);
      })
      .catch((e) => {
        console.log(e);
      });
  }

  async obrisiGlumca(film, glumac) {
    console.log(film, glumac);
    const odgovor = await FilmDataService.obrisiGlumca(film, glumac);
    if (odgovor.ok) {
      alert(odgovor.poruka);
      this.dohvatiFilm();
    } else {
      alert(odgovor.poruka);
    }
  }

  async promjeniFilm(film) {
    // ovo mora bolje
    let href = window.location.href;
    let niz = href.split("/");
    const odgovor = await FilmDataService.put(niz[niz.length - 1], film);
    if (odgovor.ok) {
      // routing na smjerovi
      window.location.href = "/filmovi";
    } else {
      // pokaži grešku
      console.log(odgovor);
    }
  }

  handlePromjena(e) {
    // Prevent the browser from reloading the page
    e.preventDefault();

    // Read the form data
    const podaci = new FormData(e.target);

    this.promjeniFilm({
      naziv: podaci.get("naziv"),
      godina: parseInt(podaci.get("godina")),
      redatelj: podaci.get("redatelj"),
      zanr: podaci.get("zanr"),
    });
  }

  async ocijeniFilm(ocjena) {
    let href = window.location.href;
    let niz = href.split("/");
    let sifraFilma = niz[niz.length - 1];

    const odgovor = await OcjenaService.post(sifraFilma, ocjena);
    if (odgovor.ok) {
      alert("Ocjena uspješno spremljena!");
      this.zatvoriModal();
      this.dohvatiProsjek();
      //window.location.href = odgovor.prusmjeri;
    } else {
      alert(odgovor.error);
      this.zatvoriModal();
    }
  }

  handleOcijeni(e) {
    e.preventDefault();
    const podaci = new FormData(e.target);

    this.ocijeniFilm({
      Vrijednost: podaci.get("ocjena"),
      Korisnik: JSON.parse(localStorage.getItem("auth"))?.korisnickoIme,
    });
  }

  async obrisiOcjenu(sifraFilma, korisnickoIme) {
    const odgovor = await OcjenaService.delete(sifraFilma, korisnickoIme);
    if (odgovor.ok) {
      alert("Ocjena uspješno obrisana!");
      this.zatvoriModal();
      this.dohvatiProsjek();
    } else {
      alert(odgovor.error);
    }
  }

  handleObrisiOcjenu() {
    const sifraFilma = this.state.film.sifra;
    const korisnickoIme = JSON.parse(
      localStorage.getItem("auth")
    )?.korisnickoIme;

    if(korisnickoIme === "") {
      alert("Morate biti prijavljeni da biste mogli ocijeniti film!")
      return;
    }else {
      this.obrisiOcjenu(sifraFilma, korisnickoIme);
    }
  }

  async komentiraj(komentarDto) {
    const odgovor = await KomentarService.postKomentar(
      this.state.film.sifra,
      komentarDto
    );
    if (odgovor.ok) {
      alert("Komentar uspješno dodan!");
      this.zatvoriModal();
      this.dohvatiFilm();
    } else {
      alert(odgovor.error);
      this.zatvoriModal();
    }
  }

  handleKomentiraj(e) {
    e.preventDefault();
    const podaci = new FormData(e.target);
    console.log(podaci.get("komentar"));
    this.komentiraj({
      korisnik: JSON.parse(localStorage.getItem("auth"))?.korisnickoIme,
      sadrzaj: podaci.get("komentar"),
    });
  }

  async obrisiKomentar(komentarSifra, korisnickoIme) {
    const odgovor = await KomentarService.delete(komentarSifra, korisnickoIme);
    if (odgovor.ok) {
      alert("Komentar uspješno obrisan!");
      this.zatvoriModal();
      this.dohvatiFilm();
    } else {
      alert(odgovor.error);
    }
  }

  handleObrisiKomentar(komentarSifra) {
    const korisnickoIme = JSON.parse(
      localStorage.getItem("auth")
    )?.korisnickoIme;
    this.obrisiKomentar(komentarSifra, korisnickoIme);
  }

  otvoriUrediKomentar(komentar) {
    this.setState({
      modal: {
        urediKomentar: true,
      },
      trenutniKomentar: komentar,
    });
  }

  async urediKomentar(komentarDto) {
    const odgovor = await KomentarService.put(komentarDto);
    if (odgovor.ok) {
      this.zatvoriModal();
      alert("Komentar uspješno izmjenjen");
      this.dohvatiFilm();
    } else {
      alert(odgovor.error);
    }
  }

  handleUrediKomentar(e) {
    e.preventDefault();
    const podaci = new FormData(e.target);
    this.urediKomentar({
      Sifra: this.state.trenutniKomentar.sifra,
      Sadrzaj: podaci.get("komentar"),
      Korisnik: this.state.trenutniKomentar.korisnik,
    });
  }

  zatvoriModal() {
    this.setState({
      modal: {
        uredi: false,
        ocijeni: false,
        komentiraj: false,
        urediKomentar: false,
      },
      trenutniKomentar: {},
    });
  }

  otvoriModal(tipModala) {
    if (tipModala === "u") {
      this.setState({
        modal: {
          uredi: true,
        },
      });
    } else if (tipModala === "o") {
      this.setState({
        modal: {
          ocijeni: true,
        },
      });
    } else if (tipModala === "k") {
      this.setState({
        modal: {
          komentiraj: true,
        },
      });
    }
  }

  render() {
    const { film, modal, prosjek, trenutnaSlika, trenutniKomentar } = this.state;

    return (
      <>
        <Container>
          <Row>
            <Col md={6} lg={5}>
              <img
                src={trenutnaSlika}
                className="imgKontejner border"
                alt="slika filma"
                width={360}
                height={250}
              />
            </Col>
            <Col>
              <ul>
                <li>Naziv: {film.naziv}</li>
                <li>Godina: {film.godina}</li>
                <li>Redatlje: {film.redatelj}</li>
                <li>Žanr: {film.zanr}</li>
                <li>
                  <div className="kontejner">
                    Glumci: <PoveziGlumca film={film.sifra} />
                  </div>
                  {film.glumci &&
                    film.glumci.map((glumac) => (
                      <div key={glumac.sifra} className="glumciFilm">
                        <Button
                          variant="danger"
                          size="sm"
                          onClick={() =>
                            this.obrisiGlumca(film.sifra, glumac.sifra)
                          }
                        >
                          <FaMinus />
                        </Button>
                        <Link to={"/glumci/" + glumac.sifra}>
                          {glumac.ime} {glumac.prezime}
                        </Link>
                      </div>
                    ))}
                </li>
                <li>Prosječna ocjena: {Math.round(prosjek * 10) / 10}</li>
              </ul>
            </Col>
          </Row>

          <ButtonGroup size="lg" className="gumb_grupa">
            <Button onClick={() => this.otvoriModal("u")}>Uredi</Button>
            <Button onClick={() => this.otvoriModal("o")}>Ocijeni</Button>
            <Button onClick={() => this.otvoriModal("k")}>Komentiraj</Button>
          </ButtonGroup>

          <div className="komentari">
            <Card>
              <ListGroup variant="flush">
                {film.komentari &&
                  film.komentari.map((komentar) => (
                    <ListGroup.Item key={komentar.sifra}>
                      <Card.Header className="komentar_glava">
                        {komentar.korisnik}
                        {komentar.korisnik ===
                          JSON.parse(localStorage.getItem("auth"))
                            ?.korisnickoIme && (
                          <div>
                            <Button
                              onClick={() => this.otvoriUrediKomentar(komentar)}
                            >
                              <FaEdit />
                            </Button>
                            <Button
                              variant="danger"
                              onClick={() =>
                                this.handleObrisiKomentar(komentar.sifra)
                              }
                            >
                              <FaTrash />
                            </Button>
                          </div>
                        )}
                      </Card.Header>
                      <Card.Body>
                        <blockquote className="blockquote mb-0">
                          {komentar.sadrzaj}
                        </blockquote>
                      </Card.Body>
                    </ListGroup.Item>
                  ))}
              </ListGroup>
            </Card>
          </div>
        </Container>
        {/* Modal promjeni */}

        <Modal show={modal.uredi}>
          <Modal.Body>
            <Container>
              <Form onSubmit={this.handlePromjena}>
                <Form.Group className="mb-3" controlId="naziv">
                  <Form.Label>Naziv</Form.Label>
                  <Form.Control
                    type="text"
                    name="naziv"
                    placeholder="Naziv filma"
                    maxLength={50}
                    defaultValue={film.naziv}
                    required
                  />
                </Form.Group>

                <Form.Group className="mb-3" controlId="godina">
                  <Form.Label>Godina</Form.Label>
                  <Form.Control
                    type="number"
                    min="1890"
                    max="2024"
                    name="godina"
                    defaultValue={film.godina}
                    placeholder="Godina"
                  />
                </Form.Group>

                <Form.Group className="mb-3" controlId="redatelj">
                  <Form.Label>Redatelj</Form.Label>
                  <Form.Control
                    type="text"
                    name="redatelj"
                    placeholder="Redatelj"
                    maxLength={50}
                    defaultValue={film.redatelj}
                  />
                </Form.Group>

                <Form.Group className="mb-3" controlId="zanr">
                  <Form.Label>Žanr</Form.Label>
                  <Form.Control
                    type="text"
                    name="zanr"
                    placeholder="Zanr"
                    maxLength={50}
                    defaultValue={film.zanr}
                  />
                </Form.Group>
                <Row style={{ margin: "10px", gap: "10px" }}>
                  <input type="file" id="file" onChange={this.onChange} />

                  <input
                    type="button"
                    onClick={this.spremiSlikuAkcija}
                    value={"Spemi sliku"}
                  />
                </Row>
                <Row>
                  <Col>
                    <Button
                      className="btn btn-danger gumb"
                      onClick={this.zatvoriModal}
                    >
                      Odustani
                    </Button>
                  </Col>
                  <Col>
                    <Button variant="primary" className="gumb" type="submit">
                      Promjeni film
                    </Button>
                  </Col>
                </Row>
              </Form>
            </Container>
          </Modal.Body>
        </Modal>

        {/*Modal ocijeni*/}

        <Modal show={modal.ocijeni}>
          <Modal.Header closeButton onHide={this.zatvoriModal}></Modal.Header>
          <Modal.Body>
            <Container>
              <Form onSubmit={this.handleOcijeni}>
                <Form.Group className="mb-3" controlId="ocjena">
                  <Form.Label>Ocjena</Form.Label>
                  <Form.Control
                    type="number"
                    name="ocjena"
                    placeholder="1-5"
                    max={5}
                    min={1}
                    step={0.5}
                    required
                  />
                </Form.Group>

                <Row>
                  <Col>
                    <Button
                      className="btn btn-danger gumb"
                      onClick={this.handleObrisiOcjenu}
                    >
                      Obriši ako ste već ocijenili
                    </Button>
                  </Col>
                  <Col>
                    <Button variant="primary" className="gumb" type="submit">
                      Ocijeni
                    </Button>
                  </Col>
                </Row>
              </Form>
            </Container>
          </Modal.Body>
        </Modal>

        {/*Modal komentiraj*/}
        <Modal show={modal.komentiraj}>
          <Modal.Body>
            <Form onSubmit={this.handleKomentiraj}>
              <Form.Group className="mb-3" controlId="komentar">
                <Form.Label>Komentar</Form.Label>
                <Form.Control
                  type="text"
                  as="textarea"
                  name="komentar"
                  placeholder="Komentiraj"
                  maxLength={250}
                  required
                />
              </Form.Group>
              <Row>
                <Col>
                  <Button
                    className="btn btn-danger gumb"
                    onClick={this.zatvoriModal}
                  >
                    Odustani
                  </Button>
                </Col>
                <Col>
                  <Button variant="primary" className="gumb" type="submit">
                    Komentiraj
                  </Button>
                </Col>
              </Row>
            </Form>
          </Modal.Body>
        </Modal>

        {/*Modal komentiraj*/}
        <Modal show={modal.urediKomentar}>
          <Modal.Body>
            <Form onSubmit={this.handleUrediKomentar}>
              <Form.Group className="mb-3" controlId="komentar">
                <Form.Label>Komentar</Form.Label>
                <Form.Control
                  type="text"
                  as="textarea"
                  name="komentar"
                  placeholder="Komentiraj"
                  defaultValue={trenutniKomentar.sadrzaj}
                  maxLength={250}
                  required
                />
              </Form.Group>
              <Row>
                <Col>
                  <Button
                    className="btn btn-danger gumb"
                    onClick={this.zatvoriModal}
                  >
                    Odustani
                  </Button>
                </Col>
                <Col>
                  <Button variant="primary" className="gumb" type="submit">
                    Komentiraj
                  </Button>
                </Col>
              </Row>
            </Form>
          </Modal.Body>
        </Modal>
      </>
    );
  }
}
