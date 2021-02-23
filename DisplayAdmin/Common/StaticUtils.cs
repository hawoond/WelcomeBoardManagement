using DisplayAdmin.Model;
using DisplayAdmin.Query;
using DisplayAdmin.Table;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static DisplayAdmin.Common.SharedAPI;

namespace DisplayAdmin.Common
{
    public class StaticUtils
    {


        private static StaticUtils instance;
        public static StaticUtils getInstance()
        {
            if (instance == null)
            {
                instance = new StaticUtils();
            }
            return instance;
        }

        public static string TODAY = DateTime.Now.ToString("yyyyMMdd");

        public static bool SaveJsonFile(string sFileName, List<object> udtList, Common.Constants.CelebrationCode sState)
        {
            bool isSuccess = false;

            string sFilePath = Properties.Settings.Default.ROOT_FILE_PATH + Properties.Settings.Default.JSON_FILE_PATH + sFileName + ".json";

            try
            {
                JArray jArray = new JArray();
                JObject jSubObject;

                //  JConstructor jc = new JConstructor();

                foreach (var item in udtList)
                {
                    jSubObject = new JObject();
                    Type tp = item.GetType();
                    FieldInfo[] flds = tp.GetFields(BindingFlags.Instance |
                                                    BindingFlags.Static |
                                                    BindingFlags.Public |
                                                    BindingFlags.NonPublic);
                    foreach (var f in flds)
                    {
                        object point = f.GetValue(item);
                        if (point is string)
                        {
                            jSubObject.Add(f.Name, point.ToString());
                        }
                        if (point is int)
                        {
                            jSubObject.Add(f.Name, point.ToString());
                        }
                    }
                    jArray.Add(jSubObject);
                }

                JObject jObject = new JObject();
                jObject.Add("groot", sFileName);
                jObject.Add("DATE", System.DateTime.Now.ToString("yyyyMMdd"));
                jObject.Add("STATE", (int)sState);
                jObject.Add(Constants.GetJsonKey(sFileName), jArray);

                isSuccess = true;

                //파일 저장
                SaveFile(sFilePath, jObject.ToString());
            }
            catch
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        private static bool SaveFile(string sFilePath, string sJson)
        {
            bool isSuccess = false;

            try
            {
                if (!File.Exists(sFilePath))
                {
                    File.Create(sFilePath);
                }

                //if (!File.Exists("\\\\192.168.0.22\\공유 폴더\\jireh\\DisplaySharedData\\JSON\\UDTCONFIG.json"))
                //{
                //    File.Create(sFilePath);
                //}

                System.IO.File.WriteAllText(sFilePath, sJson, Encoding.UTF8);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public static dynamic SaveExcelFile(string sExcelPath)
        {
            Application application = null;
            Workbook workbook = null;
            Worksheet worksheet = null;
            Range range = null;

            try
            {
                application = new Application();

                workbook = application.Workbooks.Open(sExcelPath);

                worksheet = workbook.Worksheets.Item[workbook.Worksheets.Count];
                //range = worksheet.Range[worksheet.Cells[2, 1], worksheet.Cells[5, 2]];
                range = worksheet.UsedRange;
                //workbook.Close(true);
                //application.Quit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //ReleaseExcelObject(application);
                //ReleaseExcelObject(workbook);
                //ReleaseExcelObject(worksheet);
            }

            return range.Value2;
        }


        public static string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                rtb.Document.ContentStart,
                rtb.Document.ContentEnd
            );

            return textRange.Text;
        }

        public static void SaveAllJson()
        {
            SaveBookJson();
            SaveJobJson();
            SaveSaftyInfoJson();
            SaveConversationJson();
            SaveTeachersDayJson();
            SaveWelcomeJson();
            SaveFoodMenuJson();
            SaveCelebrationJson();
            SaveMovieJson();
        }


        /// <summary>
        /// 날짜별 책 데이터를 Json으로 저장
        /// </summary>
        private static void SaveBookJson()
        {
            try
            {
                //// 책 예약 정보 xml에 저장 -> 타이머로 하지 않을까? -> 타이머로 함 wj.kim 2017.07.26
                //Common.StaticUtils.SaveJsonFile(Common.Constants.FILE_KEY.BOOK, arrTempResult);
                List<object> arrTempResult = new List<object>();
                List<UdtBook> arrUdtBook = new List<UdtBook>();
                List<OTCSBOOK> arrTodayData = ExcuteQuery.GetInstance().SelectDateBookData(DateTime.Now.ToString("yyyyMMdd"));

                //데이터 없을 때 랜덤값 넣기
                if (arrTodayData.Count == 0)
                {
                    arrUdtBook.Clear();
                    List<OTCSBOOK> arrTableBook = ExcuteQuery.GetInstance().SelectBookData();
                    if (arrTableBook.Count == 0)
                    {
                        return;
                    }
                    Random rndIndex = new Random();
                    for (int i = 0; i < 3; i++)
                    {
                        int nIndex = (rndIndex.Next(0, arrTableBook.Count - 1));

                        arrUdtBook.Add(new UdtBook()
                        {
                            AUTH = arrTableBook[nIndex].BOOK_AUTH,
                            DATE = arrTableBook[nIndex].BOOK_DT,
                            DESC = arrTableBook[nIndex].BOOK_DESC,
                            IMAGE = arrTableBook[nIndex].IMG_PATH,
                            TITLE = arrTableBook[nIndex].BOOK_NM
                        });
                    }
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        arrUdtBook.Add(new UdtBook()
                        {
                            AUTH = arrTodayData[i].BOOK_AUTH,
                            DATE = arrTodayData[i].BOOK_DT,
                            DESC = arrTodayData[i].BOOK_DESC,
                            IMAGE = arrTodayData[i].IMG_PATH,
                            TITLE = arrTodayData[i].BOOK_NM
                        });
                    }
                }

                foreach (UdtBook udtBook in arrUdtBook)
                {
                    arrTempResult.Add(udtBook);
                }

                // 책 예약 정보 xml에 저장 -> 타이머로 하지 않을까?
                Common.StaticUtils.SaveJsonFile(Common.Constants.FILE_KEY.BOOK, arrTempResult, Common.Constants.CelebrationCode.NONE);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 직업정보 Json 데이터 저장
        /// </summary>
        private static void SaveJobJson()
        {
            try
            {
                List<object> arrTempResult = new List<object>();
                List<UdtJob> arrUdtJob = new List<UdtJob>();
                List<OCTSJOB> arrTodayData = ExcuteQuery.GetInstance().SelectDateJobData(DateTime.Now.ToString("yyyyMMdd"));

                if (arrTodayData.Count == 0)
                {
                    arrUdtJob.Clear();
                    List<OCTSJOB> arrTableJob = ExcuteQuery.GetInstance().SelectJobData();
                    if (arrTableJob.Count == 0)
                    {
                        return;
                    }
                    Random rndIndex = new Random();
                    int nIndex = (rndIndex.Next(0, arrTableJob.Count - 1));

                    arrUdtJob.Add(new UdtJob()
                    {
                        TITLE = arrTableJob[nIndex].JOB_NM,
                        INFO = arrTableJob[nIndex].JOB_DESC,
                        IMAGE = arrTableJob[nIndex].IMG_PATH,
                        jobDate = arrTableJob[nIndex].JOB_DT
                    });
                }
                else
                {
                    arrUdtJob.Add(new UdtJob()
                    {
                        TITLE = arrTodayData[0].JOB_NM,
                        INFO = arrTodayData[0].JOB_DESC,
                        IMAGE = arrTodayData[0].IMG_PATH,
                        jobDate = arrTodayData[0].JOB_DT
                    });
                }


                foreach (UdtJob udtJob in arrUdtJob)
                {
                    arrTempResult.Add(udtJob);
                }

                // 직업 예약 정보 xml에 저장
                Common.StaticUtils.SaveJsonFile(Common.Constants.FILE_KEY.JOB, arrTempResult, Common.Constants.CelebrationCode.NONE);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 영어 회화 Json 저장
        /// </summary>
        private static void SaveConversationJson()
        {
            List<object> arrTempResult = new List<object>();
            List<UdtConversation> arrUdtConv = new List<UdtConversation>();
            List<OTCSCONV> arrTodayData = ExcuteQuery.GetInstance().SelectDateConvData(DateTime.Now.ToString("yyyyMMdd"));

            if (arrTodayData.Count == 0)
            {
                arrUdtConv.Clear();
                List<OTCSCONV> arrTableConv = ExcuteQuery.GetInstance().SelectConversationDatas();
                if (arrTableConv.Count == 0)
                {
                    return;
                }

                Random rndIndex = new Random();
                int nIndex = (rndIndex.Next(0, arrTableConv.Count - 1));

                arrUdtConv.Add(new UdtConversation()
                {
                    TITLE = arrTableConv[nIndex].CONV_TITLE,
                    TRANSLATE = arrTableConv[nIndex].CONV_TRANS
                });
            }
            else
            {
                arrUdtConv.Add(new UdtConversation()
                {
                    TITLE = arrTodayData[0].CONV_TITLE,
                    TRANSLATE = arrTodayData[0].CONV_TRANS
                });
            }

            foreach (UdtConversation udtConv in arrUdtConv)
            {
                arrTempResult.Add(udtConv);
            }

            // 영어 회화 예약 정보 xml에 저장
            Common.StaticUtils.SaveJsonFile(Common.Constants.FILE_KEY.CONVERSATION, arrTempResult, Common.Constants.CelebrationCode.NONE);
        }

        /// <summary>
        /// 안전정보 Json 데이터 저장
        /// </summary>
        public static void SaveSaftyInfoJson()
        {
            List<object> arrTempResult = new List<object>();
            List<UdtSaftyInfo> arrUdtSafty = new List<UdtSaftyInfo>();
            List<OTCSSAFE> arrTodayData = ExcuteQuery.GetInstance().SelectDateSaftyData(DateTime.Now.ToString("yyyyMMdd"));

            if (arrTodayData.Count == 0)
            {
                arrUdtSafty.Clear();
                List<OTCSSAFE> arrTableSafty = ExcuteQuery.GetInstance().SelectSaftyData();
                if (arrTableSafty.Count == 0)
                {
                    return;
                }
                Random rndIndex = new Random();
                int nIndex = (rndIndex.Next(0, arrTableSafty.Count - 1));

                arrUdtSafty.Add(new UdtSaftyInfo()
                {
                    TITLE = arrTableSafty[nIndex].SAFE_TITLE,
                    DESC = arrTableSafty[nIndex].SAFE_DESC,
                    saftyDate = arrTableSafty[nIndex].SAFE_DT
                });
            }
            else
            {
                arrUdtSafty.Add(new UdtSaftyInfo()
                {
                    TITLE = arrTodayData[0].SAFE_TITLE,
                    DESC = arrTodayData[0].SAFE_DESC,
                    saftyDate = arrTodayData[0].SAFE_DT
                });
            }


            foreach (UdtSaftyInfo udtSaftyInfo in arrUdtSafty)
            {
                arrTempResult.Add(udtSaftyInfo);
            }

            // 안전 예약 정보 xml에 저장
            Common.StaticUtils.SaveJsonFile(Common.Constants.FILE_KEY.SAFTY_INFO, arrTempResult, Common.Constants.CelebrationCode.NONE);
        }

        public static void SaveWeatherJson()
        {
            try
            {
                IMRUtils.CallRestFul callRestFul = new IMRUtils.CallRestFul();
                IMRUtils.Model.UdtWeather arrWeather = callRestFul.CallWeatherApi();

                string sTempResult = ExcuteQuery.GetInstance().SelectPrevWeatherData();

                string sTempMessageResult = string.Empty;

                string sMessage = string.Empty;

                double nTempResult = 0;

                if (sTempResult.Equals(string.Empty))
                {
                    sTempResult = arrWeather.Temp;
                }

                double nTodayTemp = double.Parse(arrWeather.Temp);
                double nPrevTemp = double.Parse(sTempResult);

                if (nTodayTemp > nPrevTemp)
                {
                    sMessage = Properties.Resources.MessageTempUp;
                    nTempResult = nTodayTemp - nPrevTemp;
                }
                else
                {
                    sMessage = Properties.Resources.MessageTempDown;
                    nTempResult = nPrevTemp - nTodayTemp;
                }

                sTempMessageResult = nTempResult + "★" + sMessage;

                UdtWeather udtResult = new UdtWeather()
                {
                    DESC = sTempMessageResult,
                    FINE_DUST = arrWeather.FDust,
                    TEMP = arrWeather.Temp,
                    STATE = "1",
                    ULTRA_FINE_PARTICLE = arrWeather.UfDust,
                    POP = arrWeather.Pop,
                    SENS_TEMP = arrWeather.SensTemp,
                    dateState = Common.Constants.WeatherDate.TODAY
                };

                List<object> arrTempResult = new List<object>();
                arrTempResult.Add(udtResult);

                // 책 예약 정보 xml에 저장 -> 타이머로 하지 않을까?
                Common.StaticUtils.SaveJsonFile(Common.Constants.FILE_KEY.WEATHER, arrTempResult, Common.Constants.CelebrationCode.NONE);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        /// <summary>
        /// 환영 메시지 Json 만들기
        /// </summary>
        public static void SaveWelcomeJson()
        {
            List<object> arrTempResult = new List<object>();
            List<UdtWelcomeMessage> arrUdtWelcomeMessage = new List<UdtWelcomeMessage>();

            // 저장된 데이터 가져오기
            OTCSWELC data = ExcuteQuery.GetInstance().SelectWelcomeData();

            if (data == null)
            {
                return;
            }

            arrUdtWelcomeMessage.Add(new UdtWelcomeMessage()
            {
                DESC = data.WELC_MSG
            });


            foreach (UdtWelcomeMessage udtWelcomeMessage in arrUdtWelcomeMessage)
            {
                arrTempResult.Add(udtWelcomeMessage);
            }

            // 책 예약 정보 xml에 저장 -> 타이머로 하지 않을까?
            Common.StaticUtils.SaveJsonFile(Common.Constants.FILE_KEY.WELCOME_MESSAGE, arrTempResult, Common.Constants.CelebrationCode.NONE);
        }

        /// <summary>
        /// 스승의날 메시지 JSON 만들기
        /// </summary>
        public static void SaveTeachersDayJson()
        {
            List<object> arrTempResult = new List<object>();
            List<UdtTeachersDay> arrUdtTeachersDay = new List<UdtTeachersDay>();

            // 저장된 데이터 가져오기
            OTCSTCDY data = ExcuteQuery.GetInstance().SelectTeachersDayData();

            if (data == null)
            {
                return;
            }

            arrUdtTeachersDay.Add(new UdtTeachersDay()
            {
                DESC = data.CELE_MSG
            });


            foreach (UdtTeachersDay udtTeachersDay in arrUdtTeachersDay)
            {
                arrTempResult.Add(udtTeachersDay);
            }

            // 책 예약 정보 xml에 저장 -> 타이머로 하지 않을까?
            Common.StaticUtils.SaveJsonFile(Common.Constants.FILE_KEY.TEACHERS_DAY, arrTempResult, Common.Constants.CelebrationCode.NONE);
        }

        /// <summary>
        /// 급식 식단 Json 만들기
        /// </summary>
        private static void SaveFoodMenuJson()
        {
            List<object> arrTempResult = new List<object>();
            List<UdtFoodMenu> arrUdtFoodMenu = new List<UdtFoodMenu>();
            List<OTCSMENU> arrTodayData = ExcuteQuery.GetInstance().SelectDateFoodMenu(DateTime.Now.ToString("yyyy-MM-dd"));

            //데이터 없을 때 랜덤값 넣기
            if (arrTodayData.Count == 0)
            {
                arrUdtFoodMenu.Clear();

                arrUdtFoodMenu.Add(new UdtFoodMenu()
                {
                    DAY_DESC = "식단 정보가 없습니다.",
                    NIGHT_DESC = "식단 정보가 없습니다.",
                    MENU_DATE = DateTime.Now.ToString("yyyy-MM-dd")
                });

            }
            else
            {
                for (int i = 0; i < arrTodayData.Count; i++)
                {
                    arrUdtFoodMenu.Add(new UdtFoodMenu()
                    {
                        DAY_DESC = arrTodayData[i].MENU_DAY_DESC,
                        NIGHT_DESC = arrTodayData[i].MENU_NIGHT_DESC,
                        MENU_DATE = arrTodayData[i].MENU_DT
                    });
                }
            }

            foreach (UdtFoodMenu udtFoodMenu in arrUdtFoodMenu)
            {
                arrTempResult.Add(udtFoodMenu);
            }

            // 책 예약 정보 xml에 저장 -> 타이머로 하지 않을까?
            Common.StaticUtils.SaveJsonFile(Common.Constants.FILE_KEY.FOOD_MENU, arrTempResult, Common.Constants.CelebrationCode.NONE);
        }

        /// <summary>
        /// 축하메세지 Json 만들기
        /// </summary>
        private static void SaveCelebrationJson()
        {
            Common.Constants.CelebrationCode sState;

            //입학,졸업일 체크
            bool bAdmissionCheck = ExcuteQuery.GetInstance().SelectDateAdmissionData(DateTime.Now.ToString("yyyyMMdd"));
            bool bGraduationCheck = ExcuteQuery.GetInstance().SelectDateGraduationData(DateTime.Now.ToString("yyyyMMdd"));

            if (bAdmissionCheck)
            {
                sState = Common.Constants.CelebrationCode.START_GAME;
            }
            else if (bGraduationCheck)
            {
                sState = Common.Constants.CelebrationCode.END_GAME;
            }
            else
            {
                sState = Common.Constants.CelebrationCode.BIRTH_DAY;
            }

            List<object> arrTempResult = new List<object>();
            List<UdtCelebration> arrUdtCelebration = new List<UdtCelebration>();

            // 저장된 데이터(학생 생일정보) 가져오기
            List<OTCSSTUD> arrTodayData = ExcuteQuery.GetInstance().SelectStudentData(DateTime.Now.ToString("MMdd"));

            foreach (OTCSSTUD data in arrTodayData)
            {
                arrUdtCelebration.Add(new UdtCelebration()
                {
                    NAME = data.STD_NM,
                    GRADE = data.STD_GRADE,
                    CLASS = data.STD_CLASS,
                    isState = sState
                });
            }


            foreach (UdtCelebration udtCelebration in arrUdtCelebration)
            {
                arrTempResult.Add(udtCelebration);
            }

            // xml에 저장
            Common.StaticUtils.SaveJsonFile(Common.Constants.FILE_KEY.CELEBRATION, arrTempResult, sState);
        }

        /// <summary>
        /// 엑셀 객체 클리어
        /// </summary>
        /// <param name="obj"></param>
        private static void ReleaseExcelObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// 동영상 Json 만들기
        /// </summary>
        private static void SaveMovieJson()
        {
            List<object> arrTempResult = new List<object>();
            List<UdtPromotionMovie> arrUdtMovie = new List<UdtPromotionMovie>();
            List<OTCSPRMO> arrTodayData = ExcuteQuery.GetInstance().SelectDateMovieData(DateTime.Now.ToString("yyyyMMdd"));

            if (arrTodayData.Count == 0)
            {
                arrUdtMovie.Clear();
                List<OTCSPRMO> arrTableMovie = ExcuteQuery.GetInstance().SelectMovieData();

                // 데이터 한개도 없을땐 저장 안하기
                if (arrTableMovie.Count == 0)
                {
                    return;
                }

                Random rndIndex = new Random();
                int nIndex = (rndIndex.Next(0, arrTableMovie.Count - 1));

                arrUdtMovie.Add(new UdtPromotionMovie()
                {
                    movieName = arrTableMovie[nIndex].MO_NM,
                    FILEPATH = arrTableMovie[nIndex].MO_PATH
                });
            }
            else
            {
                arrUdtMovie.Add(new UdtPromotionMovie()
                {
                    movieName = arrTodayData[0].MO_NM,
                    FILEPATH = arrTodayData[0].MO_PATH
                });
            }


            foreach (UdtPromotionMovie udtMovie in arrUdtMovie)
            {
                arrTempResult.Add(udtMovie);
            }

            // 책 예약 정보 xml에 저장 -> 타이머로 하지 않을까?
            Common.StaticUtils.SaveJsonFile(Common.Constants.FILE_KEY.PROMOTION_MOVIE, arrTempResult, Common.Constants.CelebrationCode.NONE);
        }

        /// <summary>
        /// 컨트롤을 PNG로 저장
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="fileName"></param>
        public static void SaveToPng(ImageSource visual, string fileName)
        {
            var encoder = new PngBitmapEncoder();
            SaveUsingEncoder(visual, fileName);
        }

        private static void SaveUsingEncoder(ImageSource visual, string fileName)
        {
            PngBitmapEncoder png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create((BitmapSource)visual));

            using (MemoryStream stream = new MemoryStream())
            {
                png.Save(stream);
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(stream))
                {
                    image.Save(fileName);
                }
            }
        }

        public static void FileCopy(string sOldFilePath, string sNewFilePath)
        {
            string filePath = Path.GetDirectoryName(sOldFilePath);  // 파일 경로
            string filename_without_ext = Path.GetFileNameWithoutExtension(sOldFilePath);  // 파일 이름
            string ext_only = Path.GetExtension(sOldFilePath);  // 파일 확장자

            try
            {
                if (File.Exists(sNewFilePath))  // 파일의 존재 유무 확인 : 파일이 존재하면
                {
                    return;
                }
                else   // 파일의 존재하지 않으면    
                {
                    File.Copy(sOldFilePath, sNewFilePath);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
    public class SharedAPI
    {
        // 구조체 선언
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct NETRESOURCE
        {
            public uint dwScope;
            public uint dwType;
            public uint dwDisplayType;
            public uint dwUsage;
            public string lpLocalName;
            public string lpRemoteName;
            public string lpComment;
            public string lpProvider;
        }

        // API 함수 선언
        [DllImport("mpr.dll", CharSet = CharSet.Auto)]
        public static extern int WNetUseConnection(
                    IntPtr hwndOwner,
                    [MarshalAs(UnmanagedType.Struct)] ref NETRESOURCE lpNetResource,
                    string lpPassword,
                    string lpUserID,
                    uint dwFlags,
                    StringBuilder lpAccessName,
                    ref int lpBufferSize,
                    out uint lpResult);

        // API 함수 선언 (공유해제)
        [DllImport("mpr.dll", EntryPoint = "WNetCancelConnection2", CharSet = CharSet.Auto)]
        public static extern int WNetCancelConnection2A(string lpName, int dwFlags, int fForce);
    }
}
