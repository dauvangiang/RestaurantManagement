using RestaurantManagement.DAO;
using RestaurantManagement.DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RestaurantManagement
{
    public partial class fHome : Form
    {
        private Account loginAccount;
        public Account LoginAccount { get => loginAccount; set { loginAccount = value; ChangeAccount(loginAccount.Type); } }

        BindingSource foods = new BindingSource();
        BindingSource tables = new BindingSource();
        BindingSource tableCategories = new BindingSource();
        BindingSource accounts = new BindingSource();

        public fHome(Account acc)
        {
            InitializeComponent();
            this.LoginAccount = acc;
            Loads();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Methods

        void ChangeAccount(int type)
        {
            tcAdmin.Visible = type == 0;
        }

        private void Loads()
        {
            LoadTables();
            LoadTablesIntoDGV();
            AddTableBinding();
            LoadTableCategory();
            AddTableCategoryBinding();
            LoadFoodCategories();
            LoadFoods();
            AddFoodBinding();
            LoadAccounts();
            AddAccountBingding();
        }

        private void LoadAccounts()
        {
            accounts.DataSource = AccountDAO.Instance.GetAccounts();
            dgvAccount.DataSource = accounts;
            SetFieldNameForDgv(dgvAccount, new string[] { "id", "username", "displayName", "type", "isChangedPassword" }, new string[] { "Mã số", "Tài khoản", "Tên hiển thị", "Loại tài khoản", "Đổi mật khẩu" });
        }

        private void AddAccountBingding()
        {
            txtAccountId.DataBindings.Add(new Binding("Text", dgvAccount.DataSource, "id", true, DataSourceUpdateMode.Never));
            txtAccountUsername.DataBindings.Add(new Binding("Text", dgvAccount.DataSource, "username", true, DataSourceUpdateMode.Never));
            txtAccountDisplayName.DataBindings.Add(new Binding("Text", dgvAccount.DataSource, "displayName", true, DataSourceUpdateMode.Never));
        }

        private void LoadTablesIntoDGV()
        {
            tables.DataSource = TableDAO.Instance.LoadTables();
            dgvTable.DataSource = tables;
            SetFieldNameForDgv(dgvTable, new string[] { "Id", "Name", "Type", "Status" }, new string[] { "Mã số", "Tên bàn", "Danh mục", "Trạng thái" });
        }

        private void AddTableBinding()
        {
            txtTableId.DataBindings.Add(new Binding("Text", dgvTable.DataSource, "Id", true, DataSourceUpdateMode.Never));
            txtTableName.DataBindings.Add(new Binding("Text", dgvTable.DataSource, "Name", true, DataSourceUpdateMode.Never));
        }

        private void LoadTables()
        {

            flpTableForSale.Controls.Clear();

            List<Table> tables = TableDAO.Instance.LoadTables();
            List<Table> tables2 = TableDAO.Instance.LoadTables();

            cbbSwitchTable.DataSource = tables; cbbSwitchTable.DisplayMember = "Name"; cbbSwitchTable.ValueMember = "Id";
            cbbCombineTable.DataSource = tables2; cbbCombineTable.DisplayMember = "Name"; cbbCombineTable.ValueMember = "Id";

            foreach (Table table in tables)
            {
                Button btnForSale = new Button() { Width = Table.width, Height = Table.height}; //thanh vien static cua 1 lop thuoc ve chinh no, ko thuoc ve 1 doi tuong cu the, khong the goi boi mot doi tuong cua lop do
                btnForSale.Text = table.Name + "\n" + table.Type + "\n" + table.Status;
                btnForSale.Tag = table;
                btnForSale.Click += btnTableForSale_Click;

                switch (table.Status)
                {
                    case "Có khách":
                        btnForSale.BackColor = Color.LightPink;
                        break;
                    case "Trống":
                        btnForSale.BackColor = Color.LightGreen;
                        break;
                    case "Đặt trước":
                        btnForSale.BackColor = Color.Yellow;
                        break;
                }
                flpTableForSale.Controls.Add(btnForSale);
            }
        }

        private void ShowBillDetailUnCheckoutByTableId(int id)
        {
            lvFood.Items.Clear();
            List<BillDetail> billDetails = BillDetailDAO.Instance.GetBillDetailByTableId(id);
            if (billDetails == null) return;
            double total = 0;
            foreach (BillDetail item in billDetails)
            {
                ListViewItem lvItem = new ListViewItem(item.Name);
                lvItem.SubItems.Add(item.Count.ToString());
                lvItem.SubItems.Add(item.Price.ToString());
                lvItem.SubItems.Add(item.TotalAmount.ToString());

                lvFood.Items.Add(lvItem);
                total += item.TotalAmount;
            }
            txtTotalAmount.Text = total.ToString();
            txtPayable.Text = total.ToString();
        }

        private void LoadFoodCategories()
        {
            List<FoodCategory> foodCategories = FoodCategoryDAO.Instance.GetFoodCategories();
            cbbFoodCategory.DataSource = foodCategories;
            cbbFoodCategory.DisplayMember = "name"; cbbFoodCategory.ValueMember = "id";
            cbbFoodCategoryAdm.DataSource = FoodCategoryDAO.Instance.GetFoodCategories();
            cbbFoodCategoryAdm.DisplayMember = "name"; cbbFoodCategoryAdm.ValueMember = "id";
        }

        private void LoadFoodsByCategoryId(int id)
        {
            List<Food> foods = FoodDAO.Instance.GetFoodsByFoodCategoryId(id);
            cbbFood.DataSource = foods;
            cbbFood.DisplayMember = "name";
            cbbFood.ValueMember = "id";
        }

        private void AddFood(int tableId, int foodId, int count)
        {
            if (BillDAO.Instance.GetBillIdByTableId(tableId) < 0)
            {
                BillDAO.Instance.InsertBill(tableId);
            }
            BillDetailDAO.Instance.InsertOrUpdateBillDetail(tableId, foodId, count);
        }

        private bool Checkout(int tableId)
        {
            return BillDAO.Instance.Checkout(tableId);
        }

        private bool SwitchTable(int srcTableId, int desTableId)
        {
            return TableDAO.Instance.SwitchTable(srcTableId, desTableId);
        }

        private bool CombineTable(int srcTableId, int desTableId)
        {
            return TableDAO.Instance.CombineTable(srcTableId, desTableId);
        }

        private void LoadFoods(string name = null)
        {
            foods.DataSource = name == null ? FoodDAO.Instance.GetFoods() : FoodDAO.Instance.GetFoods(name);
            dgvFood.DataSource = foods;
            SetFieldNameForDgv(dgvFood, new string[] { "Id", "Name", "Category", "Price", "Status" }, new string[] { "Mã số", "Tên món", "Danh mục", "Giá" , "Trạng thái" });
        }

        private void SetFieldNameForDgv(DataGridView dgv, string[] srcFieldNames, string[] desFieldName)
        {
            int n = srcFieldNames.Length;
            for(int i = 0; i < n; i++)
            {
                dgv.Columns[srcFieldNames[i]].HeaderText = desFieldName[i];
            }
        }

        private void AddFoodBinding()
        {
            txtFoodId.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "Id", true, DataSourceUpdateMode.Never));
            txtFoodName.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            numFoodPrice.DataBindings.Add(new Binding("Value", dgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        private void AddTableCategoryBinding()
        {
            txtTableCategoryId.DataBindings.Add(new Binding("Text", dgvTableCategory.DataSource, "Id", true, DataSourceUpdateMode.Never));
            txtTableCategoryName.DataBindings.Add(new Binding("Text", dgvTableCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
        }

        private void LoadTableCategory()
        {
            tableCategories.DataSource = TableCategoryDAO.Instance.GetTableCategories();
            dgvTableCategory.DataSource = tableCategories;
            SetFieldNameForDgv(dgvTableCategory, new string[] { "Id", "Name"}, new string[] { "Mã số", "Loại bàn" });

            cbbTableCategory.DataSource = TableCategoryDAO.Instance.GetTableCategories();
            cbbTableCategory.DisplayMember = "Name"; cbbTableCategory.ValueMember = "Id";
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Events

        private void txtFoodId_TextChanged(object sender, EventArgs e)
        {
            if (txtFoodId.Text.Trim().Length == 0 || txtFoodId.Text == "########")
            {
                return;
            }
            int foodId = Convert.ToInt32(txtFoodId.Text);
            int foodCategoryId = FoodCategoryDAO.Instance.GetFoodCategoryIdByFoodId(foodId);
            cbbFoodCategoryAdm.SelectedValue = foodCategoryId;

            int foodStatus = FoodDAO.Instance.GetFoodStatusById(foodId);
            cbbFoodStatus.SelectedIndex = foodStatus;
        }
        private void btnTableForSale_Click(object sender, EventArgs e)
        {
            Table table = ((Button)sender).Tag as Table;
            txtCurrentTable.Text = table.Name;
            ShowBillDetailUnCheckoutByTableId(table.Id);
            lvFood.Tag = ((Button)sender).Tag;
        }

        private void cbbFoodCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbb = (ComboBox)sender;
            if (cbb.SelectedItem == null) return;

            FoodCategory selected = (FoodCategory)cbb.SelectedItem;
            int id = selected.Id;

            LoadFoodsByCategoryId(id);
        }

        private void numDiscount_ValueChanged(object sender, EventArgs e)
        {
            double total = Convert.ToDouble(txtTotalAmount.Text);
            txtPayable.Text = (total - Convert.ToDouble(numDiscount.Value)/100 * total).ToString();
        }

        private void btnAddFoodIntoLvFood_Click(object sender, EventArgs e)
        {
            if (lvFood.Tag == null) return;
            int tableId = (lvFood.Tag as Table).Id;
            int foodId = (int)cbbFood.SelectedValue;
            int count = (int)numCount.Value;
            AddFood(tableId, foodId, count);
            ShowBillDetailUnCheckoutByTableId(tableId);
            LoadTables();
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (lvFood.Tag == null) return;
            int tableId = (lvFood.Tag as Table).Id;
            if (Checkout(tableId))
            {
                ShowBillDetailUnCheckoutByTableId(tableId);
                LoadTables();
                MessageBox.Show("Thanh toán thành công: " + txtCurrentTable.Text, "Thông báo");
            }
            else
            {
                MessageBox.Show("Đã có lỗi xảy ra.\nVui lòng thử lại", "Thông báo", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            if (lvFood.Tag == null) return;
            int srcTableId = (lvFood.Tag as Table).Id;
            int desTableId = (int)cbbSwitchTable.SelectedValue;
            while (!SwitchTable(srcTableId, desTableId))
            {
                DialogResult result = MessageBox.Show("Đã có lỗi xảy ra.\nVui lòng thử lại", "Thông báo", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Cancel) break;
            }
            ShowBillDetailUnCheckoutByTableId(srcTableId);
            LoadTables();
        }
        private void btnCombineTable_Click(object sender, EventArgs e)
        {
            if (lvFood.Tag == null) return;
            int srcTableId = (lvFood.Tag as Table).Id;
            int desTableId = (int)cbbCombineTable.SelectedValue;
            while (!CombineTable(srcTableId, desTableId))
            {
                DialogResult result = MessageBox.Show("Đã có lỗi xảy ra.\nVui lòng thử lại", "Thông báo", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Cancel) break;
            }
            ShowBillDetailUnCheckoutByTableId(desTableId);
            LoadTables();
        }

        private void btnRefreshFood_Click(object sender, EventArgs e)
        {
            LoadFoods();
        }

        private void txtSearchFood_TextChanged(object sender, EventArgs e)
        {
            LoadFoods(txtSearchFood.Text);
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            if (txtFoodId.Text.Trim().Length <= 0) return;
            DialogResult result = MessageBox.Show("Xóa món ăn: " + txtFoodName.Text, "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
            {
                return;
            }
            if (!FoodDAO.Instance.DeleteFood(Convert.ToInt32(txtFoodId.Text)))
            {
                MessageBox.Show("Đã có lỗi xảy ra.\nVui lòng thử lại sau", "Thông báo", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return;
            }
            LoadFoods();
            LoadFoodsByCategoryId(Convert.ToInt32(cbbFoodCategory.SelectedValue));
        }

        private void btnSaveFood_Click(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int category = (int)cbbFoodCategoryAdm.SelectedValue;
            double price = Convert.ToDouble(numFoodPrice.Value);
            int stt = (int)cbbFoodStatus.SelectedIndex;
            DialogResult result = MessageBox.Show("Lưu thông tin món ăn: " + txtFoodName.Text, "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
            {
                return;
            }
            if (!FoodDAO.Instance.SaveFood(name, category, price, stt, txtFoodId.Text))
            {
                MessageBox.Show("Đã có lỗi xảy ra.\nVui lòng thử lại sau", "Thông báo", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return;
            }
            LoadFoods();
            LoadFoodsByCategoryId(Convert.ToInt32(cbbFoodCategory.SelectedValue));
            btnDeleteFood.Visible = true;
            btnCancelAddFood.Visible = false;
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            txtFoodId.Text = "########";
            txtFoodName.Text = "";
            cbbFoodCategoryAdm.SelectedIndex = -1;
            numFoodPrice.Value = 0;
            cbbFoodStatus.SelectedIndex = 1;
            btnDeleteFood.Visible = false;
            btnCancelAddFood.Visible = true;
        }

        private void btnCancelAddFood_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn hủy quá trình thêm món ăn không?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;
            txtFoodId.Text = "";
            txtFoodName.Text = "";
            cbbFoodCategoryAdm.SelectedIndex = -1;
            numFoodPrice.Value = 0;
            cbbFoodStatus.SelectedIndex = -1;
            btnDeleteFood.Visible = true;
            btnCancelAddFood.Visible = false;
        }

        private void txtTableId_TextChanged(object sender, EventArgs e)
        {
            if (txtTableId.Text.Trim().Length == 0 || txtTableId.Text == "########")
            {
                return;
            }
            int tableId = Convert.ToInt32(txtTableId.Text);
            int tableCategoryId = TableCategoryDAO.Instance.GetTableCategoryIdByTableId(tableId);
            cbbTableCategory.SelectedValue = tableCategoryId;
        }

        private void btnRefreshTable_Click(object sender, EventArgs e)
        {
            LoadTablesIntoDGV();
        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            txtTableId.Text = "########";
            txtTableName.Text = "";
            cbbTableCategory.SelectedIndex = -1;
            btnDeleteTable.Visible = false;
            btnCancelAddTable.Visible = true;
        }

        private void btnCancelAddTable_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn hủy quá trình thêm bàn không?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;
            txtTableId.Text = "";
            txtTableName.Text = "";
            cbbTableCategory.SelectedIndex = -1;
            btnDeleteTable.Visible = true;
            btnCancelAddTable.Visible = false;
        }

        private void btnSaveTable_Click(object sender, EventArgs e)
        {
            string name = txtTableName.Text;
            int category = (int)cbbTableCategory.SelectedValue;
            DialogResult result = MessageBox.Show("Lưu thông tin bàn: " + txtFoodName.Text, "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
            {
                return;
            }
            if (!TableDAO.Instance.SaveTable(name, category, txtTableId.Text))
            {
                MessageBox.Show("Đã có lỗi xảy ra.\nVui lòng thử lại sau", "Thông báo", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return;
            }
            LoadTablesIntoDGV();
            LoadTables();
            btnDeleteTable.Visible = true;
            btnCancelAddTable.Visible = false;
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            if (txtTableId.Text.Trim().Length <= 0) return;
            DialogResult result = MessageBox.Show("Xóa bàn: " + txtTableName.Text, "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
            {
                return;
            }
            if (!TableDAO.Instance.DeleteTable(Convert.ToInt32(txtTableId.Text)))
            {
                MessageBox.Show("Đã có lỗi xảy ra.\nVui lòng thử lại sau", "Thông báo", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return;
            }
            LoadTables();
            LoadTablesIntoDGV();
        }

        private void btnRefreshTableCategory_Click(object sender, EventArgs e)
        {
            LoadTableCategory();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            txtAccountId.Text = "########";
            txtAccountUsername.Text = "";
            txtAccountDisplayName.Text = "";
            btnDeleteAccount.Visible = false;
            btnCancelAddAccount.Visible = true;
        }

        private void btnCancelAddAccount_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn hủy quá trình thêm tài khoản mới không?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;
            txtAccountId.Text = "";
            txtAccountUsername.Text = "";
            txtAccountDisplayName.Text = "";
            btnDeleteTable.Visible = true;
            btnCancelAddTable.Visible = false;
        }

        private void btnSaveAccount_Click(object sender, EventArgs e)
        {
            string username = txtAccountUsername.Text;
            string displayName = txtAccountDisplayName.Text;
            DialogResult result = MessageBox.Show("Lưu thông tin tài khoản: " + txtAccountUsername.Text, "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
            {
                return;
            }
            AccountDAO.Instance.SaveAccount(username, displayName, txtAccountId.Text);
            LoadAccounts();
            btnDeleteTable.Visible = true;
            btnCancelAddTable.Visible = false;
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            if (txtAccountId.Text.Trim().Length <= 0) return;
            DialogResult result = MessageBox.Show("Xóa tài khoản: " + txtAccountUsername.Text, "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
            {
                return;
            }
            if (!AccountDAO.Instance.DeleteAccountByAdmin(txtAccountUsername.Text))
            {
                MessageBox.Show("Đã có lỗi xảy ra.\nVui lòng thử lại sau", "Thông báo", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return;
            }
            LoadAccounts();
        }

        #endregion
    }
}
