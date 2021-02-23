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
    /// UcCelebration.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UcCelebration : UserControl
    {
        private List<UdtCelebration> BindingData;
        private ExcuteQuery mExcuteQuery;
        private OpenFileDialog mOpenFileDialog;
        public UcCelebration()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            mOpenFileDialog = new OpenFileDialog();
            mOpenFileDialog.Filter = "엑셀(*.xlsx) | *.xlsx";

            mExcuteQuery = ExcuteQuery.GetInstance();

            cdrReserveDate.SelectedDate = DateTime.Today;
            CdrReserveDate_SelectedDatesChanged(cdrReserveDate, null);
        }

        public bool GetData()
        {
            bool isResult = false;

            return isResult;
        }

        public bool SetData()
        {
            bool isResult = false;

            return isResult;
        }

        public bool SaveData()
        {
            bool isResult = false;

            return isResult;
        }

        private void CdrReserveDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            bool bAdmissionCheck = mExcuteQuery.SelectDateAdmissionData(((DateTime)((Calendar)sender).SelectedDate).ToString("yyyyMMdd"));
            bool bGraduationCheck = mExcuteQuery.SelectDateGraduationData(((DateTime)((Calendar)sender).SelectedDate).ToString("yyyyMMdd"));

            if (bAdmissionCheck)
            {
                cbAdmission.IsChecked = true;
            }
            else
            {
                cbAdmission.IsChecked = false;
            }

            if (bGraduationCheck)
            {
                cbGraduation.IsChecked = true;
            }
            else
            {
                cbGraduation.IsChecked = false;
            }

        }

        private void AdmissionButton_Click(object sender, RoutedEventArgs e)
        {
            List<int> arrIndex = new List<int>();
            arrIndex.Add(0);
            //일단 지우고 시작
            mExcuteQuery.DeleteReservationData(Common.Constants.TABLE_NAME.GRADUATION, arrIndex, ((DateTime)cdrReserveDate.SelectedDate).ToString("yyyyMMdd"));
            mExcuteQuery.DeleteReservationData(Common.Constants.TABLE_NAME.ADMISSION, arrIndex, ((DateTime)cdrReserveDate.SelectedDate).ToString("yyyyMMdd"));

            //입학체크되어있을때
            if (cbAdmission.IsChecked == true)
            {
                mExcuteQuery.InsertReservationData(Common.Constants.TABLE_NAME.ADMISSION, arrIndex, ((DateTime)cdrReserveDate.SelectedDate).ToString("yyyyMMdd"));
                //MessageBox.Show("등록되었습니다.");
            }

            //졸업체크되어있을때
            if (cbGraduation.IsChecked == true)
            {
                mExcuteQuery.InsertReservationData(Common.Constants.TABLE_NAME.GRADUATION, arrIndex, ((DateTime)cdrReserveDate.SelectedDate).ToString("yyyyMMdd"));
                //MessageBox.Show("등록되었습니다.");
            }

            SaveJson();
        }

        /// <summary>
        /// JSON 만들긔
        /// </summary>
        public void SaveJson()
        {
            Common.Constants.CelebrationCode sState;

            //입학,졸업일 체크
            bool bAdmissionCheck = mExcuteQuery.SelectDateAdmissionData(DateTime.Now.ToString("yyyyMMdd"));
            bool bGraduationCheck = mExcuteQuery.SelectDateGraduationData(DateTime.Now.ToString("yyyyMMdd"));

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
            List<OTCSSTUD> arrTodayData = mExcuteQuery.SelectStudentData(DateTime.Now.ToString("MMdd"));

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

            List<UdtCelebration> arrUdtCelebration = new List<UdtCelebration>();

            for (int i = 2; i < objExcelData.GetLength(0) + 1; i++)
            {
                arrUdtCelebration.Add(new UdtCelebration()
                {
                    // 반
                    CLASS = objExcelData[i, 3].ToString(),
                    // 학년
                    GRADE = objExcelData[i, 2].ToString(),
                    // 이름
                    NAME = objExcelData[i, 1].ToString(),
                    // 생일
                    celebrationDate = objExcelData[i, 5].ToString(),
                    isState = Common.Constants.CelebrationCode.BIRTH_DAY
                });
            }

            mExcuteQuery.InsertCelebrationData(arrUdtCelebration);
            SaveJson();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<int> arrIndex = new List<int>();
            arrIndex.Add(0);

            //일단 지우고 시작
            mExcuteQuery.DeleteReservationData(Common.Constants.TABLE_NAME.GRADUATION, arrIndex, ((DateTime)cdrReserveDate.SelectedDate).ToString("yyyyMMdd"));
            mExcuteQuery.DeleteReservationData(Common.Constants.TABLE_NAME.ADMISSION, arrIndex, ((DateTime)cdrReserveDate.SelectedDate).ToString("yyyyMMdd"));

            cbGraduation.IsChecked = false;
            cbAdmission.IsChecked = false;

            SaveJson();
        }
    }
}
