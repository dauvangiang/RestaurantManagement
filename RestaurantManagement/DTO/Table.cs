using System.Data;

namespace RestaurantManagement.DTO
{
    public class Table
    {
        private int id;
        private string name;
        private string type; //So khach toi da trong 1 ban, truy van tu TableCategory
        private string status; //0: co khach, 1: ko co khach, 2: dat truoc
        public static int width = 80;
        public static int height = 60;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Type { get => type; set => type = value; }
        public string Status { get => status; set => status = value; }

        public Table() { }

        public Table(int id, string name, string type, int status)
        {
            this.Id = id;
            this.Name = name;
            this.Type = type;
            this.Status = status == 0 ? "Có khách" : (status == 1 ? "Trống" : "Đặt trước");
        }

        public Table(DataRow row)
        {
            this.Id = (int)row["id"];
            this.Name = row["name"].ToString();
            this.Type = row["type"].ToString();
            int stt = (int)row["status"];
            this.Status = stt == 0 ? "Có khách" : (stt == 1 ? "Trống" : "Đặt trước");
        }
    }
}
