using RestaurantManagement.DAO;
using RestaurantManagement.DTO;
using System;
using System.Data;
using System.Windows.Forms;

namespace RestaurantManagement
{
    public partial class fLogin : Form
    {
        public fLogin()
        {
            if (!IsExistDB("RESTAURANT"))
            {
                DialogResult result = MessageBox.Show("Để quá trình cài đặt hoàn tất, bạn hãy làm theo các bước sau:\n1. Mở terminal (Window + R, nhập 'cmd', Enter)\n2. Nhập lệnh: sqlcmd -E -S .\\SQLExpress -i \"pathname\" (Gợi ý: pathname có dạng: <Đường dẫn đến thư mục lưu trữ ứng dụng của bạn>\\Data\\data.sql)\n3. Nhấn Enter, đợi quá trình hoàn tất để bắt đầu\n\n(Tài khoản đăng nhập: admin, mật khẩu: 0)", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Cancel) { return; };
            }
            InitializeComponent();
        }

        private bool IsExistDB(string name)
        {
            try
            {
                DataTable data = DataProvider.Instance.ExecuteQuery(String.Format("SELECT database_id FROM sys.databases WHERE name = '{0}'", name));
                return data.Rows.Count > 0;
            }
            catch
            {
                return false;
            }
            
        }

        private int Login(string username, string password)
        {
            try
            {
                bool check = AccountDAO.Instance.Login(username, password);
                return check ? 1 : 0;
            } catch { return -1; };
        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            string username = txtUsername.Text, password = txtPassword.Text;
            if (Login(username, password) == 0)
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không đúng.\nVui lòng thử lại !", "Đăng nhập thất bại");
                return;
            } else if (Login(username, password) == -1)
            {
                MessageBox.Show("Đã có lỗi xảy ra trong quá trình cài đặt\nVui lòng khởi động lại ứng dụng và làm theo hướng dẫn !", "Lỗi cài đặt");
                this.Close();
                return;
            }
            Account loginAcc = AccountDAO.Instance.GetAccount(username);
            fHome f = new fHome(loginAcc);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }
    }
}
