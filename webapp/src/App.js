import React, { Component } from 'react';
import { BrowserRouter as Router, Route, Link } from 'react-router-dom'
import './App.css';

import SimpleLetter from './pages/SimpleLetter';
import HowItWorks from './pages/HowItWorks';
const logo = '/letters-photo.png';

class App extends Component {
  render() {
    return (
      <div className="App">
        <div className="App-header">
          <h1 className="App-header-title">Letters to Juliet</h1>
          <img src={logo} alt="logo" className="App-header-image" />
        </div>
        <Router>
          <div>
            <nav className="App-nav">
              <div className="container">
                <Link to="/">How it Works</Link>
                <Link to="/letter">Letter</Link>
              </div>
            </nav>

            <Route exact path="/" component={HowItWorks} />
            <Route path="/letter" component={SimpleLetter} />
          </div>
        </Router>
      </div>
    );
  }
}

export default App;
