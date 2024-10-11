using ChessUtilities.Library.Chess;
using ChessUtilities.Library.Parser;

namespace ChessUtilities.Library.Test;

public class RookHalfMoveAdapterTests
{
    private RookHalfMoveAdapter _adapter;
    public RookHalfMoveAdapterTests()
    {
        _adapter = new RookHalfMoveAdapter();
    }

    [Fact]
    public void Recognizes_RookMove()
    {        
        Assert.NotEmpty(_adapter.Recognize(new(Position.None, new(7,7), new(Color.White, PieceType.Rook), Capture.None)));
    }

    [Fact]
    public void DoesNotRecognize_NonRookMove()
    {
        Assert.Empty(_adapter.Recognize(new(Position.None, new(7,7), new(Color.White, PieceType.King), Capture.None)));
    }

    [Fact]
    public void Unqualified_Recognize_Generates14Candidates()
    {
        Assert.Equal(14, _adapter.Recognize(new(Position.None, new(7,7), new(Color.White, PieceType.Rook), Capture.None)).Count());    
    }

    [Fact]
    public void Unqualified_Recognize_7CandidatesAreOnTheSameFile()
    {
        Assert.Equal(7, _adapter.Recognize(new(Position.None, new(7,7), new(Color.White, PieceType.Rook), Capture.None)).Where(m=>m.Source.File == 7).Count());
    }

    [Fact]
    public void Unqualified_Recognize_7CandidatesAreOnTheSameRank()
    {
        Assert.Equal(7, _adapter.Recognize(new(Position.None, new(7,7), new(Color.White, PieceType.Rook), Capture.None)).Where(m=>m.Source.Rank == 7).Count());
    }

    [Fact]
    public void Qualified_Recognize_GeneratesOneCandidate()
    {
        Assert.Single(_adapter.Recognize(new(new(0,7), new(7,7), new(Color.White, PieceType.Rook), Capture.None)));
    }

    [Fact]
    public void Qualified_DoesNotRecognize_CandidateSourceOffRankAndFile()
    {
        Assert.Empty(_adapter.Recognize(new(new(0,0), new(7,7), new(Color.White, PieceType.Rook), Capture.None)));
    }
}
