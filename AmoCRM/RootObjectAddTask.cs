using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmoCRM
{
    public class RootObjectAddTask
    {
        public List<Add> add { get; set; }

        public RootObjectAddTask()
        {
            add = new List<Add>();
        }
    }
}
