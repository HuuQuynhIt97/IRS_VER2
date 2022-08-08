import { User } from "./user";

export interface ApplicationUser {
  token: string;
  user: User;
  refreshToken: string;
}
export interface FunctionSystem {
  name: string;
  url: string;
  functionCode: string;
  childrens: Action[];
}
export interface Action {
  id: number;
  url: string;
  code: string;
}
