export class ExtraUpdateCommand {
  constructor({
    id = 0,
    name = "", 
    description = "", 
    price = 0
  } = {}) {
    this.Id = id;
    this.Name = name;
    this.Description = description;
    this.Price = price;
  }
}