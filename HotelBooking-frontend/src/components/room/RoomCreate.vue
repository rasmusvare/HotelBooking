<script lang="ts">
import { Options, Vue } from "vue-class-component";
import type { IRoom } from "@/domain/IRoom";
import { useRoomTypeStore } from "@/stores/RoomTypes";
import { RoomService } from "@/services/RoomService";
import { Modal } from "bootstrap";

@Options({
  props: {
    rooms: Object as unknown as IRoom[],
    hotelId: String,
  },
})
export default class RoomCreate extends Vue {
  roomTypeStore = useRoomTypeStore();

  roomService = new RoomService();

  errorMessage: Array<string> | null | undefined = null;

  hotelId!: string;
  rooms!: IRoom[];
  
  roomFormData: IRoom = {
    roomNumber: "",
    roomTypeId: "",
    roomStatus: "Available",
  };

  createRoomModal!: Modal;

  mounted() {
    this.roomFormData.hotelId = this.hotelId;
    this.createRoomModal = new Modal(
      document.getElementById("createRoomModal")!
    );
  }

  async handleCreateRoom() {
    if (this.roomFormData.roomTypeId == "") {
      this.errorMessage = new Array<string>();
      this.errorMessage.push("Please select a room type");
    } else {
      const res = await this.roomService.add(this.roomFormData);
      if (res.status >= 300) {
        this.errorMessage = res.errorMessage;
        console.log(res);
      } else {
        this.errorMessage = [];
        this.rooms.push(res.data as unknown as IRoom);
        this.createRoomModal.hide();
      }
    }
  }
}
</script>

<template>
  <div
    class="modal fade"
    :id="'createRoomModal'"
    tabindex="-1"
    aria-labelledby="ModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="ModalLabel">Create Room</h5>
          <button
            type="button"
            class="btn-close"
            ref="close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body">
          <div class="container">
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
            <div>
              <select
                v-model="roomFormData.roomTypeId"
                class="form-select form-control-bottom"
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
            </div>
          </div>
        </div>
        <div class="modal-footer">
          <button class="btn btn-primary" @click="handleCreateRoom()">
            Add
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
