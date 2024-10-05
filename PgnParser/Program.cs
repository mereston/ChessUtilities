using Antlr4.Runtime;
using ChessUtilities.Library.Parser;
using ChessUtilities.Library;
namespace ChessUtilities;

internal class Program
{
    public static void Main(string[] args)
    {
        //using StreamReader reader = new StreamReader(new FileStream("test.pgn", FileMode.Open));
        AntlrInputStream inputStream = new AntlrInputStream("e4");

        PortableGameNotationLexer lexer = new PortableGameNotationLexer(inputStream);

        CommonTokenStream tokenStream = new CommonTokenStream(lexer);

        PortableGameNotationParser parser = new PortableGameNotationParser(tokenStream);

        PortableGameNotationParser.HalfmoveContext context = parser.halfmove();

        MovesVisitor visitor = new MovesVisitor();

        context.Accept(visitor);        
    }
}