"use client";

import { useProducts } from "@/features/catalog/products/hooks";
import { ProductCard } from "@/components/ProductCard";
import { useCartStore } from "@/features/sales/cart/store";
import { useCart } from "@/features/sales/cart/hooks";
import { useEffect } from "react";
import Link from "next/link";

export default function ProductsPage() {
  const { data: products, isLoading, isError } = useProducts();
  const { data: cart } = useCart();
  const initCartId = useCartStore((state) => state.initCartId);

  const totalCartItems = cart?.items.reduce((acc, item) => acc + item.quantity, 0) || 0;

  useEffect(() => {
    // Initialize anonymous cart session if it doesn't exist
    initCartId();
  }, [initCartId]);

  if (isLoading) {
    return <div className="p-8 text-center text-slate-500">กำลังโหลดรายการสินค้า...</div>;
  }

  if (isError) {
    return <div className="p-8 text-center text-red-500">โหลดรายการสินค้าผิดพลาด</div>;
  }

  return (
    <div className="min-h-screen bg-white">
      <div className="max-w-7xl mx-auto p-4 sm:p-6 lg:p-8 pt-12">
        <div className="flex flex-col sm:flex-row justify-between items-start sm:items-end mb-10 pb-6 border-b border-slate-100 gap-4 sm:gap-0">
          <div>
             <h1 className="text-4xl font-extrabold text-slate-900 tracking-tight">รายการสินค้า</h1>
             <p className="text-slate-500 mt-2">ค้นพบสินค้าใหม่ล่าสุดของเรา</p>
          </div>
          <Link 
            href="/cart" 
            className="group relative inline-flex items-center justify-center bg-indigo-600 text-white px-8 py-3 rounded-full font-medium hover:bg-indigo-700 transition-all shadow-sm hover:shadow-md hover:-translate-y-0.5"
          >
            <span>ดูตะกร้าสินค้า {totalCartItems > 0 && `(${totalCartItems})`}</span>
            <span className="ml-2 group-hover:translate-x-1 transition-transform">&rarr;</span>
          </Link>
        </div>

        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-8">
          {products?.map((product) => (
            <ProductCard key={product.id} product={product} />
          ))}
          {products?.length === 0 && (
            <div className="col-span-full py-20 text-center bg-slate-50 rounded-3xl border border-dashed border-slate-200">
              <p className="text-slate-500 text-lg">ไม่มีสินค้า</p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
