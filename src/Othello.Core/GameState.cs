

using System;

namespace Othello.Core
{


    /// <summary>
    /// <remarks>Order is important, don't reorder!</remarks>
    /// </summary>
    public enum GameState
    {
        BlacksTurn,
        WhitesTurn,
        BlackWon,
        WhiteWon,
        Draw
    }
}
