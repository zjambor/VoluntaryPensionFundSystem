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
    public partial class TobbBevallasKivalaszt : VoluntaryPensionFundSystem.BaseForm
    {
        private DataTable dt;
        private int Sorindex;

        private BevRogzUj br;
        private BevBong bb;
        private bool bevrogz;
        public string pnrid, megnevezes, adoszam, nev, helyseg, cim, irszam,bevid, vonidoszak, erksorsz, erkdate, iktato, adatkozl;
        public string iktkelte, storno, megjegyz, ervenyes, tagok, sajat, munkh, rendszeres, egyszeri, total, konyvelt;

        public TobbBevallasKivalaszt(DataTable dt, object szulo)
        {
            InitializeComponent();

            this.dt = dt;
            if (szulo.GetType() == typeof(BevRogzUj))
            {
                this.br = (BevRogzUj)szulo;
                bevrogz = true;
            }
            else if (szulo.GetType() == typeof(BevBong))
            {
                this.bb = (BevBong)szulo;
                bevrogz = false;
            }

            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;
            tsExit.Enabled = false;

            Frissit();
            dgvBevKiv.DataSource = dt;
        }

        private void Frissit()
        {
            dt.Columns["bev_id"].ColumnName = "Bevall. id";
            dt.Columns["pnr_id"].ColumnName = "Pnr id";
            dt.Columns["megnevezes"].ColumnName = "Megnevezés";
            dt.Columns["helyseg"].ColumnName = "Helység";
            dt.Columns["cim"].ColumnName = "Cím";
            dt.Columns["ir_szam"].ColumnName = "Ir.szám";
            dt.Columns["ev_ho"].ColumnName = "Von.időszak";
            dt.Columns["erkez_sorszam"].ColumnName = "Érk.sorszám";
            dt.Columns["erkez_datum"].ColumnName = "Érk.dátum";
            dt.Columns["iktatoszam"].ColumnName = "Iktatószám";
            dt.Columns["iktatas_kelte"].ColumnName = "Ikt.kelte";
            dt.Columns["adatkozles_datum"].ColumnName = "Adatközl.dátum";
            dt.Columns["tagok_osszesen"].ColumnName = "Tagok összesen";
            dt.Columns["sajat_ossz"].ColumnName = "Saját össz.";
            dt.Columns["hozzajarulas_ossz"].ColumnName = "Hozzájárulás össz.";
            dt.Columns["rend_tamog_ossz"].ColumnName = "Rendsz.tám.össz.";
            dt.Columns["egysz_tamog_ossz"].ColumnName = "Egysz.tám.össz.";
            dt.Columns["mindosszesen"].ColumnName = "Mindösszesen";
            dt.Columns["ervenyes"].ColumnName = "Érvényes";
            dt.Columns["storno"].ColumnName = "Storno";
            dt.Columns["megjegyzes"].ColumnName = "Megjegyzés";
            dt.Columns["konyvelt"].ColumnName = "Könyvelt";
            dt.Columns["nev"].ColumnName = "Név";
            dt.Columns["adoszam"].ColumnName = "Adószám";
        }

        private void bKivalaszt_Click(object sender, EventArgs e)
        {
            kivalaszt();
        }

        private void kivalaszt()
        {
            try
            {
                Sorindex = dgvBevKiv.SelectedCells[0].RowIndex;
                /*"SELECT b.bev_id, i.ev_ho, b.erkez_sorszam, b.erkez_datum, b.iktatoszam, b.iktatas_kelte, b.adatkozles_datum" +
                                        ", b.tagok_osszesen, b.sajat_ossz, b.hozzajarulas_ossz, b.rend_tamog_ossz, b.egysz_tamog_ossz" +
                                        ", b.mindosszesen, b.ervenyes, b.storno, b.megjegyzes, b.konyvelt";*/
                if (bevrogz)
                {
                    br.Bevid = dgvBevKiv.Rows[Sorindex].Cells[0].Value.ToString();

                    br.VonIdoszak = dgvBevKiv.Rows[Sorindex].Cells[1].Value.ToString();
                    br.ErkSorsz = dgvBevKiv.Rows[Sorindex].Cells[2].Value.ToString();
                    br.Erkdate = dgvBevKiv.Rows[Sorindex].Cells[3].Value.ToString();
                    br.Iktatoszam = dgvBevKiv.Rows[Sorindex].Cells[4].Value.ToString();
                    br.Adatkozl = dgvBevKiv.Rows[Sorindex].Cells[6].Value.ToString();
                    br.IktKelte = dgvBevKiv.Rows[Sorindex].Cells[5].Value.ToString();
                    br.Storno = dgvBevKiv.Rows[Sorindex].Cells[14].Value.ToString();
                    br.Megjegyzes = dgvBevKiv.Rows[Sorindex].Cells[15].Value.ToString();
                    br.Ervenyes = dgvBevKiv.Rows[Sorindex].Cells[13].Value.ToString();
                    br.Tagok = dgvBevKiv.Rows[Sorindex].Cells[7].Value.ToString();
                    br.Sajat = dgvBevKiv.Rows[Sorindex].Cells[8].Value.ToString();
                    br.Munkh = dgvBevKiv.Rows[Sorindex].Cells[9].Value.ToString();
                    br.Rendszeres = dgvBevKiv.Rows[Sorindex].Cells[10].Value.ToString();
                    br.Egyszeri = dgvBevKiv.Rows[Sorindex].Cells[11].Value.ToString();
                    br.Total = dgvBevKiv.Rows[Sorindex].Cells[12].Value.ToString();
                    br.Konyvelt = dgvBevKiv.Rows[Sorindex].Cells[16].Value.ToString();
                }
                else
                {
                    pnrid = dgvBevKiv.Rows[Sorindex].Cells[0].Value.ToString();
                    megnevezes = dgvBevKiv.Rows[Sorindex].Cells[1].Value.ToString();
                    nev = dgvBevKiv.Rows[Sorindex].Cells[2].Value.ToString();
                    adoszam = dgvBevKiv.Rows[Sorindex].Cells[3].Value.ToString();
                    helyseg = dgvBevKiv.Rows[Sorindex].Cells[4].Value.ToString();
                    cim = dgvBevKiv.Rows[Sorindex].Cells[5].Value.ToString();
                    irszam = dgvBevKiv.Rows[Sorindex].Cells[6].Value.ToString();
                    bevid = dgvBevKiv.Rows[Sorindex].Cells[7].Value.ToString();
                    vonidoszak = dgvBevKiv.Rows[Sorindex].Cells[8].Value.ToString();
                    erksorsz = dgvBevKiv.Rows[Sorindex].Cells[9].Value.ToString();
                    erkdate = dgvBevKiv.Rows[Sorindex].Cells[10].Value.ToString();
                    iktato = dgvBevKiv.Rows[Sorindex].Cells[11].Value.ToString();
                    iktkelte = dgvBevKiv.Rows[Sorindex].Cells[12].Value.ToString();
                    adatkozl = dgvBevKiv.Rows[Sorindex].Cells[13].Value.ToString();
                    tagok = dgvBevKiv.Rows[Sorindex].Cells[14].Value.ToString();
                    sajat = dgvBevKiv.Rows[Sorindex].Cells[15].Value.ToString();
                    munkh = dgvBevKiv.Rows[Sorindex].Cells[16].Value.ToString();
                    rendszeres = dgvBevKiv.Rows[Sorindex].Cells[17].Value.ToString();
                    egyszeri = dgvBevKiv.Rows[Sorindex].Cells[18].Value.ToString();
                    total = dgvBevKiv.Rows[Sorindex].Cells[19].Value.ToString();
                    ervenyes = dgvBevKiv.Rows[Sorindex].Cells[20].Value.ToString();
                    storno = dgvBevKiv.Rows[Sorindex].Cells[21].Value.ToString();
                    megjegyz = dgvBevKiv.Rows[Sorindex].Cells[22].Value.ToString();
                    konyvelt = dgvBevKiv.Rows[Sorindex].Cells[23].Value.ToString();
                }

                Close();
            }
            catch
            {
                MessageBox.Show("Nincs kiválasztott sor!");
            }
        }

        private void dgvBevKiv_DoubleClick(object sender, EventArgs e)
        {
            kivalaszt();
        }

        private void dgvBevKiv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                kivalaszt();
            }
        }

        private void dgvBevKiv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

        private void dgvBevKiv_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgvBevKiv.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgvBevKiv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvBevKiv.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgvBevKiv.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
        }
    }
}
