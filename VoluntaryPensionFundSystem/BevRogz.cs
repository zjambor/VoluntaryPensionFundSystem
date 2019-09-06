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
using System.Text.RegularExpressions;

namespace VoluntaryPensionFundSystem
{
    public partial class BevRogz : VoluntaryPensionFundSystem.BaseForm, IMessageFilter
    {
        private SqlConnection sconn;
        private SqlCommand scommand;
        //private SqlDataAdapter da;
        //private DataTable dt;
        //private int i = 0;
        private int idk_id = 0;
        private int bev_id = 0;
        private int Evho = 0;
        private bool InsertTrueUpdateFalse = true;
        private bool back, enter = false;
        private bool Fejhiba = false, Sorhiba = false;
        private bool fejresz = true;
        private DateTime mainap = DateTime.Now;
        private string Mainap;
        private int Sorindex = 0;
        private int SajatSum = 0, MunkSum = 0, RendszSum = 0, EgyszeriSum = 0, TotalSum = 0, Tagokszama = 0;
        private string Pnridtext, nev;
        private int TranID = 0;
        //string querystring;

        // Napló logok:
        private TextWriterTraceListener myListener = new TextWriterTraceListener("BevallasRogzites.log", "myListener");

        public BevRogz(SqlConnection SqlConn, string Pnridtext, string adoszam, string adoazon, string nev, string helyseg, string cim, string irszam)
        {
            InitializeComponent();

            this.sconn = SqlConn;
            if (sconn.State == ConnectionState.Closed) sconn.Open();

            this.Pnridtext = Pnridtext;
            this.nev = nev;

            txPnrid.Text = Pnridtext;
            txAdoszam.Text = adoszam;
            txAdoazon.Text = adoazon;
            txMegnev.Text = nev;
            txHelyseg.Text = helyseg;
            txCim.Text = cim;
            txIrszam.Text = irszam;

            newEvHo();

            tsDelete.Enabled = true;
            tsFind.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = true;
            tsSearch.Enabled = true;
            tsUpdate.Enabled = false;

            txIktKelte.Text = mainap.ToString("yyyy.MM.dd.");
            txVonIdoszak.Focus();
        }

        private void BevRogz_Load(object sender, EventArgs e)
        {
            Application.AddMessageFilter(this);
        }

        // Auto betöltés
        private void Reload()
        {
            this.spForgalmakTableAdapter.Fill(this.vPFSDataSet.spForgalmak, new System.Nullable<int>(((int)System.Convert.ChangeType(bev_id, typeof(int)))));
        }

        private void Frissit()
        {
            
        }

        public void rejtId()
        {
            //dgw_Contacts.Columns[0].Visible = false;
        }

        private void dgwForgalmak_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                Sorindex = dgwForgalmak.SelectedCells[0].RowIndex;
            }
            catch
            {
                Sorindex = 0;
            }
        }

        private void newEvHo()
        {
            Mainap = mainap.ToString("yyyy.MM.dd");
            scommand = new SqlCommand("select ev_ho from idoszakok where kezdete<='" + Mainap + "' and vege>='" + Mainap + "';", sconn);
            try
            {
                Evho = int.Parse(scommand.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Hiba " + ex.Message);
                TraceBejegyzes(ex.Message);
            }
            txVonIdoszak.Text = Evho.ToString();
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

        public override void tsDelete_Click(object sender, EventArgs e)
        {
            if(fejresz)
                MessageBox.Show("Hiba ");
            else
                MessageBox.Show(dgwForgalmak.CurrentCell.RowIndex.ToString());
            //try
            //{
            //    MessageBox.Show(dgwForgalmak.CurrentCell.RowIndex.ToString());
            //}
            //catch
            //{
            //    MessageBox.Show("Hiba ");
            //}
        }

        public override void tsSearch_Click(object sender, EventArgs e)
        {
            yellowMode();
        }

        public override void tsFind_Click(object sender, EventArgs e)
        {
            runQuery();
        }

        public override void tsExit_Click(object sender, EventArgs e)
        {
            exit();
        }

        public override void createNew()
        {
            //txPnrid.Text = string.Empty;
            txErkSorsz.Text = string.Empty;
            txErkDate.Text = string.Empty;
            txIktatoszam.Text = string.Empty;
            txAdkozlDate.Text = string.Empty;
            txMegjegyzes.Text = string.Empty;
            newEvHo();

            txErv.Text = "I";
            txTagokSum.Value = 0;
            txSajatSum.Value = 0;
            txMunkSum.Value = 0;
            txRendszSum.Value = 0;
            TxEgyszeriSum.Value = 0;
            TxTotal.Value = 0;
            txIktKelte.Text = Mainap;

            txTagokSumC.Text = "0";
            txSajatSumC.Text = "0";
            txMunkSumC.Text = "0";
            txRendszSumC.Text = "0";
            TxEgyszeriSumC.Text = "0";
            TxTotalC.Text = "0";

            txErkSorsz.ReadOnly = false;
            txErkDate.ReadOnly = false;
            txIktatoszam.ReadOnly = false;
            txIktKelte.ReadOnly = false;
            txAdkozlDate.ReadOnly = false;
            txMegjegyzes.ReadOnly = false;

            txTagokSum.Enabled = true;
            txSajatSum.Enabled = true;
            txMunkSum.Enabled = true;
            txRendszSum.Enabled = true;
            TxEgyszeriSum.Enabled = true;
            TxTotal.Enabled = true;

            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = true;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;

            InsertTrueUpdateFalse = true;
        }

        private void updateRecord()
        {
            txPnrid.ReadOnly = false;
            txErkSorsz.ReadOnly = false;
            txErkDate.ReadOnly = false;
            txIktatoszam.ReadOnly = false;
            txIktKelte.ReadOnly = false;
            txAdkozlDate.ReadOnly = false;
            txMegjegyzes.ReadOnly = false;

            txTagokSum.Enabled = true;
            txSajatSum.Enabled = true;
            txMunkSum.Enabled = true;
            txRendszSum.Enabled = true;
            TxEgyszeriSum.Enabled = true;
            TxTotal.Enabled = true;

            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = true;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;
        }

        public override void yellowMode()
        {

        }

        public override void runQuery()
        {
        }

        public override void save()
        {
            Fejhiba = false;
            // Fejrész ellenőrzések
            if (txVonIdoszak.Text == string.Empty) { MessageBox.Show("Vonatkozási időszak nem lehet üres!"); txVonIdoszak.Focus(); return; }
            if (txErkSorsz.Text == string.Empty) { MessageBox.Show("Érkezés sorszáma nem lehet üres!"); txErkSorsz.Focus(); return; }
            if (txErkDate.Text == string.Empty) { MessageBox.Show("Érkezés dátuma nem lehet üres!"); txErkDate.Focus(); return; }
            if (txIktatoszam.Text == string.Empty) { MessageBox.Show("Iktatószám nem lehet üres!"); txIktatoszam.Focus(); return; }
            if (txAdkozlDate.Text == string.Empty) { MessageBox.Show("Adatközlés dátuma nem lehet üres!"); txAdkozlDate.Focus(); return; }
            if (txIktKelte.Text == string.Empty) { MessageBox.Show("Iktatás kelte nem lehet üres!"); txIktKelte.Focus(); return; }

            // számszaki ellenőrzések
            if (txTagokSum.Value != int.Parse(txTagokSumC.Text)) { MessageBox.Show("Tagok száma összesen nem egyezik!"); Fejhiba = true; }
            if (txSajatSum.Value != int.Parse(txSajatSumC.Text)) { MessageBox.Show("Saját tagi befizetés összesen nem egyezik!"); Fejhiba = true; }
            if (txMunkSum.Value != int.Parse(txMunkSumC.Text)) { MessageBox.Show("Munkáltatói hozzájárulás összesen nem egyezik!"); Fejhiba = true; }
            if (txRendszSum.Value != int.Parse(txRendszSumC.Text)) { MessageBox.Show("Rendszeres támogatás összesen nem egyezik!"); Fejhiba = true; }
            if (TxEgyszeriSum.Value != int.Parse(TxEgyszeriSumC.Text)) { MessageBox.Show("Egyszeri támogatás összesen nem egyezik!"); Fejhiba = true; }
            if (TxTotal.Value != int.Parse(TxTotalC.Text)) { MessageBox.Show("Mindösszesen nem egyezik!"); Fejhiba = true; }

            if (Fejhiba)
            {
                txErv.Text = "N";
                txErv.BackColor = Color.Red;
            }
            else
            {
                txErv.Text = "I";
                txErv.BackColor = Color.LightGreen;
            }

            // sorok ellenőrzése
            for (int i = 0; i < dgwForgalmak.RowCount - 1; i++)
            {
                RowVerification(0, i);
            }

            // mentés
            // Tranzakció id ellenőrzése, ha még 0, akkor tranzakció indítása
            if (TranID == 0)
            {
                TranID = int.Parse(txPnrid.Text);
                TransactionBegin();                // BEGIN TRAN
            }

            if (InsertTrueUpdateFalse)
            {
                bev_id = 0;
                // INSERT                
                scommand = new SqlCommand("spBevallasokInsert", sconn);
                scommand.CommandType = CommandType.StoredProcedure;
                scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
                scommand.Parameters.Add(new SqlParameter("@iktatoszam", SqlDbType.Int)).Value = int.Parse(txIktatoszam.Text);
                scommand.Parameters.Add(new SqlParameter("@erkez_sorszam", SqlDbType.Int)).Value = int.Parse(txErkSorsz.Text);
                scommand.Parameters.Add(new SqlParameter("@adatkozles_datum", SqlDbType.Date)).Value = DateTime.Parse(txAdkozlDate.Text).ToShortDateString();
                scommand.Parameters.Add(new SqlParameter("@iktatas_kelte", SqlDbType.Date)).Value = DateTime.Parse(txIktKelte.Text).ToShortDateString();
                scommand.Parameters.Add(new SqlParameter("@erkez_datum", SqlDbType.Date)).Value = DateTime.Parse(txErkDate.Text).ToShortDateString();
                scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = txErv.Text;
                scommand.Parameters.Add(new SqlParameter("@idk_id", SqlDbType.Int)).Value = idk_id;
                scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = txMegjegyzes.Text;
                scommand.Parameters.Add(new SqlParameter("@tagok_osszesen", SqlDbType.Int)).Value = txTagokSum.Value;
                scommand.Parameters.Add(new SqlParameter("@sajat_ossz", SqlDbType.Int)).Value = txSajatSum.Value;
                scommand.Parameters.Add(new SqlParameter("@hozzajarulas_ossz", SqlDbType.Int)).Value = txMunkSum.Value;
                scommand.Parameters.Add(new SqlParameter("@rend_tamog_ossz", SqlDbType.Int)).Value = txRendszSum.Value;
                scommand.Parameters.Add(new SqlParameter("@egysz_tamog_ossz", SqlDbType.Int)).Value = TxEgyszeriSum.Value;
                scommand.Parameters.Add(new SqlParameter("@mindosszesen", SqlDbType.Int)).Value = TxTotal.Value;
                scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

                try
                {
                    bev_id = (int)scommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL hiba: " + ex.Message);
                    TraceBejegyzes(ex.Message);
                }
                if (bev_id == 0) { MessageBox.Show("A rekord mentése nem sikerült!"); return; }

                // forgalom sorok mentése
                for (int i = 0; i < dgwForgalmak.RowCount - 1; i++)
                {
                    int idOK = (int)ForgalmakInsert(i);
                    if (idOK == 0)
                    {
                        MessageBox.Show("A rekord mentése nem sikerült!");
                        // tranzakció visszagörgetés
                        if (TranID != 0)
                        {
                            TransactionRollback();
                        }
                        return;
                    }
                }
            }
            else
            {
                // UPDATE
                int newBevId = bev_id;
                int idOK = BevallasokUpdate(newBevId);
                if (idOK == 0) { MessageBox.Show("A rekord mentése nem sikerült!"); return; }

                // forgalom sorok mentése
                if (bev_id == 0) { MessageBox.Show("A rekord mentése nem sikerült!"); return; }
                for (int i = 0; i < dgwForgalmak.RowCount - 1; i++)
                {
                    // Ha van forg id, akkor nem új sor, ha nincs, akkor új sor.
                    if (int.Parse(dgwForgalmak.Rows[i].Cells[0].Value.ToString()) > 0)
                    {
                        idOK = (int)ForgalmakUpdate(i);
                        if (idOK == 0)
                        {
                            MessageBox.Show("A rekord mentése nem sikerült!");
                            // tranzakció visszagörgetés
                            if (TranID != 0)
                            {
                                TransactionRollback();
                            }
                            return;
                        }
                    }
                    else
                    {
                        idOK = (int)ForgalmakInsert(i);
                        if (idOK == 0)
                        {
                            MessageBox.Show("A rekord mentése nem sikerült!");
                            // tranzakció visszagörgetés
                            if (TranID != 0)
                            {
                                TransactionRollback();
                            }
                            return;
                        }
                    }
                }
            }

            // tranzakció commit
            if (TranID != 0)
            {
                TransactionCommit();
            }

            tsSave.Enabled = false;
            InsertTrueUpdateFalse = false;
        }

        private int BevallasokUpdate(int id)
        {
            scommand = new SqlCommand("spBevallasokUpdate", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
            scommand.Parameters.Add(new SqlParameter("@iktatoszam", SqlDbType.Int)).Value = int.Parse(txIktatoszam.Text);
            scommand.Parameters.Add(new SqlParameter("@erkez_sorszam", SqlDbType.Int)).Value = int.Parse(txErkSorsz.Text);
            scommand.Parameters.Add(new SqlParameter("@adatkozles_datum", SqlDbType.Date)).Value = DateTime.Parse(txAdkozlDate.Text).ToShortDateString();
            scommand.Parameters.Add(new SqlParameter("@iktatas_kelte", SqlDbType.Date)).Value = DateTime.Parse(txIktKelte.Text).ToShortDateString();
            scommand.Parameters.Add(new SqlParameter("@erkez_datum", SqlDbType.Date)).Value = DateTime.Parse(txErkDate.Text).ToShortDateString();
            scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = txErv.Text;
            scommand.Parameters.Add(new SqlParameter("@idk_id", SqlDbType.Int)).Value = idk_id;
            scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = txMegjegyzes.Text;
            scommand.Parameters.Add(new SqlParameter("@tagok_osszesen", SqlDbType.Int)).Value = txTagokSum.Value;
            scommand.Parameters.Add(new SqlParameter("@sajat_ossz", SqlDbType.Int)).Value = txSajatSum.Value;
            scommand.Parameters.Add(new SqlParameter("@hozzajarulas_ossz", SqlDbType.Int)).Value = txMunkSum.Value;
            scommand.Parameters.Add(new SqlParameter("@rend_tamog_ossz", SqlDbType.Int)).Value = txRendszSum.Value;
            scommand.Parameters.Add(new SqlParameter("@egysz_tamog_ossz", SqlDbType.Int)).Value = TxEgyszeriSum.Value;
            scommand.Parameters.Add(new SqlParameter("@mindosszesen", SqlDbType.Int)).Value = TxTotal.Value;
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
                return 0;
            }
            return 1;
        }

        private int ForgalmakInsert(int row)
        {
            scommand = new SqlCommand("spForgalmakInsert", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
            scommand.Parameters.Add(new SqlParameter("@idk_id", SqlDbType.Int)).Value = idk_id;
            scommand.Parameters.Add(new SqlParameter("@bev_id", SqlDbType.Int)).Value = bev_id;
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[10].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = dgwForgalmak.Rows[row].Cells[3].Value.ToString();
            scommand.Parameters.Add(new SqlParameter("@sajat", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[4].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@hozzajarulas", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[5].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@rend_tamog", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[6].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@egysz_tamog", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[7].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@befizetendo", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[8].Value.ToString());
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
                return 0;
            }
            return 1;
        }

        private int ForgalmakUpdate(int row)
        {
            scommand = new SqlCommand("spForgalmakUpdate", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[0].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@idk_id", SqlDbType.Int)).Value = idk_id;
            scommand.Parameters.Add(new SqlParameter("@bev_id", SqlDbType.Int)).Value = bev_id;
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[10].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = dgwForgalmak.Rows[row].Cells[3].Value.ToString();
            scommand.Parameters.Add(new SqlParameter("@sajat", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[4].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@hozzajarulas", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[5].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@rend_tamog", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[6].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@egysz_tamog", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[7].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@befizetendo", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[8].Value.ToString());
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
                return 0;
            }
            return 1;
        }

        public void deleteRecord()
        {
            DialogResult dr = MessageBox.Show("Biztos törli a kijelölt rekordot? ",
                "Törlés?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                int row = dgwForgalmak.CurrentRow.Index;
                if (int.Parse(dgwForgalmak.Rows[row].Cells[0].Value.ToString()) <= 0)
                {
                    dgwForgalmak.Rows.RemoveAt(row);
                }
                else
                {
                    scommand = new SqlCommand("spForgalmakDelete", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;
                    scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[0].Value.ToString());
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
                    dgwForgalmak.Rows.RemoveAt(row);
                }
            }
        }

        public void deleteAll()
        {
            DialogResult dr = MessageBox.Show("Biztos törli a kijelölt rekordot? ",
                "Törlés?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                scommand = new SqlCommand("spBevallasokDelete", sconn);
                scommand.CommandType = CommandType.StoredProcedure;
                if (bev_id != 0)
                {
                    scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = bev_id;
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
                }
                else
                {
                    createNew();
                }
            }
        }

        private void exit()
        {
            if (tsSave.Enabled == true)
            {
                DialogResult dr = MessageBox.Show("Minden nem mentett adat el fog veszni! Biztosan kilép?", "Megerősítés", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.OK)
                {
                    Close();
                }
            }
            else Close();
        }

        private void txPnrid_Leave(object sender, EventArgs e)
        {
            //if (txPnrid.Text != string.Empty)
            //{
            //    SqlDataReader myReader = null;
            //    scommand = new SqlCommand("spFoglAdatai", sconn);
            //    scommand.CommandType = CommandType.StoredProcedure;
            //    scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
            //    try
            //    {
            //        if (sconn.State == ConnectionState.Closed) sconn.Open();
            //        myReader = scommand.ExecuteReader();
            //        while (myReader.Read())
            //        {
            //            txPnrid.Text = myReader["pnr_id"].ToString();
            //            txMegnev.Text = myReader["megnevezes"].ToString();
            //            txEV.Text = myReader["nev"].ToString();
            //            txHelyseg.Text = myReader["helyseg"].ToString();
            //            txCim.Text = myReader["cim"].ToString();
            //            txIrszam.Text = myReader["ir_szam"].ToString();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("SQL Hiba " + ex.Message);
            //        TraceBejegyzes(ex.Message);
            //    }
            //    myReader.Close();
            //    //txAlkKezdete.Focus();
            //}
        }

        private void BevRogz_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.RemoveMessageFilter(this);
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            dgwForgalmak.Dock = DockStyle.Fill;
        }

        private void BevRogz_Resize(object sender, EventArgs e)
        {
            panel1.Width = Bankszamlak.ActiveForm.Width - 53;
            panel1.Height = Bankszamlak.ActiveForm.Height - 446;
        }

        private void dgwForgalmak_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int col = 0;
            int row = 0;
            try
            {
                col = e.ColumnIndex;
                row = e.RowIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba! " + ex.Message);
                return;
            }
            switch (e.ColumnIndex)
            {
                case 2: e.CellStyle.BackColor = SystemColors.Control;
                    break;
                case 3: try
                    {
                        if (e.Value.ToString() == "N") e.CellStyle.BackColor = Color.Red;
                        else e.CellStyle.BackColor = Color.LightGreen;
                    }
                    catch
                    { }
                    break;
                default:
                    if (e.RowIndex / 2 * 2 != e.RowIndex)
                    {
                        e.CellStyle.BackColor = Color.AliceBlue;
                    }
                    else
                    {
                        e.CellStyle.BackColor = Color.White;
                    }
                    break;
            }
        }

        // GRID CELLÁK KEZELÉSE

        private void dgwForgalmak_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("CellEndEdit"); 
            int col = 0;
            int row = 0;
            try
            {
                col = e.ColumnIndex;
                row = e.RowIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba! " + ex.Message);
                return;
            }

            TagEll(row, col);

            // sum összegek kiszámítása
            try
            {
                switch (col)
                {
                    case 4: SajatSum = 0;
                        for (int i = 0; i < dgwForgalmak.RowCount - 1; i++)
                        {
                            SajatSum += int.Parse(dgwForgalmak.Rows[i].Cells[col].Value.ToString());
                        }
                        txSajatSumC.Text = SajatSum.ToString();
                        break;
                    case 5: MunkSum = 0;
                        for (int i = 0; i < dgwForgalmak.RowCount - 1; i++)
                        {
                            MunkSum += int.Parse(dgwForgalmak.Rows[i].Cells[col].Value.ToString());
                        }
                        txMunkSumC.Text = MunkSum.ToString();
                        break;
                    case 6: RendszSum = 0;
                        for (int i = 0; i < dgwForgalmak.RowCount - 1; i++)
                        {
                            RendszSum += int.Parse(dgwForgalmak.Rows[i].Cells[col].Value.ToString());
                        }
                        txRendszSumC.Text = RendszSum.ToString();
                        break;
                    case 7: EgyszeriSum = 0;
                        for (int i = 0; i < dgwForgalmak.RowCount - 1; i++)
                        {
                            EgyszeriSum += int.Parse(dgwForgalmak.Rows[i].Cells[col].Value.ToString());
                        }
                        TxEgyszeriSumC.Text = EgyszeriSum.ToString();
                        break;
                    case 8: TotalSum = 0;
                        for (int i = 0; i < dgwForgalmak.RowCount - 1; i++)
                        {
                            TotalSum += int.Parse(dgwForgalmak.Rows[i].Cells[col].Value.ToString());
                        }
                        TxTotalC.Text = TotalSum.ToString();
                        break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba: " + ex.Message);
                TraceBejegyzes(ex.Message);
            }
            //dgwForgalmak.ClearSelection();
        }

        private void TagEll(int row, int col)
        {
            // tag ellenőrzése
            long adoazon = 0;
            if (col == 1 && !dgwForgalmak.CurrentRow.IsNewRow)
            {
                try
                {
                    adoazon = long.Parse(dgwForgalmak.Rows[row].Cells[col].Value.ToString());
                }
                catch
                {
                    // új sor, null érték
                }
                if (adoazon != 0)
                {
                    //SqlDataReader myReader = null;
                    scommand = new SqlCommand("spTagEll", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;
                    scommand.Parameters.Add(new SqlParameter("@adoazonosito_jel", SqlDbType.BigInt)).Value = adoazon;
                    try
                    {
                        if (sconn.State == ConnectionState.Closed) sconn.Open();
                        SqlDataReader myReader = scommand.ExecuteReader();
                        int db = 0;
                        while (myReader.Read())
                        {
                            IDataRecord record = (IDataRecord)myReader;
                            dgwForgalmak.Rows[row].Cells[10].Value = record[0].ToString();
                            dgwForgalmak.Rows[row].Cells[2].Value = record[1].ToString();
                            db++;
                        }
                        if (db == 0)
                        {
                            MessageBox.Show("Ezen az adószámon nem található tag!");
                            Sorhiba = true;
                        }
                        else
                            Sorhiba = false;
                        myReader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("SQL Hiba " + ex.Message);
                        TraceBejegyzes(ex.Message);
                    }

                    // sum tagok száma
                    Tagokszama = 0;
                    for (int i = 0; i < dgwForgalmak.RowCount - 1; i++)
                    {
                        if (int.Parse(dgwForgalmak.Rows[i].Cells[10].Value.ToString()) > 9999)
                            Tagokszama++;
                    }
                    txTagokSumC.Text = Tagokszama.ToString();
                }
                //else
                //{
                //    dgwForgalmak.Rows[row].Cells[10].Value = 0;
                //    dgwForgalmak.Rows[row].Cells[2].Value = string.Empty;
                //    Sorhiba = true;
                //    MessageBox.Show("Adószám nem lehet üres!");
                //}
            }
        }

        private void dgwForgalmak_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            //dgwForgalmak.EndEdit();
            //MessageBox.Show("cell leave");

            int col = 0;
            int row = 0;
            try
            {
                col = e.ColumnIndex;
                row = e.RowIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba! " + ex.Message);
                return;
            }

            TagEll(row, col);
        }

        private void dgwForgalmak_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //MessageBox.Show("?");
        }

        private void dgwForgalmak_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        // numerikus mezők kezelése
        private void txPnrid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txVonIdoszak_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txIktatoszam_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txErkSorsz_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txVonIdoszak_Leave(object sender, EventArgs e)
        {
            if (txVonIdoszak.TextLength == 6)
            {
                Evho = int.Parse(txVonIdoszak.Text);
                scommand = new SqlCommand("spIdoszak1", sconn);
                scommand.CommandType = CommandType.StoredProcedure;
                scommand.Parameters.Add(new SqlParameter("@ev_ho", SqlDbType.Int)).Value = Evho;
                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    string idkid2 = scommand.ExecuteScalar().ToString();
                    idk_id = int.Parse(idkid2);
                }
                catch
                {
                    MessageBox.Show("Hibás időszak!");
                    txVonIdoszak.Focus();
                }
            }
            else
            {
                MessageBox.Show("Hibás időszak!");
                txVonIdoszak.Focus();
            }
        }

        #region DÁTUM MEZŐK KEZELÉSE

        // az érkezés dátuma kezelése
        private void txErkDate_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Back: back = true;
                    break;
                case Keys.Enter: enter = true;
                    break;
                default: break;
            }
        }

        private void txErkDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                if (txErkDate.TextLength >= 8) e.Handled = true;
            }
            else if (enter)
            {
                DateTransformErkDate();
            }
            else if (back)
            {
                if (txErkDate.TextLength == 11)
                {
                    string text;
                    text = txErkDate.Text.Substring(0, 4) + txErkDate.Text.Substring(5, 2) + txErkDate.Text.Substring(8, 3);
                    txErkDate.Text = text;
                    txErkDate.Select(txErkDate.Text.Length, 0);
                }
            }
            else
                e.Handled = true;
        }

        private void DateTransformErkDate()
        {
            if (txErkDate.TextLength == 8)
            {
                string text;
                text = txErkDate.Text.Substring(0, 4) + "." + txErkDate.Text.Substring(4, 2) + "." + txErkDate.Text.Substring(6, 2);
                try
                {
                    txErkDate.Text = DateTime.Parse(text).ToShortDateString();
                }
                catch
                {
                    MessageBox.Show("Hibás dátum!");
                    txErkDate.Focus();
                }
            }
            else
            {
                if (txErkDate.TextLength != 11 && txErkDate.TextLength != 0)
                {
                    MessageBox.Show("Hibás dátum!");
                    txErkDate.Focus();
                }
            }
        }

        private void txErkDate_KeyUp(object sender, KeyEventArgs e)
        {
            back = false;
            enter = false;
        }

        private void txErkDate_Leave(object sender, EventArgs e)
        {
            DateTransformErkDate();
            back = false;
            enter = false;

            if (txErkDate.Text != string.Empty)
            {
                if (DateTime.Parse(txErkDate.Text) > DateTime.Parse(txIktKelte.Text))
                {
                    MessageBox.Show("Az érkezés dátuma nem lehet nagyobb az iktatás dátumánál!");
                    txErkDate.Focus();
                }
                else if (txAdkozlDate.Text != string.Empty)
                {
                    if (DateTime.Parse(txErkDate.Text) < DateTime.Parse(txAdkozlDate.Text))
                    {
                        MessageBox.Show("Az érkezés dátuma nem lehet kisebb, mint az adatközlés dátuma!");
                        txErkDate.Focus();
                    }
                }
            }
        }

        // az adatközlés dátuma kezelése
        private void txAdkozlDate_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Back: back = true;
                    break;
                case Keys.Enter: enter = true;
                    break;
                default: break;
            }
        }

        private void txAdkozlDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                if (txAdkozlDate.TextLength >= 8) e.Handled = true;
            }
            else if (enter)
            {
                DateTransformAdatkozlDate();
            }
            else if (back)
            {
                if (txAdkozlDate.TextLength == 11)
                {
                    string text;
                    text = txAdkozlDate.Text.Substring(0, 4) + txAdkozlDate.Text.Substring(5, 2) + txAdkozlDate.Text.Substring(8, 3);
                    txAdkozlDate.Text = text;
                    txAdkozlDate.Select(txAdkozlDate.Text.Length, 0);
                }
            }
            else
                e.Handled = true;
        }

        private void DateTransformAdatkozlDate()
        {
            if (txAdkozlDate.TextLength == 8)
            {
                string text;
                text = txAdkozlDate.Text.Substring(0, 4) + "." + txAdkozlDate.Text.Substring(4, 2) + "." + txAdkozlDate.Text.Substring(6, 2);
                try
                {
                    txAdkozlDate.Text = DateTime.Parse(text).ToShortDateString();
                }
                catch
                {
                    MessageBox.Show("Hibás dátum!");
                    txAdkozlDate.Focus();
                }
            }
            else
            {
                if (txAdkozlDate.TextLength != 11 && txAdkozlDate.TextLength != 0)
                {
                    MessageBox.Show("Hibás dátum!");
                    txAdkozlDate.Focus();
                }
            }
        }

        private void txAdkozlDate_KeyUp(object sender, KeyEventArgs e)
        {
            back = false;
            enter = false;
        }

        private void txAdkozlDate_Leave(object sender, EventArgs e)
        {
            DateTransformAdatkozlDate();
            back = false;
            enter = false;

            if (txAdkozlDate.Text != string.Empty)
            {
                if (DateTime.Parse(txAdkozlDate.Text) > DateTime.Parse(txErkDate.Text))
                {
                    MessageBox.Show("Az adatközlés dátuma nem lehet nagyobb az érkezés dátumánál!");
                    txAdkozlDate.Focus();
                }
            }
        }

        // az iktatás kelte dátum kezelése
        private void txIktKelte_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Back: back = true;
                    break;
                case Keys.Enter: enter = true;
                    break;
                default: break;
            }
        }

        private void txIktKelte_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                if (txIktKelte.TextLength >= 8) e.Handled = true;
            }
            else if (enter)
            {
                DateTransformIktKelteDate();
            }
            else if (back)
            {
                if (txIktKelte.TextLength == 11)
                {
                    string text;
                    text = txIktKelte.Text.Substring(0, 4) + txIktKelte.Text.Substring(5, 2) + txIktKelte.Text.Substring(8, 3);
                    txIktKelte.Text = text;
                    txIktKelte.Select(txIktKelte.Text.Length, 0);
                }
            }
            else
                e.Handled = true;
        }

        private void DateTransformIktKelteDate()
        {
            if (txIktKelte.TextLength == 8)
            {
                string text;
                text = txIktKelte.Text.Substring(0, 4) + "." + txIktKelte.Text.Substring(4, 2) + "." + txIktKelte.Text.Substring(6, 2);
                try
                {
                    txIktKelte.Text = DateTime.Parse(text).ToShortDateString();
                }
                catch
                {
                    MessageBox.Show("Hibás dátum!");
                    txIktKelte.Focus();
                }
            }
            else
            {
                if (txIktKelte.TextLength != 11 && txIktKelte.TextLength != 0)
                {
                    MessageBox.Show("Hibás dátum!");
                    txIktKelte.Focus();
                }
            }
        }

        private void txIktKelte_KeyUp(object sender, KeyEventArgs e)
        {
            back = false;
            enter = false;
        }

        private void txIktKelte_Leave(object sender, EventArgs e)
        {
            DateTransformIktKelteDate();
            back = false;
            enter = false;

            if (txIktKelte.Text != string.Empty)
            {
                if (DateTime.Parse(txIktKelte.Text) > DateTime.Now)
                {
                    MessageBox.Show("Az iktatás dátuma nem lehet nagyobb az aktuális dátumnál!");
                    txIktKelte.Focus();
                }
                else if (txErkDate.Text != string.Empty)
                {
                    if (DateTime.Parse(txIktKelte.Text) < DateTime.Parse(txErkDate.Text))
                    {
                        MessageBox.Show("Az iktatás kelte nem lehet kisebb, mint az érkezés dátuma!");
                        txIktKelte.Focus();
                    }
                }
            }
        }
        #endregion

        #region numericupdown mezők kezelése

        private void txTagokSum_Enter(object sender, EventArgs e)
        {
            txTagokSum.Select(0, txTagokSum.Text.Length);
        }

        private void txSajatSum_Enter(object sender, EventArgs e)
        {
            txSajatSum.Select(0, txSajatSum.Text.Length);
        }

        private void txMunkSum_Enter(object sender, EventArgs e)
        {
            txMunkSum.Select(0, txMunkSum.Text.Length);
        }

        private void txRendszSum_Enter(object sender, EventArgs e)
        {
            txRendszSum.Select(0, txRendszSum.Text.Length);
        }

        private void TxEgyszeriSum_Enter(object sender, EventArgs e)
        {
            TxEgyszeriSum.Select(0, TxEgyszeriSum.Text.Length);
        }

        private void TxTotal_Enter(object sender, EventArgs e)
        {
            TxTotal.Select(0, TxTotal.Text.Length);
        }
        #endregion

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }        

        // A Gridben leütött billentyűknek a lekezelése
        #region IMessageFilter Members
        private const UInt32 WM_KEYDOWN = 0x0100;

        // 1: az Enter kezelése
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN)
            {
                Keys keyCode = (Keys)(int)m.WParam & Keys.KeyCode;

                if (keyCode == Keys.Enter)
                {
                    try
                    {
                        if (!dgwForgalmak.CurrentCell.Selected)
                        {
                            return false;
                        }
                        else
                        {
                            int iColumn = dgwForgalmak.CurrentCell.ColumnIndex;
                            int iRow = dgwForgalmak.CurrentCell.RowIndex;
                            if (iColumn != dgwForgalmak.Columns.Count - 1)                      // ha nem utolsó oszlop akkor jobbra lép
                                dgwForgalmak.CurrentCell = dgwForgalmak[iColumn + 1, iRow];
                            else if (iRow != dgwForgalmak.RowCount - 1)                         // ha utolsó oszlop, de nem utolsó sor, akkor köv. sor 1. oszlopa
                                dgwForgalmak.CurrentCell = dgwForgalmak[1, iRow + 1];

                            if (dgwForgalmak.CurrentCell.ColumnIndex == 2)                      // ha a névre ér akkor a saját oszlopba ugrik
                            {
                                if (Sorhiba || dgwForgalmak.CurrentCell.Value.ToString() == string.Empty)
                                    dgwForgalmak.CurrentCell = dgwForgalmak.Rows[iRow].Cells[1];
                                else
                                    dgwForgalmak.CurrentCell = dgwForgalmak.Rows[iRow].Cells[4];
                                dgwForgalmak.CurrentCell.Selected = true;
                            }
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        TraceBejegyzes(ex.Message);
                        return false;
                    }
                }
                return false;
            }
            return false;
        }

        // 2: A számjegyek és betűk kezelése
        private void MyDataGridViewInitializationMethod()
        {
            dgwForgalmak.EditingControlShowing +=
                new DataGridViewEditingControlShowingEventHandler(dataGridView_EditingControlShowing);
        }

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                e.Control.KeyPress +=
                    new KeyPressEventHandler(Control_KeyPress);
            }
            catch
            { }
        }

        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if ((keyData == Keys.Down || keyData == Keys.Up)
                     && (dgwForgalmak.IsCurrentRowDirty || ((dgwForgalmak.CurrentCell.Value.ToString() == string.Empty) && !dgwForgalmak.CurrentRow.IsNewRow)))
                    return true;
                return base.ProcessCmdKey(ref msg, keyData);        // fel-le lépegetni nem lehet, amíg az adat editálás alatt van, kivéve ha új sor és még nincs beírva adat.
            }
            catch
            {
                return false;
            }
        }

        #endregion

        private void dgwForgalmak_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dgwForgalmak.BeginEdit(true);               // írható cellába belépéskor edit módba lépünk
                //MessageBox.Show("beginedit");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba " + ex.Message);
            }
        }

        private void dgwForgalmak_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //MessageBox.Show("cellvalidating");
            dgwForgalmak.Rows[e.RowIndex].ErrorText = "";
            long newLong;
            int newInteger;

            // Don't try to validate the 'new row' until finished  
            // editing since there 
            // is not any point in validating its initial value. 
            if (dgwForgalmak.Rows[e.RowIndex].IsNewRow) { return; }

            if (e.ColumnIndex == 1)
            {
                if (!long.TryParse(e.FormattedValue.ToString(),
                    out newLong) || newLong < 0)
                {
                    e.Cancel = true;
                    dgwForgalmak.Rows[e.RowIndex].ErrorText = "the value must be a non-negative integer";
                }
            }
            else if (e.ColumnIndex >= 4 && e.ColumnIndex <= 8)
            {
                if (!int.TryParse(e.FormattedValue.ToString(),
                    out newInteger) || newInteger < 0)
                {
                    e.Cancel = true;
                    dgwForgalmak.Rows[e.RowIndex].ErrorText = "the value must be a non-negative integer";
                }
            }
        }

        private void dgwForgalmak_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());
            TraceBejegyzes(e.Exception.ToString());
        }

        private void BevRogz_Activated(object sender, EventArgs e)
        {
            BevRogz.ActiveForm.Text = "Bevallás rögzítés - Fogl.: " + nev + " ID: " + Pnridtext;
        }

        private void dgwForgalmak_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            spForgalmakBindingSource.EndEdit();
            //MessageBox.Show("rowleave");
            //MessageBox.Show(dgwForgalmak.NewRowIndex.ToString() + " " + e.RowIndex.ToString());
            if (dgwForgalmak.NewRowIndex == e.RowIndex)
            {
                //if (dgwForgalmak.IsCurrentCellDirty)
                //{
                //    dgwForgalmak.CommitEdit(DataGridViewDataErrorContexts.Commit);
                //}                
                //if (dgwForgalmak.IsCurrentRowDirty)
                //{
                //    try
                //    {
                //        dgwForgalmak.CancelEdit();
                //        dgwForgalmak.Rows.RemoveAt(e.RowIndex);
                //        MessageBox.Show("Törölve");
                //    }
                //    catch
                //    { }
                //}
            }
            
            //if (dgwForgalmak.CurrentRow.IsNewRow) return;
            if (dgwForgalmak.NewRowIndex == e.RowIndex) return;
            //MessageBox.Show("nem új sor");

            int col = 0;
            int row = 0;
            try
            {
                col = e.ColumnIndex;
                row = e.RowIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba! " + ex.Message);
                return;
            }

            if (int.Parse(dgwForgalmak.Rows[row].Cells[10].Value.ToString()) > 0)
            {
                RowVerification(col, row);
            }
        }

        private void RowVerification(int col, int row)
        {
            //MessageBox.Show("rowverification");
            // Munkaviszony ellenőrzés
            int count = 0;
            int tszs_id = 0;
            try
            {
                int tag = int.Parse(dgwForgalmak.Rows[row].Cells[10].Value.ToString());
                scommand = new SqlCommand("spTszsLekerd", sconn);
                scommand.CommandType = CommandType.StoredProcedure;
                scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = tag;
                tszs_id = int.Parse(scommand.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Hiba " + ex.Message);
                return;
            }

            // FUNCTION futtatása
            scommand = new SqlCommand("SELECT dbo.FuncMvEll2(@tszs_id, @pnr_id, @ev_ho)", sconn);
            scommand.Parameters.Add(new SqlParameter("@tszs_id", SqlDbType.Int)).Value = tszs_id;
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
            scommand.Parameters.Add(new SqlParameter("@ev_ho", SqlDbType.Int)).Value = int.Parse(txVonIdoszak.Text);
            try
            {
                count = int.Parse(scommand.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Hiba " + ex.Message);
            }
            // Az FV kiértékelése
            switch (count)
            {
                case 1: scommand = new SqlCommand("spMvInsert2", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;
                    scommand.Parameters.Add(new SqlParameter("@tszs_id", SqlDbType.Int)).Value = tszs_id;
                    scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
                    scommand.Parameters.Add(new SqlParameter("@ev_ho", SqlDbType.Int)).Value = int.Parse(txVonIdoszak.Text);
                    scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;
                    try
                    {
                        scommand.ExecuteNonQuery();
                    }
                    catch
                    {
                        // üres sor
                    }
                    break;
                case 2: // MessageBox.Show("MV OK"); 
                    break;
                case 3: // MessageBox.Show("mv már van, de nem teljes hónap"); 
                    break;
                case 4: scommand = new SqlCommand("spMvUpdate2", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;
                    scommand.Parameters.Add(new SqlParameter("@tszs_id", SqlDbType.Int)).Value = tszs_id;
                    scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
                    scommand.Parameters.Add(new SqlParameter("@ev_ho", SqlDbType.Int)).Value = int.Parse(txVonIdoszak.Text);
                    scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;
                    try
                    {
                        scommand.ExecuteNonQuery();
                    }
                    catch
                    {
                        // üres sor
                    }
                    MessageBox.Show("Updatel-ve");
                    break;
                default: break;
            }
            
            // Sor ellenőrzések
            Sorhiba = false;

            // ismétlődés
            if (dgwForgalmak.RowCount > 1)
            {
                string adoazon = dgwForgalmak.Rows[row].Cells[1].Value.ToString();
                for (int i = 0; i < dgwForgalmak.RowCount - 1; i++)
                {
                    if ((dgwForgalmak.Rows[i].Cells[1].Value.ToString() == adoazon) && (i != row))
                    {
                        MessageBox.Show("Ez a tag már szerepel a bevallásban! (" + (i + 1).ToString() + ".sor)");
                        Sorhiba = true;
                    }
                }
            }

            int SorOsszesen = 0;
            int SorSajat = int.Parse(dgwForgalmak.Rows[row].Cells[4].Value.ToString());
            int SorMH = int.Parse(dgwForgalmak.Rows[row].Cells[5].Value.ToString());
            int SorRendsz = int.Parse(dgwForgalmak.Rows[row].Cells[6].Value.ToString());
            int SorEgysz = int.Parse(dgwForgalmak.Rows[row].Cells[7].Value.ToString());
            int SorBefizetendo = int.Parse(dgwForgalmak.Rows[row].Cells[8].Value.ToString());
            SorOsszesen = SorSajat + SorMH + SorRendsz + SorEgysz;
            if (SorOsszesen != SorBefizetendo)
            {
                MessageBox.Show("Hibás befizetendő összeg! Sor: " + row.ToString() + " Adóazonosító: " + dgwForgalmak.Rows[row].Cells[1].Value.ToString());
                Sorhiba = true;
            }

            if (Sorhiba)
                dgwForgalmak.Rows[row].Cells[3].Value = "N";
            else
                dgwForgalmak.Rows[row].Cells[3].Value = "I";
        }

        private void txMegjegyzes_Leave(object sender, EventArgs e)
        {
            dgwForgalmak.Focus();
        }

        private void dgwForgalmak_Leave(object sender, EventArgs e)
        {
            fejresz = true;
            spForgalmakBindingSource.EndEdit();
            //if (Sorindex > 0)
            //{
            //MessageBox.Show(dgwForgalmak.NewRowIndex.ToString() + " " + Sorindex.ToString());
            //MessageBox.Show("törölve");
            if (dgwForgalmak.NewRowIndex == Sorindex + 1)
            {
                //dgwForgalmak.CommitEdit(DataGridViewDataErrorContexts.Commit);
                dgwForgalmak.Rows.RemoveAt(dgwForgalmak.NewRowIndex);
                MessageBox.Show("törölve2");
            }

            dgwForgalmak.SelectedCells[0].Selected = false;
            //MessageBox.Show("törölve3");
            //}
            //else
            //    return;

            //if (dgwForgalmak.NewRowIndex == Sorindex)
            //{
            //    dgwForgalmak.Rows.RemoveAt(Sorindex);
            //}
            //MessageBox.Show("leave");
            //try
            //{
            //    int row = dgwForgalmak.RowCount - 1;
            //    dgwForgalmak.CurrentCell = dgwForgalmak.Rows[row].Cells[2];
            //}
            //catch
            //{
            //    MessageBox.Show("Hibás cellaérték!");
            //}
            //dgwForgalmak.CurrentCell.Selected = true;
        }

        private void dgwForgalmak_Enter(object sender, EventArgs e)
        {
            fejresz = false;
            //MessageBox.Show("enter");
        }

        // Tranzakciók kezelése
        private void TransactionBegin()
        {
            string Query;
            Query = "BEGIN TRAN BevRogz" + TranID.ToString();
            scommand = new SqlCommand(Query, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
        }

        private void TransactionCommit()
        {
            string Query;
            Query = "COMMIT TRAN BevRogz" + TranID.ToString() + ";";
            scommand = new SqlCommand(Query, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); TraceBejegyzes(ex.Message); }
            TranID = 0;
        }

        private void TransactionRollback()
        {
            string Query;
            Query = "ROLLBACK TRAN BevRogz" + TranID.ToString() + ";";
            scommand = new SqlCommand(Query, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); TraceBejegyzes(ex.Message); }
            TranID = 0;
        }

        private void dgwForgalmak_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show("CellClick");

        }

        private void dgwForgalmak_NewRowNeeded(object sender, DataGridViewRowEventArgs e)
        {
            MessageBox.Show("NewRowNeeded");
        }

        private void dgwForgalmak_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            MessageBox.Show("RowValidating");
        }
    }
}
