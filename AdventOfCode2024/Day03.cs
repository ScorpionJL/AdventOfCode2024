using System.Text.RegularExpressions;

namespace AdventOfCode2024;

internal static class Day03
{
    public static void Solve()
    {
        Console.WriteLine("-- Day 3");

        string inputFile = @"Day3\input.txt";
        if (!File.Exists(inputFile)) inputFile = @"Day3\test-input.txt";

        var input = File.ReadAllText(inputFile);

        var total = input
            .SelectValidMulInstructions()
            .Sum(ExecuteInstruction);
        Console.WriteLine($"Total: {total}");

        var conditionalTotal = input
            .SplitOnEnabledInstruction()
            .Select(RemoveDisabledInstructions)
            .SelectMany(SelectValidMulInstructions)
            .Sum(ExecuteInstruction);
        Console.WriteLine($"Conditional Total: {conditionalTotal}");
    }

    private static string[] SelectValidMulInstructions(this string input) =>
        Regex.Matches(input, @"mul\((\d{1,3}),(\d{1,3})\)").Select(m => m.Value).ToArray();

    private static int ExecuteInstruction(string instruction)
    {
        var instructionSpan = instruction.AsSpan();
        var comma = instructionSpan.IndexOf(',');
        var firstNumber = int.Parse(instructionSpan.Slice(4, comma - 4));
        var secondNumber = int.Parse(instructionSpan.Slice(comma + 1, instructionSpan.Length - comma - 2));
        return firstNumber * secondNumber;
    }
    
    private static string[] SplitOnEnabledInstruction(this string input) => 
        input.Split("do()", StringSplitOptions.RemoveEmptyEntries);

    private static string RemoveDisabledInstructions(string input) =>
        input.IndexOf("don't()") switch
        {
            -1 => input,
            var index => input.Substring(0, index)
        };
}
