<template>
  <h1 class="h3 mb-3 fw-normal">Room Types</h1>
  <div class="d-flex">
    <div class="flex-grow-1 w-50 me-3">
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
          type="text"
          v-model="roomTypeFormData.name"
          class="form-control form-control-top"
        />
        <label for="floatingInput">Name</label>
      </div>
      <div class="form-floating">
        <input
          type="text"
          v-model="roomTypeFormData.description"
          class="form-control form-control-middle"
        />
        <label for="floatingInput">Description</label>
        <div class="form-floating">
          <input
            type="number"
            v-model="roomTypeFormData.pricePerNight"
            class="form-control form-control-middle"
          />
          <label for="floatingInput">Price per night</label>
        </div>
        <!-- <div class="d-flex"> -->
        <div class="form-floating flex-grow-1">
          <input
            type="number"
            v-model="roomTypeFormData.numberOfBeds"
            class="form-control mb-0 form-control-bottom"
          />
          <label for="floatingInput">Number of beds</label>
        </div>
        <div class="dropup my-auto ms-2">
          <a
            class="btn btn-outline-primary dropdown-toggle"
            href="#"
            role="button"
            id="dropdownMenuLink"
            data-bs-toggle="dropdown"
            aria-expanded="false"
          >
            Select Amenities
          </a>
          <div
            class="dropdown-menu overflow-auto"
            multiple
            aria-label=".form-select-sm example"
            style="height: 200px"
          >
            <template
              v-bind:key="'select-' + amenity.id"
              v-for="amenity in amenityStore.$state.data"
            >
              <option
                class="dropdown-item"
                :class="{
                  active:
                    roomTypeFormData.amenities != undefined &&
                    roomTypeFormData.amenities?.some((e) => e.id == amenity.id),
                }"
                :value="amenity.id"
                @click="handleSelectRoomAmenity(amenity)"
              >
                {{ amenity.name }}
              </option>
            </template>
          </div>
        </div>
      </div>
      <div class="d-flex mt-3">
        <button
          class="flex-grow-1 w-50 btn btn-primary"
          @click="handleCreateRoomType()"
        >
          Create New
        </button>
        <template v-if="selectedRoomTypeId != null">
          <button
            class="btn btn-primary w-50 ms-3"
            @click="handleEditRoomType()"
          >
            Edit
          </button>
          <button class="btn btn-danger ms-3" @click="handleDeleteRoomType()">
            Delete
          </button>
        </template>
      </div>
    </div>
    <template v-if="roomTypeStore.$state.data.length != 0">
      <div class="w-50">
        <div class="list-group overflow-auto" style="height: 200px">
          <template
            v-bind:key="roomType.id"
            v-for="roomType in roomTypeStore.$state.data"
          >
            <a
              href="#"
              class="list-group-item list-group-item-action"
              :class="{ active: selectedRoomTypeId == roomType.id }"
              @click="handleSelectRoomType(roomType)"
            >
              <div class="d-flex w-100 justify-content-between">
                <h5 class="mb-1">{{ roomType.name }}</h5>
                <small>
                  <object
                    class="me-2"
                    data="https://www.svgrepo.com/show/17785/bed.svg"
                    type="image/svg+xml"
                    width="12"
                    height="12"
                  ></object>
                  {{ roomType.numberOfBeds }}
                  <br />
                  Count: {{ roomType.count ?? 0 }}
                </small>
              </div>
              <p class="mb-1">{{ roomType.description }}</p>
            </a>
          </template>
        </div>
      </div>
    </template>
  </div>
  <!-- </div> -->
</template>

<script lang="ts">
import type { IRoomType } from "@/domain/IRoomType";
import { RoomTypeService } from "@/services/RoomTypeService";
import { useRoomTypeStore } from "@/stores/RoomTypes";
import { useAmenityStore } from "@/stores/Amenities";
import { Options, Vue } from "vue-class-component";
import type { IAmenity } from "@/domain/IAmenity";

@Options({
  props: {
    hotelId: String,
  },
})
export default class HotelRoomTypesEdit extends Vue {
  amenityStore = useAmenityStore();
  roomTypeStore = useRoomTypeStore();
  roomTypeService = new RoomTypeService();

  errorMessage: Array<string> | null | undefined = null;

  selectedRoomTypeId: string | null = null;

  hotelId!: string;
  roomTypeFormData: IRoomType = {
    hotelId: "",
    id: "",
    name: "",
    description: "",
    pricePerNight: 0,
    count: 0,
    numberOfBeds: 1,
    amenities: [],
  };

  mounted() {
    this.roomTypeFormData.hotelId = this.hotelId;
  }

  handleSelectRoomType(roomType: IRoomType) {
    this.selectedRoomTypeId = roomType.id!;
    this.roomTypeFormData.id = this.selectedRoomTypeId;
    this.roomTypeFormData.name = roomType.name;
    this.roomTypeFormData.description = roomType.description;
    this.roomTypeFormData.count = roomType.count;
    this.roomTypeFormData.pricePerNight = roomType.pricePerNight;
    this.roomTypeFormData.amenities = roomType.amenities;
    this.roomTypeFormData.numberOfBeds = roomType.numberOfBeds;
  }

  handleSelectRoomAmenity(amenity: IAmenity) {
    if (this.roomTypeFormData.amenities == undefined) {
      this.roomTypeFormData.amenities = [];
    }
    if (this.roomTypeFormData.amenities?.some((e) => e.id == amenity.id)) {
      let index = this.roomTypeFormData.amenities.findIndex(
        (e) => e.id == amenity.id
      );
      if (index > -1) {
        this.roomTypeFormData.amenities.splice(index, 1);
      }
      return;
    }
    this.roomTypeFormData.amenities.push(amenity);
  }

  async handleCreateRoomType() {
    const res = await this.roomTypeService.add(
      {
        hotelId: this.roomTypeFormData.hotelId,

    name: this.roomTypeFormData.name,
    description: this.roomTypeFormData.description,
    pricePerNight:this.roomTypeFormData.pricePerNight,

    numberOfBeds: this.roomTypeFormData.numberOfBeds,
    amenities: this.roomTypeFormData.amenities,
      }
      );
    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.roomTypeStore.$state.data = await this.roomTypeService.getAll(
        this.hotelId
      );
    }
    this.clearFormData();
  }

  async handleEditRoomType() {
    const res = await this.roomTypeService.update(this.roomTypeFormData);

    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.roomTypeStore.updateRoomType(
        this.selectedRoomTypeId!,
        this.roomTypeFormData!
      );

      this.clearFormData();
    }
  }

  async handleDeleteRoomType() {
    const res = await this.roomTypeService.remove(this.selectedRoomTypeId!);

    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.roomTypeStore.deleteRoomType(this.selectedRoomTypeId!);

      this.clearFormData();
    }

    // await RoomTypeService.delete(this.selectedRoomTypeId!)
    //     .then((res) => {
    //         if (!res.success) {
    //             console.log(res)
    //             return;
    //         }
    //         this.roomTypeStore.deleteRoomType(this.selectedRoomTypeId!);
    //         this.clearFormData();
    //     }
    //     ).catch((res) => {
    //         console.log(res);
    //     }
    // );
  }

  clearFormData() {
    this.selectedRoomTypeId = null;
    this.roomTypeFormData.name = "";
    this.roomTypeFormData.description = "";
    this.roomTypeFormData.count = 0;
    this.roomTypeFormData.pricePerNight = 0;
    this.roomTypeFormData.amenities = undefined;
  }
}
</script>

<style></style>
