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
    /// UcSaftyInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UcSaftyInfo : UserControl
    {
        private ExcuteQuery mExcuteQuery;
        private List<OTCSSAFE> arrSaftyList;
        private List<UdtSaftyInfo> BindingData;
        private OpenFileDialog mOpenFileDialog;

        public UcSaftyInfo()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            mOpenFileDialog = new OpenFileDialog();
            mOpenFileDialog.Filter = "엑셀(*.xlsx) | *.xlsx";

            mExcuteQuery = ExcuteQuery.GetInstance();
            // 저장된 데이터 가져오기
            List<UdtReservList> arrSaftyData = GetData();

            // 가져온 데이터 세팅
            SetData(arrSaftyData);

            cdrReserveDate.SelectedDate = DateTime.Today;
            CdrReserveDate_SelectedDatesChanged(cdrReserveDate, null);
        }

        public List<UdtReservList> GetData()
        {
            bool isResult = false;
            // 저장된 리스트 전체 조회
            this.arrSaftyList = mExcuteQuery.SelectSaftyData();
            List<UdtReservList> arrSaftyList = new List<UdtReservList>();

            // 직업명 리스트에 담기
            foreach (var itemSafty in this.arrSaftyList)
            {
                arrSaftyList.Add(new UdtReservList { DESC = itemSafty.SAFE_TITLE, INDEX = itemSafty.INDEX });
            }

            return arrSaftyList;
        }

        public bool SetData(List<UdtReservList> arrSaftyList)
        {
            bool isResult = false;
            try
            {
                lbAllSaftyList.ItemsSource = arrSaftyList;
                isResult = true;
            }
            catch
            {
                isResult = false;
            }
            return isResult;
        }

        private void CdrReserveDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            lbReserveList.Items.Clear();
            List<OTCSSAFE> arrUdtSafty = mExcuteQuery.SelectDateSaftyData(((DateTime)((Calendar)sender).SelectedDate).ToString("yyyyMMdd"));
            foreach (OTCSSAFE udtSafty in arrUdtSafty)
            {
                lbReserveList.Items.Add(new UdtReservList { DESC = udtSafty.SAFE_TITLE, INDEX = udtSafty.INDEX });
            }
        }

        /// <summary>
        /// 드래그&드랍 관련 함수
        /// </summary>
        /// <param name="source"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private object GetDataFromListBox(ListBox source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);

                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }

                    if (element == source)
                    {
                        return null;
                    }
                }

                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }
            return null;
        }

        ListBox dragSource = null;
        private void LbAllSaftyList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;

            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
            }
        }

        private void LbReserveList_Drop(object sender, DragEventArgs e)
        {
            ListBox parent = (ListBox)sender;

            if (parent.Items.Count == 1)
            {
                MessageBox.Show("정보는 1개만 등록 가능합니다.");
                return;
            }

            object data = e.Data.GetData(typeof(UdtReservList));
            //((IList)dragSource.ItemsSource).Remove(data);
            parent.Items.Add(data);
            List<int> arrIndex = new List<int>();
            // 드래그 해서 이동 한 책 이름으로 책 정보 가져오기
            UdtSaftyInfo udtResult = new UdtSaftyInfo();
            foreach (var item in arrSaftyList)
            {
                if (item.INDEX.Equals(((UdtReservList)data).INDEX))
                {
                    udtResult = new UdtSaftyInfo()
                    {
                        TITLE = udtResult.TITLE,
                        DESC = udtResult.DESC,
                        saftyDate = udtResult.saftyDate
                    };

                    arrIndex.Add(item.INDEX);
                }
            }

            // 예약 정보 DB에 저장
            if (null == cdrReserveDate.SelectedDate)
            {
                mExcuteQuery.InsertReservationData(Common.Constants.TABLE_NAME.SAFETY, arrIndex, DateTime.Now.ToString("yyyyMMdd"));
            }
            else
            {
                mExcuteQuery.InsertReservationData(Common.Constants.TABLE_NAME.SAFETY, arrIndex, ((DateTime)cdrReserveDate.SelectedDate).ToString("yyyyMMdd"));
            }

            SaveJson();
        }

        /// <summary>
        /// JSON 만들긔
        /// </summary>
        public void SaveJson()
        {
            List<object> arrTempResult = new List<object>();
            List<UdtSaftyInfo> arrUdtSafty = new List<UdtSaftyInfo>();
            List<OTCSSAFE> arrTodayData = mExcuteQuery.SelectDateSaftyData(DateTime.Now.ToString("yyyyMMdd"));

            if (arrTodayData.Count == 0)
            {
                arrUdtSafty.Clear();
                List<OTCSSAFE> arrTableSafe = mExcuteQuery.SelectSaftyData();

                // 데이터 한개도 없을땐 저장 안하기
                if (arrTableSafe.Count == 0)
                {
                    return;
                }

                Random rndIndex = new Random();
                int nIndex = (rndIndex.Next(0, arrTableSafe.Count - 1));

                arrUdtSafty.Add(new UdtSaftyInfo()
                {
                    TITLE = arrTableSafe[nIndex].SAFE_TITLE,
                    DESC = arrTableSafe[nIndex].SAFE_DESC,
                    saftyDate = arrTableSafe[nIndex].SAFE_DT
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

            // 책 예약 정보 xml에 저장 -> 타이머로 하지 않을까?
            Common.StaticUtils.SaveJsonFile(Common.Constants.FILE_KEY.SAFTY_INFO, arrTempResult, Common.Constants.CelebrationCode.NONE);
        }

        private void LbAllSaftyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UdtSaftyInfo udtResult = new UdtSaftyInfo();
            foreach (var item in arrSaftyList)
            {
                if (((UdtReservList)((ListBox)sender).SelectedItem).INDEX.Equals(item.INDEX))
                {
                    udtResult = new UdtSaftyInfo()
                    {
                        TITLE = item.SAFE_TITLE,
                        DESC = item.SAFE_DESC
                    };
                }
            }
            SetConversation(udtResult);
        }

        private void SetConversation(UdtSaftyInfo udtSafty)
        {
            tbSaftyTitle.Text = udtSafty.TITLE;
            tbSaftyDesc.Text = udtSafty.DESC;
            //이미지 넣어야됨
        }

        private void Button_Click_Table_Insert(object sender, RoutedEventArgs e)
        {
            UdtSaftyInfo udtSafty = new UdtSaftyInfo()
            {
                TITLE = tbSaftyTitle.Text,
                DESC = tbSaftyDesc.Text
            };

            mExcuteQuery.InsertSaftyData(udtSafty);
            // 다시 불러오기
            SetData(GetData());
        }

        private void LbAllSaftyList_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;

            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));
            //    parent.Items.Refresh();
            if (null == data)
            {
                return;
            }
            List<OTCSSAFE> arrUdtSafty = mExcuteQuery.SelectDateSaftyData("");

            foreach (var item in arrUdtSafty)
            {
                if (item.INDEX.Equals(((UdtReservList)data).INDEX))
                {
                    MessageBox.Show("예약 해제 후 삭제 해주세요.");
                    return;
                }
            }
            /// DB에서 데이터 지우기
            // 제거할 데이터 인덱스
            List<int> arrIndex = new List<int>();
            // 우클린 한 데이터 정보 가져오기
            UdtSaftyInfo udtSafty = new UdtSaftyInfo();
            foreach (var item in arrSaftyList)
            {
                if (item.INDEX.Equals(((UdtReservList)data).INDEX))
                {
                    udtSafty = new UdtSaftyInfo()
                    {
                        TITLE = item.SAFE_TITLE,
                        DESC = item.SAFE_DESC,
                        saftyDate = item.SAFE_DT
                    };

                    arrIndex.Add(item.INDEX);

                    arrSaftyList.Remove(item);
                    break;
                }
            }

            // DB에서 지우기
            mExcuteQuery.DeleteSaftyData(arrIndex);

            // 저장된 데이터 가져오기
            List<UdtReservList> arrSaftyData = GetData();
            // 가져온 데이터 세팅
            SetData(arrSaftyData);
        }

        private void LbReserveList_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;

            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

            //데이터 없을땐 리턴
            if (data == null)
            {
                return;
            }

            //    parent.Items.Refresh();
            parent.Items.Remove(data);

            /// DB에서 데이터 지우기
            // 제거할 데이터 인덱스
            List<int> arrIndex = new List<int>();
            // 우클린 한 데이터 정보 가져오기
            UdtConversation udtResult = new UdtConversation();
            foreach (var item in arrSaftyList)
            {
                if (item.INDEX.Equals(((UdtReservList)data).INDEX))
                {
                    arrIndex.Add(item.INDEX);
                }
            }

            // DB에서 지우기
            mExcuteQuery.DeleteReservationData(Common.Constants.TABLE_NAME.SAFETY, arrIndex, DateTime.Now.ToString("yyyyMMdd"));

            // xml 데이터 다시 저장
            SaveJson();
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

            List<object> arrUdtSaftyInfo = new List<object>();

            for (int i = 2; i < objExcelData.GetLength(0) + 1; i++)
            {
                arrUdtSaftyInfo.Add(new UdtSaftyInfo()
                {
                    // 내용
                    DESC = objExcelData[i, 2].ToString(),
                    // 제목
                    TITLE = objExcelData[i, 1].ToString()
                });
            }

            mExcuteQuery.InsertSaftyData(arrUdtSaftyInfo);
            SaveJson();

            // 가져온 데이터 세팅
            SetData(GetData());
        }
    }
}
