using System.Text.Json;
using ChessUtilities.Library.Chess;

namespace ChessUtilities.Library.Test;

public class UtilityTests
{
    [Fact]
    public void WeCanInstantiateARecordFromJson()
    {
        // Given
        string json = """{"Rank": 1, "File":1}""";
        // When
        Position position = JsonSerializer.Deserialize<Position>(json) ?? new(0,0);
        // Then
        Assert.Equal(new(1,1), position);
    }

    [Fact]
    public void WeCanInstantiateANestedRecordFromJson()
    {
        // Given
        string json = """{"Source": {"Rank": 1, "File":1}, "Target": {"Rank": 1, "File":1}, "Piece": { "Color": 0, "PieceType": 0 }, "Capture": { "Capture": false, "EnPassant": false} }""";
        // When
        Move move = JsonSerializer.Deserialize<Move>(json) ?? new(new(0,0),new(0,0), new(Color.Black, PieceType.Bishop), Capture.None);
        // Then
        Assert.Equal(new(new(1,1),new(1,1),new(Color.White,PieceType.King), Capture.None), move);
    }
}