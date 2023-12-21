using System;
using System.Windows.Media;

namespace snakeGame
{
    public static class Audio
    {
        /*public readonly static MediaPlayer GameOver = 
          LoadAudio("game-over.wav", true, .5);

        (true) is for bgm or other looping sounds;    game over is just an example
        (.5) is the volume 
         */

        public readonly static MediaPlayer EatTest =
            LoadAudio("m1_garand_ping.mp3");

        private static MediaPlayer LoadAudio(string filename, 
            double volume=1, bool repeat = false)
        {
            MediaPlayer player = new();
            player.Open(new Uri($"Assets/{filename}", UriKind.Relative));
            player.Volume = volume;

            if (repeat)
            {
                player.MediaEnded += PlayerRepeat_MediaEnded;
            }
            return player;
        }

        private static void PlayerRepeat_MediaEnded(object sender, EventArgs e)
        {
            MediaPlayer m = sender as MediaPlayer;
            m.Stop();
            m.Position = new TimeSpan(0);
            m.Play();
        }
    }
}
