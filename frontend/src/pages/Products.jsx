import { Button, Card, Table, message, Modal } from 'antd'
import {
    EditOutlined,
    DeleteOutlined,
    ExclamationCircleOutlined
} from "@ant-design/icons";
import React, { useState, useEffect } from 'react'
import ProductModal from '../components/ProductModal';
import { productService } from '../services/productService';
import { SearchCommand } from "../DTOs/SearchCommand";

const Products = () => {
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedRecord, setSelectedRecord] = useState(null);
    const [dataSource, setDataSource] = useState([]);
    const [loading, setLoading] = useState(false);

    const currencyFormatter = new Intl.NumberFormat("es-AR", {
        style: "currency",
        currency: "ARS",
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
    });

    const { confirm } = Modal;

    const handleCancel = () => {
        setIsModalOpen(false);
        setSelectedRecord(null);
    };

    const addProduct = () => {
        setSelectedRecord(null);
        setIsModalOpen(true);
    };

    const handleSuccess = () => {
        setIsModalOpen(false);
        fetchData();
    };

    const handleDelete = (record) => {
        confirm({
            title: "¿Estás seguro de eliminar este producto?",
            icon: <ExclamationCircleOutlined />,
            content: `Producto: ${record.name}`,
            okText: "Sí, eliminar",
            okType: "danger",
            cancelText: "Cancelar",
            onOk: async () => {
                try {
                    await productService.delete(record.id);
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
            title: "Producto",
            key: "name",
            render: (_, record) => record.name,
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
                        onClick={() => {
                            setSelectedRecord(record);
                            setIsModalOpen(true);
                        }}
                    />
                    <Button
                        danger
                        icon={<DeleteOutlined />}
                        onClick={() => handleDelete(record)}
                    />
                </span>
            ),
        },
    ];

    const expandedRowRender = (record) => {
        const subColumns = [
            { title: 'Versión', dataIndex: 'version', key: 'name', render: (_, record) => record.name },
            { title: 'Descripción', dataIndex: 'description', key: 'description', render: (_, record) => record.description },
            { title: 'Importe', dataIndex: 'price', key: 'price', render: (val) => currencyFormatter.format(val) },
        ];

        return (
            <Table
                columns={subColumns}
                dataSource={record.productVersions} // Aquí asumo que cada producto tiene un array 'details'
                pagination={false} // Normalmente no se usa paginación en sub-tablas
                rowKey="id"
            />
        );
    };

    // Función para obtener datos
    const fetchData = async () => {
        setLoading(true);
        try {
            const command = new SearchCommand({});
            const data = await productService.search(command);
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
            <Card title="Productos" style={{ marginTop: 5 }}>
                <Button
                    type='primary'
                    style={{ marginBottom: 10 }}
                    onClick={addProduct}
                >
                    Agregar
                </Button>
                <Table
                    columns={columns}
                    dataSource={dataSource}
                    loading={loading}
                    rowKey="id"
                    pagination={{ pageSize: 10 }}
                    expandable={{
                        expandedRowRender, // La función que creamos arriba
                        rowExpandable: (record) => record.productVersions?.length > 0, // Opcional: solo expande si tiene datos
                    }}
                />
            </Card>

            <ProductModal
                open={isModalOpen}
                initialValues={selectedRecord}
                onCancel={handleCancel}
                onSuccess={handleSuccess}
            />
        </div>
    )
}

export default Products