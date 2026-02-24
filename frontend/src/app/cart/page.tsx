"use client";

import { useCart, useUpdateCartItem, useRemoveCartItem, useClearCart, useCheckout } from "@/features/sales/cart/hooks";
import { useCartStore } from "@/features/sales/cart/store";
import Link from "next/link";
import { useState } from "react";

export default function CartPage() {
  const cartId = useCartStore((state) => state.cartId);
  const { data: cart, isLoading, isError } = useCart();
  const { mutateAsync: updateItem } = useUpdateCartItem();
  const { mutateAsync: removeItem } = useRemoveCartItem();
  const { mutateAsync: clearCart } = useClearCart();
  const { mutateAsync: checkout } = useCheckout();

  const [checkoutStatus, setCheckoutStatus] = useState<"idle" | "loading" | "success" | "error">("idle");

  if (!cartId || isLoading) return <div className="p-8 text-center text-slate-500">Loading cart...</div>;
  
  if (isError && !cart) {
     // If API returns 404, it might mean the cart is empty physically on the backend
     return (
        <div className="max-w-4xl mx-auto p-8 text-center">
            <h1 className="text-3xl font-bold text-slate-900 mb-4">Your Cart is Empty</h1>
            <Link href="/products" className="text-indigo-600 font-medium hover:underline">Keep Shopping</Link>
        </div>
     )
  }

  const handleUpdateQuantity = async (productId: string, quantity: number) => {
    try {
        await updateItem({ cartId, productId, request: { quantity } });
    } catch (error: any) {
        alert(error.response?.data?.message || "Failed to update quantity");
    }
  };

  const handleCheckout = async () => {
    setCheckoutStatus("loading");
    try {
        await checkout(cartId );
        setCheckoutStatus("success");
    } catch (error: any) {
        setCheckoutStatus("error");
        alert(error.response?.data?.message || "Checkout failed");
    }
  };

  if (checkoutStatus === "success") {
      return (
          <div className="max-w-4xl mx-auto p-12 text-center bg-white rounded-2xl shadow-sm mt-8 border border-green-100">
              <div className="w-16 h-16 bg-green-100 text-green-600 rounded-full flex items-center justify-center mx-auto mb-6 text-3xl">✓</div>
              <h1 className="text-3xl font-bold text-slate-900 mb-4">Payment Successful!</h1>
              <p className="text-slate-600 mb-8">Your stock has been deducted correctly.</p>
              <Link href="/products" className="bg-indigo-600 text-white px-8 py-3 rounded-xl font-medium hover:bg-indigo-700 transition-colors">
                  Continue Shopping
              </Link>
          </div>
      )
  }

  return (
    <div className="min-h-screen bg-white">
      <div className="max-w-5xl mx-auto p-4 sm:p-6 lg:p-8 pt-12">
        <div className="flex justify-between items-end mb-10 pb-6 border-b border-gray-100">
          <div>
            <h1 className="text-4xl font-extrabold text-gray-900 tracking-tight">ตะกร้าสินค้า</h1>
            <p className="text-gray-500 mt-2">ตรวจสอบสินค้าในตะกร้าของคุณก่อนชำระเงิน</p>
          </div>
          <Link href="/products" className="text-indigo-600 font-semibold hover:text-indigo-700 transition-colors flex items-center gap-2">
            <span>&larr;</span> กลับไปเลือกซื้อสินค้า
          </Link>
        </div>

        {!cart?.items || cart.items.length === 0 ? (
          <div className="text-center py-20 bg-white rounded-3xl border border-dashed border-gray-200 shadow-sm">
            <div className="w-20 h-20 bg-gray-50 rounded-full flex items-center justify-center mx-auto mb-6">
              <svg className="w-8 h-8 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z" />
              </svg>
            </div>
            <p className="text-gray-900 font-medium text-xl mb-2">ตะกร้าสินค้าของคุณว่างเปล่า</p>
            <p className="text-gray-500 mb-8">ดูเหมือนว่าคุณยังไม่ได้เพิ่มสินค้าใดๆ</p>
            <Link href="/products" className="inline-block bg-indigo-600 text-white font-medium px-8 py-3 rounded-full hover:bg-indigo-700 shadow-sm transition-all hover:shadow hover:-translate-y-0.5">
              เริ่มช้อปปิ้ง
            </Link>
          </div>
        ) : (
          <div className="grid grid-cols-1 lg:grid-cols-12 gap-10">
            {/* Cart Items */}
            <div className="lg:col-span-8 space-y-6">
              {cart.items.map((item) => (
                <div key={item.productId} className="group flex flex-col sm:flex-row items-start sm:items-center gap-6 bg-white p-6 rounded-3xl border border-gray-100 shadow-sm hover:shadow-md transition-all">
                  <div className="w-28 h-28 bg-gray-50 rounded-2xl overflow-hidden flex-shrink-0 border border-gray-100">
                    {item.productImage ? (
                      <img src={item.productImage} alt={item.productName} className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-500" />
                    ) : <div className="w-full h-full flex items-center justify-center text-xs text-gray-400 font-medium">No Image</div>}
                  </div>
                  
                  <div className="flex-1 min-w-0">
                    <h3 className="font-bold text-gray-900 text-lg truncate mb-1">{item.productName}</h3>
                    <p className="text-indigo-600 font-semibold">${item.productPrice.toFixed(2)}</p>
                  </div>
                  
                  <div className="flex flex-col sm:items-end gap-4 w-full sm:w-auto mt-4 sm:mt-0">
                    <div className="font-bold text-gray-900 text-xl text-right w-full sm:w-auto">
                        ${item.totalPrice.toFixed(2)}
                    </div>
                    <div className="flex items-center gap-3 w-full sm:w-auto justify-between sm:justify-end">
                      <div className="flex items-center bg-gray-50 border border-gray-200 rounded-lg p-1">
                        <span className="px-3 py-1 text-sm font-medium text-gray-600">Qty</span>
                        <select 
                          value={item.quantity}
                          onChange={(e) => handleUpdateQuantity(item.productId, Number(e.target.value))}
                          className="bg-transparent pl-2 pr-6 py-1 font-semibold text-gray-900 focus:outline-none cursor-pointer"
                        >
                          {[...Array(10)].map((_, i) => (
                            <option key={i+1} value={i+1}>{i+1}</option>
                          ))}
                        </select>
                      </div>
                      <button 
                        onClick={() => removeItem({ cartId, productId: item.productId })}
                        className="text-gray-400 hover:text-red-500 p-2 rounded-lg hover:bg-red-50 transition-colors"
                        title="Remove item"
                      >
                        <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                        </svg>
                      </button>
                    </div>
                  </div>
                </div>
              ))}
            </div>

            {/* Cart Summary */}
            <div className="lg:col-span-4 relative">
              <div className="bg-white p-8 rounded-3xl border border-gray-100 shadow-[0_8px_30px_rgb(0,0,0,0.04)] sticky top-8">
                <h2 className="text-2xl font-bold text-gray-900 mb-8">สรุปยอด</h2>
                
                <div className="space-y-4 mb-8">
                  <div className="flex justify-between items-center">
                      <span className="text-gray-500 font-medium">ราคารวม ({cart.items.reduce((acc, i) => acc + i.quantity, 0)} items)</span>
                      <span className="font-semibold text-gray-900">${cart.totalBalance.toFixed(2)}</span>
                  </div>
                  <div className="flex justify-between items-center text-sm">
                      <span className="text-gray-500 font-medium">ภาษี</span>
                      <span className="font-medium text-gray-900">$0.00</span>
                  </div>
                  <div className="flex justify-between items-center text-sm">
                      <span className="text-gray-500 font-medium">ค่าจัดส่ง</span>
                      <span className="font-medium text-green-600">Free</span>
                  </div>
                </div>
                
                <div className="border-t border-gray-100 pt-6 mb-8 flex justify-between items-end">
                    <span className="font-bold text-gray-900">ยอดรวม</span>
                    <span className="font-black text-indigo-600 text-3xl">${cart.totalBalance.toFixed(2)}</span>
                </div>
                
                <div className="space-y-3">
                  <button 
                      onClick={handleCheckout}
                      disabled={checkoutStatus === "loading"}
                      className="w-full bg-indigo-600 text-white font-bold py-4 px-6 rounded-2xl hover:bg-indigo-700 hover:shadow-lg hover:-translate-y-0.5 transition-all disabled:opacity-50 disabled:hover:translate-y-0 disabled:hover:shadow-none flex items-center justify-center gap-2"
                  >
                      {checkoutStatus === "loading" ? (
                        <>
                          <svg className="animate-spin -ml-1 mr-2 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24"><circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle><path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path></svg>
                          Processing...
                        </>
                      ) : "ชำระเงิน"}
                  </button>
                  <button 
                      onClick={() => clearCart(cartId)}
                      className="w-full bg-white text-gray-500 font-medium py-3 rounded-xl hover:text-red-600 transition-colors"
                  >
                      ล้างตะกร้า
                  </button>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
