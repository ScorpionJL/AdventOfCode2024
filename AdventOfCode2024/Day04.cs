using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

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
    }


    private static int GetWordCount(this string[] grid, string word)
    {
        var wordArray = word.ToCharArray();
        var reversedWordArray = wordArray.Reverse().ToArray();
        return GridCoordinates(grid.Length, grid[0].Length).Sum(coord =>
            grid.FindWordAtPosition(coord.row, coord.col, wordArray) +
            grid.FindWordAtPosition(coord.row, coord.col, reversedWordArray));
    }

    private static IEnumerable<(int row, int col)> GridCoordinates(int rows, int cols) =>
        Enumerable.Range(0, rows).SelectMany(row =>
        Enumerable.Range(0, cols), (row, col) => (row, col));

    private static int FindWordAtPosition(this string[] grid, int row, int col, char[] word) =>
        grid[row][col] == word[0]   // only check for the word if the first letter matches
        ? Directions.Sum(direction => grid.FindWord(row, col, direction, word) ? 1 : 0)
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
            if (row < 0 || row >= lastRow || col < 0 || col >= lastCol) { return false; }

            if (word[i] != grid[row][col]) { return false; }
        }

        return true;
    }


    private record struct SearchDirection(int X, int Y);
    private static readonly SearchDirection[] Directions =
    [
        new(0, 1),    // east
        new(1, 1),    // south-east
        new(1, 0),    // south
        new(1, -1),   // south-west
    ];

    private record struct SearchDiagnals(SearchDirection Top, SearchDirection Bottom);
    private static SearchDirection TopLeft = new(-1, -1);
    private static SearchDirection TopRight = new(-1, +1);
    private static SearchDirection BottomLeft = new(+1, -1);
    private static SearchDirection BottomRight = new(+1, +1);
    private static readonly SearchDiagnals[] Diagnals =
    [
        new(TopLeft, BottomRight),  // top-left to bottom-right
        new(TopRight, BottomLeft),  // top-right to bottom-left
    ];

    private static string ToDirectionName(this SearchDirection self)
    {
        return self switch
        {
            (0, 1) => "east",
            (1, 1) => "south-east",
            (1, 0) => "south",
            (1, -1) => "south-west",
            _ => "unknown"
        };
    }


    public static string ReverseString(this string text) => string.Create(text.Length, text, (chars, text) =>
    {
        text.AsSpan().CopyTo(chars);
        chars.Reverse();
    });
}
