export class CustomerUpdateCommand {
  constructor({
    Id = 0,
    Name = "", 
    Email = "", 
    PhoneNumber = "", 
    Business = ""
  } = {}) {
    this.Id = Id;
    this.Name = Name;
    this.Email = Email;
    this.PhoneNumber = PhoneNumber;
    this.Business = Business;
  }
}