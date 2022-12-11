string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
string file = Path.Combine(currentDirectory, "../../../input.txt");
string path = Path.GetFullPath(file);
string[] text = File.ReadAllText(path).Split("\n");

// const int ROUNDS = 20; // Part 1
const int ROUNDS = 10000; // Part 2
int modolo = 1;

List<Monkey> monkeys = new();
//Start the monkies up
for (int i = 0; i < text.Length; i++)
{
    if (string.IsNullOrEmpty(text[i]))
    { continue; }
    Monkey monkey = new();
    string[] items = text[++i].Replace(",", "").Trim().Split()[2..];
    for (int j = 0; j < items.Length; j++)
    {
        monkey.Items.Add(Convert.ToUInt64(items[j]));
    }
    string[] operation = text[++i].Trim().Split()[4..];
    if (operation[0].Equals("*")) // multiply
    {
        monkey.AdditionOp = 0;
        if (operation[1].Equals("old"))
        {
            monkey.MultiOp = 0;
        }
        else
        {
            monkey.MultiOp = Convert.ToInt32(operation[1]);
        }
    }
    else
    {
        monkey.MultiOp = 0;
        monkey.AdditionOp = Convert.ToInt32(operation[1]);
    }
    monkey.Test[0] = Convert.ToInt32(text[++i].Trim().Split()[^1]);
    monkey.Test[1] = Convert.ToInt32(text[++i].Trim().Split()[^1]);
    monkey.Test[2] = Convert.ToInt32(text[++i].Trim().Split()[^1]);
    monkeys.Add(monkey);
}

//calculate the modolo
foreach (var monkey in monkeys)
{
    modolo *= monkey.Test[0];
}

//release the monkies
for (int round = 1; round <= ROUNDS; round++)
{
    foreach (var monkey in monkeys)
    {
        if (!monkey.Items.Any())
        {
            continue;
        }
        var items = monkey.TestItems(modolo);
        monkeys[(int)items.ElementAt(0).Key].Items.AddRange(items.ElementAt(0).Value);
        monkeys[(int)items.ElementAt(1).Key].Items.AddRange(items.ElementAt(1).Value);
    }
    Console.WriteLine($"Round {round} done");
}

List<int> monkeyBusiness = new();

foreach (var monkey in monkeys)
{
    monkeyBusiness.Add(monkey.ItemsInspected);
}

monkeyBusiness.Sort();

Console.WriteLine($"Monkey business is {(Double)monkeyBusiness[^1] * (Double)monkeyBusiness[^2]}");

public class Monkey
{
    public void Operation(int modolo)
    {
        if (AdditionOp == 0) // Multiply
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i] %= modolo;
                if (MultiOp == 0)
                {
                    Items[i] *= Items[i];
                }
                else
                {
                    Items[i] *= MultiOp;
                }
                // Items[i] /= 3; Part 2 no longer divides this
            }
        }
        else
        {
            // Add
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i] %= modolo;
                Items[i] += AdditionOp;
                // Items[i] /= 3; Part 2 no longer divides this
            }
        }
    }
    public Dictionary<Double, List<Double>> TestItems(int modolo)
    {
        Operation(modolo);
        Dictionary<Double, List<Double>> result = new()
        {
            { Test[1], new List<Double>() },
            { Test[2], new List<Double>() }
        };
        foreach (Double item in Items)
        {
            ItemsInspected++;
            if (item % Test[0] == 0)
            {
                result[Test[1]].Add(item);
            }
            else
            {
                result[Test[2]].Add(item);
            }
        }
        Items = new();
        return result;
    }
    public List<Double> Items { get; set; } = new List<Double>();
    public int AdditionOp { get; set; }
    public int[] Test { get; set; } = new int[3];
    public int MultiOp { get; set; }
    public int ItemsInspected { get; set; }
}