using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set { BillDAO.instance = value; }
        }

        private BillDAO() { }

        /// <summary>
        /// Thành công: bill ID
        /// thất bại: -1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetUncheckBillIDByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Bill WHERE idTable = " + id + " AND status = 0");

            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;
            }

            return -1; //id là -1 hong có ai hết
        }
        public void insertBill(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("exec USP_InsertBill @idTable ", new object[]{id});

        }
        public void thanhtoabn(int id, int discount, float tongtt)
        {
            DataProvider.Instance.ExecuteNonQuery("update Bill set status=1, DayCheckOut = GETDATE() , discount = " + discount + ", Tong=" + tongtt + " where idTable=" + id + "");
        }
        public int GetMaxIDBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteSacalar("select MAX(id) from Bill");


            }catch
            {
                return 1;
            }
        }
        //public void CheckOut(int id, int discount , float tong)
        //{
        //    DataProvider.Instance.ExecuteNonQuery("update Bill set DayCheckOut = GETDATE(), status = 1, " + " discount = " + discount + " tong = " + tong + " where id = " + id);
        //}

        public DataTable GetBillListByDate(DateTime checkIn, DateTime checkOut) //lấy bill theo ngày
        {
           return DataProvider.Instance.ExecuteQuery("exec USP_GetListBillByDate @checkIn , @checkOut ", new object[]{ checkIn, checkOut});
        }

        public DataTable GetBillListByDateAndPage(DateTime checkIn, DateTime checkOut, int  pageNum) 
        {
            return DataProvider.Instance.ExecuteQuery("exec USP_GetListBillByDateAndPage @checkIn , @checkOut , @page", new object[] { checkIn, checkOut, pageNum });
        }
        public int GetNumBillListByDate(DateTime checkIn, DateTime checkOut) //lấy bill theo ngày
        {
            return (int)DataProvider.Instance.ExecuteSacalar("exec USP_GetNumtBillByDate @checkIn , @checkOut ", new object[] { checkIn, checkOut });
        }



      
    }
}
