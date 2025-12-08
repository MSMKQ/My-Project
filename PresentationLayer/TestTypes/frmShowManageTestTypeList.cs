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

namespace PresentationLayer.TestTypes
{
    public partial class frmShowManageTestTypeList : Form
    {
        public static int? SelectdRow { get; set; }
        private DataTable _TestTypes;

        public frmShowManageTestTypeList()
        {
            InitializeComponent();
        }

        private int? _SelectedRow()
        {
            if (dgvTestType.CurrentRow?.Cells[0].Value == null || dgvTestType.CurrentRow?.Cells[0].Value == DBNull.Value)
                return null;

            if (int.TryParse(dgvTestType.CurrentRow.Cells[0].Value.ToString(), out int Id))
                return Id;

            return null;
        }

        private void _Records()
        {
            int? _SelectRow = _SelectedRow(); SelectdRow = (_SelectRow.HasValue) ? _SelectRow : 0; 
            lblRecords.Text = $"# Records [{dgvTestType.RowCount}] TestType [{_SelectRow}]";
        }

        private void _table()
        {
            if (dgvTestType.RowCount > 0)
            {
                var headers = new (string columnName, string columnText, DataGridViewAutoSizeColumnMode Mode)[]
                {
                    ("TestTypeID", "TestType ID", DataGridViewAutoSizeColumnMode.AllCells),
                    ("Title", "Test Name", DataGridViewAutoSizeColumnMode.AllCells),
                    ("Description", "Description", DataGridViewAutoSizeColumnMode.NotSet),
                    ("Fees", "Fees", DataGridViewAutoSizeColumnMode.AllCells)
                };

                foreach (var head in headers)
                {
                    var col = dgvTestType.Columns[head.columnName];

                    if (col != null)
                    {
                        col.HeaderText = head.columnText;
                        col.AutoSizeMode = head.Mode;
                    }
                }
            }

            _Records();
        }

        private void frmShowManageTestTypeList_Load(object sender, EventArgs e)
        {
            dgvTestType.ClearSelection();
            _TestTypes = ClsTestType.GetTestTypes();
            dgvTestType.DataSource = _TestTypes;
            _table();
        }

        private void dgvTestType_SelectionChanged(object sender, EventArgs e)
        {
            _Records();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.CreateTestTypeInfo);
            }

            frmShowManageTestTypeList_Load(null, null);
        }

        private void showCreateNewTestTypeInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ( MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.CreateTestTypeInfo);
            }

            frmShowManageTestTypeList_Load(null, null);
        }

        private void showUpdateTestTypeInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ( MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.UpdateTestTypeInfo);
            }

            frmShowManageTestTypeList_Load(null, null); 
        }
    }
}
