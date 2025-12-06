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

namespace PresentationLayer.Applications.ApplicationTypes
{
    public partial class frmShowManageApplicationTypesList : Form
    {
        private DataTable _ApplicationTypes;

        public frmShowManageApplicationTypesList()
        {
            InitializeComponent();
        }

        private int? _SelectedRow()
        {
            if (dgvApplicationTypes.CurrentRow?.Cells[0].Value == null || dgvApplicationTypes.CurrentRow?.Cells[0].Value == DBNull.Value)
                return null;

            if (int.TryParse(dgvApplicationTypes.CurrentRow.Cells[0].Value.ToString(), out int SelectedRow))
                return SelectedRow;

            return null;
        }

        private void _Records()
        {
            int SelectedRow = _SelectedRow().Value;
            lblRecords.Text = $"# Records [{dgvApplicationTypes.RowCount}] AppType [{SelectedRow}]";
        }

        private void _Table()
        {
            if (dgvApplicationTypes.RowCount > 0)
            {
                var headers = new (string name, string header, DataGridViewAutoSizeColumnMode Mode)[]
                {
                    ("ApplicationTypeID", "App No", DataGridViewAutoSizeColumnMode.AllCells),
                    ("Title", "Title", DataGridViewAutoSizeColumnMode.NotSet),
                    ("Fees", "Fees", DataGridViewAutoSizeColumnMode.AllCells)
                };

                foreach (var head in headers)
                {
                    var col = dgvApplicationTypes.Columns[head.name];

                    if (col != null)
                    {
                        col.HeaderText = head.header;
                        col.AutoSizeMode = head.Mode;
                    }
                }
            }

            _Records();
        }

        private void frmShowManageApplicationTypesList_Load(object sender, EventArgs e)
        {
            dgvApplicationTypes.ClearSelection();
            _ApplicationTypes = ClsApplicationType.GetApplicationTypes();
            dgvApplicationTypes.DataSource = _ApplicationTypes;
            _Table();
        }

        private void dgvApplicationTypes_SelectionChanged(object sender, EventArgs e)
        {
            _Records();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.CreateApplicationTypeInfo);
            }
        }
    }
}
