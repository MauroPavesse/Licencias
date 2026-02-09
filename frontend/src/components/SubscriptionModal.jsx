import { Modal, Form, DatePicker, Select, message, Row, Col, Divider, Button, Table, InputNumber } from "antd";
import { PlusOutlined, DeleteOutlined } from "@ant-design/icons"; // Faltaba importar DeleteOutlined
import { useState, useEffect } from "react";
import { subscriptionService } from "../services/subscriptionService";
import { customerService } from "../services/customerService";
import { productVersionService } from "../services/productVersionService";
import { productService } from "../services/productService";
import { SearchCommand } from "../DTOs/SearchCommand";

const SubscriptionModal = ({ open, onCancel, onSuccess }) => {
  const [form] = Form.useForm();
  const [confirmLoading, setConfirmLoading] = useState(false);

  const [customers, setCustomers] = useState([]);
  const [products, setProducts] = useState([]);
  const [allVersions, setAllVersions] = useState([]);
  const [filteredVersions, setFilteredVersions] = useState([]);
  const [loadingLists, setLoadingLists] = useState(false);
  const [extras, setExtras] = useState([]);

  useEffect(() => {
    if (open) {
      loadSelectData();
      setExtras([]);
      form.resetFields();
    }
  }, [open]);

  const loadSelectData = async () => {
    setLoadingLists(true);
    try {
      const command = new SearchCommand(); 
      const [resCust, resProd, resVers] = await Promise.all([
        customerService.search(command),
        productService.search(command),
        productVersionService.search(command),
      ]);
      
      // Ajuste camelCase: resCust, resProd, resVers vienen del backend
      setCustomers(resCust);
      setProducts(resProd);
      setAllVersions(resVers);
    } catch (error) {
      message.error("Error al cargar listas: " + error.message);
    } finally {
      setLoadingLists(false);
    }
  };

  const handleProductChange = (productId) => {
    form.setFieldValue("productVersionId", null);
    // Ajuste camelCase: v.productId en minúscula
    const filtered = allVersions.filter((v) => v.productId === productId);
    setFilteredVersions(filtered);
  };

  const addExtra = () => {
    const newExtra = {
      key: Date.now(),
      name: "",        // Usamos minúsculas aquí también para consistencia
      description: "",
      price: 0,
    };
    setExtras([...extras, newExtra]);
  };

  const removeExtra = (key) => {
    setExtras(extras.filter((item) => item.key !== key));
  };

  const updateExtra = (key, field, value) => {
    const newData = extras.map((item) =>
      item.key === key ? { ...item, [field]: value } : item
    );
    setExtras(newData);
  };

  const columnsExtras = [
    {
      title: "Descripción / Nombre",
      dataIndex: "description",
      render: (_, record) => (
        <input
          className="border p-1 w-full rounded outline-none focus:ring-1 focus:ring-blue-400"
          placeholder="Ej: Instalación"
          onChange={(e) => updateExtra(record.key, "description", e.target.value)}
        />
      ),
    },
    {
      title: "Precio",
      dataIndex: "price",
      width: 120,
      render: (_, record) => (
        <InputNumber
          min={0}
          className="w-full"
          onChange={(val) => updateExtra(record.key, "price", val)}
        />
      ),
    },
    {
      title: "",
      width: 50,
      render: (_, record) => (
        <Button
          type="text"
          danger
          icon={<DeleteOutlined />}
          onClick={() => removeExtra(record.key)}
        />
      ),
    },
  ];

  const handleOk = async () => {
    try {
      const values = await form.validateFields();
      setConfirmLoading(true);

      const payload = {
        StartDate: values.dates[0].format("YYYY-MM-DDTHH:mm:ss"),
        ExpirationDate: values.dates[1].format("YYYY-MM-DDTHH:mm:ss"),
        State: values.state,
        CustomerId: values.customerId,
        ProductVersionId: values.productVersionId,
        // Mapeo final para el backend (PascalCase para que coincida con C#)
        Extras: extras.map(e => ({
          Name: e.description, // Usamos la descripción como Nombre
          Description: e.description,
          Price: e.price
        }))
      };

      await subscriptionService.create(payload);
      message.success("Suscripción creada");
      onSuccess();
    } catch (error) {
      if (!error.errorFields) message.error("Error al guardar");
    } finally {
      setConfirmLoading(false);
    }
  };

  return (
    <Modal
      title="Nueva Suscripción"
      open={open}
      onOk={handleOk}
      confirmLoading={confirmLoading}
      onCancel={onCancel}
      destroyOnClose
      width={650}
    >
      <Form form={form} layout="vertical">
        <Row gutter={16}>
          <Col span={14}>
            <Form.Item
              label="Vigencia"
              name="dates"
              rules={[{ required: true, message: "Selecciona las fechas" }]}
            >
              <DatePicker.RangePicker className="w-full" />
            </Form.Item>
          </Col>
          <Col span={10}>
            <Form.Item label="Estado" name="state" initialValue={1}>
              <Select options={[
                { value: 1, label: 'Activo' },
                { value: 2, label: 'Vencido' },
                { value: 3, label: 'Deshabilitado' },
                { value: 4, label: 'Inactivo' },
              ]} />
            </Form.Item>
          </Col>
        </Row>

        <Form.Item label="Cliente" name="customerId" rules={[{ required: true }]}>
          <Select
            placeholder="Seleccione un cliente"
            loading={loadingLists}
            showSearch
            optionFilterProp="label"
            // Ajuste camelCase: c.id y c.name
            options={customers.map((c) => ({ value: c.id, label: c.name }))}
          />
        </Form.Item>

        <Row gutter={16}>
          <Col span={12}>
            <Form.Item label="Producto" name="productId" rules={[{ required: true }]}>
              <Select
                placeholder="Seleccione producto"
                // Ajuste camelCase: p.id y p.name
                options={products.map((p) => ({ value: p.id, label: p.name }))}
                onChange={handleProductChange}
              />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item label="Versión" name="productVersionId" rules={[{ required: true }]}>
              <Select
                placeholder="Seleccione versión"
                disabled={filteredVersions.length === 0}
                // Ajuste camelCase: v.id y v.name
                options={filteredVersions.map((v) => ({ value: v.id, label: v.name }))}
              />
            </Form.Item>
          </Col>
        </Row>

        <Divider orientation="left" className="text-gray-400 text-xs">EXTRAS</Divider>
        
        <Table
          dataSource={extras}
          columns={columnsExtras}
          pagination={false}
          size="small"
          className="mb-4"
          locale={{ emptyText: "Sin cargos adicionales" }}
        />
        
        <Button
          onClick={addExtra}
          type="dashed"
          block
          icon={<PlusOutlined />}
        >
          Agregar Item Extra
        </Button>
      </Form>
    </Modal>
  );
};

export default SubscriptionModal;