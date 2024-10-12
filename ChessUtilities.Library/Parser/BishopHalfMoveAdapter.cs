using System;
using ChessUtilities.Library.Chess;

namespace ChessUtilities.Library.Parser;

public class BishopHalfMoveAdapter
{
    public IEnumerable<Move> Recognize(Move move)
    {
        if(move.Piece.PieceType == PieceType.Bishop)
        {
            return [move];
        }

        return [];
    }
}
