using ChessUtilities.Library.Chess;

namespace ChessUtilities.Library.Parser;

public class PawnHalfMoveAdapter
{
    public IEnumerable<Move> Recognize(Move move)
    {
        if(move.Piece.PieceType != PieceType.Pawn || move.Target == Position.None)
        {
            return [];
        }

        if(move.Capture.Captures)
        {
            if(move.Source != Position.None)
            {
                if(Math.Abs(move.Source.File - move.Target.File) != 1)
                {
                    return [];
                }

                if(Math.Abs(move.Source.Rank - move.Target.Rank) != 1)
                {
                    return [];
                }
                
                return [move];
            }
            return [
                new(new(move.Target.Rank - 1, move.Target.File - 1),move.Target, move.Piece,move.Capture), 
                new(new(move.Target.Rank - 1, move.Target.File + 1),move.Target, move.Piece,move.Capture)
                ];            
        }

        if(move.Source != Position.None)
        {
            if(move.Source.File != move.Target.File)
            {
                return [];
            }

            if(Math.Abs(move.Source.Rank - move.Target.Rank) > 2)
            {
                return [];
            }
            
            return [move];
        }

        if(move.Target.Rank == 3 && move.Piece.Color == Color.White)
        {
            return [new(new(1,move.Target.File), Position.None, ChessPiece.WhitePawn, move.Capture), new(new(2, move.Target.File), Position.None, ChessPiece.WhitePawn, move.Capture)];
        }

        if(move.Target.Rank == 4 && move.Piece.Color == Color.Black)
        {
            return [new(new(6, move.Target.File), Position.None, ChessPiece.BlackPawn, move.Capture), new(new(5, move.Target.File), Position.None, ChessPiece.BlackPawn, move.Capture)];
        }

        return move.Piece.Color == Color.White ? [new(new(move.Target.Rank - 1, move.Target.File), move.Target, ChessPiece.WhitePawn, move.Capture)] : 
            [new(new(move.Target.Rank + 1, move.Target.File), move.Target, ChessPiece.BlackPawn, move.Capture)];
    }
}
