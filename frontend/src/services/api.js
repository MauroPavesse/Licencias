import axios from 'axios';

const api = axios.create({
  baseURL: 'http://72.60.60.66:5000/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

export default api;