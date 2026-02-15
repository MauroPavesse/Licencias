import api from "./api";
import { SearchCommand } from "../DTOs/SearchCommand";
import { ProductCreateCommand } from "../DTOs/products/ProductCreateCommand";
import { ProductUpdateCommand } from "../DTOs/products/ProductUpdateCommand";

export const productService = {
  search: async (params) => {
    const body = new SearchCommand(params);
    const response = await api.post("/product/search", body);
    return response.data;
  },

  create: async (params) => {
    const body = new ProductCreateCommand(params);
    const response = await api.post("/product", body);
    return response.data;
  },

  update: async (params) => {
    const body = new ProductUpdateCommand(params);
    const response = await api.put("/product", body);
    return response.data;
  },

  delete: async (id) => {
    const response = await api.delete(`/product/${id}`);
    return response.data;
  },
};
