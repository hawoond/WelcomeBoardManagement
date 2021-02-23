using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayAdmin.Table
{
    /// <summary>
    /// 설정
    /// </summary>
    public class OTADCONF : Realms.RealmObject
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
        /// 학교이름
        /// </summary>
        public string SCH_NM
        {
            get;
            set;
        }

        /// <summary>
        /// 화면 전환 시간
        /// </summary>
        public int SCR_INTERVAL
        {
            get;
            set;
        }
    }
}
