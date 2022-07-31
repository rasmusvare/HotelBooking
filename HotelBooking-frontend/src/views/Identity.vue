<script lang="ts">
// import IdentityService from "@/services/IdentityService";
import { useIdentityStore } from "@/stores/identity";
import { Vue } from "vue-class-component";

export default class Identity extends Vue {
  identityStore = useIdentityStore();

  handleLogOut() {
  }
}
</script>

<template>
  <div class="container">
    <div class="px-4 py-5 my-5 text-center">
      <div>
        <RouterLink to="/">Home</RouterLink>
        |
        <RouterLink to="/bookingsearch">Search bookings</RouterLink>
      </div>
      <div class="col-lg-6 mx-auto">
        <p class="lead mb-4">Hotel booking system</p>
        <div class="d-grid gap-2 d-sm-flex justify-content-sm-center">
            <RouterLink to="/login">
              <button type="button" class="btn btn-primary btn-lg px-4 gap-3">Login</button>
            </RouterLink>
            <RouterLink to="/register">
              <button type="button" class="btn btn-primary btn-lg px-4 gap-3">Register</button>
            </RouterLink>
        </div>
      </div>
    </div>
        <!-- <router-view :key="$route.path"></router-view> -->
    <RouterView />
  </div>
</template>

<style>
.form-identity {
  width: 100%;
  max-width: 330px;
  padding: 15px;
  margin: auto;
}
</style>

