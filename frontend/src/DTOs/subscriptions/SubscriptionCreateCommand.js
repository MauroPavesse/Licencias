export class SubscriptionCreateCommand {
  constructor({
    startDate = null,
    expirationDate = null,
    state = 0,
    customerId = 0,
    productVersionId = 0,
    extras = []
  } = {}) {
    this.StartDate = startDate;
    this.ExpirationDate = expirationDate;
    this.State = state;
    this.CustomerId = customerId;
    this.ProductVersionId = productVersionId;
    this.Extras = extras.map(e => ({
      Name: e.Name || e.Description,
      Description: e.Description,
      Price: e.Price || 0
    }));
  }
}
