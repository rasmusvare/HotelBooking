<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { Modal } from "bootstrap";
import { RoomService } from "@/services/RoomService";

@Options({
  props: {
    roomId: String,
    hotelId: String,
  },
})
export default class DeleteRoomModal extends Vue {
  roomId!: string;
  hotelId!: string;

  errorMessage: Array<string> | undefined = [];

  deleteRoomModal!: Modal;

  roomService = new RoomService();

  async mounted() {
    this.deleteRoomModal = new Modal(
      document.getElementById("deleteRoomModal-" + this.roomId)!
    );
  }

  async handleDeleteRoom() {
    const res = await this.roomService.remove(this.roomId);
    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.errorMessage = [];
      this.deleteRoomModal.hide();
      this.$router.push("/admin/" + this.hotelId + "/rooms");
    }
  }
}
</script>

<template>
  <div
    class="modal fade"
    :id="'deleteRoomModal-' + roomId"
    tabindex="-1"
    aria-labelledby="ModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="ModalLabel">Delete room</h5>
          <button
            type="button"
            class="btn-close"
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
            <div>
              <p>
                As the booking system cannot guarantee that all future bookings
                for this room type can be rescheduled, all the future bookings
                from today's date will also be deleted by deleting this room.
              </p>

              <p>ARE YOU SURE YOU WANT TO DELETE THIS ROOM?</p>
            </div>
          </div>
          <div class="modal-footer">
            <div class="d-flex">
              <button
                class="btn btn-primary w-50"
                @click="deleteRoomModal.hide()"
              >
                Cancel
              </button>
              <button
                class="btn btn-danger w-50 ms-2"
                @click="handleDeleteRoom()"
              >
                Delete
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
