import { UserInfo } from "./user-info.interface";

export interface Publication {
  id: string;
  title: string;
  labels: string;
  body: string;
  dateTime: Date;
  author: UserInfo;
}
