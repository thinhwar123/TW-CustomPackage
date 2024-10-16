using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace TW.SourcesGenerator.AEnumQuickConvert
{
    [Generator]
    public class AEnumQuickConvertGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {

        }

        public void Execute(GeneratorExecutionContext context)
        {
            DefinitionSyntaxReceiver definitionSyntaxReceiver = new();
            foreach (SyntaxTree syntaxTree in context.Compilation.SyntaxTrees)
            {
                foreach (SyntaxNode syntaxNode in syntaxTree.GetRoot().DescendantNodes())
                {
                    definitionSyntaxReceiver.OnVisitSyntaxNode(syntaxNode);
                }
            }
            
            string sourceCode = definitionSyntaxReceiver.GetSource().NormalizeWhitespace().ToFullString();
            context.AddSource("AEnumQuickConvertExtensions.g.cs", SourceText.From(sourceCode, Encoding.UTF8));
        }
    }

    public class DefinitionSyntaxReceiver : ISyntaxReceiver
    {
        private List<MemberDeclarationSyntax> ListMemberDeclarationSyntax { get; set; } = new();
        
        public CompilationUnitSyntax GetSource()
        {
            return CompilationUnit()
                .WithUsings(SingletonList<UsingDirectiveSyntax>(UsingDirective(IdentifierName("System"))))
                .WithMembers(List<MemberDeclarationSyntax>(ListMemberDeclarationSyntax))
                .NormalizeWhitespace();
        }
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is EnumDeclarationSyntax enumDeclarationSyntax && enumDeclarationSyntax.AttributeLists
                .Any(attrList => attrList.Attributes
                    .Any(attr => attr.Name.ToString() == "AEnumQuickConvert")))
            {
                MemberDeclarationSyntax memberDeclarationSyntax = DefinitionNameSpace(syntaxNode,
                    DefinitionClass(enumDeclarationSyntax, 
                        DefinitionMethod(enumDeclarationSyntax)))[0];

                ListMemberDeclarationSyntax.Add(memberDeclarationSyntax);
            }
        }
        private SyntaxList<MemberDeclarationSyntax> DefinitionNameSpace(SyntaxNode syntaxNode, SyntaxList<MemberDeclarationSyntax> memberDeclarationSyntax = default)
        {
            if (syntaxNode == null)
            {
                return memberDeclarationSyntax;
            }
            
            if (syntaxNode is NamespaceDeclarationSyntax namespaceSyntax)
            {
                return DefinitionNameSpace(syntaxNode.Parent, 
                    SingletonList<MemberDeclarationSyntax>(
                        NamespaceDeclaration(
                                IdentifierName(namespaceSyntax.Name.ToString()))
                            .WithMembers(memberDeclarationSyntax)));
            }
            return DefinitionNameSpace(syntaxNode.Parent, memberDeclarationSyntax);
        }
        
        private SyntaxList<MemberDeclarationSyntax> DefinitionClass(EnumDeclarationSyntax enumDeclarationSyntax, SyntaxList<MemberDeclarationSyntax> memberDeclarationSyntax = default)
        {
            return SingletonList<MemberDeclarationSyntax>(
                ClassDeclaration($"{enumDeclarationSyntax.Identifier.Text}Extensions")
                    .WithModifiers(
                        TokenList(
                            new[]
                            {
                                Token(SyntaxKind.PublicKeyword),
                                Token(SyntaxKind.StaticKeyword)
                            }))
                    .WithMembers(memberDeclarationSyntax));
        }

        private SyntaxList<MemberDeclarationSyntax> DefinitionMethod(EnumDeclarationSyntax enumDeclarationSyntax)
        {
            List<SyntaxNodeOrToken> syntaxNodeOrTokens = new List<SyntaxNodeOrToken>();
            foreach (EnumMemberDeclarationSyntax enumMemberDeclarationSyntax in enumDeclarationSyntax.Members)
            {
                if (enumMemberDeclarationSyntax.AttributeLists
                    .Any(attrList => attrList.Attributes
                        .Any(attr => attr.Name.ToString() == "AEnumIgnore")))
                {
                    syntaxNodeOrTokens.Add(
                        DefinitionIgnoreSwitchExpressionArm(enumDeclarationSyntax, enumMemberDeclarationSyntax));
                }
                else if (enumMemberDeclarationSyntax.AttributeLists
                         .Any(attrList => attrList.Attributes
                             .Any(attr => attr.Name.ToString() == "AEnumConversion")))
                {
                    LiteralExpressionSyntax defineValue = enumMemberDeclarationSyntax.AttributeLists
                        .SelectMany(attrList => attrList.Attributes
                            .Where(attr => attr.Name.ToString() == "AEnumConversion"))
                        .First().ArgumentList?.Arguments.First().Expression as LiteralExpressionSyntax;
                    syntaxNodeOrTokens.Add(
                        DefinitionDefineSwitchExpressionArm(enumDeclarationSyntax, enumMemberDeclarationSyntax, defineValue));
                }
                else
                {
                    syntaxNodeOrTokens.Add(
                        DefinitionDefaultSwitchExpressionArm(enumDeclarationSyntax, enumMemberDeclarationSyntax));
                }
                syntaxNodeOrTokens.Add(Token(SyntaxKind.CommaToken));
            }
            syntaxNodeOrTokens.Add(DefinitionDefaultSwitchExpressionArm());
            syntaxNodeOrTokens.Add(Token(SyntaxKind.CommaToken));
            
            return SingletonList<MemberDeclarationSyntax>(
                MethodDeclaration(
                        PredefinedType(
                            Token(SyntaxKind.StringKeyword)),
                        Identifier("ToFastString"))
                    .WithModifiers(
                        TokenList(
                            new []{
                                Token(SyntaxKind.PublicKeyword),
                                Token(SyntaxKind.StaticKeyword)}))
                    .WithParameterList(
                        ParameterList(
                            SingletonSeparatedList<ParameterSyntax>(
                                Parameter(
                                        Identifier("value"))
                                    .WithModifiers(
                                        TokenList(
                                            Token(SyntaxKind.ThisKeyword)))
                                    .WithType(
                                        IdentifierName(enumDeclarationSyntax.Identifier.Text)))))
                    .WithBody(Block(
                        SingletonList<StatementSyntax>(
                            ReturnStatement(
                                SwitchExpression(
                                    IdentifierName("value"))
                                    .WithArms(
                                        SeparatedList<SwitchExpressionArmSyntax>(
                                            syntaxNodeOrTokens)))))));
        }

        private static SyntaxNodeOrToken DefinitionDefaultSwitchExpressionArm(
            EnumDeclarationSyntax enumDeclarationSyntax,
            EnumMemberDeclarationSyntax enumMemberDeclarationSyntax)
        {
            return SwitchExpressionArm(
                ConstantPattern(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(enumDeclarationSyntax.Identifier.Text),
                        IdentifierName(enumMemberDeclarationSyntax.Identifier.Text))),
                LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    Literal(enumMemberDeclarationSyntax.Identifier.Text)));
        }
        
        private static SyntaxNodeOrToken DefinitionDefineSwitchExpressionArm(
            EnumDeclarationSyntax enumDeclarationSyntax,
            EnumMemberDeclarationSyntax enumMemberDeclarationSyntax, LiteralExpressionSyntax defineValue)
        {
            return SwitchExpressionArm(
                ConstantPattern(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(enumDeclarationSyntax.Identifier.Text),
                        IdentifierName(enumMemberDeclarationSyntax.Identifier.Text))),
                LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    defineValue.Token));
        }
        
        private static SyntaxNodeOrToken DefinitionIgnoreSwitchExpressionArm(
            EnumDeclarationSyntax enumDeclarationSyntax,
            EnumMemberDeclarationSyntax enumMemberDeclarationSyntax)
        {
            return SwitchExpressionArm(
                ConstantPattern(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(enumDeclarationSyntax.Identifier.Text),
                        IdentifierName(enumMemberDeclarationSyntax.Identifier.Text))),
                ThrowExpression(
                    ObjectCreationExpression(
                            IdentifierName("InvalidOperationException"))
                        .WithArgumentList(
                            ArgumentList(
                                SingletonSeparatedList<ArgumentSyntax>(
                                    Argument(
                                        InterpolatedStringExpression(
                                                Token(SyntaxKind.InterpolatedStringStartToken))
                                            .WithContents(
                                                List<InterpolatedStringContentSyntax>(
                                                    new InterpolatedStringContentSyntax[]
                                                    {
                                                        InterpolatedStringText()
                                                            .WithTextToken(
                                                                Token(
                                                                    TriviaList(),
                                                                    SyntaxKind.InterpolatedStringTextToken,
                                                                    "Conversion for ",
                                                                    "Conversion for ",
                                                                    TriviaList())),
                                                        Interpolation(
                                                            IdentifierName("value")),
                                                        InterpolatedStringText()
                                                            .WithTextToken(
                                                                Token(
                                                                    TriviaList(),
                                                                    SyntaxKind.InterpolatedStringTextToken,
                                                                    " is ignored.",
                                                                    " is ignored.",
                                                                    TriviaList()))}))))))));
        }

        private SyntaxNodeOrToken DefinitionDefaultSwitchExpressionArm()
        {
            return SwitchExpressionArm(
                DiscardPattern(),
                ThrowExpression(
                    ObjectCreationExpression(IdentifierName("ArgumentOutOfRangeException"))
                        .WithArgumentList(ArgumentList())));
        }

    }
}
