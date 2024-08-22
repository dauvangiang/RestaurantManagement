using RestaurantManagement.DTO;
using System.Collections.Generic;
using System.Data;

namespace RestaurantManagement.DAO
{
    public class FoodCategoryDAO
    {
        private static FoodCategoryDAO instance;

        public static FoodCategoryDAO Instance { get => instance == null ? new FoodCategoryDAO() : instance; private set => instance = value; }

        private FoodCategoryDAO() { }

        public List<FoodCategory> GetFoodCategories()
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM FoodCategory");
            List<FoodCategory> foodCategories = new List<FoodCategory>();
            foreach (DataRow row in data.Rows)
            {
                FoodCategory foodCategory = new FoodCategory(row);
                foodCategories.Add(foodCategory);
            }
            return foodCategories;
        }

        public int GetFoodCategoryIdByFoodId(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT categoryId FROM Food WHERE id = " + id);
            foreach (DataRow row in data.Rows)
            {
                return (int)row["categoryId"];
            }
            return -1;
        }
    }
}
