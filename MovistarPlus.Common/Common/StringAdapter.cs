using System;
using System.Text;

namespace MovistarPlus.Common
{	
    public class StringAdapter
    {
        private string stringDecorated;
        public StringAdapter(string str)
        {
            if (str == null)
                str = string.Empty;

            this.stringDecorated = str;
        }

		[ObsoleteAttribute]
		public string RemoveSpecialCharacters()
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in this.stringDecorated)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') 
                    || c == '.' || c == '_' || c == '-' || c == ',' || c == ';' || c == '+' || c == ':'
					|| c == 'ñ' || c == 'Ñ' 
                    || c == 'á' || c == 'é' || c == 'í' || c == 'ó' || c == 'ú'
                    || c == 'Á' || c == 'É' || c == 'Í' || c == 'Ó' || c == 'Ú'
                    || c == 'ü' || c == 'Ü')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        public string SubStringTruncated(int start, int numberChar = int.MaxValue)
        {
            return this.stringDecorated.SubStringTruncated(start, numberChar);
        }
	}

	public static class StringExtender
	{
		public static string SubStringTruncated(this string extended, int start, int lenthToCut = int.MaxValue)
		{
			int extendedLengh = extended.Length;
			int maxNumChar = extendedLengh >= (long)lenthToCut + start ? lenthToCut : extendedLengh - start ;
			if (maxNumChar < 0)
				return string.Empty;
			else
				return extended.Substring(start, maxNumChar);
		}

		public static string SubStringTruncated(this string extended, string startWord, string endWord = null)
		{
			int startIndex = extended.Contains(startWord) ? extended.IndexOf(startWord) + startWord.Length : 0;
			int resultLength = (endWord == null || !extended.Contains(endWord))
				? int.MaxValue : extended.IndexOf(endWord) - startIndex;
			return extended.SubStringTruncated(startIndex, resultLength);
		}
	}
}
