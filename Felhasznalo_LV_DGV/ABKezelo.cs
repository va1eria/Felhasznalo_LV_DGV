using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Felhasznalo_LV_DGV
{
    internal class ABKezelo
    {
        static SqlConnection connection;
        static SqlCommand command;

        public static void Csatlakozas()
        {
            try
            {
                connection = new SqlConnection();
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["FelhasznaloConStr"].ConnectionString;
                connection.Open();
                command = connection.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new ABKivetel("Sikertelen csatlakozas!", ex);
            }
        }
        public static void KapcsolatBontasa()
        {
            try
            {
                connection.Close();
                command.Dispose();
            }
            catch (Exception ex)
            {
                throw new ABKivetel("Sikertelen kapcsolatbontas!", ex);

            }
        }
        public static void UjFelhasznalo(Felhasznalo uj)
        {
            try
            {
                command.Parameters.Clear();
                command.CommandText = "INSERT INTO [Felhasznalo]([Felhasznalonev], [Jelszo]) OUTPUT INSERTED.UId, INSERTED.RegisztracioIdeje VALUES (@felh, @jelszo)";
                command.Parameters.AddWithValue("@felh", uj.Felhasznalonev);
                command.Parameters.AddWithValue("@jelszo", uj.Jelszo);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) // beolvasom az egyetlen visszatero rekordot
                    {
                        uj.UID = (int)reader["UId"];
                        uj.RegisztracioIdeje = DateTime.Parse(reader["RegisztracioIdeje"].ToString());
                    }
                    reader.Close();
                }
            }

            catch (Exception ex)
            {
                throw new ABKivetel("Sikertelen felhasznalo felvitel!", ex);
            }
        }
        public static void FelhasznaloTorles(Felhasznalo torol)
        {
            try
            {
                command.Parameters.Clear();
                command.CommandText = "DELETE FROM [Felhasznalo] WHERE [UId] = @uid";
                command.Parameters.AddWithValue("@uid", torol.UID);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new ABKivetel("Sikertelen felhasznalotorles!", ex);
            }
        }
        public static List<Felhasznalo> Felolvasas(int startIndex = -1, int count = -1)
        {
            try
            {
                command.Parameters.Clear();
                command.CommandText = "SELECT * FROM [Felhasznalo]";
                List<Felhasznalo> felhasznalok = new List<Felhasznalo>();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    for (int i = 0; reader.Read(); i++)
                    {
                        if ((startIndex == -1 || startIndex <= i) && (count == -1 || felhasznalok.Count < count))
                        {
                            felhasznalok.Add(new Felhasznalo(
                                            reader["Felhasznalonev"].ToString(),
                                            reader["Jelszo"].ToString(),
                                            (int)reader["UId"],
                                            DateTime.Parse(reader["RegisztracioIdeje"].ToString())
                                            ));
                        }
                    }
                    reader.Close();
                }
                return felhasznalok;
            }
            catch (Exception ex)
            {
                throw new ABKivetel("Sikertelen felhasznalo felolvas!", ex);
            }
        }
        public static void DVGFeltoltesAB(DataGridView dgv)
        {
            try
            {
                dgv.DataSource = null;
                dgv.AllowUserToAddRows = false;
                dgv.Rows.Clear();
                dgv.Columns.Clear();

                command.Parameters.Clear();
                command.CommandText = "SELECT * FROM [Felhasznalo]";
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // 11. diasor 42. o.
                        if (dgv.Columns.Count < 1)
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                dgv.Columns.Add(reader.GetName(i), reader.GetName(i));
                            }
                        }
                        dgv.Rows.Add();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dgv.Rows[dgv.Rows.Count - 1].Cells[i].Value = reader[i];
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new ABKivetel("Sikertelen felhasznalo felolvas!", ex);

            }
        }
        public static void LVFeltoltesAB(ListView lv)
        {
            try
            {
                lv.View = View.Details;
                lv.Items.Clear();
                lv.Columns.Clear();

                command.Parameters.Clear();
                command.CommandText = "SELECT * FROM [Felhasznalo]";
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // 11. diasor 45. o.
                        if (lv.Columns.Count == 0)
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                lv.Columns.Add(reader.GetName(i));
                            }
                        }
                        string[] adat = new string[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            adat[i] = reader[i].ToString();
                        }
                        lv.Items.Add(new ListViewItem(adat));
                    }
                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                throw new ABKivetel("Sikertelen felhasznalo felolvas!", ex);
            }
        }
        public static void DVGFeltoltesReflekcio(DataGridView dgv)
        {
            dgv.DataSource = null;
            dgv.AllowUserToAddRows = false;
            dgv.Rows.Clear();
            dgv.Columns.Clear();

            // elso pont -> tipusinformacio beszerzese
            // tipus alapjan
            Type tipusInfo = typeof(Felhasznalo);
            // objektum alapjan :
            //tipusInfo = new Felhasznalo("", "").GetType();
            List<Felhasznalo> felhasznalok = Felolvasas();
            foreach (Felhasznalo felhasznalo in felhasznalok)
            {
                if (dgv.Columns.Count < 1)
                {
                    for (int i = 0; i < tipusInfo.GetProperties().Length; i++)
                    {
                        dgv.Columns.Add(tipusInfo.GetProperties()[i].Name, tipusInfo.GetProperties()[i].Name);
                    }
                }
                dgv.Rows.Add();
                for (int i = 0; i < tipusInfo.GetProperties().Length; i++)
                {
                    dgv.Rows[dgv.Rows.Count - 1].Cells[i].Value = tipusInfo.GetProperties()[i].GetValue(felhasznalo);
                }
            }
        }
        public static void LVFeltoltesReflekcio(ListView lv)
        {
            lv.View = View.Details;
            lv.Items.Clear();
            lv.Columns.Clear();

            // elso pont -> tipusinformacio beszerzese
            // tipus alapjan
            Type tipusInfo = typeof(Felhasznalo);
            List<Felhasznalo> felhasznalok = Felolvasas();
            foreach (Felhasznalo felhasznalo in felhasznalok)
            {
                if (lv.Columns.Count < 1)
                {
                    for (int i = 0; i < tipusInfo.GetProperties().Length; i++)
                    {
                        lv.Columns.Add(tipusInfo.GetProperties()[i].Name);
                    }
                }
                string[] data = new string[tipusInfo.GetProperties().Length];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = tipusInfo.GetProperties()[i].GetValue(felhasznalo).ToString();
                }
                lv.Items.Add(new ListViewItem(data));
            }

        }

        public static bool Belepes(Felhasznalo felhasznalo)
        {
            try
            {
                command.Parameters.Clear();
                command.CommandText = "SELECT COUNT([UId]) FROM [Felhasznalo] WHERE [Felhasznalonev] = @felh AND [Jelszo] = @jelszo";
                command.Parameters.AddWithValue("@felh", felhasznalo.Felhasznalonev);
                command.Parameters.AddWithValue("@jelszo", felhasznalo.Jelszo);
                return (int)command.ExecuteScalar()==1;
            }
            catch (Exception ex)
            {
                throw new ABKivetel("Sikertelen belepes!", ex);
            }
        }
    }
}
