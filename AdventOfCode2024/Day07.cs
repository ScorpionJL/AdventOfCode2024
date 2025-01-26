
namespace AdventOfCode2024;

internal static class Day07
{
    public static void Solve()
    {
        Console.WriteLine("-- Day 7");

        string inputFile = @"Day7\input.txt";
        if (!File.Exists(inputFile)) inputFile = @"Day7\test-input.txt";

        (long[] values, long expected)[] input = File.ReadLines(inputFile)
            .Select(line => line.Split(":"))
            .Select(parts => (
                parts[1]
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(long.Parse)
                    .ToArray(),
                long.Parse(parts[0])))
            .ToArray();

        var calibrationResult = input
            .Select(GetEquationValue)
            .Sum();
        Console.WriteLine($"Calibration result: {calibrationResult}");

        var concatResult = input
            .Select(GetEquationValueConcat)
            .Sum();
        Console.WriteLine($"Concat result: {concatResult}");
    }

    static long GetEquationValueConcat(this (long[] values, long expected) input) => GetEquationValueConcat(input.values, input.expected);

    static long GetEquationValueConcat(long[] values, long expected) => values
        .Skip(1)
        .Aggregate(
            seed: new HashSet<long> { values[0] },
            func: (acc, value) => ApplyOperationsConcat(acc, value, expected),
            resultSelector: result => result.Contains(expected) ? expected : 0);

    static HashSet<long> ApplyOperationsConcat(IEnumerable<long> state, long value, long max) => state
        .SelectMany<long, long>(r => [r + value, r * value, long.Parse($"{r}{value}")])
        .Where(r => r <= max)
        .ToHashSet();

    static long GetEquationValue(this (long[] values, long expected) input) => GetEquationValue(input.values, input.expected);

    static long GetEquationValue(long[] values, long expected) => values
        .Skip(1)
        .Aggregate(
            seed: new HashSet<long> { values[0] },
            func: (acc, value) => ApplyOperations(acc, value, expected),
            resultSelector: result => result.Contains(expected) ? expected : 0);

    static HashSet<long> ApplyOperations(IEnumerable<long> state, long value, long max) => state
        .SelectMany<long, long>(r => [r + value, r * value])
        .Where(r => r <= max)
        .ToHashSet();
}
