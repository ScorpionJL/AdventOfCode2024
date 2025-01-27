namespace AdventOfCode2024;

using OperatorFunc = Func<long, long, long>;

internal static class Day07
{
    public static void Solve()
    {
        Console.WriteLine("-- Day 7");

        string inputFile = @"Day7\input.txt";
        if (!File.Exists(inputFile)) inputFile = @"Day7\test-input.txt";

        var (sum, concat) = File.ReadLines(inputFile)
            .Select(line => line.Split(":"))
            .Select(parts => (
                parts[1]
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(long.Parse)
                    .ToArray(),
                long.Parse(parts[0])))
            .Select(GetEquationValues)
            .Aggregate(
                (sum: 0L, concat: 0L),
                (acc, result) => (acc.sum + result.Item1, acc.concat + result.Item2));
        Console.WriteLine($"Calibration result: {sum}");
        Console.WriteLine($"Concat result     : {concat}");
    }

    static (long, long) GetEquationValues(this (long[] values, long expected) input) =>
    (
        GetEquationValue(input.values, input.expected, BasicOperators),
        GetEquationValue(input.values, input.expected, AllOperators)
    );

    static long GetEquationValue(long[] values, long expected, OperatorFunc[] operators) => values
        .Skip(1)
        .Aggregate(
            seed: new HashSet<long> { values[0] },
            func: (acc, value) => ApplyOperations(acc, value, expected, operators),
            resultSelector: result => result.Contains(expected) ? expected : 0);

    static HashSet<long> ApplyOperations(IEnumerable<long> state, long value, long max, OperatorFunc[] operations) => state
        .SelectMany(r => operations.Select(op => op(r, value)).ToArray())
        .Where(r => r <= max)
        .ToHashSet();


    static long AddOperator(long a, long b) => a + b;
    static long MultiplyOperator(long a, long b) => a * b;
    static long ConcatOperator(long a, long b) => long.Parse($"{a}{b}");

    static readonly OperatorFunc[] BasicOperators = [AddOperator, MultiplyOperator];
    static readonly OperatorFunc[] AllOperators = [AddOperator, MultiplyOperator, ConcatOperator];
}