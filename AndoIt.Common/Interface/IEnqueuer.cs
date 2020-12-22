using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AndoIt.Common.Interface
{
	public interface IEnqueuer
	{
        ReadOnlyCollection<IEnqueable> Queue { get; }
		int ConfigTimingSecondsMaxTimerLapse { get; set; }
		void EnqueuePetitionsFromRepository(List<IEnqueable> equeableFromRepositry);
		void InsertPetition(object sender, IEnqueable enqueable);
		void Process();
		void Dispose();
	}	
}