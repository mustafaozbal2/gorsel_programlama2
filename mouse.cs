namespace mouse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool surukleme = false;
        Point baslangic;
        private void button1_MouseMove(object sender, MouseEventArgs e)
        {
            if (surukleme)
            {
                button1.Left += e.X - baslangic.X;
                button1.Top += e.Y - baslangic.Y;
            }
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            surukleme = true;
            baslangic = e.Location;
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            surukleme = false;
        }
    }
}
