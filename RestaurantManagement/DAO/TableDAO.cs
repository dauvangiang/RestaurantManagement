using RestaurantManagement.DTO;
using System.Collections.Generic;
using System.Data;

namespace RestaurantManagement.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;
        public static int w = 10;
        public static int h = 20;

        public static TableDAO Instance { get => instance == null ? new TableDAO() : instance; set => instance = value; }

        private TableDAO() { }

        public List<Table> LoadTables()
        {
            List<Table> tables = new List<Table>();
            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTables");
            foreach (DataRow row in data.Rows)
            {
                Table table = new Table(row);
                tables.Add(table);
            }
            return tables;
        }

        public bool SwitchTable(int srcTableId, int desTableId)
        {
            string query = "EXEC USP_SwitchTable @srcTableId , @desTableId";
            return DataProvider.Instance.ExecuteNonQuery(query, new object[] { srcTableId, desTableId }) > 0;
        }

        public int GetTableStatus(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT status FROM Tables WHERE id = " + id);
            foreach (DataRow row in data.Rows)
            {
                return (int)row["status"];
            }
            return -1;
        }

        public bool CombineTable(int srcTableId, int desTableId)
        {
            // Nếu có 1 bàn trống thì không thể gộp
            if (GetTableStatus(srcTableId) == 1 || GetTableStatus(desTableId) == 1)
                return false;

            List<BillDetail> billDetails = BillDetailDAO.Instance.GetBillDetailByTableId(srcTableId);
            try
            {
                foreach (BillDetail billDetail in billDetails)
                {
                    int foodId = FoodDAO.Instance.GetFoodIdByName(billDetail.Name);
                    BillDetailDAO.Instance.InsertOrUpdateBillDetail(desTableId, foodId, billDetail.Count);
                }

                int srcBillId = BillDAO.Instance.GetBillIdByTableId(srcTableId);
                return DataProvider.Instance.ExecuteNonQuery("EXEC USP_ProcessAfterCombineTable @billId", new object[] { srcBillId }) > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool SaveTable(string name, int category, string id)
        {
            int result;
            if (id == "########")
            {
                result = DataProvider.Instance.ExecuteNonQuery("USP_SaveTable @name , @category", new object[] { name, category });
            }
            else
            {
                result = DataProvider.Instance.ExecuteNonQuery("USP_SaveTable @name , @category , @id", new object[] { name, category, id });
            }
            return result > 0;
        }

        public bool DeleteTable(int id)
        {
            return DataProvider.Instance.ExecuteNonQuery("UPDATE Tables SET isExist = 0 WHERE id = " + id) > 0;
        }
    }
}
