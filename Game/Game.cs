using DodgeGame.Entities;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.System;
using System;
using Windows.UI.Popups;

namespace DodgeGame
{
    class Game
    {
        #region Private Members

        private MainPage _mainPage;
        private int _Width = 0;
        private int _Height = 0;

        private Canvas _gameCanvas;
        private Player _player;
        private Enemy[] _enemies;

        private int _enemiesCount;
        private int _maximumEnemiesToWin;
        private double _enemiesMovementInterval;

        private DispatcherTimer _gameTimer;

        #endregion

        public int Width { get { return this._Width; } }
        public int Height { get { return this._Height; } }

        public double CenterX { get { return this._Width / 2; } }
        public double CenterY { get { return this._Height / 2; } }

        public Game(MainPage mainPage, int width, int height, int enemiesCount = 10)
        {
            this._mainPage = mainPage;
            this._Width = width;
            this._Height = height;
            this._enemiesCount = enemiesCount;
            this._maximumEnemiesToWin = 1;
            this._enemiesMovementInterval = 0.01;

            this._Initialize();
        }

        private void _Initialize()
        {
            this._gameCanvas = new Canvas()
            {
                Name = "GameCanvas",
                Background = new SolidColorBrush { Color = Colors.Black },
                Opacity = 0.5,
                Width = this._Width,
                Height = this._Height,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            this._mainPage.MainGrid.Children.Add(this._gameCanvas);

            this._player = new Player(this._gameCanvas, 30, 30, 10);
            this._player.SetPosition(this.CenterX, this.CenterY);

            this.SpawnEnemies();

            // Setting timer
            this._gameTimer = new DispatcherTimer();
            this._gameTimer.Interval = TimeSpan.FromMilliseconds(1);
            this._gameTimer.Tick += Update;
        }
        //creating enemies
        private void SpawnEnemies()
        {
            this._enemies = new Enemy[this._enemiesCount];

            for (int i = 0; i < this._enemies.Length; i++)
            {
                Enemy newEnemy = new Enemy(this._gameCanvas, 30, 30, 2, this._enemiesMovementInterval);
                newEnemy.SetTarget(this._player);
                this.SetRandomPosition(newEnemy);
                this._enemies[i] = newEnemy;
            }
        }
        //making random positions for enemies and preventing coliding on spawn
        private void SetRandomPosition(Enemy enemy)
        {
            Random rand = new Random();
            double randomX = rand.Next(0, this._Width - enemy.Width);
            double randomY = rand.Next(0, this._Height - enemy.Height);
            enemy.SetPosition(randomX, randomY);
            
            for (int i = 0; i < this._enemies.Length; i++)
            {
                if (this._enemies[i] == null) continue;
                if (enemy.IsCollideWith(this._enemies[i]) && enemy.IsCollideWith(this._player))
                {
                    this.SetRandomPosition(enemy);
                }
            } 
        }
        // Making an action when coliding
        private void Update(object sender, object e)
        {
            this._player.Update();
            int aliveEnemies = 0;

            for (int i = 0; i < this._enemies.Length; i++)
            {
                if (!this._enemies[i].IsAlive) continue;

                aliveEnemies++;
                this._enemies[i].Update();

                //removing enemies on colliding
                for (int c = 0; c < this._enemies.Length; c++)
                {
                    if (i != c && this._enemies[i].IsCollideWith(this._enemies[c]) && this._enemies[c].IsAlive)
                    {
                        this._enemies[i].IsAlive = false;
                        this._enemies[i].Kill();
                    }
                }

                //removing player on colliding
                if (this._enemies[i].IsCollideWith(this._player))
                {
                    this.GameOver(false);
                    gameLost();
                    return;
                }
            }
            //when 1 enemy left and the player won:
            if (aliveEnemies <= this._maximumEnemiesToWin)
            {
                this.GameOver(true);
                gameWon();
            }

        }
        //the function will set the player's position and start the game timer
        public void StartNewGame()
        {
           this._gameTimer.Start();
        }

        public void GameOver(bool isPlayerWin)
        {
            this._gameTimer.Stop();
            if (!isPlayerWin) 
            {
                this._player.Kill();
            }
        }

        public static async void gameWon()
        {
            MessageDialog gameOverMessage = new MessageDialog("You Won! Press Enter twice to retry.");
            await gameOverMessage.ShowAsync();
        }
        public static async void gameLost()
        {
            MessageDialog gameOverMessage = new MessageDialog("You Lost! Press Enter twice to retry.");
            await gameOverMessage.ShowAsync();
        }

    }
}
