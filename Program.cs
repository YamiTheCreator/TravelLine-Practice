namespace Dictionary;

using MyDictionary = Dictionary<string, string>;

class Program
{
    private static MyDictionary _dictionaryRuEn = new();
    private static MyDictionary _dictionaryEnRu = new();
    private const string _filePath = "Dictionary.txt";
    private const char _separator = ':';

    static void LoadDictionary()
    {
        _dictionaryRuEn.Clear();
        _dictionaryEnRu.Clear();

        if ( !File.Exists( _filePath ) )
        {
            File.Create( _filePath );
            Console.WriteLine( $"File {_filePath} created" );
        }

        using ( StreamReader sr = new StreamReader( _filePath ) )
        {
            string line;
            while ( ( line = sr.ReadLine() ) != null )
            {
                string[] tokens = line.Split( _separator, 2 );

                string key = tokens[ 0 ].Trim();
                string value = tokens[ 1 ].Trim();

                MyDictionary primaryDict = IsEnglish( key ) ? _dictionaryEnRu : _dictionaryRuEn;
                MyDictionary secondaryDict = IsEnglish( key ) ? _dictionaryRuEn : _dictionaryEnRu;

                AddWordPair( primaryDict, secondaryDict, key, value );
            }

            Console.WriteLine( $"Loaded {_dictionaryRuEn.Count} dictionary entries." );
        }
    }

    static void AddWordPair( Dictionary<string, string> primaryDict, Dictionary<string, string> secondaryDict,
        string key, string value )
    {
        primaryDict[ key ] = value;

        secondaryDict[ value ] = key;
    }

    static bool IsEnglish( string word )
    {
        return word.Any( c => ( c >= 'A' && c <= 'Z' ) || ( c >= 'a' && c <= 'z' ) );
    }

    static void TranslateWord( string word )
    {
        MyDictionary dictionary = IsEnglish( word ) ? _dictionaryEnRu : _dictionaryRuEn;

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

                MyDictionary otherDictionary = IsEnglish( word ) ? _dictionaryRuEn : _dictionaryEnRu;
                AddWordPair( dictionary, otherDictionary, word, translation );

                Console.WriteLine( "Word added to dictionary." );
            }
        }
    }

    static void SaveDictionary()
    {
        List<string> lines = new List<string>();

        foreach ( ( string key, string value ) in _dictionaryRuEn )
        {
            lines.Add( $"{key} : {value}" );
        }

        File.WriteAllLines( _filePath, lines );
    }

    static void Main( string[] args )
    {
        LoadDictionary();
        Console.WriteLine( "Nice to meet you! I'm your personal translator" );

        while ( true )
        {
            Console.Write( "Enter word for translation or enter for quit: " );
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
}