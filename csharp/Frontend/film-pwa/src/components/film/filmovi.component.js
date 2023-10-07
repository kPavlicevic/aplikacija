import React, { Component } from "react";
import { Container, Table } from "react-bootstrap";


export default class Filmovi extends Component{


    render(){
        return (
            <Container>
               <a href="/filmovi/dodaj" className="btn btn-success gumb">
                Dodaj novi film
               </a>
                
               <Table striped bordered hover responsive>
                <thead>
                    <tr>
                        <th>Naziv</th>
                        <th>Godina</th>
                        <th>Redatelj</th>
                        <th>Žanr</th>
                        <th>Akcija</th>
                    </tr>
                </thead>
                <tbody>
                    {/* Ovdje će doći podaci s backend-a */}
                </tbody>
               </Table>



            </Container>


        );
    }
}