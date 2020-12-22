using System;

namespace AndoIt.Common.Interface
{
    public abstract class IEnqueable : IEquatable<IEnqueable>
    {
        public abstract string Id { get; }
        public abstract DateTime WhenToHandleNext { get; }
        public EnqueableState State { get; protected set; } = EnqueableState.Pending;

        public abstract void Handle();
        public bool Equals(IEnqueable enqueable)
        {
            return this.Id == enqueable.Id;
        }

        public enum EnqueableState
        {
            Pending,
            HandledOk,
            Error
        }
    }
}