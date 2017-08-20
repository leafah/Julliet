import React, { Component } from 'react'
import 'milligram/dist/milligram.css'
import './style.css'
import Editor from '../../components/Editor'

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
    this.submitForm = this.submitForm.bind(this)
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

  updateLetter (letter) {
    console.log('letter', letter);
    this.setState({ letter })
  }

  submitForm (event) {
    event.preventDefault()

    console.log(this.state)
    console.log(JSON.stringify(this.state, null, 2, 2))
  }

  render () {
    const {
      name,
      type,
      letter
    } = this.state

    return (
      <div className="container space">
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

          <label>Carta:</label>

          <Editor
            onCodeChange={this.updateLetter}
          />

          <div className="space">
            <button
              type="submit"
            >Salvar Carta</button>
          </div>
        </form>
      </div>
    )
  }
}
