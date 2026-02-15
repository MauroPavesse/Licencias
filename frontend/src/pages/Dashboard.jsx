import { Modal, Menu, Dropdown, Card, Col, Row, Statistic, Table, message, Button } from "antd";
import {
  EditOutlined, 
  HistoryOutlined,
  ExclamationCircleOutlined,
  DollarOutlined,
  DeleteOutlined,
  ExceptionOutlined,
  FileDoneOutlined,
  MoreOutlined,
} from "@ant-design/icons";
import { useEffect, useState } from "react";
import { subscriptionService } from "../services/subscriptionService";
import { SearchCommand } from "../DTOs/SearchCommand";
import SubscriptionModal from "../components/SubscriptionModal";
import HistoryPaymentModal from "../components/HistoryPaymentModal";

const Dashboard = () => {
  const [dataSource, setDataSource] = useState([]);
  const [loading, setLoading] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedRecord, setSelectedRecord] = useState(null);
  const [isHistoryModalOpen, setIsHistoryModalOpen] = useState(false);

  const { confirm } = Modal;

  const handleDelete = (record) => {
    confirm({
      title: "¿Estás seguro de eliminar esta subscripción?",
      icon: <ExclamationCircleOutlined />,
      content: `Cliente: ${record.customer?.name}`,
      okText: "Sí, eliminar",
      okType: "danger",
      cancelText: "Cancelar",
      onOk: async () => {
        try {
          await subscriptionService.delete(record.id);
          message.success("Eliminado correctamente");
          fetchData(); // Refrescar tabla
        } catch (e) {
          message.error("Error al eliminar: " + e);
        }
      },
    });
  };

  const menuItems = (record) => [
    {
      key: "edit",
      label: "Editar",
      icon: <EditOutlined />,
      onClick: () => {
        setSelectedRecord(record);
        setIsModalOpen(true);
      },
    },
    {
      key: "history",
      label: "Historial de pagos",
      icon: <HistoryOutlined />,
      onClick: () => {
        setSelectedRecord(record);
        setIsHistoryModalOpen(true);
      },
    },
    {
      type: "divider",
    },
    {
      key: "delete",
      label: "Eliminar",
      icon: <DeleteOutlined />,
      danger: true,
      onClick: () => handleDelete(record),
    }
  ];

  // Función para obtener datos
  const fetchData = async () => {
    setLoading(true);
    try {
      const command = new SearchCommand({});
      const data = await subscriptionService.search(command);
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

  const columns = [
    {
      title: "Cliente",
      key: "customer",
      // Acceso seguro a objetos anidados
      render: (_, record) => record.customer?.name || "N/A",
    },
    {
      title: "Producto",
      key: "product",
      render: (_, record) => record.productVersion?.product?.name || "N/A",
    },
    {
      title: "Importe",
      render: (_, record) => `$ ${record.amountTotal.toFixed(2)}`,
      key: "amountTotal",
      align: "right",
    },
    {
      title: "F. Expiración",
      dataIndex: "expirationDate", // minúscula inicial
      key: "expirationDate",
      render: (date) => (date ? new Date(date).toLocaleDateString() : "-"),
      align: "center",
    },
    {
      title: "Estado",
      dataIndex: "stateName", // minúscula inicial
      key: "state",
      align: "center",
    },
    {
    title: "Acciones",
    key: "action",
    fixed: 'right', // Se queda fija al hacer scroll lateral en móvil
    width: 80,
    render: (_, record) => (
      <Dropdown 
        menu={{ items: menuItems(record) }} 
        trigger={['click']}
        placement="bottomRight"
      >
        <Button 
          type="text" 
          icon={<MoreOutlined style={{ fontSize: '20px' }} />} 
        />
      </Dropdown>
    ),
  },
  ];

  const addSubscription = () => {
    setIsModalOpen(true);
  };

  const handleSuccess = () => {
    setIsModalOpen(false); // Cierra el modal
    fetchData(); // Recarga la tabla con el nuevo dato
  };

  // 1. Sumar el importe total de todas las suscripciones cargadas
  const totalMes = dataSource.reduce(
    (acc, curr) => acc + (curr.amountTotal || 0),
    0,
  );

  // 2. Obtener la fecha actual para comparar
  const ahora = new Date();

  // 3. Filtrar impagas (Expiración menor o igual a hoy)
  const impagas = dataSource.filter((s) => {
    const fechaExp = new Date(s.expirationDate);
    return fechaExp.getMonth() <= ahora.getMonth();
  }).length;

  // 4. Filtrar pagas/vigentes (Expiración mayor a hoy)
  const pagas = dataSource.filter((s) => {
    const fechaExp = new Date(s.expirationDate);
    return fechaExp.getMonth() > ahora.getMonth();
  }).length;

  return (
    <div style={{ padding: "2rem" }}>
      <Row gutter={16}>
        <Col xs={24} sm={8} md={8}>
          <Card variant="borderless" style={{ marginBottom: 5 }}>
            <Statistic
              title="Total mes"
              value={totalMes}
              precision={2}
              styles={{ content: { color: "#3f8600" } }}
              prefix={<DollarOutlined />}
            />
          </Card>
        </Col>
        <Col xs={24} sm={8} md={8}>
          <Card variant="borderless" style={{ marginBottom: 5 }}>
            <Statistic
              title="Subscripciones impagas"
              value={impagas} // Subscripcion cuyo mes es menor o igual al mes actual
              styles={{ content: { color: "#f1164dff" } }}
              prefix={<ExceptionOutlined />}
            />
          </Card>
        </Col>
        <Col xs={24} sm={8} md={8}>
          <Card variant="borderless" style={{ marginBottom: 5 }}>
            <Statistic
              title="Subscripciones pagas"
              value={pagas} // Subscripcion cuyo mes es mayor al mes actual
              styles={{ content: { color: "#1699f1ff" } }}
              prefix={<FileDoneOutlined />}
            />
          </Card>
        </Col>
      </Row>

      <Card title="Subscripciones" style={{ marginTop: 5 }}>
        <Button
          type="primary"
          style={{ marginBottom: 10 }}
          onClick={addSubscription}
        >
          Agregar
        </Button>
        <Table
          columns={columns}
          dataSource={dataSource}
          loading={loading}
          rowKey="Id"
          pagination={{ pageSize: 10 }}
        />
      </Card>

      <SubscriptionModal
        open={isModalOpen}
        onCancel={() => setIsModalOpen(false)}
        onSuccess={handleSuccess}
      />

      <HistoryPaymentModal
        open={isHistoryModalOpen}
        onCancel={() => setIsHistoryModalOpen(false)}
        subscriptionId={selectedRecord?.id}
        customerName={selectedRecord?.customer?.name}
      />
    </div>
  );
};

export default Dashboard;
