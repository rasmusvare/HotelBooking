<template>
  <div class="form-identity form-register">
    <h1 class="h3 mb-3 fw-normal">Register</h1>
    <div class="form-floating">
      <input
        v-model="firstname"
        type="text"
        class="form-control form-control-top"
        placeholder="John"
      />
      <label for="floatingInput">First name</label>
    </div>
    <div class="form-floating">
      <input
        v-model="lastname"
        type="text"
        class="form-control form-control-bottom"
        placeholder="John"
      />
      <label for="floatingInput">Last name</label>
    </div>
    <div class="form-floating">
      <input
        v-model="email"
        type="email"
        class="form-control form-control-top"
        placeholder="name@example.com"
      />
      <label for="floatingInput">Email</label>
    </div>
    <div class="form-floating">
      <input
        v-model="password"
        type="password"
        class="form-control form-control-bottom"
        id="floatingPassword"
        placeholder="Password"
      />
      <label for="floatingPassword">Password</label>
    </div>
    <button class="w-100 btn btn-lg btn-primary" @click="handleRegisterSubmit">
      Register
    </button>
  </div>
</template>

<script lang="ts">
// import IdentityService from "@/services/IdentityService";
import { Vue } from "vue-class-component";
export default class Register extends Vue {
  email = "";
  password = "";
  firstname = "";
  lastname = "";

  async handleRegisterSubmit() {
    // await IdentityService.register(this.email,this.password,
    //   this.firstname,
    //   this.lastname)
    //   .then((res) => {
    //       if (!res.success) {
    //         console.log(res)
    //         return;
    //       }
    //       this.$router.push({ name: 'home'});
    //     }
    //   ).catch((res) => {
    //       console.log(res);
    //     }
    //   );
  }
}
</script>

<style></style>
