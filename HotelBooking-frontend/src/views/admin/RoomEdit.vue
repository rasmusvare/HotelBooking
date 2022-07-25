<template>
<div class="container">
    <div>
        <h3 class="fw-normal">Details</h3>
        <RoomEditForm :room="room" :hotelId="hotelId"/>
    </div>
<hr />
    <div>
        <div class="d-flex justify-content-between align-items-center">
            <h3 class="mt-3 fw-normal">Stays</h3>
            <div>
                <button type="button" class="btn btn-primary btn-sm" data-bs-toggle="modal" data-bs-target="#create-stay-modal">
                    Create Stay
                </button>
                <!-- <StayCreateModal :stays="roomStays" :roomId="roomId"/> -->
            </div>
        </div>
        <div>
            <!-- <StayList :stays="roomStays"/> -->
        </div>
    <!-- </div> -->
</div>
</div>

</template>

<script lang="ts">
import { Options, Vue } from "vue-class-component";

// import type { IStay } from "@/domain/IStay";
import type { IRoom } from "@/domain/IRoom";

// import RoomService from "@/services/RoomService";
import RoomEditForm from "../../components/room/RoomEditForm.vue";
import { RoomService } from "@/services/RoomService";
// import TicketService from "@/services/TicketService";
// import StayService from "@/services/StayService";
// import TicketList from "@/components/dashboard/Hotel/TicketList.vue";
// import StayList from "@/components/dashboard/Hotel/StayList.vue";
// import TicketCreateModal from "@/components/dashboard/Hotel/TicketCreateModal.vue";
// import StayCreateModal from "@/components/dashboard/Hotel/StayCreateModal.vue";

@Options({
    props: {
        hotelId: String,
        roomId: String
    },
    components: {
        RoomEditForm,
        // StayList,
        // StayCreateModal
    }
})
export default class RoomEdit extends Vue{
    roomId!: string;
    hotelId!: string;

    roomService = new RoomService();

    room: IRoom | null = null;
    // roomStays: IStay[] = [];

    async mounted() {
        await this.roomService.get(this.roomId).then((res) => this.room = res)
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

<style>

</style>