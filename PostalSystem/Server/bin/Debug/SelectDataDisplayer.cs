using System.Data;
using System.Runtime.InteropServices;

namespace Server
{
    public partial class SelectDataDisplayer : Form
    {
        public SelectDataDisplayer(IEnumerable<dynamic> data, int amountOfColumns)
        {
            InitializeComponent();
            dgvData.DataSource = data.ToList();
            dgvData.AutoGenerateColumns = false;
            int count = dgvData.Columns.Count;
            for (int i = count - 1; i >= count - amountOfColumns; i--) 
            {
                dgvData.Columns.RemoveAt(i);
            }
        }
    }
}
