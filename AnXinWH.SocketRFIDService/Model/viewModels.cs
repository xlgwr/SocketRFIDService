using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnXinWH.SocketRFIDService.Model
{   
    public class CheckTime
    {
        public string checktime { get; set; }
        public string checktimePre { get; set; }

        public DateTime checktimeNow { get; set; }
        public bool isIn { get; set; }
    }
}
