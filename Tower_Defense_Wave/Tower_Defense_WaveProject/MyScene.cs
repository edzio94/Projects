#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Animation;
using WaveEngine.Common.Math;
using WaveEngine.Components.Gestures;
using WaveEngine.Components.UI;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Managers;
using WaveEngine.Framework.Sound;
using WaveEngine.Framework.UI;
#endregion

namespace Tower_Defense_WaveProject
{
    public class MyScene : Scene
    {        
        protected override void CreateScene()
        {

            //WaveServices.ScreenLayers.AddScene<GameScene>()
                        //   .Apply("FromMultiplayer");
          
            
            var Map_Bitmaps = new Entity("Whole_Map_global");
            
            RenderManager.BackgroundColor = Color.CornflowerBlue;
            
            Level_Classes.LevelManager Level1 = new Level_Classes.LevelManager();
         
                     var Mob_set = new Entity("juest_Player");
                   
                    
                     Map_Bitmaps.AddComponent(Level1);
                     Map_Bitmaps.AddComponent(Level1.Level);                      
                    EntityManager.Add(Level1.BaseMobEntity);
                    EntityManager.Add(Level1.BaseTowerEntity);
                     EntityManager.Add(Mob_set);
                     EntityManager.Add(Level1.Level.Map_Bitmaps);
                     EntityManager.Add(Map_Bitmaps);
                     EntityManager.Add(Level1.TowersBar);
                     EntityManager.Add(Level1.MonsterCounter);
                     EntityManager.Add(Level1.MoneyCounter);
                     EntityManager.Add(Level1.TimerCounter);
                     EntityManager.Add(Level1.ScreenLayout);

                     
                     
            

                     
        }
    }
}
