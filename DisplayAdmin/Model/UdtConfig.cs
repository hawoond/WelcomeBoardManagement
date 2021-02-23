using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DisplayAdmin.Model
{
    public class UdtConfig
    {
        public List<Common.Constants.ScreenIndex> ScreenFlow;

        public string SCH_NM;

        public int SCR_INTERVAL;

        public string currentDate;

        public Dictionary<Common.Constants.ScreenIndex, UdtDisplayTime> arrDisplayTime;
    }
}
