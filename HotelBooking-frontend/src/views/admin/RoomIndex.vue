<script lang="ts">
import { Options, Vue } from "vue-class-component";
import type { IRoom } from "@/domain/IRoom";
import { useRoomTypeStore } from "@/stores/RoomTypes";
import RoomCreate from "@/components/room/RoomCreate.vue";
import { RoomTypeService } from "@/services/RoomTypeService";
import { RoomService } from "@/services/RoomService";

@Options({
  components: {
    RoomCreate,
  },
  props: {
    hotelId: String,
  },
})
export default class RoomIndex extends Vue {
  roomTypeStore = useRoomTypeStore();
  roomTypeService = new RoomTypeService();
  roomService = new RoomService();

  hotelId!: string;

  rooms: IRoom[] = [];

  async mounted() {
    this.rooms = await this.roomService.getAll(this.hotelId);
    const roomTypes = await this.roomTypeService.getAll(this.hotelId);
    this.roomTypeStore.$state.data = roomTypes;
  }

  async handleOpenRoomModal() {
    const roomTypes = await this.roomTypeService.getAll(this.hotelId);
    this.roomTypeStore.$state.data = roomTypes;
  }
}
</script>

<template>
  <div v-if="hotelId !== undefined" class="container">
    <div class="d-flex justify-content-between">
      <h3>Rooms</h3>
      <div>
        <button
          class="btn btn-primary btn-sm"
          data-bs-toggle="modal"
          :data-bs-target="'#createRoomModal'"
          @click="handleOpenRoomModal()"
        >
          Create New
        </button>
        <RoomCreate v-bind:key="hotelId" :hotelId="hotelId" :rooms="rooms" />
      </div>
    </div>

    <table class="table">
      <thead>
        <tr>
          <th scope="col">Status</th>
          <th scope="col">Room number</th>
          <th scope="col">Room type</th>
          <th scope="col"></th>
        </tr>
      </thead>
      <tbody>
        <tr v-bind:key="room.id" v-for="room in rooms">
          <th scope="row" style="width: 10%">
            <span
              class="badge rounded-pill me-3 fs-6"
              :class="{
                'bg-success': room.roomStatus === 'Available',
                'bg-warning': room.roomStatus === 'AwaitingMaintenance',
                'bg-danger': room.roomStatus === 'Occupied',
              }"
            >
              {{ room.roomStatus }}
            </span>
          </th>
          <td>
            <span>{{ room.roomNumber }}</span>
          </td>
          <td>
            <span>{{
              roomTypeStore.getRoomTypeById(room.roomTypeId)?.name
            }}</span>
          </td>
          <td style="width: 10%">
            <RouterLink :to="{ name: 'roomedit', params: { roomId: room.id } }">
              <button class="btn btn-primary btn-sm ms-4">Manage</button>
            </RouterLink>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
