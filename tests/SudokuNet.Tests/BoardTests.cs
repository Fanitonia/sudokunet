using FluentAssertions;

namespace SudokuNet.Tests;
public class BoardTests
{
    private const string ValidSolvedBoardString = "281453697473962815596718243145236789367589124829147356614325978732894561958671432";
    private const string ValidUnsolvedBoardString = "081450697473000815506718203145236709367589024829140356614325978732890561958671430"; // 12 empty cells
    private const string EmptyBoardString = "000000000000000000000000000000000000000000000000000000000000000000000000000000000";

    [Theory]
    [InlineData(ValidSolvedBoardString, 0)]
    [InlineData(ValidUnsolvedBoardString, 12)]
    [InlineData(EmptyBoardString, 81)]
    public void EmptyCellCount_ShouldReturnExpected(string boardString, int expected)
    {
        var board = new Board();
        board.LoadBoardFrom(boardString);

        board.EmptyCellCount.Should().Be(expected);

    }
}
