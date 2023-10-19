import React, { Component } from "react";
import GlumacDataService from "../../services/glumac.service";
import Container from 'react-bootstrap/Container';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import { Link } from "react-router-dom";


export default class DodajGlumac extends Component {

    constructor(props) {
        super(props);
        this.dodajGlumac = this.dodajGlumac.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }
    async dodajGlumac(film) {
        const odgovor = await GlumacDataService.post(film);
        if(odgovor.ok){
            window.location.href='/glumci';
        }else{
            console.log(odgovor);
        }
    }

    handleSubmit(e) {
        e.preventDefault();
        const podaci = new FormData(e.target);
    
        this.dodajGlumac({
          ime: podaci.get('ime'),
          prezime: podaci.get('prezime'),
          drzavljanstvo: podaci.get('drzavljanstvo'),
        });
        
      }


      render() { 
        return (
        <Container>
            <Form onSubmit={this.handleSubmit}>
    
    
              <Form.Group className="mb-3" controlId="ime">
                <Form.Label>Ime</Form.Label>
                <Form.Control type="text" name="ime" placeholder="Ime" maxLength={255} required/>
              </Form.Group>
    
    
              <Form.Group className="mb-3" controlId="prezime">
                <Form.Label>Prezime</Form.Label>
                <Form.Control type="text" name="prezime" placeholder="Prezime" required />
              </Form.Group>


              <Form.Group className="mb-3" controlId="drzavljanstvo">
                <Form.Label>Državljanstvo</Form.Label>
                <Form.Control type="text" name="drzavljanstvo" placeholder="Državljanstvo" required />
              </Form.Group>

    
              <Row>
                <Col>
                  <Link className="btn btn-danger gumb" to={`/glumci`}>Odustani</Link>
                </Col>
                <Col>
                <Button variant="primary" className="gumb" type="submit">
                  Dodaj glumca
                </Button>
                </Col>
              </Row>
             
              
            </Form>
    
    
          
        </Container>
        );
      }
}