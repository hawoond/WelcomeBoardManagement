using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayAdmin.Table
{
    /// <summary>
    /// 직업 소개
    /// </summary>
    public class OCTSJOB : Realms.RealmObject
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
        /// 직업 이름
        /// </summary>
        public string JOB_NM
        {
            get;
            set;
        }

        /// <summary>
        /// 직업 설명
        /// </summary>
        public string JOB_DESC
        {
            get;
            set;
        }

        /// <summary>
        /// 날짜
        /// </summary>
        public DateTimeOffset JOB_DT
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

        /// <summary>
        /// 이미지 경로
        /// </summary>
        public string IMG_PATH
        {
            get;
            set;
        }
    }
}
