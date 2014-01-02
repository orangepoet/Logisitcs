using System;
using System.Collections.Generic;
using System.Linq;

namespace Logistics.Infrastructure {
    public class PagedList<T> : List<T> {
        private int pageIndex;
        public int PageIndex {
            get { return pageIndex; }
            set {
                if (value < 1) {
                    value = 1;
                } else if (value > PageCount) {
                    value = PageCount;
                }
                pageIndex = value;
            }
        }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int TotalCount { get; set; }

        public bool HasPreviousPage {
            get { return PageIndex > 1; }
        }
        public bool HasNextPage {
            get { return PageIndex < PageCount; }
        }

        public PagedList(IList<T> source, int pageIndex, int pageSize) {
            IList<string> var = new List<string>();
            TotalCount = source.Count;
            PageSize = pageSize;
            PageCount = (int)Math.Ceiling((double)source.Count() / PageSize);
            PageIndex = pageIndex;

            this.AddRange(source.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList());
        }
    }
}