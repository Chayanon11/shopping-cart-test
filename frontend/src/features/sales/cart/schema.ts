import { z } from "zod";

export const cartItemResponseSchema = z.object({
  productId: z.string().uuid(),
  productName: z.string(),
  productPrice: z.number(),
  productImage: z.string().nullable(),
  quantity: z.number(),
  totalPrice: z.number(),
});

export const cartResponseSchema = z.object({
  id: z.string().uuid(),
  items: z.array(cartItemResponseSchema),
  totalBalance: z.number(),
});

export const addItemRequestSchema = z.object({
  productId: z.string().uuid(),
  quantity: z.number().min(1),
});

export const updateItemRequestSchema = z.object({
  quantity: z.number().min(1),
});

export type CartItemResponse = z.infer<typeof cartItemResponseSchema>;
export type CartResponse = z.infer<typeof cartResponseSchema>;
export type AddItemRequest = z.infer<typeof addItemRequestSchema>;
export type UpdateItemRequest = z.infer<typeof updateItemRequestSchema>;
