using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kf2_uploadTool.Helper
{
    public class config
    {
        public Server server { get; set; }
        public Redirect redirect { get; set; }
    }

    public class Server
    {
        public string address { get; set; }
        public int port { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string path { get; set; }
    }

    public class Redirect
    {
        public string address { get; set; }
        public int port { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string path { get; set; }
    }

}
