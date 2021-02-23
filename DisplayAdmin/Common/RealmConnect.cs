using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayAdmin.Common
{
    public class RealmConnect
    {
        private Realm realm;
        private string mRealmPath = "C:\\DisplayBoard.realm";
        public RealmConnect()
        {
            var config = new RealmConfiguration(mRealmPath);
            realm = Realm.GetInstance(config);
        }  

        public Realm GetRealm()
        {
            if (realm == null)
            {
                var config = new RealmConfiguration(mRealmPath);
                realm = Realm.GetInstance(config);
            }

            return realm;
        }

    }
}
