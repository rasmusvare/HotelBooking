<script lang="ts">
import { Options, Vue } from "vue-class-component";

import type { IRoom } from "@/domain/IRoom";

import RoomEditForm from "../../components/room/RoomEditForm.vue";
import { RoomService } from "@/services/RoomService";

@Options({
  props: {
    hotelId: String,
    roomId: String,
  },
  components: {
    RoomEditForm,
  },
})
export default class RoomEdit extends Vue {
  roomId!: string;
  hotelId!: string;

  roomService = new RoomService();

  room: IRoom | null = null;

  async mounted() {
    await this.roomService.get(this.roomId).then((res) => (this.room = res));
    // await RoomService.get(this.roomId).then((res) => {
    //     if (res.data != null) {
    //         this.room = res.data
    //     }
    // });
    // await TicketService.getAllRoomTickets(this.roomId).then((res) => {
    //     if (res.data != null) {
    //         this.roomTickets = res.data;
    //     }
    // });
    // await StayService.getAllRoomStays(this.roomId).then((res) => {
    //         if (res.data != null) {
    //             this.roomStays = res.data;
    //         }
    // });
  }
}
</script>

<template>
  <div class="container">
    <div>
      <h3 class="fw-normal">Details</h3>
      <RoomEditForm :room="room" :hotelId="hotelId" />
    </div>
    <hr />
    <div>
      <div class="d-flex justify-content-between align-items-center">
        <h3 class="mt-3 fw-normal">Stays</h3>
        <div>
          <button
            type="button"
            class="btn btn-primary btn-sm"
            data-bs-toggle="modal"
            data-bs-target="#create-stay-modal"
          >
            Create Stay
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
