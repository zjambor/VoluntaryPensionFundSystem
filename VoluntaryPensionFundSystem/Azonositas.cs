using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;

namespace VoluntaryPensionFundSystem
{
    public partial class Azonositas : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;

        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\Azonositas.log", "myListener");

        public Azonositas(SqlConnection SqlConn)
        {
            InitializeComponent();

            tsUpdate.Enabled = false;
            tsNew.Enabled = false;
            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsSearch.Enabled = false;
            tsSave.Enabled = false;

            this.sconn = SqlConn;

            Fomenu.Visible = false;
            tsIkonsor.Visible = false;

            txFhnev.Text = "jamborz";
            txJelszo.Text = "jelszo";
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            string dekodoltjelszo;
            string password;
            string query = "select fho_id from felhasznalok where fho_nev='" + txFhnev.Text + "';";
            scommand = new SqlCommand(query, sconn);
            int fho_id = 0;
            try
            {
                fho_id = int.Parse(scommand.ExecuteScalar().ToString());
            }
            catch
            {
                MessageBox.Show("Hibás adat!");
            }

            if (fho_id == 0)
            {
                MessageBox.Show("Felhasználó nem létezik!");
                txFhnev.Focus();
            }
            else
            {
                query = "select password from felhasznalok where fho_id=" + fho_id;
                scommand = new SqlCommand(query, sconn);
                password = scommand.ExecuteScalar().ToString();
                dekodoltjelszo = DecryptData(password);
                txFhnev.Text = dekodoltjelszo;
                if (dekodoltjelszo == txJelszo.Text)
                {
                    // belépés engedélyezve
                    User.userid = fho_id;
                    query = "select fho_nev from felhasznalok where fho_id=" + fho_id;
                    scommand = new SqlCommand(query, sconn);
                    User.name = scommand.ExecuteScalar().ToString();
                    query = "select teljes_nev from felhasznalok where fho_id=" + fho_id;
                    scommand = new SqlCommand(query, sconn);
                    User.teljesnev = scommand.ExecuteScalar().ToString();
                    Close();
                }
                else
                {
                    MessageBox.Show("Hibás jelszó!");
                }
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public string DecryptData(string encryptedText)
        {
            string EntropyValue = txFhnev.Text;
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            //byte[] clearBytes = ProtectedData.Unprotect(encryptedBytes, Encoding.Unicode.GetBytes(EntropyValue), DataProtectionScope.LocalMachine);
            byte[] clearBytes = ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.LocalMachine);
            return Encoding.Unicode.GetString(clearBytes);
        }
    }
}
