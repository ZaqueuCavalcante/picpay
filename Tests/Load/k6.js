import http from 'k6/http'

import { check } from 'k6'

export default function () {
    let headers = { 'Content-Type': 'application/json; charset=utf-8' };
    const data = {
        name: 'user_0',
        cpf: '11924528444',
        email: 'user_0@picpay.com',
        password: 'Test@123'
    }
    let res = http.post('http://localhost:5001/customers', JSON.stringify(data), { headers: headers })

    console.log(res)

    check(res, { 'customer created': (r) => r.status === 200 })
}
