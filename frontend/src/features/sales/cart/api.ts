import { apiClient } from "@/shared/api/client";
import type { AddItemRequest, CartResponse, UpdateItemRequest } from "./schema";

export const getCart = async (cartId: string): Promise<CartResponse> => {
  const { data } = await apiClient.get<CartResponse>(`/v1/cart/${cartId}`);
  return data;
};

export const addItemToCart = async ({ cartId, request }: { cartId: string; request: AddItemRequest }): Promise<{ cartId: string }> => {
  const { data } = await apiClient.post<{ cartId: string }>(`/v1/cart/${cartId}/items`, request);
  return data;
};

export const updateCartItem = async ({ cartId, productId, request }: { cartId: string; productId: string; request: UpdateItemRequest }): Promise<{ cartId: string }> => {
  const { data } = await apiClient.put<{ cartId: string }>(`/v1/cart/${cartId}/items/${productId}`, request);
  return data;
};

export const removeCartItem = async ({ cartId, productId }: { cartId: string; productId: string }): Promise<{ cartId: string }> => {
  const { data } = await apiClient.delete<{ cartId: string }>(`/v1/cart/${cartId}/items/${productId}`);
  return data;
};

export const clearCart = async (cartId: string): Promise<void> => {
  await apiClient.delete(`/v1/cart/${cartId}`);
};

export const checkoutCart = async (cartId: string): Promise<void> => {
  await apiClient.post(`/v1/cart/${cartId}/checkout`);
};
