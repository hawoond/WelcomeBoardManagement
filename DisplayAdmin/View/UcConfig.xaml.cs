using DisplayAdmin.Model;
using DisplayAdmin.Query;
using DisplayAdmin.Table;
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
    /// UcConfig.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UcConfig : UserControl
    {
        private ExcuteQuery mExcuteQuery;
        List<UdtReservList> arrScreen;

        public UcConfig()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            mExcuteQuery = ExcuteQuery.GetInstance();
            // 저장된 데이터 가져오기
            OTADCONF udtConfig = new OTADCONF();
            udtConfig = mExcuteQuery.SelectConfigData();

            if(null != udtConfig)
            {
                tbSchoolNM.Text = udtConfig.SCH_NM;
                tbInterval.Text = udtConfig.SCR_INTERVAL.ToString();
            }
            else
            {
                mExcuteQuery.InsertConfigData("기본고등학교", 6);
                tbSchoolNM.Text = "기본고등학교";
                tbInterval.Text = "6";
            }

            arrScreen = new List<UdtReservList>();
            arrScreen.Add(new UdtReservList { INDEX = (int)Common.Constants.ScreenIndex.PROMOTION_MOVIE, DESC = Properties.Resources.PROMOTION_MOVIE });
            arrScreen.Add(new UdtReservList { INDEX = (int)Common.Constants.ScreenIndex.WEATHER, DESC = Properties.Resources.WEATHER });
            arrScreen.Add(new UdtReservList { INDEX = (int)Common.Constants.ScreenIndex.CONVERSATION, DESC = Properties.Resources.CONVERSATION });
            arrScreen.Add(new UdtReservList { INDEX = (int)Common.Constants.ScreenIndex.BOOK, DESC = Properties.Resources.BOOK });
            arrScreen.Add(new UdtReservList { INDEX = (int)Common.Constants.ScreenIndex.JOB, DESC = Properties.Resources.JOB });
            arrScreen.Add(new UdtReservList { INDEX = (int)Common.Constants.ScreenIndex.CELEBRATION, DESC = Properties.Resources.CELEBRATION });
            arrScreen.Add(new UdtReservList { INDEX = (int)Common.Constants.ScreenIndex.WELCOME_MESSAGE, DESC = Properties.Resources.WELCOME_MESSAGE });
            arrScreen.Add(new UdtReservList { INDEX = (int)Common.Constants.ScreenIndex.TEACHERS_DAY, DESC = Properties.Resources.TEACHERS_DAY });
            arrScreen.Add(new UdtReservList { INDEX = (int)Common.Constants.ScreenIndex.SAFTY_INFO, DESC = Properties.Resources.SAFTY_INFO });
            arrScreen.Add(new UdtReservList { INDEX = (int)Common.Constants.ScreenIndex.FOOD_MENU, DESC = Properties.Resources.FOOD_MENU });

            lbAllScreenList.ItemsSource = arrScreen;

            tbSharedFolder.Text = Properties.Settings.Default.ROOT_FILE_PATH;

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

        private void CdrReserveDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            lbReserveList.Items.Clear();
            OTADSCFL ScreenFlow = mExcuteQuery.SelectScreenFlowData(((DateTime)((Calendar)sender).SelectedDate).ToString("yyyyMMdd"));

            if(null == ScreenFlow)
            {
                return;
            }

            for (int i = 0; i < 10; ++i)
            {
                if(ScreenFlow.PROM_SEQ == i.ToString())
                {
                    lbReserveList.Items.Add(new UdtReservList { INDEX = (int)Common.Constants.ScreenIndex.PROMOTION_MOVIE, DESC = Properties.Resources.PROMOTION_MOVIE });
                }
                else if(ScreenFlow.WETH_SEQ == i.ToString())
                {
                    lbReserveList.Items.Add(new UdtReservList { INDEX = (int)Common.Constants.ScreenIndex.WEATHER, DESC = Properties.Resources.WEATHER });
                }
                else if (ScreenFlow.CONV_SEQ == i.ToString())
                {
                    lbReserveList.Items.Add(new UdtReservList { INDEX = (int)Common.Constants.ScreenIndex.CONVERSATION, DESC = Properties.Resources.CONVERSATION });
                }
                else if (ScreenFlow.BOOK_SEQ == i.ToString())
                {
                    lbReserveList.Items.Add(new UdtReservList { INDEX = (int)Common.Constants.ScreenIndex.BOOK, DESC = Properties.Resources.BOOK });
                }
                else if (ScreenFlow.JOB_SEQ == i.ToString())
                {
                    lbReserveList.Items.Add(new UdtReservList { INDEX = (int)Common.Constants.ScreenIndex.JOB, DESC = Properties.Resources.JOB });
                }
                else if (ScreenFlow.CELE_SEQ == i.ToString())
                {
                    lbReserveList.Items.Add(new UdtReservList { INDEX = (int)Common.Constants.ScreenIndex.CELEBRATION, DESC = Properties.Resources.CELEBRATION });
                }
                else if (ScreenFlow.SAFE_SEQ == i.ToString())
                {
                    lbReserveList.Items.Add(new UdtReservList { INDEX = (int)Common.Constants.ScreenIndex.SAFTY_INFO, DESC = Properties.Resources.SAFTY_INFO });
                }
                else if (ScreenFlow.MENU_SEQ == i.ToString())
                {
                    lbReserveList.Items.Add(new UdtReservList { INDEX = (int)Common.Constants.ScreenIndex.FOOD_MENU, DESC = Properties.Resources.FOOD_MENU });
                }
                else if (ScreenFlow.TCDY_SEQ == i.ToString())
                {
                    lbReserveList.Items.Add(new UdtReservList { INDEX = (int)Common.Constants.ScreenIndex.TEACHERS_DAY, DESC = Properties.Resources.TEACHERS_DAY });
                }
                else if (ScreenFlow.WELC_SEQ == i.ToString())
                {
                    lbReserveList.Items.Add(new UdtReservList { INDEX = (int)Common.Constants.ScreenIndex.WELCOME_MESSAGE, DESC = Properties.Resources.WELCOME_MESSAGE });
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(tbSchoolNM.Text) || string.IsNullOrEmpty(tbInterval.Text))
            {
                MessageBox.Show("내용을 입력해 주세요");
            }

            int nInterval = 0;

            if(int.TryParse(tbInterval.Text, out nInterval))
            {
                mExcuteQuery.InsertConfigData(tbSchoolNM.Text, nInterval);
                SaveConfigJson();
            }
            else
            {
                MessageBox.Show("유지시간엔 숫자만 입력해 주세요");
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
        private void LbAllScreenList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

            object data = e.Data.GetData(typeof(UdtReservList));
            //((IList)dragSource.ItemsSource).Remove(data);

            foreach(var item in parent.Items)
            {
                if (((UdtReservList)item).INDEX.Equals(((UdtReservList)data).INDEX))
                {
                    MessageBox.Show("화면은 중복될 수 없습니다.");
                    return;
                }
            }

            parent.Items.Add(data);
            List<UdtReservList> arrIndex = new List<UdtReservList>();
            UdtReservList udtResult = new UdtReservList();
            foreach (var item in arrScreen)
            {
                if (item.INDEX.Equals(((UdtReservList)data).INDEX))
                {
                    udtResult = new UdtReservList()
                    {
                        INDEX = item.INDEX,
                        DESC = item.DESC
                    };
                }
            }

            foreach(var item in parent.Items)
            {
                arrIndex.Add((UdtReservList)item);
            }


            //예약 정보 DB에 저장
            mExcuteQuery.InsertScreenSeq(arrIndex, ((DateTime)cdrReserveDate.SelectedDate).ToString("yyyyMMdd"));

            SaveJson();
        }

        /// <summary>
        /// JSON 만들긔
        /// </summary>
        public void SaveJson()
        {
            OTADSCFL TodayData = mExcuteQuery.SelectScreenFlowData(DateTime.Now.ToString("yyyyMMdd"));
            List<object> arrScreenFlow = new List<object>();

            UdtScreenFlow screenFlow = new UdtScreenFlow()
            {
                BOOK_SEQ = TodayData.BOOK_SEQ,
                CELE_SEQ = TodayData.CELE_SEQ,
                CONV_SEQ = TodayData.CONV_SEQ,
                JOB_SEQ = TodayData.JOB_SEQ,
                MENU_SEQ = TodayData.MENU_SEQ,
                PROM_SEQ = TodayData.PROM_SEQ,
                SAFE_SEQ = TodayData.SAFE_SEQ,
                TCDY_SEQ = TodayData.TCDY_SEQ,
                WELC_SEQ = TodayData.WELC_SEQ,
                WETH_SEQ = TodayData.WETH_SEQ
            };

            arrScreenFlow.Add(screenFlow);

            // 예약 정보 xml에 저장 -> 타이머로 하지 않을까?
            Common.StaticUtils.SaveJsonFile(Common.Constants.FILE_KEY.SCREEN_FLOW, arrScreenFlow, Common.Constants.CelebrationCode.NONE);
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
            List<UdtReservList> arrIndex = new List<UdtReservList>();

            foreach (var item in parent.Items)
            {
                arrIndex.Add((UdtReservList)item);
            }

            //예약 정보 DB에 저장
            mExcuteQuery.InsertScreenSeq(arrIndex, ((DateTime)cdrReserveDate.SelectedDate).ToString("yyyyMMdd"));
            SaveJson();
        }

        public void SaveConfigJson()
        {
            OTADCONF ConfigData = mExcuteQuery.SelectConfigData();
            List<object> arrConfig = new List<object>();

            UdtConfig udtConfig = new UdtConfig()
            {
                SCH_NM = ConfigData.SCH_NM,
                SCR_INTERVAL = ConfigData.SCR_INTERVAL 
            };

            arrConfig.Add(udtConfig);

            // 예약 정보 xml에 저장 -> 타이머로 하지 않을까?
            Common.StaticUtils.SaveJsonFile(Common.Constants.FILE_KEY.CONFIG, arrConfig, Common.Constants.CelebrationCode.NONE);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string sRoot = tbSharedFolder.Text;

            if (sRoot.Equals(string.Empty))
            {
                MessageBox.Show("내용을 입력해 주세요");
            }
            else
            {
                Properties.Settings.Default.ROOT_FILE_PATH = sRoot;
                Properties.Settings.Default.Save();
            }
        }
    }
}
