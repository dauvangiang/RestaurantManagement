using System.Data;

namespace RestaurantManagement.DTO
{
    public class TableCategory
    {
        private int id;
        private string name;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }

        public TableCategory() { }

        public TableCategory(DataRow row)
        {
            this.Id = (int)row["id"];
            this.Name = row["name"].ToString();
        }
    }
}
