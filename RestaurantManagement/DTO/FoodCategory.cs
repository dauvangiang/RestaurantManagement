using System.Data;

namespace RestaurantManagement.DTO
{
    public class FoodCategory
    {
        private int id;
        private string name;

        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }

        public FoodCategory() { }

        public FoodCategory(DataRow row)
        {
            this.Id = (int)row["id"];
            this.Name = row["name"].ToString();
        }
    }
}
