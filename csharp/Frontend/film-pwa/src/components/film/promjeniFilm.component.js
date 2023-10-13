import React, { Component } from "react";
import FilmDataService from "../../services/film.service";
import Container from 'react-bootstrap/Container';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import { Link } from "react-router-dom";



export default class PromjeniFilm extends Component {

  constructor(props) {
    super(props);

   
    this.film = this.dohvatiFilm();
    this.promjeniFIlm = this.promjeniFilm.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    

    this.state = {
      film: {}
    };

  }



  async dohvatiFilm() {
    let href = window.location.href;
    let niz = href.split('/'); 
    await FilmDataService.getBySifra(niz[niz.length-1])
      .then(response => {
        this.setState({
          film: response.data
        });
        console.log(response.data);
      })
      .catch(e => {
        console.log(e);
      });
    
   
  }

  async promjeniFilm(film) {
    // ovo mora bolje
    let href = window.location.href;
    let niz = href.split('/'); 
    const odgovor = await FilmDataService.put(niz[niz.length-1],film);
    if(odgovor.ok){
      // routing na smjerovi
      window.location.href='/filmovi';
    }else{
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

    this.promjeniFilm({
      naziv: podaci.get('naziv'),
      godina: parseInt(podaci.get('godina')),
      redatelj: podaci.get('redatelj'),
      zanr: podaci.get('zanr')
    });
    
  }


  render() {
    
   const { film } = this.state;


    return (
    <Container>
        <Form onSubmit={this.handleSubmit}>


          <Form.Group className="mb-3" controlId="naziv">
            <Form.Label>Naziv</Form.Label>
            <Form.Control type="text" name="naziv" placeholder="Naziv filma"
            maxLength={50} defaultValue={film.naziv} required />
          </Form.Group>


          <Form.Group className="mb-3" controlId="godina">
            <Form.Label>Godina</Form.Label>
            <Form.Control type="number" min="1890" max="2024" name="godina" defaultValue={film.godina}  placeholder="Godina" />
          </Form.Group>


          <Form.Group className="mb-3" controlId="redatelj">
            <Form.Label>Redatelj</Form.Label>
            <Form.Control type="text" name="redatelj" placeholder="Redatelj"
            maxLength={50} defaultValue={film.redatelj} />
          </Form.Group>

          <Form.Group className="mb-3" controlId="zanr">
            <Form.Label>Žanr</Form.Label>
            <Form.Control type="text" name="zanr" placeholder="Zanr"
            maxLength={50} defaultValue={film.zanr} />
          </Form.Group>

        
         
          <Row>
            <Col>
              <Link className="btn btn-danger gumb" to={`/smjerovi`}>Odustani</Link>
            </Col>
            <Col>
            <Button variant="primary" className="gumb" type="submit">
              Promjeni film
            </Button>
            </Col>
          </Row>
        </Form>


      
    </Container>
    );
  }
}