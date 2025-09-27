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
using System.IO;

namespace Hexapawn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Button[,] buttony = new Button[3,3];
        public int state = 0;
        public Button ostatni_klikniety;
        public string BP="BP", CP="CP";
        public List<Pozycja_Planszy> pozycje = new List<Pozycja_Planszy>();
        public MainWindow()
        {
            InitializeComponent();


            Startup();
            Reset();
            
            
            
        }
        
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button klikniety = sender as Button;

            //gdy biały pionek naciśnięty
            if((bool)(klikniety.Content == BP))
            {
                //reset stanu wyboru
                if(state == 1) { state = 0;Reset(); }

                //pozycja klikniętego przycisku w tablicy buttony
                int[] poz = Findpoz(klikniety);

                //szukanie pozycji do ruchu
                    //przed pionkiem

                    if (poz[1] >0)
                        {
                        
                            if(buttony[poz[0],poz[1]-1].Content == null)
                            {
                                buttony[poz[0], poz[1] - 1].BorderBrush = Brushes.Yellow;
                                state = 1;
                            }
                            ostatni_klikniety = klikniety;
                        }

                //zbicia na skos
                if (poz[1] > 0)
                {
                    //w prawo
                    if (poz[0] > 0)
                    {
                        if (buttony[poz[0]-1,poz[1]-1].Content == CP)
                        {
                            buttony[poz[0]-1, poz[1] - 1].BorderBrush = Brushes.Yellow;
                            state = 1;
                        }
                    }

                    //w lewo
                    if (poz[0] < 2)
                    {
                        if (buttony[poz[0] +1, poz[1] - 1].Content == CP)
                        {
                            buttony[poz[0] +1, poz[1] - 1].BorderBrush = Brushes.Yellow;
                            state = 1;
                        }
                    }
                }
            }

            //Przy naciśnięciu pola niebędącego białym pionkiem
            else
            {
                if (klikniety.BorderBrush == Brushes.Yellow)
                {
                    state = 0;
                    klikniety.Content = BP;
                    ostatni_klikniety.Content = null;
                    Reset();
                    Ruch_Czarnych();
                }
                else if (state == 1)
                {
                    state = 0;
                    Reset();
                }
            }
            
        }

        //Co się dzieje przed grą
        public void Startup()
        {
            buttony[0,0] = Button1;
            buttony[1,0] = Button2;
            buttony[2,0] = Button3;
            buttony[0,1] = Button4;
            buttony[1,1] = Button5;
            buttony[2,1] = Button6;
            buttony[0,2] = Button7;
            buttony[1,2] = Button8;
            buttony[2,2] = Button9;

            string plik = "pozycje.txt";

            string[] lines = File.ReadAllLines(plik);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split('/');

                string wej = parts[0].Trim();

                string[] wyj = parts[1].Split(',');

                Pozycja_Planszy poz = new Pozycja_Planszy(wej, wyj);
                pozycje.Add(poz);
                
            }

            foreach (Pozycja_Planszy poz in pozycje)
            {
                Console.WriteLine("");
                Console.Write(poz.Wej + " ");
                foreach (string w in poz.Wyj)
                {
                    Console.Write(w+ " ");
                }
              
            }


        }

        //reset wyboru 
        public void Reset()
        {
            for(int i=0; i<3; i++)
            {
                for(int j=0;j<3; j++)
                {
                    buttony[i,j].BorderBrush = Brushes.Gray;
                }
                
            }
        }
        //szukanie pozycji kliknietego przycisku
        public int[] Findpoz(Button b)
        {
            int[] result = new int[2];
            for(int j = 0; j < 3; j++)
            {
                for(int i=0;i<3;i++)
                {
                    if (b == buttony[i,j])
                    {
                        result[0] = i;
                        result[1] = j;
                        return result;
                    }
                }
                
            }
            result[0] = -1;
            result[1] = -1;
            return result;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            
            Start.Visibility = Visibility.Collapsed;
            for(int i = 0; i <=2; i++)
            {
                buttony[i,0].Content = CP;
                buttony[i, 2].Content = BP;
            }
            Console.WriteLine(Get_Poz());
        }

        private void Ruch_Czarnych()
        {
            //Szukanie pozycji i odpowiadanie na nią
            foreach (Pozycja_Planszy poz in pozycje)
            {
                if(poz.Wej == Get_Poz())
                {
                    Console.WriteLine(poz.Wyj[0]);
                    Set_Poz(poz.Wyj[0]);
                }

            }
        }

        //Pozycja jako ciąg 9 cyfr
        private string Get_Poz()
        {
            string wyn = "";
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (buttony[i,j].Content == null) { wyn += "0"; }
                    else if(buttony[i, j].Content == BP) { wyn += "1"; }
                    else if(buttony[i, j].Content == CP) { wyn += "2"; }
                }

            }
            return wyn;
        }
        //ustawianie planszy z 9 cyfr
        private void Set_Poz(string s)
        {
            int inc = 0;
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (s[inc]=='0') { buttony[i, j].Content = null; }
                    else if (s[inc] == '1') { buttony[i, j].Content = BP; }
                    else if (s[inc] == '2') { buttony[i, j].Content = CP; }
                    inc++; 
                }

            }
        }
    }
}
