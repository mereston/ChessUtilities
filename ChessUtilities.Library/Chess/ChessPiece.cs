namespace ChessUtilities.Library.Chess;

public record ChessPiece(Color Color, PieceType PieceType)
{
    public char ToChar()
    {
        switch(PieceType)
        {
            case PieceType.King:
            {
                return Color == Color.White ? 'K' : 'k';
            }
            case PieceType.Queen:
            {
                return Color == Color.White ? 'Q' : 'q';
            }
            case PieceType.Bishop:
            {
                return Color == Color.White ? 'B' : 'b';
            }
            case PieceType.Knight:
            {
                return Color == Color.White ? 'N' : 'n';
            }
            case PieceType.Rook:
            {
                return Color == Color.White ? 'R' : 'r';
            }
            case PieceType.Pawn:
            {
                return Color == Color.White ? 'P' : 'p';
            }
            default:
            {
                return '\0';
            }
        }
    }
}
