export class ProductVersionUpdateCommand {
  constructor({
    id = 0,
    name = "", 
    description = "", 
    price = 0,
    productId = 0
  } = {}) {
    this.Id = id;
    this.Name = name;
    this.Description = description;
    this.Price = price;
    this.ProductId = productId;
  }
}