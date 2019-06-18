﻿using System.Collections.Generic;
using System.Text;

namespace MovistarPlus.Common
{
	public static class GenericExtensions
	{
		public static string BetterToString(this List<string> list, bool withSimpleQuotation = true)
		{
			StringBuilder strBuilder = new StringBuilder("(");
			if (withSimpleQuotation)
				list.ForEach(x => strBuilder.Append("'").Append(x.Trim()).Append("',"));
			else
				list.ForEach(x => strBuilder.Append(x.Trim()).Append(","));
			if (strBuilder.Length > 1)
				strBuilder.Remove(strBuilder.Length - 1, 1);
			strBuilder.Append(")");
			return strBuilder.ToString();
		}

		public static string BetterToString(this List<int> list)
		{
			List<string> stringList = new List<string>();
			list.ForEach(x => stringList.Add(x.ToString()));
			return stringList.BetterToString(false);
		}
	}
}