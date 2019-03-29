using System;
using System.Text;

namespace MovistarPlus.Common
{
	public class ExceptionAdapter
	{
        private Exception exceptionDecorated;
        public ExceptionAdapter(Exception ex)
        {
            this.exceptionDecorated = ex ?? throw new ArgumentNullException("ex");
        }

        public string MessageAndInnerMessages
        {
            get
            {
                StringBuilder strBuild = new StringBuilder();
                strBuild.AppendLine(this.exceptionDecorated.Message);
                if (this.exceptionDecorated.InnerException == null)
                    return strBuild.ToString();
                else
                    return strBuild.AppendLine(new ExceptionAdapter(this.exceptionDecorated.InnerException).MessageAndInnerMessages).ToString();
            }
        }
   }
}
