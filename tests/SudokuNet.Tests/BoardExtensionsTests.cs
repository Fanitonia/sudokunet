using FluentAssertions;

namespace SudokuNet.Tests;

public class BoardExtensionsTests
{
    [Fact]
    public void Clone_ShouldCreateDeepCopy()
    {
        var board = new Board();
        board.field[0, 3].value = 5;
        board.field[0, 3].isLocked = true;

        var clonedBoard = board.Clone();
        clonedBoard.field[0, 0].value = 3;

        board.field[0, 3].value.Should().Be(5);
        board.field[0, 3].isLocked.Should().BeTrue();
        board.field[0, 0].value.Should().Be(0);

        clonedBoard.field[0, 0].value.Should().Be(3);
        clonedBoard.field[0, 0].isLocked.Should().BeFalse();
        clonedBoard.field[0, 3].value.Should().Be(5);
        clonedBoard.field[0, 3].isLocked.Should().BeTrue();
    }
}