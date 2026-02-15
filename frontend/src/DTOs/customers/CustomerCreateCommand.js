export class CustomerCreateCommand {
  constructor({
    Name = "", 
    Email = "", 
    PhoneNumber = "", 
    Business = ""
  } = {}) {
    this.Name = Name;
    this.Email = Email;
    this.PhoneNumber = PhoneNumber;
    this.Business = Business;
  }
}