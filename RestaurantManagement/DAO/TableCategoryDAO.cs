using RestaurantManagement.DTO;
using System.Collections.Generic;
using System.Data;

namespace RestaurantManagement.DAO
{
    public class TableCategoryDAO
    {
        private static TableCategoryDAO instance;

        public static TableCategoryDAO Instance { get => instance == null ? new TableCategoryDAO() : instance; private set => instance = value; }

        private TableCategoryDAO() { }

        public List<TableCategory> GetTableCategories()
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM TableCategory");
            List<TableCategory> tableCategories = new List<TableCategory>();
            foreach (DataRow row in data.Rows)
            {
                TableCategory tableCategory = new TableCategory(row);
                tableCategories.Add(tableCategory);
            }
            return tableCategories;
        }

        public int GetTableCategoryIdByTableId(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT categoryId FROM Tables WHERE id = " + id);
            foreach (DataRow row in data.Rows)
            {
                return (int)row["categoryId"];
            }
            return -1;
        }
    }
}
