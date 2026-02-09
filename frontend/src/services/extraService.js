import api from "./api";
import { SearchCommand } from "../DTOs/SearchCommand";
import { ExtraCreateCommand } from "../DTOs/extras/ExtraCreateCommand";
import { ExtraUpdateCommand } from "../DTOs/extras/ExtraUpdateCommand";

export const extraService = {
  search: async (params) => {
    const body = new SearchCommand(params);

    const response = await api.post("/extra/search", body);
    return response.data;
  },

  create: async (params) => {
    const body = new ExtraCreateCommand(params);

    const response = await api.post("/extra", body);
    return response.data;
  },

  update: async (params) => {
    const body = new ExtraUpdateCommand(params);

    const response = await api.put("/extra", body);
    return response.data;
  },

  delete: async (id) => {
    const response = await api.delete(`/extra/${id}`);
    return response.data;
  },
};
