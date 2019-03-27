using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GasWebApi.Controllers
{
    public class DonHangController : BaseController
    {
        GasData.GasAppEntities gasData;
        List<string> shopStat = new List<string>();
        List<string> shipperStat = new List<string>();

        public DonHangController()
        {
            gasData = new GasData.GasAppEntities();

            gasData.Configuration.LazyLoadingEnabled = false;
            gasData.Configuration.ProxyCreationEnabled = false;

            shipperStat.Add("Đang giao");
            shipperStat.Add("Đã giao");
            shipperStat.Add("Giao hàng thất bại");

            shopStat.Add("Cửa hàng xác nhận");
            shopStat.Add("Đã hủy");

        }
        // GET: DonHang
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult getStatusList()
        {
            var list = gasData.TrangThaiDhs.OrderBy(x=>x.TrangThaiId).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult getList(int page)
        {
            page -= 1;
            var list = gasData.DonHangs.OrderByDescending(x => x.MaDonHang).Skip(page * 15).Take(15).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getListByUser(int page, int Usn, string status)
        {
            page -= 1;
            var list = gasData.DonHangs.Where(x=>x.MaNguoiDung== Usn && x.TrangThai.Contains(status)).OrderByDescending(x => x.MaDonHang).Skip(page * 15).Take(15).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

       // get dh off shop
       //
       
        [HttpGet]
        public JsonResult getListByShop(int page, int Shop,  string status)
        {
            page -= 1;
            var list = gasData.DonHangs.Where(x => (x.ShopId == Shop||x.MaNguoiDung == Shop) && x.TrangThai.Contains(status)).OrderByDescending(x => x.MaDonHang).Skip(page * 15).Take(15).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getListByPostMan(int page, int postman, string status)
        {
            page -= 1;
            var list = gasData.DonHangs.Where(x => (x.PostMan == postman || x.MaNguoiDung == postman) && x.TrangThai.Contains(status)).OrderByDescending(x => x.MaDonHang).Skip(page * 15).Take(15).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //
        //Notify shop Asynchronize
        //
        [HttpGet]
        public JsonResult getNotifyByShop(int Shop)
        {
            var list = gasData.DonHangs.Where(x => x.ShopId == Shop && (x.SynchronizedShop==null || x.SynchronizedShop == false)).OrderByDescending(x => x.MaDonHang).ToList();
            foreach (GasData.DonHang dh in list)
                dh.SynchronizedShop = true;
            gasData.SaveChanges();

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult getNotifyByPostMan(int postman)
        {
            var list = gasData.DonHangs.Where(x => x.PostMan == postman && (x.SyncPostMan == null || x.SyncPostMan == false)).OrderByDescending(x => x.MaDonHang).ToList();
            foreach (GasData.DonHang dh in list)
                dh.SyncPostMan = true;
            gasData.SaveChanges();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //
        //Notify User Asynchronize
        //
        [HttpGet]
        public JsonResult getNotifyByUser(int usn)
        {
            var list = gasData.DonHangs.Where(x => x.MaNguoiDung == usn && (x.SynchronizedUser==null|| x.SynchronizedUser == false) && x.TrangThai != "đặt hàng").OrderByDescending(x => x.MaDonHang).ToList();
            foreach (GasData.DonHang dh in list)
                dh.SynchronizedUser = true;
            gasData.SaveChanges();
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        // filter by date notify
        ////
        //yyyy-MM-dd
        //
        //
        [HttpGet]
        public JsonResult getListByDateShop(int page, int Shop, String dateTime)
        {
            page -= 1;
            DateTime dateT = Convert.ToDateTime(dateTime);
            var list = gasData.DonHangs.Where(x => x.ShopId == Shop && x.ThoiGianDatHang > dateT && (x.TrangThai.ToLower().Contains("chờ") || x.TrangThai.Contains("-1"))).OrderByDescending(x => x.MaDonHang).Skip(page * 15).Take(15).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // filter by date notify//
        //yyyy-MM-dd
        //
        //
        [HttpGet]
        public JsonResult getListByDateUser(int page, int Usn, string dateTime)
        {
            page -= 1; DateTime dateT =Convert.ToDateTime(dateTime);
            var list = gasData.DonHangs.Where(x => x.MaNguoiDung == Usn && x.ThoiGianDatHang > dateT && !(x.TrangThai.ToLower().Contains("chờ")|| x.TrangThai.Contains("-1"))).OrderByDescending(x => x.MaDonHang).Skip(page * 15).Take(15).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getDh(int id, int idUser)
        {
            var list = gasData.DonHangs.Where(x => x.MaDonHang == id || x.MaNguoiDung == idUser).First();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getDetailsDh(int id)
        {
            var list = gasData.DonHangChiTiets.Where(x => x.MaDonHang == id).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult saveDh(GasData.DonHang donhang)//include details
        {
            foreach(GasData.DonHangChiTiet dct in donhang.DonHangChiTiets){
                dct.DonHangChiTiet1 = Guid.NewGuid();
            }
            donhang.ThoiGianDatHang = DateTime.Now;

            gasData.DonHangs.Add(donhang);
            SucceedUpdate scu = new SucceedUpdate();
            scu.Succeed = gasData.SaveChanges();
            return Json(scu, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult updateDh(GasData.DonHang donhang)
        {
            var res = gasData.DonHangs.Where(x=>x.MaDonHang== donhang.MaDonHang).First();
            res.MaNguoiDung = donhang.MaNguoiDung;
            res.NgayGioTrangThai = DateTime.Now;
            res.DanhGia = donhang.DanhGia;
            res.LoaiYeuCau = donhang.LoaiYeuCau;
            res.TongTien = donhang.TongTien;
            if (!res.TrangThai.Equals(donhang.TrangThai))
            {
                res.TrangThai = donhang.TrangThai;
                if (shopStat.Contains(donhang.TrangThai))
                    res.SyncPostMan = false;
                else if (shipperStat.Contains(donhang.TrangThai))
                    res.SynchronizedShop = false;
                res.SynchronizedUser = false;
            }
            if (donhang.TrangThai == "Đã giao" && donhang.ThoiGianGiao == null)
                donhang.ThoiGianGiao = DateTime.Now;
            res.PostMan = donhang.PostMan;
            SucceedUpdate scu = new SucceedUpdate();
            scu.Succeed = gasData.SaveChanges();
            return Json(scu, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult updateChitietDh(List<GasData.DonHangChiTiet> donhangchitiets)
        {
            SucceedUpdate scu = new SucceedUpdate();
            scu.Succeed = 0;
            GasData.DonHangChiTiet item;
            foreach (GasData.DonHangChiTiet ct in donhangchitiets)
            {
                item = gasData.DonHangChiTiets.Where(x => x.DonHangChiTiet1 == ct.DonHangChiTiet1).FirstOrDefault();
                if (item != null)
                    gasData.DonHangChiTiets.Remove(item);
                gasData.SaveChanges();
                gasData.DonHangChiTiets.Add(ct);
                scu.Succeed++;
            }
            scu.Succeed += gasData.SaveChanges() < 0 ? -1 : 0;
            return Json(scu, JsonRequestBehavior.AllowGet);
        }
    }
}