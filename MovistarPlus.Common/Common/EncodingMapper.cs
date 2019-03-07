﻿using System;
using System.Text;

namespace MovistarPlus.Common
{
	public class EncodingMapper
	{
		public Encoding FromTextToEncoding(string encoding)
		{
			switch (encoding)
			{
				case "ASCII":
					return Encoding.ASCII;
				case "UTF8":
					return Encoding.UTF8;
				case "UTF16":
				case "Unicode":
					return Encoding.Unicode;
				case "UTF32":
					return Encoding.UTF32;
				case "UTF7":
					return Encoding.UTF7;
				default:
					throw new Exception(string.Format("Mapping Exception: Encoding '{0}' not valid", encoding));
			}
		}
	}
}