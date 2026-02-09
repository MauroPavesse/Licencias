import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7241/api', // Cambia esto por tu URL
  headers: {
    'Content-Type': 'application/json',
  },
});

export default api;