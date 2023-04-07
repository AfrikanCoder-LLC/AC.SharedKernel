using MediatR;
using TrackableEntities.Common.Core;

namespace Kernel
{
    /// <summary>
    /// Enforces comparison operations so that the Id of the Entity is used. Often equality of Domain Objects 
    /// is mistakenly used and reference and actual equality are confused. 
    /// The question of identity is discussed by Evans
    /// Also See : https://martinfowler.com/eaaCatalog/separatedInterface.html
    /// </summary>
    public abstract class Entity : ITrackable, IMergeable
    {
        protected  Entity() 
        { 

        }

        int? _requestedHashCode;
        int _id;
        public virtual int Id { get { return _id; } protected set { _id = value; } }
        public void ResetAsNew()
        {
            Id = 0;
            TrackingState = TrackingState.Added;
        }
        private List<INotification> _domainEvents;
        public List<INotification> DomainEvents => _domainEvents;

        public void AddDomainEvent(INotification domainEvent)
        {
            _domainEvents ??= new List<INotification>();
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(INotification domainEvent)
        {
            if (_domainEvents is null) return;
            _domainEvents.Remove(domainEvent);
        }

        /// <summary>
        /// Returns TRUE if the item has not yet been persisted
        /// </summary>
        /// <returns></returns>
        public bool IsTransient()
        {
            return Id == default;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            Entity item = (Entity)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.Id == this.Id;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = this.Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();
        }

        public static bool operator ==(Entity left, Entity right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }

        #region Trackable Entities
        
        public TrackingState TrackingState { get; set; }
        public ICollection<string> ModifiedProperties { get; set; }
        public Guid EntityIdentifier { get; set; }

        #endregion
    }
}
