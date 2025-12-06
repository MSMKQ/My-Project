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

namespace PresentationLayer.People
{
    public partial class frmShowManagePeopleList : Form
    {
        private static DataTable _People;
        private DataTable _dtPeople;

        private static int? _SelectedRow_;
        private static bool _IsVisible = true;

        public static int? SelectedRow
        {
            get
            {
                return _SelectedRow_;
            }
        }

        public frmShowManagePeopleList()
        {
            InitializeComponent();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilter.Text != "None" && cbFilter.Text != "Date Of Birth" && cbFilter.Text != "Gender" && cbFilter.Text != "Country");
            cbGender.Visible = (cbFilter.Text == "Gender");
            cbCountry.Visible = (cbFilter.Text == "Country");
            dateTimePicker1.Visible = (cbFilter.Text == "Date Of Birth");

            if (cbFilter.Text == "None")
            {
                _dtPeople.DefaultView.RowFilter = string.Empty;
            }
            else if (txtFilterValue.Visible)
            {
                txtFilterValue.Clear();
                txtFilterValue.Focus();
            }
            else if (cbGender.Visible)
            {
                cbGender.SelectedIndex = 0;
                cbGender.Focus();
            }
            else if (cbCountry.Visible)
            {
                cbCountry.SelectedIndex = 0;
                cbCountry.Focus();
            }
            else if (dateTimePicker1.Visible)
            {
                dateTimePicker1.MinDate = DateTime.Now.AddYears(-100);
                dateTimePicker1.MaxDate = DateTime.Now.AddYears(-18);
                dateTimePicker1.Focus();
            }

                _Records();
        }

        private int? _SelectedRow()
        {
            if (dgvPeople.CurrentRow?.Cells[0].Value == null || dgvPeople.CurrentRow?.Cells[0].Value == DBNull.Value)
                return 0;

            if (int.TryParse(dgvPeople.CurrentRow.Cells[0].Value.ToString(), out int Num))
                return Num;

            return 0;
        }

        private void _Records()
        {
            lblRecords.Text = $"# Records [{dgvPeople.RowCount}] Person [{_SelectedRow().Value}]";
            _SelectedRow_ = _SelectedRow().Value;
        }

        private void _FillGenderInComboBox()
        {
            foreach (DataRow row in _dtPeople.Rows)
            {
                if (!cbGender.Items.Contains(row["Gender"]))
                {
                    cbGender.Items.Add(row["Gender"]);
                }
            }

            cbGender.Items.Insert(0, "None");
        }

        private void _FillCountryInComboBox()
        {
            foreach (DataRow row in _dtPeople.Rows)
            {
                if (!cbCountry.Items.Contains(row["CountryName"]))
                {
                    cbCountry.Items.Add(row["CountryName"]);
                }
            }

            cbCountry.Items.Insert(0, "None");
        }

        private void _table()
        {
            cbFilter.SelectedIndex = 0;

            if (dgvPeople.RowCount > 0)
            {
                _FillGenderInComboBox();
                _FillCountryInComboBox();

                var header = new (string name, string header, DataGridViewAutoSizeColumnMode Mood)[]
                {
                    ("PersonID", "Person ID", DataGridViewAutoSizeColumnMode.AllCells),
                    ("FullName", "Full Name", DataGridViewAutoSizeColumnMode.NotSet),
                    ("NationID", "Nation ID", DataGridViewAutoSizeColumnMode.AllCells),
                    ("DateOfBirth", "Birth Day", DataGridViewAutoSizeColumnMode.AllCells),
                    ("Gender","Gender", DataGridViewAutoSizeColumnMode.AllCells),
                    ("Phone","Phone", DataGridViewAutoSizeColumnMode.AllCells),
                    ("Email", "Email Address", DataGridViewAutoSizeColumnMode.NotSet),
                    ("CountryName", "Country", DataGridViewAutoSizeColumnMode.AllCells),
                    ("Address", "Address", DataGridViewAutoSizeColumnMode.AllCells)
                };

                foreach (var h in header)
                {
                    var col = dgvPeople.Columns[h.name];

                    if (col != null)
                    {
                        col.HeaderText = h.header;
                        col.AutoSizeMode = h.Mood;
                    }
                }

                _IsVisible = (dgvPeople.Width < 750);

                dgvPeople.Columns["PersonID"].Visible = _IsVisible;
                dgvPeople.Columns["NationID"].Visible = _IsVisible;
                dgvPeople.Columns["DateOfBirth"].Visible = _IsVisible;
                dgvPeople.Columns["CountryName"].Visible = _IsVisible;
                dgvPeople.Columns["Address"].Visible = _IsVisible;
            }

            _Records();
        }

        private void frmShowManagePeopleList_Load(object sender, EventArgs e)
        {
            dgvPeople.ClearSelection();
            _People = ClsPerson.GetPeople();
            _dtPeople = _People.DefaultView.ToTable(true, "PersonID", "FullName", "NationID", "DateOfBirth", "Gender", "Phone", "Email", "CountryName", "Address");
            dgvPeople.DataSource = _dtPeople;
            _table();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parant)
            {
                parant.ShowForm(frmShowNewMainMenue.EnForm.CreatePerson);
            }
        }

        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.PersonInfo);
            }
        }

        private void dgvPeople_SelectionChanged(object sender, EventArgs e)
        {
            _Records();
        }

        private void dgvPeople_Resize(object sender, EventArgs e)
        {
            _IsVisible = (dgvPeople.Width > 900);

            if (dgvPeople.Columns.Contains("PersonID"))
            {
                dgvPeople.Columns["PersonID"].Visible = _IsVisible;
                dgvPeople.Columns["NationID"].Visible = _IsVisible;
                dgvPeople.Columns["DateOfBirth"].Visible = _IsVisible;
                dgvPeople.Columns["CountryName"].Visible = _IsVisible;

            }
        }

        private void showUpdatePersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.UpdatePerson);
            }
        }

        private void showCreateNewPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.CreatePerson);
            }
        }

        private void showDeletePersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int Id = _SelectedRow().Value;

            if (MessageBox.Show($"Are you sure you want delete this person no.{Id} ?", $"Delete Person No.{Id}", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            if (ClsPerson.Delete(Id))
            {
                MessageBox.Show($"This Person No.{Id} Deleted successfully.", $"Person No.{Id} Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                frmShowManagePeopleList_Load(null, null);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.FindPerson);
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterCol = string.Empty;

            switch (cbFilter.Text)
            {
                case "Person ID":
                    FilterCol = "PersonID";
                    break;

                case "Full Name":
                    FilterCol = "FullName";
                    break;

                case "Nation ID":
                    FilterCol = "NationID";
                    break;

                case "Phone":
                    FilterCol = "Phone";
                    break;

                case "Email":
                    FilterCol = "Email";
                    break;

                default:
                    FilterCol = "None";
                    break;
            }

            if (txtFilterValue.Text == string.Empty || cbFilter.Text == "None" || FilterCol == "None")
            {
                _dtPeople.DefaultView.RowFilter = string.Empty;
                _Records();
                return;
            }

            if (FilterCol == "PersonID")
                _dtPeople.DefaultView.RowFilter = string.Format($"[{FilterCol}] = {txtFilterValue.Text.Trim()}");
            else
                _dtPeople.DefaultView.RowFilter = string.Format($"[{FilterCol}] LIKE '%{txtFilterValue.Text.Trim()}%'");

            _Records();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "Person ID")
                e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void cbGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbGender.Text == "None")
            {
                _dtPeople.DefaultView.RowFilter = string.Empty;
            }
            else
            {
                _dtPeople.DefaultView.RowFilter = string.Format($"[Gender] LIKE '{cbGender.Text}%'");
            }

            _Records();
        }

        private void cbCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCountry.Text == "None")
            {
                _dtPeople.DefaultView.RowFilter = string.Empty;
            }
            else
            {
                _dtPeople.DefaultView.RowFilter = string.Format($"[CountryName] LIKE '{cbCountry.Text}%'");
            }

            _Records();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value == DateTime.Now.AddYears(-18))
            {
                _dtPeople.DefaultView.RowFilter = string.Empty;
            }
            else
            {
                _dtPeople.DefaultView.RowFilter = string.Format("[DateOfBirth] = #{0:yyyy-MM-dd}#", dateTimePicker1.Value);

                if (dgvPeople.Rows.Count == 0)
                {
                    _dtPeople.DefaultView.RowFilter = string.Empty;
                }
            }
        }
    }
}
