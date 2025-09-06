using FluentAssertions;

namespace SudokuNet.Tests;

public class SudokuTests
{
    private const string ValidSolvedBoardString = "281453697473962815596718243145236789367589124829147356614325978732894561958671432";
    private const string ValidUnsolvedBoardString = "081450697473000815506718203045206709307589024829140056614305978032090561058671430"; // 20 empty cells
    private const string EmptyBoardString = "000000000000000000000000000000000000000000000000000000000000000000000000000000000";

    [Fact]
    public void GeneratePuzzle_ShouldGenerateValidBoardWithSpecifiedClues()
    {
        for (int clues = 0; clues < 81; clues++)
        {
            var board = Sudoku.GeneratePuzzle(clues);

            board.EmptyCellCount.Should().Be(81 - clues);
            board.IsSudokuValid().Should().BeTrue();
        }
    }

    [Theory]
    [InlineData(ValidSolvedBoardString)]
    [InlineData(ValidUnsolvedBoardString)]
    [InlineData(EmptyBoardString)]
    public void TrySolve_ShouldSolveValidBoard(string boardString)
    {
        var board = new Board();
        board.LoadBoardFrom(boardString);
        var result = Sudoku.TrySolve(board, out var solvedBoard, 100);
        result.Should().BeTrue();
        solvedBoard.IsSudokuSolved().Should().BeTrue();
    }
}
