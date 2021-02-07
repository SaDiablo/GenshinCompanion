using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenshinWishCalculator
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Banner characterBanner;
        public List<WishDrop> _characterBanner;
        public List<WishDrop> standardBanner;
        //private BindingSource characterBanner = new BindingSource();
        public List<WishDrop> weaponBanner;

        public Form()
        {
            InitializeComponent();
            //dataGridView1 = InitializeGrid(dataGridView1);
        }
        //DataGridViewComboBoxColumn CreateComboBoxColumn()
        //{
        //    DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
        //    combo.DataSource = Enum.GetValues(typeof(DropType));
        //    combo.DataPropertyName = "dropType";
        //    combo.Name = "Item Type";
        //    return combo;
        //}

        //DataGridView InitializeGrid(DataGridView dataGridView)
        //{
        //    dataGridView.Columns[0].DataPropertyName = "dropType";
        //    dataGridView.Columns[1].DataPropertyName = "dropName";
        //    dataGridView.Columns[2].DataPropertyName = "dropTime";
        //    dataGridView.Columns[0].Name = "Item Type";
        //    dataGridView.Columns[1].Name = "Item Name";
        //    dataGridView.Columns[2].Name = "Time Received";
        //    dataGridView.Columns[0].Width = 80;
        //    dataGridView.Columns[1].Width = 176;
        //    dataGridView.Columns[2].Width = 110;
        //    //dataGridView.AutoGenerateColumns = false;
        //    return dataGridView;
        //}

        void UpdateGrids(DataGridView dataGridView)
        {
            //dataGridView.Columns[0].Name = "Item Type";
            //dataGridView.Columns[1].Name = "Item Name";
            //dataGridView.Columns[2].Name = "Time Received";
            //dataGridView1.AutoGenerateColumns = false;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //List<WishDrop> drops = filterInput(textBox1.Text);
                switch (tabControl1.SelectedIndex)
                {
                    case 0:
                        characterBanner = new Banner(textBox1.Text, Type.Character);
                        dataGridView1.DataSource = characterBanner.wishList;
                        label3.Text = characterBanner.wishList.Count.ToString();
                        //var selected = characterBanner.Select(WishDrop => new { WishDrop.dropType, WishDrop.dropName, WishDrop.dropTime }).ToList();
                        //dataGridView1.DataSource = selected;
                        break;
                    case 1:
                        //weaponBanner = drops;
                        //dataGridView2.DataSource = weaponBanner;
                        break;
                    case 2:
                        //standardBanner = drops;
                        //dataGridView2.DataSource = standardBanner;
                        break;
                    case 3:

                    default:
                        break;
                }
            }
        }
    }
}
