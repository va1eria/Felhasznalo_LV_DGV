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
    public partial class BelepesFrm : Form
    {
        Felhasznalo felhasznalo;
        public BelepesFrm()
        {
            InitializeComponent();
        }

        internal Felhasznalo Felhasznalo { get => felhasznalo;}

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim()!=null && textBox2.Text.Trim()!=null)
            {
                felhasznalo = new Felhasznalo(textBox1.Text, textBox2.Text);
                //ABKezelo csekkolasa
                try
                {
                    if (!ABKezelo.Belepes(felhasznalo))
                    {
                        MessageBox.Show("Hibas felhnev es/vagy jelszo!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        DialogResult = DialogResult.None;
                    }
                }
                catch (ABKivetel ex)
                {
                    MessageBox.Show(ex.Message, "Hiba!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.None;
                }
            }
            else
            {
                MessageBox.Show("A felhnev es jelszo kitoltese kotelezo!", "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
            }
        }
    }
}
