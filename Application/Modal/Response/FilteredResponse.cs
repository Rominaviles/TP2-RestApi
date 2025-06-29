using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modal.Response
{
    public class FilteredResopnse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public int Duration { get; set; }
        public string Area { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
    }

}
