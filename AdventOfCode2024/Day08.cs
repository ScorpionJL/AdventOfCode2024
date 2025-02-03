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

        /* get the list of coordinates for each antenna type */
        var antennas = GetAntennaList(grid);

        var antiNodes = antennas.SelectMany(antenna => GetAntiNodes(antenna.Value, rows, cols));
        Console.WriteLine($"Anti-nodes: {antiNodes.ToHashSet().Count}");  /* 34:1352 */

        var harmonicNodes = antennas.SelectMany(antenna => GetHarmonicNodes(antenna.Value, rows, cols));
        Console.WriteLine($"Harmonic nodes: {harmonicNodes.ToHashSet().Count}"); /* 34:1352 */
    }


    private static Dictionary<char, (int row, int col)[]> GetAntennaList(string[] grid) => grid
        .SelectMany((line, row) => line.Select((symbol, col) => (symbol, coord: (row, col))))
        .Where(e => e.symbol != '.')
        .GroupBy(e => e.symbol, e => e.coord)
        .ToDictionary(group => group.Key, group => group.ToArray());


    private static IEnumerable<(int row, int col)> GetAntiNodes(Coordinate[] antennaCoords, int maxRows, int maxCols) => antennaCoords
        .SelectMany(origin => antennaCoords                         /* foreach origin - get the target coordinates[] */
            .Where(target => target != origin)                      /* where the target coordinate is not the origin  */
            .Select(target => origin.MirrorCoordinate(target))      /* mirror the target coordinate */
            .Where(anti => anti.IsValidCoordinate(maxRows, maxCols))      /* where the coordinate is valid */
        );

    private static Coordinate MirrorCoordinate(this Coordinate origin, Coordinate target) =>
    (
        origin.row - (target.row - origin.row),
        origin.col - (target.col - origin.col)
    );


    private static IEnumerable<Coordinate> GetHarmonicNodes(Coordinate[] antennaCoords, int maxRows, int maxCols) => antennaCoords
        .SelectMany((origin, index) => antennaCoords
            .Skip(index + 1)
            .SelectMany(target => GetNodesInLine(origin, target, maxRows, maxCols)));

    private static IEnumerable<Coordinate> GetNodesInLine(Coordinate origin, Coordinate target, int maxRows, int maxcols)
    {
        var (rows, cols) = origin.GetOffset(target);

        // towards target
        for (var node = origin; node.IsValidCoordinate(maxRows, maxcols); node = node.ApplyOffset(rows, cols)) { yield return node; }

        // away from target
        for (var node = target; node.IsValidCoordinate(maxRows, maxcols); node = node.ApplyOffset(-rows, -cols)) { yield return node; }
    }

    private static (int rows, int cols) GetOffset(this Coordinate origin, Coordinate target) =>
        (target.row - origin.row, target.col - origin.col);

    private static Coordinate ApplyOffset(this Coordinate coord, int rows, int cols) =>
        (coord.row + rows, coord.col + cols);


    private static bool IsValidCoordinate(this Coordinate coord, int maxRows, int maxCols) =>
        coord.row >= 0 && coord.row < maxRows && coord.col >= 0 && coord.col < maxCols;
}