using FluentAssertions;

namespace SudokuNet.Tests;
public class BoardTests
{
    private const string ValidSolvedBoardString = "281453697473962815596718243145236789367589124829147356614325978732894561958671432";
    private const string ValidUnsolvedBoardString = "081450697473000815506718203145236709367589024829140356614325978732890561958671430"; // 12 empty cells
    private const string UnvalidSolvedBoardString = "281453687473962815596718243145236789367589124839147356614325978732894561958671432";
    private const string UnvalidUnsolvedBoardString = "081850697473000815506718203145236709367589024829140356614325978732890561958671430";
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

    [Theory]
    [InlineData(3, 0, 9)] // Same row
    [InlineData(0, 3, 9)] // Same column
    [InlineData(2, 1, 9)] // Same 3x3 square
    [InlineData(0, 0, 9)] // Same cell
    public void IsPositionSuitable_ShouldReturnFalse(
        int cordX,
        int cordY,
        int value)
    {
        var board = new Board();
        board.field[0, 0].value = 9;

        var result = board.IsPositionSuitable(cordX, cordY, value);

        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(3, 0, 7)] // Same row
    [InlineData(0, 3, 5)] // Same column
    [InlineData(2, 1, 3)] // Same 3x3 square
    public void IsPositionSuitable_ShouldReturnTrue(
        int cordX,
        int cordY,
        int value)
    {
        var board = new Board();
        board.field[0, 0].value = 9;

        var result = board.IsPositionSuitable(cordX, cordY, value);

        result.Should().BeTrue();
    }

    [Fact]
    public void IsSudokuSolved_ShouldReturnTrue()
    {
        var board = new Board();
        board.LoadBoardFrom(ValidSolvedBoardString);
        board.IsSudokuSolved().Should().BeTrue();
    }

    [Theory]
    [InlineData(ValidUnsolvedBoardString)]
    [InlineData(UnvalidSolvedBoardString)]
    [InlineData(UnvalidUnsolvedBoardString)]
    [InlineData(EmptyBoardString)]
    public void IsSudokuSolved_ShouldReturnFalse(string boardString)
    {
        var board = new Board();
        board.LoadBoardFrom(boardString);
        board.IsSudokuSolved().Should().BeFalse();
    }

    [Theory]
    [InlineData(ValidSolvedBoardString)]
    [InlineData(ValidUnsolvedBoardString)]
    [InlineData(EmptyBoardString)]
    public void IsSudokuValid_ShouldReturnTrue(string boardString)
    {
        var board = new Board();
        board.LoadBoardFrom(boardString);
        board.IsSudokuValid().Should().BeTrue();
    }

    [Theory]
    [InlineData(UnvalidSolvedBoardString)]
    [InlineData(UnvalidUnsolvedBoardString)]
    public void IsSudokuValid_ShouldReturnFalse(string boardString)
    {
        var board = new Board();
        board.LoadBoardFrom(boardString);
        board.IsSudokuValid().Should().BeFalse();
    }

    [Fact]
    public void IsCellLocked_ShouldReturnTrue()
    {
        var board = new Board();
        board.field[0, 0].value = 5;
        board.field[0, 0].isLocked = true;

        board.IsCellLocked(0, 0).Should().BeTrue();
    }

    [Fact]
    public void IsCellLocked_ShouldReturnFalse()
    {
        var board = new Board();
        board.field[0, 0].value = 5;

        board.IsCellLocked(0, 0).Should().BeFalse();
    }

    [Fact]
    public void GetCandidates_ShouldReturnAsExpected()
    {
        var board = new Board();
        board.field[0, 0].value = 3;
        board.UpdateCandidates();

        board.GetCandidates(5, 0).Should().BeEquivalentTo([1, 2, 4, 5, 6, 7, 8, 9]);
    }
}
