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

    [Fact]
    public void SetCellValue_ShouldSetValue_WhenCellIsNotLocked()
    {
        var board = new Board();
        board.field[1, 0].isLocked = true;

        board.SetCell(7, 5, 5).Should().BeTrue();
        board.SetCell(0, 1, 5).Should().BeFalse();
    }

    [Fact]
    public void GetCell_ShouldReturnCellValue()
    {
        var board = new Board();
        board.field[5, 5].value = 5;
        board.GetCell(5, 5).Should().Be(5);
        board.GetCell(0, 0).Should().Be(0);

    }

    [Fact]
    public void DeleteCell_ShouldSetCellToEmpty_WhenCellIsNotLocked()
    {
        var board = new Board();
        board.field[5, 4].value = 5;
        board.field[5, 4].isLocked = true;
        board.field[4, 5].value = 9;

        board.DeleteCellValue(4, 5).Should().BeFalse();
        board.field[5, 4].value.Should().Be(5);

        board.DeleteCellValue(5, 4).Should().BeTrue();
        board.field[4, 5].value.Should().Be(0);
    }
}
