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
    public partial class UjJelszo : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;

        public string fhoid, fhonev, teljesnev;

        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\UjJelszo.log", "myListener");

        public UjJelszo(SqlConnection SqlConn)
        {
            InitializeComponent();

            this.sconn = SqlConn;
            if (sconn.State == ConnectionState.Closed) sconn.Open();

            tsUpdate.Enabled = false;
            tsNew.Enabled = false;
            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsSearch.Enabled = false;
            tsSave.Enabled = false;
        }

        public override void tsSave_Click(object sender, EventArgs e)
        {
            save();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            // ellenőrzés
            if (txJelszo.Text == string.Empty) { label6.Text = "A mező nem lehet üres!"; label6.Visible = true; txJelszo.Focus(); return; }
            if (txJelszo2.Text == string.Empty) { label7.Text = "A mező nem lehet üres!"; label7.Visible = true; txJelszo2.Focus(); return; }

            if ((txJelszo.Text != string.Empty && txJelszo2.Text != string.Empty) && txJelszo.Text == txJelszo2.Text)
            {
                string kodoltjelszo = EncryptData(txJelszo.Text);

                string query = "update felhasznalok set password='" + kodoltjelszo + "' where fho_id=" + txFhoid.Text;
                scommand = new SqlCommand(query, sconn);

                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    scommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Hiba " + ex.Message);
                    TraceBejegyzes(ex.Message);
                }
                MessageBox.Show("Jelszó megadása sikeres!");
                Close();
            }
            else
            {
                label7.Text = "A megadott jelszavak nem egyeznek!";
                label7.Visible = true;
                txJelszo2.Focus(); 
            }
        }

        public string EncryptData(string stringToEncrypt)
        {
            string EntropyValue = txFhnev.Text;
            byte[] encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(stringToEncrypt), Encoding.Unicode.GetBytes(EntropyValue), DataProtectionScope.LocalMachine);
            return Convert.ToBase64String(encryptedData);
        }

        private void txJelszo_Leave(object sender, EventArgs e)
        {
            if (txJelszo.Text.Length < 6)
            {
                label6.Text = "A jelszó túl rövid!";
                label6.Visible = true;
                txJelszo.Focus();
            }
            else
                label6.Visible = false;
        }

        private void txJelszo2_Leave(object sender, EventArgs e)
        {
            if (txJelszo2.Text.Length < 6)
            {
                label7.Text = "A jelszó túl rövid!";
                label7.Visible = true;
                txJelszo2.Focus();
            }
            else
                label7.Visible = false;
        }

        private void UjJelszo_Load(object sender, EventArgs e)
        {
            this.txFhoid.Text = fhoid;
            this.txFhnev.Text = fhonev;
            this.txTeljesnev.Text = teljesnev;
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("- " + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }
    }
}
