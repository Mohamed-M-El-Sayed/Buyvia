using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Application.Common.Models
{
    public class PageResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages {get; set;}
        public int PageSize {  get; set;}
        public int PageIndex {  get; set;}
        public PageResult(IEnumerable<T> items , int pageIndex , int pageSize , int totalCount)
        {
            Items = items;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
