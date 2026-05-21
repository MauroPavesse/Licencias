import axios from 'axios';

const api = axios.create({
  baseURL: 'http://72.60.60.66:81/api',
  //baseURL: 'https://localhost:7241/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

export default api;