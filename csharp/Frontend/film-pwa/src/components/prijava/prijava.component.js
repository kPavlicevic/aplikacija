import React, { Component } from "react";
import { Button, Container, Form } from "react-bootstrap";
import { Link } from "react-router-dom";
import AuthService from "../../services/auth.service";

export default class Prijava extends Component {
  constructor(props) {
    super(props);
    this.state = {
      tip: props.tip,
      error: "",
    };
    this.submit = this.submit.bind(this);
  }

  componentDidUpdate(prevProps) {
    const { naslov, link } = this.props.tip;
    if (naslov !== prevProps.tip.naslov) {
      this.setState({
        tip: { naslov, link },
      });
    }
  }

  async submit(e) {
    e.preventDefault();
    const korisnickoIme = e.target.uName.value.trim();
    const lozinka = e.target.pwd.value;

    if (!korisnickoIme) {
      this.setState({
        error: "Neispravan unos korisničkog imena",
      });
      return;
    }

    if (!lozinka || lozinka.length < 4) {
      this.setState({
        error: "Lozinka mora sadržavati barem 4 znaka!",
      });
      return;
    }

    const odgovor = await AuthService.prijava({
      korisnickoIme,
      lozinka,
    });
    if (odgovor.ok) {
      window.location.href = odgovor.preusmjeri;
    } else {
      this.setState({
        error: odgovor.error,
      });
    }
  }

  render() {
    const { naslov, link } = this.state.tip;
    const { error } = this.state;
    return (
      <Container className="center">
        {<h2>{naslov}</h2>}
        <Form className="loginForm center" onSubmit={this.submit}>
          <Form.Group className="mb-3" controlId="uName">
            <Form.Label>Username</Form.Label>
            <Form.Control type="text" placeholder="Enter username" required />
          </Form.Group>

          <Form.Group className="mb-3" controlId="pwd">
            <Form.Label>Password</Form.Label>
            <Form.Control type="password" placeholder="Password" required />
          </Form.Group>

          <Button variant="primary" type="submit" size="md">
            Submit
          </Button>
          <hr />
          <Form.Text>
            {link.pitanje}
            <Link to={link.href}>{link.text}</Link>
          </Form.Text>
          <Form.Text style={{ color: "red" }}>{error}</Form.Text>
        </Form>
      </Container>
    );
  }
}
