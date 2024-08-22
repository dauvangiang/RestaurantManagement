using RestaurantManagement.DTO;
using System.Collections.Generic;
using System.Data;

namespace RestaurantManagement.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance { get => instance == null ? new FoodDAO() : instance; private set => instance = value; }

        private FoodDAO() { }

        public List<Food> GetFoodsByFoodCategoryId(int id)
        {

            DataTable data = DataProvider.Instance.ExecuteQuery("EXEC USP_GetFoodsByFoodCategoryId @ffoodCategoryId", new object[] {id});
            List<Food> foods = new List<Food>();
            foreach (DataRow row in data.Rows)
            {
                Food food = new Food(row);
                foods.Add(food);
            }
            return foods;
        }

        public int GetFoodIdByName(string name)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("EXEC USP_GetFoodIdByName @foodName", new object[] { name });
            foreach(DataRow row in data.Rows)
            {
                return (int)row["id"];
            }
            return -1;
        }

        public List<Food> GetFoods(string name = null)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery(name == null ? "EXEC USP_GetFoods" : "EXEC USP_GetFoods @name", new object[] {name});
            List<Food> foods = new List<Food>();
            foreach (DataRow row in data.Rows)
            {
                Food food = new Food(row);
                foods.Add(food);
            }
            return foods;
        }

        public int GetFoodStatusById(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT status FROM Food WHERE id = " + id);
            foreach (DataRow row in data.Rows)
            {
                return (int)row["status"];
            }
            return -1;
        }

        public bool DeleteFood(int id)
        {
            return DataProvider.Instance.ExecuteNonQuery("EXEC USP_DeleteFood @id", new object[] { id }) > 0;
        }

        public bool SaveFood(string name, int category, double price, int stt, string id = "########")
        {
            int result;
            if (id == "########")
            {
                result = DataProvider.Instance.ExecuteNonQuery("USP_SaveFood @name , @category , @price , @stt", new object[] { name, category, price, stt });
            }
            else
            {
                result = DataProvider.Instance.ExecuteNonQuery("USP_SaveFood @name , @category , @price , @stt , @id", new object[] { name, category, price, stt, id });
            }
            return result > 0;
        }
    }
}
