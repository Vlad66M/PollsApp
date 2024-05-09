using PollsApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Application
{
    public class PagedListModel
    {
        public List<Poll> polls = new List<Poll>();
        public int currentPage;
        public int totalPages;
        public int pageSize;
        public int totalCount;
        public bool hasPrevious;
        public bool hasNext;

        public bool Test = true;
    }
}
