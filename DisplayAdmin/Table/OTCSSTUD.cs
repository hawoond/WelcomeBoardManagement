using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayAdmin.Table
{
    /// <summary>
    /// 학생정보
    /// </summary>
    public class OTCSSTUD : Realms.RealmObject
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
        /// 학생 이름
        /// </summary>
        public string STD_NM
        {
            get;
            set;
        }

        /// <summary>
        /// 학년
        /// </summary>
        public string STD_GRADE
        {
            get;
            set;
        }

        /// <summary>
        /// 반
        /// </summary>
        public string STD_CLASS
        {
            get;
            set;
        }

        /// <summary>
        /// 출석번호
        /// </summary>
        public string STD_NO
        {
            get;
            set;
        }

        /// <summary>
        /// 생일
        /// </summary>
        public string STD_BIRTHDAY
        {
            get;
            set;
        }
    }
}
