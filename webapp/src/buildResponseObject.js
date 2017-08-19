const letterRequest = {
  "amount": "{{valor}}",
  "installments": "{{parcelas}}",
  "cards": {
    "brand": "{{bandeira}}"
  }
}

const rawResponse = {
  "amount": 12312,
  "installments": 3,
  "cards": {
    "brand": "visa"
  }
}

const response = {
  "valor": 12312,
  "parcelas": 3,
  "bandeira": "visa"
}
