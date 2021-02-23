using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayAdmin.Table
{
    /// <summary>
    /// 영어회화
    /// </summary>
    public class OTCSCONV : Realms.RealmObject
    {
        public int INDEX
        {
            get;
            set;
        }

        public string CONV_TITLE
        {
            get;
            set;
        }

        public string CONV_TRANS
        {
            get;
            set;
        }

        public DateTimeOffset CONV_DT
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
