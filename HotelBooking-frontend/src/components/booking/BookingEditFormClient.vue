<script lang="ts">
import type { IBooking } from "@/domain/IBooking";
import type { IGuest } from "@/domain/IGuest";
import { BookingService } from "@/services/BookingService";
import { RoomTypeService } from "@/services/RoomTypeService";
import { useRoomTypeStore } from "@/stores/RoomTypes";
import { Options, Vue } from "vue-class-component";

@Options({
  props: {
    editDisabled: Boolean,
    bookingData: Object as unknown as IBooking,
  },
  watch: {
    bokingData() {
      this.bookingData.id = this.hotel.id;
      this.bookingData.name = this.hotel.name;
      this.bookingData.description = this.hotel.description;
      this.bookingData.email = this.hotel.email;
      this.hotelbookingDataFormData.address = this.hotel.address;
      this.bookingData.telephoneNumber = this.hotel.telephoneNumber;
    },
  },
})
export default class BookingEditFormClient extends Vue {
  roomTypeService = new RoomTypeService();
  roomTypeStore = useRoomTypeStore();

  bookingService = new BookingService();
  bookingData!: IBooking;

  editDisabled!: boolean;

  bookingHolder: IGuest = {
    firstName: "",
    lastName: "",
    idCode: "",
    email: "",
    phoneNumber: "",
    isBookingOwner: true,
  };

  async beforeUpdate() {
    this.bookingHolder = this.bookingData.guests.find(
      (b) => b.isBookingOwner == true
    )!;
  }
}
</script>

<template>
  <div class="container">
    <div>
      <h4>Contact Information</h4>
      <div class="form-floating" v-if="bookingHolder != undefined">
        <input
          :disabled="editDisabled"
          v-model="bookingHolder.firstName"
          type="text"
          class="form-control form-control-top"
        />
        <label for="floatingInput">First Name</label>
      </div>
      <div class="form-floating">
        <input
          :disabled="editDisabled"
          v-model="bookingHolder.lastName"
          type="text"
          class="form-control form-control-middle"
        />
        <label for="floatingInput">Last Name</label>
      </div>
      <div class="form-floating">
        <input
          :disabled="editDisabled"
          v-model="bookingHolder.idCode"
          type="text"
          class="form-control form-control-middle"
        />
        <label for="floatingInput">National identification number</label>
      </div>
      <div class="form-floating">
        <input
          :disabled="editDisabled"
          v-model="bookingHolder.email"
          type="text"
          class="form-control form-control-middle"
        />
        <label for="floatingInput">Email</label>
      </div>
      <div class="form-floating">
        <input
          :disabled="editDisabled"
          v-model="bookingHolder.phoneNumber"
          type="text"
          class="form-control form-control-bottom"
        />
        <label for="floatingInput">Phone Number</label>
      </div>
    </div>
    <div v-if="bookingData.guests.length > 1">
      <h4>Guests</h4>
      <table class="table">
        <thead>
          <tr>
            <th scope="col">First name</th>
            <th scope="col">Last name</th>
            <th scope="col">Id Code</th>
            <th scope="col">Email</th>
            <th scope="col">Telephone</th>
            <th scope="col"></th>
          </tr>
        </thead>
        <tbody>
          <template v-for="each in bookingData.guests">
            <tr>
              <td>
                {{ each.firstName }}
              </td>
              <td>
                {{ each.lastName }}
              </td>
              <td>
                {{ each.idCode }}
              </td>
              <td>
                {{ each.email }}
              </td>
              <td>
                {{ each.phoneNumber }}
              </td>
              <td>EDIT</td>
            </tr>
          </template>
        </tbody>
      </table>
    </div>

    <h4>Duration</h4>
    <div class="form-floating">
      <input
        :disabled="editDisabled"
        v-model="bookingData.dateFrom"
        type="date"
        class="form-control form-control-top"
      />
      <label for="floatingInput">Start date</label>
    </div>
    <div class="form-floating">
      <input
        :disabled="editDisabled"
        v-model="bookingData!.dateTo"
        type="date"
        class="form-control form-control-bottom"
      />
      <label for="floatingInput">End date</label>
    </div>
  </div>
</template>
