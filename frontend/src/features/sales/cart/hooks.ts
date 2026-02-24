import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { getCart, addItemToCart, updateCartItem, removeCartItem, clearCart, checkoutCart } from "./api";
import { useCartStore } from "./store";

export const useCart = () => {
  const cartId = useCartStore((state) => state.cartId);
  return useQuery({
    queryKey: ["cart", cartId],
    queryFn: () => getCart(cartId),
    enabled: !!cartId,
    retry: false, // Don't retry if cart is 404
  });
};

export const useAddToCart = () => {
  const queryClient = useQueryClient();
  const cartId = useCartStore((state) => state.cartId);
  
  return useMutation({
    mutationFn: addItemToCart,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["cart", cartId] });
    },
  });
};

export const useUpdateCartItem = () => {
  const queryClient = useQueryClient();
  const cartId = useCartStore((state) => state.cartId);

  return useMutation({
    mutationFn: updateCartItem,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["cart", cartId] });
    },
  });
};

export const useRemoveCartItem = () => {
  const queryClient = useQueryClient();
  const cartId = useCartStore((state) => state.cartId);

  return useMutation({
    mutationFn: removeCartItem,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["cart", cartId] });
    },
  });
};

export const useClearCart = () => {
  const queryClient = useQueryClient();
  const cartId = useCartStore((state) => state.cartId);

  return useMutation({
    mutationFn: clearCart,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["cart", cartId] });
    },
  });
};

export const useCheckout = () => {
  const queryClient = useQueryClient();
  const cartId = useCartStore((state) => state.cartId);
  const resetCartId = useCartStore((state) => state.resetCartId);

  return useMutation({
    mutationFn: checkoutCart,
    onSuccess: () => {
      resetCartId();
      queryClient.invalidateQueries({ queryKey: ["cart"] });
      queryClient.invalidateQueries({ queryKey: ["products"] });
    },
  });
};
