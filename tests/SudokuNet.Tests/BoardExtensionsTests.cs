using FluentAssertions;

namespace SudokuNet.Tests;

public class BoardExtensionsTests
{
    private const string ValidSolvedBoardString = "281453697473962815596718243145236789367589124829147356614325978732894561958671432";
    private const string ValidUnsolvedBoardString = "081450697473000815506718203145236709367589024829140356614325978732890561958671430";
    private const string UnvalidSolvedBoardString = "281453687473962815596718243145236789367589124839147356614325978732894561958671432";
    private const string UnvalidUnsolvedBoardString = "081850697473000815506718203145236709367589024829140356614325978732890561958671430";
    private const string EmptyBoardString = "000000000000000000000000000000000000000000000000000000000000000000000000000000000";

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
        board.SetCell(0, 0, 9);

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
        board.SetCell(0, 0, 9);

        var result = board.IsPositionSuitable(cordX, cordY, value);

        result.Should().BeTrue();
    }

    [Fact]
    public void IsSudokuSolved_ShouldReturnTrue()
    {
        var board = new Board();
        board.LoadFromString(ValidSolvedBoardString);
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
        board.LoadFromString(boardString);
        board.IsSudokuSolved().Should().BeFalse();
    }

    [Theory]
    [InlineData(ValidSolvedBoardString)]
    [InlineData(ValidUnsolvedBoardString)]
    [InlineData(EmptyBoardString)]
    public void IsSudokuValid_ShouldReturnTrue(string boardString)
    {
        var board = new Board();
        board.LoadFromString(boardString);
        board.IsSudokuValid().Should().BeTrue();
    }

    [Theory]
    [InlineData(UnvalidSolvedBoardString)]
    [InlineData(UnvalidUnsolvedBoardString)]
    public void IsSudokuValid_ShouldReturnFalse(string boardString)
    {
        var board = new Board();
        board.LoadFromString(boardString);
        board.IsSudokuValid().Should().BeFalse();
    }

    [Fact]
    public void IsCellLocked_ShouldReturnTrue()
    {
        var board = new Board();
        board.SetCell(0, 0, 5);
        board.Lock(0, 0);

        board.IsCellLocked(0, 0).Should().BeTrue();
    }

    [Fact]
    public void IsCellLocked_ShouldReturnFalse()
    {
        var board = new Board();
        board.SetCell(0, 0, 5);

        board.IsCellLocked(0, 0).Should().BeFalse();
    }
}