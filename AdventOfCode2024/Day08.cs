namespace AdventOfCode2024;

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
            )
            .ToHashSet();
        Console.WriteLine($"Anti-nodes: {antiNodes.Count}");
    }

    private static bool IsValidCoordinate(this Coordinate coord, int rows, int cols) =>
        coord.row >= 0 && coord.row < rows && coord.col >= 0 && coord.col < cols;

    private static Coordinate MirrorCoordinate(this Coordinate origin, Coordinate target) =>
    (
        origin.row - (target.row - origin.row),
        origin.col - (target.col - origin.col)
    );


}