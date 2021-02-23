using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayAdmin.Table
{
    /// <summary>
    /// 스승의 날 축하
    /// </summary>
    public class OTCSTCDY : Realms.RealmObject
    {
        public int INDEX
        {
            get;
            set;
        }

        public string CELE_MSG
        {
            get;
            set;
        }

        public DateTimeOffset CELE_DT
        {
            get;
            set;
        }


    }
}
