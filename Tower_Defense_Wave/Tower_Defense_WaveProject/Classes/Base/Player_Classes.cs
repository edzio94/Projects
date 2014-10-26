using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Player_Classes
{
    class Player_Base{
       
        private string ID = "PLAYER";
        public int Lives;
        public int Gold;


        public Player_Base(int L, int G)
        {
            Lives = L;
            Gold = G;
        }

        internal void Add_gold(int p)
        {
            Gold += p;
        }
    }


}
