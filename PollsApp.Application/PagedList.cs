using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Application
{
    public class PagedList<T>
    {
        public List<T> data { get; set; } = new();
        public int currentPage;
        public int totalPages;
        public int pageSize;
        public int totalCount;
        public bool hasPrevious;
        public bool hasNext;
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            totalCount = count;
            this.pageSize = pageSize;
            currentPage = pageNumber;
            totalPages = (int)Math.Ceiling(count / (double)pageSize);
            hasPrevious = currentPage > 1;
            hasNext = currentPage < totalPages;
            data.AddRange(items);
        }

        public PagedList()
        {


        }
        public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
