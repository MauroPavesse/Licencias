export class PaymentCreateCommand {
  constructor({
    Amount = 0,
    PaymentDate = null,
    Period = null,
    SubscriptionId = 0
  } = {}) {
    this.Amount = Amount;
    this.PaymentDate = PaymentDate;
    this.Period = Period;
    this.SubscriptionId = SubscriptionId;
  }
}