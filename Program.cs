namespace Dictionary;

class Program
{
    private static Dictionary<string, string> _dictionaryRuEn = new Dictionary<string, string>();
    private static Dictionary<string, string> _dictionaryEnRu = new Dictionary<string, string>();
    private const string _filePath = "Dictionary.txt";
    private const char _separator = ':';

    static void LoadDictionary()
    {
        _dictionaryRuEn.Clear();
        _dictionaryEnRu.Clear();

        if ( !File.Exists( _filePath ) )
        {
            File.Create( _filePath ).Dispose();
            Console.WriteLine( $"File {_filePath} created" );
        }

        using ( StreamReader sr = new StreamReader( _filePath ) )
        {
            string line;
            while ( ( line = sr.ReadLine() ) != null )
            {
                string[] tokens = line.Split( _separator, 2 );
                if ( tokens.Length == 2 )
                {
                    string key = tokens[ 0 ].Trim();
                    string value = tokens[ 1 ].Trim();
                    
                    var primaryDict = IsEnglish( key ) ? _dictionaryEnRu : _dictionaryRuEn;
                    var secondaryDict = IsEnglish( key ) ? _dictionaryRuEn : _dictionaryEnRu;

                    AddWordPair( primaryDict, secondaryDict, key, value );
                }
            }
        }

        Console.WriteLine( $"Loaded {_dictionaryRuEn.Count} dictionary entries." );
    }

    static void AddWordPair( Dictionary<string, string> primaryDict, Dictionary<string, string> secondaryDict,
        string key, string value )
    {
        if ( primaryDict.ContainsKey( key ) )
        {
            primaryDict[ key ] = value;
        }
        else
        {
            primaryDict.Add( key, value );
        }
        
        if ( secondaryDict.ContainsKey( value ) )
        {
            secondaryDict[ value ] = key;
        }
        else
        {
            secondaryDict.Add( value, key );
        }
    }

    static bool IsEnglish( string word )
    {
        foreach ( char c in word )
        {
            if ( c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' )
            {
                return true;
            }
        }

        return false;
    }

    static void TranslateWord( string word )
    {
        var dictionary = IsEnglish( word ) ? _dictionaryEnRu : _dictionaryRuEn;

        if ( dictionary.ContainsKey( word ) )
        {
            Console.WriteLine( $"Translation: {dictionary[ word ]}" );
        }
        else
        {
            Console.WriteLine( "Word not found. Do you want to add it to dictionary? (y/n)" );
            string response = Console.ReadLine();
            if ( response.ToLower() == "y" )
            {
                Console.Write( "Input translation or enter for quit: " );
                string translation = Console.ReadLine();
                
                var otherDictionary = IsEnglish( word ) ? _dictionaryRuEn : _dictionaryEnRu;
                AddWordPair( dictionary, otherDictionary, word, translation );

                Console.WriteLine( "Word added to dictionary." );
            }
        }
    }

    static void SaveDictionary()
    {
        List<string> lines = new List<string>();

        foreach ( var pair in _dictionaryRuEn )
        {
            lines.Add( $"{pair.Key} : {pair.Value}" );
        }

        File.WriteAllLines( _filePath, lines );
    }

    static void Main( string[] args )
    {
        try
        {
            LoadDictionary();
            Console.WriteLine( "Nice to meet you! I'm your personal translator" );

            while ( true )
            {
                Console.Write( "Enter word for translation: " );
                string input = Console.ReadLine();
                if ( string.IsNullOrEmpty( input ) )
                {
                    SaveDictionary();
                    break;
                }

                TranslateWord( input );
            }

            SaveDictionary();
        }
        catch ( Exception ex )
        {
            Console.WriteLine( $"Error: {ex.Message}" );
        }
    }
}