import React, { Component } from "react";

import GlumacDataService from "../../services/glumac.service";
export default class Glumci extends Component {
  constructor(props) {
    super(props);

    this.state = {
      glumci: 0,
    };
  }

  componentDidMount() {
    this.dohvatiGlumce();
  }

  async dohvatiGlumce() {
    await GlumacDataService.get()
      .then((response) => {
        console.log(response.data);
        this.setState({
          glumci: response.data,
        });
      })
      .catch((e) => {
        console.log(e);
      });
  }

  render() {
       const {glumci} = this.state;
    return <div>
        {glumci && glumci.map((glumac, index) => {

            return <div>
                <p>{glumac.ime} {glumac.prezime}</p>
            </div>
        })}
    </div>;
  }
}
