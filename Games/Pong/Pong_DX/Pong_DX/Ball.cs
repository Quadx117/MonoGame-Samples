namespace Pong_DX
{
    using System;
    using Microsoft.Xna.Framework;

    public class Ball
    {
        private const int MAX_VELOCITY = 64;

        private const int BALL_RADIUS = 4;

        /// <summary>
        /// The bounding box of our rectangular ball for this game.
        /// </summary>
        public Rectangle BoundingBox { get; private set; }

        /// <summary>
        /// Represents the velocity vector.
        /// </summary>
        public Point Velocity { get; private set; }

        public Ball(Random random, bool leftSideScored)
        {
            Reset(random, leftSideScored);
        }

        // TODO(PERE): Check params (nullable VS simple int and pass 0 instead of null?)
        public void IncreaseVelocity(int? x = null, int? y = null)
        {
            // TODO(PERE): Refactor so we can change the values directly?
            // Currently we can't beceause Point is a Struct.
            Point tmpVelocity = Velocity;

            // TODO(PERE): Could use null coalescing operator?
            if (x != null)
            {
                tmpVelocity.X += x.Value;
            }

            if (y != null)
            {
                tmpVelocity.Y += y.Value;
            }

            // Cap the ball speed
            if (Math.Abs(tmpVelocity.X) > MAX_VELOCITY)
            {
                tmpVelocity.X = Math.Sign(tmpVelocity.X) * MAX_VELOCITY;
            }

            if (Math.Abs(tmpVelocity.Y) > MAX_VELOCITY)
            {
                tmpVelocity.X = Math.Sign(tmpVelocity.Y) * MAX_VELOCITY;
            }

            Velocity = tmpVelocity;
        }

        // TODO(PERE): Pass the game state instead?

        /// <summary>
        /// Moves the ball following the game rules.
        /// </summary>
        /// <param name="bounceOffSides">Whether or not the ball should bounce off
        /// the left or right sides. This is used when in idle mode.</param>
        /// <returns></returns>
        public (int, bool) Move(bool bounceOffSides)
        {
            bool bounced = false;

            // TODO(PERE): Refactor so we can change the values directly?
            // Currently we can't beceause Point is a Struct.
            Point pos = BoundingBox.Location;

            // TODO(PERE): See if can do the add in one line (i.e. directly add both points like Vectors2D)
            pos.X += Velocity.X;
            pos.Y += Velocity.Y;

            // TODO(PERE): Use Minkowski for collision detection?
            if (pos.Y < 0)
            {
                bounced = true;
                pos.Y = -pos.Y;
                ReverseVelocity(y: true);
            }
            else if (pos.Y + BoundingBox.Height > 480) //_renderTarget.Height) // TODO(PERE): Need to keep a reference on the renderTarget
            {
                bounced = true;
                //pos.Y = _renderTarget.Height - pos.Height - (pos.Y + pos.Height - _renderTarget.Height);
                pos.Y = 480 - (pos.Y + BoundingBox.Height - 480);
                ReverseVelocity(y: true);
            }

            int score = 0;
            // TODO(PERE): Find a better way to handle the scoring as it is not very clear
            if (pos.X < 0)
            {
                if (bounceOffSides)
                {
                    bounced = true;
                    pos.X = 0;
                    ReverseVelocity(x: true);
                }
                else
                {
                    score = -1;
                }
            }
            else if (pos.X + BoundingBox.Width > 640) //_renderTarget.Width)
            {
                if (bounceOffSides)
                {
                    bounced = true;
                    //pos.X = _renderTarget.Width - pos.Width;
                    pos.X = 640 - BoundingBox.Width;
                    ReverseVelocity(x: true);
                }
                else
                {
                    score = 1;
                }
            }

            SetPosition(pos);

            return (score, bounced);
        }

        public void Reset(Random random, bool leftSideScored)
        {
            int ballDiameter = BALL_RADIUS * 2;
            // TODO(PERE): Do something better for the center of the screen (i.e. pass in the constructor?)
            // Probably keep a reference to the render target se we can query its size.
            // TODO(PERE): Modify BoundingBox so we don't need to create a new Rectangle each time ?
            //             Maybe we should keep the position of the ball and its size separate, since the
            //             size will not change while playing.
            BoundingBox = new Rectangle((640 / 2) - BALL_RADIUS,
                                        (480 / 2) - BALL_RADIUS,
                                        ballDiameter,
                                        ballDiameter);
            // TODO(PERE): We had random.Next(2, 7) before, check what it does.
            // TODO(PERE): Make the intention clearer for the yPos by doing _random.Next(0, 1) == 0?
            // TODO(PERE): Use class constants for the min and max values
            Velocity = new Point(leftSideScored ? random.Next(3, 7) : -random.Next(3, 7),
                                 random.Next() > int.MaxValue / 2 ? random.Next(3, 7) : -random.Next(3, 7));
        }

        // TODO(EPRE): Do something better for the params
        public void ReverseVelocity(bool x = false, bool y = false)
        {
            // TODO(PERE): Refactor so we can change the values directly?
            // Currently we can't beceause Point is a Struct.
            Point tmpVelocity = Velocity;

            // TODO(PERE): Could maybe remove the if and use a multiply by
            // converting the bools or passing ints instead.
            if (x)
            {
                tmpVelocity.X = -tmpVelocity.X;
            }

            if (y)
            {
                tmpVelocity.Y = -tmpVelocity.Y;
            }

            Velocity = tmpVelocity;
        }

        public void SetPosition(Point point)
        {
            // TODO(PERE): Modify BoundingBox so we don't need to create a new Rectangle each time ?
            BoundingBox = new Rectangle(point, BoundingBox.Size);
        }
    }
}
