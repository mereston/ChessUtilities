using System;
using ChessUtilities.Library.Chess;

namespace ChessUtilities.Library.Parser;

public class KingHalfMoveAdapter
{
    public IEnumerable<Move> Recognize(Move move)
    {
        if(move.Piece.PieceType == PieceType.King)
        {
            Move[] moves = 
            [
                new(new(move.Target.Rank - 1, move.Target.File - 1), move.Target,  move.Piece, move.Capture),
                new(new(move.Target.Rank, move.Target.File - 1), move.Target,  move.Piece, move.Capture),
                new(new(move.Target.Rank - 1, move.Target.File), move.Target,  move.Piece, move.Capture),
                new(new(move.Target.Rank - 1, move.Target.File + 1), move.Target,  move.Piece, move.Capture),
                new(new(move.Target.Rank + 1, move.Target.File - 1), move.Target,  move.Piece, move.Capture),
                new(new(move.Target.Rank + 1, move.Target.File + 1), move.Target,  move.Piece, move.Capture),
                new(new(move.Target.Rank + 1, move.Target.File), move.Target,  move.Piece, move.Capture),
                new(new(move.Target.Rank, move.Target.File + 1), move.Target,  move.Piece, move.Capture),
            ];
            return moves;
        }
        else
        {
            return [];
        }
    }
}
