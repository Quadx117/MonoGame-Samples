namespace Pong_DX;

public enum GameState
{
    /// <summary>
    /// Waiting for the player to start a game
    /// </summary>
    Idle,

    /// <summary>
    /// Let the players choose their input mechanism from the available ones
    /// </summary>
    ChooseInput,

    /// <summary>
    /// Setting up a new game
    /// </summary>
    Start,

    /// <summary>
    /// Playing
    /// </summary>
    Play,

    /// <summary>
    /// After a scoring event, we check if a player has won the match
    /// </summary>
    CheckEnd,
}
