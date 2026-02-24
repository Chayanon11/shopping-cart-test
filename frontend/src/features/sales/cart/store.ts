import { create } from "zustand";
import { persist } from "zustand/middleware";
import { v4 as uuidv4 } from "uuid";

interface CartState {
  cartId: string;
  initCartId: () => void;
  resetCartId: () => void;
}

export const useCartStore = create<CartState>()(
  persist(
    (set, get) => ({
      cartId: "",
      
      initCartId: () => {
        const currentCartId = get().cartId;
        if (!currentCartId) {
          set({ cartId: uuidv4() });
        }
      },

      resetCartId: () => {
        set({ cartId: uuidv4() });
      },
    }),
    {
      name: "cart-storage", 
    }
  )
);
