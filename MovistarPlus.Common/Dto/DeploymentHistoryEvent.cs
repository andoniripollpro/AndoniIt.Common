using System;
using System.Collections.Generic;

namespace MovistarPlus.Common.Dto
{
    public class DeploymentHistoryEvent
    {
        public List<ConfigFileSelection> ConfigFileSelection { get; set; } = new List<Dto.ConfigFileSelection>();
        public DateTime DeploymentDate { get; set; }
        public string CommitId { get; set; }
        public string Comments { get; set; }
        public string Author { get; set; }
    }
}
