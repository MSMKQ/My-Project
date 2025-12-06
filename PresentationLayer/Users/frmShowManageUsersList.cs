using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer.Users
{
    public partial class frmShowManageUsersList : Form
    {
        private DataTable _Users;
        public static int? SelectedRow;

        public frmShowManageUsersList()
        {
            InitializeComponent();
        }

        private int? _SelectedRow()
        {
            if (dgvUsers.CurrentRow?.Cells[0].Value == null || dgvUsers.CurrentRow?.Cells[0].Value == DBNull.Value)
                return null;

            if (int.TryParse(dgvUsers.CurrentRow.Cells[0].Value.ToString(), out int Num))
            {
                return Num;
            }

            return null;
        }

        private void _Records()
        {
            int? Id = _SelectedRow();
            lblRecords.Text = $"# Records [{dgvUsers.RowCount}] User [{Id}]";
            SelectedRow = Id;
        }

        private void _FillGenderInComboBox()
        {
            foreach (DataRow row in _Users.Rows)
            {
                if (!cbIsActive.Items.Contains(row["IsActive"]))
                {
                    cbIsActive.Items.Add(row["IsActive"]);
                }
            }

            cbIsActive.Items.Insert(0, "None");
        }

        private void _table()
        {
            cbFilter.SelectedIndex = 0;
            _FillGenderInComboBox();

            if (dgvUsers.RowCount > 0)
            {
                var headers = new(string name, string header, DataGridViewAutoSizeColumnMode Mode)[]
                {
                    ("UserID", "User ID", DataGridViewAutoSizeColumnMode.AllCells),
                    ("PersonID", "Person ID", DataGridViewAutoSizeColumnMode.AllCells),
                    ("FullName", "Full Name", DataGridViewAutoSizeColumnMode.NotSet),
                    ("Username", "User Name", DataGridViewAutoSizeColumnMode.AllCells),
                    ("IsActive", "Is Active", DataGridViewAutoSizeColumnMode.AllCells)
                };


                foreach (var header in headers)
                {
                    var col = dgvUsers.Columns[header.name];

                    if (col != null)
                    {
                        col.HeaderText = header.header;
                        col.AutoSizeMode = header.Mode;
                    }
                }
            }

            _Records();
        }

        private void frmShowManageUsersList_Load(object sender, EventArgs e)
        {
            dgvUsers.ClearSelection();
            _Users = ClsUser.GetUsers();
            dgvUsers.DataSource = _Users;
            _table();
        }

        private void dgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            _Records();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilter.Text != "None" && cbFilter.Text != "Is Active");
            cbIsActive.Visible = (cbFilter.Text == "Is Active");

            if (cbFilter.Text == "None")
            {
                _Users.DefaultView.RowFilter = null;
            }
            else if (txtFilterValue.Visible)
            {
                txtFilterValue.Clear();
                txtFilterValue.Focus();
            }
            else if (cbIsActive.Visible)
            {
                cbIsActive.SelectedIndex = 0;
                cbIsActive.Focus();
            }

            _Records();
        }

        private void showUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.UserInfo);
            }

            frmShowManageUsersList_Load(null, null);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if ( MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.CreateUser);
            }

            frmShowManageUsersList_Load(null, null);
        }

        private void showCreateNewUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.CreateUser);
            }

            frmShowManageUsersList_Load(null, null);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int Id = _SelectedRow().Value;

            if (MessageBox.Show($"Are you sure you want delete User No.{Id} ?", $"Delete User No.{Id}", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            if (ClsUser.Delete(Id))
            {
                MessageBox.Show($"This User No.{Id} was Delete it Saccessfuly.", $"User No.{Id} Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                frmShowManageUsersList_Load(null, null);
            }
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.UpdateUser);
            }

            frmShowManageUsersList_Load(null, null);
        }

        private void showChangePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.UserChangePassword);
            }

            frmShowManageUsersList_Load(null, null);
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterCol = string.Empty;

            switch (cbFilter.Text)
            {
                case "User ID":
                    FilterCol = "UserID";
                    break;

                case "Person ID":
                    FilterCol = "PersonID";
                    break;

                case "Full Name":
                    FilterCol = "FullName";
                    break;

                case "User Name":
                    FilterCol = "Username";
                    break;

                default:
                    FilterCol = "None";
                    break;
            }

            if (FilterCol == "None" || cbFilter.Text == "None" || txtFilterValue.Text == string.Empty)
            {
                _Users.DefaultView.RowFilter = string.Empty;
                _Records();
                return;
            }

            if (FilterCol == "UserID" || FilterCol == "PersonID")
                _Users.DefaultView.RowFilter = string.Format($"[{FilterCol}] = {txtFilterValue.Text.Trim()}");
            else
                _Users.DefaultView.RowFilter = string.Format($"[{FilterCol}] LIKE '{txtFilterValue.Text.Trim()}%'");

            _Records();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "User ID" || cbFilter.Text == "Person ID")
                e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIsActive.Text == "None")
            {
                _Users.DefaultView.RowFilter = string.Empty;
            }
            else
            {
                _Users.DefaultView.RowFilter = string.Format($"[IsActive] = {cbIsActive.Text}");
            }

            _Records();
        }
    }
}
