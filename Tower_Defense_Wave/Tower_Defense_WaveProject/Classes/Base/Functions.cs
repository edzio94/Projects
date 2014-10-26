using System;
using System.Collections.Generic;
using WaveEngine.Common.Graphics;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Common.Math;
using WaveEngine.Framework.UI;

namespace Static
{
    static class Functions
    {                          
       
        //private static int Last_grid_x = -1;
        //private static int Last_grid_y = -1;
        public static void get_Directions(int difX,int difY,ref int X,ref int Y)
        {
            if (difX > 0)
                X = -1;
            else if (difX < 0)
                X = 1;
            else
                X = 0;

            if (difY > 0)
                Y = -1;
            else if (difY < 0)
                Y = 1;
            else
                Y = 0;
        }

        public static List<T> Swap<T>(this List<T> list, int indexA, int indexB)
        {
           
           
            for (int i = indexA + 1; i < list.Count;i++ )
            {
                //Console.WriteLine("Getting " + i + "element at " + (i - 1) + " Place");   
              //  T tmp = list[i - 1];
                list[i - 1] = list[i];
              //  list[i] = tmp;
            }
            list.RemoveRange(list.Count - 1, 1);
           
                return list;
           }
        
         /*  ZROBIC NA RAZIE PROSTA FUNKCJE SPRAWDZAJACA
         * 
         * POTEM ULEPSZYC BY PIERWSZENSTWO MIAŁO SPRAWDZENIE,
         * CZY W KTÓRYMŚ Z MOŻLIWYCH KIERUNKOW JEST POLE | END | 
          * /////////////////////////////
          * CHEKING IF NEXT FIELD IS EMPYTY AND GOOD TO GO
         */
        private static bool Array_range(int x, int y, int [,] map, int lX, int lY)
        {
            
            int d1 = map.GetLength(0);
            int d2 = map.GetLength(1);
            bool ret = false;
            if ((x >= 0) && (x < d2) && (y >= 0) && (y < d1))
            {
                if ((map[y, x] <= 12) && (map[y, x] >= 1)  )
                
                {
                    if (x != lX || y != lY)
                    ret = true;
                    else
                    ret = false;
                }
            }
            return ret;
        }




        public static Tuple <int, int,int ,int> Astar(int indexX, int indexY, int lastIndexX,int lastIndexY,int [,] gridMap)
        {
            int [] index1 = new int[] {indexX + 1, indexX + 0, indexX - 1, indexX + 0 };
            int [] index2 = new int[] {indexY + 0, indexY + 1, indexY + 0, indexY - 1 };
            int a_x = indexX; 
            int a_y =indexY;
            for (int i = 0; i < 4; i++ )
            {
                if (Array_range(index1[i], index2[i],gridMap,lastIndexX, lastIndexY))
                {
                    lastIndexX = indexX;
                    lastIndexY = indexY;
                    a_x = index1[i]; a_y = index2[i];
                    break;
                }
            }

            return Tuple.Create(a_y, a_x,lastIndexX,lastIndexY);
        }

        // Wave Engine Static Functions


        // Make a TextBlock functions

        public static TextBlock CreateTextBlock(string playerGold, int width, Color color, TextAlignment textAlignment,
            HorizontalAlignment horiAligmAlignment, Thickness marginThickness)
        {
            var textBlock = new TextBlock
            {
                Text = playerGold.ToString(),
                Width = width,
                Foreground = color,
                HorizontalAlignment = horiAligmAlignment,
                Margin = marginThickness
            };
            return textBlock;
        }
             

        // Make an Entity functions
        public static Entity Create_entity(string name, float x, float y, string sprite ) 
        {
            Entity temp = new Entity(name)
            .AddComponent(new Transform2D()
            {
                X = x,
                Y = y,
                Origin = new Vector2(0.5f, 0.5f)
            })
            .AddComponent(new Sprite(sprite))
            .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
            return temp;
        }
        public static Entity Create_entity(string name, float x, float y, string sprite,ref Transform2D t2d)
        {
            Entity temp = new Entity(name)
            .AddComponent(t2d = new Transform2D()
            {
                X = x,
                Y = y,
                Origin = new Vector2(0.5f, 0.5f)
            })
            .AddComponent(new Sprite(sprite))
            .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
            return temp;
        }
    }

    static class Const
    {
        public static int List_Counter = 0;
        public static List<TimeSpan> L_Wave_Break_Timer = new List<TimeSpan>()
        {
            {new TimeSpan(0,0,0,30)},
            {new TimeSpan(0,0,0,45)}
        };
        
        public const int OFFSET = 100;
        public const int BITMAP_SIZE = 64;
        public static Dictionary<int, string> Bitmaps_names = new Dictionary<int, string>()
        {
            {(int)Enums.Block.HORI, "[DROGA] prosta.wpk"},
            {(int)Enums.Block.HORI_LEFT, "[DROGA] w prawo_reverse.wpk"},
            {(int)Enums.Block.HORI_LEFT_REV,"[DROGA] w lewo_reverse.wpk"},
            {(int)Enums.Block.HORI_RIGHT, "[DROGA] w prawo.wpk"},
            {(int)Enums.Block.HORI_RIGHT_REV, "[DROGA] w lewo.wpk"},
            {(int)Enums.Block.VERT, "[DROGA] w dol.wpk"},
            {(int)Enums.Block.VERT_LEFT, "[DROGA] w prawo.wpk"},
            {(int)Enums.Block.VERT_LEFT_REV,"[DROGA] w prawo_reverse.wpk"},
            {(int)Enums.Block.VERT_RIGHT, "[DROGA] w lewo_reverse.wpk"},
            {(int)Enums.Block.VERT_RIGHT_REV,"[DROGA] w lewo.wpk"},
            {(int)Enums.Block.START, "[DROGA] test.wpk"},
            {(int)Enums.Block.END, "[DROGA] test.wpk"},
            {(int)Enums.Block.FIELD1,"[FIELD] bush.wpk"}
        };
        public static Dictionary<string, Tuple<int, int>> Tower_Position = new Dictionary<string, Tuple<int, int>>()
        {
            {"Tower1", Tuple.Create(0,0)},
            {"Chandler_tower", Tuple.Create(172,0)}
        };
        public static Dictionary<string, int> Tower_Costs = new Dictionary<string, int>()
        {
            {"Tower1", 75},
            {"Chandler_tower", 100}
        };

        public static Dictionary<string, string> Tower_bitmap = new Dictionary<string, string>()
        {
            {"Tower1", "Content/Towers/Tower_Beta/Tower1.wpk"},
            {"Chandler_tower","Content/Towers/Tower_Chandler/Chandler_tower.wpk"}
        };
    }
    /// <summary>
    ///  Position of towers bitmap in tower bars
    /// </summary>
    static class Bitmaps_positions
    {

    }

}

