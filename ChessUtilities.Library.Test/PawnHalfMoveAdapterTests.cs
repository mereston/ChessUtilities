using ChessUtilities.Library.Parser;
using ChessUtilities.Library.Chess;
using System.Text.Json;
using System.Data.Common;

namespace ChessUtilities.Test;

public class PawnHalfMoveAdapterTests
{
    private PawnHalfMoveAdapter _adapter;

    public PawnHalfMoveAdapterTests()
    {
        _adapter = new PawnHalfMoveAdapter();
    }    
    
    [Fact]    
    public void RecognizesPawnMove()
    {       
        Assert.NotEmpty(_adapter.Recognize(new Move(Position.None, new(3,3), ChessPiece.WhitePawn, Capture.None)));
    }

    [Fact]
    public void DoNotRecognizeNoneTarget()
    {
        var moves = _adapter.Recognize(new Move(Position.None, Position.None, ChessPiece.WhitePawn, Capture.None));
        Assert.Empty(moves);
    }

    [Fact]
    public void DoNotRecognizeNonPawnMove()
    {
        var moves = _adapter.Recognize(new Move(Position.None, new(2,2), ChessPiece.WhiteKnight, Capture.None));
        Assert.Empty(moves);
    }

    [Theory]
    [InlineData(3, Color.White)]
    [InlineData(4, Color.Black)]
    public void Unqualified_JumpRank_GeneratesTwoCandidates(int rank, Color color)
    {
        var moves = _adapter.Recognize(new Move(Position.None, new(rank,3), new(color, PieceType.Pawn), Capture.None));
        Assert.Equal(2, moves.Count());
    }

    [Theory]
    [InlineData(3, Color.White)]
    [InlineData(4, Color.Black)]
    public void Qualified_JumpRank_GeneratesOneCandidate(int rank, Color color)
    {        
        var moves = _adapter.Recognize(new Move(new(rank-1,3), new(rank,3), new(color, PieceType.Pawn), Capture.None));
        Assert.Single(moves);
    }

    [Theory]
    [InlineData(2, Color.White)]
    [InlineData(5, Color.Black)]
    public void NonJumpRank_NonCapturing_GeneratesOneCandidate(int rank, Color color)
    {
        var moves = _adapter.Recognize(new(Position.None, new(rank,3), new(color,PieceType.Pawn), Capture.None));
        Assert.Single(moves);
    }

    [Theory]
    [InlineData("""{"Rank":-1, "File":-1}""", 3, Color.White)] // Unqualified
    [InlineData("""{"Rank":-1, "File":-1}""", 4, Color.Black)]
    [InlineData("""{"Rank":1, "File": 3}""", 3, Color.White)] // Qualified
    [InlineData("""{"Rank":1, "File": 4}""", 4, Color.Black)]
    public void NonCapture_GeneratesCandidatesWithSameFile(string positionJson, int file, Color color)
    {
        Position position = JsonSerializer.Deserialize<Position>(positionJson) ?? new(8,8);
        var moves = _adapter.Recognize(new(position, new(3,file), new(color, PieceType.Pawn), Capture.None));
        Assert.Empty(moves.Where(m=> m.Source.File != file));
    }

    [Theory]
    [InlineData("""{"Rank":1, "File": 2}""", 3, Color.White)] // Qualified
    [InlineData("""{"Rank":1, "File": 8}""", 4, Color.Black)]
    public void NonCapture_RejectsSourcesWithImpossibleFiles(string positionJson, int file, Color color)
    {
        Position position = JsonSerializer.Deserialize<Position>(positionJson) ?? new(8,8);        
        Assert.Empty(_adapter.Recognize(new(position, new(3,file), new(color, PieceType.Pawn), Capture.None)));
    }

    [Theory]    
    [InlineData("""{"Rank":1, "File": 2}""", 5, Color.White)] // Qualified
    [InlineData("""{"Rank":1, "File": 2}""", 4, Color.Black)]
    public void NonCapture_RejectsSourcesWithImpossibleRanks(string positionJson, int rank, Color color)
    {
        Position position = JsonSerializer.Deserialize<Position>(positionJson) ?? new(8,8);        
        Assert.Empty(_adapter.Recognize(new(position, new(rank,2), new(color, PieceType.Pawn), Capture.None)));
    }

    [Theory]
    [InlineData("""{"Rank":-1, "File":-1}""", """{"Rank":3, "File":3}""", 2, Color.White)] // Unqualified
    [InlineData("""{"Rank":-1, "File":-1}""", """{"Rank":4, "File":3}""", 5, Color.Black)]
    [InlineData("""{"Rank":-1, "File":-1}""", """{"Rank":3, "File":3}""", 1, Color.White)] 
    [InlineData("""{"Rank":-1, "File":-1}""", """{"Rank":4, "File":3}""", 6, Color.Black)]
    [InlineData("""{"Rank":1, "File": 3}""", """{"Rank":3, "File":3}""", 1, Color.White)] // Qualified
    [InlineData("""{"Rank":6, "File": 3}""", """{"Rank":4, "File":3}""", 6,Color.Black)]
    [InlineData("""{"Rank":2, "File": 3}""", """{"Rank":3, "File":3}""", 2, Color.White)]
    [InlineData("""{"Rank":5, "File": 3}""", """{"Rank":4, "File":3}""", 5,Color.Black)]
    public void NonCapture_GeneratesCandidatesSpecifiedRank(string sourcePositionJson, string targetPositionJson, int expectedRank, Color color)
    {
        Position sourcePosition = JsonSerializer.Deserialize<Position>(sourcePositionJson) ?? new(8,8);
        Position targetPosition = JsonSerializer.Deserialize<Position>(targetPositionJson) ?? new(8,8);
        Assert.Single(_adapter.Recognize(new(sourcePosition, targetPosition, new(color, PieceType.Pawn), Capture.None)).Where(m=> m.Source.Rank == expectedRank));
    }   

    [Fact]
    public void NonFourthRankWhiteMoveGeneratesSourceFromPreceedingRank()
    {
        Move move = new(Position.None, new(4,3), new(Color.White, PieceType.Pawn), Capture.None);
        var moves = _adapter.Recognize(move);
        
        Assert.Single(moves.Where(m=> m.Source.Rank == move.Target.Rank - 1));
    }

    [Fact]
    public void NonFifthRankBlackMoveGeneratesSourceFromSucceedingRank()
    {
        Move move = new(Position.None, new(3,3), new(Color.Black, PieceType.Pawn), Capture.None);
        var moves = _adapter.Recognize(move);
        
        Assert.Single(moves.Where(m=> m.Source.Rank == move.Target.Rank + 1));
    }

    [Theory]
    [InlineData("""{"Source": {"Rank": -1, "File":-1}, "Target": {"Rank": 4, "File":1}, "Piece": { "Color": 0, "PieceType": 5} , "Capture": {"Captures": false, "EnPassant": false}}""")]
    [InlineData("""{"Source": {"Rank": -1, "File":-1}, "Target": {"Rank": 3, "File":1}, "Piece": { "Color": 1, "PieceType": 5} , "Capture": {"Captures": false, "EnPassant": false}}""")]
    public void RecognizeIsIdempotentOnColor(string moveJson)
    {
        Move move = JsonSerializer.Deserialize<Move>(moveJson) ?? new(new(0,0),new(0,0), new(Color.Black, PieceType.Bishop), Capture.None);
        var moves = _adapter.Recognize(move);
        Assert.Empty(moves.Where(m=> m.Piece.Color != move.Piece.Color));
    }

    [Theory]
    [InlineData("""{"Source": {"Rank": -1, "File":-1}, "Target": {"Rank": 4, "File":1}, "Piece": { "Color": 0, "PieceType": 5} , "Capture": {"Captures": false, "EnPassant": false}}""")]
    [InlineData("""{"Source": {"Rank": -1, "File":-1}, "Target": {"Rank": 3, "File":1}, "Piece": { "Color": 1, "PieceType": 5} , "Capture": {"Captures": false, "EnPassant": false}}""")]
    public void NonCapturingCandidateSourcesHaveSameFileAsTarget(string moveJson)
    {
        Move move = JsonSerializer.Deserialize<Move>(moveJson) ?? new(new(0,0),new(0,0), new(Color.Black, PieceType.Bishop), Capture.None);
        var moves = _adapter.Recognize(move);
        Assert.Empty(moves.Where(m=> m.Source.File != move.Target.File));
    }

    [Fact]
    public void RecognizeIsIdempotentOnTarget()
    {
        Move move = new(Position.None, new(4,3), new(Color.White, PieceType.Pawn), Capture.None);
        var moves = _adapter.Recognize(move);
        
        Assert.Empty(moves.Where(m=> m.Target != move.Target));
    }

    [Theory]
    [InlineData("""{"Source": {"Rank": -1, "File":-1}, "Target": {"Rank": 1, "File":1}, "Piece": { "Color": 0, "PieceType": 5} , "Capture": {"Captures": true, "EnPassant": false}}""")]
    [InlineData("""{"Source": {"Rank": -1, "File":-1}, "Target": {"Rank": 1, "File":1}, "Piece": { "Color": 1, "PieceType": 5} , "Capture": {"Captures": true, "EnPassant": false}}""")]
    [InlineData("""{"Source": {"Rank": -1, "File":-1}, "Target": {"Rank": 1, "File":5}, "Piece": { "Color": 0, "PieceType": 5} , "Capture": {"Captures": true, "EnPassant": false}}""")]
    [InlineData("""{"Source": {"Rank": -1, "File":-1}, "Target": {"Rank": 1, "File":3}, "Piece": { "Color": 1, "PieceType": 5} , "Capture": {"Captures": true, "EnPassant": false}}""")]
    public void Unqualified_Capture_GeneratesTwoCandidates(string moveJson)
    {
        Move move = JsonSerializer.Deserialize<Move>(moveJson) ?? new(new(0,0),new(0,0), new(Color.Black, PieceType.Bishop), Capture.None);
        var moves = _adapter.Recognize(move);
        Assert.Equal(2, moves.Count());
    }

    [Fact]
    public void Qualified_Capture_GeneratesOneCandidate()
    {
        Assert.Single(_adapter.Recognize(new(new(1,3), new(2,2), ChessPiece.WhitePawn, new(true))));
    }
}
