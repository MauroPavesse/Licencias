import api from "./api";
import { SearchCommand } from "../DTOs/SearchCommand";
import { SubscriptionCreateCommand } from "../DTOs/subscriptions/SubscriptionCreateCommand";
import { SubscriptionUpdateCommand } from "../DTOs/subscriptions/SubscriptionUpdateCommand";

export const subscriptionService = {
  search: async (params) => {
    console.log("Buscando...");
    const body = new SearchCommand(params);

    const response = await api.post("/subscription/search", body);
    console.log(response);
    return response.data;
  },

  create: async (formValues) => {
    const response = await api.post("/subscription", formValues);
    return response.data;
  },

  update: async (formValues) => {
    const command = new SubscriptionUpdateCommand({
      id: formValues.id,
      startDate: formValues.dates[0].format('YYYY-MM-DDTHH:mm:ssZ'),
      expirationDate: formValues.dates[1].format('YYYY-MM-DDTHH:mm:ssZ'),
      state: formValues.state,
      customerId: formValues.customerId,
      productVersionId: formValues.productVersionId
    });

    const response = await api.put("/subscription", command);
    return response.data;
  },

  delete: async (id) => {
    const response = await api.delete(`/subscription/${id}`);
    return response.data;
  },
};
