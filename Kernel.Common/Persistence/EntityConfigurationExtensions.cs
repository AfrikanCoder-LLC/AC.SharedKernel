using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace Kernel.Common.Persistence
{
    public static class EntityConfigurationExtensions
    {

        /// <summary>
        /// Runs through fields in an entity and appends the type name of the derived calss for all fields that are not inherited. IE That belong directly to that derived class
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entityTypeConfiguration"></param>
        public static void RenameDerivedEntityFields<TEntity>(this EntityTypeBuilder<TEntity> entityTypeConfiguration) where TEntity : Entity
        {
            var x = entityTypeConfiguration.Metadata.ClrType;
            foreach (var mutableProperty in entityTypeConfiguration.Metadata.GetProperties())
            {
                var property = entityTypeConfiguration.Property(mutableProperty.Name);
                if (property.Metadata.DeclaringEntityType.ClrType == x)
                {
                    property.HasColumnName($"{typeof(TEntity).Name}_{property.Metadata.Name}");
                }
            }
        }


        /// <summary>
        /// Allows a field to be configured so that a backing id field is added. Necessary for DDD decoupling between aggragates. Pass Id's instead of objects
        /// In the domain entity, it is necessary to define a field such as _ExternalReferenceId and to assign this field in the factory methods for the entity  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TRelatedEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="expr"></param>
        public static void ConfigureListBackedField<T, TRelatedEntity>(this EntityTypeBuilder<T> entity,
            Expression<Func<T, TRelatedEntity>> expr) where T : Entity where TRelatedEntity : IEnumerable<Entity>
        {
            var member = expr.Body as MemberExpression;
            var shadowMemberName = $"_{Char.ToLowerInvariant(member.Member.Name[0]) + member.Member.Name.Substring(1)}";

            var navigation = entity.Metadata.FindNavigation(member.Member.Name);

            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

        }


        /// <summary>
        /// Allows a field to be configured so that a backing id field is added. Necessary for DDD decoupling between aggragates. Pass Id's instead of objects
        /// In the domain entity, it is necessary to define a field such as _entityId and to assign this field in the factory methods for the entity
        /// Here we configure a shadow property, assign it to the backing field defined in the entity and then set property access mode to field.    
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TRelatedEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="expr"></param>
        public static void ConfigureIdBackedField<T, TRelatedEntity>(this EntityTypeBuilder<T> entity,
            Expression<Func<T, TRelatedEntity>> expr, bool required) where T : Entity where TRelatedEntity : Entity
        {
            var member = expr.Body as MemberExpression;
            var backingFieldName = $"_{Char.ToLowerInvariant(member.Member.Name[0]) + member.Member.Name.Substring(1)}Id";
            var propertyName = $"{member.Member.Name}Id";

            if (required)
            {
                entity.Property<int>(propertyName)
                    .HasField(backingFieldName)
                    .UsePropertyAccessMode(PropertyAccessMode.Field)
                    .IsRequired();
            }
            else
            {
                entity.Property<int?>(propertyName)
                    .HasField(backingFieldName)
                    .UsePropertyAccessMode(PropertyAccessMode.Field);
            }

            entity.HasOne(expr)
                .WithMany()
                .HasForeignKey(propertyName);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Expression<Func<TEntity, bool>> BuildLambdaForFindByKey<TEntity>(int id)
        {
            var item = Expression.Parameter(typeof(TEntity), "entity");
            var prop = Expression.Property(item, typeof(TEntity).Name + "Id");
            var value = Expression.Constant(id);
            var equal = Expression.Equal(prop, value);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, item);
            return lambda;
        }        
    }
}
