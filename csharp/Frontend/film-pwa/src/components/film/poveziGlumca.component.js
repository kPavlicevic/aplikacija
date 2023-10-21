import React, { Component } from 'react'
import { Dropdown } from 'react-bootstrap'
import GlumacDataService from "../../services/glumac.service";
import http from '../../http-common';

export default class PoveziGlumca extends Component {

    constructor(props){
        super(props);
        this.glumci = this.dohvatiGlumce();
        this.dodajGlumca = this.dodajGlumca.bind(this);
        this.state = {
            glumci:[],
        };
    }

    componentDidUpdate(prevProps){
        if(prevProps !== this.props){
            this.setState({
                film: this.props.film
            })
        }
    }

    async dohvatiGlumce(){
        await GlumacDataService.getAll()
        .then(response => {
            console.log(response);
            this.setState({
                glumci: response.data
            });
        }).catch(e =>{
            console.log(e.response.data);
        });
    }

    async dodajGlumca(sifra){
        const href = window.location.href;
        const niz = href.split("/");
        const sifraFilm = niz[niz.length - 1];
        const url = "Film/" + sifraFilm + "/dodajGlumca/" + sifra;
        await http.post(url).then(response => {
            window.location.reload();
        }).catch(e => {
            alert(e.response.data);
        })
    }


  render() {
    const {glumci} = this.state;
    return (
      <Dropdown onSelect={this.dodajGlumca}>
        <Dropdown.Toggle varian="success" id="glumciDropdown">
            Dodaj glumca 
        </Dropdown.Toggle>
        <Dropdown.Menu>
            {glumci && glumci.map(glumac => {
             return <Dropdown.Item key={glumac.sifra} eventKey={glumac.sifra}>{glumac.ime} {glumac.prezime}</Dropdown.Item>
            
            })}
        </Dropdown.Menu>
      </Dropdown>
    )
  }
}
