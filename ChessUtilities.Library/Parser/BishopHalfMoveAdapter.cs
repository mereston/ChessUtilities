using System;
using ChessUtilities.Library.Chess;

namespace ChessUtilities.Library.Parser;

public class BishopHalfMoveAdapter
{
    public IEnumerable<Move> Recognize(Move move)
    {
        if(move.Piece.PieceType == PieceType.Bishop)
        {
            List<Move> moves = new List<Move>();

            Position position = new(move.Target.Rank + 1, move.Target.File + 1);

            while(position.Rank < 8 && position.File < 8)
            {
                moves.Add(new(position, move.Target, move.Piece, move.Capture));
                position = new(position.Rank + 1, position.File + 1);
            }

            position = new(move.Target.Rank + 1, move.Target.File - 1);

            while(position.Rank < 8 && position.File > -1)
            {
                moves.Add(new(position, move.Target, move.Piece, move.Capture));
                position = new(position.Rank + 1, position.File - 1);
            }

            position = new(move.Target.Rank - 1, move.Target.File + 1);

            while(position.Rank >-1 && position.File < 8)
            {
                moves.Add(new(position, move.Target, move.Piece, move.Capture));
                position = new(position.Rank - 1, position.File + 1);
            }

            position = new(move.Target.Rank - 1, move.Target.File - 1);

            while(position.Rank > -1 && position.File > -1)
            {
                moves.Add(new(position, move.Target, move.Piece, move.Capture));
                position = new(position.Rank - 1, position.File - 1);
            }

            return moves;
        }

        return [];
    }
}
