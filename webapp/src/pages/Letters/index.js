import React, { Component } from 'react'

export default class Letters extends Component {
  constructor () {
    super()
    this.state = {
      letters: []
    }
  }

  render () {
    return (
      <div className="container">
        <h1>Letters</h1>
        <p>These are the letters avaiable!</p>
      </div>
    )
  }
}
