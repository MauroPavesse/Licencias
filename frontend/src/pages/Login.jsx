import { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';
import { message } from 'antd';

const Login = () => {
    const [clave, setClave] = useState('');
    const { login } = useAuth();
    const navigate = useNavigate();

    const handleLogin = (e) => {
        e.preventDefault();

        if (clave != "43041735Mau#") {
            message.open({
                type: 'error',
                content: 'Clave incorrecta',
            });
        }
        else {
            login();
            navigate('/dashboard');
        }
    };

  return (
    <div className="min-h-screen flex items-center justify-center bg-slate-100">
        <form onSubmit={handleLogin} className="bg-white p-8 rounded-xl shadow-lg w-96">
            <h2 className="text-2xl font-bold mb-6 text-center text-slate-800">Bienvenido</h2>
            <div className="space-y-4">
            <input 
                type="password"
                className="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 outline-none"
                placeholder="La clave"
                onChange={(e) => setClave(e.target.value)}
            />
            <button className="w-full bg-blue-600 text-white p-3 rounded-lg font-semibold hover:bg-blue-700 transition">
                Entrar
            </button>
            </div>
        </form>
    </div>
    );
};

export default Login;