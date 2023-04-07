using System.Linq.Expressions;
using System.Reflection;

namespace Kernel
{
    public abstract class Enumeration : Entity, IComparable
    {
        protected Enumeration(int id, string displayName) 
        { 
            Id = id;
            DisplayName = displayName;
        }
        protected Enumeration()
        {

        }
        public int CompareTo(object obj)
        {
            return Id.CompareTo(((Enumeration)obj).Id);
        }

        public static bool operator ==(Enumeration left, Enumeration right)
        {
            if (object.Equals(left, null))
                return (object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Enumeration left, Enumeration right)
        {
            return !(left == right);
        }

        public static ICollection<string> ValidateId<T>(int id) where T : Enumeration
        {
            List<string> errors = new List<string>();
            var exists = GetAll<T>().Any(im => im.Id == id);
            if (!exists)
            {
                errors.Add(
                    $"{typeof(T).Name} Id [{id}] does not exist");
            }
            return errors;
        }

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                         .Where(fi => fi.FieldType == typeof(T));

            foreach (var info in fields)
            {
                yield return info.GetValue(null) as T;
            }
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;

            if (otherValue == null)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Id - secondValue.Id);
            return absoluteDifference;
        }

        public static T FromValue<T>(int value, bool returnNullAndDontThrowIfNotFound = false) where T : Enumeration //, new()
        {
            try
            {
                var matchingItem = Parse<T, int>(value, "value", item => item.Id == value);
                return matchingItem;
            }
            catch (Exception e)
            {
                if (returnNullAndDontThrowIfNotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }

            }
        }

        public static T FromDisplayName<T>(string displayName) where T : Enumeration
        {
            var matchingItem = Parse<T, string>(displayName, "DisplayName", item => item.DisplayName == displayName);
            return matchingItem;
        }

        public static T FromCustomField<T, TProperty>(Expression<Func<T, TProperty>> dataValueField, string dataValue)
            where T : Enumeration
        {
            //TODO : Check for data member type. Ensure it is string
            var memberName = GetMemberInfo(dataValueField).Name;

            var parameter = Expression.Parameter(typeof(T), "entity");
            var member = Expression.Property(parameter, memberName);
            var constant = Expression.Constant(dataValue);
            var body = Expression.Equal(member, constant);
            var finalExpression = Expression.Lambda<Func<T, bool>>(body, parameter);


            var matchingItem = Enumeration.Parse<T, string>(dataValue, memberName, finalExpression.Compile());
            return matchingItem;
        }

        private static MemberInfo GetMemberInfo<TObject, TProperty>(Expression<Func<TObject, TProperty>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member != null)
            {
                return member.Member;
            }
            throw new ArgumentException("Member does not exist.");
        }

        internal static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                var message = $"Cannot find a matching enumeration value in {typeof(T)} where {description} = {value}";

                throw new InvalidOperationException(message);
            }

            return matchingItem;
        }

        public override string ToString()
        {
            return DisplayName;
        }
        public string DisplayName { get; private set; }
    }
}
