import { Modal, Table, Button, Space, message, Tag } from "antd";
import { PlusOutlined, DollarCircleOutlined } from "@ant-design/icons";
import { useState, useEffect } from "react";
import { paymentService } from "../services/paymentService";
import { SearchCommand } from "../DTOs/SearchCommand";
import NewPaymentModal from "./NewPaymentModal";
import dayjs from "dayjs";

const HistoryPaymentModal = ({ open, onCancel, subscriptionId, customerName }) => {
  const [dataSource, setDataSource] = useState([]);
  const [loading, setLoading] = useState(false);
  const [isNewPaymentModalOpen, setIsNewPaymentModalOpen] = useState(false);

  // Cargar pagos al abrir
  useEffect(() => {
    if (open && subscriptionId) {
      fetchPayments();
    }
  }, [open, subscriptionId]);

  const fetchPayments = async () => {
    setLoading(true);
    try {
      // Filtramos por subscriptionId en el backend
      const command = new SearchCommand({
        filters: [{ field: "SubscriptionId", ids: [subscriptionId] }]
      });
      const data = await paymentService.search(command);
      setDataSource(data);
    } catch (error) {
      message.error("Error al cargar el historial de pagos: " + error);
    } finally {
      setLoading(false);
    }
  };

  const handleSuccess = () => {
    setIsNewPaymentModalOpen(false); // Cierra el modal
    fetchPayments(); // Recarga la tabla con el nuevo dato
  };

  const columns = [
    {
      title: "Fecha de pago",
      dataIndex: "paymentDate",
      key: "paymentDate",
      render: (date) => new Date(date).toLocaleDateString(),
    },
    {
      title: "Importe",
      dataIndex: "amount",
      key: "amount",
      render: (amount) => (
        <span style={{ fontWeight: 'bold', color: '#3f8600' }}>
          ${amount.toLocaleString()}
        </span>
      ),
    },
    {
      title: "Periodo",
      dataIndex: "period", // Agregamos dataIndex para obtener el valor directamente
      key: "period",
      render: (date) => date ? dayjs(date).format("MM/YYYY") : "-",
    }
  ];

  return (
    <Modal
      title={
        <Space>
          <DollarCircleOutlined />
          <span>Historial de Pagos - {customerName}</span>
        </Space>
      }
      open={open}
      onCancel={onCancel}
      width={700}
      footer={[
        <Button key="close" onClick={onCancel}>
          Cerrar
        </Button>
      ]}
    >
      <div style={{ marginBottom: 16, display: 'flex', justifyContent: 'right' }}>
        <Button
          type="primary"
          icon={<PlusOutlined />}
          onClick={() => setIsNewPaymentModalOpen(true)}
        >
          Registrar Pago
        </Button>
      </div>

      <Table
        dataSource={dataSource}
        columns={columns}
        loading={loading}
        rowKey="id"
        pagination={{ pageSize: 5 }}
        size="small"
        bordered
      />

      <NewPaymentModal
        open={isNewPaymentModalOpen}
        onCancel={() => setIsNewPaymentModalOpen(false)}
        onSuccess={handleSuccess}
        subscriptionId={subscriptionId}
      />
    </Modal>
  );
};

export default HistoryPaymentModal;