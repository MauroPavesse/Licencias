export class SearchFilterCommand {
  constructor(field = "", value = "", ids = []) {
    this.Field = field;
    this.Value = value;
    this.Ids = ids;
  }
}

export class SearchCommand {
  constructor({
    id = null,
    filters = [],
    includes = [],
    disableTracking = true,
    deleted = 0,
  } = {}) {
    this.Id = id;
    this.Filters = filters;
    this.Includes = includes;
    this.DisableTracking = disableTracking;
    this.Deleted = deleted;
  }
}
