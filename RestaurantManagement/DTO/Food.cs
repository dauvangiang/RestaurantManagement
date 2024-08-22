using System;
using System.Data;

namespace RestaurantManagement.DTO
{
    public class Food
    {
        private int id;
        private string name;
        private string category;
        private double price;
        private int status;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Category { get => category; set => category = value; }
        public double Price { get => price; set => price = value; }
        public int Status { get => status; set => status = value; }

        public Food() { }

        public Food(DataRow row)
        {
            try
            {
                this.Id = (int)row["id"];
                this.Name = row["name"].ToString();
                this.Category = row["category"].ToString();
                this.Price = Convert.ToDouble(row["price"].ToString());
                this.Status = (int)row["status"];
            }
            catch
            {
                this.Id = (int)row["Mã số"];
                this.Name = row["Tên món"].ToString();
                this.Category = row["Danh mục"].ToString();
                this.Price = Convert.ToDouble(row["Giá"].ToString());
                this.Status = (int)row["Trạng thái"];
            }
        }
    }
}
