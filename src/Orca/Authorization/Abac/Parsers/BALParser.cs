using Antlr4.Runtime;
using Orca.Authorization.Abac.Context;
using Orca.Authorization.Abac.Grammars;
using Orca.Authorization.Abac.Grammars.BAL;
using Orca.DSL.Grammar.Bal;

namespace Orca.Authorization.Abac.Parsers
{
    internal class BALParser
        : IGrammarParser
    {
        public bool CanParse(WellKnownGrammars grammar)
        {
            return grammar == WellKnownGrammars.Bal;
        }

        public AbacAuthorizationPolicy Parse(string policy)
        {
            var inputStream = new AntlrInputStream(policy);
            var lexer = new BalLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new BalParser(tokenStream);

            return new BalVisitor().Visit(parser.policy());
        }
    }
}
