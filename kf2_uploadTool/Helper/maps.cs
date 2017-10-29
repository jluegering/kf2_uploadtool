using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kf2_uploadTool.Helper
{
    class maps
    {
        public string path { get; set; }
        public string name { get; set; }

        public maps (string path, string name)
        {
            this.path = path;
            this.name = name;
        }
    }
}
