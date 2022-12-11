string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
string file = Path.Combine(currentDirectory, "../../../input.txt");
string path = Path.GetFullPath(file);
string[] text = File.ReadAllText(path).Split("\n");