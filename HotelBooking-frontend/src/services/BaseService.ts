import httpClient from "@/http-client";
import { AxiosError } from "axios";
import type { IServiceResult } from "@/domain/IServiceResult";
import { IdentityService } from "@/services/IdentityService";
import { useIdentityStore } from "@/stores/identity";
import router from "@/router";
import type { IBaseEntity } from "@/domain/IBaseEntity";

export class BaseService<TEntity extends IBaseEntity> {
  identityStore = useIdentityStore();
  identityService = new IdentityService();

  constructor(public path: string, public apiVersion: string) {}

  async getAll(id?: string, extraPath?: string): Promise<TEntity[]> {
    try {
      let response;
      if (id == null) {
        response = await httpClient.get(`/${this.apiVersion}/${this.path}`, {
          headers: {
            Authorization: "bearer " + this.identityStore.$state.jwt?.token,
          },
        });
      } else {
        response = await httpClient.get(
          `/${this.apiVersion}/${this.path}/${
            extraPath == null ? id : extraPath + "/" + id
          }`,
          {
            headers: {
              Authorization: "bearer " + this.identityStore.$state.jwt?.token,
            },
          }
        );
      }
      const res = response.data as TEntity[];
      return res;
    } catch (e) {
      await this.handleError(e);

      if (!this.identityStore.$state.jwt) return [];

      const response = await httpClient.get(
        `/${this.apiVersion}/${this.path}`,
        {
          headers: {
            Authorization: "bearer " + this.identityStore.$state.jwt?.token,
          },
        }
      );
      const res = response.data as TEntity[];
      return res;
    }
  }

  async get(id: string): Promise<TEntity | null> {
    try {
      const response = await httpClient.get(
        `/${this.apiVersion}/${this.path}/details/${id}`,
        {
          headers: {
            Authorization: "bearer " + this.identityStore.$state.jwt?.token,
          },
        }
      );
      const res = response.data as TEntity;
      return res;
    } catch (e) {
      await this.handleError(e);
      if (!this.identityStore.$state.jwt) return null;

      const response = await httpClient.get(
        `/${this.apiVersion}/${this.path}`,
        {
          headers: {
            Authorization: "bearer " + this.identityStore.$state.jwt?.token,
          },
        }
      );
      const res = response.data as TEntity;
      return res;
    }
  }

  async add(entity: TEntity): Promise<IServiceResult<TEntity | void>> {
    let response;
    try {
      response = await httpClient.post(
        `/${this.apiVersion}/${this.path}`,
        entity,
        {
          headers: {
            Authorization: "bearer " + this.identityStore.$state.jwt?.token,
          },
        }
      );
    } catch (e) {
      return this.handleError(e);
    }

    return { status: response.status, data: response.data as TEntity };
  }

  async update(entity: TEntity): Promise<IServiceResult<void>> {
    let response;
    try {
      response = await httpClient.put(
        `/${this.apiVersion}/${this.path}/${entity.id}`,
        entity,
        {
          headers: {
            Authorization: "bearer " + this.identityStore.$state.jwt?.token,
          },
        }
      );
    } catch (e) {
      return this.handleError(e);
    }

    return { status: response.status };
  }

  async remove(id: string): Promise<IServiceResult<void>> {
    let response;
    try {
      response = await httpClient.delete(
        `/${this.apiVersion}/${this.path}/${id}`,
        {
          headers: {
            Authorization: "bearer " + this.identityStore.$state.jwt?.token,
          },
        }
      );
    } catch (e) {
      return this.handleError(e);
    }

    return { status: response.status };
  }

  async handleError(error: unknown): Promise<IServiceResult<void>> {
    console.log(error);
    if (error instanceof AxiosError) {
      const errors: string[] = [];

      if (error.response?.status == 401 && !this.identityStore.$state.jwt) {
        await router.push("/identity/account/login");
      }

      if (
        error.response?.status == 401 &&
        error.response?.data.errors == null &&
        this.identityStore.jwt?.refreshToken
      ) {
        const refresh = await this.identityService.refreshIdentity();

        return {
          status: 200,
        };
      }

      if (error.response?.data.errors != null) {
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
    }

    return {
      status: 0,
      errorMessage: ["Unknown error"],
    };
  }
}
