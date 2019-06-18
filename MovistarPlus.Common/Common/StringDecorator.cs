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

        public string RemoveSpecialCharacters()
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in this.stringDecorated)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') 
                    || c == '.' || c == '_' || c == '-' || c == ',' || c == ':' || c == ';' || c == '+'
					|| c == 'ñ' || c == 'Ñ' 
                    || c == 'á' || c == 'é' || c == 'í' || c == 'ó' || c == 'ú'
                    || c == 'Á' || c == 'É' || c == 'Í' || c == 'Ó' || c == 'Ú'
                    || c == 'ü' || c == 'Ú')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        public string SubStringTruncated(int start, int numberChar)
        {
            int lengh = this.stringDecorated.Length;
            int maxNumChar = lengh >= numberChar + start ? numberChar : lengh - start ;
            if (maxNumChar < 0)
                return string.Empty;
            else
                return this.stringDecorated.Substring(start, maxNumChar);
        }
    }
}
