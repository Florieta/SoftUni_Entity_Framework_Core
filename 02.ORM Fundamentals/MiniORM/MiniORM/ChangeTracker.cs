using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace MiniORM
{
	class ChangeTracker<T>
		where T : class, new()
	{
		private readonly List<T> allEntities;

		private readonly List<T> added;

		private readonly List<T> removed;


		public ChangeTracker(IEnumerable<T> entities)
		{
			this.added = new List<T>();

			this.removed = new List<T>();

			this.allEntities = CloneEntities(entities);
		}
		//Current state of the entities in the memory
		public IReadOnlyCollection<T> AllEntities => this.allEntities.AsReadOnly();
		//State of the added entities
		public IReadOnlyCollection<T> Added => this.added.AsReadOnly();
		//State of the removed entities (logically, not physically)
		public IReadOnlyCollection<T> Removed => this.removed.AsReadOnly();


		public void Add(T item) => this.added.Add(item);

		public void Remove(T item) => this.removed.Add(item);


		public IEnumerable<T> GetModifiedEntities(DbSet<T> dbSet)
        {
			List<T> modifiedEntities = new List<T>();

			PropertyInfo[] primaryKeys = typeof(T)
				.GetProperties()
				.Where(pi => pi.HasAttribute<KeyAttribute>())
				.ToArray();

            foreach (T proxyEntity in this.AllEntities)
            {
				object[] primaryKeyValues = GetPrimaryKeysValues(primaryKeys, proxyEntity).ToArray();

				T dbEntity = dbSet
					.Entities
					.Single(e => GetPrimaryKeysValues(primaryKeys, e).SequenceEqual(primaryKeyValues));

				bool isModified = IsModified (proxyEntity, dbEntity);
				if(isModified)
                {
					modifiedEntities.Add(dbEntity)
                }
            }

			return modifiedEntities;
        }
		private static List<T> CloneEntities(IEnumerable<T> entities)
		{
			List<T> clonedEntities = new List<T>();

			PropertyInfo[] propertiesToClone = typeof(T)
				.GetProperties()
				.Where(pi => DbContext.AllowedSqlTypes.Contains(pi.PropertyType))
				.ToArray();

			foreach (T entity in entities)
			{
				T clonedEntity = Activator.CreateInstance<T>();
				foreach (PropertyInfo property in propertiesToClone)
				{
					object value = property.GetValue(entity);
					property.SetValue(clonedEntity, value);
				}

				clonedEntities.Add(clonedEntity);
			}

			return clonedEntities;
		}

		private static IEnumerable<object> GetPrimaryKeysValues(IEnumerable<PropertyInfo> primaryKeys, T entity)
			=> primaryKeys.Select(pk => pk.GetValue(entity));

		private static bool IsModified(T proxyEntity, T dbEntity)
		{
			PropertyInfo[] monitoredProperties = typeof(T)
				.GetProperties()
				.Where(pi => DbContext.AllowedSqlTypes.Contains(pi.PropertyType))
				.ToArray();

			PropertyInfo[] modifiedProperties = monitoredProperties
				.Where(pi => !Equals(pi.GetValue(proxyEntity), pi.GetValue(dbEntity)))
				.ToArray();
			return modifiedProperties.Any();
		}

        
	}

}