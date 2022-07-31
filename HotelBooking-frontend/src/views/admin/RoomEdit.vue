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
  }
}
</script>

<template>
  <div class="container">
    <div>
      <h3 class="fw-normal">Details</h3>
      <RoomEditForm :room="room" :hotelId="hotelId" />
    </div>
    </div>
</template>
