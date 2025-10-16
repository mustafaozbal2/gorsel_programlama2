using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SayiSiralamaOyunu
{
    public partial class Form1 : Form
    {
        List<Button> sayiButonlari = new List<Button>();
        List<int> siraliCiftSayilar = new List<int>();
        int beklenenCiftSayiIndexi = 0;
        int kalanSure = 60;
        Random random = new Random();

        Color anaButonRenk = Color.FromArgb(0, 122, 204);
        Color anaButonUzeriRenk = Color.FromArgb(28, 151, 234);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ModernTasarimUygula();
            positionTimer.Interval = 2000;
            positionTimer.Tick += positionTimer_Tick;
            OyunDurumunuAyarla(false);
        }

        private void ModernTasarimUygula()
        {
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = "Sayı Sıralama Oyunu";

            panelOyunAlani.BackColor = Color.FromArgb(62, 62, 66);
            panelOyunAlani.BorderStyle = BorderStyle.None;

            lblSure.Font = new Font("Segoe UI Semibold", 12F);
            lblKalanSure.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblKalanSure.ForeColor = Color.FromArgb(0, 198, 145);

            listBoxGecenler.BackColor = Color.FromArgb(62, 62, 66);
            listBoxGecenler.ForeColor = Color.White;
            listBoxGecenler.Font = new Font("Segoe UI", 14F);
            listBoxGecenler.BorderStyle = BorderStyle.None;

            foreach (Button btn in new[] { btnBasla, btnBitir })
            {
                btn.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                btn.ForeColor = Color.White;
                btn.BackColor = anaButonRenk;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Cursor = Cursors.Hand;
                btn.MouseEnter += Buton_MouseEnter;
                btn.MouseLeave += Buton_MouseLeave;
            }
        }

        private void Buton_MouseEnter(object sender, EventArgs e)
        {
            (sender as Button).BackColor = anaButonUzeriRenk;
        }

        private void Buton_MouseLeave(object sender, EventArgs e)
        {
            (sender as Button).BackColor = anaButonRenk;
        }

        private void btnBasla_Click(object sender, EventArgs e)
        {
            OyunuBaslat();
        }

        private void OyunuBaslat()
        {
            TemizlikYap();

            List<int> sayilar = new List<int>();
            while (sayilar.Count < 10)
            {
                int yeniSayi = random.Next(1, 101);
                if (!sayilar.Contains(yeniSayi))
                {
                    sayilar.Add(yeniSayi);
                }
            }

            Color[] oyunButonRenkleri = new[] { Color.FromArgb(209, 52, 52), Color.FromArgb(76, 175, 80), Color.FromArgb(255, 152, 0), Color.FromArgb(63, 81, 181), Color.FromArgb(156, 39, 176) };

            foreach (int sayi in sayilar)
            {
                Button btn = new Button();
                btn.Text = sayi.ToString();
                btn.Size = new Size(60, 50);
                btn.Font = new Font("Segoe UI Semibold", 14F);
                btn.ForeColor = Color.White;
                btn.BackColor = oyunButonRenkleri[random.Next(oyunButonRenkleri.Length)];
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Cursor = Cursors.Hand;
                btn.Location = new Point(random.Next(5, panelOyunAlani.Width - btn.Width - 5), random.Next(5, panelOyunAlani.Height - btn.Height - 5));
                btn.Click += SayiButonu_Click;
                sayiButonlari.Add(btn);
                panelOyunAlani.Controls.Add(btn);
            }

            siraliCiftSayilar = sayilar.Where(s => s % 2 == 0).OrderBy(s => s).ToList();

            OyunDurumunuAyarla(true);
            gameTimer.Start();
            positionTimer.Start();
        }

        private void SayiButonu_Click(object sender, EventArgs e)
        {
            Button tiklananButon = (Button)sender;
            int tiklananSayi = Convert.ToInt32(tiklananButon.Text);

            tiklananButon.Visible = false;
            listBoxGecenler.Items.Add(tiklananSayi);

            bool dogruHamle = false;

            if (tiklananSayi % 2 == 0)
            {
                if (siraliCiftSayilar.Count > beklenenCiftSayiIndexi && tiklananSayi == siraliCiftSayilar[beklenenCiftSayiIndexi])
                {
                    dogruHamle = true;
                    beklenenCiftSayiIndexi++;
                }
            }

            if (!dogruHamle)
            {
                OyunBitti("Sıralama hatalı!", false);
                return;
            }

            if (beklenenCiftSayiIndexi == siraliCiftSayilar.Count)
            {
                OyunBitti(kalanSure + " saniye kala oyunu doğru bitirdin!", true);
            }
        }

        private void TemizlikYap()
        {
            panelOyunAlani.Controls.Clear();
            sayiButonlari.Clear();
            siraliCiftSayilar.Clear();
            listBoxGecenler.Items.Clear();
            beklenenCiftSayiIndexi = 0;
            kalanSure = 60;
            lblKalanSure.Text = kalanSure + " sn";
        }

        private void OyunDurumunuAyarla(bool aktifMi)
        {
            panelOyunAlani.Enabled = aktifMi;
            btnBitir.Enabled = aktifMi;
            btnBasla.Enabled = !aktifMi;
        }

        private void OyunBitti(string mesaj, bool kazandiMi)
        {
            gameTimer.Stop();
            positionTimer.Stop();
            string baslik = kazandiMi ? "Tebrikler!" : "Oyun Bitti";
            MessageBox.Show(mesaj, baslik);
            OyunDurumunuAyarla(false);
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            kalanSure--;
            lblKalanSure.Text = kalanSure + " sn";

            if (kalanSure <= 0)
            {
                OyunBitti("Süre bitti!", false);
            }
        }

        private void positionTimer_Tick(object sender, EventArgs e)
        {
            foreach (Button btn in sayiButonlari)
            {
                if (btn.Visible)
                {
                    int x = random.Next(5, panelOyunAlani.Width - btn.Width - 5);
                    int y = random.Next(5, panelOyunAlani.Height - btn.Height - 5);
                    btn.Location = new Point(x, y);
                }
            }
        }

        private void btnBitir_Click(object sender, EventArgs e)
        {
            OyunBitti("Oyun sizin tarafınızdan bitirildi.", false);
        }
    }
}