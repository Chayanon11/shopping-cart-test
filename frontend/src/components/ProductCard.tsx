"use client";

import { useAddToCart, useCart } from "@/features/sales/cart/hooks";
import type { ProductResponse } from "@/features/catalog/products/schema";
import { useCartStore } from "@/features/sales/cart/store";
import { useState } from "react";

interface ProductCardProps {
  product: ProductResponse;
}

export const ProductCard = ({ product }: ProductCardProps) => {
  const [adding, setAdding] = useState(false);
  const cartId = useCartStore((state) => state.cartId);
  const { data: cart } = useCart();
  const { mutateAsync: addToCart } = useAddToCart();

  const quantityInCart = cart?.items.find((i) => i.productId === product.id)?.quantity || 0;
  const remainingStock = product.stockQuantity - quantityInCart;

  const handleAddToCart = async () => {
    if (remainingStock <= 0) return;
    
    setAdding(true);
    try {
      await addToCart({
        cartId,
        request: {
          productId: product.id,
          quantity: 1,
        },
      });
      alert(`เพิ่ม ${product.name} ลงตะกร้าแล้ว`);
    } catch (error: any) {
      // ทำ popup แจ้งเตือน
      alert(error.response?.data?.message || "เพิ่มรายการสินค้าลงตะกร้าไม่สำเร็จ");
    } finally {
      setAdding(false);
    }
  };

  return (
    <div className="bg-white rounded-2xl p-4 shadow-sm border border-slate-100 flex flex-col hover:shadow-md transition-shadow">
      {/* Fallback image if null */}
      <div className="aspect-square bg-slate-100 rounded-xl mb-4 overflow-hidden">
        {product.image ? (
          <img src={product.image} alt={product.name} className="w-full h-full object-cover" />
        ) : (
          <div className="w-full h-full flex items-center justify-center text-slate-400">No Image</div>
        )}
      </div>
      
      <div className="flex-1">
        <h3 className="font-semibold text-slate-800 text-lg line-clamp-2">{product.name}</h3>
        <p className="text-xl font-bold text-indigo-600 mt-2">${product.price.toFixed(2)}</p>
        <p className="text-sm text-slate-500 mt-1">Stock: {remainingStock}</p>
      </div>

      <button
        onClick={handleAddToCart}
        disabled={adding || remainingStock <= 0}
        className={`mt-4 py-2 px-4 rounded-xl font-medium transition-colors ${
          remainingStock <= 0
            ? "bg-slate-100 text-slate-400 cursor-not-allowed"
            : adding
            ? "bg-indigo-400 text-white cursor-wait"
            : "bg-indigo-600 text-white hover:bg-indigo-700"
        }`}
      >
        {remainingStock <= 0 ? "สินค้าหมด" : adding ? "กำลังเพิ่ม..." : "เพิ่มลงตะกร้า"}
      </button>
    </div>
  );
};
