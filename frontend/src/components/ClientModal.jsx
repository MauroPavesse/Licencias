import { Col, Form, Input, Modal, Row, message } from 'antd'
import React, { useState, useEffect } from 'react'
import { customerService } from '../services/customerService';

const ClientModal = ({open, onCancel, onSuccess, initialValues}) => {
    const [form] = Form.useForm();
    const [confirmLoading, setConfirmLoading] = useState(false);

    useEffect(() => {
        if (open) {
            if (initialValues) {
                form.setFieldsValue(initialValues);
            } else {
                form.resetFields();
            }
        }
    }, [open, initialValues, form]);

    const handleOk = async () => {
        try {
            const values = await form.validateFields();
            setConfirmLoading(true);

            const payload = {
                Id: initialValues?.id ? initialValues.id : 0,
                Name: values.name,
                Email: values.email,
                PhoneNumber: values.phoneNumber,
                Business: values.business
            };

            if (initialValues?.id) {
                await customerService.update(payload);
                message.success("Cliente actualizado");
            } else {
                await customerService.create(payload);
                message.success("Cliente creado");
            }
            
            form.resetFields();
            onSuccess();
        } catch (error) {
            if (!error.errorFields) message.error("Error al guardar");
        } finally {
            setConfirmLoading(false);
        }
    };

  return (
    <Modal
        title="Nuevo Cliente"
        open={open}
        onOk={handleOk}
        confirmLoading={confirmLoading}
        onCancel={onCancel}
    >
        <Form form={form} layout="vertical">
            <Form.Item label='Nombre' name='name'>
                <Input 
                    placeholder='Nombre del Cliente'
                    required
                />
            </Form.Item>
            <Row gutter={10}>
                <Col span={14}>
                    <Form.Item label='Email' name='email'>
                        <Input
                            placeholder='Correo electronico'
                        />
                    </Form.Item>
                </Col>
                <Col span={10}>
                    <Form.Item label="Celular" name='phoneNumber'>
                        <Input
                            placeholder='Celular'
                        />
                    </Form.Item>
                </Col>
            </Row>
            <Form.Item label="Empresa" name='business'>
                <Input 
                    placeholder='Nombre de la empresa'
                />
            </Form.Item>
        </Form>
    </Modal>
  )
}

export default ClientModal