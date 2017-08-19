import shortid from 'shortid'
import { merge } from 'ramda'
import axios from 'axios'

export function addKeyPairItem (stateKey) {
  return (state, props) => ({
    [stateKey]: [...state[stateKey], {
      key: 'Key Name',
      value: 'Key Content',
      id: shortid.generate()
    }]
  })
}

export function updateKeyPairItem (stateKey, index, key, value) {
  return (state, props) => {
    let oldArray = merge({}, { [stateKey]: state[stateKey] })

    oldArray[stateKey][index][key] = value

    return {
      [stateKey]: [...oldArray[stateKey]]
    }
  }
}

export function submitForm (event) {
  if (!this.state.headers.length) {
    event.preventDefault()
    alert('Insert at least one header option')
    this.addHeaderItem()
    return
  }

  if (!this.state.body.length) {
    event.preventDefault()
    alert('Insert at least one body option')
    this.addBodyItem()
    return
  }

  event.preventDefault()

  console.log(JSON.stringify(this.state, null, 2, 2));

  axios({
    method: 'post',
    url: 'https://requestb.in/ulz7wvul',
    data: merge({}, this.state)
  }).then(() => {
    alert('Submited with success!')
  }).catch(() => {
    alert('Something went wrong!')
  })
}

export function toggleJsonXml (type) {
  return (state, props) => {
    const hasContentType = state
      .headers.filter(
        pair => pair.key.toLowerCase() === 'content-type'
      ).length

    if (!hasContentType) {
      return {
        headers: state.headers.concat([{
          key: 'Content-Type',
          value: type
        }])
      }
    }

    return {
      headers: state.headers.map((pair) => {
        if (pair.key.toLowerCase() === 'content-type') {
          return {
            key: 'Content-Type',
            value: type
          }
        }

        return pair
      })
    }
  }
}
