using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmoCRM
{
    public class Add
    {
        public int element_id { get; set; }
        public ElementType element_type { get; set; }
        public long complete_till_at { get; set; } // timestamp
        public TaskType task_type { get; set; }
        public string text { get; set; }
        public long created_at { get; set; } // timestamp
        public long updated_at { get; set; } // timestamp
        public int responsible_user_id { get; set; }
        public int created_by { get; set; }
    }
}
