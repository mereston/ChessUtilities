namespace ChessUtilities.Library.Chess;

public record ForsythEdwardsNotation(string BoardState, Color ToMove, CastlingAvailability CastlingState, string EnPassantSquare, int HalfMoveClock, int FullMoveNumber)
{
    public static ForsythEdwardsNotation Start = new("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", Color.White, new(true,true,true,true), "-", 0, 1);            
    public static ForsythEdwardsNotation Empty = new(string.Empty, Color.White, new(true,true,true,true), "-", 0, 1);
    public override string ToString()
    {
        string activeColor = ToMove==Color.Black? "b": "w";
        string castling = 
            !(CastlingState.BlackKingside && CastlingState.BlackQueenside && CastlingState.WhiteKingside && CastlingState.WhiteQueenside) ? 
                "-" :
                $"{(CastlingState.BlackKingside ? "k" : string.Empty)}{(CastlingState.BlackQueenside ? "q" : string.Empty)}{(CastlingState.WhiteKingside ? "K" : string.Empty)}{(CastlingState.WhiteQueenside ? "Q" : string.Empty)}";

        return $"{BoardState} {activeColor} {castling} {EnPassantSquare} {HalfMoveClock} {FullMoveNumber}";
    }
}