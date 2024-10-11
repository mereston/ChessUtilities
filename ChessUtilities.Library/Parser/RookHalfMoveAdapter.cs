using System;
using ChessUtilities.Library.Chess;

namespace ChessUtilities.Library.Parser;

public class RookHalfMoveAdapter
{
    public IEnumerable<Move> Recognize(Move move)
    {
        if(move.Piece.PieceType == PieceType.Rook)
        {
            if(move.Source != Position.None)
            {
                if(move.Source.Rank != move.Target.Rank && move.Source.File != move.Target.File)
                {
                    return [];
                }
                
                return [move];
            }

            List<Move> moves = new List<Move>();
            for(int i = 0; i < 8; i++)
            {
                if(i == move.Target.Rank)
                {
                    continue;
                }
                moves.Add(new(new(i,move.Target.File), move.Target, move.Piece,move.Capture));
            }
            for(int j = 0; j < 8; j++)
            {
                if(j == move.Target.File)
                {
                    continue;
                }
                moves.Add(new(new(move.Target.Rank, j), move.Target, move.Piece,move.Capture));
            }
            return moves;
        }

        return [];
    }
}
