using System;
using System.Linq;
using System.Web.Mvc;

namespace GasWebApi.Controllers
{
    public class CustomerController : Controller
    {
        GasData.GasAppEntities gasData;

        public CustomerController()
        {
            gasData = new GasData.GasAppEntities();

            gasData.Configuration.LazyLoadingEnabled = false;
            gasData.Configuration.ProxyCreationEnabled = false;
        }
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult getList(int page)
        {
            page -= 1;
            var list = gasData.NguoiDungs.OrderBy(x => x.MaNguoiDung).Skip(page * 15).Take(15).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getKH(int id, string name)
        {
            var list = gasData.NguoiDungs.Where(x=>x.MaNguoiDung==id || x.TenNguoiDung==name).First();
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult getCustomerByType(string type)
        {
            var list = gasData.NguoiDungs.Where(x => x.LoaiNguoiDung == type).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult getCustomerByStaff(int shop)
        {
            var list = gasData.NguoiDungs.Where(x => x.Staffin == shop).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult saveKH(string usn, string sdt)
        {
            //var nd= gasData.NguoiDungs.Where(x => x.SoDienThoai == sdt).First();
            //if (nd != null) return Json("existed",JsonRequestBehavior.AllowGet);

            GasData.NguoiDung kh = new GasData.NguoiDung();
            
            kh.TenNguoiDung = usn;
            kh.SoDienThoai = sdt;
            kh.LoaiNguoiDung = "1";
            kh.MatKhau = CodePassRandom();
            var list = gasData.NguoiDungs.Add(kh);
            gasData.SaveChanges();
            return Json(list,JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult updateKH(GasData.NguoiDung khachHang)
        {
            var res = gasData.NguoiDungs.Where(x => x.MaNguoiDung == khachHang.MaNguoiDung).First();
            res.DiaChi = khachHang.DiaChi;
            res.LoaiNguoiDung = khachHang.LoaiNguoiDung;
            res.MatKhau = khachHang.MatKhau;
            res.TenNguoiDung = khachHang.TenNguoiDung;
            res.Email = khachHang.Email;

            SucceedUpdate scu = new SucceedUpdate();
            scu.Succeed = gasData.SaveChanges();
            return Json(scu, JsonRequestBehavior.AllowGet);
        }

		[HttpPost]
        //  public JsonResult loginKH(GasData.NguoiDung kh)  //tuanna del 19/11/2018
        public JsonResult login(string SoDienThoai, string MatKhau)
        {
            //tuanna del 19/11/2018  var list = gasData.NguoiDungs.Where(x => x.TenNguoiDung == kh.TenNguoiDung && x.MatKhau == kh.MatKhau).FirstOrDefault();
                       
            var list = gasData.NguoiDungs.Where(x => x.SoDienThoai == SoDienThoai && x.MatKhau == MatKhau).FirstOrDefault(); //tuan add  del 19/11/2018  
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        private string CodePassRandom()
        {
            Random r = new Random();
            int res = r.Next(1001, 9889);
            return res.ToString();
        }
    }
}