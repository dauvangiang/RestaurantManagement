using System.Data;

namespace RestaurantManagement.DTO
{
    public class Account
    {
        private int id;
        private string username;
        private string password;
        private string displayName;
        private int type;
        private string changedPassword;

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public int Id { get => id; set => id = value; }
        public int Type { get => type; set => type = value; }
        public string ChangedPassword { get => changedPassword; set => changedPassword = value; }

        public Account() { }
        public Account(DataRow row)
        {
            this.Id = (int)row["id"];
            this.Username = row["username"].ToString();
            this.Password = row["password"].ToString();
            this.DisplayName = row["displayName"].ToString();
            this.Type = (int)row["type"];
            this.ChangedPassword = (int)row["isChangedPassword"] == 0 ? "Chưa" : "Đã đổi";
        }
    }
}
