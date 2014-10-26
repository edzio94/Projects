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
namespace Tower_Defense_WaveProject
{
    class test : Behavior
    {
        public Slider Colors;

        public test()
        {
            CreateUI();
        }
        protected override void Update(TimeSpan gameTime)
        {   
            //if (Colors.Value > (Math.Abs(Colors.Minimum + Colors.Maximum)/2))
              //  RenderManager.BackgroundColor = Colors.Foreground;
            //else
              //  RenderManager.BackgroundColor = Colors.Background;
            RenderManager.BackgroundColor /= Colors.Value;
            RenderManager.BackgroundColor = WaveEngine.Common.Graphics.Color.FromHsv((float)Colors.Value, 0.1f, 0.2f);
            Console.WriteLine(Colors.Value);
        }
        public void CreateUI()
        {
             Colors = new Slider()
            {
                Margin = new Thickness(100, 20, 0, 32),
                VerticalAlignment = VerticalAlignment.Center,
                Width = 500,
                Height = 100,
                Minimum = -360,
                Maximum = 0,
                Foreground = WaveEngine.Common.Graphics.Color.Bisque,
                Background = WaveEngine.Common.Graphics.Color.DarkGreen,


            };
            
        }
    }
    class Ui : Scene
    {

        protected override void CreateScene()
        {
            Entity x = new Entity("ww");            
            test ob = new test();
            x.AddComponent(ob);
            EntityManager.Add(ob.Colors);
            EntityManager.Add(x);
        }

    }
}
