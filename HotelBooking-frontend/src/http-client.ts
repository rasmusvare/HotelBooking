import axios from "axios";

export const httpClient = axios.create({
  baseURL: "https://localhost:7009/api/",
  headers: {
    "Content-Type": "application/json",
  },
});

export default httpClient;
