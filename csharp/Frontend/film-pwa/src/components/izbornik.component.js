import React, { Component } from "react";
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavDropdown from 'react-bootstrap/NavDropdown';

import logo from '../images/logo.png';

export default class Izbornik extends Component{


    render(){
        return (

            <Navbar expand="lg" className="bg-body-tertiary" id="mojIzbornik">
            <Container>
              <Navbar.Brand href="/"> <img className="App-logo" src={logo} alt="" /> Recenzija na film App</Navbar.Brand>
              <Navbar.Toggle aria-controls="basic-navbar-nav" />
              <Navbar.Collapse id="basic-navbar-nav">
                <Nav className="me-auto">
                  <NavDropdown title="Pregled" id="basic-nav-dropdown">
                    <NavDropdown.Item href="/filmovi">Filmovi</NavDropdown.Item>
                    <NavDropdown.Item href="/glumci">
                      Glumci
                    </NavDropdown.Item>
                    <NavDropdown.Divider />
                    <NavDropdown.Item target="_blank" href="https://kpavlicevic-001-site1.ctempurl.com/swagger/index.html">
                      Swagger
                    </NavDropdown.Item>
                  </NavDropdown>
                </Nav>
              </Navbar.Collapse>
            </Container>
          </Navbar>



        );
    }
}