using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayAdmin.Table
{
    public class OTADHSTRY : Realms.RealmObject
    {
        /// <summary>
        /// 테이블 명
        /// </summary>
        public string TB_NM
        {
            get;
            set;
        }

        /// <summary>
        /// 아이템 인덱스
        /// </summary>
        public int INDEX
        {
            get;
            set;
        }

        /// <summary>
        /// 지정일
        /// </summary>
        public string VIEW_DT
        {
            get;
            set;
        }

        /// <summary>
        /// 입력자
        /// </summary>
        public string ENT_STF
        {
            get;
            set;
        }
    }
}
