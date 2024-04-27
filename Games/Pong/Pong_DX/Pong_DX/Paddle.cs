namespace Pong_DX;

using System;
using Microsoft.Xna.Framework;

public class Paddle
{
    // TODO(PERE): Use inheritance and interfaces to separate AIPaddle from HumanPaddle?
    public const int AI_PADDLE_SPEED = 4;

    private const int PADDLE_WIDTH = 8;

    private const int PADDLE_HEIGHT = 32;

    public Rectangle BoundingBox { get; private set; }

    private readonly bool _side;

    // TODO(PERE): Use enum for the players
    public Paddle(bool side)
    {
        _side = side;
        // TODO(PERE): Pass renderTarget and use constants from the sides instead.
        int xPos = side ? 600 : 32;
        BoundingBox = new Rectangle(xPos, 224, PADDLE_WIDTH, PADDLE_HEIGHT);
        // TODO(PERE): This is what we had before
        //_paddles = new Rectangle[]
        //{
        //    new Rectangle(32, (_renderTarget.Height / 2) - 16, 8, 32),
        //    new Rectangle(_renderTarget.Width - 40, (_renderTarget.Height / 2) - 16, 8, 32),
        //}
    }

    public bool BallIsAbleToBeHit(Ball ball)
    {
        bool directionCheck;
        bool distanceCheck;
        if (_side)
        {
            directionCheck = ball.Velocity.X > 0;
            distanceCheck = ball.BoundingBox.X + BoundingBox.Width > BoundingBox.X;
        }
        else
        {
            directionCheck = ball.Velocity.X < 0;
            distanceCheck = ball.BoundingBox.X < BoundingBox.X + BoundingBox.Width;
        }

        return directionCheck && distanceCheck;
    }

    public void AIMove(Ball ball)
    {
        // TODO(PERE): Allow the AI to make "errors", i.e. do not track perfectly?
        int delta = ball.BoundingBox.Y + (ball.BoundingBox.Height / 2) - (BoundingBox.Y + (BoundingBox.Height / 2));
        Point pos = BoundingBox.Location;

        // TODO(PERE): Math.Clamp
        if (Math.Abs(delta) > AI_PADDLE_SPEED)
        {
            delta = Math.Sign(delta) * AI_PADDLE_SPEED;
        }

        pos.Y += delta;

        FixBounds(pos);
    }

    public void PlayerMove(int yDelta)
    {
        Point pos = BoundingBox.Location;
        pos.Y += yDelta;
        FixBounds(pos);
    }

    // TODO(PERE): Validate this VS what I did in the past at uLaval or HandmadeHero, etc.
    // TODO(PERE): Use enum for the paddles/players to make it clearer.
    public bool CollisionCheck(Ball ball)
    {
        // TODO(PERE): Refactor to have only 1 return point at the end?
        if (!BallIsAbleToBeHit(ball))
        {
            return false;
        }

        (float delta, bool wayPastPaddle) = FindDeltaInBallMovement(ball);
        if (wayPastPaddle)
        {
            return false;
        }

        float deltaTime = delta / ball.Velocity.X;
        int collY = (int)(ball.BoundingBox.Y - (ball.Velocity.Y * deltaTime));
        int collX = (int)(ball.BoundingBox.X - (ball.Velocity.X * deltaTime));

        // Check if we actually collided with a paddle
        if (PaddleCheck(ball, collX, collY))
        {
            ball.SetPosition(new Point(collX, collY));

            int diffY = collY + (ball.BoundingBox.Height / 2) - (BoundingBox.Y + (BoundingBox.Height / 2));
            diffY /= BoundingBox.Height / 8;
            diffY -= Math.Sign(diffY);

            // Reverse ball velocity and increase by 1
            //ball.Velocity.X = -(ball.Velocity.X + Math.Sign(ball.Velocity.X));
            ball.IncreaseVelocity(Math.Sign(ball.Velocity.X), diffY);
            ball.ReverseVelocity(x: true);

            return true;
        }
        else
        {
            return false;
        }
    }

    private (float, bool) FindDeltaInBallMovement(Ball ball)
    {
        float delta;
        bool wayPastPaddle;
        if (_side)
        {
            delta = ball.BoundingBox.X + ball.BoundingBox.Width - BoundingBox.X;
            wayPastPaddle = delta > ball.Velocity.X + ball.BoundingBox.Width;
        }
        else
        {
            delta = ball.BoundingBox.X - (BoundingBox.X + BoundingBox.Width);
            wayPastPaddle = delta < ball.Velocity.X;
        }

        return (delta, wayPastPaddle);
    }

    // TODO(PERE): Math.Clamp instead?
    // Rename if not using Math.Clamp.
    private void FixBounds(Point pos)
    {
        // TODO(PERE): Add a value so the paddle does not go all the way
        // against the sides like the original pong game?
        // Maybe add a config option for this
        if (pos.Y < 0)
        {
            pos.Y = 0;
        }

        if (pos.Y + BoundingBox.Height > 480) // TODO(PERE): renderTarget.Height
        {
            pos.Y = 480 - BoundingBox.Height;
        }

        BoundingBox = new Rectangle(pos, BoundingBox.Size);
    }

    // TODO(PERE): Minkowski?
    // TODO(PERE): Better collision by stepping (i.e. ray-tracing the old position to the new position)?
    // TODO(PERE): Pass a Point or a Vector2D?
    private bool PaddleCheck(Ball ball, int x, int y)
    {
        // NOTE(PERE): We could use Rectangle.Intersect instead of doing it manually,
        // but it is possible for this check to fail if the ball moves too fast between
        // consecutive frames.
        //_paddles[index].Intersects(_ball);
        return x <= BoundingBox.X + BoundingBox.Width &&
               x + ball.BoundingBox.Width >= BoundingBox.X &&
               y <= BoundingBox.Y + BoundingBox.Height &&
               y + ball.BoundingBox.Height >= BoundingBox.Y;
    }
}
