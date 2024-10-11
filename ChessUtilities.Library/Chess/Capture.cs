namespace ChessUtilities.Library.Chess;

public record Capture(bool Captures = false, bool EnPassant= false)
{
    public static Capture None => new();
}