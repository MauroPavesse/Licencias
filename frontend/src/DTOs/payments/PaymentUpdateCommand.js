export class PaymentUpdateCommand {
  constructor({
    id = 0,
    amount = 0,
    paymentDate = null,
    period = null,
    subscriptionId = 0
  } = {}) {
    this.Id = id;
    this.Amount = amount;
    this.PaymentDate = paymentDate;
    this.Period = period;
    this.SubscriptionId = subscriptionId;
  }
}