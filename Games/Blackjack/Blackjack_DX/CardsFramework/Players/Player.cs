#region File Description
//-----------------------------------------------------------------------------
// Player.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

namespace CardsFramework;

#region Using Statements
#endregion

/// <summary>
/// Represents base player to be extended by inheritance for each
/// card game.
/// </summary>
public class Player
{
    #region Property
    public string Name { get; set; }
    public CardsGame Game { get; set; }
    public Hand Hand { get; set; }
    #endregion

    public Player(string name, CardsGame game)
    {
        Name = name;
        Game = game;
        Hand = new Hand();
    }
}
