using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayAdmin.Table
{
    /// <summary>
    /// 급식
    /// </summary>
    public class OTCSMENU : Realms.RealmObject
    {
        /// <summary>
        /// 날짜
        /// </summary>
        public string MENU_DT
        {
            get;
            set;
        }

        /// <summary>
        /// 중식 메뉴
        /// </summary>
        public string MENU_DAY_DESC
        {
            get;
            set;
        }

        /// <summary>
        /// 석식 메뉴
        /// </summary>
        public string MENU_NIGHT_DESC
        {
            get;
            set;
        }
    }
}
