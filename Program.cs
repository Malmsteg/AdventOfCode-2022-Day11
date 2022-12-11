string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
string file = Path.Combine(currentDirectory, "../../../test.txt");
string path = Path.GetFullPath(file);
string[] text = File.ReadAllText(path).Split("\n");

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
        monkey.Items.Add(Convert.ToInt32(items[j]));
    }
    string[] operation = text[++i].Trim().Split()[4..];
    if (operation[0].Equals("*")) // multiply
    {
        monkey.AdditionOp = 0;
        if (operation[1].Equals("old"))
        {
            monkey.MultiOp = -1;
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
}

//release the monkies
for (int turn = 1; turn <= 20; turn++)
{

}


public class Monkey
{
    public void Operation()
    {
        if (AdditionOp == 0) // Multiply
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (MultiOp == -1)
                {
                    Items[i] *= Items[i];
                }
                else
                {
                    Items[i] *= MultiOp;
                }
            }
        }
        else
        {
            // Add
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i] += AdditionOp;
            }
        }
    }
    public Dictionary<int, List<int>> TestItems()
    {
        Dictionary<int, List<int>> result = new()
        {
            { Test[1], new List<int>() },
            { Test[2], new List<int>() }
        };
        foreach (int item in Items)
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
    public List<int> Items { get; set; } = new List<int>();
    public int AdditionOp { get; set; }
    public int[] Test { get; set; } = new int[3];
    public int MultiOp { get; set; }
    public int ItemsInspected { get; set; }
}