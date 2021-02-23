using DisplayAdmin.Model;
using DisplayAdmin.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayAdmin.Query
{
    public class ExcuteQuery
    {
        private static Realms.Realm realm;

        private static ExcuteQuery instance;

        public static ExcuteQuery GetInstance()
        {
            if (instance == null)
            {
                instance = new ExcuteQuery();
                if (realm == null)
                {
                    Common.RealmConnect realmConnect = new Common.RealmConnect();
                    realm = realmConnect.GetRealm();
                }
            }

            return instance;
        }

        #region SELECT

        /// <summary>
        /// 직업 정보 데이터 조회
        /// </summary>
        /// <param name="sSearchText"></param>
        /// <returns></returns>
        public List<OCTSJOB> SelectJobData()
        {
            List<OCTSJOB> arrReuslt = new List<OCTSJOB>();
            try
            {
                realm.Write(() =>
                {
                    var tOCTSJOBs = from d in realm.All<OCTSJOB>() orderby d.INDEX ascending select d;
                    arrReuslt = tOCTSJOBs.ToList();
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return arrReuslt;
        }

        /// <summary>
        /// 예약일로 직업정보 조회
        /// </summary>
        /// <param name="dtoReservation"></param>
        /// <returns></returns>
        public List<OCTSJOB> SelectDateJobData(string dtoReservation)
        {
            List<OCTSJOB> arrUdtJob = new List<OCTSJOB>();
            try
            {
                var jobInfo = realm.All<OTADHSTRY>().Where(d => d.TB_NM == Common.Constants.TABLE_NAME.JOB);

                // 날짜 없을때
                if (dtoReservation.Equals(string.Empty))
                {
                    foreach (var index in jobInfo)
                    {
                        var jobResult = realm.All<OCTSJOB>().Where(d => d.INDEX == index.INDEX).SingleOrDefault();
                        arrUdtJob.Add(jobResult);
                    }
                }
                else
                {
                    foreach (var index in jobInfo)
                    {
                        if (index.VIEW_DT.Equals(dtoReservation))
                        {
                            var jobResult = realm.All<OCTSJOB>().Where(d => d.INDEX == index.INDEX).SingleOrDefault();
                            //var bookResult = (from d in realm.All<OTCSBOOK>() where d.INDEX == index select d).SingleOrDefault();

                            arrUdtJob.Add(jobResult);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return arrUdtJob;
        }

        /// <summary>
        /// 안전 정보 조회
        /// </summary>
        /// <returns></returns>
        public List<OTCSSAFE> SelectSaftyData()
        {
            List<OTCSSAFE> arrResult = new List<OTCSSAFE>();

            try
            {
                var tOTCSSAFEs = from d in realm.All<OTCSSAFE>() orderby d.INDEX ascending select d;

                foreach (OTCSSAFE item in tOTCSSAFEs)
                {
                    arrResult.Add(item);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return arrResult;
        }

        /// <summary>
        /// 예약일로 안전정보 조회
        /// </summary>
        /// <param name="dtoReservation"></param>
        /// <returns></returns>
        public List<OTCSSAFE> SelectDateSaftyData(string dtoReservation)
        {
            List<OTCSSAFE> arrUdtSafty = new List<OTCSSAFE>();
            try
            {
                var saftyInfo = realm.All<OTADHSTRY>().Where(d => d.TB_NM == Common.Constants.TABLE_NAME.SAFETY);

                // 날짜 없을때
                if (dtoReservation.Equals(string.Empty))
                {
                    foreach (var index in saftyInfo)
                    {
                        var saftyResult = realm.All<OTCSSAFE>().Where(d => d.INDEX == index.INDEX).SingleOrDefault();
                        arrUdtSafty.Add(saftyResult);
                    }
                }
                else
                {
                    foreach (var index in saftyInfo)
                    {
                        if (index.VIEW_DT.Equals(dtoReservation))
                        {
                            var saftyResult = realm.All<OTCSSAFE>().Where(d => d.INDEX == index.INDEX).SingleOrDefault();
                            //var bookResult = (from d in realm.All<OTCSBOOK>() where d.INDEX == index select d).SingleOrDefault();

                            arrUdtSafty.Add(saftyResult);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return arrUdtSafty;
        }

        /// <summary>
        /// 책 정보 전체 조회
        /// </summary>
        /// <returns></returns>
        public List<OTCSBOOK> SelectBookData()
        {
            List<OTCSBOOK> arrResult = new List<OTCSBOOK>();

            try
            {
                var tOTCSBOOKs = from d in realm.All<OTCSBOOK>() orderby d.INDEX ascending select d;
                arrResult = tOTCSBOOKs.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return arrResult;
        }

        /// <summary>
        /// 책 이름으로 조회
        /// </summary>
        /// <returns></returns>
        public int SelectBookData(string sBookNm)
        {
            OTCSBOOK nResult;
            try
            {
                nResult = (from d in realm.All<OTCSBOOK>() where d.BOOK_NM == sBookNm orderby d.INDEX ascending select d).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return nResult.INDEX;
        }

        /// <summary>
        /// 예약일로 책 정보 조회
        /// </summary>
        /// <returns></returns>
        public List<OTCSBOOK> SelectDateBookData(string dtoReservation)
        {
            List<OTCSBOOK> arrUdtBook = new List<OTCSBOOK>();
            try
            {
                var bookInfo = realm.All<OTADHSTRY>().Where(d => d.TB_NM == Common.Constants.TABLE_NAME.BOOK);
                //var bookInfo = from d in realm.All<OTADHSTRY>() where d.TB_NM == Common.Constants.TABLE_NAME.BOOK && d.VIEW_DT == dtoReservation select d.INDEX;

                // 날짜 없을때
                if (dtoReservation.Equals(string.Empty))
                {
                    foreach (var index in bookInfo)
                    {
                        var bookResult = realm.All<OTCSBOOK>().Where(d => d.INDEX == index.INDEX).SingleOrDefault();
                        arrUdtBook.Add(bookResult);
                    }
                }
                else
                {
                    //foreach (var index in bookInfo.ToArray())
                    foreach (var index in bookInfo)
                    {
                        if (index.VIEW_DT.Equals(dtoReservation))
                        {
                            var bookResult = realm.All<OTCSBOOK>().Where(d => d.INDEX == index.INDEX).SingleOrDefault();
                            //var bookResult = (from d in realm.All<OTCSBOOK>() where d.INDEX == index select d).SingleOrDefault();

                            arrUdtBook.Add(bookResult);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return arrUdtBook;
        }

        /// <summary>
        /// 영어회화 정보 조회
        /// </summary>
        /// <returns></returns>
        public List<OTCSCONV> SelectConversationDatas()
        {
            List<OTCSCONV> arrResult = new List<OTCSCONV>();

            try
            {
                var tOTCSCONVs = from d in realm.All<OTCSCONV>() orderby d.INDEX ascending select d;
                arrResult = tOTCSCONVs.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return arrResult;
        }

        /// <summary>
        /// 예약일로 회화정보 조회
        /// </summary>
        /// <param name="dtoReservation"></param>
        /// <returns></returns>
        public List<OTCSCONV> SelectDateConvData(string dtoReservation)
        {
            List<OTCSCONV> arrUdtConv = new List<OTCSCONV>();
            try
            {
                var convInfo = realm.All<OTADHSTRY>().Where(d => d.TB_NM == Common.Constants.TABLE_NAME.CONVERSATION);
                //var bookInfo = from d in realm.All<OTADHSTRY>() where d.TB_NM == Common.Constants.TABLE_NAME.BOOK && d.VIEW_DT == dtoReservation select d.INDEX;
                // 날짜 없을때
                if (dtoReservation.Equals(string.Empty))
                {
                    foreach (var index in convInfo)
                    {
                        var convResult = realm.All<OTCSCONV>().Where(d => d.INDEX == index.INDEX).SingleOrDefault();
                        arrUdtConv.Add(convResult);
                    }
                }
                else
                {
                    //foreach (var index in bookInfo.ToArray())
                    foreach (var index in convInfo)
                    {
                        if (index.VIEW_DT.Equals(dtoReservation))
                        {
                            var convResult = realm.All<OTCSCONV>().Where(d => d.INDEX == index.INDEX).SingleOrDefault();
                            //var bookResult = (from d in realm.All<OTCSBOOK>() where d.INDEX == index select d).SingleOrDefault();

                            arrUdtConv.Add(convResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return arrUdtConv;
        }

        /// <summary>
        /// 가장 최근에 입력한 환영 메시지 조회
        /// </summary>
        /// <returns></returns>
        public OTCSWELC SelectWelcomeData()
        {
            OTCSWELC Result = new OTCSWELC();

            try
            {
                Result = (from a in realm.All<OTCSWELC>() orderby a.WELC_DT descending select a).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Result;
        }

        /// <summary>
        /// 가장 최근에 입력한 스승의날 메시지 조회
        /// </summary>
        /// <returns></returns>
        public OTCSTCDY SelectTeachersDayData()
        {
            OTCSTCDY Result = new OTCSTCDY();

            try
            {
                Result = (from a in realm.All<OTCSTCDY>() orderby a.CELE_DT descending select a).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Result;
        }

        /// <summary>
        /// 입학일 체크
        /// </summary>
        /// <param name="dtoReservation"></param>
        /// <returns></returns>
        public bool SelectDateAdmissionData(string dtoReservation)
        {
            bool bResult = false;
            List<int> arrAdmission = new List<int>();
            try
            {
                var admissionInfo = realm.All<OTADHSTRY>();//.Where(d => d.TB_NM.Equals(Common.Constants.TABLE_NAME.ADMISSION));
                foreach (var index in admissionInfo)
                {
                    if (Common.Constants.TABLE_NAME.ADMISSION.Equals(index.TB_NM))
                    {
                        if (index.VIEW_DT.Equals(dtoReservation))
                        {
                            var arrResult = index.INDEX;

                            arrAdmission.Add(arrResult);
                        }
                    }
                }
                if (arrAdmission.Count != 0)
                {
                    bResult = true;
                }
                else
                {
                    bResult = false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bResult;
        }

        /// <summary>
        /// 졸업일 체크
        /// </summary>
        /// <param name="dtoReservation"></param>
        /// <returns></returns>
        public bool SelectDateGraduationData(string dtoReservation)
        {
            bool bResult = false;
            List<int> arrAdmission = new List<int>();
            try
            {
                var admissionInfo = realm.All<OTADHSTRY>();//.Where(d => d.TB_NM == Common.Constants.TABLE_NAME.GRADUATION);
                foreach (var index in admissionInfo)
                {
                    if (Common.Constants.TABLE_NAME.GRADUATION.Equals(index.TB_NM))
                    {
                        if (index.VIEW_DT.Equals(dtoReservation))
                        {
                            var arrResult = index.INDEX;

                            arrAdmission.Add(arrResult);
                        }
                    }
                }
                if (arrAdmission.Count != 0)
                {
                    bResult = true;
                }
                else
                {
                    bResult = false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bResult;
        }

        /// <summary>
        /// 오늘 생일인 학생정보 조회
        /// </summary>
        /// <returns></returns>
        public List<OTCSSTUD> SelectStudentData(string sToday)
        {
            List<OTCSSTUD> arrResult = new List<OTCSSTUD>();

            try
            {
                //var tOTCSSTUD = from d in realm.All<OTCSSTUD>() where d.STD_BIRTHDAY.ToString("MMdd").Contains(sToday) orderby d.INDEX ascending select d;
                var tOTCSSTUD = realm.All<OTCSSTUD>().OrderBy(d => d.STD_NM);
                foreach (var item in tOTCSSTUD)
                {
                    if (item.STD_BIRTHDAY.Equals(sToday))
                    {
                        arrResult.Add(item);
                    }
                }

                //arrResult = tOTCSSTUD.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return arrResult;
        }

        /// <summary>
        /// 전체설정 데이터 조회
        /// </summary>
        /// <returns></returns>
        public OTADCONF SelectConfigData()
        {
            OTADCONF Result = new OTADCONF();

            try
            {
                var tOTADCONF = (from a in realm.All<OTADCONF>() orderby a.INDEX descending select a).FirstOrDefault();
                Result = tOTADCONF;

                //arrResult = tOTCSSTUD.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Result;
        }

        /// <summary>
        /// 날짜별 식단 조회
        /// </summary>
        /// <param name="dtoReservation"></param>
        /// <returns></returns>
        public List<OTCSMENU> SelectDateFoodMenu(string dtFoodMenuDate)
        {
            List<OTCSMENU> arrUdtFoodMenu = new List<OTCSMENU>();
            try
            {
                var tempUdtFoodMenu = realm.All<OTCSMENU>().Where(d => d.MENU_DT.Equals(dtFoodMenuDate));

                foreach (var item in tempUdtFoodMenu)
                {
                    arrUdtFoodMenu.Add(item);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return arrUdtFoodMenu;
        }

        /// <summary>
        /// 예약일로 화면순서 조회
        /// </summary>
        /// <param name="dtoReservation"></param>
        /// <returns></returns>
        public OTADSCFL SelectScreenFlowData(string dtoReservation)
        {
            OTADSCFL result = new OTADSCFL();
            try
            {
                var todayData = realm.All<OTADSCFL>().Where(b => b.SCRN_DT == dtoReservation && b.DEFAULT_YN == "N").FirstOrDefault();

                if (todayData == null)
                {
                    result = realm.All<OTADSCFL>().Where(b => b.DEFAULT_YN == "Y").FirstOrDefault();
                }
                else
                {
                    result = todayData;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 동영상 조회
        /// </summary>
        /// <returns></returns>
        public List<OTCSPRMO> SelectMovieData()
        {
            List<OTCSPRMO> arrResult = new List<OTCSPRMO>();

            try
            {
                var tOTCSPRMOs = from d in realm.All<OTCSPRMO>() orderby d.INDEX ascending select d;
                arrResult = tOTCSPRMOs.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return arrResult;
        }

        /// <summary>
        /// 예약일로 동영상 조회
        /// </summary>
        /// <param name="dtoReservation"></param>
        /// <returns></returns>
        public List<OTCSPRMO> SelectDateMovieData(string dtoReservation)
        {
            List<OTCSPRMO> arrUdtPromotionMovie = new List<OTCSPRMO>();
            try
            {
                var MovieInfo = realm.All<OTADHSTRY>().Where(d => d.TB_NM == Common.Constants.TABLE_NAME.PROMOTION_MOVIE);

                // 날짜 없을때
                if (dtoReservation.Equals(string.Empty))
                {
                    foreach (var index in MovieInfo)
                    {
                        var MovieResult = realm.All<OTCSPRMO>().Where(d => d.INDEX == index.INDEX).SingleOrDefault();
                        arrUdtPromotionMovie.Add(MovieResult);
                    }
                }
                else
                {

                    foreach (var index in MovieInfo)
                    {
                        if (index.VIEW_DT.Equals(dtoReservation))
                        {
                            var MovieResult = realm.All<OTCSPRMO>().Where(d => d.INDEX == index.INDEX).SingleOrDefault();
                            //var bookResult = (from d in realm.All<OTCSBOOK>() where d.INDEX == index select d).SingleOrDefault();

                            arrUdtPromotionMovie.Add(MovieResult);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return arrUdtPromotionMovie;
        }

        #endregion

        #region INSERT
        /// <summary>
        /// 추천도서 데이터 저장
        /// </summary>
        /// <param name="arrBookData"></param>
        /// <returns></returns>
        public bool InsertBookData(UdtBook udtBookData)
        {
            bool isSuccess = false;
            try
            {
                var vIndex = (from a in realm.All<OTCSBOOK>() orderby a.INDEX descending select a).FirstOrDefault();
                int nIndex = 0;
                if (null != vIndex)
                {
                    nIndex = vIndex.INDEX + 1;
                }

                OTCSBOOK nResult = (from d in realm.All<OTCSBOOK>() where d.BOOK_NM == udtBookData.TITLE orderby d.INDEX ascending select d).FirstOrDefault();

                //같은 책 제목이 없으면 Insert
                if (nResult == null)
                {
                    using (var transaction = realm.BeginWrite())
                    {
                        OTCSBOOK tOTCSBOOK = new OTCSBOOK();
                        tOTCSBOOK.BOOK_AUTH = udtBookData.AUTH;
                        tOTCSBOOK.BOOK_DESC = udtBookData.DESC;
                        tOTCSBOOK.BOOK_NM = udtBookData.TITLE;
                        tOTCSBOOK.ENT_STF = Properties.Settings.Default.ENT_USER;
                        tOTCSBOOK.BOOK_DT = System.DateTimeOffset.Parse("9999-12-31");
                        tOTCSBOOK.IMG_PATH = udtBookData.IMAGE;
                        tOTCSBOOK.INDEX = nIndex;
                        realm.Add(tOTCSBOOK);
                        transaction.Commit();
                    }
                }
                else //Update
                {
                    using (var transaction = realm.BeginWrite())
                    {
                        nResult.BOOK_DESC = udtBookData.DESC;
                        nResult.BOOK_AUTH = udtBookData.AUTH;
                        nResult.BOOK_DT = udtBookData.DATE;
                        nResult.IMG_PATH = udtBookData.IMAGE;
                        transaction.Commit();
                    }
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //   realm.Dispose();
            return isSuccess;
        }

        /// <summary>
        /// 직업 정보 데이터 저장 (전체) 
        /// </summary>
        /// <param name="arrJobData"></param>
        /// <returns></returns>
        public bool InsertJobData(List<IMRUtils.Model.UdtJob> arrJobData)
        {
            bool isSuccess = false;
            try
            {
                var tOCTSJOBs = realm.All<OCTSJOB>();
                foreach (var item in arrJobData)
                {
                    int nIndex = realm.All<OCTSJOB>().Count();
                    using (var transaction = realm.BeginWrite())
                    {
                        OCTSJOB uOCTSJOB = new OCTSJOB();
                        uOCTSJOB.ENT_STF = Properties.Settings.Default.ENT_USER;
                        uOCTSJOB.JOB_DESC = item.JobDesc;
                        uOCTSJOB.JOB_NM = item.JobName;
                        uOCTSJOB.JOB_DT = System.DateTimeOffset.Now;
                        uOCTSJOB.IMG_PATH = Properties.Settings.Default.ROOT_FILE_PATH + Properties.Settings.Default.IMAGE_FILE_PATH + "job_imagesource_imagepreparing.png";
                        uOCTSJOB.INDEX = nIndex;
                        realm.Add(uOCTSJOB);
                        transaction.Commit();
                    }
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSuccess;
        }

        /// <summary>
        /// 직업 정보 데이터 저장 (개별)
        /// </summary>
        /// <param name="udtJobData"></param>
        /// <returns></returns>
        public bool InsertJobData(UdtJob udtJobData)
        {
            bool isSuccess = false;
            try
            {
                var vIndex = (from a in realm.All<OCTSJOB>() orderby a.INDEX descending select a).FirstOrDefault();
                int nIndex = 0;
                if (null != vIndex)
                {
                    nIndex = vIndex.INDEX + 1;
                }
                OCTSJOB nResult = (from d in realm.All<OCTSJOB>() where d.JOB_NM == udtJobData.TITLE orderby d.INDEX ascending select d).FirstOrDefault();

                //같은 책 제목이 없으면 Insert
                if (nResult == null)
                {
                    using (var transaction = realm.BeginWrite())
                    {
                        OCTSJOB tOCTSJOB = new OCTSJOB();
                        tOCTSJOB.ENT_STF = Properties.Settings.Default.ENT_USER;
                        tOCTSJOB.IMG_PATH = udtJobData.IMAGE;
                        tOCTSJOB.INDEX = nIndex;
                        tOCTSJOB.JOB_DESC = udtJobData.INFO;
                        tOCTSJOB.JOB_DT = System.DateTimeOffset.Now;
                        tOCTSJOB.JOB_NM = udtJobData.TITLE;
                        realm.Add(tOCTSJOB);
                        transaction.Commit();
                    }
                }
                else
                {
                    using (var transaction = realm.BeginWrite())
                    {
                        nResult.JOB_DESC = udtJobData.INFO;
                        nResult.IMG_PATH = udtJobData.IMAGE;
                        nResult.JOB_DT = System.DateTimeOffset.Now;
                        transaction.Commit();
                    }
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSuccess;
        }

        /// <summary>
        /// 안전 정보 저장
        /// </summary>
        /// <param name="arrSaftyData"></param>
        /// <returns></returns>
        public bool InsertSaftyData(List<object> arrSaftyData)
        {
            bool isSuccess = false;
            try
            {
                foreach (var item in arrSaftyData)
                {
                    var vIndex = (from a in realm.All<OTCSSAFE>() orderby a.INDEX descending select a).FirstOrDefault();
                    int nIndex = 0;
                    if (null != vIndex)
                    {
                        nIndex = vIndex.INDEX + 1;
                    }
                    using (var transaction = realm.BeginWrite())
                    {
                        OTCSSAFE uOTCSSAFE = new OTCSSAFE();
                        uOTCSSAFE.ENT_STF = Properties.Settings.Default.ENT_USER;
                        uOTCSSAFE.SAFE_DESC = ((UdtSaftyInfo)item).DESC;
                        uOTCSSAFE.SAFE_TITLE = ((UdtSaftyInfo)item).TITLE;
                        uOTCSSAFE.SAFE_DT = System.DateTimeOffset.Now;
                        uOTCSSAFE.INDEX = nIndex;
                        realm.Add(uOTCSSAFE);
                        transaction.Commit();
                    }

                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSuccess;
        }

        /// <summary>
        /// 안전 정보 데이터 저장 (개별)
        /// </summary>
        /// <param name="udtSaftyData"></param>
        /// <returns></returns>
        public bool InsertSaftyData(UdtSaftyInfo udtSaftyData)
        {
            bool isSuccess = false;
            try
            {
                var vIndex = (from a in realm.All<OTCSSAFE>() orderby a.INDEX descending select a).FirstOrDefault();
                int nIndex = 0;
                if (null != vIndex)
                {
                    nIndex = vIndex.INDEX + 1;
                }
                OTCSSAFE nResult = (from d in realm.All<OTCSSAFE>() where d.SAFE_TITLE == udtSaftyData.TITLE orderby d.INDEX ascending select d).FirstOrDefault();

                //같은 책 제목이 없으면 Insert
                if (nResult == null)
                {
                    using (var transaction = realm.BeginWrite())
                    {
                        OTCSSAFE tOTCSSAFE = new OTCSSAFE();
                        tOTCSSAFE.SAFE_TITLE = udtSaftyData.TITLE;
                        tOTCSSAFE.SAFE_DT = System.DateTimeOffset.Now;
                        tOTCSSAFE.SAFE_DESC = udtSaftyData.DESC;
                        tOTCSSAFE.ENT_STF = Properties.Settings.Default.ENT_USER;
                        tOTCSSAFE.INDEX = nIndex;
                        realm.Add(tOTCSSAFE);
                        transaction.Commit();
                    }
                }
                else
                {
                    using (var transaction = realm.BeginWrite())
                    {
                        nResult.SAFE_DESC = udtSaftyData.DESC;
                        nResult.SAFE_DT = System.DateTimeOffset.Now;
                        transaction.Commit();
                    }
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSuccess;
        }

        /// <summary>
        /// 추천도서 데이터 저장
        /// </summary>
        /// <param name="arrBookData"></param>
        /// <returns></returns>
        public bool InsertReservationData(string sTbNm, List<int> nItemIndex, string dtViewDt)
        {
            bool isSuccess = false;

            if (sTbNm.Equals(Common.Constants.TABLE_NAME.GRADUATION))
            {
                if (SelectDateGraduationData(DateTime.Now.ToString("yyyyMMdd")))
                {
                    return false;
                }
            }
            else if (sTbNm.Equals(Common.Constants.TABLE_NAME.ADMISSION))
            {

                if (SelectDateAdmissionData(DateTime.Now.ToString("yyyyMMdd")))
                {
                    return false;
                }
            }

            try
            {
                foreach (int nIndex in nItemIndex)
                {
                    using (var transaction = realm.BeginWrite())
                    {
                        OTADHSTRY tOTADHSTRY = new OTADHSTRY();
                        tOTADHSTRY.TB_NM = sTbNm;
                        tOTADHSTRY.VIEW_DT = dtViewDt;
                        tOTADHSTRY.ENT_STF = Properties.Settings.Default.ENT_USER;
                        tOTADHSTRY.INDEX = nIndex;
                        realm.Add(tOTADHSTRY);
                        transaction.Commit();
                    }
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //   realm.Dispose();
            return isSuccess;
        }

        /// <summary>
        /// 영어회화 데이터 저장
        /// </summary>
        /// <param name="udtBookData"></param>
        /// <returns></returns>
        public bool InsertConvData(UdtConversation udtConvData)
        {
            bool isSuccess = false;
            try
            {
                var vIndex = (from a in realm.All<OTCSCONV>() orderby a.INDEX descending select a).FirstOrDefault();
                int nIndex = 0;
                if (null != vIndex)
                {
                    nIndex = vIndex.INDEX + 1;
                }

                OTCSCONV nResult = (from d in realm.All<OTCSCONV>() where d.CONV_TITLE == udtConvData.TITLE orderby d.INDEX ascending select d).FirstOrDefault();

                //같은 책 제목이 없으면 Insert
                if (nResult == null)
                {
                    using (var transaction = realm.BeginWrite())
                    {
                        OTCSCONV tOTCSCONV = new OTCSCONV();
                        tOTCSCONV.CONV_TITLE = udtConvData.TITLE;
                        tOTCSCONV.CONV_DT = System.DateTimeOffset.Now;
                        tOTCSCONV.CONV_TRANS = udtConvData.TRANSLATE;
                        tOTCSCONV.ENT_STF = Properties.Settings.Default.ENT_USER;
                        tOTCSCONV.INDEX = nIndex;
                        realm.Add(tOTCSCONV);
                        transaction.Commit();
                    }
                }
                else
                {
                    using (var transaction = realm.BeginWrite())
                    {
                        nResult.CONV_TRANS = udtConvData.TRANSLATE;
                        transaction.Commit();
                    }
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSuccess;
        }

        /// <summary>
        /// 환영 메시지 저장
        /// </summary>
        /// <param name="udtWelcomeMessage"></param>
        /// <returns></returns>
        public bool InsertWelcomeData(string sMessage)
        {
            bool isSuccess = false;
            try
            {
                int nIndex = realm.All<OTCSWELC>().Count();
                using (var transaction = realm.BeginWrite())
                {
                    OTCSWELC tOTCSWELC = new OTCSWELC();
                    tOTCSWELC.WELC_MSG = sMessage;
                    tOTCSWELC.WELC_DT = System.DateTimeOffset.Now;
                    tOTCSWELC.INDEX = nIndex;
                    realm.Add(tOTCSWELC);
                    transaction.Commit();
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSuccess;
        }

        /// <summary>
        /// 스승의날 축하메시지 저장
        /// </summary>
        /// <param name="sMessage"></param>
        /// <returns></returns>
        public bool InsertTeachersDayData(string sMessage)
        {
            bool isSuccess = false;
            try
            {
                int nIndex = realm.All<OTCSTCDY>().Count();
                using (var transaction = realm.BeginWrite())
                {
                    OTCSTCDY tOTCSTCDY = new OTCSTCDY();
                    tOTCSTCDY.CELE_MSG = sMessage;
                    tOTCSTCDY.CELE_DT = System.DateTimeOffset.Now;
                    tOTCSTCDY.INDEX = nIndex;
                    realm.Add(tOTCSTCDY);
                    transaction.Commit();
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSuccess;
        }

        /// <summary>
        /// 식단표 저장
        /// </summary>
        /// <param name="arrUdtFoodMenu"></param>
        /// <returns></returns>
        public bool InsertFoodData(List<UdtFoodMenu> arrUdtFoodMenu)
        {
            bool isSuccess = false;
            try
            {
                // 기존 데이터 제거
                using (var transaction = realm.BeginWrite())
                {
                    realm.RemoveAll<OTCSMENU>();
                    transaction.Commit();
                }

                foreach (UdtFoodMenu item in arrUdtFoodMenu)
                {
                    using (var transaction = realm.BeginWrite())
                    {
                        OTCSMENU tOTCSMENU = new OTCSMENU();
                        tOTCSMENU.MENU_DAY_DESC = item.DAY_DESC;
                        tOTCSMENU.MENU_NIGHT_DESC = item.NIGHT_DESC;
                        tOTCSMENU.MENU_DT = item.MENU_DATE;
                        realm.Add(tOTCSMENU);
                        transaction.Commit();
                    }
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSuccess;
        }

        /// <summary>
        /// 생일 축하 학생정보 저장
        /// </summary>
        /// <param name="arrUdtFoodMenu"></param>
        /// <returns></returns>
        public bool InsertCelebrationData(List<UdtCelebration> arrUdtCelebration)
        {
            bool isSuccess = false;
            try
            {
                // 기존 데이터 제거
                using (var transaction = realm.BeginWrite())
                {
                    realm.RemoveAll<OTCSSTUD>();
                    transaction.Commit();
                }

                foreach (UdtCelebration item in arrUdtCelebration)
                {
                    using (var transaction = realm.BeginWrite())
                    {
                        OTCSSTUD tOTCSSTUD = new OTCSSTUD();
                        tOTCSSTUD.STD_BIRTHDAY = item.celebrationDate;
                        tOTCSSTUD.STD_CLASS = item.CLASS;
                        tOTCSSTUD.STD_GRADE = item.GRADE;
                        tOTCSSTUD.STD_NM = item.NAME;
                        realm.Add(tOTCSSTUD);
                        transaction.Commit();
                    }
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSuccess;
        }

        /// <summary>
        /// 설정 데이터 저장하기
        /// </summary>
        /// <param name="sSchoolNM"></param>
        /// <param name="nInterval"></param>
        /// <returns></returns>
        public bool InsertConfigData(string sSchoolNM, int nInterval)
        {
            bool isSuccess = false;
            try
            {
                int nIndex = realm.All<OTADCONF>().Count();
                using (var transaction = realm.BeginWrite())
                {
                    OTADCONF tOTADCONF = new OTADCONF();
                    tOTADCONF.INDEX = nIndex;
                    tOTADCONF.SCH_NM = sSchoolNM;
                    tOTADCONF.SCR_INTERVAL = nInterval;
                    realm.Add(tOTADCONF);
                    transaction.Commit();
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSuccess;
        }

        /// <summary>
        /// 화면 순서 저장하기
        /// </summary>
        /// <param name="arrReserv"></param>
        /// <param name="dtViewDt"></param>
        /// <returns></returns>
        public bool InsertScreenSeq(List<UdtReservList> arrReserv, string dtViewDt)
        {
            bool isSuccess = false;
            try
            {
                OTADSCFL data = realm.All<OTADSCFL>().Where(b => b.SCRN_DT == dtViewDt).SingleOrDefault();

                if (null == data)
                {
                    using (var transaction = realm.BeginWrite())
                    {
                        OTADSCFL tOTADSCFL = new OTADSCFL();
                        tOTADSCFL.SCRN_DT = dtViewDt;
                        tOTADSCFL.PROM_SEQ = "";
                        tOTADSCFL.WETH_SEQ = "";
                        tOTADSCFL.CONV_SEQ = "";
                        tOTADSCFL.BOOK_SEQ = "";
                        tOTADSCFL.JOB_SEQ = "";
                        tOTADSCFL.CELE_SEQ = "";
                        tOTADSCFL.SAFE_SEQ = "";
                        tOTADSCFL.MENU_SEQ = "";
                        tOTADSCFL.TCDY_SEQ = "";
                        tOTADSCFL.WELC_SEQ = "";
                        tOTADSCFL.DEFAULT_YN = "N";

                        for (int i = 0; i < arrReserv.Count(); i++)
                        {
                            switch (arrReserv[i].INDEX)
                            {
                                case (int)Common.Constants.ScreenIndex.PROMOTION_MOVIE:
                                {
                                    tOTADSCFL.PROM_SEQ = i.ToString();
                                    break;
                                }
                                case (int)Common.Constants.ScreenIndex.WEATHER:
                                {
                                    tOTADSCFL.WETH_SEQ = i.ToString();
                                    break;
                                }
                                case (int)Common.Constants.ScreenIndex.CONVERSATION:
                                {
                                    tOTADSCFL.CONV_SEQ = i.ToString();
                                    break;
                                }
                                case (int)Common.Constants.ScreenIndex.BOOK:
                                {
                                    tOTADSCFL.BOOK_SEQ = i.ToString();
                                    break;
                                }
                                case (int)Common.Constants.ScreenIndex.JOB:
                                {
                                    tOTADSCFL.JOB_SEQ = i.ToString();
                                    break;
                                }
                                case (int)Common.Constants.ScreenIndex.CELEBRATION:
                                {
                                    tOTADSCFL.CELE_SEQ = i.ToString();
                                    break;
                                }
                                case (int)Common.Constants.ScreenIndex.SAFTY_INFO:
                                {
                                    tOTADSCFL.SAFE_SEQ = i.ToString();
                                    break;
                                }
                                case (int)Common.Constants.ScreenIndex.FOOD_MENU:
                                {
                                    tOTADSCFL.MENU_SEQ = i.ToString();
                                    break;
                                }
                                case (int)Common.Constants.ScreenIndex.TEACHERS_DAY:
                                {
                                    tOTADSCFL.TCDY_SEQ = i.ToString();
                                    break;
                                }
                                case (int)Common.Constants.ScreenIndex.WELCOME_MESSAGE:
                                {
                                    tOTADSCFL.WELC_SEQ = i.ToString();
                                    break;
                                }
                                default:
                                {
                                    tOTADSCFL.PROM_SEQ = i.ToString();
                                    break;
                                }
                            }
                        }

                        realm.Add(tOTADSCFL);
                        transaction.Commit();
                    }
                }
                else
                {
                    using (var transaction = realm.BeginWrite())
                    {
                        data.SCRN_DT = dtViewDt;
                        data.PROM_SEQ = "";
                        data.WETH_SEQ = "";
                        data.CONV_SEQ = "";
                        data.BOOK_SEQ = "";
                        data.JOB_SEQ = "";
                        data.CELE_SEQ = "";
                        data.SAFE_SEQ = "";
                        data.MENU_SEQ = "";
                        data.TCDY_SEQ = "";
                        data.WELC_SEQ = "";
                        data.DEFAULT_YN = "N";

                        for (int i = 0; i < arrReserv.Count(); i++)
                        {
                            switch (arrReserv[i].INDEX)
                            {
                                case (int)Common.Constants.ScreenIndex.PROMOTION_MOVIE:
                                {
                                    data.PROM_SEQ = i.ToString();
                                    break;
                                }
                                case (int)Common.Constants.ScreenIndex.WEATHER:
                                {
                                    data.WETH_SEQ = i.ToString();
                                    break;
                                }
                                case (int)Common.Constants.ScreenIndex.CONVERSATION:
                                {
                                    data.CONV_SEQ = i.ToString();
                                    break;
                                }
                                case (int)Common.Constants.ScreenIndex.BOOK:
                                {
                                    data.BOOK_SEQ = i.ToString();
                                    break;
                                }
                                case (int)Common.Constants.ScreenIndex.JOB:
                                {
                                    data.JOB_SEQ = i.ToString();
                                    break;
                                }
                                case (int)Common.Constants.ScreenIndex.CELEBRATION:
                                {
                                    data.CELE_SEQ = i.ToString();
                                    break;
                                }
                                case (int)Common.Constants.ScreenIndex.SAFTY_INFO:
                                {
                                    data.SAFE_SEQ = i.ToString();
                                    break;
                                }
                                case (int)Common.Constants.ScreenIndex.FOOD_MENU:
                                {
                                    data.MENU_SEQ = i.ToString();
                                    break;
                                }
                                case (int)Common.Constants.ScreenIndex.TEACHERS_DAY:
                                {
                                    data.TCDY_SEQ = i.ToString();
                                    break;
                                }
                                case (int)Common.Constants.ScreenIndex.WELCOME_MESSAGE:
                                {
                                    data.WELC_SEQ = i.ToString();
                                    break;
                                }
                                default:
                                {
                                    data.PROM_SEQ = i.ToString();
                                    break;
                                }
                            }

                        }
                        transaction.Commit();
                    }
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSuccess;
        }

        #endregion

        #region UPDATE
        #endregion

        /// <summary>
        /// 학생정보 저장(엑셀 로직 작성 후 구현)
        /// </summary>
        /// <param name="arrBookData"></param>
        /// <returns></returns>
        public bool InsertStudentData(List<object> arrBookData)
        {
            bool isSuccess = false;
            try
            {

                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSuccess;
        }

        /// <summary>
        /// 홍보영상 데이터 저장
        /// </summary>
        /// <param name="arrMovieData"></param>
        /// <returns></returns>
        public bool InsertPromotionMovieData(List<UdtPromotionMovie> arrMovieData)
        {
            bool isSuccess = false;

            try
            {
                //realm.Write(() =>
                //{
                //    // 기존 데이터 제거
                //    realm.RemoveAll<OTCSPRMO>();
                //});

                var vIndex = (from a in realm.All<OTCSPRMO>() orderby a.INDEX descending select a).FirstOrDefault();
                int nIndex = 0;
                if (null != vIndex)
                {
                    nIndex = vIndex.INDEX + 1;
                }
                var tOTCSPRMOs = realm.All<OTCSPRMO>();


                for(int i=0; i<arrMovieData.Count; i++)
                {
                    using (var transaction = realm.BeginWrite())
                    {

                        OTCSPRMO uOTCSPRMO = new OTCSPRMO();
                        uOTCSPRMO.MO_PATH = arrMovieData[i].FILEPATH;
                        uOTCSPRMO.MO_NM = arrMovieData[i].movieName;
                        uOTCSPRMO.MO_DT = System.DateTimeOffset.Now;
                        uOTCSPRMO.INDEX = nIndex + i;
                        realm.Add(uOTCSPRMO);
                        transaction.Commit();
                    }
                }


                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSuccess;
        }

        /// <summary>
        /// 환영 메세지 데이터 저장
        /// </summary>
        /// <param name="arrMovieData"></param>
        /// <returns></returns>
        public bool InsertWelcomeData(List<object> arrWelcomeData)
        {
            bool isSuccess = false;

            try
            {
                realm.Write(() =>
                {
                    int nContainIndex = -1;




                    var tOTCSWELCs = realm.All<OTCSWELC>();

                    //foreach (var containItem in arrWelcomeData)
                    //{
                    //    var checkItemIndex = (from b in realm.All<OTCSWELC>() where b.WELC_DT.Equals(System.DateTimeOffset.Parse(((UdtWelcomeMessage)containItem).welcomeDate)) select b.INDEX).SingleOrDefault();

                    //    // 날짜가 세개?
                    //    // 데이터가 세개?
                    //    if (default != checkItemIndex)
                    //    {
                    //        nContainIndex = (int)checkItemIndex;

                    //        var tOTCSWELC = from c in realm.All<OTCSWELC>() where c.INDEX == nContainIndex select c;

                    //        foreach (var tOTCSWELC in tOTCSWELCs)
                    //        {
                    //            using (var transaction = realm.BeginWrite())
                    //            {
                    //                foreach (var item in arrWelcomeData)
                    //                {
                    //                    OTCSWELC uOTCSWELC = new OTCSWELC();
                    //                    uOTCSWELC.WELC_MSG = ((UdtWelcomeMessage)item).DESC;
                    //                    uOTCSWELC.WELC_DT = System.DateTimeOffset.Parse(((UdtWelcomeMessage)item).welcomeDate);
                    //                    uOTCSWELC.INDEX = (from d in realm.All<OTCSWELC>() select d.INDEX).Count() - 1;
                    //                    realm.Add(uOTCSWELC);
                    //                    transaction.Commit();
                    //                }
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        foreach (var tOTCSWELC in tOTCSWELCs)
                    //        {
                    //            using (var transaction = realm.BeginWrite())
                    //            {

                    //                foreach (var item in arrWelcomeData)
                    //                {
                    //                    OTCSWELC uOTCSWELC = new OTCSWELC();
                    //                    uOTCSWELC.WELC_MSG = ((UdtWelcomeMessage)item).DESC;
                    //                    uOTCSWELC.WELC_DT = System.DateTimeOffset.Parse(((UdtWelcomeMessage)item).welcomeDate);
                    //                    uOTCSWELC.INDEX = (from d in realm.All<OTCSWELC>() select d.INDEX).Count() - 1;
                    //                    realm.Add(uOTCSWELC);
                    //                    transaction.Commit();
                    //                }

                    //            }
                    //        }
                    //    }
                    //}

                    foreach (var tOTCSWELC in tOTCSWELCs)
                    {
                        using (var transaction = realm.BeginWrite())
                        {
                            if (nContainIndex != -1)
                            {

                            }
                            else
                            {
                                foreach (var item in arrWelcomeData)
                                {
                                    OTCSWELC uOTCSWELC = new OTCSWELC();
                                    uOTCSWELC.WELC_MSG = ((UdtWelcomeMessage)item).DESC;
                                    uOTCSWELC.WELC_DT = System.DateTimeOffset.Parse(((UdtWelcomeMessage)item).welcomeDate);
                                    uOTCSWELC.INDEX = (from d in realm.All<OTCSWELC>() select d.INDEX).Count();
                                    realm.Add(uOTCSWELC);
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                });
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSuccess;
        }

        /// <summary>
        /// 예약 정보 데이터 삭제
        /// </summary>
        /// <param name="sTbNm"></param>
        /// <param name="nItemIndex"></param>
        /// <param name="dtViewDt"></param>
        /// <returns></returns>
        public bool DeleteReservationData(string sTbNm, List<int> nItemIndex, string dtViewDt)
        {
            bool isSuccess = false;
            try
            {
                var removeData = from d in realm.All<OTADHSTRY>() where d.TB_NM.Contains(sTbNm) && d.VIEW_DT.Equals(dtViewDt) select d;
                foreach (var item in removeData)
                {
                    using (var transaction = realm.BeginWrite())
                    {
                        foreach (var nIndex in nItemIndex)
                        {
                            if (nIndex == item.INDEX)
                            {
                                realm.Remove(item);
                            }
                        }
                        transaction.Commit();
                    }
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //   realm.Dispose();
            return isSuccess;
        }

        /// <summary>
        /// 책 정보 데이터 제거
        /// </summary>
        /// <param name="nItemIndex"></param>
        /// <returns></returns>
        public bool DeleteBookData(List<int> nItemIndex)
        {
            bool isSuccess = false;
            try
            {
                var removeData = realm.All<OTCSBOOK>();
                foreach (var item in removeData)
                {
                    using (var transaction = realm.BeginWrite())
                    {
                        foreach (var nIndex in nItemIndex)
                        {
                            if (nIndex == item.INDEX)
                            {
                                realm.Remove(item);
                                break;
                            }
                        }
                        transaction.Commit();
                    }
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //   realm.Dispose();
            return isSuccess;
        }

        /// <summary>
        /// 영어회화 정보 삭제
        /// </summary>
        /// <param name="nItemIndex"></param>
        /// <returns></returns>
        public bool DeleteConversationData(List<int> nItemIndex)
        {
            bool isSuccess = false;
            foreach (var index in nItemIndex)
            {
                try
                {
                    var removeData = realm.All<OTCSCONV>();
                    foreach (var item in removeData)
                    {
                        using (var transaction = realm.BeginWrite())
                        {
                            if (index == item.INDEX)
                            {
                                realm.Remove(item);
                            }

                            transaction.Commit();
                        }
                    }
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //   realm.Dispose();

            }
            return isSuccess;
        }

        /// <summary>
        /// 직업정보 삭제
        /// </summary>
        /// <param name="nItemIndex"></param>
        /// <returns></returns>
        public bool DeleteJobData(List<int> nItemIndex)
        {
            bool isSuccess = false;
            foreach (var index in nItemIndex)
            {
                try
                {
                    var removeData = realm.All<OCTSJOB>();
                    foreach (var item in removeData)
                    {
                        using (var transaction = realm.BeginWrite())
                        {
                            if (index == item.INDEX)
                            {
                                realm.Remove(item);
                            }

                            transaction.Commit();
                        }
                    }
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            //   realm.Dispose();
            return isSuccess;
        }

        /// <summary>
        /// 안전정보 삭제
        /// </summary>
        /// <param name="nItemIndex"></param>
        /// <returns></returns>
        public bool DeleteSaftyData(List<int> nItemIndex)
        {
            bool isSuccess = false;
            foreach (var index in nItemIndex)
            {
                try
                {
                    var removeData = realm.All<OTCSSAFE>();
                    foreach (var item in removeData)
                    {
                        using (var transaction = realm.BeginWrite())
                        {
                            if (index == item.INDEX)
                            {
                                realm.Remove(item);
                            }

                            transaction.Commit();
                        }
                    }
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            //   realm.Dispose();
            return isSuccess;
        }

        /// <summary>
        /// 동영상 정보 삭제
        /// </summary>
        /// <param name="nItemIndex"></param>
        /// <returns></returns>
        public bool DeleteMovieData(List<int> nItemIndex)
        {
            bool isSuccess = false;
            foreach (var index in nItemIndex)
            {
                try
                {
                    var removeData = realm.All<OTCSPRMO>();
                    foreach (var item in removeData)
                    {
                        using (var transaction = realm.BeginWrite())
                        {
                            if (index == item.INDEX)
                            {
                                realm.Remove(item);
                            }

                            transaction.Commit();
                        }
                    }
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            //   realm.Dispose();
            return isSuccess;
        }

        /// <summary>
        /// 어제 날씨 조회
        /// </summary>
        /// <returns></returns>
        public string SelectPrevWeatherData()
        {
            string sOTCSWETH = string.Empty;

            try
            {
                sOTCSWETH = (from d in realm.All<OTCSWETH>() where d.WETH_DT_STATE == Common.Constants.WeatherDate.YESTERDAY.ToString() orderby d.WETH_TEMP descending select d).FirstOrDefault().WETH_TEMP;
            }
            catch (Exception ex)
            {

                return "";
            }

            return sOTCSWETH;
        }

        /// <summary>
        /// 날씨 데이터 중 모든 날짜 상태를 자정 기준으로 Yesterday로 변경
        /// </summary>
        /// <returns></returns>
        public bool UpdatePrevWeatherData()
        {
            bool isSuccess = false;
            try
            {
                var sOTCSWETH = realm.All<OTCSWETH>();

                foreach (var item in sOTCSWETH)
                {
                    using (var transaction = realm.BeginWrite())
                    {
                        if (item.WETH_DT_STATE.Equals(Common.Constants.WeatherDate.YESTERDAY))
                        {
                            item.WETH_DT_STATE = Common.Constants.WeatherDate.PREV.ToString();
                        }
                        else if (item.WETH_DT_STATE.Equals(Common.Constants.WeatherDate.TODAY))
                        {
                            item.WETH_DT_STATE = Common.Constants.WeatherDate.YESTERDAY.ToString();
                        }
                        transaction.Commit();
                    }
                }

                isSuccess = true;

            }
            catch (Exception ex)
            {
                isSuccess = false;
                throw ex;
            }

            return isSuccess;
        }
    }
}
