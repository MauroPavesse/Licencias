import { Col, Form, Input, Modal, message, InputNumber } from 'antd'
import React, { useState, useEffect } from 'react'
import { extraService } from '../services/extraService';

const ExtraModal = ({open, onCancel, onSuccess, initialValues}) => {
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
                Description: values.description,
                Price: values.price
            };

            if (initialValues?.id) {
                await extraService.update(payload);
                message.success("Extra actualizado");
            } else {
                await extraService.create(payload);
                message.success("Extra creado");
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
        title="Nuevo Extra"
        open={open}
        onOk={handleOk}
        confirmLoading={confirmLoading}
        onCancel={onCancel}
    >
        <Form form={form} layout="vertical">
            <Form.Item label='Extra' name='name'>
                <Input 
                    placeholder='Nombre del Extra'
                    required
                />
            </Form.Item>
            <Form.Item label="Descripcion" name='description'>
                <Input 
                    placeholder='Breve descripcion'
                />
            </Form.Item>
            <Form.Item label="Precio" name='price'>
                <InputNumber
                    min={0}
                    className="w-full"
                />
            </Form.Item>
        </Form>
    </Modal>
  )
}

export default ExtraModal