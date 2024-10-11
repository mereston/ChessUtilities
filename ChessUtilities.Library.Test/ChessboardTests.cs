using System.Text.Json;
using ChessUtilities.Library.Chess;

namespace ChessUtilities.Library.Test;

public class ChessboardTests
{
    [Fact]
    public void EmptyBoardDumpsAsExpected()
    {
        Chessboard empty = Chessboard.EmptyBoard;

        Assert.Equal($"[ {new string(' ',64)} ]", empty.DumpBoard());
    }

    [Fact]
    public void EmptyBoardReportsAsExpected()
    {
        Assert.Equal("8/8/8/8/8/8/8/8", Chessboard.EmptyBoard.ToString());
    }

    [Fact]
    public void CanHandleSingleRank()
    {
        Chessboard board = new Chessboard(new("rnbqkbnr",Color.White, new(true,true,true,true),"-",0,0));
    }

    [Fact]
    public void SingleRankInducesExpectedInternalState()
    {
        Chessboard board = new Chessboard(new("rnbqkbnr",Color.White, new(true,true,true,true),"-",0,0));

        var expected = $"[ rnbqkbnr{new string(' ',56)} ]";
        Assert.Equal(expected, board.DumpBoard());
    }

    [Fact]
    public void MultipleRanksInduceExpectedInternalState()
    {
        Chessboard board = new Chessboard(new("rnbqkbnr/pppppppp",Color.White, new(true,true,true,true),"-",0,0));

        var expected = $"[ rnbqkbnrpppppppp{new string(' ',48)} ]";
        Assert.Equal(expected, board.DumpBoard());
    }

    [Fact]
    public void EmptyRanksInduceExpectedInternalState()
    {
        Chessboard board = new Chessboard(new("rnbqkbnr/pppppppp/8/8/8/8",Color.White, new(true,true,true,true),"-",0,0));

        var expected = $"[ rnbqkbnrpppppppp{new string(' ',48)} ]";
        Assert.Equal(expected, board.DumpBoard());
    }

    [Fact]
    public void StartInducesExpectedInternalState()
    {
        Chessboard board = new Chessboard(ForsythEdwardsNotation.Start);

        var expected = $"[ rnbqkbnrpppppppp{new string(' ',32)}PPPPPPPPRNBQKBNR ]";
        Assert.Equal(expected, board.DumpBoard());
    }

    [Fact]
    public void CanHandleSingleRankPlusDelimiter()
    {
        Chessboard board = new Chessboard(new("rnbqkbnr/",Color.White, new(true,true,true,true),"-",0,0));
    }

    [Fact]
    public void CanHandleTwoRanks()
    {
        Chessboard board = new Chessboard(new("rnbqkbnr/pppppppp",Color.White, new(true,true,true,true),"-",0,0));
    }

    [Fact]
    public void CanHandleEmptyRank()
    {
        Chessboard board = new Chessboard(new("rnbqkbnr/pppppppp/8",Color.White, new(true,true,true,true),"-",0,0));
    }

    [Fact]
    public void DefaultConstructorGivesStartingPosition()
    {
        Chessboard board = new Chessboard();

        Assert.Equal("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", board.ToString());
    }

    [Theory]
    [InlineData("""{"Source": {"Rank": 1, "File":3}, "Target": {"Rank": 3, "File":3}, "Piece": { "Color": 0, "PieceType": 5} }""", "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR")]
    public void FirstMoveReportsCorrectly(string moveJson, string expectedBoardState)
    {
        Chessboard board = new Chessboard();
        Move move = JsonSerializer.Deserialize<Move>(moveJson) ?? new(new(0,0),new(0,0),new(0,0), Capture.None);
        board.Move(move);

        Assert.Equal(expectedBoardState, board.ToString());
    }
}