using PollsApp.Application.DTOs;
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
        public List<PollDto> polls = new List<PollDto>();
        public int currentPage;
        public int totalPages;
        public int pageSize;
        public int totalCount;
        public bool hasPrevious;
        public bool hasNext;

        public bool Test = true;
    }
}
