import React from 'react';
import logo from './logo.svg';
import './App.css';
import HomePage from './pages/HomePage';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Kata's Movie Database
        </p>
        <a
          className="App-link"
          href="https://imdb.com"
          target="_blank"
          rel="noopener noreferrer"
        >
          Enter
        </a>
      </header>
    </div>
  );
}

export default App;
