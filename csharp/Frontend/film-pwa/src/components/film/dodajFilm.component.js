import React, { Component } from "react";
import FilmDataService from "../../services/film.service";
import Container from 'react-bootstrap/Container';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import { Link } from "react-router-dom";

export default class DodajFilm extends Component {

    constructor(props) {
        super(props);
        this.dodajFilm = this.dodajFilm.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    async dodajFilm(film) {
        const odgovor = await FilmDataService.post(film);

        if(odgovor.ok) {
            window.location.href='/filmovi';
        }else{
            let poruke = '';
            console.log(odgovor);
            for (const key in odgovor.poruka.errors){
                if (odgovor.poruka.errors.hasOwnProperty(key)) {
                    poruke += `${odgovor.poruka.errors[key]}` + '\n';
                }
            }

            alert(poruke);
        }
    }

    handleSubmit(e) {
        e.preventDefault();

        const podaci = new FormData(e.target);

        let godina=0;
        if (podaci.get('godina').trim().length>0){
         godina = parseInt(podaci.get('godina'))
        }

        this.dodajFilm({
            naziv: podaci.get('naziv'),
            godina: godina,
            redatelj: podaci.get('redatelj'),
            zanr: podaci.get('zanr')
        });
    }

    render() { 
        return (
        <Container>
            <Form onSubmit={this.handleSubmit}>
    
    
              <Form.Group className="mb-3" controlId="naziv">
                <Form.Label>Naziv</Form.Label>
                <Form.Control type="text" name="naziv" placeholder="Naziv filma" maxLength={50} required/>
              </Form.Group>
    
    
              <Form.Group className="mb-3" controlId="godina">
                <Form.Label>Godina</Form.Label>
                <Form.Control type="number" min="1890" max="2024" name="godina" placeholder="Godina" />
              </Form.Group>


              <Form.Group className="mb-3" controlId="redatelj">
                <Form.Label>Redatelj</Form.Label>
                <Form.Control type="text" name="redatelj" placeholder="Redatelj" maxLength={50}/>
              </Form.Group>
    
    
              <Form.Group className="mb-3" controlId="zanr">
                <Form.Label>Žanr</Form.Label>
                <Form.Control type="text" name="zanr" placeholder="Žanr" maxLength={50}/>
              </Form.Group>
    

              <Row>
                <Col>
                  <Link className="btn btn-danger gumb" to={`/filmovi`}>Odustani</Link>
                </Col>
                <Col>
                <Button variant="primary" className="gumb" type="submit">
                  Dodaj film
                </Button>
                </Col>
              </Row>
             
              
            </Form>
    
    
          
        </Container>
        );
      }
}