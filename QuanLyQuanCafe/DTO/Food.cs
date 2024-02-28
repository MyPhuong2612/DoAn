using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Food
    {
        private string name;
        private int iD;
        private int categoryID;
        private float price;
        public Food (int id, string name, int categoryID, float price)
        {
            this.ID = id;
            this.Name = name;
            this.CategoryID = categoryID;
            this.Price = price;

        }
        public Food(DataRow row)
        {
            this.ID = (int)row["id"];
            this.Name = row["name"].ToString();
            this.CategoryID = (int)row["idCategory"];
            this.Price = (float)Convert.ToDouble(row["price"].ToString());
        }
     

        public float Price
        {
            get { return price; }
            set { price = value; }
        }
      

        public int CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }

      
       

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
        

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

    }
}
