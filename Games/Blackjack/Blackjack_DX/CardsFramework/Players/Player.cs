//-----------------------------------------------------------------------------
// Player.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

namespace CardsFramework;

/// <summary>
/// Represents base player to be extended by inheritance for each
/// card game.
/// </summary>
public class Player
{
    public string Name { get; set; }
    public CardsGame Game { get; set; }
    public Hand Hand { get; set; }

    public Player(string name, CardsGame game)
    {
        Name = name;
        Game = game;
        Hand = new Hand();
    }
}
