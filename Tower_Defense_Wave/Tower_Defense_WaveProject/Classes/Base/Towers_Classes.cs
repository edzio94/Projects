using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Sound;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework.Diagnostic;
namespace Towers_Classes
{
     class Bullet_Base
    {
         public const string ID = "Bullet";
         
         public int Bullet_dir_x, Bullet_dir_y;
         public int Bullet_damage;
         public float Bullet_speed_x, Bullet_speed_y;
         public Entity Bullet;
         public Transform2D Bullet_bitmap;         

         public bool Bullet_access;
         public float Bullet_speed;
         public Bullet_Base(int damage, float speed,string tower_name ,string sprite)
         {
             Bullet_damage = damage;
             Bullet_speed = speed;
             Bullet_access = true;
             Bullet =  Static.Functions.Create_entity("Ammo[" + tower_name + "]", 0, 0, sprite,ref Bullet_bitmap);
             Bullet.Enabled = false;

         }
         public void Get_var_value()
         {
             Console.WriteLine("GET VALUE !11");
             Bullet_access = true;

         }
         public void Move_bullet(float Parent_x, float Parent_y)
         {
            Bullet_bitmap.X += ((float)(Bullet_dir_x * Bullet_speed_x)) - Parent_x;
           Bullet_bitmap.Y += ((float)(Bullet_dir_y * Bullet_speed_y)) - Parent_y;
         }

    };
    class Tower_Base
    {
             
        private const float Tower_range = (float)(30 * Math.PI);
        protected TimeSpan Tower_delay;
        public const string ID = "Tower";               

        public const float Bar_X = 0;
        public const int Bar_Y = 0;
                                   
        public int Tower_cost;


        public string Tower_target;
        private string Tower_sprite;
        public Transform2D Tower_bitmap;
        public Bullet_Base Tower_bullet;
        public Entity Tower;
        protected Action Get_action;
        public delegate void get_action();
        public Tower_Base(int x, int y, int counter, string sprite)
        {
                       
            Tower_delay = new TimeSpan(0, 0, 0, 1, 2);            
            Tower_cost = 75;          
            Tower_target = "";
            var X = x;
            var Y = y;
            Tower_sprite = sprite;
            
            Tower = Static.Functions.Create_entity("Tower1["+counter+"]", x, y,Tower_sprite ,ref Tower_bitmap);
            Tower_bitmap.DrawOrder = 0;

            Tower_bullet = new Bullet_Base(20, 4.0f, Tower.Name, "Content/Towers/Tower_Beta/Ammo1.wpk");
            get_action access = new get_action(Tower_bullet.Get_var_value);
            Get_action = new Action(access);
            WaveServices.TimerFactory.CreateTimer("Shooting_break["+Tower.Name+"]", Tower_delay, Get_action);

            
            Tower.AddChild(Tower_bullet.Bullet);           

        }


        protected void Get_ammo_speed(int dif_x, int dif_y)        
        {
            float step_x = Math.Abs(dif_x / (Tower_bullet.Bullet_speed - 1.4f));
            float step_y = Math.Abs(dif_y / (Tower_bullet.Bullet_speed - 1.4f));
            if (step_y != 0 && step_x != 0)
            {

                if (step_x >= step_y)
                {
                    Tower_bullet.Bullet_speed_y /= (step_x / step_y);
                }
                else
                    Tower_bullet.Bullet_speed_x /= (step_y / step_x);
            }
        }


        protected bool Check_Range(int Pos_x, int Pos_y)
        {
            var X = Tower.FindComponent<Transform2D>().X;
            var Y = Tower.FindComponent<Transform2D>().Y;
            
            if ((Pos_x >= X - Tower_range) && (Pos_x <= X + Tower_range)
                && (Pos_y >= Y - Tower_range) && (Pos_y <= Y + Tower_range))
                return true;
            else
            return false;
        }


        protected bool Get_hit(int monster_pos_x, int monster_pos_y,
            int ammo_pos_x, int ammo_pos_y)
        {
            if (Math.Abs(ammo_pos_x - monster_pos_x) < 4 && Math.Abs(ammo_pos_y - monster_pos_y) < 4)
                return true;
            else
                return false;
        }
        

        protected virtual void Get_values(string Monster_name)
        {
            Tower_bullet.Bullet.Enabled = true;
            Tower_target = Monster_name;
            
        }

        protected void Get_directions(int Pos_x, int Pos_y, Entity Monsters_set)
        {
            if ((Tower_bullet.Bullet.Enabled) && (Tower_bullet.Bullet_bitmap.X != Pos_x || Tower_bullet.Bullet_bitmap.Y != Pos_y) && (Tower_target != ""))
            {

                var Monster_targeted = Monsters_set.FindChild(Tower_target);

                int dif_x = (int)Tower_bullet.Bullet_bitmap.X - (int)Monster_targeted.FindComponent<Transform2D>().X;
                int dif_y = (int)Tower_bullet.Bullet_bitmap.Y - (int)Monster_targeted.FindComponent<Transform2D>().Y;

                Get_ammo_speed(dif_x, dif_y);

                Static.Functions.get_Directions((int)dif_x, (int)dif_y, ref Tower_bullet.Bullet_dir_x, ref Tower_bullet.Bullet_dir_y);
                Tower_bullet.Move_bullet(Tower_bitmap.X, Tower_bitmap.Y);
            }
        }
        public void Reset_values()
        {
            
            Tower_bullet.Bullet_access = false;
            Tower_bullet.Bullet_bitmap.X = 0;
            Tower_bullet.Bullet_bitmap.Y = 0;
            Tower_bullet.Bullet.Enabled = false;
            Tower_target = "";
        }

        protected void Move_bullet()
        {
           Tower_bullet.Bullet_bitmap.X += ((float)(Tower_bullet.Bullet_dir_x * Tower_bullet.Bullet_speed_x)) - Tower_bitmap.X;
           Tower_bullet.Bullet_bitmap.Y += ((float)(Tower_bullet.Bullet_dir_y * Tower_bullet.Bullet_speed_y)) - Tower_bitmap.Y;
        }

    public int Get_Damage(Mobs.Mob_Base Monster, Entity Monsters_set)
    {

        if (Tower_bullet.Bullet_access)
        {
            Tower_bullet.Bullet_speed_x = Tower_bullet.Bullet_speed_y = Tower_bullet.Bullet_speed;
            if (Tower_target == "" || Monster.Mob.Name == Tower_target)
            {
                int Pos_x = (int)Monster.Mob.FindComponent<Transform2D>().X;
                int Pos_y = (int)Monster.Mob.FindComponent<Transform2D>().Y;

                if ((Check_Range(Pos_x, Pos_y)) && 
                    (!Tower_bullet.Bullet.Enabled)
                    && (Tower_target == ""))
                    Get_values(Monster.Mob.Name);

                Get_directions(Pos_x, Pos_y, Monsters_set);

                if (Tower_target != "")
                {
                    if (((Get_hit(Pos_x, Pos_y, 
                        (int)Tower_bullet.Bullet_bitmap.X,
                        (int)Tower_bullet.Bullet_bitmap.Y)) &&
                        Tower_bullet.Bullet.Enabled)
                        || (Monsters_set.FindChild(Tower_target).Enabled == false))
                    {
                        if (Monster.Mob.Name == Tower_target)
                            Monster.HP -= Tower_bullet.Bullet_damage;
                        Reset_values();
                        return Tower_bullet.Bullet_damage;
                    }
                    else
                        return 0;
                }

            }
        }
        return 0;
    }
    }

   class Tower_Chandler : Tower_Base
    {
       // Protected override get_values and make sound there;
       private SoundInfo sample;
       private SoundBank SoundBank;
       public Tower_Chandler(int x, int y, int counter, string sprite) : base(x,y,counter,sprite)
       {
           Console.WriteLine("GOT CHANDLER TOWER !");
           sample = new SoundInfo("Content/Towers/Tower_Chandler/whip_chandler.wpk");
           SoundBank = new SoundBank();
           SoundBank.Add(sample);
           WaveServices.SoundPlayer.RegisterSoundBank(SoundBank);
           
       }
    
    protected override void Get_values(string Monster_name)
       {
          
           Tower_bullet.Bullet.Enabled = true;
           Tower_target = Monster_name;
           WaveServices.SoundPlayer.Play(sample);
        
       }
   };


}
