using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayAdmin.Table
{
    /// <summary>
    /// 화면 순서
    /// </summary>
    public class OTADSCFL : Realms.RealmObject
    {
        /// <summary>
        /// 날짜
        /// </summary>
        public string SCRN_DT
        {
            get;
            set;
        }

        /// <summary>
        /// 홍보영상
        /// </summary>
        public string PROM_SEQ
        {
            get;
            set;
        }

        /// <summary>
        /// 날씨 정보
        /// </summary>
        public string WETH_SEQ
        {
            get;
            set;
        }

        /// <summary>
        /// 영어회화
        /// </summary>
        public string CONV_SEQ
        {
            get;
            set;
        }

        /// <summary>
        /// 추천도서
        /// </summary>
        public string BOOK_SEQ
        {
            get;
            set;
        }

        /// <summary>
        /// 직업
        /// </summary>
        public string JOB_SEQ
        {
            get;
            set;
        }

        /// <summary>
        /// 축하
        /// </summary>
        public string CELE_SEQ
        {
            get;
            set;
        }

        /// <summary>
        /// 생활 안전
        /// </summary>
        public string SAFE_SEQ
        {
            get;
            set;
        }

        /// <summary>
        /// 급식
        /// </summary>
        public string MENU_SEQ
        {
            get;
            set;
        }

        /// <summary>
        /// 스승의날
        /// </summary>
        public string TCDY_SEQ
        {
            get;
            set;
        }

        /// <summary>
        /// 방문환영
        /// </summary>
        public string WELC_SEQ
        {
            get;
            set;
        }

        /// <summary>
        /// 기본값 여부
        /// </summary>
        public string DEFAULT_YN
        {
            get;
            set;
        }
    }
}
