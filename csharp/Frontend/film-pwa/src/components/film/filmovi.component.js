import React, { Component } from "react";
import { Button, Container, Table } from "react-bootstrap";
import FilmDataServices from "../../services/film.service";
import { Link } from "react-router-dom";
import {FaEdit, FaTrash} from "react-icons/fa";

export default class Filmovi extends Component{

    constructor(props){
        super(props);

        this.state = {
            filmovi: []
        };
    }

    componentDidMount(){
        this.dohvatiFilmovi();
    }

    async dohvatiFilmovi(){

        await FilmDataServices.get()
        .then(response => {
            this.setState({
                filmovi: response.data
            });
            console.log(response.data);
        })
        .catch(e =>{
            console.log(e);
        });
    }

    async obrisiFilm(sifra){
        const odgovor = await FilmDataServices.delete(sifra);
        if(odgovor.ok){
            this.dohvatiFilmovi();
        }else{
            alert(odgovor.poruka);
        }
    }

    render(){

        const { filmovi } = this.state;

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
                    { filmovi ? filmovi.map((film,index) => (

                        <tr key={index}>
                            <td>{film.naziv}</td>
                            <td className="broj">{film.godina}</td>
                            <td>{film.redatelj}</td>
                            <td>{film.zanr}</td>
                            <td>
                                <Link className="btn btn-primary gumb"
                                to={`/filmovi/${film.sifra}`}>
                                    <FaEdit />
                                </Link>

                                <Button variant="danger" className="gumb"
                                onClick={()=>this.obrisiFilm(film.sifra)} >
                                    <FaTrash />
                                </Button>

                            </td>
                        </tr>
                    ))
                    :
                    <tr>
                        <td align="center" colspan="5">Ne postoji niti jedan film</td>
                    </tr>
                }
                </tbody>
               </Table>



            </Container>


        );
    }
}