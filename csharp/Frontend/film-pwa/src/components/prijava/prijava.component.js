import React, { Component } from "react";
import { Button, Container, Form } from "react-bootstrap";
import { Link } from "react-router-dom";

export default class Prijava extends Component {
  constructor(props) {
    super(props);
    console.log(props.tip);
    this.state = props.tip;
  }

  render() {
    const { naslov, link } = this.state;
    return (
      <Container>
        {<h2>{naslov}</h2>}
        <Form className="loginForm">
          <Form.Group className="mb-3" controlId="uName">
            <Form.Label>Username</Form.Label>
            <Form.Control type="text" placeholder="Enter username" />
          </Form.Group>

          <Form.Group className="mb-3" controlId="pwd">
            <Form.Label>Password</Form.Label>
            <Form.Control type="password" placeholder="Password" />
          </Form.Group>

          <Button variant="primary" type="submit" size="lg">
            Submit
          </Button>
          <hr />
          <Form.Text>
            {link.pitanje}
            <Link to={link.href} className="App-link">
              {link.text}
            </Link>
          </Form.Text>
        </Form>
      </Container>
    );
  }
}
