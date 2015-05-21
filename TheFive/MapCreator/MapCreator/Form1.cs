using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapCreator
{
    public partial class MapCreator : Form
    {
        public MapCreator()
        {
            InitializeComponent();
        }

        private void beginBtn_Click(object sender, EventArgs e)
        {
            Creator create = new Creator((int)rows.Value, (int)columns.Value, (int)mapNumber.Value, nameBox.Text);
            //this.Hide();
            create.Show();
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            this.nameBox.Text = "";
            this.rows.Value = 5;
            this.columns.Value = 5;
            this.mapNumber.Value = 1;
        }

        private void rows_ValueChanged(object sender, EventArgs e)
        {
            columns.Value = rows.Value;
        }

        private void columns_ValueChanged(object sender, EventArgs e)
        {
            rows.Value = columns.Value;
        }
    }
}
