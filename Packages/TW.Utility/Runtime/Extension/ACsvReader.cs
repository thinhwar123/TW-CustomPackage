using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

namespace TW.Utility.Extension
{
	public class ACsvReader
	{
		/// <summary>
		/// Read the specified data.
		/// </summary>
		public static List<Dictionary<string, string>> Read(TextAsset data)
		{
			CultureInfo ci = new CultureInfo("en-us");
			Thread.CurrentThread.CurrentCulture = ci;
			Thread.CurrentThread.CurrentUICulture = ci;
			var list = new List<Dictionary<string, string>>();
			if (data == null)
			{
				return list;
			}

			string[] lines = Regex.Split(data.text, @"\r\n|\n\r|\n|\r").Where(value => value != "").ToArray();
			if (lines.Length <= 1)
				return list;

			string[] header = Regex.Split(lines[0], ",");
			for (var i = 1; i < lines.Length; i++)
			{

				var values = Regex.Split(lines[i], ",");
				if (values.Length == 0 || values[0] == "")
					continue;

				var entry = new Dictionary<string, string>();
				for (var j = 0; j < header.Length && j < values.Length; j++)
				{
					string value = values[j];
					value = value.TrimStart('\"').TrimEnd('\"');
					entry[header[j]] = value;
				}

				list.Add(entry);
			}

			return list;
		}

		public static List<Dictionary<string, string>> ReadDataFromString(string data)
		{
			CultureInfo ci = new CultureInfo("en-us");
			Thread.CurrentThread.CurrentCulture = ci;
			Thread.CurrentThread.CurrentUICulture = ci;
			var list = new List<Dictionary<string, string>>();
			if (data == null)
			{
				return list;
			}

			string[] lines = Regex.Split(data, @"\r\n|\n\r|\n|\r").Where(value => value != "").ToArray();
			if (lines.Length <= 1)
				return list;

			string[] header = Regex.Split(lines[0], ",");
			for (var i = 1; i < lines.Length; i++)
			{
				var values = Regex.Split(lines[i], ",");
				if (values.Length == 0)
					continue;

				var entry = new Dictionary<string, string>();
				for (var j = 0; j < header.Length && j < values.Length; j++)
				{
					string value = values[j];
					value = value.TrimStart('\"').TrimEnd('\"');
					entry[header[j]] = value;
				}

				list.Add(entry);
			}

			return list;
		}
	}
}
