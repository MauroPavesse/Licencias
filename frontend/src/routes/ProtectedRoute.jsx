import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import DashboardLayout from '../layouts/DashboardLayout';

const ProtectedRoute = () => {
    const { user } = useAuth();

    // Si no hay usuario, redirigimos al login
    if (!user) {
        return <Navigate to="/login" replace />;
    }

    // Si hay usuario, permitimos el acceso al contenido (Outlet)
    return (
        <DashboardLayout>
            <Outlet />
        </DashboardLayout>
    );
};

export default ProtectedRoute;