import api from "./api";
import { SearchCommand } from "../DTOs/SearchCommand";
import { CustomerCreateCommand } from "../DTOs/customers/CustomerCreateCommand";
import { CustomerUpdateCommand } from "../DTOs/customers/CustomerUpdateCommand";

export const customerService = {
  search: async (params) => {
    const body = new SearchCommand(params);

    const response = await api.post("/customer/search", body);
    return response.data;
  },

  create: async (params) => {
    const body = new CustomerCreateCommand(params);

    const response = await api.post("/customer", body);
    return response.data;
  },

  update: async (params) => {
    const body = new CustomerUpdateCommand(params);
    const response = await api.put("/customer", body);
    return response.data;
  },

  delete: async (id) => {
    const response = await api.delete(`/customer/${id}`);
    return response.data;
  },
};
