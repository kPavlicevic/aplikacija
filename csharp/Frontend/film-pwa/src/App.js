import React from 'react';
import './App.css';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Izbornik from './components/izbornik.component';
import Pocetna from './components/pocetna.component';
import Filmovi from './components/film/filmovi.component';
import DodajFilm from './components/film/dodajFilm.component';
import PromjeniFilm from './components/film/promjeniFilm.component';

export default function App() {
  return (
    <Router>
      <Izbornik />
      <Routes>
        <Route path='/' element={<Pocetna />} />
        <Route path='/filmovi' element={<Filmovi />} />
        <Route path="/filmovi/dodaj" element={<DodajFilm />} />
        <Route path="/filmovi/:sifra" element={<PromjeniFilm />} />
      </Routes>

    </Router>
  );
}

