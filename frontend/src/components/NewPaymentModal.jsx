import { useEffect, useState } from 'react';
import { paymentService } from '../services/paymentService'
import { Col, DatePicker, Form, InputNumber, Modal, Row, message } from 'antd';

const NewPaymentModal = ({ open, onCancel, onSuccess, subscriptionId }) => {
    const [form] = Form.useForm();
    const [confirmLoading, setConfirmLoading] = useState(false);

    useEffect(() => {
        if (open) {
            form.resetFields();
        }
    }, [open, form]);

    const handleOk = async () => {
        try {
            const values = await form.validateFields();
            setConfirmLoading(true);

            const payload = {
                Amount: values.amount,
                PaymentDate: values.paymentDate,
                Period: values.period,
                SubscriptionId: subscriptionId
            };

            await paymentService.create(payload);
            message.success("Pago realizado");
            
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
            title="Nuevo pago"
            open={open}
            onOk={handleOk}
            confirmLoading={confirmLoading}
            onCancel={onCancel}
        >
            <Form form={form} layout='vertical'>
                <Row gutter={10}>
                    <Col xs={24} sm={12} md={12}>
                        <Form.Item label='Fecha de pago' name='paymentDate'>
                            <DatePicker
                                style={{ width: '100%' }}
                            />
                        </Form.Item>
                    </Col>
                    <Col xs={24} sm={12} md={12}>
                        <Form.Item label='Periodo' name='period'>
                            <DatePicker
                                style={{ width: '100%' }}
                                picker="month"
                            />
                        </Form.Item>
                    </Col>
                </Row>
                <Form.Item label='Importe' name='amount'>
                    <InputNumber
                        style={{ width: '50%' }}
                        min={0}
                    />
                </Form.Item>
            </Form>
        </Modal>
    )
}

export default NewPaymentModal