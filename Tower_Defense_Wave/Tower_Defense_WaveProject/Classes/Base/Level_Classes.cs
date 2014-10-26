using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using WaveEngine.Components.Gestures;
using WaveEngine.Framework;
using WaveEngine.Framework.Managers;
using WaveEngine.Framework.Services;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;
using WaveEngine.Common;

using WaveEngine.Components.UI;
using WaveEngine.Framework.UI;

using Map_Classes;
namespace Level_Classes
{
    class LevelManager : Behavior
    {
         
        //public List<Entity> ENTITIES;
        public Entity TowersBar;
        public Entity Tower1Button;
        //private List<Entity> Towers_choice; // BUTTONS WITH TOWERS

        public TextBlock MonsterCounter;
        public TextBlock MoneyCounter;
        public TextBlock TimerCounter;

        public Entity BaseMobEntity;
        public Entity BaseTowerEntity;
        public Entity ScreenLayout;

        // Making timer for waves sets.
        protected Stopwatch WaveClock;
        protected TimeSpan WaveTimerBreak;
        protected Action AccessFunction;
        public delegate void GetAction();

        protected bool MoveMonster;

        public Map_Base Level;
        public Player_Classes.Player_Base Player;
        public List<Mobs.Mob_Base> MonsterList;
        public List<Towers_Classes.Tower_Base> TowersList; 
        // List of towers placed on fields;
        private int _counter;

        private int _lastIndex; // INDEX OF LAST SPAWNED MONSTER
        
        protected void ChangeWaveStatus()
        {
            Console.WriteLine("TIMER PASSED OUT !!!");
            MoveMonster = true;
        }


        public LevelManager()
        {
            MoveMonster = false;
            // Creating Timer for Wave
            GetAction obj = new GetAction(ChangeWaveStatus);
            AccessFunction = new Action(obj);
            WaveTimerBreak = new TimeSpan(0, 0, 0, 30, 0);
            WaveServices.TimerFactory.CreateTimer("Wave_Break", Static.Const.L_Wave_Break_Timer[
                Static.Const.List_Counter], AccessFunction);

            WaveClock = new Stopwatch();
            WaveClock.Start();
            
            _counter = 0;
            BaseTowerEntity = new Entity("Tower_set");                
            BaseMobEntity = new Entity("Monster_Set");
            TowersBar = new Entity("Towers_Bar");
            ScreenLayout = new Entity("Screen_layout");

            
            Player = new Player_Classes.Player_Base(20, 500);         
            
            MonsterList = new List<Mobs.Mob_Base>();
            TowersList = new List<Towers_Classes.Tower_Base>();
            
            Level = new Map_Base("Content/Maps/Test/", 5, 0, 0, 4, 4);

            MonsterList.Add(new Mobs.Mob_Base(Level.starting_x, Level.starting_y, Level.map,BaseMobEntity.NumChildrens));
            _lastIndex= MonsterList.Count -1;
            
            BaseMobEntity.AddChild(MonsterList[0].Mob);
           
            Create_Layout();
            Create_Towers_Bar();
            
        }
        
        private void Create_Layout()
        {
            
            // Creating layout bitmaps(money, lives).
            int x = WaveServices.Platform.ScreenWidth - 32;
            var y = (WaveServices.Platform.ScreenHeight / 2) - (WaveServices.Platform.ScreenHeight / 2) + 32;
            var sprite = "Content/Layout/Lives.wpk";
            
            Entity lives = Static.Functions.Create_entity("Lives",x,y,sprite);
            ScreenLayout.AddChild(lives);

            x = WaveServices.Platform.ScreenWidth - (64 + 75);
            y = (WaveServices.Platform.ScreenHeight / 2) - (WaveServices.Platform.ScreenHeight / 2) + 32;
            sprite = "Content/Layout/Money.wpk";

            Entity money = Static.Functions.Create_entity("Money", x, y, sprite);
            ScreenLayout.AddChild(money);

            // Creating text for money and lives.
            MoneyCounter = new TextBlock()
            {
                Text = Player.Gold.ToString(CultureInfo.InvariantCulture),
                Width = 75,
                Foreground = WaveEngine.Common.Graphics.Color.DarkGreen,
                TextAlignment = TextAlignment.Right,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0,0,180,0)
           
            };
            
            MonsterCounter = new TextBlock()
            {
                
                Text = Player.Lives.ToString(CultureInfo.InvariantCulture),
                Width = 75,
                Foreground = WaveEngine.Common.Graphics.Color.Gainsboro,
                TextAlignment = TextAlignment.Right,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0,0,70,0)
                
            };

            TimerCounter = new TextBlock()
            {
                Text = "0",
                Width = 100,
                Foreground = WaveEngine.Common.Graphics.Color.BlanchedAlmond,
                TextAlignment = TextAlignment.Left,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(120,0,0,0)
               
            };
            

            Create_Timer();
        }
        
        private void Create_Towers_Bar()
        {
           var touchGestures = new TouchGestures();
           var x = WaveServices.Platform.ScreenWidth / 2;
           var y = WaveServices.Platform.ScreenHeight - 150;
           var sprite = "Content/Other/TowerBar/Bar_Background.wpk";
           TowersBar = Static.Functions.Create_entity("Tower_Bar", x, y, sprite);               
           
           x = 0;
           y = 0;
           sprite = "Content/Other/TowerBar/Tower1_Sprite.wpk";

           Tower1Button = Static.Functions.Create_entity("Tower1",x,y,sprite);          
           Tower1Button.AddComponent(new RectangleCollider())   
            .AddComponent(touchGestures);                                    
           Tower1Button.FindComponent<Transform2D>().ParentDependencyObject = null;
           Tower1Button.FindComponent<Transform2D>().DrawOrder = 0;
             
            

            sprite = "Content/Other/TowerBar/Chandler_Icon.wpk";
            var Chandler_tower = Static.Functions.Create_entity("Chandler_tower", Static.Const.Tower_Position["Chandler_tower"].Item1
                ,Static.Const.Tower_Position["Chandler_tower"].Item2,sprite);
            Chandler_tower.FindComponent<Transform2D>().ParentDependencyObject = null;
            Chandler_tower.FindComponent<Transform2D>().DrawOrder = 0;
            Chandler_tower.AddComponent(new RectangleCollider());
            Chandler_tower.AddComponent(new TouchGestures());

            touchGestures.TouchMoved += touchGestures_TouchMoved;
            touchGestures.TouchReleased += touchGestures_TouchReleased;
            
            Chandler_tower.FindComponent<TouchGestures>().TouchMoved += touchGestures_TouchMoved;
            Chandler_tower.FindComponent<TouchGestures>().TouchReleased += touchGestures_TouchReleased;
            
            TowersBar.AddChild(Chandler_tower);
            TowersBar.AddChild(Tower1Button);
                        

        }

        protected void Create_Timer()
        {
            var timerSprite = Static.Functions.Create_entity("TimerSprite", (float)64
                , (float)32, "Content/Layout/ClockSprite.wpk");
            timerSprite.AddComponent(new RectangleCollider());
            timerSprite.AddComponent(new TouchGestures());
            timerSprite.FindComponent<TouchGestures>().TouchPressed += touchGestures_ResetTimer;
            TimerCounter.Text = WaveTimerBreak.ToString();
            ScreenLayout.AddChild(timerSprite);


        }

        protected void touchGestures_ResetTimer(object sender, GestureEventArgs e)
        {
            Console.WriteLine("TEST111");
            AccessFunction();
            WaveServices.TimerFactory.RemoveTimer("Wave_Break");
            WaveServices.TimerFactory.UpdateTimer("Wave_Break",
                Static.Const.L_Wave_Break_Timer[
                ++Static.Const.List_Counter], AccessFunction);

            WaveClock.Restart();

        }

        protected void Get_Timer()
        {
            TimeSpan q = WaveTimerBreak.Duration();

            int w = (Static.Const.L_Wave_Break_Timer
                [Static.Const.List_Counter].Seconds
                - (int)(WaveClock.ElapsedMilliseconds / 1000));
            if (w <= 0)
                TimerCounter.Text = "0";
            else
                TimerCounter.Text = w.ToString();
            
        }
        
        protected override void Update(TimeSpan gameTime)
        {

            Get_Timer();
            
            for (int k = 0; k < TowersList.Count; k++)
            {
                for (int l = 0; l < MonsterList.Count; l++)
                {                   
                    var damage = TowersList[k].Get_Damage(MonsterList[l], BaseMobEntity);
                    if (damage == TowersList[k].Tower_bullet.Bullet_damage || MonsterList[l].Mob.Name == TowersList[k].Tower_target)
                        break;
  
                }
            }


                for (int i = 0; i < MonsterList.Count; i++)
                {

                    var temp = MonsterList[i];
                     _lastIndex = MonsterList.Count -1;
                                        
                    // CHEKING DISTANCE LAST MOB FROM STARTING POINT
                    var distanceX = Math.Abs(MonsterList[_lastIndex].trans2D.X -
                            Level.Map_Blocks[Level.starting_y, Level.starting_x].Block_Position_X);
                    var distanceY = Math.Abs(MonsterList[_lastIndex].trans2D.Y -
                            Level.Map_Blocks[Level.starting_y, Level.starting_x].Block_Poisiton_Y);

                    Add_monster(distanceX, distanceY);
                    
                    if(Check_monster(temp,i))                                  
                        break;

                    if (MoveMonster)
                    temp.change_pos(Level.Map_Blocks[temp.F_Grid_y, temp.F_Grid_x].Block_Position_X,
                        Level.Map_Blocks[temp.F_Grid_y, temp.F_Grid_x].Block_Poisiton_Y, Level.map);

                }           

        }
        private bool Check_monster(Mobs.Mob_Base monster,int i)
        {
            if ((monster.Grid_x == Level.ending_x && monster.Grid_y == Level.ending_y) || (monster.HP <= 0))
            {
                for (int j = 0; j < TowersList.Count; j++ )
                {
                    if (TowersList[j].Tower_target == monster.Mob.Name)
                        TowersList[j].Reset_values();
                }
                    Check_death(monster);
                monster.Mob.Enabled = false;
                Static.Functions.Swap(MonsterList, i, MonsterList.Count - 1);
                return true;
            }
            else
                return false;
        }
        
        private void Check_death(Mobs.Mob_Base monster)
        {
            if (monster.HP > 0)
                Check_lives(monster);
            else
            {
                Player.Add_gold(monster.Monster_gold);
                MoneyCounter.Text = Player.Gold.ToString();
            }
        }
        
        private void Check_lives(Mobs.Mob_Base monster)
        {
            Player.Lives -= monster.Lives_value;
            MonsterCounter.Text = Player.Lives.ToString();
            if (Player.Lives <=0)
            {
                Console.WriteLine("No lives. Ending game");
                WaveServices.Platform.Exit();
            }
        }
        
        private void Add_monster(float distance_x, float distance_y)
        {
            if (((int)distance_x == 64) || ((int)distance_y == 64) 
                && (MonsterList.Count <= 10))
            {
                if (_counter % 2 == 0)
                    MonsterList.Add(new Mobs.Mob_Base(Level.starting_x, Level.starting_y, Level.map, ++_counter));
                else
                    MonsterList.Add(new Mobs.monster2(Level.starting_x, Level.starting_y, Level.map, ++_counter));

                BaseMobEntity.AddChild(MonsterList[MonsterList.Count - 1].Mob);
                
                _lastIndex = MonsterList.Count - 1;

            }
        }

     void touchGestures_TouchMoved(object sender, GestureEventArgs e)
        {
           var transform2D = ((Entity)sender).FindComponent<Transform2D>();  
            //SCALING IMAGE
           transform2D.XScale = transform2D.YScale = 0.5f;

            //GETTING DIFFERENCE BETWEEN SCENE STARTING POINT AND BART STARTING POINT
            // FOR GETTING IMAGE ON MOUSE
            
         var xDifference = TowersBar.FindComponent<Transform2D>().X;           
         var yDifference = TowersBar.FindComponent<Transform2D>().Y;
            
            
         transform2D.X = e.GestureSample.Position.X - xDifference;           
         transform2D.Y = e.GestureSample.Position.Y - yDifference;
            
              
        }
   void  touchGestures_TouchReleased(object sender, GestureEventArgs e)
     {
       var bitmap = ((Entity)sender).FindComponent<Transform2D>();
       var getPosition = Level.Get_Grid_Position((float)bitmap.X, (float)bitmap.Y);
       var costTower = Static.Const.Tower_Costs[((Entity)sender).Name];
       var name = ((Entity)sender).Name;
       //  ITEM1 = y for map_grid 
       //  ITEM2 = x for map_grid
       //  ITEM3 = grid y for map_grid
       //  ITEM4 = grid x for map_grid
       //  Item5 = ID of Block       
       var towerY = getPosition.Item1;
       var towerX =  getPosition.Item2;
       var towerGridX = getPosition.Item4;
       var towerGridY = getPosition.Item3;
       var sprite = Static.Const.Tower_bitmap[name];
       
         if (getPosition.Item5 == "FIELD" && (getPosition.Item1 != -1 && getPosition.Item2 != -1)
             && Player.Gold >= costTower)
         {

             Level.Map_Blocks[towerGridY, towerGridX].ID = "TOWER_FIELD";
             Player.Gold -= costTower;
                        
             // ADD TO TOWER LIST NEW TOWER !!!!!!
            if (name == "Tower1")
             TowersList.Add(new Towers_Classes.Tower_Base(towerX, towerY, BaseTowerEntity.NumChildrens,sprite));
            else if (name == "Chandler_tower")
                TowersList.Add(new Towers_Classes.Tower_Chandler(towerX,towerY, BaseTowerEntity.NumChildrens,sprite));
             BaseTowerEntity.AddChild(TowersList[TowersList.Count - 1].Tower);
             MoneyCounter.Text = Player.Gold.ToString();
             
            
         }
             
          bitmap.XScale = 1;
          bitmap.YScale = 1;
          var tempName = ((Entity)sender).Name;
          var position = Static.Const.Tower_Position[tempName];
          bitmap.Y = position.Item2;
          bitmap.X = position.Item1;                                    
     }
    }

}
