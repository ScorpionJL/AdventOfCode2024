namespace AdventOfCode2024;

using System;
using System.Dynamic;
using Coordinate = (int row, int col);

internal static class Day08
{
    public static void Solve()
    {
        Console.WriteLine("-- Day 8");

        string inputFile = @"Day8\input.txt";
        if (!File.Exists(inputFile)) inputFile = @"Day8\test-input.txt";

        var grid = File.ReadLines(inputFile).ToArray();
        var rows = grid.Length;
        var cols = grid[0].Length;

        Dictionary<char, Coordinate[]> antennas = grid
            .SelectMany((line, row) => line.Select((symbol, col) => (symbol, coord: (row, col))))
            .Where(e => e.symbol != '.')
            .GroupBy(e => e.symbol, e => e.coord)
            .ToDictionary(group => group.Key, group => group.ToArray());

        var antiNodes = antennas                                            /*  */
            .SelectMany(antenna => antenna.Value                            /* foreach antenna - get the origin coordinates */
                .SelectMany(origin => antenna.Value                         /* foreach origin - get the target coordinates */
                    .Where(target => target != origin)                      /* where the target coordinate is not the origin  */
                    .Select(target => origin.MirrorCoordinate(target))      /* mirror the target coordinate */
                    .Where(anti => anti.IsValidCoordinate(rows, cols))      /* where the coordinate is valid */
                )
            );
        Console.WriteLine($"Anti-nodes: {antiNodes.ToHashSet().Count}");

        HashSet<Coordinate> harmonicNodes = GetHarmonicNodes(antennas, rows, cols);
        Console.WriteLine($"Harmonic nodes: {harmonicNodes.Count}"); /* 34:1352 */
    }

    private static HashSet<Coordinate> GetHarmonicNodes(Dictionary<char, Coordinate[]> antennas, int maxRows, int maxCols)
    {
        HashSet<Coordinate> result = [];
        foreach (var antenna in antennas)
        {
            for (var i = 0; i < antenna.Value.Length; i++)
            {
                var origin = antenna.Value[i];
                for (var j = i + 1; j < antenna.Value.Length; j++)
                {
                    var target = antenna.Value[j];
                    foreach (var node in GetNodesInLine(origin, target, maxRows, maxCols))
                    {
                        result.Add(node);
                    }
                }
            }
        }
        return result;
    }

    private static IEnumerable<Coordinate> GetNodesInLine(Coordinate origin, Coordinate target, int maxRows, int maxcols)
    {
        var (rows, cols) = origin.GetOffset(target);

        // towards target
        for (var node = origin; node.IsValidCoordinate(maxRows, maxcols); node = node.ApplyOffset(rows, cols))
        {
            yield return node;
        }

        // away from target
        for (var node = target; node.IsValidCoordinate(maxRows, maxcols); node = node.ApplyOffset(-rows, -cols))
        {
            yield return node;
        }
    }

    private static (int rows, int cols) GetOffset(this Coordinate origin, Coordinate target) =>
        (target.row - origin.row, target.col - origin.col);

    private static Coordinate ApplyOffset(this Coordinate coord, int rows, int cols) =>
        (coord.row + rows, coord.col + cols);

    private static bool IsValidCoordinate(this Coordinate coord, int maxRows, int maxCols) =>
        coord.row >= 0 && coord.row < maxRows && coord.col >= 0 && coord.col < maxCols;

    private static Coordinate MirrorCoordinate(this Coordinate origin, Coordinate target) =>
    (
        origin.row - (target.row - origin.row),
        origin.col - (target.col - origin.col)
    );
}