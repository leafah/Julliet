import React, { Component } from 'react';
import { BrowserRouter as Router, Route, Link } from 'react-router-dom'
import './App.css';

import Register from './pages/Register';
import Letters from './pages/Letters';
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
              <span>How it Works</span>
              <span>Letters</span>
              <span>Add Letter</span>

            </nav>

            <Route exact path="/" component={HowItWorks} />
            <Route path="/register" component={Register} />
            <Route path="/letters" component={Letters} />
          </div>
        </Router>
      </div>
    );
  }
}

export default App;
