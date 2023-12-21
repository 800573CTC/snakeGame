using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace snakeGame
{
    public static class Images
    {
        public readonly static ImageSource Empty = LoadImage("Empty.png");
        public readonly static ImageSource Body = LoadImage("Body.png");
        public readonly static ImageSource Head = LoadImage("Head.png");

        public readonly static ImageSource Apple = LoadImage("Food.png");

        public readonly static ImageSource Orange = LoadImage("Orange.png");
        public readonly static ImageSource Melon = LoadImage("Melon.png");
        public readonly static ImageSource Slow = LoadImage("Slow.png");
        public readonly static ImageSource Gold = LoadImage("Gold.png");
        public readonly static ImageSource Dupe = LoadImage("Dupe.png");
        public readonly static ImageSource Death = LoadImage("Death.png");
        public readonly static ImageSource Speed = LoadImage("Speed.png");
        public readonly static ImageSource Null = LoadImage("Potion_Template.png");
        public readonly static ImageSource Wall = LoadImage("Wall.png");
        public readonly static ImageSource Starfruit = LoadImage("Starfruit.png");
        public readonly static ImageSource Illusion = LoadImage("Wall.png");


        public readonly static ImageSource DeadBody = LoadImage("Deadbody.png");
        public readonly static ImageSource DeadHead = LoadImage("DeadHead.png");

        private static ImageSource LoadImage(string filename)
        {
            return new BitmapImage(new Uri($"Assets/{filename}", UriKind.Relative));
        }
    }
}
