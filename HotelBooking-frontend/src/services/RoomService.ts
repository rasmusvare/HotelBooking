import { BaseService } from "./BaseService";
import type { IRoom } from "@/domain/IRoom";

const apiVersion = "v1";

export class RoomService extends BaseService<IRoom> {
  constructor() {
    super("room", apiVersion);
  }
}
