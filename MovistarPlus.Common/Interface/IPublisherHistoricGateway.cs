using MovistarPlus.Common.Dto;
using System.Collections.Generic;

namespace MovistarPlus.Common.Interface
{
	public interface IPublisherHistoricGateway
    {
        string GetHistoryByHistoryEvent(DeploymentHistoryEvent deploymentHistoryEvent);
        List<DeploymentHistoryEvent> GetSelectionHistory(CompleteSelection completeSelection);
        string RestoreHistory(DeploymentHistoryEvent deploymentHistoryEvent);
        bool StatusIsUpToDate();
        List<string> GetStatusModifiedFiles();
        string CommitPush(CompleteSelection completeSelection);
    }
}