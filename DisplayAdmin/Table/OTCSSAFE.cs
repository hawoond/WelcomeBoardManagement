using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayAdmin.Table
{
    /// <summary>
    /// 생활 안전 정보
    /// </summary>
    public class OTCSSAFE : Realms.RealmObject
    {
        public int INDEX
        {
            get;
            set;
        }

        public string SAFE_TITLE
        {
            get;
            set;
        }

        public string SAFE_DESC
        {
            get;
            set;
        }

        public DateTimeOffset SAFE_DT
        {
            get;
            set;
        }

        public string ENT_STF
        {
            get;
            set;
        }
    }
}
