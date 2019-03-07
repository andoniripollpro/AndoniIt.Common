using System.Collections.Generic;
using MovistarPlus.Common.Dto;

namespace MovistarPlus.Common.Interface
{
	public interface IPublisherService
    {
        List<string> AskAnythingNotMatching(CompleteSelection completeSelection);
        string GetFileContentFromDestination(ConfigFileSelection configFileSelection);
        string GetFileContentFromHistoric(DeploymentHistoryEvent deploymentHistoryEvent);
        string GetFileContentFromLocal(ConfigFileSelection configFileSelection);
        List<string> GetModifiedFiles();
        string Publish(CompleteSelection completeSelection);
        void RefreshCaches(CompleteSelection completeSelection);
    }
}