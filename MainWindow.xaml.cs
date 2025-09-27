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
using System.Threading;

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
        public string BP= "♔", CP= "♚";
        public List<Pozycja_Planszy> pozycje = new List<Pozycja_Planszy>();
        public int PktG = 0;
        public int PktK = 0;
        public string wej_ostatniejpoz;
        public string wej_przedostatniejpoz;
        public MainWindow()
        {
            InitializeComponent();

            
            Startup();
            Reset_Border();
            
            
            
        }
        
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button klikniety = sender as Button;

            //gdy biały pionek naciśnięty
            if((bool)(klikniety.Content == BP))
            {
                //Reset stanu wyboru
                if(state == 1) { state = 0;Reset_Border(); }

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
                    Reset_Border();
                    if (!Check_EndAsync()) { Ruch_Czarnych(); }
                    
                    
                }
                else if (state == 1)
                {
                    state = 0;
                    Reset_Border();
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
            Pkt_restart();
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

            // Wypisanie pozycji i odpowiedzi na te pozycje
            //foreach (Pozycja_Planszy poz in pozycje)
            //{
            //    Console.WriteLine("");
            //    Console.Write(poz.Wej + " ");
            //    foreach (string w in poz.Wyj)
            //    {
            //        Console.Write(w+ " ");
            //    }
              
            //}


        }

        //Reset wyboru 
        public void Reset_Border()
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
            
        }

        private async void Ruch_Czarnych()
        {
            
            bool poz_found = false;
            //Szukanie pozycji i odpowiadanie na nią
            foreach (Pozycja_Planszy poz in pozycje)
            {
                if(poz.Wej == Get_Poz())
                {
                    wej_przedostatniejpoz = wej_ostatniejpoz;
                    wej_ostatniejpoz = poz.Wej;
                    Set_Poz(poz.Wyj[0]);
                    await Task.Delay(500);
                    poz_found = true;
                }

            }
            if (!poz_found)
            {
                string plik = "pozycje.txt";
                File.AppendAllText(plik, Environment.NewLine + Get_Poz());
                Console.WriteLine("Pozycja do dodania");
            }
            
            Check_EndAsync();


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

        public void Pkt_restart()
        {
            Pkt_Gracza.Content = "Pkt Gracza: " + PktG;
            Pkt_AI.Content = "Pkt Kompa: "+ PktK;
        }

        public bool Check_EndAsync()
        {
            string poz = Get_Poz();
            bool Czy_brak_bialych=true;
            bool Czy_brak_czarnych=false;
            for(int i = 0; i < 3; i++)
            {
                if(poz[i] == '1')
                {
                    PktG += 1;
                    Pkt_restart();
                    PopUpAsync(1);
                    Popraw();
                    Reset_Gry();
                    return true;
                }
                if (poz[i + 6] == '2')
                {
                    PktK += 1;
                    Pkt_restart();
                    PopUpAsync(2);
                    Reset_Gry();
                    return true;
                }
            }
            bool remis = true;
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    if(buttony[i,j].Content == BP)
                    {
                        Czy_brak_bialych = false;
                        if (j > 0)
                        {

                            if (buttony[i, j - 1].Content == null)
                            {
                                remis = false;
                            }
                        }

                        //zbicia na skos
                        if (j > 0)
                        {
                            //w prawo
                            if (i > 0)
                            {
                                if (buttony[i - 1, j - 1].Content == CP)
                                {
                                    remis = false;
                                }
                            }

                            //w lewo
                            if (i < 2)
                            {
                                if (buttony[i + 1, j - 1].Content == CP)
                                {
                                    remis=false;
                                }
                            }
                        }
                    }
                    if(buttony[i, j].Content == CP)
                    {
                        Czy_brak_czarnych = false;
                    }
                }

            }
            if (remis) {
                Reset_Gry();
                PopUpAsync(0);
                return true;
            }
            else if (Czy_brak_czarnych)
            {
                PktG += 1;
                Pkt_restart();
                PopUpAsync(1);
                Popraw();
                Reset_Gry();
                return true;
            }
            else if (Czy_brak_bialych)
            {
                PktK += 1;
                Pkt_restart();
                PopUpAsync(2);
                Reset_Gry();
                return true;
            }
            return false;
            
        }

        public void Reset_Gry()
        {
            for (int i = 0; i <= 2; i++)
            {
                buttony[i, 0].Content = CP;
                buttony[i, 1].Content = null;
                buttony[i, 2].Content = BP;
            }
        }

        public async Task PopUpAsync(int i)
        {
            if (i == 0) { Komunikat.Content = "Remis"; }
            else if (i == 1) { Komunikat.Content = "Wygrywają Białe"; }
            else if (i == 2) { Komunikat.Content = "Wygrywają czarne"; }
            await Task.Delay(1000);
            Komunikat.Content = null;
        }

        public void Popraw()
        {
            foreach (Pozycja_Planszy poz in pozycje)
            {
                if (poz.Wej == wej_ostatniejpoz)
                {
                    Console.WriteLine("Usunieto "+ poz.Wyj[0]);
                    poz.Wyj.RemoveAt(0);
                    if(poz.Wyj.Count==0)
                    {
                        foreach (Pozycja_Planszy poz2 in pozycje)
                        {
                            if (poz2.Wej == wej_przedostatniejpoz)
                            {
                                Console.WriteLine("Usunieto " + poz2.Wyj[0]);
                                poz2.Wyj.RemoveAt(0);
                            }

                        }
                    }
                }

            }
        }
    }
}
