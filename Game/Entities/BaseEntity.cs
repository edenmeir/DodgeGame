using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace DodgeGame.Entities
{
    //Game Logic, player and enemy will inherit from BaseEntity
    abstract class BaseEntity
    {
        private Canvas _gameCanvas;

        public double PositionX;
        public double PositionY;
        public float Speed;
        public bool IsAlive;
        public int Width;
        public int Height;
        public double timePassed;
        public UIElement Element;

        //creating Canvas
        public Canvas GameCanvas { get { return this._gameCanvas; } }

        public BaseEntity(Canvas gameCanvas, int width, int height, int speed)
        {
            this._gameCanvas = gameCanvas;

            this.Width = width;
            this.Height = height;
            this.Speed = speed;

            this.PositionX = 0;
            this.PositionY = 0;
            this.IsAlive = true;
        }
        //Canvas Position on the grid
        public void SetPosition(double x, double y)
        {
            this.PositionX = x;
            this.PositionY = y;
            Canvas.SetLeft(this.Element, this.PositionX);
            Canvas.SetTop(this.Element, this.PositionY);
        }

        public bool IsCollideWith(BaseEntity entity, int distance = 0)
        {
            Rect rect1 = new Rect()
            {
                X = Canvas.GetLeft(this.Element),
                Y = Canvas.GetTop(this.Element),
                Width = this.Width,
                Height = this.Height
            };
            Rect rect2 = new Rect()
            {
                X = Canvas.GetLeft(entity.Element) - distance,
                Y = Canvas.GetTop(entity.Element) - distance,
                Width = entity.Width + distance,
                Height = entity.Height + distance
            };

            Rect collider = RectHelper.Intersect(rect1, rect2);

            return !collider.IsEmpty;
        }

        public void Kill()
        {
            this._gameCanvas.Children.Remove(this.Element);
        }

        public virtual void Update()
        {
            this.timePassed += 0.02f;
        }
    }
}
