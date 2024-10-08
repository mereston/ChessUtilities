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

        if(move.Source != Position.None)
        {
            return [move];
        }

        if(move.Target.Rank == 3 && move.Piece.Color == Color.White)
        {
            return [new(new(1,move.Target.File), Position.None, ChessPiece.WhitePawn), new(new(2, move.Target.File), Position.None, ChessPiece.WhitePawn)];
        }

        if(move.Target.Rank == 4 && move.Piece.Color == Color.Black)
        {
            return [new(new(6, move.Target.File), Position.None, ChessPiece.BlackPawn), new(new(5, move.Target.File), Position.None, ChessPiece.BlackPawn)];
        }

        return move.Piece.Color == Color.White ? [new(new(move.Target.Rank - 1, move.Target.File), move.Target, ChessPiece.WhitePawn)] : 
            [new(new(move.Target.Rank + 1, move.Target.File), move.Target, ChessPiece.BlackPawn)];
    }
}
