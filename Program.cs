namespace Dictionary
{
    using MyDictionary = Dictionary<string, string>;

    internal static class Program
    {
        private static readonly MyDictionary Dictionary = new();
        private const string FilePath = "Dictionary.txt";
        private const string Separator = ":";
        private const string ConfirmLiteral = "y";

        private static void LoadDictionary()
        {
            Dictionary.Clear();

            if (!File.Exists(FilePath))
            {
                File.Create(FilePath).Close();
                Console.WriteLine($"File {FilePath} created");
                return;
            }

            using StreamReader sr = new(FilePath);
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] tokens = line.Split(Separator, 2);
                if (tokens.Length != 2) continue;

                string key = tokens[0].Trim();
                string value = tokens[1].Trim();

                if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value)) continue;

                AddWordPair(key, value);
            }

            Console.WriteLine($"Loaded {Dictionary.Count / 2} dictionary entries.");
        }

        private static void AddWordPair(string key, string value)
        {
            Dictionary[key] = value;
            Dictionary[value] = key;
        }

        private static void TranslateWord(string word)
        {
            if (Dictionary.TryGetValue(word, out string? translation))
            {
                Console.WriteLine($"Translation: {translation}");
            }
            else
            {
                Console.WriteLine($"Word not found. Do you want to add it to dictionary? ({ConfirmLiteral.ToUpper()}/N)");
                string? response = Console.ReadLine()?.Trim().ToLower();

                if (response == ConfirmLiteral)
                {
                    AddTranslation(word);
                }
            }
        }

        private static void AddTranslation(string word)
        {
            Console.Write("Input translation (or press Enter to cancel): ");
            string? translation = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(translation))
            {
                Console.WriteLine("Operation cancelled.");
                return;
            }

            AddWordPair(word, translation);
            Console.WriteLine("Word added to dictionary.");
        }

        private static void SaveDictionary()
        {
            HashSet<string> savedPairs = [];
            List<string> lines = [];

            foreach (KeyValuePair<string, string> pair in Dictionary.Where(pair =>
                     !savedPairs.Contains(pair.Value + Separator + pair.Key)))
            {
                lines.Add($"{pair.Key}{Separator}{pair.Value}");
                savedPairs.Add(pair.Key + Separator + pair.Value);
            }

            File.WriteAllLines(FilePath, lines);
        }

        private static void Main()
        {
            LoadDictionary();
            Console.WriteLine("Nice to meet you! I'm your personal translator");

            while (true)
            {
                Console.Write("Enter word for translation (or press Enter to quit): ");
                string? input = Console.ReadLine()?.Trim();
                
                if (string.IsNullOrEmpty(input))
                {
                    break;
                }

                TranslateWord(input);
            }

            SaveDictionary();
        }
    }
}