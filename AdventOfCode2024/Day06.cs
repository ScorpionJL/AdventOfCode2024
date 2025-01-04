using Coordinate = (int row, int col);

namespace AdventOfCode2024;

internal static class Day06
{
    public static void Solve()
    {
        Console.WriteLine("-- Day 6");

        string inputFile = @"Day6\input.txt";
        if (!File.Exists(inputFile)) inputFile = @"Day6\test-input.txt";

        char[] guardSymbols = ['^', '>', 'v', '<'];
        var grid = File.ReadLines(inputFile).ToArray();
        var guard = Common.GridCoordinates(grid.Length, grid[0].Length)
            .Where(coord => coord.IsGuard(grid))
            .Select(coord => new Guard(grid[coord.row][coord.col], coord))
            .First();

        HashSet<Coordinate> visited = [guard.Position, .. grid.Traverse(guard)];
        Console.WriteLine($"Visited: {visited.Count}");

        var obstacleOptions = visited
            .Where(position => grid.TraverseWithNewObstacle(guard, position))
            .Count();
        Console.WriteLine($"Obstacle options: {obstacleOptions}");
    }


    private static IEnumerable<Coordinate> Traverse(this string[] grid, Guard guard)
    {
        var rows = grid.Length;
        var cols = grid[0].Length;

        while (true)
        {
            var nextPosition = guard.NextPosition;
            if (nextPosition.IsExit(rows, cols)) { yield break; }
            else if (nextPosition.IsObstacle(grid)) { guard.Turn(); }
            else { yield return guard.MoveTo(nextPosition); }
        }
    }

    private static bool IsGuard(this Coordinate position, string[] grid) =>
        grid[position.row][position.col] == '^' || grid[position.row][position.col] == '>' ||
        grid[position.row][position.col] == 'v' || grid[position.row][position.col] == '<';

    private static bool IsObstacle(this Coordinate position, string[] grid) =>
        grid[position.row][position.col] == '#';

    private static bool IsExit(this Coordinate position, int rows, int cols) =>
        position.row < 0 || position.row >= rows || position.col < 0 || position.col >= cols;


    private static bool TraverseWithNewObstacle(this string[] grid, Guard guard, Coordinate newObstacle)
    {
        var rows = grid.Length;
        var cols = grid[0].Length;

        HashSet<Guard> guardPath = [guard];

        while (true)
        {
            var nextPosition = guard.NextPosition;
            if (nextPosition.IsExit(rows, cols)) { return false; }

            if (nextPosition.IsObstacle(grid) || nextPosition == newObstacle) { guard.Turn(); }
            else { _ = guard.MoveTo(nextPosition); }

            if (!guardPath.Add(guard)) return true;
        }
    }


    private record struct Guard(char Direction, Coordinate Position)
    {
        public Coordinate MoveTo(Coordinate position) => Position = position;

        public readonly Coordinate NextPosition => Direction switch
        {
            '^' => (Position.row - 1, Position.col),
            'v' => (Position.row + 1, Position.col),
            '<' => (Position.row, Position.col - 1),
            '>' => (Position.row, Position.col + 1),
            _ => throw new InvalidOperationException("Invalid guard direction")
        };

        public void Turn() => Direction = Direction switch
        {
            '^' => '>',
            '>' => 'v',
            'v' => '<',
            '<' => '^',
            _ => throw new InvalidOperationException("Invalid guard direction")
        };
    }
}
