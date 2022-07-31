import type { IAmenity } from "@/domain/IAmenity";
import { BaseService } from "./BaseService";

const apiVersion: string = "v1";

export class AmenityService extends BaseService<IAmenity> {
  constructor() {
    super("amenity", apiVersion);
  }
}
