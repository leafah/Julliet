import React, { Component } from 'react'
import 'milligram/dist/milligram.css'
import './style.css'
import { merge } from 'ramda'
import axios from 'axios'
import shortid from 'shortid'
import {
  addKeyPairItem,
  updateKeyPairItem,
  submitForm,
  toggleJsonXml
} from './setStates'
import KeyPairList from './KeyPairList.js'

export default class Register extends Component {
  constructor() {
    super()
    this.state = {
      headers: [{
        key: 'Content-Type',
        value: 'application/json',
        id: shortid.generate()
      }],
      body: [],
      type: 'json',
      method: 'post',
      url: 'https://api.pagar.me/1',
      letterType: 'response',
      letterPath: '',
      latterName: ''
    }

    this.addHeaderItem = this.addHeaderItem.bind(this)
    this.addBodyItem = this.addBodyItem.bind(this)
    this.onChange = this.onChange.bind(this)
    this.deleteItem = this.deleteItem.bind(this)
    this.changeType = this.changeType.bind(this)
    this.changeUrl = this.changeUrl.bind(this)
    this.changeMethod = this.changeMethod.bind(this)
    this.submitForm = this.submitForm.bind(this)
    this.updateLetterType = this.updateLetterType.bind(this)
    this.updateLetterName = this.updateLetterName.bind(this)
    this.updateLetterPath = this.updateLetterPath.bind(this)
  }

  changeType (event) {
    const { value } = event.target

    this.setState({
      type: value
    })

    this.setState(toggleJsonXml(value))
  }

  changeUrl (event) {
    this.setState({
      url: event.target.value
    })
  }

  changeMethod (event) {
    this.setState({
      method: event.target.value
    })
  }

  addHeaderItem (event) {
    if (event) event.preventDefault()

    this.setState(addKeyPairItem('headers'))
  }

  addBodyItem (event) {
    if (event) event.preventDefault()

    this.setState(addKeyPairItem('body'))
  }

  onChange (type, index, key) {
    return (event) => {
      event.preventDefault()
      const { value } = event.target
      this.setState(updateKeyPairItem(type, index, key, value))
    }
  }

  deleteItem (type, id) {
    return (event) => {
      let newArray = this.state[type].filter((item) => item.id !== id)
      this.setState({
        [type]: newArray
      })
    }
  }

  submitForm (event) { submitForm.bind(this)(event) }

  updateLetterType (event) {
    const { value } = event.target
    this.setState({
      letterType: value
    })
  }

  updateLetterPath (event) {
    const { value } = event.target
    this.setState({
      letterPath: value
    })
  }

  updateLetterName (event) {
    const { value } = event.target
    this.setState({
      letterName: value,
      letterPath: value + this.state.letterPath
    })
  }

  render() {
    return (
      <main className="container">
        <h1>Register</h1>
        <p>Here you can register your template data</p>
        <form
          onSubmit={this.submitForm.bind(this)}
        >
          <div className="input-row">
            <div>
              <label>
                Letter Name:
                <input
                  type="text"
                  name="letterName"
                  required
                  onChange={this.updateLetterName}
                  defaultValue={this.state.letterName}
                />
              </label>
            </div>
            <div>
              <label>
                Letter Path:
                <input
                  type="text"
                  required
                  name="letterPath"
                  onChange={this.updateLetterPath}
                  defaultValue={this.state.letterPath}
                />
              </label>
            </div>
          </div>
          <div>
            <label>
              Letter Type:
              <select
                name="letterType"
                onChange={this.updateLetterType}
                defaultValue={this.state.letterType}
              >
                <option value="response">response</option>
                <option value="request">request</option>
              </select>
            </label>
          </div>
          <div>
            <label>
              type:
              <select
                name="type"
                onChange={this.changeType}
                required
                defaultValue={this.type}
              >
                <option value="application/json">json</option>
                <option value="application/xml">xml</option>
              </select>
            </label>
          </div>

          <div>
            <label>
              method:
              <select
                name="method"
                onChange={this.changeMethod}
                required
                defaultValue={this.method}
              >
                <option value="post">post</option>
                <option value="get">get</option>
                <option value="put">put</option>
                <option value="delete">delete</option>
              </select>
            </label>
          </div>

          <div>
            <label>
              url:
              <input
                type="url"
                placeholder="https://"
                required
                defaultValue={this.state.url}
                onChange={this.changeUrl}
              />
            </label>
          </div>

          <div>
            <label>Headers:</label>

            <KeyPairList
              type="headers"
              items={this.state.headers}
              onChange={this.onChange}
              deleteItem={this.deleteItem}
              addItem={this.addHeaderItem}
            />
          </div>

          <div>
            <label>Body:</label>

            <KeyPairList
              type="body"
              items={this.state.body}
              onChange={this.onChange}
              deleteItem={this.deleteItem}
              addItem={this.addBodyItem}
            />
          </div>

          <div className="submit-row">
            <button
              type="submit"
              className="button large"
            >
              Registrar!
            </button>
          </div>
        </form>
      </main>
    )
  }
}
