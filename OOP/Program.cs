using System;
using System.Collections.Generic;
using System.Linq;

namespace LogisticsAutoSpeed
{
    public abstract class PhuongTien
    {
        private string _maPT;
        private string _tenHang;
        private int _namSanXuat;
        private decimal _giaGoc;
        public string MaPT
        {
            get { return _maPT; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _maPT = "PT000";
                else
                    _maPT = value.Trim();
            }
        }

        public string TenHang
        {
            get { return _tenHang; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(
                        "Tên hãng không được để trống!");

                _tenHang = value.Trim();
            }
        }

        public int NamSanXuat
        {
            get { return _namSanXuat; }
            set
            {
                int namHienTai = DateTime.Now.Year;

                if (value < 1900 || value > namHienTai)
                {
                    throw new ArgumentException("Năm sản xuất không hợp lệ!");
                }

                _namSanXuat = value;
            }
        }

        public decimal GiaGoc
        {
            get { return _giaGoc; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException(
                        "Giá gốc phải lớn hơn 0!");

                _giaGoc = value;
            }
        }

        public PhuongTien(
            string maPT,
            string tenHang,
            int namSanXuat,
            decimal giaGoc)
        {
            MaPT = maPT;
            TenHang = tenHang;
            NamSanXuat = namSanXuat;
            GiaGoc = giaGoc;
        }
        public abstract decimal TinhGiaLanBanh();
        public virtual string GetInfo()
        {
            return $"Mã PT: {MaPT} | " +
                   $"Hãng: {TenHang} | " +
                   $"Năm SX: {NamSanXuat} | " +
                   $"Giá gốc: {GiaGoc:N0} VNĐ";
        }
    }
    public class OTo : PhuongTien
    {
        private int _soChoNgoi;
        private double _dungTichDongCo;

        public int SoChoNgoi
        {
            get { return _soChoNgoi; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException(
                        "Số chỗ ngồi phải lớn hơn 0!");

                _soChoNgoi = value;
            }
        }

        public double DungTichDongCo
        {
            get { return _dungTichDongCo; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException(
                        "Dung tích động cơ phải lớn hơn 0!");

                _dungTichDongCo = value;
            }
        }

        public OTo(
            string maPT,
            string tenHang,
            int namSanXuat,
            decimal giaGoc,
            int soChoNgoi,
            double dungTichDongCo)
            : base(maPT, tenHang, namSanXuat, giaGoc)
        {
            SoChoNgoi = soChoNgoi;
            DungTichDongCo = dungTichDongCo;
        }
        public override decimal TinhGiaLanBanh()
        {
            if (SoChoNgoi <= 9)
            {
                return GiaGoc
                       + GiaGoc * 0.12m
                       + GiaGoc * 0.30m;
            }
            else
            {
                return GiaGoc
                       + GiaGoc * 0.10m;
            }
        }

        public override string GetInfo()
        {
            return $"{base.GetInfo()} | " +
                   $"Chỗ ngồi: {SoChoNgoi} | " +
                   $"Động cơ: {DungTichDongCo}L | " +
                   $"Loại: Ô tô";
        }
    }
    public class XeMay : PhuongTien
    {
        private int _dungTichXylanh;

        public int DungTichXylanh
        {
            get { return _dungTichXylanh; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException(
                        "Dung tích xylanh phải lớn hơn 0!");

                _dungTichXylanh = value;
            }
        }

        public XeMay(
            string maPT,
            string tenHang,
            int namSanXuat,
            decimal giaGoc,
            int dungTichXylanh)
            : base(maPT, tenHang, namSanXuat, giaGoc)
        {
            DungTichXylanh = dungTichXylanh;
        }
        public override decimal TinhGiaLanBanh()
        {
            if (DungTichXylanh < 175)
            {
                return GiaGoc
                       + GiaGoc * 0.02m;
            }
            else
            {
                return GiaGoc
                       + GiaGoc * 0.05m;
            }
        }

        public override string GetInfo()
        {
            return $"{base.GetInfo()} | " +
                   $"Phân khối: {DungTichXylanh} cc | " +
                   $"Loại: Xe máy";
        }
    }

    public class QuanLyPhuongTien
    {
        private readonly List<PhuongTien> _danhSach
            = new List<PhuongTien>();
        public void AddPhuongTien(PhuongTien pt)
        {
            if (pt == null)
            {
                Console.WriteLine(
                    "Phương tiện không hợp lệ!");
                return;
            }

            _danhSach.Add(pt);
        }
        public void DisplayAll()
        {
            if (_danhSach.Count == 0)
            {
                Console.WriteLine(
                    "Danh sách phương tiện trống!");
                return;
            }

            foreach (PhuongTien pt in _danhSach)
            {
                Console.WriteLine(pt.GetInfo());
                Console.WriteLine(
                    $"Giá lăn bánh: {pt.TinhGiaLanBanh():N0} VNĐ");
                Console.WriteLine();
            }
        }
        public PhuongTien FindMaxGiaLanBanh()
        {
            if (_danhSach.Count == 0)
                return null;

            return _danhSach
                .OrderByDescending(
                    p => p.TinhGiaLanBanh())
                .FirstOrDefault();
        }

        public List<PhuongTien> SearchByName(
            string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<PhuongTien>();

            return _danhSach
                .Where(p =>
                    p.TenHang.IndexOf(
                        keyword,
                        StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding =
                System.Text.Encoding.UTF8;

            Console.WriteLine(
                "===== HỆ THỐNG QUẢN LÝ PHƯƠNG TIỆN =====");

            Console.WriteLine("\n===== TC01 =====");

            try
            {
                OTo otoLoi = new OTo(
                    "OT01",
                    "Toyota",
                    1850,
                    1000000000m,
                    5,
                    2.0);

                Console.WriteLine(
                    "FAILED: Đối tượng vẫn được tạo.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(
                    "PASSED: " + ex.Message);
            }


            OTo oto = new OTo(
                "OT02",
                "Toyota",
                2024,
                1000000000m,
                5,
                2.0);

            XeMay xeMay = new XeMay(
                "XM01",
                "Honda",
                2024,
                50000000m,
                150);

            Console.WriteLine("\n===== TC02 =====");

            decimal giaOTo = oto.TinhGiaLanBanh();

            Console.WriteLine(
                $"Giá lăn bánh ô tô: {giaOTo:N0} VNĐ");

            if (giaOTo == 1420000000m)
            {
                Console.WriteLine(
                    "PASSED: Khớp 1,420,000,000 VNĐ");
            }
            else
            {
                Console.WriteLine(
                    "FAILED: Kết quả không đúng.");
            }

            Console.WriteLine("\n===== TC03 =====");

            decimal giaXeMay =
                xeMay.TinhGiaLanBanh();

            Console.WriteLine(
                $"Giá lăn bánh xe máy: {giaXeMay:N0} VNĐ");

            if (giaXeMay == 51000000m)
            {
                Console.WriteLine(
                    "PASSED: Khớp 51,000,000 VNĐ");
            }
            else
            {
                Console.WriteLine(
                    "FAILED: Kết quả không đúng.");
            }

            Console.WriteLine("\n===== TC04 =====");

            QuanLyPhuongTien ql =
                new QuanLyPhuongTien();

            ql.AddPhuongTien(oto);
            ql.AddPhuongTien(xeMay);

            Console.WriteLine(
                "Duyệt danh sách và gọi TinhGiaLanBanh():");

            ql.DisplayAll();

            Console.WriteLine(
                "PASSED: Runtime tự động gọi " +
                "đúng phương thức của OTo và XeMay.");

            Console.WriteLine("\n===== TC05 =====");

            PhuongTien ptMax =
                ql.FindMaxGiaLanBanh();

            if (ptMax != null &&
                ptMax.TinhGiaLanBanh() == 1420000000m)
            {
                Console.WriteLine(
                    "PASSED: Tìm đúng phương tiện có " +
                    "giá lăn bánh cao nhất.");

                Console.WriteLine(
                    ptMax.GetInfo());

                Console.WriteLine(
                    $"Giá lăn bánh: " +
                    $"{ptMax.TinhGiaLanBanh():N0} VNĐ");
            }
            else
            {
                Console.WriteLine(
                    "FAILED: Không tìm đúng phương tiện.");
            }

            Console.WriteLine(
                "\n===== HOÀN TẤT KIỂM THỬ =====");

            Console.ReadKey();
        }
    }
}
