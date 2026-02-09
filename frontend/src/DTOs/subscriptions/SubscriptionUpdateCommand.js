export class SubscriptionUpdateCommand {
  constructor({
    id = 0,
    startDate = null,
    expirationDate = null,
    state = 0,
    customerId = 0,
    productVersionId = 0,
  } = {}) {
    this.Id = id;
    this.StartDate = startDate;
    this.ExpirationDate = expirationDate;
    this.State = state;
    this.CustomerId = customerId;
    this.ProductVersionId = productVersionId;
  }
}
