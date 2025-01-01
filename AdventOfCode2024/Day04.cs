using CharPair = (char top, char bottom);

namespace AdventOfCode2024;

internal static class Day04

{
    public static void Solve()
    {
        Console.WriteLine("-- Day 4");

        string inputFile = @"Day4\input.txt";
        if (!File.Exists(inputFile)) inputFile = @"Day4\test-input.txt";

        var grid = File.ReadLines(inputFile).ToArray();


        int count = grid.GetWordCount("XMAS");
        Console.WriteLine($"Word count: {count}");

        int xCount = grid.GetXWordCount();
        Console.WriteLine($"X Word count: {xCount}");
    }


    private static int GetWordCount(this string[] grid, string word)
    {
        var wordArray = word.ToCharArray();
        var reversedWordArray = wordArray.Reverse().ToArray();
        return GridCoordinates(grid.Length, grid[0].Length).Sum(coord =>
            grid.FindWordAtPosition(coord.row, coord.col, wordArray) +
            grid.FindWordAtPosition(coord.row, coord.col, reversedWordArray));
    }

    private static int FindWordAtPosition(this string[] grid, int row, int col, char[] word) =>
        grid[row][col] == word[0]   // only check for the word if the first letter matches
        ? SearchDirections.Sum(direction => grid.FindWord(row, col, direction, word) ? 1 : 0)
        : 0;

    private static bool FindWord(this string[] grid, int row, int col, SearchDirection direction, char[] word)
    {
        var lastRow = grid.Length;
        var lastCol = grid[0].Length;

        // start from 1 since we already know the first letter matches
        for (var i = 1; i < word.Length; i++)
        {
            row += direction.X;
            col += direction.Y;
            if (row >= lastRow || col < 0 || col >= lastCol) { return false; }

            if (word[i] != grid[row][col]) { return false; }
        }

        return true;
    }


    private static int GetXWordCount(this string[] grid) => 
        GridCoordinates(grid.Length - 2, grid[0].Length - 2, 1, 1)
        .Where(coord => grid[coord.row][coord.col] == 'A')
        .Count(coord => grid.GetDiagnalChars(coord.row, coord.col).All(IsValidDiagnalPair));

    private static CharPair[] GetDiagnalChars(this string[] grid, int row, int col) =>
    [
        grid.LeftRightDiagnalChars(row, col),
        grid.RightLeftDiagnalChars(row, col)
    ];
    private static CharPair LeftRightDiagnalChars(this string[] grid, int row, int col) =>
    (
        grid[row - 1][col - 1], // top-left
        grid[row + 1][col + 1]  // bottom-right
    );
    private static CharPair RightLeftDiagnalChars(this string[] grid, int row, int col) =>
    (
        grid[row - 1][col + 1], // top-right
        grid[row + 1][col - 1]  // bottom-left  
    );

    private static bool IsValidDiagnalPair(this CharPair chars) => chars switch
    {
        ('M', 'S') => true,
        ('S', 'M') => true,
        _ => false
    };


    private static IEnumerable<(int row, int col)> GridCoordinates(int rows, int cols, int rowStart = 0, int colStart = 0) =>
        Enumerable.Range(rowStart, rows).SelectMany(row =>
        Enumerable.Range(colStart, cols), (row, col) => (row, col));

    private record struct SearchDirection(int X, int Y);
    private static readonly SearchDirection[] SearchDirections =
    [
        new(0, 1),    // east
        new(1, 1),    // south-east
        new(1, 0),    // south
        new(1, -1),   // south-west
    ];
}
