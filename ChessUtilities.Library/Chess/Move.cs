namespace ChessUtilities.Library.Chess;

public record Move(Position Source, Position Target, ChessPiece Piece, Capture Capture);