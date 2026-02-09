export class PaymentCreateCommand {
  constructor({
    amount = 0,
    paymentDate = null,
    period = null,
    subscriptionId = 0
  } = {}) {
    this.Amount = amount;
    this.PaymentDate = paymentDate;
    this.Period = period;
    this.SubscriptionId = subscriptionId;
  }
}