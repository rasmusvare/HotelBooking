<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useHotelStore } from "@/stores/Hotels";
import { HotelService } from "@/services/HotelService";
import HotelDropdown from "@/components/HotelDropdown.vue";
import { useIdentityStore } from "@/stores/identity";

@Options({
  components: {
    HotelDropdown,
  },
})
export default class AdminIndex extends Vue {
  hotelStore = useHotelStore();
  hotelService = new HotelService();
  identityStore = useIdentityStore();
  name = "user";
  hotelId = 0;

  async mounted(): Promise<void> {
    if (this.identityStore.jwt != null) {
      this.name = `${this.identityStore.jwt?.firstName} ${this.identityStore.jwt?.lastName}`;
    }
    const hotels = await this.hotelService.getAll();

    this.hotelStore.$state.data = hotels;
    this.hotelStore.$state.current = hotels[0];
  }

  beforeCreate() {
    if (this.identityStore.$state.jwt == undefined) {
      window.location.href = "/identity";
    }
  }

  handleLogout() {
    this.identityStore.$state.jwt = undefined;
    window.location.href = "/";
  }
}
</script>

<template>
  <div
    class="d-flex flex-column flex-shrink-0 p-3 bg-light"
    style="width: 280px"
  >
    <a
      href="/"
      class="d-flex align-items-center mb-3 mb-md-0 me-md-auto link-dark text-decoration-none"
    >
      <object
        class="me-2"
        data="https://www.svgrepo.com/show/49263/building.svg"
        type="image/svg+xml"
        width="24"
        height="24"
      ></object>
      <span class="fs-4">Hotel Admin</span>
    </a>
    <hr />
    <ul class="nav nav-pills flex-column mb-auto">
      <li class="nav-item" v-if="hotelStore.$state.current != undefined">
        <RouterLink
          class="px-2 nav-link link-dark d-flex align-items-center"
          :to="{
            name: 'roomindex',
            params: { hotelId: hotelStore.$state.current.id },
          }"
          :class="{ active: $route.name === 'roomindex' }"
        >
          <object
            class="me-4"
            data="https://www.svgrepo.com/show/342786/room.svg"
            type="image/svg+xml"
            width="16"
            height="16"
          ></object>
          Rooms
        </RouterLink>
      </li>
      <li class="nav-item" v-if="hotelStore.$state.current != undefined">
        <RouterLink
          class="px-2 nav-link link-dark d-flex align-items-center"
          :to="{
            name: 'hotelbookings',
            params: { hotelId: hotelStore.$state.current.id },
          }"
          :class="{ active: $route.name === 'hotelbookings' }"
        >
          <object
            class="me-4"
            data="https://www.svgrepo.com/show/355174/plan.svg"
            type="image/svg+xml"
            width="16"
            height="16"
          ></object>
          Bookings
        </RouterLink>
      </li>
    </ul>
    <RouterLink
      class="h6 mb-3 d-flex align-items-center link-dark text-decoration-none"
      v-if="hotelStore.$state.current != undefined"
      :to="{
        name: 'home',
        params: { hotelId: hotelStore.$state.current.id },
      }"
      :class="{ active: $route.name == 'home' }"
    >
      <object
        class="me-4"
        data="https://www.svgrepo.com/show/84870/link.svg"
        type="image/svg+xml"
        width="16"
        height="16"
      ></object>
      Public room search form
    </RouterLink>
    <div class="dropup">
      <a
        href="#"
        class="d-flex align-items-center link-dark text-decoration-none dropdown-toggle"
        id="dropdownHotel"
        data-bs-toggle="dropdown"
        aria-expanded="false"
      >
        <h5>Hotels</h5>
      </a>
      <HotelDropdown />
    </div>

    <hr />
    <div>
      <span
        class="d-flex align-items-center justify-content-between link-dark text-decoration-none"
      >
        <div class="d-flex align-items-center">
          <object
            class="me-2"
            data="https://www.svgrepo.com/show/5319/user.svg"
            type="image/svg+xml"
            width="24"
            height="24"
          ></object>
          <strong>{{ name }}</strong>
        </div>
        <button type="button" class="btn btn-danger" @click="handleLogout()">
          Log Out
        </button>
      </span>
    </div>
  </div>
  <div
    class="d-flex my-3 flex-fill flex-grow-1 justify-content-center overflow-auto container"
  >
    <router-view :key="$route.path"></router-view>
  </div>
</template>

<style>
#app,
main {
  display: flex;
  height: 100%;
  width: 100%;
}
</style>
