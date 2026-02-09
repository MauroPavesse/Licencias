import { createContext, useState, useContext } from 'react';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  // Al iniciar, revisa si ya habías entrado antes (se guarda en el navegador)
  const [user, setUser] = useState(() => {
    return localStorage.getItem('user_auth') ? JSON.parse(localStorage.getItem('user_auth')) : null;
  });

  const login = () => {
    const userData = { loggedIn: true }; // Aunque no uses datos, pon un objeto
    setUser(userData);
    localStorage.setItem('user_auth', JSON.stringify(userData));
  };

  const logout = () => {
    localStorage.removeItem('user_auth');
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

// eslint-disable-next-line react-refresh/only-export-components
export const useAuth = () => useContext(AuthContext);