import { message, Button, Divider, Form, Input, Modal, Table, InputNumber } from 'antd'
import { useState, useEffect } from 'react'
import { productService } from '../services/productService';
import { PlusOutlined, DeleteOutlined } from "@ant-design/icons";

const ProductModal = ({ open, onCancel, onSuccess, initialValues }) => {
  const [form] = Form.useForm();
  const [confirmLoading, setConfirmLoading] = useState(false);
  const [versions, setVersions] = useState([]);
  const [loadingLists, setLoadingLists] = useState(false);

  useEffect(() => {
    if (open) {
      if (initialValues) {
        form.setFieldsValue(initialValues)
        setVersions(initialValues.productVersions)
      } else {
        form.resetFields();
      }
    }
  }, [open]);

  const addVersion = () => {
    const newVersion = {
      key: Date.now(),
      name: "",        // Usamos minúsculas aquí también para consistencia
      description: "",
    };
    setVersions([...versions, newVersion]);
  };

  const handleOk = async () => {
    try {
      const values = await form.validateFields();

      // Validar que al menos haya una versión (opcional)
      if (versions.length === 0) {
        return message.warning("Debes agregar al menos una versión");
      }

      setConfirmLoading(true);

      const payload = {
        Id: initialValues?.id ? initialValues.id : 0,
        Name: values.name,
        Description: values.description,
        ProductVersions: versions.map(v => ({
          Name: v.name,      // Coincide con 'string Name' del record C#
          Description: v.description || "",
          Price: v.price || 0,
          ProductId: 0          // El ID será asignado por la DB al crear el padre
        }))
      };

      if (initialValues?.id) {
        await productService.update(payload);
        message.success("Producto actualizado");
      } else {
        await productService.create(payload);
        message.success("Producto creado");
      }

      setVersions([]); // Limpiar versiones
      onSuccess();
    } catch (error) {
      console.error(error);
      message.error("Error al guardar");
    } finally {
      setConfirmLoading(false);
    }
  };

  const updateVersion = (key, field, value) => {
    const newData = versions.map((item) =>
      item.key === key ? { ...item, [field]: value } : item
    );
    setVersions(newData);
  };

  const removeVersion = (key) => {
    setVersions(versions.filter((item) => item.key !== key));
  };

  const columnsVersions = [
    // Dentro de columnsVersions
    {
      title: "Version",
      dataIndex: "name",
      render: (value, record) => ( // Cambiamos _ por value
        <Input
          value={value} // Importante para que el input sea controlado
          placeholder="Ej: Instalación"
          onChange={(e) => updateVersion(record.key, "name", e.target.value)}
        />
      ),
    },
    {
      title: "Precio",
      dataIndex: "price",
      render: (value, record) => (
        <InputNumber
          value={value} // Importante
          min={0}
          className="w-full"
          onChange={(val) => updateVersion(record.key, "price", val)}
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
          onClick={() => removeVersion(record.key)}
        />
      ),
    },
  ];

  return (
    <Modal
      title="Nuevo Producto"
      open={open}
      onOk={handleOk}
      confirmLoading={confirmLoading}
      onCancel={onCancel}
    >
      <Form form={form} layout="vertical">
        <Form.Item label='Producto' name='name'>
          <Input
            placeholder='Nombre del Producto'
            required
          />
        </Form.Item>
        <Form.Item label='Descripcion' name='description'>
          <Input
            placeholder='Descripcion del Producto'
          />
        </Form.Item>

        <Divider titlePlacement="left" className="text-gray-400 text-xs">
          VERSIONES
        </Divider>

        <Table
          dataSource={versions}
          columns={columnsVersions}
          pagination={false}
          size="small"
          className="mb-4"
          locale={{ emptyText: "Sin versiones" }}
          rowKey={(record) => record.id || record.key}
        />

        <Button
          onClick={addVersion}
          type="dashed"
          block
          icon={<PlusOutlined />}
        >
          Agregar Version
        </Button>
      </Form>
    </Modal>
  )
}

export default ProductModal