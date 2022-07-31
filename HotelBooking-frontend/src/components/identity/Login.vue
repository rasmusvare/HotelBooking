<script lang="ts">
import { IdentityService } from "@/services/IdentityService";
import { useIdentityStore } from "@/stores/identity";
import { Vue } from "vue-class-component";
import router from "@/router";

export default class Login extends Vue {
  identityService = new IdentityService();
  identityStore = useIdentityStore();
  email = "";
  password = "";
  error = "";

  errorMessage: Array<string> | null | undefined = null;

  async handleLoginSubmit() {
    if (this.email.length > 0 && this.password.length > 0) {
      const res = await this.identityService.login(this.email, this.password);

      if (res.status >= 300) {
        this.errorMessage = res.errorMessage;
        console.log(res);
      } else {
        this.errorMessage = [];
        this.identityStore.$state.jwt = res.data;
      }
    }
    await router.push("/admin");
  }
}
</script>

<template>
  <div class="form-identity form-signin">
    <h1 class="h3 mb-3 fw-normal">Sign in</h1>
    <div
      v-if="errorMessage != null"
      class="text-danger validation-summary-errors"
      data-valmsg-summary="true"
      data-valmsg-replace="true"
    >
      <ul v-for="error of errorMessage" :key="error">
        <li>{{ error }}</li>
      </ul>
    </div>
    <div class="form-floating">
      <input
        v-model="email"
        class="form-control form-control-top"
        id="floatingEmail"
        :class="{ 'is-invalid': error !== '' }"
        type="email"
        placeholder="name@example.com"
      />
      <label for="floatingEmail">Email</label>
    </div>
    <div class="form-floating">
      <input
        v-model="password"
        class="form-control form-control-bottom"
        id="floatingPass"
        :class="{ 'is-invalid': error !== '' }"
        type="password"
        placeholder="Password"
      />
      <label for="floatingPass">Password</label>
      <div class="invalid-feedback">
        <template v-if="error !== ''">{{ error }}</template>
      </div>
    </div>

    <div class="checkbox mb-3">
      <!-- <label>
          <input type="checkbox" value="remember-me"> Remember me
      </label> -->
    </div>
    <button class="w-100 btn btn-lg btn-primary" @click="handleLoginSubmit()">
      Sign in
    </button>
  </div>
</template>

<style></style>
