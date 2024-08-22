using System;
using System.Data;

namespace RestaurantManagement.DTO
{
    public class BillDetail
    {
        private int id;
        private string name;
        private int count;
        private double price;
        private double totalAmount;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int Count { get => count; set => count = value; }
        public double Price { get => price; set => price = value; }
        public double TotalAmount { get => totalAmount; set => totalAmount = value; }

        public BillDetail() { }

        public BillDetail(DataRow row)
        {
            this.Id = (int)row["id"];
            this.Name = row["name"].ToString();
            this.Count = (int)row["count"];
            this.Price = Convert.ToDouble(row["price"].ToString());
            this.TotalAmount = Convert.ToDouble(row["totalAmount"].ToString());
        }
    }
}
