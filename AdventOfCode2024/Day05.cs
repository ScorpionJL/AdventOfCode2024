namespace AdventOfCode2024;

// page number -> set of pages that must be printed before it
using PageOrderRules = Dictionary<int, HashSet<int>>;

internal static class Day05
{
    public static void Solve()
    {
        Console.WriteLine("-- Day 5");

        string inputFile = @"Day5\input.txt";
        if (!File.Exists(inputFile)) inputFile = @"Day5\test-input.txt";

        PageOrderRules pageOrderRules = [];  
        List<int[]> pageUpdates = [];
        ReadInputFile(inputFile, pageOrderRules, pageUpdates);

        var validPageTotal = pageUpdates
            .Where(pages => pages.AllPagesInCorrectOrder(pageOrderRules))
            .Sum(pages => pages[pages.Length / 2]);
        Console.WriteLine($"Valid page total: {validPageTotal}");

        var correctedPageTotal = pageUpdates
            .Where(pages => pages.AnyPagesInWrongOrder(pageOrderRules))
            .Select(pages => pages.SortPages(pageOrderRules))
            .Sum(pages => pages[pages.Length / 2]);
        Console.WriteLine($"Corrected page total: {correctedPageTotal}");
    }


    private static void ReadInputFile(string inputFile, PageOrderRules pageOrderRules, List<int[]> pageUpdates)
    {
        var rulesMode = true;
        foreach (var line in File.ReadLines(inputFile))
        {
            if (line == string.Empty) { rulesMode = false; }
            else if (rulesMode) { pageOrderRules.AddRule(line); }
            else { pageUpdates.Add(SplitPageUpdates(line)); }
        }
    }

    private static void AddRule(this PageOrderRules orderRules, string line)
    {
        var span = line.AsSpan();

        var delimiter = span.IndexOf("|");
        if (delimiter == -1) { return; }

        var earlierPage = int.Parse(span[..delimiter]);
        var laterPage = int.Parse(span[(delimiter + 1)..]);

        if (!orderRules.TryGetValue(laterPage, out var value)) { value = ([]); orderRules[laterPage] = value; }
        value.Add(earlierPage);
    }

    private static int[] SplitPageUpdates(string line) => line
        .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(page => int.Parse(page))
        .ToArray();

    
    private static bool AllPagesInCorrectOrder(this int[] pages, PageOrderRules pageOrderRules) => 
        !pages.AnyPagesInWrongOrder(pageOrderRules);

    private static bool AnyPagesInWrongOrder(this int[] pages, PageOrderRules pageOrderRules) => pages
        .Select((page, index) => (page, remainingPages: pages[(index + 1)..]))
        .Any(pageList => pageList.remainingPages.AnyThatMustPrintBefore(pageList.page, pageOrderRules));

    private static bool AnyThatMustPrintBefore(this int[] remainingPages, int page, PageOrderRules pageOrderRules) => 
        remainingPages.Any(remainingPage => pageOrderRules.GetValueOrDefault(page, []).Contains(remainingPage));


    private static int[] SortPages(this int[] pages, PageOrderRules pageOrderRules)
    {
        var pageList = pages.ToList();
        pageList.Sort((a, b) => (a == b) ? 0 : pageOrderRules.GetValueOrDefault(a, []).Contains(b) ? 1 : -1);
        return [.. pageList];
    }
}
