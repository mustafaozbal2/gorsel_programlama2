// Form1.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        // Butonları ve hedef pozisyonlarını tutacak iç içe bir sınıf (nested class)
        private class ButtonData
        {
            public Button Button { get; set; }
            public Point TargetCorner { get; set; }
            public Point StartCenter { get; set; }
        }

        private readonly List<ButtonData> buttonList = new List<ButtonData>();
        private const int ButtonSize = 75; // Buton boyutu
        private bool shouldAnimate = true; // Animasyonun devam edip etmeyeceğini kontrol eden bayrak

        public Form1()
        {
            InitializeComponent();
            // Form yüklendiğinde çalışacak olan metodu burada bağlıyoruz.
            this.Load += new EventHandler(Form1_Load);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.SizeChanged += new EventHandler(Form1_SizeChanged);
            this.Text = "Buton Animasyonu (Task ile)";
            this.DoubleBuffered = true; // Pürüzsüz hareket için titremeyi önler

            // Butonları oluştur ve yerleştir
            CreateAndPositionButtons();

            // Animasyonu başlat
            StartAnimationTask();
        }

        private void CreateAndPositionButtons()
        {
            // Formun merkezini hesapla
            Point formCenter = new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2);

            // Butonların başlangıç pozisyonu (merkezden)
            Point centerStart = new Point(formCenter.X - ButtonSize / 2, formCenter.Y - ButtonSize / 2);

            // 4 Köşe Hedefleri: (SolÜst, SağÜst, SolAlt, SağAlt)
            Point[] targetCorners = new Point[]
            {
                new Point(10, 10), // Sol Üst
                new Point(this.ClientSize.Width - ButtonSize - 10, 10), // Sağ Üst
                new Point(10, this.ClientSize.Height - ButtonSize - 10), // Sol Alt
                new Point(this.ClientSize.Width - ButtonSize - 10, this.ClientSize.Height - ButtonSize - 10) // Sağ Alt
            };

            for (int i = 0; i < 4; i++)
            {
                Button btn = new Button
                {
                    Text = $"Buton {i + 1}",
                    Size = new Size(ButtonSize, ButtonSize),
                    Location = centerStart // Başlangıçta hepsi merkezde
                };
                this.Controls.Add(btn);

                buttonList.Add(new ButtonData
                {
                    Button = btn,
                    TargetCorner = targetCorners[i],
                    StartCenter = centerStart
                });
            }
        }

        // Form boyutu değişirse, buton hedeflerini yeniden hesapla
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            Point formCenter = new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2);
            Point centerStart = new Point(formCenter.X - ButtonSize / 2, formCenter.Y - ButtonSize / 2);

            // Yeni Köşe Hedefleri
            if (buttonList.Count == 4)
            {
                buttonList[0].TargetCorner = new Point(10, 10); // Sol Üst
                buttonList[1].TargetCorner = new Point(this.ClientSize.Width - ButtonSize - 10, 10); // Sağ Üst
                buttonList[2].TargetCorner = new Point(10, this.ClientSize.Height - ButtonSize - 10); // Sol Alt
                buttonList[3].TargetCorner = new Point(this.ClientSize.Width - ButtonSize - 10, this.ClientSize.Height - ButtonSize - 10); // Sağ Alt
            }

            // Başlangıç merkez pozisyonunu da güncelle
            foreach (var data in buttonList)
            {
                data.StartCenter = centerStart;
            }
        }

        private void StartAnimationTask()
        {
            // Form kapandığında animasyonu durdurmak için olay ekle
            this.FormClosing += (sender, e) => { shouldAnimate = false; };

            // Arka planda animasyonu çalıştıracak bir Task başlat
            Task.Run(async () =>
            {
                float speed = 0.06f; // Hareket hızı (0.01 yavaş, 0.1 hızlı)
                float t = 0; // Animasyon ilerlemesi (0.0 ile 1.0 arası)
                bool movingToCorner = true; // Köşeye mi gidiyor, merkeze mi?

                // Ana animasyon döngüsü
                while (shouldAnimate)
                {
                    t += speed;

                    // Animasyon süresi dolduysa (0'dan 1'e ulaştıysa)
                    if (t > 1.0f)
                    {
                        t = 0; // Sıfırla
                        movingToCorner = !movingToCorner; // Hedef değiştir (Köşe <-> Merkez)
                        await Task.Delay(500); // Yön değiştirmeden önce yarım saniye bekle
                    }

                    // UI güncellemesi için Invoke kullan (Farklı thread'den UI'a güvenli erişim)
                    this.Invoke((MethodInvoker)delegate
                    {
                        UpdateButtonPositions(t, movingToCorner);
                    });

                    // Döngüyü yavaşlat (yaklaşık 60 FPS simülasyonu)
                    await Task.Delay(16);
                }
            });
        }

        private void UpdateButtonPositions(float t, bool toCorner)
        {
            // Her butonun yeni pozisyonunu hesapla
            foreach (var data in buttonList)
            {
                Point startPoint, endPoint;

                if (toCorner)
                {
                    // Merkezden Köşeye doğru hareket
                    startPoint = data.StartCenter;
                    endPoint = data.TargetCorner;
                }
                else
                {
                    // Köşeden Merkeze doğru hareket
                    startPoint = data.TargetCorner;
                    endPoint = data.StartCenter;
                }

                // *Lineer İnterpolasyon (Lerp)* formülü ile pozisyon hesaplama
                int newX = (int)(startPoint.X + (endPoint.X - startPoint.X) * t);
                int newY = (int)(startPoint.Y + (endPoint.Y - startPoint.Y) * t);

                data.Button.Location = new Point(newX, newY);
            }
        }
    }
}
