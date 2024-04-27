//-----------------------------------------------------------------------------
// BlackjackGameEventArgs.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

namespace Blackjack_DX;

using System;
using CardsFramework;

public class BlackjackGameEventArgs : EventArgs
{
    public Player Player { get; set; }
    public HandTypes Hand { get; set; }
}
