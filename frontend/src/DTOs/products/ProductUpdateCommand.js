export class ProductUpdateCommand {
  constructor({
    id = 0,
    name = "",
    description = ""
  } = {}) {
    this.Id = id;
    this.Name = name;
    this.Description = description;
  }
}