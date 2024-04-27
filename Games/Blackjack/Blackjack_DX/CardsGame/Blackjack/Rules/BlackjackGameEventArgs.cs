#region File Description
//-----------------------------------------------------------------------------
// BlackjackGameEventArgs.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

namespace Blackjack_DX;

#region Using Statements
using System;
using CardsFramework;

#endregion

public class BlackjackGameEventArgs : EventArgs
{
    public Player Player { get; set; }
    public HandTypes Hand { get; set; }
}
