using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace Loan.Core.Helper.Helper
{
    public static class LinqHelper
    {
        /// <summary>
        /// 对IEnumerable 类型进行排序，通过属性的 字符串名字 如 "Name"等，可以达到动态排序的效果
        /// </summary>
        /// <typeparam name="T">需要排序的泛型类</typeparam>
        /// <param name="source">需要排序的源数据</param>
        /// <param name="sortExpression">需要排序的列名(应该和T中具有的属性对应,反之异常)</param>
        /// <param name="sortDirection">需要排序的方向</param>
        /// <returns>操作后的结果集</returns>
        public static IEnumerable<T> DataSorting<T>(this IEnumerable<T> source, string sortPropertyName, SortDirectionEnum sortDirection)
        {
            try
            {
                string sortingDir = string.Empty;
                if (sortDirection == SortDirectionEnum.ASC)
                    sortingDir = "OrderBy";
                else if (sortDirection == SortDirectionEnum.DESC)
                    sortingDir = "OrderByDescending";

                ParameterExpression param = Expression.Parameter(typeof(T), sortPropertyName);
                PropertyInfo pi = typeof(T).GetProperty(sortPropertyName);

                if (pi == null) throw new Exception(string.Format("无法进行排序,类型({0})中不存在属性{1}。", typeof(T).Name, sortPropertyName));

                Type[] types = new Type[2];
                types[0] = typeof(T);
                types[1] = pi.PropertyType;

                Expression expr = Expression.Call(typeof(Queryable), sortingDir, types, source.AsQueryable().Expression, Expression.Lambda(Expression.Property(param, sortPropertyName), param));

                IQueryable<T> query = source.AsQueryable().Provider.CreateQuery<T>(expr);
                return query;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 构建分页集合
        /// </summary>
        /// <typeparam name="T">需要排序的泛型类</typeparam>
        /// <param name="source">需要排序的源数据</param>
        /// <param name="pageNum">当前页号(默认从0开始)</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <returns>操作后的结果集</returns>
        public static IEnumerable<T> DataPaging<T>(this IEnumerable<T> source, int pageNum, int pageSize)
        {
            return source.Skip<T>(pageNum * pageSize).Take(pageSize);
        }

        /// <summary>
        /// 构建排序分页集合
        /// </summary>
        /// <typeparam name="T">需要处理的泛型类</typeparam>
        /// <param name="source">需要排序分页的源数据</param>
        /// <param name="sortExpression">需要动态排序的列名(应该和T中具有的属性对应,反之异常)</param>
        /// <param name="sortDirection">需要排序的方向(asc|desc)</param>
        /// <param name="pageNum">当前页号(默认从0开始)</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <returns>操作后的结果集</returns>
        public static IEnumerable<T> DataSortingAndPaging<T>(this IEnumerable<T> source, string sortExpression, SortDirectionEnum sortDirection, int pageNum, int pageSize)
        {
            var query = source.DataSorting<T>(sortExpression, sortDirection);
            return query.DataPaging<T>(pageNum, pageSize);
        }
    }

    /// <summary>
    /// 排序类型
    /// </summary>
    public enum SortDirectionEnum
    {
        /// <summary>
        /// 升序
        /// </summary>
        ASC,
        /// <summary>
        /// 降序
        /// </summary>
        DESC
    }
}
