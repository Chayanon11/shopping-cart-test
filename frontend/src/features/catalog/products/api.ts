import { apiClient } from "@/shared/api/client";
import type { ProductResponse } from "./schema";

export const getProducts = async (): Promise<ProductResponse[]> => {
  const { data } = await apiClient.get<ProductResponse[]>("/v1/products");
  return data;
};
