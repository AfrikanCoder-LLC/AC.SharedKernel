using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kernel
{
    public abstract class ReferenceEntity : AggregateRoot
    {
        protected ReferenceEntity() 
        {
        }
        protected ReferenceEntity(string displayName)
        {
            DisplayName = displayName ?? string.Empty;
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public static IEnumerable<T> GetAll<T>() where T : ReferenceEntity
        {
            var type = typeof(T);
            var valuesField = type.GetFields();

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(fi => fi.FieldType == typeof(T));

            foreach (var info in fields)
            {
                yield return info.GetValue(null) as T;               
            }
        }

        public string DisplayName { get; protected set; }
    }
}
