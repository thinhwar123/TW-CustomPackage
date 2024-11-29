using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
#endif

namespace TW.Utility.CustomType
{

    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class BigNumberEditorAttribute : System.Attribute
    {

    }

#if UNITY_EDITOR
    public sealed class BigNumberEditorAttributeDrawer : OdinAttributeDrawer<BigNumberEditorAttribute, BigNumber>
    {
        private static readonly char[] OperatorChars = { '+', '-', '*', '/', '(', ')' };

        protected override void DrawPropertyLayout(GUIContent label)
        {
            Rect rect = EditorGUILayout.GetControlRect();

            // In Odin, labels are optional and can be null, so we have to account for that.
            if (label != null)
            {
                rect = EditorGUI.PrefixLabel(rect, label);
            }
            this.ValueEntry.SmartValue = Evaluate(EditorGUI.TextField(rect, this.ValueEntry.SmartValue.ToString())).Normalize();
        }
        private static bool IsBigNumber(string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            if (!value.Any(char.IsDigit)) return false;
            if (value.Count(x => x == '.') > 1) return false;
            int abbreviationsLength = 0;
            for (int i = value.Length - 1; i >= 0; i--)
            {
                if (char.IsLetter(value[i]))
                {
                    abbreviationsLength++;
                }
                else
                {
                    break;
                }
            }
            string exponent = value[^abbreviationsLength..];
            return BigNumber.Abbreviations.IsExponent(exponent) && value[..^abbreviationsLength].All(c => char.IsDigit(c) || c == '.');
        }
        private static bool IsCalculation(string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            int bracketCount = 0;
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == '(')
                {
                    bracketCount++;
                }
                else if (value[i] == ')')
                {
                    if (bracketCount == 0) return false;
                    bracketCount--;
                }
                else if (OperatorChars.Contains(value[i]))
                {
                    if (value[i..].All(x => !char.IsDigit(x)))
                    {
                        return false;
                    }
                }
            }

            return value.Replace(" ", "").Split(OperatorChars).Where(x => !string.IsNullOrEmpty(x)).All(IsBigNumber);
        }
        private static BigNumber Evaluate(string expression)
        {
            if (!IsCalculation(expression)) return BigNumber.ZERO;

            List<object> tokens = new List<object>();
            MatchCollection matches = Regex.Matches(expression, @"\d+(\.\d+)?((K|M|B|T)|(" + string.Join("|", BigNumber.Abbreviations.AbbreviationArray) + @"))*\d*(\.\d+)?|[()+\-*/]");
            
            for (int index = 0; index < matches.Count; index++)
            {
                Match match = matches[index];

                if (IsBigNumber(match.Value))
                {
                    tokens.Add(new BigNumber(match.Value));

                }
                else
                {
                    tokens.Add(match.Value[0]);
                }
            }

            // Create a stack to hold the intermediate results of the evaluation.
            Stack<BigNumber> values = new Stack<BigNumber>();
            Stack<char> operators = new Stack<char>();

            foreach (object token in tokens)
            {
                if (token is BigNumber bigNumber)
                {
                    values.Push(bigNumber);
                }
                else if (token.ToString() == "(")
                {
                    operators.Push('(');
                }
                else if (token.ToString() == ")")
                {
                    while (operators.Peek() != '(')
                    {
                        ApplyOperation(values, operators.Pop());
                    }
                    operators.Pop();
                }
                else if (token is char op)
                {
                    while (operators.Count > 0 && Precedence(operators.Peek()) >= Precedence(op))
                    {
                        ApplyOperation(values, operators.Pop());
                    }
                    operators.Push(op);
                }
            }

            while (operators.Count > 0)
            {
                ApplyOperation(values, operators.Pop());
            }

            return values.Pop();
        }
        private static void ApplyOperation(Stack<BigNumber> values, char op)
        {
            BigNumber b = values.Pop();
            BigNumber a = values.Count == 0 ? BigNumber.ZERO : values.Pop();

            switch (op)
            {
                case '+':
                    values.Push(a + b);
                    break;
                case '-':
                    values.Push(a - b);
                    break;
                case '*':
                    values.Push(a * b);
                    break;
                case '/':
                    values.Push(a / b);
                    break;
            }
        }
        private static int Precedence(char op)
        {
            return op switch
            {
                '+' => 1,
                '-' => 1,
                '*' => 2,
                '/' => 2,
                _ => 0
            };
        }
    }
#endif
}
