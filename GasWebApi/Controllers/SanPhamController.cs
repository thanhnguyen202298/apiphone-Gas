using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GasWebApi.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        GasData.GasAppEntities gasData;

        public SanPhamController()
        {
            gasData = new GasData.GasAppEntities();

            gasData.Configuration.LazyLoadingEnabled = false;
            gasData.Configuration.ProxyCreationEnabled = false;
        }
        // GET: DonHang
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult getList()
        {
            var list = gasData.SanPhams.ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getsp(string id, string name)
        {
            var list = gasData.SanPhams.Where(x => x.MaSanPham == id || x.TenSanPham == name).First();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult savesp(GasData.SanPham sanPham)
        {
            gasData.SanPhams.Add(sanPham);

            SucceedUpdate scu = new SucceedUpdate();
            scu.Succeed = gasData.SaveChanges();
            return Json(scu, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult updatesp(GasData.SanPham sanPham)
        {
            var res = gasData.SanPhams.Where(x => x.MaSanPham == sanPham.MaSanPham).First();
            res.DonGia = sanPham.DonGia;
            res.LoaiSanPham = sanPham.LoaiSanPham;
            res.MauSac = sanPham.MauSac;

            SucceedUpdate scu = new SucceedUpdate();
            scu.Succeed = gasData.SaveChanges();
            return Json(scu, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public JsonResult deletesp(GasData.SanPham sanPham)
        {
            if (sanPham.MaSanPham == null) return Json("không tồn tại đối tượng");
            gasData.SanPhams.Remove(sanPham);

            SucceedUpdate scu = new SucceedUpdate();
            scu.Succeed = gasData.SaveChanges();
            return Json(scu, JsonRequestBehavior.AllowGet);
        }
    }
}