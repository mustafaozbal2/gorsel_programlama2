using System.Globalization;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool calisti = false;
        
        int salise = 0;
        int saniye = DateTime.Now.Second - 3;
        int dakika = DateTime.Now.Minute;
        int saat = DateTime.Now.Hour; 
        int gun = 0;

        int sistemsaat = DateTime.Now.Hour;
        int sistemdakika = DateTime.Now.Minute;
        int sistemsaniye = DateTime.Now.Second;

        private void button1_Click(object sender, EventArgs e)
        {
            label8.Text = string.Empty;
            label9.Text = string.Empty;
            label10.Text = string.Empty;
            label6.Text = string.Empty;
            label7.Text = string.Empty;

            label2.Text = saniye.ToString();
            label3.Text = dakika.ToString();
            label1.Text = saat.ToString();
            label5.Text = salise.ToString();
            label4.Text = gun.ToString();

            timer1.Interval = 10;
            timer1.Start();
            button1.Enabled = false;

            if(button1.BackColor == Color.Green)
            {
                Environment.Exit(0);
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(calisti == false)
            {
                salise++;
                if (salise == 100)
                {
                    saniye++;
                    salise = 0;
                }

                if (saniye == 60)
                {
                    dakika++;
                    saniye = 0;
                }

                if (dakika == 60)
                {
                    saat++;
                    dakika = 0;
                }

                if (saat == 24)
                {
                    gun++;
                    saat = 0;
                }

                label2.Text = saniye.ToString();
                label3.Text = dakika.ToString();
                label1.Text = saat.ToString();
                label5.Text = salise.ToString();
                label4.Text = gun.ToString();
            }

            if (calisti == true)
            {
                label2.Text = string.Empty;
                label3.Text = string.Empty;
                label1.Text = string.Empty;
                label5.Text = string.Empty;
                label4.Text = string.Empty;

                button1.Enabled = true;
                button1.BackColor = Color.Green;

                label8.Text = salise.ToString();
                label9.Text = saniye.ToString();
                label10.Text = dakika.ToString();
                label6.Text = saat.ToString();
                label7.Text = dakika.ToString();

                if (salise == 0)
                {
                    salise = 100;
                    saniye--;
                }

                salise--;

                if (saniye == 0)
                {
                    dakika--;
                    saniye = 60;
                }

                if (dakika == 00)
                {
                    saat--;
                    dakika = 60;
                }

                if (saat == 00)
                {
                    gun--;
                    saat = 24;
                }
            }

            if (saat == sistemsaat && dakika == sistemdakika && saniye == sistemsaniye && calisti == false)
            {
                timer1.Stop();
                calisti = true;
                button1.Enabled = true;
            }
        }

    }
}
