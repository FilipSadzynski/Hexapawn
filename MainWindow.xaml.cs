using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hexapawn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Button[] buttony = new Button[9];
        public int state = 0;
        public Button ostatni_klikniety;
        public MainWindow()
        {
            InitializeComponent();
            Startup();
            Reset();
        }
        
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button klikniety = sender as Button;
            if((bool)(klikniety.Content == "BP"))
            {
                int poz = Findpoz(klikniety);
                if (poz > 2)
                {
                    state = 1;
                    buttony[poz - 3].BorderBrush = Brushes.Yellow;
                }
            }
            
        }

        //Co się dzieje przed grą
        public void Startup()
        {
            buttony[0] = Button1;
            buttony[1] = Button2;
            buttony[2] = Button3;
            buttony[3] = Button4;
            buttony[4] = Button5;
            buttony[5] = Button6;
            buttony[6] = Button7;
            buttony[7] = Button8;
            buttony[8] = Button9;
        }

        //reset wyboru 
        public void Reset()
        {
            for(int i=0; i<buttony.Length; i++)
            {
                buttony[i].BorderBrush = Brushes.Gray;
            }
        }

        public int Findpoz(Button b)
        {
            for(int i = 0; i < buttony.Length; i++)
            {
                if (b == buttony[i])
                {
                    return i;
                }
            }
            return -1;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            
            Start.Visibility = Visibility.Collapsed;
            for(int i = 0; i <=2; i++)
            {
                buttony[i].Content = "CP";
            }
            for (int i = 6; i <=8; i++)
            {
                buttony[i].Content = "BP";
            }
        }
    }
}
