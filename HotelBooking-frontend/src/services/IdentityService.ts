import { useIdentityStore } from "@/stores/identity";
import type { IJWTResponse } from "@/domain/IJWTResponse";
import type { IServiceResult } from "@/domain/IServiceResult";
import httpClient from "@/http-client";
import { AxiosError } from "axios";

const apiVersion = "v1";

export class IdentityService {
  identityStore = useIdentityStore();

  async login(
    email: string,
    password: string
  ): Promise<IServiceResult<IJWTResponse | null>> {
    try {
      const loginInfo = {
        email,
        password,
      };

      const response = await httpClient.post(
        apiVersion + "/identity/account/login",
        loginInfo
      );

      return { status: response.status, data: response.data as IJWTResponse };
    } catch (e) {
      return this.returnError(e);
    }
  }

  async register(
    email: string,
    password: string,
    firstName: string,
    lastName: string,
  ): Promise<IServiceResult<IJWTResponse | null>> {
    try {
      const registerInfo = {
        email,
        password,
        firstName,
        lastName,
      };

      const response = await httpClient.post(
        apiVersion + "/identity/account/register",
        registerInfo
      );
      return {
        status: response.status,
        data: response.data as IJWTResponse,
      };
    } catch (e) {
      return this.returnError(e);
    }
  }

  async refreshIdentity(): Promise<IServiceResult<null>> {
    try {
      const response = await httpClient.post(
        apiVersion + "/identity/account/refreshtoken",
        {
          jwt: this.identityStore.$state.jwt?.token,
          refreshtoken: this.identityStore.$state.jwt?.refreshToken,
        }
      );
      this.identityStore.$state.jwt = response.data;
      return {
        status: response.status,
      };
    } catch (e) {
      return this.returnError(e);
    }
  }

  returnError(error: unknown): IServiceResult<null> {
    if (error instanceof AxiosError) {
      const errors: string[] = [];
      const keys = Object.keys(error.response?.data.errors);

      keys.forEach((key) => {
        // eslint-disable-next-line no-unsafe-optional-chaining
        errors.push(...error.response?.data.errors[key]);
      });
      console.log(errors);

      return {
        status: error.response?.status as number,
        errorMessage: errors,
      };
    }

    return {
      status: 0,
      errorMessage: ["Unknown error"],
    };
  }
}
