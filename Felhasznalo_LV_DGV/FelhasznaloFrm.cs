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
    public partial class FelhasznaloFrm : Form
    {
        Felhasznalo felhasznalo;
        internal Felhasznalo Felhasznalo { get => felhasznalo;}
        public FelhasznaloFrm()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text.Trim() != "" && textBox2.Text.Trim() != "")
                {
                    felhasznalo = new Felhasznalo(textBox1.Text, textBox2.Text);
                    ABKezelo.UjFelhasznalo(felhasznalo);
                }
                else
                {
                    MessageBox.Show("Minden mezo kitoltese kotelezo", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (ABKivetel ex)
            {
                MessageBox.Show(ex.Message, "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult= DialogResult.None;
            }
        }
    }
}
