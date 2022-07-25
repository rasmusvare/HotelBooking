<template>
<div class="container">
    <div class="d-flex">
        <div
        class="text-danger validation-summary-errors"
        data-valmsg-summary="true"
        data-valmsg-replace="true"
      >
        <ul v-for="error of errorMessage">
          <li>{{ error }}</li>
        </ul>
      </div>
    <BookingEditForm :bookingData="bookingData"/>
    </div>
    <div class="d-flex">
        <button class="btn btn-primary w-50" @click="handleEditReservation()">Save changes</button>
        <button class="btn btn-danger ms-3 w-50" @click="handleDeleteReservation()">Delete booking</button>
    </div>
    
</div>
</template>

<script lang="ts">
import BookingEditForm from "@/components/booking/BookingEditForm.vue";
import type { IBooking } from "@/domain/IBooking";
import type { IRoomType } from "@/domain/IRoomType";
import { BookingService } from "@/services/BookingService";
// import ReservationService from "@/services/ReservationService";
// import RoomTypeService from "@/services/RoomTypeService";
import { Options, Vue } from "vue-class-component";
// import BookingEditForm from "../../components/booking/BookingEditForm.vue";

@Options({
    props: {
        // hotelId: String,
        bookingId: String
    },
    components: {
        BookingEditForm
    }
})
export default class BookingEdit extends Vue {
        bookingService = new BookingService();

    // hotelId!: string;
    bookingId!: string;

    // bookingData: IBooking | undefined = undefined;
    bookingData: IBooking ={
        hotelId: "",
  roomTypeId: "",
  dateFrom: "",
  dateTo: "",
  noOfGuests: 0,
  guests: []
    }

    roomTypes: IRoomType[] = []


      errorMessage: Array<string> | null | undefined = null;

    
    // reservationFormData: IBooking = {
    //     hotelId: "",
    //     roomTypeId: "",
    //     noOfGuests: 1,
    //     // client: {
    //     //     firstName: "",
    //     //     lastName: "",
    //     //     phoneNumber: "",
    //     //     email: ""
    //     // },
    //     dateFrom: "",
    //     dateTo: "",
        


//         hotelId: string;
//   roomTypeId: string;
//   dateFrom: Date | string;
//   dateTo: Date | string;
//   noOfGuests: number;
    

    

    async mounted() {
           await this.bookingService.get(this.bookingId).then((res) => this.bookingData = res!);
           console.log(this.bookingData.guests.length);
           

        // await RoomTypeService.getAllHotelRoomTypes(this.hotelId).then((res) => {
        //         if (res.data != null) {
        //             this.roomTypes = res.data;
        //         }
        // });
        // await ReservationService.get(this.reservationId).then((res) => {
        //     if (res.data != null) {
        //         this.reservationFormData = res.data;

        //         // abysmal
        //         // source: https://stackoverflow.com/a/66558369
        //         let startDate = new Date(res.data.start);
        //         this.reservationFormData.start = (new Date(
        //             startDate.getTime() - startDate.getTimezoneOffset() * 60000).toISOString()).slice(0, -1);

        //         let endDate = new Date(res.data.end);
        //         this.reservationFormData.end = (new Date(
        //             endDate.getTime() - endDate.getTimezoneOffset() * 60000).toISOString()).slice(0, -1);
        //     }
        // });
    }

    handleSelectRoomType(roomType: IRoomType) {
        this.bookingData!.roomTypeId = roomType.id!;
    }

    async handleEditReservation() {
        await this.bookingService.update(this.bookingData);
        // await ReservationService.put(this.reservationId, this.reservationFormData)
        //     .then((res) => {
        //         if (!res.success) {
        //             console.log(res)
        //             return;
        //         }
        //     }
        //     ).catch((res) => {
        //         console.log(res);
        //     }
        // );
    }
    
    async handleDeleteReservation() {
const res = await this.bookingService.remove(this.bookingData.id!);
    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.$router.push({name: "hotelbookings", params:{hotelId:this.bookingData.hotelId}})

        // await ReservationService.delete(this.reservationId)
        //     .then((res) => {
        //         if (!res.success) {
        //             console.log(res)
        //             return;
        //         }
        //         this.$router.push({ name: 'hotelreservations', params: { hotelId: this.hotelId } });
        //     }
        //     ).catch((res) => {
        //         console.log(res);
        //     }
        // );
    }
}
}
</script>


<style>
/* #app,
main {
  display: flex;
  height: 1300px;
  width: 100%;
} */
</style>

