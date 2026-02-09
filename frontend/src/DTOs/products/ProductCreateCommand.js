export class ProductCreateCommand {
  constructor({
    name = "",
    description = ""
  } = {}) {
    this.Name = name;
    this.Description = description;
  }
}