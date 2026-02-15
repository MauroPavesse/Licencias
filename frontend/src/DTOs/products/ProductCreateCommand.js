export class ProductCreateCommand {
  constructor({
    Name = "",
    Description = "",
    ProductVersions = [],
  } = {}) {
    this.Name = Name;
    this.Description = Description;
    this.ProductVersions = ProductVersions;
  }
}