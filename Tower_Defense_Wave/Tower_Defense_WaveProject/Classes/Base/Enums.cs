using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enums
{
    enum Grid_chars : int { START = 1, END = 2, BLOCK = 3, FREE = 4, USED = 5, PLAYER = 6 }

    enum Block : int{FIELD1 =0 ,START = 1, END =2 , VERT = 3, VERT_LEFT=4, VERT_RIGHT=5, VERT_LEFT_REV=6, VERT_RIGHT_REV=7 ,HORI = 8,
                       HORI_LEFT = 9, HORI_RIGHT= 10, HORI_LEFT_REV = 11, HORI_RIGHT_REV = 12}
   enum AnimState { Idle, Right, Left, Up, Down };

   enum IDs : int { TOWER =1 , BULLET, MONSTER, ROAD, FIELD};

   enum Towers_Bar_Position_X : int {Tower1 = 0 };
   enum Tower_Bar_Position_Y : int {Tower1 = 0 };
}
    