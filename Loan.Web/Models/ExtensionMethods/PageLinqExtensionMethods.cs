using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace Loan.Web.Models
{
    public static class PageLinqExtensionMethods
    {
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> currentPageItems, int pageIndex, int pageSize, int totalItemCount)
        {
            return currentPageItems.AsQueryable<T>().ToPagedList<T>(pageIndex, pageSize, totalItemCount);
        }

        public static PagedList<T> ToPagedList<T>(this IQueryable<T> currentPageItems, int pageIndex, int pageSize, int totalItemCount)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int count = (pageIndex - 1) * pageSize;
            while ((totalItemCount <= count) && (pageIndex > 1))
            {
                count = (--pageIndex - 1) * pageSize;
            }
            return new PagedList<T>(currentPageItems, pageIndex, pageSize, totalItemCount);
        }
    }
}