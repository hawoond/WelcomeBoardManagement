using DisplayAdmin.Model;
using DisplayAdmin.Query;
using DisplayAdmin.Table;
using DisplayAdmin.View.Control;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
    /// ucConversation.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UcConversation : UserControl
    {
        private ExcuteQuery mExcuteQuery;
        private List<UdtConversation> BindingData;
        private List<OTCSCONV> arrConversationLists;
        private OpenFileDialog mOpenFileDialog;

        private IMRUtils.OverrideType.MTObservableCollection<UcConversationItem> mConversations;
        public IMRUtils.OverrideType.MTObservableCollection<UcConversationItem> Conversations
        {
            get
            {
                if (mConversations == null)
                {
                    mConversations = new IMRUtils.OverrideType.MTObservableCollection<UcConversationItem>();
                }
                return mConversations;
            }
            set
            {
                mConversations = value;
                OnPropertyChanged("Conversations");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public UcConversation()
        {
            InitializeComponent();
            this.DataContext = this;
            Init();
        }

        public void Init()
        {
            mOpenFileDialog = new OpenFileDialog();
            mOpenFileDialog.Filter = "엑셀(*.xlsx) | *.xlsx";
            //InitData();
            mExcuteQuery = ExcuteQuery.GetInstance();
            // 저장된 데이터 가져오기
            List<UdtReservList> arrConvData = GetData();
            // 가져온 데이터 세팅
            SetData(arrConvData);
            //lbConversationList.ItemsSource = 
            cdrReserveDate.SelectedDate = DateTime.Today;
            CdrReserveDate_SelectedDatesChanged(cdrReserveDate, null);
        }

        public List<UdtReservList> GetData()
        {
            bool isResult = false;
            // 저장된 책 리스트 전체 조회
            arrConversationLists = mExcuteQuery.SelectConversationDatas();

            List<UdtReservList> arrConvList = new List<UdtReservList>();

            // 책 이름만 리스트에 담기
            foreach (var itemConv in arrConversationLists)
            {
                arrConvList.Add(new UdtReservList { DESC = itemConv.CONV_TITLE + "\n" + itemConv.CONV_TRANS, INDEX = itemConv.INDEX });
            }

            return arrConvList;
        }

        public bool SetData(List<UdtReservList> arrConvList)
        {
            bool isResult = false;
            try
            {
                lbAllConvList.ItemsSource = arrConvList;
                isResult = true;
            }
            catch
            {
                isResult = false;
            }
            return isResult;
        }

        public bool InsertData()
        {
            bool isResult = false;

            return isResult;
        }

        public bool SaveData()
        {
            bool isResult = false;

            UdtConversation udtConversation = new UdtConversation()
            {
                TITLE = tbEnglish.Text,
                TRANSLATE = tbKorean.Text
            };

            mExcuteQuery.InsertConvData(udtConversation);
            // 다시 불러오기
            SetData(GetData());

            return isResult;
        }

        private void Button_Click_Table_Insert(object sender, RoutedEventArgs e)
        {
            UdtConversation udtConversation = new UdtConversation()
            {
                TITLE = tbEnglish.Text,
                TRANSLATE = tbKorean.Text
            };

            mExcuteQuery.InsertConvData(udtConversation);
            // 다시 불러오기
            SetData(GetData());
        }

        private T FindAncestor<T>(DependencyObject obj)
            where T : DependencyObject
        {
            do
            {
                if (obj is T)
                {
                    return (T)obj;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
            while (obj != null);
            return null;
        }

        private void CdrReserveDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            lbReserveList.Items.Clear();
            List<OTCSCONV> arrUdtConv = mExcuteQuery.SelectDateConvData(((DateTime)((Calendar)sender).SelectedDate).ToString("yyyyMMdd"));
            foreach (OTCSCONV udtConv in arrUdtConv)
            {
                lbReserveList.Items.Add(new UdtReservList { DESC = udtConv.CONV_TITLE + "\n" + udtConv.CONV_TRANS, INDEX = udtConv.INDEX });
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
        private void LbAllConvList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
            UdtConversation udtResult = new UdtConversation();
            foreach (var item in arrConversationLists)
            {
                if (item.INDEX.Equals(((UdtReservList)data).INDEX))
                {
                    udtResult = new UdtConversation()
                    {
                        TITLE = item.CONV_TITLE,
                        TRANSLATE = item.CONV_TRANS
                    };

                    arrIndex.Add(item.INDEX);
                }
            }

            // 책 예약 정보 DB에 저장
            //udtResult.BookAuth;
            if (null == cdrReserveDate.SelectedDate)
            {
                mExcuteQuery.InsertReservationData(Common.Constants.TABLE_NAME.CONVERSATION, arrIndex, DateTime.Now.ToString("yyyyMMdd"));
            }
            else
            {
                mExcuteQuery.InsertReservationData(Common.Constants.TABLE_NAME.CONVERSATION, arrIndex, ((DateTime)cdrReserveDate.SelectedDate).ToString("yyyyMMdd"));
            }

            SaveJson();
        }

        /// <summary>
        /// JSON 만들긔
        /// </summary>
        public void SaveJson()
        {
            List<object> arrTempResult = new List<object>();
            List<UdtConversation> arrUdtConv = new List<UdtConversation>();
            List<OTCSCONV> arrTodayData = mExcuteQuery.SelectDateConvData(DateTime.Now.ToString("yyyyMMdd"));

            if (arrTodayData.Count == 0)
            {
                arrUdtConv.Clear();
                List<OTCSCONV> arrTableConv = mExcuteQuery.SelectConversationDatas();
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

            // 책 예약 정보 xml에 저장 -> 타이머로 하지 않을까?
            Common.StaticUtils.SaveJsonFile(Common.Constants.FILE_KEY.CONVERSATION, arrTempResult, Common.Constants.CelebrationCode.NONE);
        }

        private void LbAllConvList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UdtConversation udtResult = new UdtConversation();
            foreach (var item in arrConversationLists)
            {
                if (((UdtReservList)((ListBox)sender).SelectedItem).INDEX.Equals(item.INDEX))
                {
                    udtResult = new UdtConversation()
                    {
                        TITLE = item.CONV_TITLE,
                        TRANSLATE = item.CONV_TRANS,
                        conversationDate = item.CONV_DT
                    };
                }
            }
            SetConversation(udtResult);
        }

        private void SetConversation(UdtConversation udtConversation)
        {
            tbEnglish.Text = udtConversation.TITLE;
            tbKorean.Text = udtConversation.TRANSLATE;
        }

        private void LbAllConvList_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;

            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));
            //    parent.Items.Refresh();

            if (null == data)
            {
                return;
            }

            List<OTCSCONV> arrUdtConversation = mExcuteQuery.SelectDateConvData("");

            foreach (var item in arrUdtConversation)
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
            UdtConversation udtConversation = new UdtConversation();
            foreach (var item in arrConversationLists)
            {
                if (item.INDEX.Equals(((UdtReservList)data).INDEX))
                {
                    udtConversation = new UdtConversation()
                    {
                        TITLE = item.CONV_TITLE,
                        TRANSLATE = item.CONV_TRANS
                    };

                    arrIndex.Add(item.INDEX);

                    arrConversationLists.Remove(item);
                    break;
                }
            }

            // DB에서 지우기
            mExcuteQuery.DeleteConversationData(arrIndex);

            // 저장된 데이터 가져오기
            List<UdtReservList> arrBookData = GetData();
            // 가져온 데이터 세팅
            SetData(arrBookData);
        }

        /// <summary>
        /// 예약정보 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LbReserveList_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;

            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));
            //    parent.Items.Refresh();
            parent.Items.Remove(data);

            /// DB에서 데이터 지우기
            // 제거할 데이터 인덱스
            List<int> arrIndex = new List<int>();
            // 우클린 한 데이터 정보 가져오기
            UdtConversation udtResult = new UdtConversation();
            foreach (var item in arrConversationLists)
            {
                if (item.INDEX.Equals(((UdtReservList)data).INDEX))
                {
                    arrIndex.Add(item.INDEX);
                }
            }

            // DB에서 지우기
            mExcuteQuery.DeleteReservationData(Common.Constants.TABLE_NAME.CONVERSATION, arrIndex, DateTime.Now.ToString("yyyyMMdd"));

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

            List<UdtConversation> arrUdtConversation = new List<UdtConversation>();

            for (int i = 2; i < objExcelData.GetLength(0)+1; i++)
            {
                arrUdtConversation.Add(new UdtConversation()
                {
                    TITLE = objExcelData[i, 1].ToString(),
                    TRANSLATE = objExcelData[i, 2].ToString(),
                });
            }

            for (int i = 0; i < arrUdtConversation.Count; i++)
            {
                mExcuteQuery.InsertConvData(arrUdtConversation[i]);
            }

            // 다시 불러오기
            SetData(GetData());
        }
    }
}
