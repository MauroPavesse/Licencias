import { Button, Card, Table, message, Modal } from 'antd'
import {
  EditOutlined,
  DeleteOutlined,
  ExclamationCircleOutlined
} from "@ant-design/icons";
import React, { useState, useEffect } from 'react'
import { extraService } from '../services/extraService';
import { SearchCommand } from "../DTOs/SearchCommand";
import ExtraModal from '../components/ExtraModal';

const Extras = () => {
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedRecord, setSelectedRecord] = useState(null);
    const [dataSource, setDataSource] = useState([]);
    const [loading, setLoading] = useState(false);

    const { confirm } = Modal;

    const handleCancel = () => {
        setIsModalOpen(false);
        setSelectedRecord(null);
    };

    const addExtra = () => {
        setSelectedRecord(null);
        setIsModalOpen(true);
    };

    const handleSuccess = () => {
        setIsModalOpen(false);
        fetchData();
    };

    const handleDelete = (record) => {
        confirm({
            title: "¿Estás seguro de eliminar este extra?",
            icon: <ExclamationCircleOutlined />,
            content: `Extra: ${record.name}`,
            okText: "Sí, eliminar",
            okType: "danger",
            cancelText: "Cancelar",
            onOk: async () => {
                try {
                    await extraService.delete(record.id);
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
            title: "Extra",
            key: "name",
            render: (_, record) => record.name,
        },
        {
            title: "Descripción",
            key: "description",
            render: (_, record) => record.description,
        },
        { 
            title: 'Importe', 
            key: 'price', 
            render: (_, record) => `$${record.price}` 
        },
        {
            title: "Acciones",
            key: "action",
            fixed: 'right',
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
            const data = await extraService.search(command);
            setDataSource(data);
        } catch (error) {
            message.error("Error al cargar los datos: " + error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchData();
    }, []);

    return (
        <div>
            <Card title="Extras" style={{ marginTop: 5 }}>
                <Button
                    type='primary'
                    style={{ marginBottom: 10 }}
                    onClick={addExtra}
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

            <ExtraModal 
                open={isModalOpen}
                initialValues={selectedRecord}
                onCancel={handleCancel}
                onSuccess={handleSuccess}
            />
        </div>
    )
}

export default Extras