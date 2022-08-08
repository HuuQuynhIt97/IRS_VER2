using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IRS.Helpers
{

    /// <summary>
    /// Hierarchy node class which contains a nested collection of hierarchy nodes
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public class HierarchyNode<T> where T : class
    {
        public HierarchyNode()
        {
            ChildNodes = new List<HierarchyNode<T>>();
        }
        public T Entity { get; set; }
        public IEnumerable<HierarchyNode<T>> ChildNodes { get; set; }
        public int Depth { get; set; }
        public bool HasChildren
        {
            get { return ChildNodes.Any(); }
        }
        public T Parent { get; set; }
    }
    public static class TreeViewHelper
    {
        public static IEnumerable<T> Flatten<T>(
          this IEnumerable<T> source,
          Func<T, IEnumerable<T>> childSelector)
        {
            HashSet<T> added = new HashSet<T>();
            Queue<T> queue = new Queue<T>();
            foreach (T t in source)
                if (added.Add(t))
                    queue.Enqueue(t);
            while (queue.Count > 0)
            {
                T current = queue.Dequeue();
                yield return current;
                if (current != null)
                {
                    IEnumerable<T> children = childSelector(current);
                    if (children != null)
                        foreach (T t in childSelector(current))
                            if (added.Add(t))
                                queue.Enqueue(t);
                }
            }
        }
        private static IEnumerable<HierarchyNode<TEntity>>
     CreateHierarchy<TEntity, TProperty>(
       IEnumerable<TEntity> allItems,
       TEntity parentItem,
       Func<TEntity, TProperty> idProperty,
       Func<TEntity, TProperty> parentIDProperty,
       object rootItemID,
       int maxDepth,
       int depth) where TEntity : class
        {
            IEnumerable<TEntity> childs;

            if (rootItemID != null)
            {
                childs = allItems.Where(i => idProperty(i).Equals(rootItemID));
            }
            else
            {
                if (parentItem == null)
                {
                    childs = allItems.Where(i => parentIDProperty(i).Equals(default(TProperty)));
                }
                else
                {
                    childs = allItems.Where(i => parentIDProperty(i).Equals(idProperty(parentItem)));
                }
            }

            if (childs.Count() > 0)
            {
                depth++;

                if ((depth <= maxDepth) || (maxDepth == 0))
                {
                    foreach (var item in childs)
                        yield return
                          new HierarchyNode<TEntity>()
                          {
                              Entity = item,
                              ChildNodes =
                                CreateHierarchy(allItems.AsEnumerable(), item, idProperty, parentIDProperty, null, maxDepth, depth),
                              Depth = depth,
                              Parent = parentItem
                          };
                }
            }
        }

        /// <summary>
        /// LINQ to Objects (IEnumerable) AsHierachy() extension method
        /// </summary>
        /// <typeparam name="TEntity">Entity class</typeparam>
        /// <typeparam name="TProperty">Property of entity class</typeparam>
        /// <param name="allItems">Flat collection of entities</param>
        /// <param name="idProperty">Func delegete to ID/Key of entity</param>
        /// <param name="parentIDProperty">Func delegete to parent ID/Key</param>
        /// <returns>Hierarchical structure of entities</returns>
        public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TProperty>(
          this IEnumerable<TEntity> allItems,
          Func<TEntity, TProperty> idProperty,
          Func<TEntity, TProperty> parentIDProperty) where TEntity : class
        {
            return CreateHierarchy(allItems, default(TEntity), idProperty, parentIDProperty, null, 0, 0);
        }

        /// <summary>
        /// LINQ to Objects (IEnumerable) AsHierachy() extension method
        /// </summary>
        /// <typeparam name="TEntity">Entity class</typeparam>
        /// <typeparam name="TProperty">Property of entity class</typeparam>
        /// <param name="allItems">Flat collection of entities</param>
        /// <param name="idProperty">Func delegete to ID/Key of entity</param>
        /// <param name="parentIDProperty">Func delegete to parent ID/Key</param>
        /// <param name="rootItemID">Value of root item ID/Key</param>
        /// <returns>Hierarchical structure of entities</returns>
        public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TProperty>(
          this IEnumerable<TEntity> allItems,
          Func<TEntity, TProperty> idProperty,
          Func<TEntity, TProperty> parentIDProperty,
          object rootItemID) where TEntity : class
        {
            return CreateHierarchy(allItems, default(TEntity), idProperty, parentIDProperty, rootItemID, 0, 0);
        }

        /// <summary>
        /// LINQ to Objects (IEnumerable) AsHierachy() extension method
        /// </summary>
        /// <typeparam name="TEntity">Entity class</typeparam>
        /// <typeparam name="TProperty">Property of entity class</typeparam>
        /// <param name="allItems">Flat collection of entities</param>
        /// <param name="idProperty">Func delegete to ID/Key of entity</param>
        /// <param name="parentIDProperty">Func delegete to parent ID/Key</param>
        /// <param name="rootItemID">Value of root item ID/Key</param>
        /// <param name="maxDepth">Maximum depth of tree</param>
        /// <returns>Hierarchical structure of entities</returns>
        public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TProperty>(
          this IEnumerable<TEntity> allItems,
          Func<TEntity, TProperty> idProperty,
          Func<TEntity, TProperty> parentIDProperty,
          object rootItemID,
          int maxDepth) where TEntity : class
        {
            return CreateHierarchy(allItems, default(TEntity), idProperty, parentIDProperty, rootItemID, maxDepth, 0);
        }

        private static IEnumerable<HierarchyNode<TEntity>>
    CreateHierarchy<TEntity>(IQueryable<TEntity> allItems,
      TEntity parentItem,
      string propertyNameID,
      string propertyNameParentID,
      object rootItemID,
      int maxDepth,
      int depth) where TEntity : class
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "e");
            Expression<Func<TEntity, bool>> predicate;

            if (rootItemID != null)
            {
                Expression left = Expression.Property(parameter, propertyNameID);
                left = Expression.Convert(left, rootItemID.GetType());
                Expression right = Expression.Constant(rootItemID);

                predicate = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(left, right), parameter);
            }
            else
            {
                if (parentItem == null)
                {
                    predicate =
                      Expression.Lambda<Func<TEntity, bool>>(
                        Expression.Equal(Expression.Property(parameter, propertyNameParentID),
                                         Expression.Constant(null)), parameter);
                }
                else
                {
                    Expression left = Expression.Property(parameter, propertyNameParentID);
                    left = Expression.Convert(left, parentItem.GetType().GetProperty(propertyNameID).PropertyType);
                    Expression right = Expression.Constant(parentItem.GetType().GetProperty(propertyNameID).GetValue(parentItem, null));

                    predicate = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(left, right), parameter);
                }
            }

            IEnumerable<TEntity> childs = allItems.Where(predicate).ToList();

            if (childs.Count() > 0)
            {
                depth++;

                if ((depth <= maxDepth) || (maxDepth == 0))
                {
                    foreach (var item in childs)
                        yield return
                          new HierarchyNode<TEntity>()
                          {
                              Entity = item,
                              ChildNodes =
                              CreateHierarchy(allItems, item, propertyNameID, propertyNameParentID, null, maxDepth, depth),
                              Depth = depth,
                              Parent = parentItem
                          };
                }
            }
        }

        /// <summary>
        /// LINQ to SQL (IQueryable) AsHierachy() extension method
        /// </summary>
        /// <typeparam name="TEntity">Entity class</typeparam>
        /// <param name="allItems">Flat collection of entities</param>
        /// <param name="propertyNameID">String with property name of ID/Key</param>
        /// <param name="propertyNameParentID">String with property name of parent ID/Key</param>
        /// <returns>Hierarchical structure of entities</returns>
        public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity>(
          this IQueryable<TEntity> allItems,
          string propertyNameID,
          string propertyNameParentID) where TEntity : class
        {
            return CreateHierarchy(allItems, null, propertyNameID, propertyNameParentID, null, 0, 0);
        }

        /// <summary>
        /// LINQ to SQL (IQueryable) AsHierachy() extension method
        /// </summary>
        /// <typeparam name="TEntity">Entity class</typeparam>
        /// <param name="allItems">Flat collection of entities</param>
        /// <param name="propertyNameID">String with property name of ID/Key</param>
        /// <param name="propertyNameParentID">String with property name of parent ID/Key</param>
        /// <param name="rootItemID">Value of root item ID/Key</param>
        /// <returns>Hierarchical structure of entities</returns>
        public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity>(
          this IQueryable<TEntity> allItems,
          string propertyNameID,
          string propertyNameParentID,
          object rootItemID) where TEntity : class
        {
            return CreateHierarchy(allItems, null, propertyNameID, propertyNameParentID, rootItemID, 0, 0);
        }

        /// <summary>
        /// LINQ to SQL (IQueryable) AsHierachy() extension method
        /// </summary>
        /// <typeparam name="TEntity">Entity class</typeparam>
        /// <param name="allItems">Flat collection of entities</param>
        /// <param name="propertyNameID">String with property name of ID/Key</param>
        /// <param name="propertyNameParentID">String with property name of parent ID/Key</param>
        /// <param name="rootItemID">Value of root item ID/Key</param>
        /// <param name="maxDepth">Maximum depth of tree</param>
        /// <returns>Hierarchical structure of entities</returns>
        public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity>(
          this IQueryable<TEntity> allItems,
          string propertyNameID,
          string propertyNameParentID,
          object rootItemID,
          int maxDepth) where TEntity : class
        {
            return CreateHierarchy(allItems, null, propertyNameID, propertyNameParentID, rootItemID, maxDepth, 0);
        }
    }

}

