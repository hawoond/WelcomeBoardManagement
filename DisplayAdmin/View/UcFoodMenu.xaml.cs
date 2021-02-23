using DisplayAdmin.Model;
using DisplayAdmin.Query;
using DisplayAdmin.Table;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DisplayAdmin.View
{
    /// <summary>
    /// UcFoodMenu.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UcFoodMenu : UserControl
    {
        private ExcuteQuery mExcuteQuery;
        private List<UdtFoodMenu> BindingData;
        private OpenFileDialog mOpenFileDialog;
        public UcFoodMenu()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            mOpenFileDialog = new OpenFileDialog();
            mOpenFileDialog.Filter = "엑셀(*.xlsx) | *.xlsx";
            //InitData();
            mExcuteQuery = ExcuteQuery.GetInstance();
        }

        public bool GetData()
        {
            bool isResult = false;

            return isResult;
        }

        public bool InsertExcelData(string path)
        {
            bool isResult = false;

            return isResult;
        }

        public bool SaveData()
        {
            bool isResult = false;

            return isResult;
        }

        private void BtnOpenFileDialog_Click(object sender, RoutedEventArgs e)
        {
            Byte[] buffer;
            mOpenFileDialog.ShowDialog();
            string sExcelPath = string.Empty;
            if (mOpenFileDialog.FileNames.Length > 0)
            {
                for (int i = 0; i < mOpenFileDialog.FileNames.Length; i++)
                {
                    mOpenFileDialog.FileNames[i].ToString();

                    sExcelPath = mOpenFileDialog.FileNames[i].ToString();
                }
            }
            tbFile.Text = sExcelPath;
            if (0 == sExcelPath.Length)
            {
                return;
            }

            dynamic objExcelData = Common.StaticUtils.SaveExcelFile(sExcelPath);

            List<UdtFoodMenu> arrUdtFoodMenu = new List<UdtFoodMenu>();

            for (int i = 2; i < objExcelData.GetLength(0) + 1; i++)
            {
                arrUdtFoodMenu.Add(new UdtFoodMenu()
                {
                    // 중식
                    DAY_DESC = objExcelData[i, 1].ToString(),
                    // 석식
                    NIGHT_DESC = objExcelData[i, 2].ToString(),
                    // 일자
                    MENU_DATE = objExcelData[i, 3].ToString()
                });
            }

            mExcuteQuery.InsertFoodData(arrUdtFoodMenu);
            SaveJson();
            // 다시 불러오기
            //SetData(GetData());
        }

        /// <summary>
        /// JSON 데이터 저장
        /// </summary>
        public void SaveJson()
        {
            List<object> arrTempResult = new List<object>();
            List<UdtFoodMenu> arrUdtFoodMenu = new List<UdtFoodMenu>();
            List<OTCSMENU> arrTodayData = mExcuteQuery.SelectDateFoodMenu(DateTime.Now.ToString("yyyy-MM-dd"));

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
    }
}
