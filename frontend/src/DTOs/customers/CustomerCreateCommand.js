export class CustomerCreateCommand {
  constructor({
    name = "", 
    email = "", 
    phoneNumber = "", 
    business = ""
  } = {}) {
    this.Name = name;
    this.email = email;
    this.PhoneNumber = phoneNumber;
    this.Business = business;
  }
}