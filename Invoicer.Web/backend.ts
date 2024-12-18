import axios, { AxiosInstance } from 'axios'

const backend: AxiosInstance = axios.create( {
    baseURL: 'http://localhost:5279',
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
    },
} )

export default backend