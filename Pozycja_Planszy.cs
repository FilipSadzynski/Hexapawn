using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Hexapawn
{

    
    public class Pozycja_Planszy
    {
        public string Wej { get; set; }
        public List<string> Wyj = new List<string>();
        public Pozycja_Planszy(string wej, string[] wyj)
        {
            Wej = wej;
            foreach(string w in wyj)
            {
                if (!string.IsNullOrEmpty(w))
                {
                    Wyj.Add(w);
                }
                
            }
        }

        
    }

    

   


}
