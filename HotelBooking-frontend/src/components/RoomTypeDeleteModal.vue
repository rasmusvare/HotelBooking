<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { Modal } from "bootstrap";
import { RoomTypeService } from "@/services/RoomTypeService";
import { useRoomTypeStore } from "@/stores/RoomTypes";

@Options({
  props: {
    roomTypeId: String,
    hotelId: String,
  },
})
export default class RoomTypeDeleteModal extends Vue {
  roomTypeId!: string;
  hotelId!: string;

  roomTypeStore = useRoomTypeStore();

  errorMessage: Array<string> | undefined = [];

  deleteRoomTypeModal!: Modal;

  roomTypeService = new RoomTypeService();

  mounted() {
    console.log(this.roomTypeId);

    this.deleteRoomTypeModal = new Modal(
      document.getElementById("deleteRoomTypeModal-" + this.roomTypeId)!
    );
  }

  async handleDeleteRoomType() {
    const res = await this.roomTypeService.remove(this.roomTypeId);
    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.errorMessage = [];
      this.roomTypeStore.deleteRoomType(this.roomTypeId!);
      this.deleteRoomTypeModal.hide();
    }
  }
}
</script>

<template>
  <div
    class="modal fade"
    :id="'deleteRoomTypeModal-' + roomTypeId"
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
                By deleting this room type you will also delete all the rooms
                and bookings associated with this room type
              </p>

              <p>ARE YOU SURE YOU WANT TO DELETE THIS ROOM TYPE?</p>
            </div>
          </div>
          <div class="modal-footer">
            <div class="d-flex">
              <button
                class="btn btn-primary w-50"
                @click="deleteRoomTypeModal.hide()"
              >
                Cancel
              </button>
              <button
                class="btn btn-danger w-50 ms-2"
                @click="handleDeleteRoomType()"
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
