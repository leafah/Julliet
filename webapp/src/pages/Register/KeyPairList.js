import React from 'react'

export default function KeyPairList (props) {
  return (
    <div>
      {props.items.map((item, index) => (
        <div
          className="key-value-item"
          key={item.id}
        >
          <input
            type="text"
            required
            autoFocus
            value={item.key}
            onChange={props.onChange(
              props.type,
              index,
              'key'
            )}
          />
          <input
            type="text"
            required
            onChange={props.onChange(
              props.type,
              index,
              'value'
            )}
            value={item.value}
          />
          <button
            onClick={props.deleteItem(
              props.type,
              item.id
            )}
          >Delete</button>
        </div>
      ))}

      <div>
        <button
          onClick={props.addItem}
        >+ Add Item</button>
      </div>
    </div>
  )
}
