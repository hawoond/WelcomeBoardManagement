using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayAdmin.Table
{
    /// <summary>
    /// 책 정보
    /// </summary>
    public class OTCSBOOK : Realms.RealmObject
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
        /// 책 이름
        /// </summary>
        public string BOOK_NM
        {
            get;
            set;
        }

        /// <summary>
        /// 저자
        /// </summary>
        public string BOOK_AUTH
        {
            get;
            set;
        }

        /// <summary>
        /// 책 설명
        /// </summary>
        public string BOOK_DESC
        {
            get;
            set;
        }

        /// <summary>
        /// 이미지 경로
        /// </summary>
        public string IMG_PATH
        {
            get;
            set;
        }

        /// <summary>
        /// 날짜
        /// </summary>
        public DateTimeOffset BOOK_DT
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
