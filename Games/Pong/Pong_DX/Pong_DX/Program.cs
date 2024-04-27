namespace Pong_DX;

using System;

public static class Program
{
    [STAThread]
    static void Main()
    {
        using var game = new PongGame();
        game.Run();
    }
}
