using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace snakeGame
{
    public class GameState
    {
        public int Rows { get; }
        public int Cols { get; }
        public GridValue[,] Grid {  get; }
        public Direction Dir { get; private set; }
        public int Score { get; private set; }
        public bool GameOver { get; private set; }

        private readonly LinkedList<Direction> dirChanges = new LinkedList<Direction>();
        private readonly LinkedList<Position> snakePositions = new LinkedList<Position>();
        private readonly Random random = new Random();

        public bool speedLimit = false;

        public GameState(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Grid = new GridValue[Rows, Cols];
            Dir = Direction.Right;

            AddSnake();
            AddFood();
            AddFood();
            AddWalsl();
            //AddApple();
            //AddOrange();
        }

        private void AddSnake()
        {
            int r = Rows / 2;
            
            for (int c = 1; c <= 3; c++) 
            {
                Grid[r, c] = GridValue.Snake;
                snakePositions.AddFirst(new Position(r, c));
            }
        }

        private IEnumerable<Position> EmptyPositions()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0;  c < Cols; c++)
                {
                    if (Grid[r, c] == GridValue.Empty)
                    {
                        yield return new Position(r, c);
                    }
                }
            }
        }

        public int only = 1;
        public int onlyA = 18;
        //random food generator
        private void AddFood()
        {
            Random random = new Random();
            int rInt = random.Next(only, onlyA);
            if (rInt == 1 || rInt == 2 || rInt == 3 || rInt == 4)       //there's probably [definitely] an easier way but idk
            {
                AddApple();
            }
            else if (rInt == 5 || rInt == 6)
            {
                AddMelon();
            }
            else if (rInt == 7 || rInt == 8 || rInt == 9)
            {
                AddOrange();
            }
            else if (rInt == 10)
            {
                AddStarfruit();
            }
            else if (rInt == 11 || rInt == 12)
            {
                AddSlow();
            }
            else if (rInt == 13)
            {
                AddGold();
            }

            else if (rInt == 14)
            {
                AddDupe();
            }
            else if (rInt == 15 || rInt == 16)
            {
                AddSpeed();
            }
            else if (rInt == 17)
            {
                AddDeath();
                AddNull();
            }
            else if (rInt == -1)
            {
                AddClassic();
            }
            else if (rInt == -2)
            {
                AddWalsl();
                AddIllusion();
            }
            else if (rInt == -3)
            {
                AddIllusion();
            }
        }


        private void AddClassic()
        {
            List<Position> empty = new List<Position>(EmptyPositions());

            if (empty.Count == 0)
                return;

            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Col] = GridValue.Classic;
        }
        private void AddStarfruit()
        {
            List<Position> empty = new List<Position>(EmptyPositions());

            if (empty.Count == 0)
                return;

            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Col] = GridValue.Starfruit;
        }
        //adds an apple
        private void AddApple()
        {
            List<Position> empty = new List<Position>(EmptyPositions());

            if (empty.Count == 0) 
                return;

            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Col] = GridValue.Apple;
        }

        private void AddMelon()
        {
            List<Position> empty = new List<Position>(EmptyPositions());

            if (empty.Count == 0)
                return;

            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Col] = GridValue.Melon;
        }

        // adds a Orangeous apple                       I auto replaced poison with Orange so instead of poisonous it's Orangeous; I refer to the 'poison' as Death but poison is easier to understand
        private void AddOrange()
            {
            List<Position> empty = new List<Position>(EmptyPositions());

            if (empty.Count == 0)
                return;

            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Col] = GridValue.Orange;
        }

        private void AddNull()
        {
            List<Position> empty = new List<Position>(EmptyPositions());

            if (empty.Count == 0)
                return;

            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Col] = GridValue.Null;
        }
        private void AddSlow()
        {
            List<Position> empty = new List<Position>(EmptyPositions());

            if (empty.Count == 0)
                return;

            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Col] = GridValue.Slow;
        }
        private void AddGold()
        {
            List<Position> empty = new List<Position>(EmptyPositions());

            if (empty.Count == 0)
                return;

            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Col] = GridValue.Gold;
        }
        private void AddDupe()
        {
            List<Position> empty = new List<Position>(EmptyPositions());

            if (empty.Count == 0)
                return;

            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Col] = GridValue.Dupe;
        }
        private void AddDeath()
        {
            List<Position> empty = new List<Position>(EmptyPositions());

            if (empty.Count == 0)
                return;

            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Col] = GridValue.Death;
        }
        private void AddSpeed()
        {
            List<Position> empty = new List<Position>(EmptyPositions());

            if (empty.Count == 0)
                return;

            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Col] = GridValue.Speed;
        }

        private void AddWalsl()     //a typo that I left in     :)
        {
            List<Position> empty = new List<Position>(EmptyPositions());

            if (empty.Count == 0)
                return;

            int numberOfWalls =(int)(Rows * Cols * GameSettings.WallDensity);
           
            for (int c = 0; c < numberOfWalls; c++)
            {
                if (empty.Count == 0)
                    return;

                int posNumber = random.Next(0, empty.Count);

                Position pos = empty[posNumber];
                Grid[pos.Row, pos.Col] = GridValue.Wall;
                empty.RemoveAt(posNumber);
            }

           /* for (int c = 0; c < (Rows * Cols * GameSettings.WallDensity); c++)
            {

            } */

           // Position pos = empty[random.Next(empty.Count)];
            //Grid[pos.Row, pos.Col] = GridValue.Wall;
        }
        private void AddIllusion()     
        {
            List<Position> empty = new List<Position>(EmptyPositions());

            if (empty.Count == 0)
                return;

            int numberOfWalls = (int)(Rows * Cols * GameSettings.WallDensity);

            for (int c = 0; c < numberOfWalls; c++)
            {
                if (empty.Count == 0)
                    return;

                int posNumber = random.Next(0, empty.Count);

                Position pos = empty[posNumber];
                Grid[pos.Row, pos.Col] = GridValue.Wall;
                empty.RemoveAt(posNumber);
            }
        }
        public Position HeadPosition()
        {
            return snakePositions.First.Value;
        }

        public Position TailPosition()
        {
            return snakePositions.Last.Value;
        }

        public IEnumerable<Position> SnakePositions()
        {
            return snakePositions;
        }

        //makes new head
        private void AddHead(Position pos)
        {
            snakePositions.AddFirst(pos);
            Grid[pos.Row, pos.Col] = GridValue.Snake;
        }

        //removes the tail
        //with AddHead it simulates movement
        private void RemoveTail()
        {
            Position tail = snakePositions.Last.Value;
            Grid[tail.Row, tail.Col] = GridValue.Empty;
            snakePositions.RemoveLast();
        }


        private Direction GetLastDirection()
        {
            if (dirChanges.Count == 0)
            {
                return Dir;
            }

            return dirChanges.Last.Value;
        }

        //movement queue
        private bool CanChangeDirection(Direction newDir)
        {
            if (dirChanges.Count == 4)
            {
                return false;
            }

            Direction lastDir = GetLastDirection();
            return newDir != lastDir
                && newDir != lastDir.Opposite();
        }
        public void ChangeDirection(Direction dir)
        {
            if (CanChangeDirection(dir))
            {
                dirChanges.AddLast(dir);
            }
        }

        private bool OutsideGrid(Position pos)
        {
            return pos.Row < 0 || pos.Row >= Rows || pos.Col < 0 || pos.Col >= Cols;
        }

        private GridValue WillHit(Position newHeadPos)
        {
            if (OutsideGrid(newHeadPos))
            {
                return GridValue.Outside;
            }

            if (newHeadPos == TailPosition())
            {
                return GridValue.Empty;
            }

            return Grid[newHeadPos.Row, newHeadPos.Col];
        }

        //movement and collisions 
        public void Move()
        {
            if (dirChanges.Count > 0)
            {
                Dir = dirChanges.First.Value;
                dirChanges.RemoveFirst();
            }

            Position newHeadPos = HeadPosition().Translate(Dir);
            GridValue hit = WillHit(newHeadPos);

            if( hit == GridValue.Outside || hit == GridValue.Snake ||
                (hit == GridValue.Wall && GameSettings.WallFatality))
            {
                GameOver = true;
            }
            else if (hit == GridValue.Empty)
            {
                RemoveTail();
                AddHead(newHeadPos);
            }
            else if (hit == GridValue.Apple)
            {
                AddHead(newHeadPos);
                Audio.EatTest.Play();
                Score = Score + 100 * dbpts;
                //changed score from 1 to 100

                //AddApple();
                AddFood();
                if (speedLimit == false)
                speed = speed - 5;
            }
            else if (hit == GridValue.Orange)
            {
                RemoveTail();
                AddHead(newHeadPos);
                Audio.EatTest.Play();

                Score = Score - 75 * dbpts;
                //changed score from -1 to -75

                // AddOrange();
                AddFood();

                if (speedLimit == false)
                    speed = speed + 5;
               

              
            }
            else if (hit == GridValue.Melon)
            {

                AddHead(newHeadPos);
                Audio.EatTest.Play();

                Score = Score + 150 * dbpts;
                //changed score from 2 to 150
                AddFood();
                if (speedLimit == false)
                speed = speed - 7;
            }
            else if (hit == GridValue.Starfruit)
            {

                AddHead(newHeadPos);
                Audio.EatTest.Play();

                Score = Score + 300 * dbpts;
                //changed score from 2 to 150
                AddFood();
                    
            }
            else if (hit == GridValue.Slow)
            {
                RemoveTail();
                AddHead(newHeadPos);
                AddFood();
                if (speedLimit == false)
                    speed = speed + 10;
            }
            else if (hit == GridValue.Gold)
            {
                RemoveTail();
                AddHead(newHeadPos);
                AddFood();
                if (dbpts == 1)
                {
                    dbpts = 2 * dbpts;
                }
                else
                {
                    Score = Score + 400;
                    //changed score from 4 to 400
                }
            }
            else if (hit == GridValue.Dupe)
            {
                RemoveTail();
                AddHead(newHeadPos);
                AddFood();
                AddFood();
                Score = Score / 2;
                //this is so evil       too many times have I gone "wow a dupe I can get a higher score only
                //                                                  by cutting my current score in half"
                //                                                      and then I die with less score than before :(
                     
            }
            else if (hit == GridValue.Speed)
            {
                RemoveTail();
                AddHead(newHeadPos);
                AddFood();
                if (speedLimit == false)
                    speed = speed - 10;
            }
            else if (hit == GridValue.Death)
            {
                AddHead(newHeadPos);
                if (GameSettings.HardDeath == true)
                {
                    speed = speed - 20000;
                    //crashes the game
                }
                else
                {
                    GameOver = true;
                }
            }
            else if (hit == GridValue.Classic)
            {
                AddHead(newHeadPos);
                AddFood();
            }
            else if (hit == GridValue.Null)
            {
                RemoveTail();
                AddHead(newHeadPos);
                AddFood();
                Score = Score + 50 * dbpts;
                //changed score from 1 to 50
            }
            else if (hit == GridValue.Illusion)
            {
                RemoveTail();
                AddHead(newHeadPos);
                AddFood();
                Score = Score + 50 * dbpts;
            }

        }
        public int dbpts = 1;
        public int speed = 130;
    }
}






/*
 
              _____
             /     \
            /       \
            |  ___  |
            |  [_]  |               shack
            |_______|                in
            |       |\               da
            |       | \             code
 
 */