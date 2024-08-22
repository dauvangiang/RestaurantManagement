using System.Data;

namespace RestaurantManagement.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance { get => instance == null ? new BillDAO() : instance; private set => instance = value; }

        private BillDAO() { }

        public bool InsertBill(int tableId)
        {
            return DataProvider.Instance.ExecuteNonQuery("EXEC USP_InsertBill @tableId", new object[] { tableId }) > 0;
        }
        
        public int GetBillIdByTableId(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("EXEC USP_GetBillUnCheckoutByTableId @tableId", new object[] {id});
            foreach (DataRow row in data.Rows)
            {
                return (int)row["id"];
            }
            return -1;
        }

        public bool Checkout(int tableId)
        {
            int billId = GetBillIdByTableId(tableId);
            if (billId < 0) return false;
            return DataProvider.Instance.ExecuteNonQuery("EXEC USP_Checkout @billId", new object[] { billId }) > 0;
        }

    }
}
