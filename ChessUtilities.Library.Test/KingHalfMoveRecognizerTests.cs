using Xunit;
using ChessUtilities.Library.Parser;
using ChessUtilities.Library.Chess;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Antlr4.Runtime.Dfa;

namespace ChessUtilities.Library.Test;

public class KingHalfMoveAdapterTests
{
    private KingHalfMoveAdapter _adapter;
    public KingHalfMoveAdapterTests()
    {
        _adapter = new KingHalfMoveAdapter();
    }

    [Fact]
    public void Recognize_KingMove()
    {
        Assert.NotEmpty(_adapter.Recognize(new(Position.None, Position.None, new(Color.White, PieceType.King), Capture.None)));
    }

    [Fact]
    public void DoNotRecognize_NonKingMove()
    {
        Assert.Empty(_adapter.Recognize(new(Position.None, Position.None, ChessPiece.WhiteKnight, Capture.None)));
    }

    [Fact]
    public void Recognize_Generates8Candidates()
    {
        Assert.Equal(8,_adapter.Recognize(new(Position.None, Position.None, new(Color.White, PieceType.King), Capture.None)).Count());
    }

    [Fact]
    public void Recognize_All8CandidateSourcesHaveChebyshevDistance1FromTarget()
    {
        var chebyshevMetric = (Position p1, Position p2 ) => Math.Max(Math.Abs(p1.Rank - p2.Rank),Math.Abs(p1.File-p2.File));
        Assert.Empty(_adapter.Recognize(new(Position.None, Position.None, new(Color.White, PieceType.King), Capture.None)).Where(m=>chebyshevMetric(m.Source,m.Target) != 1));
    }
}
