namespace AdventOfCode2024;

internal static class Day01
{
    public static void Solve()
    {
        string inputFile = @"Day1\input.txt";
        if (!File.Exists(inputFile)) inputFile = @"Day1\test-input.txt";

        (List<int> first, List<int> second) lists = File.ReadLines(inputFile)
            .Select(SplitLine)
            .Select(ToIntPairs)
            .WhereNotNull()
            .ToLists()
            .SortLists();

        /* part 1 */
        var totalDistance = lists.CalcualteTotalDistance();
        Console.WriteLine($"Total Distance: {totalDistance}");

        /* part 2 */
        var secondCounts = lists.second.CountBy(i => i)
            .ToDictionary(pair => pair.Key, pair => pair.Value);

        var totalSimilarity = lists.first
            .Where(i => secondCounts.ContainsKey(i))
            .Sum(i => i * secondCounts[i]);

        Console.WriteLine($"similarity score: {totalSimilarity}");
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

    private static (List<int> first, List<int> second) ToLists(this IEnumerable<(int, int)> list)
    {
        (List<int> first, List<int> second) = ([], []);
        foreach (var (a, b) in list)
        {
            first.Add(a);
            second.Add(b);
        }

        return (first, second);
    }

    private static (List<int> first, List<int> second) SortLists(this (List<int> first, List<int> second) lists)
    {
        lists.first.Sort(); 
        lists.second.Sort();
        return (lists.first, lists.second);
    }

    private static int CalcualteTotalDistance(this (List<int> first, List<int> second) list) =>
        list.first.Zip(list.second, (a, b) => Math.Abs(a - b)).Sum();
}
