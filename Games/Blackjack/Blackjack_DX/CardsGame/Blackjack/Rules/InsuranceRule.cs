//-----------------------------------------------------------------------------
// InsuranceRule.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

namespace Blackjack_DX;

using System;
using CardsFramework;

/// <summary>
/// Represents a rule which checks if the human player can use insurance
/// </summary>
class InsuranceRule : GameRule
{
    readonly Hand dealerHand;
    bool done = false;

    /// <summary>
    /// Creates a new instance of the <see cref="InsuranceRule"/> class.
    /// </summary>
    /// <param name="dealerHand">The dealer's hand.</param>
    public InsuranceRule(Hand dealerHand)
    {
        this.dealerHand = dealerHand;
    }

    /// <summary>
    /// Checks whether or not the dealer's revealed card is an ace.
    /// </summary>
    public override void Check()
    {
        if (!done)
        {
            if (dealerHand.Count > 0)
            {
                if (dealerHand[0].Value == CardValue.Ace)
                {
                    FireRuleMatch(EventArgs.Empty);
                }

                done = true;
            }
        }
    }
}
