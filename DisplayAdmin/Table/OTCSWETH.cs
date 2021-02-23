using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayAdmin.Table
{
    /// <summary>
    /// 날씨
    /// </summary>
    public class OTCSWETH : Realms.RealmObject
    {
        public string WETH_DT_STATE
        {
            get;
            set;
        }

        public string WETH_TEMP
        {
            get;
            set;
        }

        public string WETH_POP
        {
            get;
            set;
        }

        public string WETH_FDUST
        {
            get;
            set;
        }

        public string WETH_UFDUST
        {
            get;
            set;
        }

        public string WETH_SENSTEMP
        {
            get;
            set;
        }

        public string WETH_STATE
        {
            get;
            set;
        }
    }
}
