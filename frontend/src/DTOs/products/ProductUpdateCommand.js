export class ProductUpdateCommand {
  constructor({
    Id = 0,
    Name = "",
    Description = "",
    ProductVersions = [],
  } = {}) {
    this.Id = Id;
    this.Name = Name;
    this.Description = Description;
    this.ProductVersions = ProductVersions;
  }
}