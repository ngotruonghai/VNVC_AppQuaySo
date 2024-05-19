using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppQuaySo.Model
{
    public class BaseModel
    {
        public string? Activity { get; set; }

        public void SetActivity(string activity)
        {
            this.Activity = activity;
        }
    }
}
