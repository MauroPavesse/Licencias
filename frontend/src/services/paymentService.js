import api from "./api";
import { SearchCommand } from "../DTOs/SearchCommand";
import { PaymentCreateCommand } from "../DTOs/payments/PaymentCreateCommand";
import { PaymentUpdateCommand } from "../DTOs/payments/PaymentUpdateCommand";

export const paymentService = {
  search: async (params) => {
    const body = new SearchCommand(params);

    const response = await api.post("/payment/search", body);
    return response.data;
  },

  create: async (params) => {
    const body = new PaymentCreateCommand(params);

    const response = await api.post("/payment", body);
    return response.data;
  },

  update: async (params) => {
    const body = new PaymentUpdateCommand(params);

    const response = await api.put("/payment", body);
    return response.data;
  },

  delete: async (id) => {
    const response = await api.delete(`/payment/${id}`);
    return response.data;
  },
};
