import React, { Component } from 'react'
import 'milligram/dist/milligram.css'
import './style.css'

export default class SimpleLetter extends Component {
  constructor () {
    super()
    this.state = {
      name: '',
      type: '',
      letter: ''
    }

    this.updateName = this.updateName.bind(this)
    this.updateType = this.updateType.bind(this)
    this.updateLetter = this.updateLetter.bind(this)
  }

  updateName ({ target }) {
    this.setState({
      name: target.value
    })
  }

  updateType ({ target }) {
    this.setState({
      type: target.value
    })
  }

  updateLetter ({ target }) {
    this.setState({
      letter: target.value
    })
  }

  submitForm (event) {
    event.preventDefault()

    console.log(JSON.stringify(this.state, null, 2, 2))
  }

  render () {
    const {
      name,
      type,
      letter
    } = this.state

    return (
      <div className="container">
        <h1>Cadastrar Carta</h1>

        <p>Entre com as informações abaixo corretamente!</p>

        <form onSubmit={this.submitForm}>
          <div className="input-row">
            <div>
              <label>
                Nome:
                <input
                  id="input-001"
                  type="text"
                  name="name"
                  required
                  onChange={this.updateName}
                  defaultValue={name}
                />
              </label>
            </div>

            <div>
              <label>
                Tipo:
                <select
                  id="select-001"
                  type="text"
                  name="type"
                  required
                  onChange={this.updateType}
                  defaultValue={type}
                >
                  <option value="">Escolha um</option>
                  <option value="request">request</option>
                  <option value="response">response</option>
                </select>
              </label>
            </div>
          </div>

          <label htmlFor="textarea-001">Carta:</label>
          <textarea
            name="letter"
            id="textarea-001"
            required
            onChange={this.updateLetter}
            defaultValue={letter}
          ></textarea>

          <div>
            <button
              type="submit"
            >Salvar Carta</button>
          </div>
        </form>
      </div>
    )
  }
}
