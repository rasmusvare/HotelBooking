<script lang="ts">
import { Vue } from "vue-class-component";
import { useIdentityStore } from "@/stores/identity";
// import { useRoomTypeStore } from "@/stores/RoomTypes";

// import IdentityService from "./services/IdentityService";

export default class App extends Vue {
  created() {
    // IdentityService.loadStoredIdentity();
  }
}
</script>

<template>
  <main>
    <RouterView />
  </main>
</template>


