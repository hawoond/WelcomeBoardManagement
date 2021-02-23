using DisplayAdmin.Model;
using DisplayAdmin.Query;
using DisplayAdmin.Table;
using System;
using System.Collections.Generic;
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
    /// UcPromotionMovie.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UcPromotionMovie : UserControl
    {
        private ExcuteQuery mExcuteQuery;
        private List<OTCSPRMO> arrMovieList;
        private List<UdtPromotionMovie> BindingData;
        public UcPromotionMovie()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            ucOpenMovieDialog.SetOpenFileDialogFilter("동영상 파일(*.mp4)|*.mp4|동영상 파일(*.mpeg)|*.mpeg");

            mExcuteQuery = ExcuteQuery.GetInstance();

            List<UdtReservList> arrMovieData = GetData();
            SetData(arrMovieData);

            cdrReserveDate.SelectedDate = DateTime.Today;
            CdrReserveDate_SelectedDatesChanged(cdrReserveDate, null);
        }

        public List<UdtReservList> GetData()
        {
            bool isResult = false;
            // 저장된 리스트 전체 조회
            this.arrMovieList = mExcuteQuery.SelectMovieData();
            List<UdtReservList> arrMovieList = new List<UdtReservList>();

            // 직업명 리스트에 담기
            foreach (var itemMovie in this.arrMovieList)
            {
                arrMovieList.Add(new UdtReservList { DESC = itemMovie.MO_NM, INDEX = itemMovie.INDEX });
            }

            return arrMovieList;
        }

        public bool SetData(List<UdtReservList> arrMovieList)
        {
            bool isResult = false;
            try
            {
                lbAllMovieList.ItemsSource = arrMovieList;
                isResult = true;
            }
            catch
            {
                isResult = false;
            }
            return isResult;
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
        private void LbAllMovieList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

            if (parent.Items.Count == 3)
            {
                MessageBox.Show("동영상은 3개까지 등록 가능합니다.");
                return;
            }

            object data = e.Data.GetData(typeof(UdtReservList));
            //((IList)dragSource.ItemsSource).Remove(data);
            parent.Items.Add(data);
            List<int> arrIndex = new List<int>();
            // 드래그 해서 이동 한 책 이름으로 책 정보 가져오기
            UdtPromotionMovie udtResult = new UdtPromotionMovie();
            foreach (var item in arrMovieList)
            {
                if (item.INDEX.Equals(((UdtReservList)data).INDEX))
                {
                    udtResult = new UdtPromotionMovie()
                    {
                        movieName = item.MO_NM,
                        FILEPATH = item.MO_PATH
                    };

                    arrIndex.Add(item.INDEX);
                }
            }

            // 예약 정보 DB에 저장
            if (null == cdrReserveDate.SelectedDate)
            {
                mExcuteQuery.InsertReservationData(Common.Constants.TABLE_NAME.PROMOTION_MOVIE, arrIndex, DateTime.Now.ToString("yyyyMMdd"));
            }
            else
            {
                mExcuteQuery.InsertReservationData(Common.Constants.TABLE_NAME.PROMOTION_MOVIE, arrIndex, ((DateTime)cdrReserveDate.SelectedDate).ToString("yyyyMMdd"));
            }

            SaveJson();
        }

        /// <summary>
        /// JSON 만들긔
        /// </summary>
        public void SaveJson()
        {
            List<object> arrTempResult = new List<object>();
            List<UdtPromotionMovie> arrUdtMovie = new List<UdtPromotionMovie>();
            List<OTCSPRMO> arrTodayData = mExcuteQuery.SelectDateMovieData(DateTime.Now.ToString("yyyyMMdd"));

            if (arrTodayData.Count == 0)
            {
                //동영상은 안넣어주면 랜덤생성 안함
                //arrUdtMovie.Clear();
                //List<OTCSPRMO> arrTableMovie = mExcuteQuery.SelectMovieData();

                //// 데이터 한개도 없을땐 저장 안하기
                //if (arrTableMovie.Count == 0)
                //{
                //    return;
                //}

                //Random rndIndex = new Random();
                //int nMovieCount = 0;

                //if (arrTableMovie.Count > 3)
                //{

                //}

                //for (int i = 0; i < 3; i++)
                //{
                //    int nIndex = (rndIndex.Next(0, arrTableMovie.Count - 1));

                //    arrUdtMovie.Add(new UdtPromotionMovie()
                //    {
                //        movieName = arrTableMovie[nIndex].MO_NM,
                //        FILEPATH = arrTableMovie[nIndex].MO_PATH
                //    });
                //}
            }
            else
            {
                for (int i = 0; i < arrTodayData.Count; i++)
                {
                    arrUdtMovie.Add(new UdtPromotionMovie()
                    {
                        movieName = arrTodayData[i].MO_NM,
                        FILEPATH = arrTodayData[i].MO_PATH
                    });
                }
            }


            foreach (UdtPromotionMovie udtMovie in arrUdtMovie)
            {
                arrTempResult.Add(udtMovie);
            }

            // 책 예약 정보 xml에 저장 -> 타이머로 하지 않을까?
            Common.StaticUtils.SaveJsonFile(Common.Constants.FILE_KEY.PROMOTION_MOVIE, arrTempResult, Common.Constants.CelebrationCode.NONE);
        }

        private void CdrReserveDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            lbReserveList.Items.Clear();
            List<OTCSPRMO> arrUdtMovie = mExcuteQuery.SelectDateMovieData(((DateTime)((Calendar)sender).SelectedDate).ToString("yyyyMMdd"));
            foreach (OTCSPRMO udtMovie in arrUdtMovie)
            {
                lbReserveList.Items.Add(new UdtReservList { DESC = udtMovie.MO_NM, INDEX = udtMovie.INDEX });
            }
        }

        private void LbAllMovieList_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

            ListBox parent = (ListBox)sender;
            dragSource = parent;

            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));
            //    parent.Items.Refresh();

            if (null == data)
            {
                return;
            }

            List<OTCSPRMO> arrUdtMovie = mExcuteQuery.SelectDateMovieData("");

            foreach (var item in arrUdtMovie)
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
            UdtPromotionMovie udtMovie = new UdtPromotionMovie();
            foreach (var item in arrMovieList)
            {
                if (item.INDEX.Equals(((UdtReservList)data).INDEX))
                {
                    udtMovie = new UdtPromotionMovie()
                    {
                        movieName = item.MO_NM,
                        FILEPATH = item.MO_PATH
                    };

                    arrIndex.Add(item.INDEX);

                    arrMovieList.Remove(item);
                    break;
                }
            }

            // DB에서 지우기
            mExcuteQuery.DeleteMovieData(arrIndex);

            // 저장된 데이터 가져오기
            List<UdtReservList> arrMovieData = GetData();
            // 가져온 데이터 세팅
            SetData(arrMovieData);
        }

        /// <summary>
        /// 예약 정보 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            UdtPromotionMovie udtResult = new UdtPromotionMovie();
            foreach (var item in arrMovieList)
            {
                if (item.INDEX.Equals(((UdtReservList)data).INDEX))
                {
                    arrIndex.Add(item.INDEX);
                }
            }

            // DB에서 지우기
            mExcuteQuery.DeleteReservationData(Common.Constants.TABLE_NAME.PROMOTION_MOVIE, arrIndex, DateTime.Now.ToString("yyyyMMdd"));

            // xml 데이터 다시 저장
            SaveJson();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<string> arrMoviePath = ucOpenMovieDialog.GetMovieList();
            List<UdtPromotionMovie> arrInsertMovieList = new List<UdtPromotionMovie>();

            if (arrMoviePath.Count.Equals(0))
            {
                return;
            }

            //동영상 파일 공유폴더에 저장하기
            Byte[] buffer;
            foreach (var path in arrMoviePath)
            {
                string[] arrMovName = path.Split('\\');

                string sNewFilePath = Properties.Settings.Default.ROOT_FILE_PATH + Properties.Settings.Default.MOVIE_FILE_PATH + arrMovName[arrMovName.Length - 1];
                Common.StaticUtils.FileCopy(path, sNewFilePath);

                arrInsertMovieList.Add(new UdtPromotionMovie { movieName = arrMovName[arrMovName.Length - 1], FILEPATH = sNewFilePath });
            }

            foreach (var item in lbAllMovieList.Items)
            {
                if (((UdtReservList)item).DESC.Equals(arrInsertMovieList[0].movieName))
                {
                    MessageBox.Show("중복 데이터는 입력할 수 없습니다.");
                    return;
                }
            }

            mExcuteQuery.InsertPromotionMovieData(arrInsertMovieList);

            // 가져온 데이터 세팅
            SetData(GetData());
        }
    }
}
