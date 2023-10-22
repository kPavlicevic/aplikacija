import React, { Component } from "react";
import GlumacDataService from "../../services/glumac.service";
import SlikaService from "../../services/slike.service";
import Container from "react-bootstrap/Container";
import Button from "react-bootstrap/Button";
import Form from "react-bootstrap/Form";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import { Link } from "react-router-dom";
import { Modal, ModalBody } from "react-bootstrap";

import noimage from "../../images/no-image-found-360x250.png";
import Cropper from "react-cropper";
import "cropperjs/dist/cropper.css";
import { Image } from "react-bootstrap";

export default class PromjeniGlumac extends Component {
  constructor(props) {
    super(props);
    this.glumac = this.dohvatiGlumac();
    this.trenutnaSlika = this.dohvatiSlikuGlumca();
    this.promjeniGlumac = this.promjeniGlumac.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.zatvoriModal = this.zatvoriModal.bind(this);
    this.otvoriModal = this.otvoriModal.bind(this);
    this.spremiSliku = this.spremiSliku.bind(this);

    this.state = {
      glumac: {},
      trenutnaSlika: noimage,
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

  _crop() {
    // image in dataUrl
    // console.log(this.cropper.getCroppedCanvas().toDataURL());
    this.setState({
      slikaZaServer: this.cropper.getCroppedCanvas().toDataURL(),
    });
  }

  onCropperInit(cropper) {
    this.cropper = cropper;
  }

  onChange = (e) => {
    e.preventDefault();
    let files;
    if (e.dataTransfer) {
      files = e.dataTransfer.files;
    } else if (e.target) {
      files = e.target.files;
    }
    const reader = new FileReader();
    reader.onload = () => {
      this.setState({
        image: reader.result,
      });
    };
    try {
      reader.readAsDataURL(files[0]);
    } catch (error) {}
  };

  spremiSlikuAkcija = () => {
    const { glumac } = this.state;

    this.spremiSliku(glumac.sifra);
  };

  async spremiSliku(sifra) {
    const file = document.getElementById("file").files[0];
    const formData = new FormData();
    formData.append("file", file);
    formData.append("Vrsta", 2);
    formData.append("SifraVeze", sifra);
    const odgovor = await SlikaService.postaviSliku(formData);
    if (odgovor.ok) {
      //window.location.href='/polaznici';
      this.dohvatiSlikuGlumca();
    } else {
      // pokaži grešku
      alert(odgovor.poruka);
      console.log(odgovor);
    }
  }

  async dohvatiSlikuGlumca() {
    const href = window.location.href;
    const niz = href.split("/");
    const sifra = niz[niz.length - 1];
    const odgovor = await SlikaService.getSlikuGlumcaPoSifri(sifra);
    if (odgovor.ok) {
      this.setState({
        trenutnaSlika: "data:image/png;base64," + odgovor.slika,
      });
    } else {
      alert(odgovor.poruka);
    }
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
    const { glumac, modal, trenutnaSlika } = this.state;
    const { image } = this.state;
    const { slikaZaServer } = this.state;

    return (
      <>
        <Container>
          <Row>
            <Col md={6} lg={5}>
              <img
                src={trenutnaSlika}
                className="imgKontejner border"
                alt="slika glumca"
                width="360px"
                height="250px"
              />
            </Col>
            <Col>
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
            </Col>
          </Row>
          <Button style={{marginTop: "10px"}} onClick={this.otvoriModal}>Uredi</Button>
        </Container>
        <Modal show={modal.otvori}>
          <ModalBody>
            <Container>
              <Form onSubmit={this.handleSubmit}>
                <Row>
                  <Col key="1" sm={12} lg={6} md={6}>
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
                  </Col>
                  <Col key="2" sm={12} lg={6} md={6}>
                    Trenutna slika
                    <br />
                    <Image src={trenutnaSlika} className="imgKontejner" width={183} height={270}/>
                  </Col>
                </Row>
                <Row>
                  <Col key="3" sm={12} lg={6} md={6}>
                    <input id="file" type="file" onChange={this.onChange} />

                    <input
                      type="button"
                      onClick={this.spremiSlikuAkcija}
                      value={"Spremi sliku"}
                    />
                  </Col>
                  <Row>
                    <Col key="4" sm={6} lg={6} md={6}>
                      Nova slika
                      <br />
                      <Image src={slikaZaServer} width="183px" height="270px" />
                    </Col>
                    <Col key="5" sm={6}>
                      <Cropper
                        src={image}
                        style={{ height: 270 }}
                        initialAspectRatio={1}
                        guides={true}
                        viewMode={1}
                        minCropBoxWidth={183}
                        minCropBoxHeight={270}
                        cropBoxResizable={false}
                        background={false}
                        responsive={true}
                        checkOrientation={false}
                        crop={this._crop.bind(this)}
                        onInitialized={this.onCropperInit.bind(this)}
                      />
                    </Col>
                  </Row>
                </Row>

                <Row>
                  <Col>
                    <Button
                      className="btn btn-danger gumb"
                      onClick={this.zatvoriModal}
                    >
                      Zatvori
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
