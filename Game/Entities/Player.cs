using System;
using System.Threading;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace DodgeGame.Entities
{
    class Player : BaseEntity
    {
        private bool _directionUp = false;
        private bool _directionDown = false;
        private bool _directionRight = false;
        private bool _directionLeft = false;
        private bool _jump = false;

        public Player(Canvas gameCanvas, int width, int height, int speed) : base(gameCanvas, width, height, speed)
        {
            this.Element = new Image
            {
                Name = "Player",
                Width = width,
                Height = height,
                Source = new BitmapImage(new Uri("ms-appx:///Assets/player.png"))
            };

            this.GameCanvas.Children.Add(this.Element);
            this._Initialize();
        }
        //key events
        private void _Initialize()
        {
            Window.Current.CoreWindow.KeyDown += HandleKeyDown;
            Window.Current.CoreWindow.KeyUp += HandleKeyUp;

        }

        // Occur on key down
        private void HandleKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Up:
                    this._directionUp = true;
                    break;
                case VirtualKey.Down:
                    this._directionDown = true;
                    break;
                case VirtualKey.Right:
                    this._directionRight = true;
                    break;
                case VirtualKey.Left:
                    this._directionLeft = true;
                    break;
                case VirtualKey.Space:
                    this._jump = true;
                    break;
            }
        }
        // Occur on key up
        private void HandleKeyUp(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Up:
                    this._directionUp = false;
                    break;
                case VirtualKey.Down:
                    this._directionDown = false;
                    break;
                case VirtualKey.Right:
                    this._directionRight = false;
                    break;
                case VirtualKey.Left:
                    this._directionLeft = false;
                    break;
                case VirtualKey.Space:
                    this._jump = false;
                    break;
            }
        }

        public override void Update()
        {
            base.Update();

            // Get player current position on the canvas
            double playerX = Canvas.GetLeft(this.Element);
            double playerY = Canvas.GetTop(this.Element);

            double newPosition;

            double newRandomPositionX;
            double newRandomPositionY;

            // prevent up And down simultaneously
            if (this._directionUp)
            {
                newPosition = playerY - this.Speed;
                Canvas.SetTop(this.Element, newPosition < 0 ? 0 : newPosition);
            }
            else if (this._directionDown)
            {
                newPosition = playerY + this.Speed;
                Canvas.SetTop(this.Element, newPosition + this.Height > this.GameCanvas.Height ? this.GameCanvas.Height - this.Height : newPosition);
            }

            // prevent right And left simultaneously
            if (this._directionRight)
            {
                newPosition = playerX + this.Speed;
                Canvas.SetLeft(this.Element, newPosition + this.Width > this.GameCanvas.Width ? this.GameCanvas.Width - this.Width : newPosition);
            }
            else if (this._directionLeft)
            {
                newPosition = playerX - this.Speed;
                Canvas.SetLeft(this.Element, newPosition < 0 ? 0 : newPosition);
            }

            //creating the jump function, using thread.sleep will give the player time to respond after he switched position
            if (this._jump)
            {
                Random rnd = new Random();
                Canvas.SetLeft(this.Element, rnd.Next(1280));
                Canvas.SetTop(this.Element, rnd.Next(720));
                Thread.Sleep(100);

            }
        }
    }
}

