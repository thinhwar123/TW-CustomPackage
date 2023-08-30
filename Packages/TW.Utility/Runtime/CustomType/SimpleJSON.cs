﻿/* * * * *
 * A simple JSON Parser / builder
 * ------------------------------
 * 
 * It mainly has been written as a simple JSON parser. It can build a JSON string
 * from the node-tree, or generate a node tree from any valid JSON string.
 * 
 * Written by Bunny83 
 * 2012-06-09
 * 
 * Changelog now external. See Changelog.txt
 * 
 * The MIT License (MIT)
 * 
 * Copyright (c) 2012-2019 Markus GĂ¶bel (Bunny83)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 * * * * */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TW.Utility.CustomType
{
    namespace SimpleJSON
    {
        public enum JSONNodeType
        {
            Array = 1,
            Object = 2,
            String = 3,
            Number = 4,
            NullValue = 5,
            Boolean = 6,
            None = 7,
            Custom = 0xFF,
        }
        public enum JSONTextMode
        {
            Compact,
            Indent
        }

        public abstract partial class JSONNode
        {
            #region Enumerators
            public struct Enumerator
            {
                private enum Type { None, Array, Object }
                private Type type;
                private Dictionary<string, JSONNode>.Enumerator m_Object;
                private List<JSONNode>.Enumerator m_Array;
                public bool IsValid => type != Type.None;

                public Enumerator(List<JSONNode>.Enumerator aArrayEnum)
                {
                    type = Type.Array;
                    m_Object = default(Dictionary<string, JSONNode>.Enumerator);
                    m_Array = aArrayEnum;
                }
                public Enumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum)
                {
                    type = Type.Object;
                    m_Object = aDictEnum;
                    m_Array = default(List<JSONNode>.Enumerator);
                }
                public KeyValuePair<string, JSONNode> Current
                {
                    get
                    {
                        if (type == Type.Array)
                            return new KeyValuePair<string, JSONNode>(string.Empty, m_Array.Current);
                        else if (type == Type.Object)
                            return m_Object.Current;
                        return new KeyValuePair<string, JSONNode>(string.Empty, null);
                    }
                }
                public bool MoveNext()
                {
                    if (type == Type.Array)
                        return m_Array.MoveNext();
                    else if (type == Type.Object)
                        return m_Object.MoveNext();
                    return false;
                }
            }
            public struct ValueEnumerator
            {
                private Enumerator m_Enumerator;
                public ValueEnumerator(List<JSONNode>.Enumerator aArrayEnum) : this(new Enumerator(aArrayEnum)) { }
                public ValueEnumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum) : this(new Enumerator(aDictEnum)) { }
                public ValueEnumerator(Enumerator aEnumerator) { m_Enumerator = aEnumerator; }
                public JSONNode Current => m_Enumerator.Current.Value;
                public bool MoveNext() { return m_Enumerator.MoveNext(); }
                public ValueEnumerator GetEnumerator() { return this; }
            }
            public struct KeyEnumerator
            {
                private Enumerator m_Enumerator;
                public KeyEnumerator(List<JSONNode>.Enumerator aArrayEnum) : this(new Enumerator(aArrayEnum)) { }
                public KeyEnumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum) : this(new Enumerator(aDictEnum)) { }
                public KeyEnumerator(Enumerator aEnumerator) { m_Enumerator = aEnumerator; }
                public string Current => m_Enumerator.Current.Key;
                public bool MoveNext() { return m_Enumerator.MoveNext(); }
                public KeyEnumerator GetEnumerator() { return this; }
            }

            public class LinqEnumerator : IEnumerator<KeyValuePair<string, JSONNode>>, IEnumerable<KeyValuePair<string, JSONNode>>
            {
                private JSONNode m_Node;
                private Enumerator m_Enumerator;
                internal LinqEnumerator(JSONNode aNode)
                {
                    m_Node = aNode;
                    if (m_Node != null)
                        m_Enumerator = m_Node.GetEnumerator();
                }
                public KeyValuePair<string, JSONNode> Current => m_Enumerator.Current;
                object IEnumerator.Current => m_Enumerator.Current;
                public bool MoveNext() { return m_Enumerator.MoveNext(); }

                public void Dispose()
                {
                    m_Node = null;
                    m_Enumerator = new Enumerator();
                }

                public IEnumerator<KeyValuePair<string, JSONNode>> GetEnumerator()
                {
                    return new LinqEnumerator(m_Node);
                }

                public void Reset()
                {
                    if (m_Node != null)
                        m_Enumerator = m_Node.GetEnumerator();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return new LinqEnumerator(m_Node);
                }
            }

            #endregion Enumerators

            #region common interface

            public static bool forceASCII = false; // Use Unicode by default
            public static bool longAsString = false; // lazy creator creates a JSONString instead of JSONNumber
            public static bool allowLineComments = true; // allow "//"-style comments at the end of a line

            public abstract JSONNodeType Tag { get; }

            public virtual JSONNode this[int aIndex] { get { return null; } set { } }

            public virtual JSONNode this[string aKey] { get { return null; } set { } }

            public virtual string Value { get { return ""; } set { } }

            public virtual int Count { get { return 0; } }

            public virtual bool IsNumber { get { return false; } }
            public virtual bool IsString { get { return false; } }
            public virtual bool IsBoolean { get { return false; } }
            public virtual bool IsNull { get { return false; } }
            public virtual bool IsArray { get { return false; } }
            public virtual bool IsObject { get { return false; } }

            public virtual bool Inline { get { return false; } set { } }

            public virtual void Add(string aKey, JSONNode aItem)
            {
            }
            public virtual void Add(JSONNode aItem)
            {
                Add("", aItem);
            }

            public virtual JSONNode Remove(string aKey)
            {
                return null;
            }

            public virtual JSONNode Remove(int aIndex)
            {
                return null;
            }

            public virtual JSONNode Remove(JSONNode aNode)
            {
                return aNode;
            }
            public virtual void Clear() { }

            public virtual JSONNode Clone()
            {
                return null;
            }

            public virtual IEnumerable<JSONNode> Children
            {
                get
                {
                    yield break;
                }
            }

            public IEnumerable<JSONNode> DeepChildren
            {
                get { return Children.SelectMany(C => C.DeepChildren); }
            }

            public virtual bool HasKey(string aKey)
            {
                return false;
            }

            public virtual JSONNode GetValueOrDefault(string aKey, JSONNode aDefault)
            {
                return aDefault;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                WriteToStringBuilder(sb, 0, 0, JSONTextMode.Compact);
                return sb.ToString();
            }

            public virtual string ToString(int aIndent)
            {
                StringBuilder sb = new StringBuilder();
                WriteToStringBuilder(sb, 0, aIndent, JSONTextMode.Indent);
                return sb.ToString();
            }
            internal abstract void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode);

            public abstract Enumerator GetEnumerator();
            public IEnumerable<KeyValuePair<string, JSONNode>> Linq => new LinqEnumerator(this);
            public KeyEnumerator Keys => new KeyEnumerator(GetEnumerator());
            public ValueEnumerator Values => new ValueEnumerator(GetEnumerator());

            #endregion common interface

            #region typecasting properties


            public virtual double AsDouble
            {
                get => double.TryParse(Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double v) ? v : 0.0;
                set => Value = value.ToString(CultureInfo.InvariantCulture);
            }

            public virtual int AsInt
            {
                get => (int)AsDouble;
                set => AsDouble = value;
            }

            public virtual float AsFloat
            {
                get => (float)AsDouble;
                set => AsDouble = value;
            }

            public virtual bool AsBool
            {
                get
                {
                    if (bool.TryParse(Value, out bool v))
                        return v;
                    return !string.IsNullOrEmpty(Value);
                }
                set => Value = (value) ? "true" : "false";
            }

            public virtual long AsLong
            {
                get => long.TryParse(Value, out long val) ? val : 0L;
                set => Value = value.ToString();
            }

            public virtual ulong AsULong
            {
                get => ulong.TryParse(Value, out ulong val) ? val : (ulong)0;
                set => Value = value.ToString();
            }

            public virtual JSONArray AsArray => this as JSONArray;

            public virtual JSONObject AsObject => this as JSONObject;

            #endregion typecasting properties

            #region operators

            public static implicit operator JSONNode(string s)
            {
                return (s == null) ? (JSONNode)JSONNull.CreateOrGet() : new JSONString(s);
            }
            public static implicit operator string(JSONNode d)
            {
                return d?.Value;
            }

            public static implicit operator JSONNode(double n)
            {
                return new JSONNumber(n);
            }
            public static implicit operator double(JSONNode d)
            {
                return (d == null) ? 0 : d.AsDouble;
            }

            public static implicit operator JSONNode(float n)
            {
                return new JSONNumber(n);
            }
            public static implicit operator float(JSONNode d)
            {
                return (d == null) ? 0 : d.AsFloat;
            }

            public static implicit operator JSONNode(int n)
            {
                return new JSONNumber(n);
            }
            public static implicit operator int(JSONNode d)
            {
                return (d == null) ? 0 : d.AsInt;
            }

            public static implicit operator JSONNode(long n)
            {
                if (longAsString)
                    return new JSONString(n.ToString());
                return new JSONNumber(n);
            }
            public static implicit operator long(JSONNode d)
            {
                return (d == null) ? 0L : d.AsLong;
            }

            public static implicit operator JSONNode(ulong n)
            {
                if (longAsString)
                    return new JSONString(n.ToString());
                return new JSONNumber(n);
            }
            public static implicit operator ulong(JSONNode d)
            {
                return (d == null) ? 0 : d.AsULong;
            }

            public static implicit operator JSONNode(bool b)
            {
                return new JSONBool(b);
            }
            public static implicit operator bool(JSONNode d)
            {
                return d != null && d.AsBool;
            }

            public static implicit operator JSONNode(KeyValuePair<string, JSONNode> aKeyValue)
            {
                return aKeyValue.Value;
            }

            public static bool operator ==(JSONNode a, object b)
            {
                if (ReferenceEquals(a, b))
                    return true;
                bool aIsNull = a is JSONNull or null or JSONLazyCreator;
                bool bIsNull = b is JSONNull or null or JSONLazyCreator;
                if (aIsNull && bIsNull)
                    return true;
                return !aIsNull && a.Equals(b);
            }

            public static bool operator !=(JSONNode a, object b)
            {
                return !(a == b);
            }

            public override bool Equals(object obj)
            {
                return ReferenceEquals(this, obj);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            #endregion operators

            [ThreadStatic]
            private static StringBuilder m_EscapeBuilder;
            internal static StringBuilder EscapeBuilder => m_EscapeBuilder ??= new StringBuilder();

            internal static string Escape(string aText)
            {
                StringBuilder sb = EscapeBuilder;
                sb.Length = 0;
                if (sb.Capacity < aText.Length + aText.Length / 10)
                    sb.Capacity = aText.Length + aText.Length / 10;
                foreach (char c in aText)
                {
                    switch (c)
                    {
                        case '\\':
                            sb.Append("\\\\");
                            break;
                        case '\"':
                            sb.Append("\\\"");
                            break;
                        case '\n':
                            sb.Append("\\n");
                            break;
                        case '\r':
                            sb.Append("\\r");
                            break;
                        case '\t':
                            sb.Append("\\t");
                            break;
                        case '\b':
                            sb.Append("\\b");
                            break;
                        case '\f':
                            sb.Append("\\f");
                            break;
                        default:
                            if (c < ' ' || (forceASCII && c > 127))
                            {
                                ushort val = c;
                                sb.Append("\\u").Append(val.ToString("X4"));
                            }
                            else
                                sb.Append(c);
                            break;
                    }
                }
                string result = sb.ToString();
                sb.Length = 0;
                return result;
            }

            private static JSONNode ParseElement(string token, bool quoted)
            {
                if (quoted)
                    return token;
                if (token.Length <= 5)
                {
                    string tmp = token.ToLower();
                    switch (tmp)
                    {
                        case "false" or "true":
                            return tmp == "true";
                        case "null":
                            return JSONNull.CreateOrGet();
                    }
                }

                if (double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out double val))
                    return val;
                else
                    return token;
            }

            public static JSONNode Parse(string aJSON)
            {
                Stack<JSONNode> stack = new Stack<JSONNode>();
                JSONNode ctx = null;
                int i = 0;
                StringBuilder token = new StringBuilder();
                string tokenName = "";
                bool quoteMode = false;
                bool tokenIsQuoted = false;
                bool hasNewlineChar = false;
                while (i < aJSON.Length)
                {
                    switch (aJSON[i])
                    {
                        case '{':
                            if (quoteMode)
                            {
                                token.Append(aJSON[i]);
                                break;
                            }
                            stack.Push(new JSONObject());
                            ctx?.Add(tokenName, stack.Peek());
                            tokenName = "";
                            token.Length = 0;
                            ctx = stack.Peek();
                            hasNewlineChar = false;
                            break;

                        case '[':
                            if (quoteMode)
                            {
                                token.Append(aJSON[i]);
                                break;
                            }

                            stack.Push(new JSONArray());
                            ctx?.Add(tokenName, stack.Peek());
                            tokenName = "";
                            token.Length = 0;
                            ctx = stack.Peek();
                            hasNewlineChar = false;
                            break;

                        case '}':
                        case ']':
                            if (quoteMode)
                            {

                                token.Append(aJSON[i]);
                                break;
                            }
                            if (stack.Count == 0)
                                throw new Exception("JSON Parse: Too many closing brackets");

                            stack.Pop();
                            if (token.Length > 0 || tokenIsQuoted)
                                ctx.Add(tokenName, ParseElement(token.ToString(), tokenIsQuoted));
                            if (ctx != null)
                                ctx.Inline = !hasNewlineChar;
                            tokenIsQuoted = false;
                            tokenName = "";
                            token.Length = 0;
                            if (stack.Count > 0)
                                ctx = stack.Peek();
                            break;

                        case ':':
                            if (quoteMode)
                            {
                                token.Append(aJSON[i]);
                                break;
                            }
                            tokenName = token.ToString();
                            token.Length = 0;
                            tokenIsQuoted = false;
                            break;

                        case '"':
                            quoteMode ^= true;
                            tokenIsQuoted |= quoteMode;
                            break;

                        case ',':
                            if (quoteMode)
                            {
                                token.Append(aJSON[i]);
                                break;
                            }
                            if (token.Length > 0 || tokenIsQuoted)
                                ctx.Add(tokenName, ParseElement(token.ToString(), tokenIsQuoted));
                            tokenIsQuoted = false;
                            tokenName = "";
                            token.Length = 0;
                            tokenIsQuoted = false;
                            break;

                        case '\r':
                        case '\n':
                            hasNewlineChar = true;
                            break;

                        case ' ':
                        case '\t':
                            if (quoteMode)
                                token.Append(aJSON[i]);
                            break;

                        case '\\':
                            ++i;
                            if (quoteMode)
                            {
                                char C = aJSON[i];
                                switch (C)
                                {
                                    case 't':
                                        token.Append('\t');
                                        break;
                                    case 'r':
                                        token.Append('\r');
                                        break;
                                    case 'n':
                                        token.Append('\n');
                                        break;
                                    case 'b':
                                        token.Append('\b');
                                        break;
                                    case 'f':
                                        token.Append('\f');
                                        break;
                                    case 'u':
                                        {
                                            string s = aJSON.Substring(i + 1, 4);
                                            token.Append((char)int.Parse(
                                                s,
                                                System.Globalization.NumberStyles.AllowHexSpecifier));
                                            i += 4;
                                            break;
                                        }
                                    default:
                                        token.Append(C);
                                        break;
                                }
                            }
                            break;
                        case '/':
                            if (allowLineComments && !quoteMode && i + 1 < aJSON.Length && aJSON[i + 1] == '/')
                            {
                                while (++i < aJSON.Length && aJSON[i] != '\n' && aJSON[i] != '\r') ;
                                break;
                            }
                            token.Append(aJSON[i]);
                            break;
                        case '\uFEFF': // remove / ignore BOM (Byte Order Mark)
                            break;

                        default:
                            token.Append(aJSON[i]);
                            break;
                    }
                    ++i;
                }
                if (quoteMode)
                {
                    throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
                }
                return ctx == null ? ParseElement(token.ToString(), tokenIsQuoted) : ctx;
            }

        }
        // End of JSONNode

        public partial class JSONArray : JSONNode
        {
            private List<JSONNode> m_List = new List<JSONNode>();
            private bool m_Inline = false;
            public override bool Inline
            {
                get => m_Inline;
                set => m_Inline = value;
            }

            public override JSONNodeType Tag => JSONNodeType.Array;
            public override bool IsArray => true;
            public override Enumerator GetEnumerator() { return new Enumerator(m_List.GetEnumerator()); }

            public override JSONNode this[int aIndex]
            {
                get
                {
                    if (aIndex < 0 || aIndex >= m_List.Count)
                        return new JSONLazyCreator(this);
                    return m_List[aIndex];
                }
                set
                {
                    if (value == null)
                        value = JSONNull.CreateOrGet();
                    if (aIndex < 0 || aIndex >= m_List.Count)
                        m_List.Add(value);
                    else
                        m_List[aIndex] = value;
                }
            }

            public override JSONNode this[string aKey]
            {
                get => new JSONLazyCreator(this);
                set
                {
                    if (value == null)
                        value = JSONNull.CreateOrGet();
                    m_List.Add(value);
                }
            }

            public override int Count => m_List.Count;

            public override void Add(string aKey, JSONNode aItem)
            {
                if (aItem == null)
                    aItem = JSONNull.CreateOrGet();
                m_List.Add(aItem);
            }

            public override JSONNode Remove(int aIndex)
            {
                if (aIndex < 0 || aIndex >= m_List.Count)
                    return null;
                JSONNode tmp = m_List[aIndex];
                m_List.RemoveAt(aIndex);
                return tmp;
            }

            public override JSONNode Remove(JSONNode aNode)
            {
                m_List.Remove(aNode);
                return aNode;
            }

            public override void Clear()
            {
                m_List.Clear();
            }

            public override JSONNode Clone()
            {
                JSONArray node = new JSONArray();
                node.m_List.Capacity = m_List.Capacity;
                foreach (JSONNode n in m_List)
                {
                    node.Add(n?.Clone());
                }
                return node;
            }

            public override IEnumerable<JSONNode> Children => m_List;


            internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
            {
                aSB.Append('[');
                int count = m_List.Count;
                if (m_Inline)
                    aMode = JSONTextMode.Compact;
                for (int i = 0; i < count; i++)
                {
                    if (i > 0)
                        aSB.Append(',');
                    if (aMode == JSONTextMode.Indent)
                        aSB.AppendLine();

                    if (aMode == JSONTextMode.Indent)
                        aSB.Append(' ', aIndent + aIndentInc);
                    m_List[i].WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
                }
                if (aMode == JSONTextMode.Indent)
                    aSB.AppendLine().Append(' ', aIndent);
                aSB.Append(']');
            }
        }
        // End of JSONArray

        public partial class JSONObject : JSONNode
        {
            private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();

            private bool m_Inline = false;
            public override bool Inline
            {
                get => m_Inline;
                set => m_Inline = value;
            }

            public override JSONNodeType Tag => JSONNodeType.Object;
            public override bool IsObject => true;

            public override Enumerator GetEnumerator() { return new Enumerator(m_Dict.GetEnumerator()); }


            public override JSONNode this[string aKey]
            {
                get => m_Dict.ContainsKey(aKey) ? m_Dict[aKey] : new JSONLazyCreator(this, aKey);
                set
                {
                    if (value == null)
                        value = JSONNull.CreateOrGet();
                    if (m_Dict.ContainsKey(aKey))
                        m_Dict[aKey] = value;
                    else
                        m_Dict.Add(aKey, value);
                }
            }

            public override JSONNode this[int aIndex]
            {
                get
                {
                    if (aIndex < 0 || aIndex >= m_Dict.Count)
                        return null;
                    return m_Dict.ElementAt(aIndex).Value;
                }
                set
                {
                    if (value == null)
                        value = JSONNull.CreateOrGet();
                    if (aIndex < 0 || aIndex >= m_Dict.Count)
                        return;
                    string key = m_Dict.ElementAt(aIndex).Key;
                    m_Dict[key] = value;
                }
            }

            public override int Count => m_Dict.Count;

            public override void Add(string aKey, JSONNode aItem)
            {
                if (aItem == null)
                    aItem = JSONNull.CreateOrGet();

                if (aKey != null)
                {
                    if (m_Dict.ContainsKey(aKey))
                        m_Dict[aKey] = aItem;
                    else
                        m_Dict.Add(aKey, aItem);
                }
                else
                    m_Dict.Add(Guid.NewGuid().ToString(), aItem);
            }

            public override JSONNode Remove(string aKey)
            {
                if (!m_Dict.ContainsKey(aKey))
                    return null;
                JSONNode tmp = m_Dict[aKey];
                m_Dict.Remove(aKey);
                return tmp;
            }

            public override JSONNode Remove(int aIndex)
            {
                if (aIndex < 0 || aIndex >= m_Dict.Count)
                    return null;
                var item = m_Dict.ElementAt(aIndex);
                m_Dict.Remove(item.Key);
                return item.Value;
            }

            public override JSONNode Remove(JSONNode aNode)
            {
                try
                {
                    KeyValuePair<string, JSONNode> item = m_Dict.First(k => k.Value == aNode);
                    m_Dict.Remove(item.Key);
                    return aNode;
                }
                catch
                {
                    return null;
                }
            }

            public override void Clear()
            {
                m_Dict.Clear();
            }

            public override JSONNode Clone()
            {
                JSONObject node = new JSONObject();
                foreach (KeyValuePair<string, JSONNode> n in m_Dict)
                {
                    node.Add(n.Key, n.Value.Clone());
                }
                return node;
            }

            public override bool HasKey(string aKey)
            {
                return m_Dict.ContainsKey(aKey);
            }

            public override JSONNode GetValueOrDefault(string aKey, JSONNode aDefault)
            {
                return m_Dict.TryGetValue(aKey, out JSONNode res) ? res : aDefault;
            }

            public override IEnumerable<JSONNode> Children
            {
                get { return m_Dict.Select(n => n.Value); }
            }

            internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
            {
                aSB.Append('{');
                bool first = true;
                if (m_Inline)
                    aMode = JSONTextMode.Compact;
                foreach (KeyValuePair<string, JSONNode> k in m_Dict)
                {
                    if (!first)
                        aSB.Append(',');
                    first = false;
                    if (aMode == JSONTextMode.Indent)
                        aSB.AppendLine();
                    if (aMode == JSONTextMode.Indent)
                        aSB.Append(' ', aIndent + aIndentInc);
                    aSB.Append('\"').Append(Escape(k.Key)).Append('\"');
                    if (aMode == JSONTextMode.Compact)
                        aSB.Append(':');
                    else
                        aSB.Append(" : ");
                    k.Value.WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
                }
                if (aMode == JSONTextMode.Indent)
                    aSB.AppendLine().Append(' ', aIndent);
                aSB.Append('}');
            }

        }
        // End of JSONObject

        public partial class JSONString : JSONNode
        {
            private string m_Data;

            public override JSONNodeType Tag => JSONNodeType.String;
            public override bool IsString => true;

            public override Enumerator GetEnumerator() { return new Enumerator(); }


            public override string Value
            {
                get => m_Data;
                set => m_Data = value;
            }

            public JSONString(string aData)
            {
                m_Data = aData;
            }
            public override JSONNode Clone()
            {
                return new JSONString(m_Data);
            }

            internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
            {
                aSB.Append('\"').Append(Escape(m_Data)).Append('\"');
            }
            public override bool Equals(object obj)
            {
                if (base.Equals(obj))
                    return true;
                if (obj is string s)
                    return m_Data == s;
                JSONString s2 = obj as JSONString;
                if (s2 != null)
                    return m_Data == s2.m_Data;
                return false;
            }
            public override int GetHashCode()
            {
                return m_Data.GetHashCode();
            }
            public override void Clear()
            {
                m_Data = "";
            }
        }
        // End of JSONString

        public partial class JSONNumber : JSONNode
        {
            private double m_Data;

            public override JSONNodeType Tag => JSONNodeType.Number;
            public override bool IsNumber => true;
            public override Enumerator GetEnumerator() { return new Enumerator(); }

            public override string Value
            {
                get => m_Data.ToString(CultureInfo.InvariantCulture);
                set
                {
                    if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double v))
                        m_Data = v;
                }
            }

            public override double AsDouble
            {
                get => m_Data;
                set => m_Data = value;
            }
            public override long AsLong
            {
                get => (long)m_Data;
                set => m_Data = value;
            }
            public override ulong AsULong
            {
                get => (ulong)m_Data;
                set => m_Data = value;
            }

            public JSONNumber(double aData)
            {
                m_Data = aData;
            }

            public JSONNumber(string aData)
            {
                Value = aData;
            }

            public override JSONNode Clone()
            {
                return new JSONNumber(m_Data);
            }

            internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
            {
                aSB.Append(Value);
            }
            private static bool IsNumeric(object value)
            {
                return value is int or uint or float or double or decimal or long or ulong or short or ushort or sbyte or byte;
            }
            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;
                if (base.Equals(obj))
                    return true;
                JSONNumber s2 = obj as JSONNumber;
                if (s2 != null)
                    return Math.Abs(m_Data - s2.m_Data) < 0.0000001;
                if (IsNumeric(obj))
                    return Math.Abs(Convert.ToDouble(obj) - m_Data) < 0.0000001;
                return false;
            }
            public override int GetHashCode()
            {
                return m_Data.GetHashCode();
            }
            public override void Clear()
            {
                m_Data = 0;
            }
        }
        // End of JSONNumber

        public partial class JSONBool : JSONNode
        {
            private bool m_Data;

            public override JSONNodeType Tag => JSONNodeType.Boolean;
            public override bool IsBoolean => true;
            public override Enumerator GetEnumerator() { return new Enumerator(); }

            public override string Value
            {
                get => m_Data.ToString();
                set
                {
                    if (bool.TryParse(value, out bool v))
                        m_Data = v;
                }
            }
            public override bool AsBool
            {
                get => m_Data;
                set => m_Data = value;
            }

            public JSONBool(bool aData)
            {
                m_Data = aData;
            }

            public JSONBool(string aData)
            {
                Value = aData;
            }

            public override JSONNode Clone()
            {
                return new JSONBool(m_Data);
            }

            internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
            {
                aSB.Append((m_Data) ? "true" : "false");
            }
            public override bool Equals(object obj)
            {
                return obj switch
                {
                    null => false,
                    bool b => m_Data == b,
                    _ => false
                };
            }
            public override int GetHashCode()
            {
                return m_Data.GetHashCode();
            }
            public override void Clear()
            {
                m_Data = false;
            }
        }
        // End of JSONBool

        public partial class JSONNull : JSONNode
        {
            static JSONNull m_StaticInstance = new JSONNull();
            public static bool ReuseSameInstance = true;
            public static JSONNull CreateOrGet()
            {
                return ReuseSameInstance ? m_StaticInstance : new JSONNull();
            }
            private JSONNull() { }

            public override JSONNodeType Tag { get { return JSONNodeType.NullValue; } }
            public override bool IsNull { get { return true; } }
            public override Enumerator GetEnumerator() { return new Enumerator(); }

            public override string Value
            {
                get { return "null"; }
                set { }
            }
            public override bool AsBool
            {
                get { return false; }
                set { }
            }

            public override JSONNode Clone()
            {
                return CreateOrGet();
            }

            public override bool Equals(object obj)
            {
                if (object.ReferenceEquals(this, obj))
                    return true;
                return (obj is JSONNull);
            }
            public override int GetHashCode()
            {
                return 0;
            }

            internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
            {
                aSB.Append("null");
            }
        }
        // End of JSONNull

        internal partial class JSONLazyCreator : JSONNode
        {
            private JSONNode m_Node = null;
            private string m_Key = null;
            public override JSONNodeType Tag { get { return JSONNodeType.None; } }
            public override Enumerator GetEnumerator() { return new Enumerator(); }

            public JSONLazyCreator(JSONNode aNode)
            {
                m_Node = aNode;
                m_Key = null;
            }

            public JSONLazyCreator(JSONNode aNode, string aKey)
            {
                m_Node = aNode;
                m_Key = aKey;
            }

            private T Set<T>(T aVal) where T : JSONNode
            {
                if (m_Key == null)
                    m_Node.Add(aVal);
                else
                    m_Node.Add(m_Key, aVal);
                m_Node = null; // Be GC friendly.
                return aVal;
            }

            public override JSONNode this[int aIndex]
            {
                get { return new JSONLazyCreator(this); }
                set { Set(new JSONArray()).Add(value); }
            }

            public override JSONNode this[string aKey]
            {
                get { return new JSONLazyCreator(this, aKey); }
                set { Set(new JSONObject()).Add(aKey, value); }
            }

            public override void Add(JSONNode aItem)
            {
                Set(new JSONArray()).Add(aItem);
            }

            public override void Add(string aKey, JSONNode aItem)
            {
                Set(new JSONObject()).Add(aKey, aItem);
            }

            public static bool operator ==(JSONLazyCreator a, object b)
            {
                if (b == null)
                    return true;
                return System.Object.ReferenceEquals(a, b);
            }

            public static bool operator !=(JSONLazyCreator a, object b)
            {
                return !(a == b);
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                    return true;
                return System.Object.ReferenceEquals(this, obj);
            }

            public override int GetHashCode()
            {
                return 0;
            }

            public override int AsInt
            {
                get { Set(new JSONNumber(0)); return 0; }
                set { Set(new JSONNumber(value)); }
            }

            public override float AsFloat
            {
                get { Set(new JSONNumber(0.0f)); return 0.0f; }
                set { Set(new JSONNumber(value)); }
            }

            public override double AsDouble
            {
                get { Set(new JSONNumber(0.0)); return 0.0; }
                set { Set(new JSONNumber(value)); }
            }

            public override long AsLong
            {
                get
                {
                    if (longAsString)
                        Set(new JSONString("0"));
                    else
                        Set(new JSONNumber(0.0));
                    return 0L;
                }
                set
                {
                    if (longAsString)
                        Set(new JSONString(value.ToString()));
                    else
                        Set(new JSONNumber(value));
                }
            }

            public override ulong AsULong
            {
                get
                {
                    if (longAsString)
                        Set(new JSONString("0"));
                    else
                        Set(new JSONNumber(0.0));
                    return 0L;
                }
                set
                {
                    if (longAsString)
                        Set(new JSONString(value.ToString()));
                    else
                        Set(new JSONNumber(value));
                }
            }

            public override bool AsBool
            {
                get { Set(new JSONBool(false)); return false; }
                set { Set(new JSONBool(value)); }
            }

            public override JSONArray AsArray
            {
                get { return Set(new JSONArray()); }
            }

            public override JSONObject AsObject
            {
                get { return Set(new JSONObject()); }
            }
            internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
            {
                aSB.Append("null");
            }
        }
        // End of JSONLazyCreator

        public static class JSON
        {
            public static JSONNode Parse(string aJSON)
            {
                return JSONNode.Parse(aJSON);
            }
        } 
    }
}