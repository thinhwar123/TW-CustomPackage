﻿using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using System.Linq;
using System.Threading;

namespace TW.SourceGenerator.ACacheEverything
{
    [Generator]
    public class AMethodCacheGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var methodDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(IsMethodWithAttribute, GetMethodWithAttribute)
                .Where(m => m is not null)
                .Collect();

            context.RegisterSourceOutput(methodDeclarations, (spc, methods) => GenerateCode(spc, methods!));
        }

        private bool IsMethodWithAttribute(SyntaxNode node, CancellationToken cancellationToken)
        {
            return node is MethodDeclarationSyntax { AttributeLists: { Count: > 0 } };
        }

        private MethodDeclarationSyntax GetMethodWithAttribute(GeneratorSyntaxContext context,
            CancellationToken cancellationToken)
        {
            var methodDeclaration = (MethodDeclarationSyntax)context.Node;

            foreach (var attributeList in methodDeclaration.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    if (attribute.Name.ToString() == "ACacheMethod")
                    {
                        return methodDeclaration;
                    }
                }
            }

            return null;
        }

        private void GenerateCode(SourceProductionContext context, ImmutableArray<MethodDeclarationSyntax> methods)
        {
            foreach (var method in methods)
            {
                var classDeclaration = (ClassDeclarationSyntax)method.Parent!;
                var className = classDeclaration.Identifier.Text;
                var methodName = method.Identifier.Text;
                var source = GetFullSource(method,
                    GetFullNamespace(method, 
                        GetFullClassName(method, 
                            GetFullMethodName(method))));
                context.AddSource($"{className}_{methodName}_Cache.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        private string GetFullSource(SyntaxNode node, string other)
        {
            return $@"// <auto-generated/>
using System;
{other}";
        }

        private string GetFullNamespace(SyntaxNode node, string other)
        {
            Stack<string> namespaces = new Stack<string>();
            while (node != null)
            {
                node = node.Parent;
                if (node is NamespaceDeclarationSyntax namespaceDeclaration)
                {
                    namespaces.Push(namespaceDeclaration.Name.ToString());
                }
            }

            return namespaces.Count == 0 ? $"{other}" : $@"
namespace {string.Join(".", namespaces)}
{{{other}
}}";
        }

        private string GetFullClassName(SyntaxNode node, string other)
        {
            while (node != null)
            {
                node = node.Parent;
                if (node is ClassDeclarationSyntax classDeclaration)
                {
                    string classDeclarationText = $@"
public partial class {classDeclaration.Identifier.Text}
{{{other}
}}";
                    return GetFullClassName(classDeclaration, classDeclarationText);
                }
            }

            return $"{other}";
        }

        private string GetFullMethodName(MethodDeclarationSyntax method)
        {
            var methodType = GetFullMethodType(method);
            var methodName = method.Identifier.Text;
            var cacheFieldName = $"m_{methodName}Cache";
            var cachePropertyName = $"{methodName}Cache";
            return $@"
private {methodType} {cacheFieldName};
public {methodType} {cachePropertyName} => {cacheFieldName} ??= {methodName};";
        }

        private string GetFullMethodType(MethodDeclarationSyntax method)
        {
            if (method.ReturnType.ToString() == "void")
            {
                return GetFullActionMethodType(method);
            }
            else
            {
                return GetFullFuncMethodType(method);
            }

        }

        private string GetFullActionMethodType(MethodDeclarationSyntax method)
        {
            var parameters = method.ParameterList.Parameters;
            if (parameters.Count == 0)
            {
                return "Action";
            }
            else
            {
                var parameterTypes = string.Join(", ", parameters.Select(p => p.Type));
                return $"Action<{parameterTypes}>";
            }
        }
        private string GetFullFuncMethodType(MethodDeclarationSyntax method)
        {
            var parameters = method.ParameterList.Parameters;
            if (parameters.Count == 0)
            {
                return $"Func<{method.ReturnType}>";
            }
            else
            {
                var parameterTypes = string.Join(", ", parameters.Select(p => p.Type));
                return $"Func<{parameterTypes}, {method.ReturnType}>";
            }
        }
    }
}