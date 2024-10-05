using System.Text;
using Antlr4.Runtime.Misc;
using ChessUtilities.Library.Parser;
using static ChessUtilities.Library.Parser.PortableGameNotationParser;

namespace ChessUtilities.Library
{
    public class MovesVisitor : PortableGameNotationBaseVisitor<object>
    {
        public override object VisitMove([NotNull] MoveContext context)
        {
            MovenumberContext moveNumber = context.movenumber();            

            IEnumerable<HalfmoveContext> halfmoves = context.halfmove();

            StringBuilder builder = new StringBuilder(moveNumber.GetText());

            builder.Append($" {string.Join(' ', halfmoves.Select(hm=>hm.GetText()))}");

            Console.WriteLine(builder.ToString());
            return base.VisitMove(context);
        }

        public override object VisitHalfmove([NotNull] HalfmoveContext context)
        {
            Console.WriteLine(context.GetText());
            return base.VisitHalfmove(context);
        }
    }
}