namespace Dictionary
{
    internal static class Program
    {
        private static readonly Dictionary<string, HashSet<string>> _dictionary = new();
        private const string _filePath = "Dictionary.txt";
        private const string _separator = ":";
        private const string _translationsSeparator = ",";
        private const string _confirmLiteral = "y";

        private static void LoadDictionary()
        {
            _dictionary.Clear();

            if ( !File.Exists( _filePath ) )
            {
                File.Create( _filePath ).Close();
                Console.WriteLine( $"File {_filePath} created" );
                return;
            }

            foreach ( string line in File.ReadAllLines( _filePath ) )
            {
                if ( string.IsNullOrWhiteSpace( line ) )
                {
                    continue;
                }

                string[] tokens = line.Split( _separator, 2 );
                if ( tokens.Length != 2 )
                {
                    continue;
                }

                string key = tokens[ 0 ].Trim();
                string[] translations = tokens[ 1 ].Split( _translationsSeparator )
                    .Select( t => t.Trim() )
                    .Where( t => !string.IsNullOrWhiteSpace( t ) )
                    .ToArray();

                if ( translations.Length == 0 )
                {
                    continue;
                }

                AddTranslations( key, translations );
            }

            Console.WriteLine( $"Loaded {_dictionary.Count} dictionary entries." );
        }

        private static void AddTranslations( string word, IEnumerable<string> translations )
        {
            if ( !_dictionary.TryGetValue( word, out HashSet<string>? translationSet ) )
            {
                translationSet = [ ];
                _dictionary[ word ] = translationSet;
            }

            foreach ( string translation in translations )
            {
                translationSet.Add( translation );

                if ( !_dictionary.TryGetValue( translation, out HashSet<string>? reverseSet ) )
                {
                    reverseSet = [ ];
                    _dictionary[ translation ] = reverseSet;
                }

                reverseSet.Add( word );
            }
        }

        private static void TranslateWord( string word )
        {
            if ( _dictionary.TryGetValue( word, out HashSet<string>? translations ) )
            {
                Console.WriteLine( $"Available translations: {string.Join( ", ", translations )}" );

                Console.WriteLine( $"Do you want to add another translation? ({_confirmLiteral.ToUpper()}/N)" );
                string? response = Console.ReadLine()?.Trim().ToLower();

                if ( response == _confirmLiteral )
                {
                    AddNewTranslation( word );
                }
            }
            else
            {
                Console.WriteLine(
                    $"Word not found. Do you want to add it to dictionary? ({_confirmLiteral.ToUpper()}/N)" );
                string? response = Console.ReadLine()?.Trim().ToLower();

                if ( response == _confirmLiteral )
                {
                    AddNewTranslation( word );
                }
            }
        }

        private static void AddNewTranslation( string word )
        {
            Console.Write(
                $"Enter new translations separated by '{_translationsSeparator}' (or press Enter to cancel): " );
            string? input = Console.ReadLine()?.Trim();

            if ( string.IsNullOrWhiteSpace( input ) )
            {
                Console.WriteLine( "Operation cancelled." );
                return;
            }

            string[] newTranslations = input.Split( _translationsSeparator )
                .Select( t => t.Trim() )
                .Where( t => !string.IsNullOrWhiteSpace( t ) )
                .ToArray();

            if ( newTranslations.Length == 0 )
            {
                Console.WriteLine( "No valid translations provided." );
                return;
            }

            AddTranslations( word, newTranslations );
            Console.WriteLine( "Translations/words added to dictionary." );
        }

        private static void SaveDictionary()
        {
            HashSet<string> savedWords = [ ];
            List<string> lines = [ ];

            foreach ( KeyValuePair<string, HashSet<string>> pair in _dictionary.Where( pair =>
                         !savedWords.Contains( pair.Key ) ) )
            {
                lines.Add( $"{pair.Key}{_separator}{string.Join( _translationsSeparator, pair.Value )}" );
                savedWords.Add( pair.Key );

                foreach ( string translation in pair.Value )
                {
                    savedWords.Add( translation );
                }
            }

            File.WriteAllLines( _filePath, lines );
        }

        private static void Main()
        {
            LoadDictionary();
            Console.WriteLine( "Welcome to Multi-Translator Dictionary!" );
            bool isDictionaryActive = true;

            while ( isDictionaryActive )
            {
                Console.Write( "Enter word for translation (or press Enter to quit): " );
                string? input = Console.ReadLine()?.Trim();

                if ( string.IsNullOrEmpty( input ) )
                {
                    isDictionaryActive = false;
                }

                if ( input != null )
                {
                    TranslateWord( input );
                }
            }

            SaveDictionary();
        }
    }
}