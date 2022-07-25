import type { IBaseEntity } from "@/domain/IBaseEntity";

export interface IHotel extends IBaseEntity{
  name: string;
  description: string;
  email: string;
  telephoneNumber: string;
  address: string;
}