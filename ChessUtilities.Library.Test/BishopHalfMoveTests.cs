using Xunit;
using ChessUtilities.Library.Parser;
using ChessUtilities.Library.Chess;

namespace ChessUtilities.Library.Test;

public class BishopHalfMoveTests
{
    BishopHalfMoveAdapter _adapter;
    public BishopHalfMoveTests()
    {
        _adapter = new BishopHalfMoveAdapter();
    }

    [Fact]
    public void Recognize_BishopMove()
    {
        Assert.NotEmpty(_adapter.Recognize(new(Position.None, Position.None, new(Color.None, PieceType.Bishop), Capture.None)));
    }

    [Fact]
    public void DoNotRecognize_NonBishopMove()
    {
        Assert.Empty(_adapter.Recognize(new(Position.None, Position.None, new(Color.None, PieceType.Knight), Capture.None)));
    }

    [Fact]
    public void Recognize_Generates14Candidates()
    {
        
    }
}
