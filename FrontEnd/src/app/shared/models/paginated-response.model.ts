export interface PaginatedResponse<T>{
  totalData: number;
  data: Array<T>;
}
