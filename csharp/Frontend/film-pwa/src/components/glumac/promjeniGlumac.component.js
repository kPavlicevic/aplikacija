import React, { Component } from "react";
import GlumacDataService from "../../services/glumac.service";
import Container from "react-bootstrap/Container";
import Button from "react-bootstrap/Button";
import Form from "react-bootstrap/Form";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import { Link } from "react-router-dom";
import { Modal, ModalBody } from "react-bootstrap";
import noimage from "../../images/no-image-found-360x250.png";

export default class PromjeniGlumac extends Component {
  constructor(props) {
    super(props);
    this.glumac = this.dohvatiGlumac();
    this.promjeniGlumac = this.promjeniGlumac.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.zatvoriModal = this.zatvoriModal.bind(this);
    this.otvoriModal = this.otvoriModal.bind(this);

    this.state = {
      glumac: {},
      modal: {
        otvori: false,
      },
    };
  }

  async dohvatiGlumac() {
    let href = window.location.href;
    let niz = href.split("/");
    await GlumacDataService.getBySifra(niz[niz.length - 1])
      .then((response) => {
        this.setState({
          glumac: response.data,
        });
        console.log(response.data);
      })
      .catch((e) => {
        console.log(e);
      });
  }

  async promjeniGlumac(glumac) {
    let href = window.location.href;
    let niz = href.split("/");
    const odgovor = await GlumacDataService.put(niz[niz.length - 1], glumac);
    if (odgovor.ok) {
      window.location.href = "/glumci";
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
    //Object.keys(formData).forEach(fieldName => {
    // console.log(fieldName, formData[fieldName]);
    //})

    //console.log(podaci.get('verificiran'));
    // You can pass formData as a service body directly:

    this.promjeniGlumac({
      ime: podaci.get("ime"),
      prezime: podaci.get("prezime"),
      drzavljanstvo: podaci.get("drzavljanstvo"),
    });
  }

  zatvoriModal() {
    this.setState({
      modal: {
        otvori: false,
      },
    });
  }

  otvoriModal() {
    this.setState({
      modal: {
        otvori: true,
      },
    });
  }

  render() {
    const { glumac, modal } = this.state;

    return (
      <>
        <img src={noimage} alt="slika glumca" />
        <ul>
          <li>Ime: {glumac.ime}</li>
          <li>Prezime: {glumac.prezime}</li>
          <li>Državljanstvo: {glumac.drzavljanstvo}</li>
          <li>
            Film:{" "}
            {glumac.filmovi &&
              glumac.filmovi.map((film) => (
                <div key={film.sifra}>
                  <Link to={"/filmovi/" + film.sifra}>
                    {film.naziv} {film.godina}
                  </Link>
                </div>
              ))}
          </li>
        </ul>
        <Button onClick={this.otvoriModal}>Uredi</Button>
        <Modal show={modal.otvori}>
          <ModalBody>
            <Container>
              <Form onSubmit={this.handleSubmit}>
                <Form.Group className="mb-3" controlId="ime">
                  <Form.Label>Ime</Form.Label>
                  <Form.Control
                    type="text"
                    name="ime"
                    placeholder="Ime glumca"
                    maxLength={50}
                    defaultValue={glumac.ime}
                    required
                  />
                </Form.Group>

                <Form.Group className="mb-3" controlId="prezime">
                  <Form.Label>Prezime</Form.Label>
                  <Form.Control
                    type="text"
                    name="prezime"
                    placeholder="Prezime glumca"
                    maxLength={50}
                    defaultValue={glumac.prezime}
                    required
                  />
                </Form.Group>

                <Form.Group className="mb-3" controlId="drzavljanstvo">
                  <Form.Label>Državljanstvo</Form.Label>
                  <Form.Control
                    type="text"
                    name="drzavljanstvo"
                    placeholder="Državljanstvo"
                    maxLength={50}
                    defaultValue={glumac.drzavljanstvo}
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
                      Promjeni glumca
                    </Button>
                  </Col>
                </Row>
              </Form>
            </Container>
          </ModalBody>
        </Modal>
      </>
    );
  }
}
