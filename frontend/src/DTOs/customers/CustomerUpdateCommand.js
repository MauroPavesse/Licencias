export class CustomerUpdateCommand {
  constructor(id = 0, name = "", email = "", phoneNumber = "", business = "") {
    this.Id = id,
    this.Name = name,
    this.email = email,
    this.PhoneNumber = phoneNumber,
    this.Business = business
  }
}