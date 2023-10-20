import React, { useEffect, useState } from "react";
import "./App.css";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Izbornik from "./components/izbornik.component";
import Pocetna from "./components/pocetna.component";
import Filmovi from "./components/film/filmovi.component";
import DodajFilm from "./components/film/dodajFilm.component";
import PromjeniFilm from "./components/film/promjeniFilm.component";
import Glumci from "./components/glumac/glumci.component";
import DodajGlumac from "./components/glumac/dodajGlumac.componenet";
import PromjeniGlumac from "./components/glumac/promjeniGlumac.component";
import Prijava from "./components/prijava/prijava.component";
import { AUTH, LOGIN, REGISTER } from "./konstante";

export default function App() {

  const [auth, setAuth] = useState(JSON.parse(localStorage.getItem("auth")));
  
  return (
      <Router>
        <Izbornik auth={auth}/>
        <Routes>
          <Route path="/" element={<Pocetna />} />
          <Route path="/filmovi" element={<Filmovi />} />
          <Route path="/filmovi/dodaj" element={<DodajFilm />} />
          <Route path="/filmovi/:sifra" element={<PromjeniFilm />} />
          <Route path="/glumci" element={<Glumci />} />
          <Route path="/glumci/dodaj" element={<DodajGlumac />} />
          <Route path="/glumci/:sifra" element={<PromjeniGlumac />} />
          <Route path="/prijava" element={<Prijava tip={LOGIN} />} />
          <Route path="/registracija" element={<Prijava tip={REGISTER} />} />
        </Routes>
      </Router>
  );
}
