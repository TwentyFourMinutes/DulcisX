using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;

namespace DulcisX.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DulcisXAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "DulcisXAnalyzer";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "API_Usage";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            //context.RegisterOperationAction(AnalyzeOperation, OperationKind.MethodReference, OperationKind.PropertyReference, OperationKind.EventReference);
            context.RegisterSyntaxNodeAction(AnalyzeOperation, SyntaxKind.InvocationExpression);
        }

        //private static void AnalyzeOperation(OperationAnalysisContext context)
        //{
        //    if (context.Operation is IMethodReferenceOperation operation)
        //    {
        //        var test = operation.Method.GetAttributes();
        //        //var test2 = test.Select(x=> x.);
        //    }
        //}

        private static void AnalyzeOperation(SyntaxNodeAnalysisContext context)
        {
            if (!(context.Node is InvocationExpressionSyntax node))
                return;

        }
    }
}
