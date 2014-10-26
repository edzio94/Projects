using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Libs;
using Static;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Animation;
using WaveEngine.Common.Graphics;
using WaveEngine.Common;
using WaveEngine.Common.Math;
using WaveEngine.Components.Gestures;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Managers;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Sound;
using WaveEngine.Framework.UI;

namespace Map_Classes
{
   
    
    
    /*
     *    VERT GOES UP, VERT_REV GOES DOWN
     *    HORI GOES RIGHT, HORI_REV GOES LEFT
     * */

    
    class Block 
    {
        public string ID;
        public string Block_Name;
        public int Block_Width;
        public int Block_Height;
        public int Grid_X, Grid_Y; 

        public int Block_Position_X, Block_Poisiton_Y;
        public float[] Block_Edges; // [0] = x [1] = x+width [2] = y [3] = y+height;
        //ADD STRING ID TO CONSTRUCTOR BLOCK !
       
        public Block(string ID,string path_name, int width, int height, int grid_x, int grid_y)
        {
            this.ID = ID;

            Block_Edges = new float[4];

            Block_Name = path_name;
            Block_Height = height;
            Block_Width = width;

            Grid_X = grid_x;
            Grid_Y = grid_y;

            Block_Position_X = width * Grid_X + Const.OFFSET ; Block_Edges[0] = Block_Position_X - (width/2) ; Block_Edges[1] = Block_Position_X + (Block_Width/2);
            Block_Poisiton_Y = height * Grid_Y + Const.OFFSET ; Block_Edges[2] = Block_Poisiton_Y - (height/2) ; Block_Edges[3] = Block_Poisiton_Y + (Block_Height/2);

        

        }
        // IF Monster x > Block_position_x && Monster x  < Block_positiokn_x+Block_Width then player_x = grid_x;
        public string id
        {
            get
            {
                return ID;
            }
            set
            {
                ID = value;
            }

        }

        public bool check_grid_position(float pos_x, float pos_y)
        {
            // ERROR: MIGHT BE MISTAKE WITH EDGES OF EVERY BLOCK, SOME EDGES MAY BE LOCATED IN SAME PLACE
            // X AND Y SPRITE ARE IN RIGHT DOWN CORNER/ TRY TO GET THEM INTO MIDDLE
             if(pos_x >= Block_Edges[0] && pos_x <= Block_Edges[1] && pos_y >= Block_Edges[2] && pos_y <= Block_Edges[3])
                return true;
            else
                return false;
        }

    }

    class Map_Base : Behavior 
    {
        public int[,] map;
        public Entity Map_Bitmaps;
        public int starting_x, starting_y;
        public int ending_x, ending_y;
        public int range;
        public List<Block> Bitmaps;
        private string Bitmap_Path;
        public Block[,] Map_Blocks;

        
        private Entity Create_Bitmap_Map(string name, string path,int x,int y, int height, int width
            ,float good_x, float good_y)
        {

            Entity Bitmap_child = new Entity(name)
            .AddComponent(new Transform2D()
            {
                X = x,
                Y = y,
                Origin = new Vector2(0.5f, 0.5f)
            })
            .AddComponent(new Sprite(path))
            .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));

        
            return Bitmap_child;
        }

        public Tuple<int,int,int,int,string> Get_Grid_Position(float x, float y)
        {
            int d1 = Map_Blocks.GetLength(0);
            int d2 = Map_Blocks.GetLength(1);
            int return_x = -1;
            int return_y= -1;
            int return_grid_x = -1;
            int return_grid_y = -1;

            string return_ID = "";
            for (int i = 0; i < d1; i++)
            {
                for (int j = 0; j < d2; j++)
                {
                    if (Map_Blocks[j, i].check_grid_position(x, y))
                    {
                        return_y = Map_Blocks[j, i].Block_Poisiton_Y;
                        return_x = Map_Blocks[j, i].Block_Position_X;
                        return_grid_x = i;
                        return_grid_y = j;
                        return_ID = Map_Blocks[j, i].ID;
                        break;
                    }
                }
            }
            return Tuple.Create(return_y,return_x,return_grid_y,return_grid_x,return_ID) ;
        }
        

        protected override void Update(TimeSpan gameTime)

        {

            
        }
        public void Create_Blocks()
        {
            for (int i = 0; i < range; i++)
            {
                for (int j = 0; j < range; j++)
                {

                    if ((map[j, i] >= 1) && map[j,i] <= 12)
                    {                          
                        Map_Blocks[j, i] = new Block("ROAD",Bitmap_Path + Const.Bitmaps_names[map[j, i]], Const.BITMAP_SIZE, Const.BITMAP_SIZE, i, j);
                    }
                    else
                    {
                        Map_Blocks[j, i] = new Block("FIELD", Bitmap_Path + Const.Bitmaps_names[map[j, i]], Const.BITMAP_SIZE, Const.BITMAP_SIZE, i, j);
                        //MAP_BLOCK[J,I] = ("FIELD")
                    }
                }
            }
        }
        public Map_Base(string path,int n, int start_x, int start_y, int end_x, int end_y)
        {
           
            Map_Bitmaps = new Entity("Whole_Map");
            Bitmap_Path = path;
            Bitmaps = new List<Block>();
            Map_Blocks = new Block[5, 5];
            range = n;
            ending_y = end_y;
            ending_x = end_x;

            map = new int[5, 5] { { 1, 0, 0, 0, 0 },
                                   { 7, 8, 8, 10, 0 }, 
                                   { 0, 0, 0, 3, 0 },
                                   { 0, 0, 0, 3, 0 },
                                   { 0, 0, 0, 7, 2 } };

            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    if (map[j, i] == 1)
                    {
                        starting_y = j;
                        starting_x = i;
                    }
                }
            }

              
             Create_Blocks();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0 ; j < 5; j++)
                {
                   
                    //if ( Map_Blocks[j,i] != null && Map_Blocks[j,i].ID == "ROAD")
                    //{
                        var toadd = Create_Bitmap_Map("Block [" + j + "]["+i+"]", Map_Blocks[j,i].Block_Name, Map_Blocks[j,i].Block_Position_X, Map_Blocks[j,i].Block_Poisiton_Y, Map_Blocks[j,i].Block_Width,
      Map_Blocks[j,i].Block_Height, Map_Blocks[j,i].Block_Position_X, Map_Blocks[j,i].Block_Poisiton_Y);
                        Map_Bitmaps.AddChild(toadd);
                    //}
                }
            }
            
            

        }


    }



}
