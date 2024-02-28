using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyQuanCafe.DTO;

namespace QuanLyQuanCafe
{
    public partial class fTableManager : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }
        public fTableManager( Account acc)
        {
            InitializeComponent();
            this.LoginAccount = acc;
            LoadTable();
            LoadComboboxTable(cbSwitchTable);
        }
        #region Method
        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += "(" + LoginAccount.DisplayName + ")";
        }
        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
            
        }
        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";

          

        }
        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList =  TableDAO.Instance.LoadTableList();

            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableHeight, Height = TableDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += btn_Click;
                btn.Tag = item;
                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Aqua;
                        break;
                    default:
                        btn.BackColor = Color.LightPink;
                        break;
                }

                flpTable.Controls.Add(btn);
            }
        }

        
        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<QuanLyQuanCafe.DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float TotalPrice = 0;
            foreach (QuanLyQuanCafe.DTO.Menu item in listBillInfo)
            {

                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                TotalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            CultureInfo curtule = new CultureInfo("vi-VN");
            txbTotalPrice.Text = TotalPrice.ToString("c", curtule);
        }
        void LoadComboboxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }

        #endregion
        #region Eventsl.
        private void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }
        private void thôngTinTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(LoginAccount);
            f.UpdateAccount += f_UpdateAccount;
            f.ShowDialog();
        }

        private void f_UpdateAccount(object sender, QuanLyQuanCafe.fAccountProfile.AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.InsertFood += f_InsertAccount;
            f.UpdateFood += f_UpdateAccount;
            f.DeleteFood += f_DeleteAccount;
            f.ShowDialog();
        }

        void f_UpdateAccount(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        void f_InsertAccount(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if(lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            
        }

        void f_DeleteAccount(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }
        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadFoodListByCategoryID(id);
        }
        private void fTableManager_Load(object sender, EventArgs e)
        {
            LoadCategory();

        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if(table == null)
            {
                MessageBox.Show("Hãy chọn bàn!!!");
                return;
            }
            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int foodID = (cbFood.SelectedItem as Food).ID;
            int count = (int)nmFoodCount.Value;
            if(idBill == -1)
            {
                BillDAO.Instance.insertBill(table.ID);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);

            }
            ShowBill(table.ID);
            LoadTable();
            
        }
        private void flpTable_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) //thanh toán
        {
            try
            {
                Table t = lsvBill.Tag as Table;
                int mahd_curent = BillDAO.Instance.GetUncheckBillIDByTableID(t.ID);
                int discount = (int)nmDisCount.Value;

                double tongtien = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0]);
                double tongtientra = tongtien - (tongtien / 100) * discount;
                if (mahd_curent != -1)
                {
                    if (MessageBox.Show(string.Format("Thanh Toán \n Tổng tiền: {1}",t.Name,tongtientra), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    {

                        BillDAO.Instance.thanhtoabn(t.ID, discount, (float)tongtien);
                        LoadTable();
                        ShowBill(t.ID);
                    }
                }
            }
            catch
            {
                MessageBox.Show("chưa chọn bàn để thanh toán");
            }
        }

        private void lsvBill_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txbTotalPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            int id1 = (lsvBill.Tag as Table).ID;
            int id2 = (cbSwitchTable.SelectedItem as Table).ID;
            if(MessageBox.Show(string.Format("Bạn có thật sự muốn chuyển bàn {0} qua bàn {1}", (lsvBill.Tag as Table).Name, (cbSwitchTable.SelectedItem as Table).Name), "Thông báo",MessageBoxButtons.OKCancel)==System.Windows.Forms.DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(id1, id2);
                LoadTable();
            }

        }

        #endregion

       

    


       
    }
}
