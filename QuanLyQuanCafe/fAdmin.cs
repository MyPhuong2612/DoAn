using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodlist = new BindingSource();
        BindingSource accountList = new BindingSource();
        public fAdmin()
        {
            InitializeComponent();
            Load();
        }

       

        #region methods

        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);
            return listFood;
        }

        void Load()
        {
            dtgvFood.DataSource = foodlist;
            dtgvAccount.DataSource = accountList;

            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListFood();
            LoadAccount();
            LoadCategoryIntoCombodbox(cbFoodCategory);
            AddFoodBinfding();
            AddAccountBinding();
        }

        void AddAccountBinding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nbAccountType.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));

        }

        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }

       
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }

        void AddFoodBinfding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name",true, DataSourceUpdateMode.Never)); // giá trị thay dổi text = vs gia trị thay đổi của name, cho phép sửa giá trị nhưng khong cho sửa ngược lại.
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodFrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));

        }

        void LoadCategoryIntoCombodbox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }

        void LoadListFood()
        {
            foodlist.DataSource = FoodDAO.Instance.GetListFood();
        }

        void AddAccount(string userName, string displayName, int type )
        {
            if(AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công.");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản không thành công!!!");
            }
            LoadAccount();
        }

        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Sửa tài khoản thành công.");
            }
            else
            {
                MessageBox.Show("Sửa tài khoản không thành công!!!");
            }
            LoadAccount();
        }

        void DeleteAccount(string userName)
        {
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công.");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản không thành công!!!");
            }
            LoadAccount();
        }

        #endregion

        #region events

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)nbAccountType.Value;

            AddAccount(userName, displayName, type);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            DeleteAccount(userName);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)nbAccountType.Value;

            EditAccount(userName, displayName, type);
          
        }

        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodlist.DataSource = SearchFoodByName(txbSearchFoodName.Text);
        }
        private void btnViewBill_Click(object sender, EventArgs e)//thống kê
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }
        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value; //lấy 1 seo của ô bất kì trong dtgv

                    Category category = CategoryDAO.Instance.GetCategoryByID(id);
                    cbFoodCategory.SelectedItem = category;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbFoodCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbFoodCategory.SelectedIndex = index;
                }
            }
            catch { }
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodFrice.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công.");
                LoadListFood();
                if ( insertFood != null)
                    insertFood(this, new  EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm món ăn");
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodFrice.Value;
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công.");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa món ăn");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công.");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Xóa lỗi khi sửa món ăn");
            }
        }

        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }
       
        #endregion

        private void btnFristBillPage_Click(object sender, EventArgs e)
        {
            txbPageBill.Text = "1";
        }

        private void btnLastBillPage_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);
            int lastPage = sumRecord / 10;
            if (sumRecord % 10 != 0 )
            
                lastPage++;
            txbPageBill.Text = lastPage.ToString();

        }

        private void txtPageBill_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txbPageBill.Text));

        }

        private void btnPrevioursBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text); //page hiện tại
            if (page > 1)
                page--;
            txbPageBill.Text = page.ToString();     
        }

        private void btnNextBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);
            if (page < sumRecord)
                page++;
            txbPageBill.Text = page.ToString();
        }

    }
}
