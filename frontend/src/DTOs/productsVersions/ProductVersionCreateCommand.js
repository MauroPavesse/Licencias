export class ProductVersionCreateCommand {
  constructor({
    name = "", 
    description = "", 
    price = 0,
    productId = 0
  } = {}) {
    this.Name = name;
    this.Description = description;
    this.Price = price;
    this.ProductId = productId;
  }
}