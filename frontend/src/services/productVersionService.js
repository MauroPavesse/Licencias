import api from "./api";
import { SearchCommand } from "../DTOs/SearchCommand";
import { ProductVersionCreateCommand } from "../DTOs/productsVersions/ProductVersionCreateCommand";
import { ProductVersionUpdateCommand } from "../DTOs/productsVersions/ProductVersionUpdateCommand";

export const productVersionService = {
  search: async (params) => {
    const body = new SearchCommand(params);

    const response = await api.post("/productVersion/search", body);
    return response.data;
  },

  create: async (params) => {
    const body = new ProductVersionCreateCommand(params);

    const response = await api.post("/productVersion", body);
    return response.data;
  },

  update: async (params) => {
    const body = new ProductVersionUpdateCommand(params);

    const response = await api.put("/productVersion", body);
    return response.data;
  },

  delete: async (id) => {
    const response = await api.delete(`/productVersion/${id}`);
    return response.data;
  },
};
