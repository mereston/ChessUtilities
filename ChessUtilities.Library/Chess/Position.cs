namespace ChessUtilities.Library.Chess;

public record Position(int Rank, int File)
{
    public static Position None = new(-1,-1);
}