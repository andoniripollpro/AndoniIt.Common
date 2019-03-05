using System.ComponentModel;

namespace MovistarPlus.Common.Dto
{
    public enum EnvironmentDestinationEnum
	{
        [Description("prod")]
        Production,
        [Description("pre")]
        Preproduction,
        [Description("lab")]
        Laboratory
    }
}
