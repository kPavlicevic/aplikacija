import React, { Component } from "react";
import FilmDataService from "../../services/film.service";
import Container from "react-bootstrap/Container";
import Button from "react-bootstrap/Button";
import Form from "react-bootstrap/Form";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import { Link } from "react-router-dom";
import { Modal } from "react-bootstrap";
import noimage from "../../images/no-image-found-360x250.png";

export default class PromjeniFilm extends Component {
  constructor(props) {
    super(props);

    this.film = this.dohvatiFilm();
    this.promjeniFIlm = this.promjeniFilm.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.zatvoriModal = this.zatvoriModal.bind(this);
    this.otvoriModal = this.otvoriModal.bind(this);
    this.state = {
      film: {},
      modal: {
        uredi: false,
        ocijeni: false,
        komentiraj: false,
      },
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

  handleSubmit(e) {
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

  zatvoriModal() {
    this.setState({
      modal: {
        uredi: false,
        ocijeni: false,
        komentiraj: false,
      },
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
    const { film, modal } = this.state;
    return (
      <>
        <img src={noimage} alt="slika filma" />
        <ul>
          <li>Naziv: {film.naziv}</li>
          <li>Godina: {film.godina}</li>
          <li>Redatlje: {film.redatelj}</li>
          <li>Žanr: {film.zanr}</li>
          <li>
            Glumci:{" "}
            {film.glumci &&
              film.glumci.map((glumac) => (
                <div key={glumac.sifra}>
                  <Link to={"/glumci/" + glumac.sifra}>
                    {glumac.ime} {glumac.prezime}
                  </Link>
                </div>
              ))}
          </li>
        </ul>
        <Button onClick={() => this.otvoriModal("u")}>Uredi</Button>
        <Button onClick={() => this.otvoriModal("o")}>Ocijeni</Button>
        <Button onClick={() => this.otvoriModal("k")}>Komentiraj</Button>

        {/* Modal promjeni */}

        <Modal show={modal.uredi}>
          <Modal.Body>
            <Container>
              <Form onSubmit={this.handleSubmit}>
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
          <Modal.Body>
            <Container>
              <Form>
                <Form.Group className="mb-3" controlId="ocjena">
                  <Form.Label>Ocjena</Form.Label>
                  <Form.Control
                    type="number"
                    name="ocjena"
                    placeholder="1-5"
                    max={5}
                    min={1}
                    step={0.1}
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
            <Form>
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
      </>
    );
  }
}
