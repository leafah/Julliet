import React, { Component } from 'react'
import AceEditor from 'react-ace'
import 'brace/mode/java'
import 'brace/theme/monokai'

export default class Editor extends Component {
  constructor () {
    super()
    this.state = {
      code: ''
    }

    this.updateCode = this.updateCode.bind(this)
  }

  updateCode ({ code }) {
    this.setState({ code })
    this.props.onCodeChange(code)
  }

  shouldComponentUpdate() {
    return false
  }

  render () {
    return (
      <AceEditor
        id="textarea-001"
        mode="java"
        theme="monokai"
        onChange={this.updateCode}
        name="UNIQUE_ID_OF_DIV"
        editorProps={{$blockScrolling: true}}
        width="100%"
        showPrintMargin={false}
      />
    )
  }
}
