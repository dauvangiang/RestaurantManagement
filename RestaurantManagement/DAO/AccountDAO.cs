using RestaurantManagement.DTO;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace RestaurantManagement.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance { get => instance == null ? new AccountDAO() : instance; private set => instance = value; }

        private AccountDAO() { }

        public bool Login(string username, string password)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(password);
            byte[] hashData = new MD5CryptoServiceProvider().ComputeHash(temp);
            string hashPass = "";
            foreach (byte b in hashData)
            {
                hashPass += b;
            }

            string query = "EXEC USP_Login @username , @password";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { username, hashPass });
            return result.Rows.Count > 0;
        }

        public Account GetAccount(string username)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("EXEC USP_GetAccountByUsername @username", new object[] { username });
            foreach (DataRow row in data.Rows)
            {
                return new Account(row);
            }
            return null;
        }

        public DataTable GetAccounts()
        {
            string query = "SELECT id, username, displayName, type, isChangedPassword FROM Account";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            return data;
        }

        public bool DeleteAccountByAdmin(string username)
        {
            string query = "DELETE FROM Account WHERE username = '" + username + "'";
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool SaveAccount(string username, string displayName, string id = "########")
        {
            int result;
            if (id == "########")
            {
                result = DataProvider.Instance.ExecuteNonQuery("USP_SaveAccount @username , @displayName ", new object[] { username, displayName });
            }
            else
            {
                result = DataProvider.Instance.ExecuteNonQuery("USP_SaveAccount @username , @displayName  , @id", new object[] { username, displayName, id });
            }
            return result > 0;
        }
    }
}
