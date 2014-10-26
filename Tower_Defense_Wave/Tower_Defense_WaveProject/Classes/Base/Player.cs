using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WaveEngine.Graphics;

using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Sound;
using WaveEngine.Common.Media;

using WaveEngine.Common.Input;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;

using WaveEngine.Components.Animation;
using WaveEngine.Components;
using WaveEngine.Components.Graphics2D;

using Static;

namespace Mobs
{
    class Mob_Base : Behavior
    {
        public const string ID = "MONSTER";
   
        private const float SPEED = 1.4f;
     
        public Animation2D anim2D;
        public Transform2D trans2D;
        

        public int HP;
        public int Monster_gold;
        public int Lives_value;
        public bool Alive;
        protected int Last_Grid_X;
        protected int Last_Grid_Y;

        protected int Direction_x;
        protected int Direction_y;
              
        public int Grid_x, Grid_y, F_Grid_x, F_Grid_y;
        //public int Direction_x, Direction_y;
        

        /// <summary>
        /// 1 or -1 indicating right or left respectively
        /// </summary>


        public Entity Mob;
        public Mob_Base()
        {

        }

        public Mob_Base(int start_grid_x, int start_grid_y, int [,] map, int i)
        {

            HP = 100;
            Alive = true;
            Lives_value = 1;
            Monster_gold = 20;                     
            Alive = true;
            
            Last_Grid_X = Last_Grid_Y = -1;
                       
            this.anim2D = null;//new Animation2D();
            this.trans2D = null;
                     
            this.Grid_x = start_grid_x;
            this.Grid_y = start_grid_y;

   
            var Ast = Static.Functions.Astar(Grid_x, Grid_y,Last_Grid_X ,Last_Grid_Y,map);
            this.F_Grid_x = Ast.Item2;
            this.F_Grid_y = Ast.Item1;
            get_direction();
            
            
            Mob = new Entity("Enemy " + i)
                .AddComponent(trans2D = new Transform2D()
                {
                    X = (start_grid_x * Const.BITMAP_SIZE) + Const.OFFSET,
                    Y = (start_grid_y * Const.BITMAP_SIZE) + Const.OFFSET,

                    Origin = new Vector2(0.5f, 0.5f)
                })                
                .AddComponent(new Sprite("Content/Monsters/Monster_Beta/Mob.wpk"))
                .AddComponent(Animation2D.Create<TexturePackerGenericXml>("Content/Monsters/Monster_Beta/Mob.xml")
                .Add("Idle", new SpriteSheetAnimationSequence() { First = 1, Length = 1, FramesPerSecond = 30 })
                    .Add("Running", new SpriteSheetAnimationSequence() { First = 1, Length = 4, FramesPerSecond = 5 }))
                .AddComponent(new AnimatedSpriteRenderer(DefaultLayers.Alpha));
            
                
            anim2D = Mob.FindComponent<Animation2D>();
            anim2D.CurrentAnimation = "Running";
            anim2D.Play(true);
            
           
        }

        public void change_pos(int Level_Block_X, int Level_Block_Y,int [,] Map)
        {
            this.trans2D.X += this.Direction_x*2;
            this.trans2D.Y += this.Direction_y*2;

            if (trans2D.X == Level_Block_X && trans2D.Y == Level_Block_Y)
            {
                this.Grid_x = this.F_Grid_x;
                this.Grid_y = this.F_Grid_y;
                var temp = Static.Functions.Astar(Grid_x, Grid_y, Last_Grid_X, Last_Grid_Y, Map);
                this.F_Grid_x = temp.Item2;
                this.F_Grid_y = temp.Item1;
                Last_Grid_X = temp.Item3;
                Last_Grid_Y = temp.Item4;

            }
            get_direction();

        }

        protected void get_direction()
        {
            if (Grid_x < F_Grid_x)
                Direction_x = 1;
            else if (Grid_x > F_Grid_x)
                Direction_x = -1;
            else
                Direction_x = 0;

            if (Grid_y < F_Grid_y)
                Direction_y = 1;
            else if (Grid_y > F_Grid_y)
                Direction_y = -1;
            else
                Direction_y = 0;

        
        
        }

        
        protected override void Update(TimeSpan gameTime)
        {

            trans2D.X += Direction_x*SPEED;
            trans2D.Y += Direction_y*SPEED;

            if (this.HP <= 0)
            {
                this.Alive = false;
                Console.WriteLine("I'm dead !");
            }
        
        }



    }

    class monster2 : Mob_Base
    {
        private const float monsterSpeed = 1.5f;

        public monster2(int start_grid_x, int start_grid_y, int[,] map, int i)
        {
            HP = 100;
            Alive = true;
            Lives_value = -1;
            Monster_gold = 20;
            Alive = true;

            Last_Grid_X = Last_Grid_Y = -1;

            this.anim2D = null;//new Animation2D();
            this.trans2D = null;

            this.Grid_x = start_grid_x;
            this.Grid_y = start_grid_y;


            var Ast = Static.Functions.Astar(Grid_x, Grid_y, Last_Grid_X, Last_Grid_Y, map);
            this.F_Grid_x = Ast.Item2;
            this.F_Grid_y = Ast.Item1;
            get_direction();


            Mob = new Entity("Enemy2 " + i)
                .AddComponent(trans2D = new Transform2D()
                {
                    X = (start_grid_x * Const.BITMAP_SIZE) + Const.OFFSET,
                    Y = (start_grid_y * Const.BITMAP_SIZE) + Const.OFFSET,

                    Origin = new Vector2(0.5f, 0.5f)
                })
                .AddComponent(new Sprite("Content/Monsters/Monster2/Monster2XML.wpk"))
                .AddComponent(Animation2D.Create<TexturePackerGenericXml>("Content/Monsters/Monster2/Monster2XML.xml")
                .Add("Idle", new SpriteSheetAnimationSequence() { First = 1, Length = 1, FramesPerSecond = 30 })
                    .Add("Running", new SpriteSheetAnimationSequence() { First = 1, Length = 4, FramesPerSecond = 20 }))
                .AddComponent(new AnimatedSpriteRenderer(DefaultLayers.Alpha));

            Mob.FindComponent<Transform2D>().DrawOrder = 0;
            anim2D = Mob.FindComponent<Animation2D>();
            anim2D.CurrentAnimation = "Running";
            anim2D.Play(true);
            
        }
        protected override void Update(TimeSpan gameTime)
        {
            base.Update(gameTime);
        }


    }
}
