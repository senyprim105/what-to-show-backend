using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server_api.Data.ViewModels
{
    public class CheckBoxListViewModel
    {
        public string Caption { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Id { get; set; }
        public bool? Checked { get; set; } = false;
        public bool? Enabled { get; set; } = false;
        public bool? Visible { get; set; } = false;
    }
}
