using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace DodgeGame.Entities
{
    class Enemy : BaseEntity
    {
        private Player _target;
        private double _movementInterval;

        public Enemy(Canvas gameCanvas, int width, int height, int speed, double movementInterval = 1) : base(gameCanvas, width, height, speed)
        {          
            this.Element = new Image
            {
                Name = "Enemy",
                Width = width,
                Height = height,
                Source = new BitmapImage(new Uri("ms-appx:///Assets/enemy.png"))
            };

            this.GameCanvas.Children.Add(this.Element);
            this._movementInterval = movementInterval;
        }

        public override void Update()
        {
            base.Update();

            if (this.timePassed >= this._movementInterval) 
            {
                this.timePassed = 0;
            }
            else
            {
                return;
            }

            if (!this.IsAlive || this._target == null) return;

            double targetX = Canvas.GetLeft(this._target.Element);
            double targetY = Canvas.GetTop(this._target.Element);

            double selfX = Canvas.GetLeft(this.Element);
            double selfY = Canvas.GetTop(this.Element);

            double distanceX = Math.Abs(Math.Round(targetX - selfX));
            double distanceY = Math.Abs(Math.Round(targetY - selfY));

            double newEnemyX = selfX;
            double newEnemyY = selfY;

            double minDistanceX = this._target.Width / 2;
            double minDistanceY = this._target.Height / 2;

            if (distanceX > this._target.Width / 2 || distanceY > this._target.Height / 2)
            {
                if (selfX + minDistanceX <= targetX || selfX - minDistanceX >= targetX)
                {
                    if (targetX > selfX)
                        newEnemyX += this.Speed;
                    else
                        newEnemyX -= this.Speed;
                }

                if (selfY + minDistanceY <= targetY || selfY - minDistanceY >= targetY)
                {
                    if (targetY > selfY)
                        newEnemyY += this.Speed;
                    else
                        newEnemyY -= this.Speed;
                }
                this.SetPosition(newEnemyX, newEnemyY);
            }
        }

        public void SetTarget(Player target)
        {
            this._target = target;
        }

    }
}
