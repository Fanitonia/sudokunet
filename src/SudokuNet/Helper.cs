namespace SudokuNet;

internal static class Helper
{
    internal static bool IsCordValid(int cordX, int cordY)
    {
        if (cordX > 8 || cordX < 0)
            return false;

        if(cordY > 8 || cordY < 0)
            return false;

        return true;

    }
}
