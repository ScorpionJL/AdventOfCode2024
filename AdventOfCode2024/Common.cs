namespace AdventOfCode2024;

internal static class Common
{
    public static IEnumerable<(int row, int col)> GridCoordinates(int rows, int cols, int rowStart = 0, int colStart = 0) =>
        Enumerable.Range(rowStart, rows).SelectMany(row =>
        Enumerable.Range(colStart, cols), (row, col) => (row, col));
}
