using DisplayAdmin.Model;
using DisplayAdmin.Query;
using DisplayAdmin.Table;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DisplayAdmin.View
{
    /// <summary>
    /// UcBook.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UcBook : UserControl
    {
        private ExcuteQuery mExcuteQuery;
        private List<UdtBook> BindingData;
        private List<OTCSBOOK> arrBookLists;
        private OpenFileDialog mOpenFileDialog;
        public UcBook()
        {
            InitializeComponent();
            Init();

        }

        public void Init()
        {
            mOpenFileDialog = new OpenFileDialog();
            mOpenFileDialog.Filter = "이미지(*.png) | *.png";

            mExcuteQuery = ExcuteQuery.GetInstance();
            // 저장된 데이터 가져오기
            List<UdtReservList> arrBookData = GetData();
            // 가져온 데이터 세팅
            SetData(arrBookData);

            cdrReserveDate.SelectedDate = DateTime.Today;
            CdrReserveDate_SelectedDatesChanged(cdrReserveDate, null);
        }

        /// <summary>
        /// 저장된 데이터 가져오기
        /// </summary>
        /// <returns></returns>
        public List<UdtReservList> GetData()
        {
            bool isResult = false;
            // 저장된 책 리스트 전체 조회
            arrBookLists = mExcuteQuery.SelectBookData();

            List<UdtReservList> arrBookList = new List<UdtReservList>();

            // 책 이름만 리스트에 담기
            foreach (var itemBook in arrBookLists)
            {
                arrBookList.Add(new UdtReservList { DESC = itemBook.BOOK_NM, INDEX = itemBook.INDEX });
            }

            return arrBookList;
        }

        /// <summary>
        /// 가져온 데이터 화면에 표시
        /// </summary>
        /// <param name="arrBookList"></param>
        /// <returns></returns>
        public bool SetData(List<UdtReservList> arrBookList)
        {
            bool isResult = false;
            try
            {
                lbAllBookList.ItemsSource = arrBookList;
                isResult = true;
            }
            catch
            {
                isResult = false;
            }
            return isResult;
        }

        /// <summary>
        /// 데이터 저장
        /// </summary>
        /// <param name="udtTempData"></param>
        /// <returns></returns>
        public bool SaveData(UdtBook udtTempData)
        {
            bool isSuccess = false;
            List<object> arrTempData = new List<object>();
            arrTempData.Add(udtTempData);
            isSuccess = Common.StaticUtils.SaveJsonFile(Common.Constants.FILE_KEY.BOOK, arrTempData, Common.Constants.CelebrationCode.NONE);

            if (!isSuccess)
            {
                return isSuccess;
            }

            try
            {
                isSuccess = mExcuteQuery.InsertBookData(udtTempData);
            }
            catch
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IMRUtils.CallRestFul callRestFul = new IMRUtils.CallRestFul();
                callRestFul.BOOK_NAME = tbBookSearch.Text;
                List<IMRUtils.Model.UdtBook> arrBook = callRestFul.CallBookApi(tbBookSearch.Text);

                lvBookList.ItemsSource = arrBook;
            }
            catch
            {


            }
        }
        private void LvBookList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                IMRUtils.Model.UdtBook udtBook = ((List<IMRUtils.Model.UdtBook>)lvBookList.ItemsSource)[((ListView)sender).SelectedIndex];

                Byte[] buffer;
                if (udtBook.ImgPath.Length > 0)
                {
                    if (udtBook.ImgPath.Substring(0, 4).Equals("http"))
                    {
                        WebClient wc = new WebClient();
                        buffer = wc.DownloadData(new Uri(udtBook.ImgPath, UriKind.Absolute));
                        wc.Dispose();
                    }
                    else
                    {
                        buffer = System.IO.File.ReadAllBytes(udtBook.ImgPath);
                    }
                    if (null == buffer)
                    {
                        imgPreview.Source = null;
                    }
                    MemoryStream ms = new MemoryStream(buffer);
                    BitmapImage img = new BitmapImage();

                    img.BeginInit();
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.StreamSource = ms;
                    img.EndInit();

                    imgPreview.Source = img;
                }
                else
                {
                    imgPreview.Source = null;
                }

                rtbBookDesc.Document.Blocks.Clear();
                tbBookTitle.Text = udtBook.BookName;
                tbAuth.Text = udtBook.BookAuth;
                rtbBookDesc.AppendText(udtBook.BookDesc);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 버튼 누르면 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Table_Insert(object sender, RoutedEventArgs e)
        {
            string sBookName = string.Empty;
            sBookName = tbBookTitle.Text;

            if(sBookName.Equals(string.Empty))
            {
                MessageBox.Show("책이름을 입력해주세요");
                return;
            }

            if (tbBookTitle.Text.Contains(":") || tbBookTitle.Text.Contains("?"))
            {
                sBookName = tbBookTitle.Text.Replace(":", "-").Replace("?", "");
            }

            string sImagePath = Properties.Settings.Default.ROOT_FILE_PATH + Properties.Settings.Default.IMAGE_FILE_PATH + sBookName + ".png";

            try
            {
                Common.StaticUtils.SaveToPng(imgPreview.Source, sImagePath);
            }
            catch (Exception ex)
            {
                sImagePath = string.Empty;
            }

            UdtBook udtBook = new UdtBook()
            {
                AUTH = tbAuth.Text,
                DESC = Common.StaticUtils.StringFromRichTextBox(rtbBookDesc),
                IMAGE = sImagePath,
                TITLE = tbBookTitle.Text
            };
            mExcuteQuery.InsertBookData(udtBook);
            // 다시 불러오기
            SetData(GetData());
        }

        private void SetBook(IMRUtils.Model.UdtBook udtBook)
        {
            Byte[] buffer;
            if (udtBook.ImgPath.Length > 0)
            {
                if (udtBook.ImgPath.Substring(0, 4).Equals("http"))
                {
                    WebClient wc = new WebClient();
                    buffer = wc.DownloadData(new Uri(udtBook.ImgPath, UriKind.Absolute));
                    wc.Dispose();
                }
                else
                {
                    buffer = System.IO.File.ReadAllBytes(udtBook.ImgPath);
                }

                MemoryStream ms = new MemoryStream(buffer);
                BitmapImage img = new BitmapImage();

                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.StreamSource = ms;
                img.EndInit();

                imgPreview.Source = img;
            }

            rtbBookDesc.Document.Blocks.Clear();
            tbBookTitle.Text = udtBook.BookName;
            tbAuth.Text = udtBook.BookAuth;
            rtbBookDesc.AppendText(udtBook.BookDesc);
        }

        private void LbAllBookList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                IMRUtils.Model.UdtBook udtResult = new IMRUtils.Model.UdtBook();
                foreach (var item in arrBookLists)
                {
                    if (((UdtReservList)((ListBox)sender).SelectedItem).INDEX.Equals(item.INDEX))
                    {
                        udtResult = new IMRUtils.Model.UdtBook()
                        {
                            BookAuth = item.BOOK_AUTH,
                            BookDesc = item.BOOK_DESC,
                            BookName = item.BOOK_NM,
                            ImgPath = item.IMG_PATH

                        };
                    }
                }

                SetBook(udtResult);
            }
            catch
            {

            }
        }

        ListBox dragSource = null;
        private void LbAllBookList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;

            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
            }
        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            ListBox parent = (ListBox)sender;

            if (parent.Items.Count == 3)
            {
                MessageBox.Show("책 정보는 3개 까지 등록 가능합니다.");
                return;
            }

            object data = e.Data.GetData(typeof(UdtReservList));
            //((IList)dragSource.ItemsSource).Remove(data);
            parent.Items.Add(data);
            List<int> arrIndex = new List<int>();
            // 드래그 해서 이동 한 책 이름으로 책 정보 가져오기
            IMRUtils.Model.UdtBook udtResult = new IMRUtils.Model.UdtBook();
            foreach (var item in arrBookLists)
            {

                if (item.INDEX.Equals(((UdtReservList)data).INDEX))
                {
                    udtResult = new IMRUtils.Model.UdtBook()
                    {
                        BookAuth = item.BOOK_AUTH,
                        BookDesc = item.BOOK_DESC,
                        BookName = item.BOOK_NM,
                        ImgPath = item.IMG_PATH
                    };

                    arrIndex.Add(item.INDEX);
                }
            }

            // 책 예약 정보 DB에 저장
            //udtResult.BookAuth;
            if (null == cdrReserveDate.SelectedDate)
            {
                mExcuteQuery.InsertReservationData(Common.Constants.TABLE_NAME.BOOK, arrIndex, DateTime.Now.ToString("yyyyMMdd"));
            }
            else
            {
                mExcuteQuery.InsertReservationData(Common.Constants.TABLE_NAME.BOOK, arrIndex, ((DateTime)cdrReserveDate.SelectedDate).ToString("yyyyMMdd"));
            }

            SaveJson();
        }

        /// <summary>
        /// Drag&Drop 관련 함수
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

        /// <summary>
        /// 선택 한 날짜의 예약 데이터 불러오기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CdrReserveDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            lbReserveList.Items.Clear();
            List<OTCSBOOK> arrUdtBook = mExcuteQuery.SelectDateBookData(((DateTime)((Calendar)sender).SelectedDate).ToString("yyyyMMdd"));
            foreach (OTCSBOOK udtBook in arrUdtBook)
            {
                lbReserveList.Items.Add(new UdtReservList { DESC = udtBook.BOOK_NM, INDEX = udtBook.INDEX });
            }
        }

        /// <summary>
        /// JSON 데이터 저장
        /// </summary>
        public void SaveJson()
        {
            List<object> arrTempResult = new List<object>();
            List<UdtBook> arrUdtBook = new List<UdtBook>();
            List<OTCSBOOK> arrTodayData = mExcuteQuery.SelectDateBookData(DateTime.Now.ToString("yyyyMMdd"));

            //데이터 없을 때 랜덤값 넣기
            if (arrTodayData.Count == 0)
            {
                arrUdtBook.Clear();
                List<OTCSBOOK> arrTableBook = mExcuteQuery.SelectBookData();

                // 데이터 한개도 없을땐 저장 안하기
                if(arrTableBook.Count == 0)
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
                for (int i = 0; i < arrTodayData.Count; i++)
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

        private void LbReserveList_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;

            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

            //데이터 없을땐 리턴
            if(data == null)
            {
                return;
            }

            parent.Items.Remove(data);

            /// DB에서 데이터 지우기
            // 제거할 데이터 인덱스
            List<int> arrIndex = new List<int>();
            // 우클린 한 데이터 정보 가져오기
            IMRUtils.Model.UdtBook udtResult = new IMRUtils.Model.UdtBook();
            foreach (var item in arrBookLists)
            {

                if (item.INDEX.Equals(((UdtReservList)data).INDEX))
                {
                    udtResult = new IMRUtils.Model.UdtBook()
                    {
                        BookAuth = item.BOOK_AUTH,
                        BookDesc = item.BOOK_DESC,
                        BookName = item.BOOK_NM,
                        ImgPath = item.IMG_PATH
                    };

                    arrIndex.Add(item.INDEX);
                }
            }

            // DB에서 지우기
            mExcuteQuery.DeleteReservationData(Common.Constants.TABLE_NAME.BOOK, arrIndex, DateTime.Now.ToString("yyyyMMdd"));

            // xml 데이터 다시 저장
            SaveJson();
        }

        private void LbAllBookList_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;

            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));
            //    parent.Items.Refresh();

            if(null == data)
            {
                return;
            }

            List<OTCSBOOK> arrUdtBook = mExcuteQuery.SelectDateBookData("");

            foreach (var item in arrUdtBook)
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
            IMRUtils.Model.UdtBook udtResult = new IMRUtils.Model.UdtBook();
            foreach (var item in arrBookLists)
            {
                if (item.INDEX.Equals(((UdtReservList)data).INDEX))
                {
                    udtResult = new IMRUtils.Model.UdtBook()
                    {
                        BookAuth = item.BOOK_AUTH,
                        BookDesc = item.BOOK_DESC,
                        BookName = item.BOOK_NM,
                        ImgPath = item.IMG_PATH
                    };

                    arrIndex.Add(item.INDEX);

                    arrBookLists.Remove(item);
                    break;
                }
            }

            // DB에서 지우기
            mExcuteQuery.DeleteBookData(arrIndex);

            // 저장된 데이터 가져오기
            List<UdtReservList> arrBookData = GetData();
            // 가져온 데이터 세팅
            SetData(arrBookData);

        }

        /// <summary>
        /// OpenFileDialog 호출 이벤트
        /// 파일 경로 저장 로직 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenFileDialog_Click(object sender, RoutedEventArgs e)
        {
            Byte[] buffer;
            mOpenFileDialog.ShowDialog();
            string sImgPath = string.Empty;
            if (mOpenFileDialog.FileNames.Length > 0)
            {
                for (int i = 0; i < mOpenFileDialog.FileNames.Length; i++)
                {
                    mOpenFileDialog.FileNames[i].ToString();

                    sImgPath = mOpenFileDialog.FileNames[i].ToString();
                }
            }

            if (0 == sImgPath.Length)
            {
                return;
            }

            buffer = System.IO.File.ReadAllBytes(sImgPath);
            MemoryStream ms = new MemoryStream(buffer);
            BitmapImage img = new BitmapImage();

            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.StreamSource = ms;
            img.EndInit();

            imgPreview.Source = img;

            tbImgPath.Text = sImgPath;
        }
    }
}
