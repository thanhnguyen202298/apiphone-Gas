//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GasData
{
    using System;
    using System.Collections.Generic;
    
    public partial class DonHangChiTiet
    {
        public System.Guid DonHangChiTiet1 { get; set; }
        public int MaDonHang { get; set; }
        public string MaSanPham { get; set; }
        public Nullable<decimal> SoLuong { get; set; }
        public Nullable<decimal> DonGia { get; set; }
        public Nullable<decimal> ThanhTien { get; set; }
        public Nullable<byte> ChietKhauPercent { get; set; }
        public Nullable<decimal> GiamTienMat { get; set; }
        public string Mau { get; set; }
        public string TenSanPham { get; set; }
    
        public virtual DonHang DonHang { get; set; }
        public virtual SanPham SanPham { get; set; }
    }
}