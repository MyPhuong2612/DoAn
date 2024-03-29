﻿using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAccountProfile : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }
        }
        public fAccountProfile( Account acc )
        {
            InitializeComponent();
            LoginAccount = acc;
        }
        void UpdateAccountInfo() // muon doi ten phai nhap mat khau--- muon doi mat khau thi nhap mat khau va mat khau moi
        {
            string displayName = txbDisPlayName.Text;
            string password = txbPassWord.Text;
            string newpass = txbNewPassWord.Text;
            string reenterPass = txbReEnterPass.Text;
            string userName = txbUserName.Text;

            if(!newpass.Equals(reenterPass))
            {
                MessageBox.Show(" Vui lòng nhập lại mật khẩu đúng với mật khẩu mới!!!");
            }
            else
            {
                if(AccountDAO.Instance.UpdateAccount(userName, displayName, password, newpass))
                {
                    MessageBox.Show("Cập nhật thành công.");
                    if(updateAccount != null)
                    {
                        updateAccount(this, new AccountEvent(AccountDAO.Instance.GetAccountByUserName(userName)));
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập đúng mật khẩu!!!");
                }
            }
        }

        private event EventHandler<AccountEvent> updateAccount;  // dữ liệu from cha sang con và ngược lại

        public event EventHandler<AccountEvent> UpdateAccount
        {
            add { updateAccount += value; }  //update xong tra ve account moi
            remove { updateAccount -= value; }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }

        public void ChangeAccount(Account acc )
        {
            txbUserName.Text = LoginAccount.UserName;
            txbDisPlayName.Text = LoginAccount.DisplayName;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public class AccountEvent:EventArgs
        {
            private Account acc;

            public Account Acc
            {
                get { return acc; }
                set { acc = value; }
            }


            public AccountEvent(Account acc)
            {
                this.Acc = acc;
            }
        }

        
    }
}
