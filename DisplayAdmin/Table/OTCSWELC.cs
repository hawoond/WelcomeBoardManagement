using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayAdmin.Table
{
    /// <summary>
    /// 방문자 환영
    /// </summary>
    public class OTCSWELC : Realms.RealmObject
    {
        /// <summary>
        /// 인덱스
        /// </summary>
        public int INDEX
        {
            get;
            set;
        }

        /// <summary>
        /// 환영 메세지
        /// </summary>
        public string WELC_MSG
        {
            get;
            set;
        }

        /// <summary>
        /// 날짜
        /// </summary>
        public DateTimeOffset WELC_DT
        {
            get;
            set;
        }
    }
}
