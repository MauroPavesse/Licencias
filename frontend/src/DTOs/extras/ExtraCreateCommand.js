export class ExtraCreateCommand {
  constructor({
    name = "", 
    description = "", 
    price = 0
  } = {}) {
    this.Name = name;
    this.Description = description;
    this.Price = price;
  }
}