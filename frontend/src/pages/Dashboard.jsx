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
      fixed: 'left',
      with: 150,
      ellipsis: true,
      render: (_, record) => record.customer?.name || "N/A",
    },
    {
      title: "Producto",
      key: "product",
      width: 150,
      ellipsis: true,
      render: (_, record) => record.productVersion?.product?.name || "N/A",
    },
    {
      title: "Importe",
      key: "amountTotal",
      width: 110,
      align: "right",
      render: (amount) => (
        <span style={{ fontWeight: 'bold' }}>
          ${amount.amountTotal.toLocaleString()}
        </span>
      ),
    },
    {
      title: "F. Expiración",
      dataIndex: "expirationDate",
      key: "expirationDate",
      width: 120,
      align: "center",
      render: (date) => (date ? new Date(date).toLocaleDateString() : "-"),
    },
    {
      title: "Estado",
      dataIndex: "stateName",
      key: "state",
      width: 100,
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
            icon={<MoreOutlined style={{ fontSize: '18px' }} />}
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
    <div style={{ padding: window.innerWidth < 768 ? "1rem" : "2rem" }}>
      <Row gutter={[16, 16]}> {/* gutter con segundo valor añade espacio vertical al apilarse */}
        <Col xs={24} sm={8}>
          <Card variant="borderless">
            <Statistic
              title="Total mes"
              value={totalMes}
              precision={2}
              valueStyle={{ color: "#3f8600", fontSize: 'clamp(18px, 4vw, 24px)' }} // Tamaño de fuente adaptativo
              prefix={<DollarOutlined />}
            />
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card variant="borderless">
            <Statistic
              title="Impagas"
              value={impagas}
              valueStyle={{ color: "#f1164dff", fontSize: 'clamp(18px, 4vw, 24px)' }}
              prefix={<ExceptionOutlined />}
            />
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card variant="borderless">
            <Statistic
              title="Pagas"
              value={pagas}
              valueStyle={{ color: "#1699f1ff", fontSize: 'clamp(18px, 4vw, 24px)' }}
              prefix={<FileDoneOutlined />}
            />
          </Card>
        </Col>
      </Row>

      <Card
        title="Subscripciones"
        style={{ marginTop: 20 }}
        styles={{ body: { padding: window.innerWidth < 768 ? "8px" : "24px" } }}
      >
        <Button
          type="primary"
          style={{ marginBottom: 16 }}
          onClick={addSubscription}
          block={window.innerWidth < 768}
        >
          Agregar Subscripción
        </Button>

        <Table
          columns={columns}
          dataSource={dataSource}
          loading={loading}
          rowKey="id" // Asegúrate de que sea "id" en minúscula si así viene del backend
          pagination={{ pageSize: 10, size: 'small' }}
          scroll={{ x: 700 }} // Permite scroll horizontal sin romper el layout
        />
      </Card>

      <SubscriptionModal
        open={isModalOpen}
        onCancel={() => setIsModalOpen(false)}
        onSuccess={handleSuccess}
        initialValues={selectedRecord}
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
