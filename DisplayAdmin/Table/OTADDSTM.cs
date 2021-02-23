using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayAdmin.Table
{
    /// <summary>
    /// 화면 출력 시간
    /// </summary>
    public class OTADDSTM : Realms.RealmObject
    {
        /// <summary>
        /// 날짜
        /// </summary>
        public DateTimeOffset SCRN_DT
        {
            get;
            set;
        }

        /// <summary>
        /// 홍보영상 시작 시각
        /// </summary>
        public DateTimeOffset PRMO_START_TM
        {
            get;
            set;
        }

        /// <summary>
        /// 홍보영상 종료 시각
        /// </summary>
        public DateTimeOffset PRMO_END_TM
        {
            get;
            set;
        }

        /// <summary>
        /// 날씨정보 시작 시각
        /// </summary>
        public DateTimeOffset WETH_START_TM
        {
            get;
            set;
        }

        /// <summary>
        /// 날씨 정보 종료 시각
        /// </summary>
        public DateTimeOffset WETH_END_TM
        {
            get;
            set;
        }

        /// <summary>
        /// 영어회화 시작 시각
        /// </summary>
        public DateTimeOffset CONV_START_TM
        {
            get;
            set;
        }

        /// <summary>
        /// 영어 회화 종료 시각
        /// </summary>
        public DateTimeOffset CONV_END_TM
        {
            get;
            set;
        }

        /// <summary>
        /// 추천도서 시작 시각
        /// </summary>
        public DateTimeOffset BOOK_START_TM
        {
            get;
            set;
        }

        /// <summary>
        /// 추천도서 종료 시각
        /// </summary>
        public DateTimeOffset BOOK_END_TM
        {
            get;
            set;
        }

        /// <summary>
        /// 직업 시작 시각
        /// </summary>
        public DateTimeOffset JOB_START_TM
        {
            get;
            set;
        }

        /// <summary>
        /// 직업 종료 시각
        /// </summary>
        public DateTimeOffset JOB_END_TM
        {
            get;
            set;
        }

        /// <summary>
        /// 압학, 졸업, 생일 축하 시작 시각
        /// </summary>
        public DateTimeOffset CELE_START_TM
        {
            get;
            set;
        }

        /// <summary>
        /// 압학, 졸업, 생일 축하 종료 시각
        /// </summary>
        public DateTimeOffset CELE_ENT_TM
        {
            get;
            set;
        }

        /// <summary>
        /// 생활안전 시작 시각
        /// </summary>
        public DateTimeOffset SAFE_START_TM
        {
            get;
            set;
        }

        /// <summary>
        /// 생활안전 종료 시각
        /// </summary>
        public DateTimeOffset SAFE_END_TM
        {
            get;
            set;
        }

        /// <summary>
        /// 급식 시작 시각
        /// </summary>
        public DateTimeOffset MENU_START_TM
        {
            get;
            set;
        }

        /// <summary>
        /// 급식 종료 시각
        /// </summary>
        public DateTimeOffset MENU_END_TM
        {
            get;
            set;
        }

        /// <summary>
        /// 스승의 날 시작 시각
        /// </summary>
        public DateTimeOffset TCDY_START_TM
        {
            get;
            set;
        }

        /// <summary>
        /// 스승의 날 종료 시각
        /// </summary>
        public DateTimeOffset TCDY_END_TM
        {
            get;
            set;
        }

        /// <summary>
        /// 방문 환영 시작 시각
        /// </summary>
        public DateTimeOffset WELC_START_TM
        {
            get;
            set;
        }

        /// <summary>
        /// 방문 환영 종료 시각
        /// </summary>
        public DateTimeOffset WELC_END_TM
        {
            get;
            set;
        }
    }
}
