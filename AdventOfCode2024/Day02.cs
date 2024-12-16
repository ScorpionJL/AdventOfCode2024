namespace AdventOfCode2024;

internal static class Day02
{
    public static void Solve(bool debug = false)
    {
        Console.WriteLine("-- Day 2");

        string inputFile = @"Day2\input.txt";
        if (!File.Exists(inputFile)) inputFile = @"Day2\test-input.txt";

        var reports = File.ReadLines(inputFile)     /* lines */
            .Select(SplitLine)                      /* line parts */
            .Select(ToLevels)                       /* reports */
            .Where(levels => levels.Length > 0)     
            .ToArray();                             /* save in memory */

        /* part 1 - safe reports */
        if (debug) { reports.ShowReportStatus(); }
        var safeReports = reports.Count(IsReportSafe);
        Console.WriteLine($"Safe reports: {safeReports}");

        /* part 2 - safe dampended reports */
        var safeDampendedReports = reports.Count(IsReportSafeDampended);
        Console.WriteLine($"Safe dampended reports: {safeDampendedReports}");
    }


    private static string[] SplitLine(string line) =>
        line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    private static int[] ToLevels(this string[] list)
    {
        // we expect the input to contain only integers, so size accordingly
        int[] result = new int[list.Length];

        for (int i = 0; i < list.Length; i++)
        {
            // add to the int array; return if parsing fails
            if (!int.TryParse(list[i], out result[i])) { return []; }
        }

        return result;
    }


    private static void ShowReportStatus(this int[][] reports)
    {
        foreach (var report in reports)
        {
            Console.WriteLine($"Report: {string.Join(", ", report)} is safe: {IsReportSafe(report)}");
        }
    }


    private static bool IsReportSafe(int[] report) =>
        report.Length == 0 ? false :
        report.Length == 1 ? true :
        report.IsReportSafe(Math.Sign(report[1] - report[0]));

    private static bool IsReportSafe(this int[] report, int expectedSign)
    {
        for (int i = 1; i < report.Length; i++)
        {
            if (!AreLevelsSafe(report[i], report[i - 1], expectedSign)) return false;
        }
        return true;
    }

    private static bool AreLevelsSafe(int a, int b, int expectedSign) =>
        Math.Abs(a - b) is >= 1 and <= 3 &&
        Math.Sign(a - b) == expectedSign;

    private static bool IsReportSafeDampended(int[] report)
    {
        for (int i = 0; i < report.Length; i++)
        {
            if (IsReportSafe([.. report[0..i], .. report[(i + 1)..]])) return true;
        }
        return false;
    }
}
