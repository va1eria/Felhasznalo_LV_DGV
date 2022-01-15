using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Felhasznalo_LV_DGV
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            try
            {
                ABKezelo.Csatlakozas();
            }
            catch (ABKivetel ex)
            {
                MessageBox.Show(ex.Message, "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void LBFrissit()
        {
            try
            {
                listBox1.DataSource = null;
                listBox1.DataSource = ABKezelo.Felolvasas();
            }
            catch (ABKivetel ex)
            {
                MessageBox.Show(ex.Message, "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FelhasznaloFrm frm=new FelhasznaloFrm();
            if (frm.ShowDialog()==DialogResult.OK)
            {
                LBFrissit();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex!=-1 && MessageBox.Show("Biztosan torli az elemet?", "Biztosan?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    ABKezelo.FelhasznaloTorles((Felhasznalo)listBox1.SelectedItem);
                    LBFrissit();
                }
            }
            catch (ABKivetel ex)
            {
                MessageBox.Show(ex.Message, "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                ABKezelo.LVFeltoltesAB(listView1);
            }
            catch (ABKivetel ex)
            {
                MessageBox.Show(ex.Message, "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                ABKezelo.DVGFeltoltesAB(dataGridView1);
            }
            catch (ABKivetel ex)
            {
                MessageBox.Show(ex.Message, "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                ABKezelo.LVFeltoltesReflekcio(listView1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                ABKezelo.DVGFeltoltesReflekcio(dataGridView1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BelepesFrm frm=new BelepesFrm();
            if (frm.ShowDialog()!=DialogResult.OK)
            {
                Application.Exit();
            }
        }
    }
}
