

using WaveEngine.Common.Graphics;
using WaveEngine.Framework.UI;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Components.Gestures;

namespace Layout
{
    class HUD
    {
        public TextBlock LivesCounter;
        public TextBlock MoneyCounter;
        public TextBlock TimerCounter;

        public Entity TowersBar;
        public Entity Tower1Sprite;
        public Entity Tower2Sprite;
        public Entity ScreenLayout;
        public Entity Lives;
        public HUD(Player_Classes.Player_Base player)
        {
            ScreenLayout = new Entity("Screen_layout");
            TowersBar = new Entity("Towers_Bar");

            // Creating Icons (Lives, Money) and TextBlocks

            Entity lives = Static.Functions.Create_entity(
                "Lives", WaveServices.Platform.ScreenWidth - 32,
                (WaveServices.Platform.ScreenHeight / 2 - (WaveServices.Platform.ScreenHeight / 2) + 32),
                "Content/Layout/Lives.wpk");
            
            Entity money = Static.Functions.Create_entity(
                "Money", WaveServices.Platform.ScreenWidth - (64 + 75),
                (WaveServices.Platform.ScreenHeight / 2) - (WaveServices.Platform.ScreenHeight / 2) + 32,
                "Content/Layout/Money.wpk");

            MoneyCounter = Static.Functions.CreateTextBlock(player.Gold.ToString(),
                75, Color.DarkGreen, TextAlignment.Right, HorizontalAlignment.Right,
                new Thickness(0, 0, 180, 0));


            MoneyCounter = new TextBlock()
            {
                Text = player.Gold.ToString(),
                Width = 75,
                Foreground = WaveEngine.Common.Graphics.Color.DarkGreen,
                TextAlignment = TextAlignment.Right,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, 180, 0)

            };
            LivesCounter = Static.Functions.CreateTextBlock(player.Lives.ToString(),
                75, Color.Gainsboro, TextAlignment.Right, HorizontalAlignment.Right,
                new Thickness(0, 0, 70, 0));
            
            LivesCounter = new TextBlock()
            {

                Text = player.Lives.ToString(),
                Width = 75,
                Foreground = WaveEngine.Common.Graphics.Color.Gainsboro,
                TextAlignment = TextAlignment.Right,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, 70, 0)

            };

            TimerCounter = new TextBlock()
            {
                Text = "0",
                Width = 100,
                Foreground = WaveEngine.Common.Graphics.Color.BlanchedAlmond,
                TextAlignment = TextAlignment.Left,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(120, 0, 0, 0)

            };

            Tower1Sprite = Static.Functions.Create_entity(
                "Tower1", 0, 0, "Content/Other/TowerBar/Tower1_Sprite.wpk");
            Tower1Sprite.AddComponent(new RectangleCollider())
                .AddComponent(new TouchGestures());
            //Tower1_Sprite.FindComponent<Transform2D>().ParentDependencyObject = null;
           // Tower1_Sprite.FindComponent<Transform2D>().DrawOrder = 0;


        }

    }
}
