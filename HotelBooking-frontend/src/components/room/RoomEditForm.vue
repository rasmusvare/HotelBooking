<script lang="ts">
import type { IRoom } from "@/domain/IRoom";
import { Options, Vue } from "vue-class-component";
import { useRoomTypeStore } from "@/stores/RoomTypes";
import { RoomService } from "@/services/RoomService";

@Options({
  props: {
    hotelId: String,
    room: Object as unknown as IRoom,
  },
  watch: {
    room() {
      this.roomFormData.id = this.room.id;
      this.roomFormData.hotelId = this.hotelId;
      this.roomFormData.roomNumber = this.room.roomNumber;
      this.roomFormData.roomTypeId = this.room.roomTypeId;
      this.roomFormData.roomStatus = this.room.roomStatus;
    },
  },
})
export default class RoomEdit extends Vue {
  roomTypeStore = useRoomTypeStore();
  roomService = new RoomService();

  room!: IRoom;
  hotelId!: string;

  roomFormData: IRoom = {
    id: "",
    hotelId: "",
    roomNumber: "",
    roomTypeId: "",
    roomStatus: "",
  };

  errorMessage: Array<string> | null | undefined = null;

  async handleEditRoom() {
    const res = await this.roomService.update(this.roomFormData);
    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.errorMessage = null;
      this.$router.push("/admin/" + this.hotelId + "/rooms");
    }
  }

  async handleDeleteRoom() {
    const res = await this.roomService.remove(this.roomFormData.id!);

    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.errorMessage = null;
      this.$router.push("/admin/" + this.hotelId + "/rooms");
    }
  }
}
</script>

<template>
  <div>
    <div
      class="text-danger validation-summary-errors"
      data-valmsg-summary="true"
      data-valmsg-replace="true"
    >
      <ul v-for="error of errorMessage">
        <li>{{ error }}</li>
      </ul>
    </div>
    <div class="form-floating">
      <input
        v-model="roomFormData.roomNumber"
        type="text"
        class="form-control form-control-top"
      />
      <label for="floatingInput">Room number</label>
    </div>
    <div class="form-floating">
      <select
        v-model="roomFormData.roomTypeId"
        class="form-select form-control-middle"
        aria-label="Default select example"
      >
        <option disabled value="">Select a Room Type</option>
        <template
          v-bind:key="roomType.id"
          v-for="roomType in roomTypeStore.$state.data"
        >
          <option :value="roomType.id">{{ roomType.name }}</option>
        </template>
      </select>
      <label for="floatingInput">Room Type</label>
    </div>
    <div class="form-floating">
      <select
        v-model="roomFormData.roomStatus"
        class="form-select form-control-bottom"
        aria-label="Default select example"
      >
        <option disabled value="">Select a Status</option>
        <option value="Available">Available</option>
        <option value="AwaitingMaintenance">Awaiting Maintenance</option>
        <option value="Occupied">Occupied</option>
      </select>
      <label for="floatingInput">Status</label>
    </div>
    <div class="d-flex">
      <button
        class="w-100 me-3 btn btn-lg btn-primary"
        @click="handleEditRoom()"
      >
        Save changes
      </button>
      <button
        class="w-100 ms-3 btn btn-lg btn-danger"
        @click="handleDeleteRoom()"
      >
        Delete Room
      </button>
    </div>
  </div>
</template>
