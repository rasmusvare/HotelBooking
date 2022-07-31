<script lang="ts">
import type { IHotel } from "@/domain/IHotel";
import { HotelService } from "@/services/HotelService";
import { useHotelStore } from "@/stores/Hotels";
import { Options, Vue } from "vue-class-component";

@Options({
  props: {
    hotel: Object as unknown as IHotel,
  },
  watch: {
    hotel() {
      this.hotelFormData.id = this.hotel.id;
      this.hotelFormData.name = this.hotel.name;
      this.hotelFormData.description = this.hotel.description;
      this.hotelFormData.email = this.hotel.email;
      this.hotelFormData.address = this.hotel.address;
      this.hotelFormData.telephoneNumber = this.hotel.telephoneNumber;
    },
  },
})
export default class HotelEditForm extends Vue {
  hotelStore = useHotelStore();
  hotelService = new HotelService();
  hotel!: IHotel;

  errorMessage: Array<string> | null | undefined = null;

  hotelFormData: IHotel = {
    name: "",
    description: "",
    email: "",
    address: "",
    telephoneNumber: "",
  };

  async handleEditHotel() {
    const res = await this.hotelService.update(this.hotelFormData);
    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.errorMessage = [];
      this.hotelStore.$state.data = await this.hotelService.getAll();
    }
  }
}
</script>

<template>
  <h1 class="h3 mb-3 fw-normal">Details</h1>
  <div class="form-floating">
    <input
      v-model="hotelFormData.name"
      type="text"
      class="form-control form-control-top"
    />
    <label for="floatingInput">Name</label>
  </div>
  <div class="form-floating">
    <input
      v-model="hotelFormData.description"
      type="text"
      class="form-control form-control-bottom"
    />
    <label for="floatingInput">Description</label>
  </div>
  <div class="form-floating">
    <input
      v-model="hotelFormData.address"
      type="text"
      class="form-control form-control-bottom"
    />
    <label for="floatingInput">Address</label>
  </div>
  <div class="form-floating">
    <input
      v-model="hotelFormData.email"
      type="text"
      class="form-control form-control-bottom"
    />
    <label for="floatingInput">Email</label>
  </div>
  <div class="form-floating">
    <input
      v-model="hotelFormData.telephoneNumber"
      type="text"
      class="form-control form-control-bottom"
    />
    <label for="floatingInput">Telephone</label>
  </div>
  <div class="checkbox mb-3"></div>
  <div class="d-flex">
    <button
      class="w-50 me-3 btn btn-lg btn-primary"
      @click="handleEditHotel()"
    >
      Edit Hotel
    </button>
  </div>
</template>
