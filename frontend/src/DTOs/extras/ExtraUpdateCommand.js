export class ExtraUpdateCommand {
  constructor({
    Id = 0,
    Name = "", 
    Description = "", 
    Price = 0
  } = {}) {
    this.Id = Id;
    this.Name = Name;
    this.Description = Description;
    this.Price = Price;
  }
}