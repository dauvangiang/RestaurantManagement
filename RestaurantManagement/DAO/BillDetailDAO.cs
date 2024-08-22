using RestaurantManagement.DTO;
using System.Collections.Generic;
using System.Data;

namespace RestaurantManagement.DAO
{
    public class BillDetailDAO
    {
        private static BillDetailDAO instance;

        public static BillDetailDAO Instance { get => instance == null ? new BillDetailDAO() : instance; private set => instance = value; }

        private BillDetailDAO() { }

        public List<BillDetail> GetBillDetailByTableId(int id)
        {
            int billId = BillDAO.Instance.GetBillIdByTableId(id);
            if (billId < 0) return null; 
 
            List<BillDetail> billDetails = new List<BillDetail>();
            DataTable data = DataProvider.Instance.ExecuteQuery("EXEC USP_GetBillDetailByBillId @billId", new object[] { billId });
                foreach (DataRow row in data.Rows)
                {
                    BillDetail bd = new BillDetail(row);
                    billDetails.Add(bd);
                }
                return billDetails;
        }

        public bool InsertOrUpdateBillDetail(int tableId, int foodId, int count)
        {
            int billId = BillDAO.Instance.GetBillIdByTableId(tableId);
            string query = "EXEC USP_InsertOrUpdateBillDetail @billId , @foodId , @count";
            return DataProvider.Instance.ExecuteNonQuery(query, new object[] { billId, foodId, count }) > 0;
        }
    }
}
