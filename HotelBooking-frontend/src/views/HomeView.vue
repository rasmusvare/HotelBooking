<script lang="ts">
// import IdentityService from "@/services/IdentityService";
import { useIdentityStore } from "@/stores/identity";
import { Vue } from "vue-class-component";

export default class HomeView extends Vue {
  identityStore = useIdentityStore();

  handleLogOut() {
    // IdentityService.logout();
  }
}
</script>

<template>
  <div class="container">
    <div class="px-4 py-5 my-5 text-center">
      <h1 class="display-5 fw-bold">Hotel!</h1>
      <div class="col-lg-6 mx-auto">
        <p class="lead mb-4">Hotel booking system</p>
        <div class="d-grid gap-2 d-sm-flex justify-content-sm-center">
          <template v-if="identityStore.$state.jwt == null">
            <RouterLink to="/login">
              <button type="button" class="btn btn-primary btn-lg px-4 gap-3">Login</button>
            </RouterLink>
            <RouterLink to="/register">
              <button type="button" class="btn btn-primary btn-lg px-4 gap-3">Register</button>
            </RouterLink>
          </template>
          <template v-else>
            <button type="button" class="btn btn-primary btn-lg px-4 gap-3" @click="handleLogOut()">Logout</button>
            <RouterLink to="/dashboard">
              <button type="button" class="btn btn-primary btn-lg px-4 gap-3">To Dashboard</button>
            </RouterLink>
          </template>
        </div>
      </div>
    </div>
    <RouterView />
  </div>
</template>

