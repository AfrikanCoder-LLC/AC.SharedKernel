using System.ComponentModel.DataAnnotations.Schema;
using TrackableEntities.Common.Core;

namespace Kernel
{
    public abstract class ValueObject : ITrackable, IMergeable
    {
        protected ValueObject(bool hasValue) 
        { 
            HasValue = hasValue;
        }

        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }
            return ReferenceEquals(left, null) || left.Equals(right);
        }


        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !(EqualOperator(left, right));
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }
            ValueObject other = (ValueObject)obj;
            IEnumerator<object> thisValues = GetAtomicValues().GetEnumerator();
            IEnumerator<object> otherValues = other.GetAtomicValues().GetEnumerator();
            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (ReferenceEquals(thisValues.Current, null) ^ ReferenceEquals(otherValues.Current, null))
                {
                    return false;
                }
                if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
                {
                    return false;
                }
            }
            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        public override int GetHashCode()
        {
            return GetAtomicValues()
             .Select(x => x != null ? x.GetHashCode() : 0)
             .Aggregate((x, y) => x ^ y);
        }

        public ValueObject GetCopy()
        {
            return this.MemberwiseClone() as ValueObject;
        }

        protected abstract IEnumerable<object> GetAtomicValues();
        public bool HasValue { get; protected set; }

        #region Trackable Entities
        [NotMapped]
        public TrackingState TrackingState { get; set; }
        
        [NotMapped]
        public ICollection<string> ModifiedProperties { get; set; }
        
        [NotMapped]
        public Guid EntityIdentifier { get; set; }

        #endregion

    }
}
