using ChessUtilities.Library.Parser;
using ChessUtilities.Library.Chess;
using System.Text.Json;
using System.Data.Common;

namespace ChessUtilities.Test;

public class HalfMoveAdapterTests
{
    [Fact]
    public void CanInstantiateAHalfMoveAdapter()
    {
        PawnHalfMoveAdapter adapter = new PawnHalfMoveAdapter();
    }

    [Fact]    
    public void RecognizesPawnMove()
    {
        PawnHalfMoveAdapter adapter = new PawnHalfMoveAdapter();

        var moves = adapter.Recognize(new Move(Position.None, new(3,3), ChessPiece.WhitePawn));
        Assert.NotEmpty(moves);
    }

    [Fact]
    public void DoNotRecognizeNoneTarget()
    {
        PawnHalfMoveAdapter adpater = new PawnHalfMoveAdapter();

        var moves = adpater.Recognize(new Move(Position.None, Position.None, ChessPiece.WhitePawn));
        Assert.Empty(moves);
    }

    [Fact]
    public void DoNotRecognizeNonPawnMove()
    {
        PawnHalfMoveAdapter adpater = new PawnHalfMoveAdapter();

        var moves = adpater.Recognize(new Move(Position.None, new(2,2), ChessPiece.WhiteKnight));
        Assert.Empty(moves);
    }

    [Fact]
    public void FourthRankUnqualifiedWhitePawnMoveGeneratesTwoCandidates()
    {
        PawnHalfMoveAdapter adapter = new PawnHalfMoveAdapter();

        var moves = adapter.Recognize(new Move(Position.None, new(3,3), ChessPiece.WhitePawn));
        Assert.Equal(2, moves.Count());
    }

    [Fact]
    public void FourthRankQualifiedWhitePawnMoveGeneratesOneCandidate()
    {
        PawnHalfMoveAdapter adapter = new PawnHalfMoveAdapter();

        var moves = adapter.Recognize(new Move(new(1,3), new(3,3), ChessPiece.WhitePawn));
        Assert.Single(moves);
    }

    [Theory]
    [InlineData("""{"Source": {"rank": -1, "file":-1}, "Target": {"rank": 2, "file":3}, "Piece": { "Color": 0, "PieceType": 5} , "Captures": false}""")]
    public void NonFourthRankNonCapturingWhitePawnMoveGeneratesOneCandidate(string moveJson)
    {
        Move move = JsonSerializer.Deserialize<Move>(moveJson) ?? new(Position.None, Position.None, new(Color.Black, PieceType.Bishop));
        PawnHalfMoveAdapter adapter = new PawnHalfMoveAdapter();

        var moves = adapter.Recognize(move);
        Assert.Single(moves);
    }

    [Fact]
    public void FifthRankUnqualifiedBlackMoveGeneratesTwoCandidates()
    {
        PawnHalfMoveAdapter adapter = new PawnHalfMoveAdapter();

        var moves = adapter.Recognize(new Move(Position.None, new(4,3), ChessPiece.BlackPawn));
        Assert.Equal(2, moves.Count());
    }

    [Fact]    
    public void FifthRankQualifiedBlackMoveGeneratesOneCandidate()
    {
        PawnHalfMoveAdapter adapter = new PawnHalfMoveAdapter();

        var moves = adapter.Recognize(new(new(5,3), new(4,3), new(Color.Black, PieceType.Pawn)));
        Assert.Single(moves);
    }

    [Fact]
    public void FourthRankUnqualifiedNonCapturingWhitePawnMoveGeneratesCandidatesWithSameFile()
    {
        PawnHalfMoveAdapter adapter = new PawnHalfMoveAdapter();

        var moves = adapter.Recognize(new(Position.None, new(3,3), new(Color.White, PieceType.Pawn)));
        Assert.Empty(moves.Where(m=> m.Source.File != 3));
    }

    [Fact]
    public void FourthRankQualifiedNonCapturingWhitePawnMoveGeneratesCandidateWithSameFile()
    {
        PawnHalfMoveAdapter adapter = new PawnHalfMoveAdapter();

        var moves = adapter.Recognize(new(Position.None, new(3,3), new(Color.White, PieceType.Pawn)));
        Assert.Empty(moves.Where(m=> m.Source.File != 3));
    }

    [Fact]
    public void FifthRankUnqualifiedNonCapturingBlackPawnMoveGeneratesCandidatesWithSameFile()
    {
        PawnHalfMoveAdapter adapter = new PawnHalfMoveAdapter();

        var moves = adapter.Recognize(new(Position.None, new(4,3), new(Color.Black, PieceType.Pawn)));
        Assert.Empty(moves.Where(m=> m.Source.File != 3));
    }

    [Fact]
    public void FifthRankQualifiedNonCapturingBlackPawnMoveGeneratesCandidateWithSameFile()
    {
        PawnHalfMoveAdapter adapter = new PawnHalfMoveAdapter();

        var moves = adapter.Recognize(new(Position.None, new(4,3), new(Color.Black, PieceType.Pawn)));
        Assert.Empty(moves.Where(m=> m.Source.File != 3));
    }

    [Fact]
    public void FourthRankUnqualifiedNonCapturingWhitePawnMoveGeneratesMoveWithSecondRankSource()
    {
        PawnHalfMoveAdapter adapter = new PawnHalfMoveAdapter();

        var moves = adapter.Recognize(new(Position.None, new(3,3), new(Color.White, PieceType.Pawn)));
        Assert.Single(moves.Where(m=> m.Source.Rank == 1));
    }

    [Fact]
    public void FourthRankQualifiedNonCapturingWhitePawnMoveGeneratesMoveWithThirdRankSource()
    {
        PawnHalfMoveAdapter adapter = new PawnHalfMoveAdapter();

        var moves = adapter.Recognize(new(Position.None, new(3,3), new(Color.White, PieceType.Pawn)));
        Assert.Single(moves.Where(m=> m.Source.Rank == 2));
    }

    [Fact]
    public void FifthRankUnqualifiedNonCapturingBlackPawnMoveGeneratesMoveWithSeventhRankSource()
    {
        PawnHalfMoveAdapter adapter = new PawnHalfMoveAdapter();

        var moves = adapter.Recognize(new(Position.None, new(4,3), new(Color.Black, PieceType.Pawn)));
        Assert.Single(moves.Where(m=> m.Source.Rank == 6));
    }

    [Fact]
    public void FifthRankQualifiedNonCapturingBlackPawnMoveGeneratesMoveWithSixthRankSource()
    {
        PawnHalfMoveAdapter adapter = new PawnHalfMoveAdapter();

        var moves = adapter.Recognize(new(Position.None, new(4,3), new(Color.Black, PieceType.Pawn)));
        Assert.Single(moves.Where(m=> m.Source.Rank == 5));
    }

    [Fact]
    public void NonFourthRankWhiteMoveGeneratesSourceFromPreceedingRank()
    {
        Move move = new(Position.None, new(4,3), new(Color.White, PieceType.Pawn));
        PawnHalfMoveAdapter adapter = new PawnHalfMoveAdapter();
        var moves = adapter.Recognize(move);
        
        Assert.Single(moves.Where(m=> m.Source.Rank == move.Target.Rank - 1));
    }

    [Fact]
    public void NonFifthRankBlackMoveGeneratesSourceFromSucceedingRank()
    {
        Move move = new(Position.None, new(3,3), new(Color.Black, PieceType.Pawn));
        PawnHalfMoveAdapter adapter = new PawnHalfMoveAdapter();
        var moves = adapter.Recognize(move);
        
        Assert.Single(moves.Where(m=> m.Source.Rank == move.Target.Rank + 1));
    }

    [Theory]
    [InlineData("""{"Source": {"Rank": -1, "File":-1}, "Target": {"Rank": 4, "File":1}, "Piece": { "Color": 0, "PieceType": 5} , "Captures": false}""")]
    [InlineData("""{"Source": {"Rank": -1, "File":-1}, "Target": {"Rank": 3, "File":1}, "Piece": { "Color": 1, "PieceType": 5} , "Captures": false}""")]
    public void RecognizeIsIdempotentOnColor(string moveJson)
    {
        Move move = JsonSerializer.Deserialize<Move>(moveJson) ?? new(new(0,0),new(0,0), new(Color.Black, PieceType.Bishop));
        PawnHalfMoveAdapter adapter = new PawnHalfMoveAdapter();

        var moves = adapter.Recognize(move);
        Assert.Empty(moves.Where(m=> m.Piece.Color != move.Piece.Color));
    }

    [Theory]
    [InlineData("""{"Source": {"Rank": -1, "File":-1}, "Target": {"Rank": 4, "File":1}, "Piece": { "Color": 0, "PieceType": 5} , "Captures": false}""")]
    [InlineData("""{"Source": {"Rank": -1, "File":-1}, "Target": {"Rank": 3, "File":1}, "Piece": { "Color": 1, "PieceType": 5} , "Captures": false}""")]
    public void NonCapturingCandidateSourcesHaveSameFileAsTarget(string moveJson)
    {
        Move move = JsonSerializer.Deserialize<Move>(moveJson) ?? new(new(0,0),new(0,0), new(Color.Black, PieceType.Bishop));

        PawnHalfMoveAdapter adapter = new PawnHalfMoveAdapter();

        var moves = adapter.Recognize(move);
        Assert.Empty(moves.Where(m=> m.Source.File != move.Target.File));
    }

    [Fact]
    public void RecognizeIsIdempotentOnTarget()
    {
        Move move = new(Position.None, new(4,3), new(Color.White, PieceType.Pawn));
        PawnHalfMoveAdapter adapter = new PawnHalfMoveAdapter();
        var moves = adapter.Recognize(move);
        
        Assert.Empty(moves.Where(m=> m.Target != move.Target));
    }

    [Theory(Skip = "We're not ready for this yet.")]
    [InlineData("""{"Source": {"rank": -1, "file":-1}, "Target": {"rank": 1, "file":1}, "Piece": { "Color": 0, "PieceType": 5} , "Captures": true}""")]
    public void UnqualifiedCaptureOnInteriorFilesGeneratesTwoCandidates(string moveJson)
    {
        Move move = JsonSerializer.Deserialize<Move>(moveJson) ?? new(new(0,0),new(0,0), new(Color.Black, PieceType.Bishop));
        PawnHalfMoveAdapter adapter = new PawnHalfMoveAdapter();

        var moves = adapter.Recognize(move);
        Assert.Equal(2, moves.Count());
    }
}
