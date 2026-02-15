import { Button, Card, Table, message, Modal } from 'antd'
import {
  EditOutlined,
  DeleteOutlined,
  ExclamationCircleOutlined
} from "@ant-design/icons";
import React, { useState, useEffect } from 'react'
import ClientModal from '../components/ClientModal';
import { customerService } from '../services/customerService';
import { SearchCommand } from "../DTOs/SearchCommand";

const Clients = () => {
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedRecord, setSelectedRecord] = useState(null);
    const [dataSource, setDataSource] = useState([]);
    const [loading, setLoading] = useState(false);

    const { confirm } = Modal;

    const handleCancel = () => {
        setIsModalOpen(false);
        setSelectedRecord(null);
    };

    const addClient = () => {
        setSelectedRecord(null);
        setIsModalOpen(true);
    };

    const handleSuccess = () => {
        setIsModalOpen(false);
        fetchData();
    };

    const handleDelete = (record) => {
        confirm({
            title: "¿Estás seguro de eliminar este cliente?",
            icon: <ExclamationCircleOutlined />,
            content: `Cliente: ${record.name}`,
            okText: "Sí, eliminar",
            okType: "danger",
            cancelText: "Cancelar",
            onOk: async () => {
                try {
                    await customerService.delete(record.id);
                    message.success("Eliminado correctamente");
                    fetchData();
                } catch (e) {
                    message.error("Error al eliminar: " + e);
                }
            },
        });
    };

    const columns = [
        {
            title: "Cliente",
            key: "name",
            render: (_, record) => record.name,
        },
        {
            title: "Celular",
            key: "phoneNumber",
            render: (_, record) => record.phoneNumber,
        },
        {
            title: "Email",
            render: (_, record) => record.email,
            key: "email",
        },
        {
            title: "Empresa",
            render: (_, record) => record.business,
            key: "business",
        },
        {
            title: "Acciones",
            key: "action",
            fixed: 'right', // Se queda fija al hacer scroll lateral en móvil
            width: 80,
            render: (_, record) => (
                <span className='flex'>
                    <Button 
                        icon={<EditOutlined />}
                        onClick= {() => {
                            setSelectedRecord(record);
                            setIsModalOpen(true);
                        }}
                    />
                    <Button 
                        danger
                        icon={<DeleteOutlined />}
                        onClick= {() => handleDelete(record)}
                    />
                </span>
            ),
        },
    ];

     // Función para obtener datos
    const fetchData = async () => {
        setLoading(true);
        try {
            const command = new SearchCommand({});
            const data = await customerService.search(command);
            setDataSource(data);
        } catch (error) {
            message.error("Error al cargar los datos: " + error);
        } finally {
            setLoading(false);
        }
    };

    // useEffect con array vacío [] se ejecuta solo UNA VEZ al montar el componente
    useEffect(() => {
        fetchData();
    }, []);

    return (
        <div>
            <Card title="Clientes" style={{ marginTop: 5 }}>
                <Button
                    type='primary'
                    style={{ marginBottom: 10 }}
                    onClick={addClient}
                >
                    Agregar
                </Button>
                <Table
                    columns={columns}
                    dataSource={dataSource}
                    loading={loading}
                    rowKey="id"
                    pagination={{ pageSize: 10 }}
                />
            </Card>

            <ClientModal 
                open={isModalOpen}
                initialValues={selectedRecord}
                onCancel={handleCancel}
                onSuccess={handleSuccess}
            />
        </div>
    )
}

export default Clients