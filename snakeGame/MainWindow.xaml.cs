using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace snakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //image instructions
        private readonly Dictionary<GridValue, ImageSource>
            gridValToImage = new()
            {
                {GridValue.Empty, Images.Empty },
                {GridValue.Snake, Images.Body },
                {GridValue.Apple, Images.Apple },

                {GridValue.Orange, Images.Orange },
                {GridValue.Melon, Images.Melon },
                {GridValue.Slow, Images.Slow },
                {GridValue.Gold, Images.Gold },
                {GridValue.Dupe, Images.Dupe },
                {GridValue.Death, Images.Death },
                {GridValue.Speed, Images.Speed },
                {GridValue.Null, Images.Null },
                {GridValue.Classic, Images.Apple },
                {GridValue.Wall, Images.Wall },
                {GridValue.Starfruit, Images.Starfruit },
                {GridValue.Illusion, Images.Illusion },


            };

        //head rotation cinstructions
        private readonly Dictionary<Direction, int> dirToRotation = new()
        {
            {Direction.Up, 0},
            {Direction.Down, 180},
            {Direction.Left, 270},
            {Direction.Right, 90}
        };

        //bunch of variables
        private int rows = 25;
        private int cols = 25;
        //size of grid  ^       15,20,25
        private readonly Image[,] gridImages;
        private GameState gameState;
        private bool gameRunning;
        private bool gamePaused;
        private double wacky = 0.5;
        private int boostSpeed = 50;
       // public bool speedLimit = false;

        //default highscore
       private int highScore = 0;
        private Random random = new Random();

       // public int speed = 130;
        //speed of snake
        //private int multiplier = 0;

        //Constructor   v
        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetupGrid();
            gameState = new GameState(rows, cols);


            string fileName = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "highscore.txt");
            if (File.Exists(fileName))
            {
                StreamReader sr = new StreamReader(fileName);
                highScore = int.Parse(sr.ReadLine());
                sr.Close();
                HighScoreText.Text = $"High Score: {highScore}";
            }
            else
            {
                StreamWriter sw = new StreamWriter(fileName);
                sw.WriteLine(highScore);
                sw.Close();
            }
            //  ^   persistent high-score
        }



        private async Task GameLoop()
        {
            
            while (!gameState.GameOver)
            {
                // multiplier = gameState.Score / 5;
                // speed = speed - multiplier; 
               //int speedInt = int.Parse(gameState.speed);
                await Task.Delay(gameState.speed);
                //messing around with delay default=100     this is speed of the snake
                gameState.Move();
                Draw();
            }
        }

        //creates grid 
        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[rows, cols];
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;
            GameGrid.Width = GameGrid.Height * (cols / (double)rows);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Image image = new Image
                    {
                        Source = Images.Empty,
                        RenderTransformOrigin = new Point(wacky, wacky)
                        //  ^ without this wacky stuff happens
                    };

                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                }
            }

            return images;
        }

        private async Task RunGame()
        {
            Draw();
            await ShowCountDown();
            Overlay.Visibility = Visibility.Hidden;
            await GameLoop();
            await HighScoreUpdate();
            await ShowGameOver();
            gameState = new GameState(rows, cols);
        }

        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Overlay.Visibility == Visibility.Visible)
            {
                e.Handled = true;
            }

            if (!gameRunning)
            {
                gameRunning = true;
                await RunGame();
                gameRunning = false;
            }
        }

        //controls
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    gameState.ChangeDirection(Direction.Left);
                    break;
                case Key.Right:
                    gameState.ChangeDirection(Direction.Right);
                    break;
                case Key.Up:
                    gameState.ChangeDirection(Direction.Up);
                    break;
                case Key.Down:
                    gameState.ChangeDirection(Direction.Down);
                    break;
                case Key.A:
                    gameState.ChangeDirection(Direction.Left);
                    break;
                case Key.D:
                    gameState.ChangeDirection(Direction.Right);
                    break;
                case Key.W:
                    gameState.ChangeDirection(Direction.Up);
                    break;
                case Key.S:
                    gameState.ChangeDirection(Direction.Down);
                    break;
                case Key.D9:    //9 or nine    to set mode to potions only
                    gameState.only = 10;
                    gameState.onlyA = 18;
                    gameState.speedLimit = false;
                    GameSettings.HardDeath = false;

                    GameModeText.Text = "Potions Only";
                    break;
                case Key.D0: //0 or zero    to set mode to all food types
                    gameState.only = 1;
                    gameState.onlyA = 18;
                    gameState.speedLimit = false;
                    GameSettings.HardDeath = false;

                    GameModeText.Text = "Normal";
                    break;
                case Key.D8: //8 or eight    to set mode to food only
                    gameState.onlyA = 11;
                    gameState.only = 1;
                    gameState.speedLimit = false;
                    GameSettings.HardDeath = false;

                    GameModeText.Text = "Food Only";
                    break;
                case Key.D7: //7 or seven    to set mode to apples only
                    gameState.onlyA = 0;
                    gameState.only = -1;
                    gameState.speedLimit = true;
                    GameSettings.HardDeath = false;

                    GameModeText.Text = "Classic";
                    
                    break;
                case Key.D6: //6 or six    to set mode to dupe only
                    gameState.onlyA = 15;
                    gameState.only = 14;
                    gameState.speedLimit = false;
                    GameSettings.HardDeath = false;

                    GameModeText.Text = "Dupe Only";
                    break;
                case Key.D5: //5 or five    to set mode to have food not affect speed
                    if (gameState.speedLimit == false)
                    gameState.speedLimit = true;
                    else
                    gameState.speedLimit = false;
                    GameSettings.HardDeath = false;

                    GameModeText.Text = "Cruise ctrl";
                    break;
                case Key.D4: //4 or four    to set mode to no death potions
                    gameState.onlyA = 17;
                    gameState.only = 1;
                    gameState.speedLimit = false;
                    GameSettings.HardDeath = false;

                    GameModeText.Text = "No Poison";
                    break;
                case Key.D3: //3 or three    to set mode to walls
                    gameState.onlyA = -2;
                    gameState.only = -4;
                    gameState.speedLimit = false;
                    GameSettings.HardDeath = false;
                    GameSettings.WallDensity = 0.005;

                    GameModeText.Text = "Maze";
                    break;
                case Key.D2: //2 or two     to set mode to hard death aka poison crashes game
                        GameSettings.HardDeath = true;
                    GameModeText.Text = "Hardlock";
                    break;



                case Key.RightCtrl: //wall fatality toggle
                    if (GameSettings.WallFatality == true)
                    {
                        GameSettings.WallFatality = false;
                        WallBehaviorText.Text = "Non-Fatal";

                    }
                    else
                    {
                        GameSettings.WallFatality = true;
                        WallBehaviorText.Text = "Fatal";
                    } 
                    break;
                case Key.E: //wall fatality toggle
                    if (GameSettings.WallFatality == true)
                    {
                        GameSettings.WallFatality = false;
                        WallBehaviorText.Text = "Non-Fatal";

                    }
                    else
                    {
                        GameSettings.WallFatality = true;
                        WallBehaviorText.Text = "Fatal";
                    }
                    break;

                case Key.RightShift:
                    // boostSpeed = (boostSpeed == 0) ? 50 : 0;
                    if (gameState.speedLimit == false)
                    {
                        if (boostSpeed == -50)
                        {
                            boostSpeed = 50;
                            gameState.speed = gameState.speed + boostSpeed;
                        }
                        else
                        {
                            boostSpeed = -50;
                            gameState.speed = gameState.speed + boostSpeed;
                        }
                    }
                    break;
                case Key.LeftShift:
                    // boostSpeed = (boostSpeed == 0) ? 50 : 0;
                    if (gameState.speedLimit == false)
                    {
                        if (boostSpeed == -50)
                        {
                            boostSpeed = 50;
                            gameState.speed = gameState.speed + boostSpeed;
                        }
                        else
                        {
                            boostSpeed = -50;
                            gameState.speed = gameState.speed + boostSpeed;
                        }
                    }
                    break;
                
                    /*case Key.D1: //1 or one    to set mode to wacky aka it's not a bug it's a feature
                        wacky = 0;
                        GameModeText.Text = "Not a bug";
                        SetupGrid();
                        break; */
            }
        }

        //writes the score
        private void Draw()
        {
            DrawGrid();
            DrawSnakeHead();
            ScoreText.Text = $"Score {gameState.Score}";
            SpeedText.Text = $"Speed {gameState.speed}";

            //gameState.Score = gameState.Score + 1;
        }




        private void DrawGrid()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    GridValue gridVal = gameState.Grid[r, c];
                    gridImages[r, c].Source = gridValToImage[gridVal];
                    gridImages[r, c].RenderTransform = Transform.Identity;
                    //  ^ wackiness remover
                }
            }
        }

        //applies rotation to head
        private void DrawSnakeHead()
        {
            Position headPos = gameState.HeadPosition();
            Image image = gridImages[headPos.Row, headPos.Col];
            image.Source = Images.Head;

            int rotation = dirToRotation[gameState.Dir];
            image.RenderTransform = new RotateTransform(rotation);
        }

        private async Task DrawDeadSnake()
        {
        List<Position> positions = new List<Position>(gameState.SnakePositions());

            for (int i = 0; i < positions.Count; i++)
            {
                Position pos = positions[i];
                ImageSource source = (i == 0) ? Images.DeadHead : Images.DeadBody;
                gridImages[pos.Row, pos.Col].Source = source;
               /* switch (i) 
                {
                    case i < 10:
                }*/

                // delay of 50 - 1 per segment      but if 1 is greater than it will choose 1
                // instead of becoming negative
                await Task.Delay(Math.Max(50 - (i * 3),1));
                //  ^ time between each section     default=50
            }
        }

        //starts count down
        private async Task ShowCountDown()
        {
            for (int i = 3; i >= 1; i--)
            {
                OverlayText.Text = i.ToString();
                await Task.Delay(500);
            }
        }

        private async Task HighScoreUpdate()
        {
            if (gameState.Score > highScore)
            {
                highScore = gameState.Score;
                StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "//highscore.txt");
                sw.WriteLine(highScore);
                sw.Close();
            }
            HighScoreText.Text = $"High Score: {highScore}";

        }

        //gameOver
        private async Task ShowGameOver()
        {
            //Audio.GameOver.Play();

            //multiplier = 0;
            gameState.speed = 130;
            GameSettings.WallDensity = 0.01;

            ShakeWindow(2000);      //ignore warning if await it screws things up
            GameModeText.Text = "Normal";
            await DrawDeadSnake();
            await Task.Delay(1000);
            //  ^ time till restart after gameover
            OverlayText.Text = "PRESS ANY KEY TO START";
            Overlay.Visibility = Visibility.Visible;
        }

        private async Task ShakeWindow(int durationMs)
        {
            var oLeft = this.Left;
            var oTop = this.Top;

            var shakeTimer = new DispatcherTimer(DispatcherPriority.Send);
            //send is highest priority
            shakeTimer.Tick += (sender, args) =>
            {
                this.Left = oLeft + random.Next(-10, 11);
                this.Top = oTop + random.Next(-10, 11);
            };

            shakeTimer.Interval = TimeSpan.FromMilliseconds(200);
            shakeTimer.Start();

            await Task.Delay(durationMs);
            shakeTimer.Stop();
        }


        /*private Task Timer()
        {
            
        }*/
    }
}
