import { z } from "zod";

export const productResponseSchema = z.object({
  id: z.string().uuid(),
  name: z.string(),
  price: z.number(),
  image: z.string().nullable(),
  stockQuantity: z.number(),
});

export type ProductResponse = z.infer<typeof productResponseSchema>;
