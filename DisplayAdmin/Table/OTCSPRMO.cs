using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayAdmin.Table
{
    /// <summary>
    /// 프로모션 무비
    /// </summary>
    public class OTCSPRMO : Realms.RealmObject
    {
        public int INDEX
        {
            get;
            set;
        }

        public string MO_NM
        {
            get;
            set;
        }

        public string MO_PATH
        {
            get;
            set;
        }

        public DateTimeOffset MO_DT
        {
            get;
            set;
        }
    }
}
