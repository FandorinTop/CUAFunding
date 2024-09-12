using System;
using System.Collections.Generic;

namespace CUAFunding.ViewModels
{
    public class ApiResult<T>
    {
        #region Properties
        public List<T> Data { get; private set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }
        public bool HasNextPage
        {
            get
            {
                return ((PageIndex + 1) < TotalPages);
            }
        }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public string FilterColumn { get; set; }
        public string FilterQuery { get; set; }
        #endregion
        public ApiResult(
            List<T> data,
            int pageIndex,
            int pageSize,
            int totalCount,
            string sortColumn,
            string sortOrder,
            string filterColumn,
            string filterQuery)
        {
            Data = data;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);
            SortColumn = sortColumn;
            SortOrder = sortOrder;
            FilterColumn = filterColumn;
            FilterQuery = filterQuery;
        }
    }
}
