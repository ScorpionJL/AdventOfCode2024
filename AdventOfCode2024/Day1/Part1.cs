namespace AdventOfCode2024.Day1;

public static class Part1
{
    /* Pair up the smallest number in the left list with the smallest number in the right list, 
     * then the second-smallest left number with the second-smallest right number, and so on.
     * 
     * Within each pair, figure out how far apart the two numbers are; 
     * you'll need to add up all of those distances. */
    public static void Solve()
    {
        string inputFile = @"Day1\test-input.txt";

        var totalDistance = File.ReadLines(inputFile)
            .Select(SplitLine)
            .Select(ToIntPairs)
            .WhereNotNull()
            .ToSortedList()
            .CalcualteTotalDistance();
        Console.WriteLine($"Total Distance: {totalDistance}");
    }

    private static string[] SplitLine(string line) =>
        line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    private static (int, int)? ToIntPairs(this string[] list) =>
        list.Length != 2 ||
        !int.TryParse(list[0], out int a) ||
        !int.TryParse(list[1], out int b)
        ? null : (a, b);

    private static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source) where T : struct =>
        source.Where(e => e.HasValue).Select(e => e!.Value);

    private static (List<int> first, List<int> second) ToSortedList(this IEnumerable<(int, int)> list)
    {
        (List<int> first, List<int> second) = ([], []);
        foreach (var (a, b) in list)
        {
            first.Add(a);
            second.Add(b);
        }

        first.Sort();
        second.Sort();
        return (first, second);
    }

    private static int CalcualteTotalDistance(this (List<int> first, List<int> second) list) =>
        list.first.Zip(list.second, (a, b) => Math.Abs(a - b)).Sum();
}
