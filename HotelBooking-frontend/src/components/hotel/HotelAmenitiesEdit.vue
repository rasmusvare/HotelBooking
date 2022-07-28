

<script lang="ts">
import type { IAmenity } from "@/domain/IAmenity";
import { AmenityService } from "@/services/AmenityService";
import { useAmenityStore } from "@/stores/Amenities";
import { Options, Vue } from "vue-class-component";

@Options({
  props: {
    hotelId: String,
  },
})
export default class HotelAmenitiesEdit extends Vue {
  amenityStore = useAmenityStore();
  amenityService = new AmenityService();

  errorMessage: Array<string> | null | undefined = null;

  selectedAmenityId: string | undefined = undefined;

  hotelId!: string;
  amenityFormData: IAmenity = {
    name: "",
    description: "",
  };

  mounted() {
    this.amenityFormData.hotelId = this.hotelId;
  }

  handleSelectAmenity(amenity: IAmenity) {
    this.selectedAmenityId =
      this.selectedAmenityId == amenity.id! ? undefined : amenity.id!;
    this.amenityFormData.name = amenity.name;
    this.amenityFormData.description = amenity.description;
  }

  async handleCreateAmenity() {
    const res = await this.amenityService.add(this.amenityFormData);
    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.amenityStore.$state.data = await this.amenityService.getAll(
        this.hotelId
      );
    }

  }

  async handleEditAmenity() {
    this.amenityFormData.id = this.selectedAmenityId;
    const res = await this.amenityService.update(this.amenityFormData);
    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.amenityStore.$state.data = await this.amenityService.getAll(
        this.hotelId
      );
    }

  }

  async handleDeleteAmenity() {
    // await await AmenityService.delete(this.selectedAmenityId!)
    //   .then((res) => {
    //     if (!res.success) {
    //       console.log(res);
    //       return;
    //     }
    //     this.amenityStore.deleteAmenity(this.selectedAmenityId!);
    //     this.selectedAmenityId = null;
    //     this.amenityFormData.name = "";
    //     this.amenityFormData.description = "";
    //   })
    //   .catch((res) => {
    //     console.log(res);
    //   });
  }
}
</script>

<template>
  <h1 class="h3 mb-3 fw-normal">Amenities</h1>
  <div class="d-flex">
    <div class="flex-grow-1 w-50 me-3">
      <div class="form-floating">
        <input
          v-model="amenityFormData.name"
          type="text"
          class="form-control form-control-top"
        />
        <label for="floatingInput">Name</label>
      </div>
      <div class="form-floating">
        <input
          v-model="amenityFormData.description"
          type="text"
          class="form-control form-control-bottom"
        />
        <label for="floatingInput">Description</label>
      </div>
      <div class="checkbox mb-3"></div>
      <div class="d-flex">
        <button
          class="flex-grow-1 w-50 btn btn-primary"
          @click="handleCreateAmenity()"
        >
          Create New
        </button>
        <template v-if="selectedAmenityId != null">
          <button
            class="btn btn-primary w-50 ms-3"
            @click="handleEditAmenity()"
          >
            Edit
          </button>
          <button class="btn btn-danger ms-3" @click="handleDeleteAmenity()">
            Delete
          </button>
        </template>
      </div>
    </div>
    <template v-if="amenityStore.$state.data.length != 0">
      <div class="w-50">
        <div class="list-group overflow-auto" style="height: 200px">
          <template
            v-bind:key="amenity.id"
            v-for="amenity in amenityStore.$state.data"
          >
            <a
              href="#"
              class="list-group-item list-group-item-action"
              :class="{ active: selectedAmenityId == amenity.id }"
              @click="handleSelectAmenity(amenity)"
            >
              <div class="d-flex w-100 justify-content-between">
                <div>
                  <h5 class="mb-1">{{ amenity.name }}</h5>
                  <p class="mb-1">{{ amenity.description }}</p>
                </div>
              </div>
            </a>
          </template>
        </div>
      </div>
    </template>
  </div>
</template>

