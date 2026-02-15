export class ExtraCreateCommand {
  constructor({
    Name = "", 
    Description = "", 
    Price = 0
  } = {}) {
    this.Name = Name;
    this.Description = Description;
    this.Price = Price;
  }
}