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
    
    public partial class SanPham
    {
        public SanPham()
        {
            this.DonHangChiTiets = new HashSet<DonHangChiTiet>();
        }
    
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string DonGia { get; set; }
        public string LoaiSanPham { get; set; }
        public string MauSac { get; set; }
    
        public virtual ICollection<DonHangChiTiet> DonHangChiTiets { get; set; }
    }
}