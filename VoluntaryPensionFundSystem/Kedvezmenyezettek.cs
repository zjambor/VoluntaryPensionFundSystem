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

namespace VoluntaryPensionFundSystem
{
    public partial class Kedvezmenyezettek : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand, lastScommand;
        private SqlDataAdapter da;
        private DataTable dt;
        private int tagPnr, kedvPnr;
        private bool insertTrue = false;
        private int i = 0;
        private int index = 0;

        Okiratok okir;
        OkiratokModositas om;
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\Kedvezmenyezettek.log", "myListener");

        public Kedvezmenyezettek(SqlConnection SqlConn, int Pnr_id, object Szulo)
        {
            InitializeComponent();

            this.sconn = SqlConn;
            if (sconn.State == ConnectionState.Closed) sconn.Open();
            this.tagPnr = Pnr_id;
            txPnrid.Text = tagPnr.ToString();

            // adattábla
            dt = new DataTable();

            tsUpdate.Enabled = true;
            tsNew.Enabled = true;
            tsDelete.Enabled = true;
            tsFind.Enabled = false;
            tsSearch.Enabled = false;
            tsSave.Enabled = false;

            if (Szulo.GetType() == typeof(Okiratok))
            {
                this.okir = (Okiratok)Szulo;

                scommand = new SqlCommand("select count(*) from kedvezmenyezettek where pnr_id=" + tagPnr.ToString(), sconn);
                int van = (int)scommand.ExecuteScalar();

                if (van > 0)
                {
                    scommand = new SqlCommand("spKedvezmenyezettek", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;
                    scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = tagPnr;
                    lastScommand = scommand;
                    da = new SqlDataAdapter(scommand);
                    Frissit();

                    this.dgvKedvezmenyezettek.DataSource = dt;
                    rejtId();

                    decimal sum = 0;
                    for (int i = 0; i < dgvKedvezmenyezettek.RowCount; i++)
                    {
                        sum += decimal.Parse(dgvKedvezmenyezettek.Rows[i].Cells[3].Value.ToString());
                    }
                    txKedvOssz.Value = dgvKedvezmenyezettek.RowCount;
                    txReszOssz.Text = sum.ToString() + " %";                    
                }

                createNew();
            }
            else if (Szulo.GetType() == typeof(OkiratokModositas))
            {
                this.om = (OkiratokModositas)Szulo;

                scommand = new SqlCommand("select count(*) from kedvezmenyezettek where pnr_id=" + tagPnr.ToString(), sconn);
                int van = (int)scommand.ExecuteScalar();

                if (van > 0)
                {
                    scommand = new SqlCommand("spKedvezmenyezettek", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;
                    scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = tagPnr;
                    lastScommand = scommand;
                    da = new SqlDataAdapter(scommand);
                    Frissit();

                    this.dgvKedvezmenyezettek.DataSource = dt;
                    rejtId();

                    decimal sum = 0;
                    for (int i = 0; i < dgvKedvezmenyezettek.RowCount; i++)
                    {
                        sum += decimal.Parse(dgvKedvezmenyezettek.Rows[i].Cells[3].Value.ToString());
                    }
                    txKedvOssz.Value = dgvKedvezmenyezettek.RowCount;
                    txReszOssz.Text = sum.ToString() + " %";

                    txAdoazon.ReadOnly = true;
                    txAdoszam.ReadOnly = true;
                    txAnyjaNeve.ReadOnly = true;
                    txBankszla.ReadOnly = true;
                    txCim.ReadOnly = true;
                    txHelyseg.ReadOnly = true;
                    txIrszam.ReadOnly = true;
                    txNev.ReadOnly = true;
                    txPnrid.ReadOnly = true;
                    txSzulHely.ReadOnly = true;
                    txSzulNev.ReadOnly = true;
                    txTelefon.ReadOnly = true;

                    chTerm.Enabled = false;
                    numReszesedes.ReadOnly = true;
                    dtSzulIdo.Enabled = false;

                    dgvKedvezmenyezettek.Focus();
                }
            }
        }

        private void Frissit()
        {
            dt.Dispose();
            //dt = new DataTable();

            try
            {
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                TraceBejegyzes(ex.Message);
            }
             /*kdv.kdv_id,kdv.kdpnr_id,kdv.pnr_id,kdv.reszesedes,kdv.term_szemely,coalesce(pnr.nev,pnr.megnevezes) as nev,pnr.leanykori_nev,
		pnr.anyja_neve,pnr.szul_dat,pnr.szul_helye,	pnr.ir_szam,pnr.helyseg,pnr.cim,pnr.telefon,pnr.adoazonosito_jel,pnr.adoszam,bszla.szamlaszam,
		kdv.rogzit_neve,kdv.rogzit_datum,kdv.modosit_neve,kdv.modosit_datum */
            dt.Columns["kdv_id"].ColumnName = "kdv_id";
            dt.Columns["kdpnr_id"].ColumnName = "Kedv. pnr id";
            dt.Columns["pnr_id"].ColumnName = "Pnr id";
            dt.Columns["nev"].ColumnName = "Kedvezményezett neve";
            dt.Columns["helyseg"].ColumnName = "Helység";
            dt.Columns["cim"].ColumnName = "Cím";
            dt.Columns["ir_szam"].ColumnName = "Ir.szám";
            dt.Columns["term_szemely"].ColumnName = "Term.személy";
            dt.Columns["reszesedes"].ColumnName = "Részesedés %";
            dt.Columns["leanykori_nev"].ColumnName = "Szül. név";
            dt.Columns["anyja_neve"].ColumnName = "Anyja neve";
            dt.Columns["szul_helye"].ColumnName = "Születési hely";
            dt.Columns["szul_dat"].ColumnName = "Születési dátum";
            dt.Columns["telefon"].ColumnName = "Telefon";
            dt.Columns["adoazonosito_jel"].ColumnName = "Adóazonositó jel";
            dt.Columns["adoszam"].ColumnName = "Adószám";
            dt.Columns["szamlaszam"].ColumnName = "Számlaszám";
        }

        public void rejtId()
        {
            dgvKedvezmenyezettek.Columns[0].Visible = false;
            dgvKedvezmenyezettek.Columns[17].Visible = false;
            dgvKedvezmenyezettek.Columns[18].Visible = false;
            dgvKedvezmenyezettek.Columns[19].Visible = false;
            dgvKedvezmenyezettek.Columns[20].Visible = false;
        }

        private void dgvKedvezmenyezettek_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                /*kdv.kdv_id,kdv.kdpnr_id,kdv.pnr_id,kdv.reszesedes,kdv.term_szemely,coalesce(pnr.nev,pnr.megnevezes) as nev,pnr.leanykori_nev,
		        pnr.anyja_neve,pnr.szul_dat,pnr.szul_helye,	pnr.ir_szam,pnr.helyseg,pnr.cim,pnr.telefon,pnr.adoazonosito_jel,pnr.adoszam,bszla.szamlaszam,
		        kdv.rogzit_neve,kdv.rogzit_datum,kdv.modosit_neve,kdv.modosit_datum*/
                i = dgvKedvezmenyezettek.SelectedCells[0].RowIndex;
                index = dgvKedvezmenyezettek.SelectedCells[0].RowIndex;
                txKdv_id.Text = dgvKedvezmenyezettek.Rows[i].Cells[0].Value.ToString();
                txKdvPnrid.Text = dgvKedvezmenyezettek.Rows[i].Cells[1].Value.ToString();
                txPnrid.Text = dgvKedvezmenyezettek.Rows[i].Cells[2].Value.ToString();

                txNev.Text = dgvKedvezmenyezettek.Rows[i].Cells[5].Value.ToString();
                txSzulNev.Text = dgvKedvezmenyezettek.Rows[i].Cells[6].Value.ToString();
                txAnyjaNeve.Text = dgvKedvezmenyezettek.Rows[i].Cells[7].Value.ToString();
                txSzulHely.Text = dgvKedvezmenyezettek.Rows[i].Cells[9].Value.ToString();
                dtSzulIdo.Text = dgvKedvezmenyezettek.Rows[i].Cells[8].Value.ToString();
                numReszesedes.Value = decimal.Parse(dgvKedvezmenyezettek.Rows[i].Cells[3].Value.ToString());
                txIrszam.Text = dgvKedvezmenyezettek.Rows[i].Cells[10].Value.ToString();
                txHelyseg.Text = dgvKedvezmenyezettek.Rows[i].Cells[11].Value.ToString();
                txCim.Text = dgvKedvezmenyezettek.Rows[i].Cells[12].Value.ToString();
                txAdoazon.Text = dgvKedvezmenyezettek.Rows[i].Cells[14].Value.ToString();
                txTelefon.Text = dgvKedvezmenyezettek.Rows[i].Cells[13].Value.ToString();
                chTerm.Checked = (dgvKedvezmenyezettek.Rows[i].Cells[4].Value.ToString() == "I" ? true : false);
                if (chTerm.Checked)
                {
                    txAdoszam.Text = dgvKedvezmenyezettek.Rows[i].Cells[15].Value.ToString();
                    txBankszla.Text = dgvKedvezmenyezettek.Rows[i].Cells[16].Value.ToString();
                }

                txAdoazon.ReadOnly = true;
                txAdoszam.ReadOnly = true;
                txAnyjaNeve.ReadOnly = true;
                txBankszla.ReadOnly = true;
                txCim.ReadOnly = true;
                txHelyseg.ReadOnly = true;
                txIrszam.ReadOnly = true;
                txKedvOssz.ReadOnly = true;
                txNev.ReadOnly = true;
                txPnrid.ReadOnly = true;
                txSzulHely.ReadOnly = true;
                txSzulNev.ReadOnly = true;
                txTelefon.ReadOnly = true;

                chTerm.Enabled = false;
                numReszesedes.ReadOnly = true;
                dtSzulIdo.Enabled = false;

                tsUpdate.Enabled = true;
                tsNew.Enabled = true;
                tsDelete.Enabled = true;
                tsFind.Enabled = false;
                tsSearch.Enabled = false;
                txNev.Select(0, 0);
            }
            catch
            {
                // kezdeti hiba
            }
        }

        public override void tsNew_Click(object sender, EventArgs e)
        {
            createNew();
        }

        public override void tsSave_Click(object sender, EventArgs e)
        {
            save();
        }

        public override void tsUpdate_Click(object sender, EventArgs e)
        {
            updateRecord();
        }

        public override void tsExit_Click(object sender, EventArgs e)
        {
            exit();
        }

        public override void createNew()
        {
            txAdoazon.ReadOnly = false;
            txAdoszam.ReadOnly = false;
            txAnyjaNeve.ReadOnly = false;
            txBankszla.ReadOnly = false;
            txCim.ReadOnly = false;
            txHelyseg.ReadOnly = false;
            txIrszam.ReadOnly = false;
            txNev.ReadOnly = false;
            txSzulHely.ReadOnly = false;
            txSzulNev.ReadOnly = false;
            txTelefon.ReadOnly = false;

            chTerm.Enabled = true;
            numReszesedes.ReadOnly = false;
            numReszesedes.Enabled = true;
            dtSzulIdo.Enabled = true;

            txKdv_id.Text = string.Empty;
            txKdvPnrid.Text = string.Empty;

            txAdoazon.Text = string.Empty;
            txAdoszam.Text = string.Empty;
            txAnyjaNeve.Text = string.Empty;
            txBankszla.Text = string.Empty;
            txCim.Text = string.Empty;
            txHelyseg.Text = string.Empty;
            txIrszam.Text = string.Empty;
            //txKedvOssz.Text = string.Empty;
            txNev.Text = string.Empty;
            //txReszOssz.Text = string.Empty;
            txSzulHely.Text = string.Empty;
            txSzulNev.Text = string.Empty;
            txTelefon.Text = string.Empty;

            chTerm.Checked = true;
            numReszesedes.Value = 0;
            dtSzulIdo.Value = DateTime.Now;

            tsUpdate.Enabled = false;
            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsSearch.Enabled = false;
            tsSave.Enabled = true;
            insertTrue = true;
        }

        private void updateRecord()
        {
            txAdoazon.ReadOnly = false;
            txAdoszam.ReadOnly = false;
            txAnyjaNeve.ReadOnly = false;
            txBankszla.ReadOnly = false;
            txCim.ReadOnly = false;
            txHelyseg.ReadOnly = false;
            txIrszam.ReadOnly = false;
            txNev.ReadOnly = false;
            txSzulHely.ReadOnly = false;
            txSzulNev.ReadOnly = false;
            txTelefon.ReadOnly = false;

            chTerm.Enabled = true;
            numReszesedes.ReadOnly = false;
            numReszesedes.Enabled = true;
            dtSzulIdo.Enabled = true;

            tsUpdate.Enabled = false;
            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsSearch.Enabled = false;
            tsSave.Enabled = true;
            insertTrue = false;
        }

        public override void save()
        {
            int x = index;
            tagPnr = int.Parse(txPnrid.Text);

            if (txNev.Text == string.Empty) { MessageBox.Show("Név nem lehet üres!"); txNev.Focus(); return; }
            if (txCim.Text == string.Empty) { MessageBox.Show("Cím nem lehet üres!"); txCim.Focus(); return; }
            if (numReszesedes.Value == 0) { MessageBox.Show("A részesedés nem lehet 0!"); numReszesedes.Focus(); return; }
            if (chTerm.Checked)
            {
                if (txAdoazon.Text == string.Empty) { MessageBox.Show("Adóazonosító jel nem lehet üres!"); txAdoazon.Focus(); return; }
            }
            else
            {
                if (txAdoszam.Text == string.Empty) { MessageBox.Show("Adószám nem lehet üres!"); txAdoszam.Focus(); return; }
            }

            decimal sum = 0;

            if (insertTrue)
            {
                if (txKedvOssz.Value > 0)
                {
                    for (int i = 0; i < dgvKedvezmenyezettek.RowCount; i++)
                    {
                        sum += decimal.Parse(dgvKedvezmenyezettek.Rows[i].Cells[3].Value.ToString());
                    }
                    sum += decimal.Parse(numReszesedes.Value.ToString());
                    if (sum > 100) { MessageBox.Show("Az összesített részesedés nem haladhatja meg a 100%-ot!"); numReszesedes.Focus(); return; }
                }

                scommand = new SqlCommand("spKedvezmenyezettekInsert", sconn);
                scommand.CommandType = CommandType.StoredProcedure;
                scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = tagPnr;
                scommand.Parameters.Add(new SqlParameter("@kdv_id", SqlDbType.Int)).Value = 1;
                scommand.Parameters.Add(new SqlParameter("@kdpnr_id", SqlDbType.Int)).Value = 1;
                scommand.Parameters.Add(new SqlParameter("@bszla_id", SqlDbType.Int)).Value = 1;
                scommand.Parameters.Add(new SqlParameter("@nev", SqlDbType.VarChar, 50)).Value = txNev.Text;
                scommand.Parameters.Add(new SqlParameter("@adoazonosito_jel", SqlDbType.Decimal, 10)).Value = double.Parse(txAdoazon.Text);
                scommand.Parameters.Add(new SqlParameter("@ir_szam", SqlDbType.Decimal, 4)).Value = int.Parse(txIrszam.Text);
                scommand.Parameters.Add(new SqlParameter("@helyseg", SqlDbType.VarChar, 20)).Value = txHelyseg.Text;
                scommand.Parameters.Add(new SqlParameter("@cim", SqlDbType.VarChar, 80)).Value = txCim.Text;
                scommand.Parameters.Add(new SqlParameter("@telefon", SqlDbType.VarChar, 15)).Value = txTelefon.Text;
                scommand.Parameters.Add(new SqlParameter("@leanykori_nev", SqlDbType.VarChar, 50)).Value = txSzulNev.Text;
                scommand.Parameters.Add(new SqlParameter("@anyja_neve", SqlDbType.VarChar, 50)).Value = txAnyjaNeve.Text;
                scommand.Parameters.Add(new SqlParameter("@szul_dat", SqlDbType.Date)).Value = dtSzulIdo.Value;
                scommand.Parameters.Add(new SqlParameter("@szul_helye", SqlDbType.VarChar, 20)).Value = txSzulHely.Text;
                scommand.Parameters.Add(new SqlParameter("@term_szemely", SqlDbType.VarChar, 1)).Value = (chTerm.Checked ? "I" : "N");
                scommand.Parameters.Add(new SqlParameter("@reszesedes", SqlDbType.Float)).Value = numReszesedes.Value;
                if (!chTerm.Checked)
                {
                    scommand.Parameters.Add(new SqlParameter("@adoszam", SqlDbType.VarChar, 15)).Value = txAdoszam.Text;
                    scommand.Parameters.Add(new SqlParameter("@szamlaszam", SqlDbType.Decimal, 24)).Value = double.Parse(txBankszla.Text);
                }
                else
                {
                    scommand.Parameters.Add(new SqlParameter("@adoszam", SqlDbType.VarChar, 15)).Value = string.Empty;
                    scommand.Parameters.Add(new SqlParameter("@szamlaszam", SqlDbType.Decimal, 24)).Value = 1;
                }
                scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    txKdv_id.Text = scommand.ExecuteNonQuery().ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Hiba " + ex.Message);
                    TraceBejegyzes(ex.Message);
                }

                // grid frissítése
                scommand = new SqlCommand("spKedvezmenyezettek", sconn);
                scommand.CommandType = CommandType.StoredProcedure;
                scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = tagPnr;
                lastScommand = scommand;

                da = new SqlDataAdapter(scommand);
                dt = new DataTable();
                Frissit();
                this.dgvKedvezmenyezettek.DataSource = dt;
                rejtId();

                sum = 0;
                for (int i = 0; i < dgvKedvezmenyezettek.RowCount; i++)
                {
                    sum += decimal.Parse(dgvKedvezmenyezettek.Rows[i].Cells[3].Value.ToString());
                }
                txKedvOssz.Value = dgvKedvezmenyezettek.RowCount;
                txReszOssz.Text = sum.ToString() + " %";

                dgvKedvezmenyezettek.CurrentCell = dgvKedvezmenyezettek.Rows[x].Cells[1];
            }
            else
            {
                if (txKedvOssz.Value > 0)
                {
                    for (int i = 0; i < dgvKedvezmenyezettek.RowCount; i++)
                    {
                        sum += decimal.Parse(dgvKedvezmenyezettek.Rows[i].Cells[3].Value.ToString());
                    }
                    sum -= decimal.Parse(dgvKedvezmenyezettek.Rows[index].Cells[3].Value.ToString());
                    sum += decimal.Parse(numReszesedes.Value.ToString());
                    if (sum > 100) { MessageBox.Show("Az összesített részesedés nem haladhatja meg a 100%-ot!"); numReszesedes.Focus(); return; }
                }

                kedvPnr = int.Parse(txKdvPnrid.Text);
                // kedv pnr id-hez tartozó bankszla lekérdezése
                string query = "select bszla_id from bankszamlak where pnr_id=" + kedvPnr.ToString();
                scommand = new SqlCommand(query, sconn);
                int bankszlaid = 0;
                try
                {
                    bankszlaid = (int)scommand.ExecuteScalar();
                }
                catch
                {
                    // nincs találat
                }

                scommand = new SqlCommand("spKedvezmenyezettekUpdate", sconn);
                scommand.CommandType = CommandType.StoredProcedure;
                scommand.Parameters.Add(new SqlParameter("@kdv_id", SqlDbType.Int)).Value = int.Parse(txKdv_id.Text);
                scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = tagPnr;
                scommand.Parameters.Add(new SqlParameter("@kdpnr_id", SqlDbType.Int)).Value = kedvPnr;
                scommand.Parameters.Add(new SqlParameter("@bszla_id", SqlDbType.Int)).Value = bankszlaid;
                scommand.Parameters.Add(new SqlParameter("@nev", SqlDbType.VarChar, 50)).Value = txNev.Text;
                scommand.Parameters.Add(new SqlParameter("@adoazonosito_jel", SqlDbType.Decimal, 10)).Value = double.Parse(txAdoazon.Text);
                scommand.Parameters.Add(new SqlParameter("@ir_szam", SqlDbType.Decimal, 4)).Value = int.Parse(txIrszam.Text);
                scommand.Parameters.Add(new SqlParameter("@helyseg", SqlDbType.VarChar, 20)).Value = txHelyseg.Text;
                scommand.Parameters.Add(new SqlParameter("@cim", SqlDbType.VarChar, 80)).Value = txCim.Text;
                scommand.Parameters.Add(new SqlParameter("@telefon", SqlDbType.VarChar, 15)).Value = txTelefon.Text;
                scommand.Parameters.Add(new SqlParameter("@leanykori_nev", SqlDbType.VarChar, 50)).Value = txSzulNev.Text;
                scommand.Parameters.Add(new SqlParameter("@anyja_neve", SqlDbType.VarChar, 50)).Value = txAnyjaNeve.Text;
                scommand.Parameters.Add(new SqlParameter("@szul_dat", SqlDbType.Date)).Value = dtSzulIdo.Value;
                scommand.Parameters.Add(new SqlParameter("@szul_helye", SqlDbType.VarChar, 20)).Value = txSzulHely.Text;
                scommand.Parameters.Add(new SqlParameter("@term_szemely", SqlDbType.VarChar, 1)).Value = (chTerm.Checked ? "I" : "N");
                scommand.Parameters.Add(new SqlParameter("@reszesedes", SqlDbType.Float)).Value = numReszesedes.Value;
                if (!chTerm.Checked)
                {
                    scommand.Parameters.Add(new SqlParameter("@adoszam", SqlDbType.VarChar, 15)).Value = txAdoszam.Text;
                    scommand.Parameters.Add(new SqlParameter("@szamlaszam", SqlDbType.Decimal, 24)).Value = double.Parse(txBankszla.Text);
                }
                else
                {
                    scommand.Parameters.Add(new SqlParameter("@adoszam", SqlDbType.VarChar, 15)).Value = string.Empty;
                    scommand.Parameters.Add(new SqlParameter("@szamlaszam", SqlDbType.Decimal, 24)).Value = 1;
                }
                scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

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

                // grid frissítése
                scommand = lastScommand;
                da = new SqlDataAdapter(scommand);
                dt = new DataTable();
                Frissit();
                this.dgvKedvezmenyezettek.DataSource = dt;
                rejtId();

                sum = 0;
                for (int i = 0; i < dgvKedvezmenyezettek.RowCount; i++)
                {
                    sum += decimal.Parse(dgvKedvezmenyezettek.Rows[i].Cells[3].Value.ToString());
                }
                txKedvOssz.Value = dgvKedvezmenyezettek.RowCount;
                txReszOssz.Text = sum.ToString() + " %";

                dgvKedvezmenyezettek.CurrentCell = dgvKedvezmenyezettek.Rows[x].Cells[1];
            }
            txAdoazon.ReadOnly = true;
            txAdoszam.ReadOnly = true;
            txAnyjaNeve.ReadOnly = true;
            txBankszla.ReadOnly = true;
            txCim.ReadOnly = true;
            txHelyseg.ReadOnly = true;
            txIrszam.ReadOnly = true;
            txKedvOssz.ReadOnly = true;
            txNev.ReadOnly = true;
            txPnrid.ReadOnly = true;
            txSzulHely.ReadOnly = true;
            txSzulNev.ReadOnly = true;
            txTelefon.ReadOnly = true;

            chTerm.Enabled = false;
            numReszesedes.ReadOnly = true;
            dtSzulIdo.Enabled = false;

            tsSave.Enabled = false;
            tsNew.Enabled = true;
            tsUpdate.Enabled = true;
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        private void exit()
        {
            if (tsSave.Enabled == true)
            {
                DialogResult dr = MessageBox.Show("Menti a változtatásokat?", "Megerősítés", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                {
                    Close();
                }
                else if (dr == DialogResult.Yes)           // mentés
                {
                    save();
                    Close();
                }
            }
            else
                Close();
        }

        public override void tsDelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Biztos törli a kijelölt rekordot? ",
                "Törlés?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string query = "delete from kedvezmenyezettek where kdv_id=" + txKdv_id.Text;
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

                scommand = lastScommand;
                da = new SqlDataAdapter(scommand);
                dt = new DataTable();
                Frissit();
                this.dgvKedvezmenyezettek.DataSource = dt;
            }
        }

        private void dgvKedvezmenyezettek_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex / 2 * 2 != e.RowIndex)
            {
                e.CellStyle.BackColor = Color.AliceBlue;
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
            }
        }

        private void numReszesedes_Enter(object sender, EventArgs e)
        {
            numReszesedes.Select(0, numReszesedes.Text.Length);
        }

        private void txAdoazon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txIrszam_Leave(object sender, EventArgs e)
        {
            // település beolvasás
            scommand = new SqlCommand("SELECT telepules FROM iranyitoszamok where irszam=" + txIrszam.Text, sconn);
            if (sconn.State == ConnectionState.Closed) sconn.Open();
            SqlDataReader sqlReader = scommand.ExecuteReader();

            while (sqlReader.Read())
            {
                txHelyseg.Text = (sqlReader["telepules"].ToString());
            }

            sqlReader.Close();
        }

        private void dgvKedvezmenyezettek_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgvKedvezmenyezettek.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgvKedvezmenyezettek.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvKedvezmenyezettek.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgvKedvezmenyezettek.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
        }

        private void Kedvezmenyezettek_Resize(object sender, EventArgs e)
        {
            panel1.Width = Kedvezmenyezettek.ActiveForm.Width - 67;
            dgvKedvezmenyezettek.Width = Kedvezmenyezettek.ActiveForm.Width - 67;
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            dgvKedvezmenyezettek.Dock = DockStyle.Fill;
        }
    }
}
