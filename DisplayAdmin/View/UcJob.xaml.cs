using DisplayAdmin.Model;
using DisplayAdmin.Query;
using DisplayAdmin.Table;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
    /// UcJob.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UcJob : UserControl
    {
        private ExcuteQuery mExcuteQuery;
        private List<OCTSJOB> arrJobList;
        private List<UdtJob> BindingData;
        private OpenFileDialog mOpenFileDialog;
        public UcJob()
        {
            InitializeComponent();
            this.DataContext = this;
            Init();
        }

        public void Init()
        {
            mOpenFileDialog = new OpenFileDialog();
            mOpenFileDialog.Filter = "이미지(*.png) | *.png";

            mExcuteQuery = ExcuteQuery.GetInstance();
            // 저장된 데이터 가져오기
            List<UdtReservList> arrJobData = GetData();

            // 가져온 데이터 세팅
            if (arrJobData.Count > 0)
            {
                SetData(arrJobData);
            }
            else
            {
                //데이터 없을 때 API에서 데이터 불러와서 넣어줌
                InsertApiData();
            }

            cdrReserveDate.SelectedDate = DateTime.Today;
            CdrReserveDate_SelectedDatesChanged(cdrReserveDate, null);
        }

        public void InsertApiData()
        {
            try
            {
                IMRUtils.CallRestFul callRestFul = new IMRUtils.CallRestFul();
                List<IMRUtils.Model.UdtJob> arrJob = callRestFul.CallJobApi();

                mExcuteQuery.InsertJobData(arrJob);
                SetData(GetData());
            }
            catch
            {


            }
        }

        public List<UdtReservList> GetData()
        {
            bool isResult = false;
            // 저장된 리스트 전체 조회
            this.arrJobList = mExcuteQuery.SelectJobData();
            List<UdtReservList> arrJobList = new List<UdtReservList>();

            // 직업명 리스트에 담기
            foreach (var itemJob in this.arrJobList)
            {
                arrJobList.Add(new UdtReservList { DESC = itemJob.JOB_NM, INDEX = itemJob.INDEX });
            }

            return arrJobList;
        }

        public bool SetData(List<UdtReservList> arrJobList)
        {
            bool isResult = false;
            try
            {
                lbAllJobList.ItemsSource = arrJobList;
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
        private void LbAllJobList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
            UdtJob udtResult = new UdtJob();
            foreach (var item in arrJobList)
            {
                if (item.INDEX.Equals(((UdtReservList)data).INDEX))
                {
                    udtResult = new UdtJob()
                    {
                        TITLE = item.JOB_NM,
                        INFO = item.JOB_DESC,
                        jobDate = item.JOB_DT,
                        IMAGE = item.IMG_PATH
                    };

                    arrIndex.Add(item.INDEX);
                }
            }

            // 예약 정보 DB에 저장
            if (null == cdrReserveDate.SelectedDate)
            {
                mExcuteQuery.InsertReservationData(Common.Constants.TABLE_NAME.JOB, arrIndex, DateTime.Now.ToString("yyyyMMdd"));
            }
            else
            {
                mExcuteQuery.InsertReservationData(Common.Constants.TABLE_NAME.JOB, arrIndex, ((DateTime)cdrReserveDate.SelectedDate).ToString("yyyyMMdd"));
            }

            SaveJson();
        }

        /// <summary>
        /// JSON 만들긔
        /// </summary>
        public void SaveJson()
        {
            List<object> arrTempResult = new List<object>();
            List<UdtJob> arrUdtJob = new List<UdtJob>();
            List<OCTSJOB> arrTodayData = mExcuteQuery.SelectDateJobData(DateTime.Now.ToString("yyyyMMdd"));

            if (arrTodayData.Count == 0)
            {
                arrUdtJob.Clear();
                List<OCTSJOB> arrTableJob = mExcuteQuery.SelectJobData();

                // 데이터 한개도 없을땐 저장 안하기
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

            // 책 예약 정보 xml에 저장 -> 타이머로 하지 않을까?
            Common.StaticUtils.SaveJsonFile(Common.Constants.FILE_KEY.JOB, arrTempResult, Common.Constants.CelebrationCode.NONE);
        }

        private void LbAllJobList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UdtJob udtResult = new UdtJob();
            foreach (var item in arrJobList)
            {
                if (((UdtReservList)((ListBox)sender).SelectedItem).INDEX.Equals(item.INDEX))
                {
                    Byte[] buffer;
                    if (item.IMG_PATH.Length > 0)
                    {
                        if (item.IMG_PATH.Substring(0, 4).Equals("http"))
                        {
                            WebClient wc = new WebClient();
                            buffer = wc.DownloadData(new Uri(item.IMG_PATH, UriKind.Absolute));
                            wc.Dispose();
                        }
                        else
                        {
                            buffer = System.IO.File.ReadAllBytes(item.IMG_PATH);
                        }

                        MemoryStream ms = new MemoryStream(buffer);
                        BitmapImage img = new BitmapImage();

                        img.BeginInit();
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.StreamSource = ms;
                        img.EndInit();

                        imgPreview.Source = img;
                    }

                    udtResult = new UdtJob()
                    {
                        TITLE = item.JOB_NM,
                        IMAGE = item.IMG_PATH,
                        INFO = item.JOB_DESC,
                        jobDate = item.JOB_DT
                    };
                }
            }
            SetConversation(udtResult);
        }

        private void SetConversation(UdtJob udtJob)
        {
            tbJobTitle.Text = udtJob.TITLE;
            rtbJobDesc.Document.Blocks.Clear();
            rtbJobDesc.AppendText(udtJob.INFO);
            //이미지 넣어야됨
        }

        private void CdrReserveDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            lbReserveList.Items.Clear();
            List<OCTSJOB> arrUdtJob = mExcuteQuery.SelectDateJobData(((DateTime)((Calendar)sender).SelectedDate).ToString("yyyyMMdd"));
            foreach (OCTSJOB udtJob in arrUdtJob)
            {
                lbReserveList.Items.Add(new UdtReservList { DESC = udtJob.JOB_NM, INDEX = udtJob.INDEX });
            }
        }

        private void Button_Click_Table_Insert(object sender, RoutedEventArgs e)
        {
            string sbJobName = string.Empty;
            sbJobName = tbJobTitle.Text;

            if(sbJobName.Equals(string.Empty)|| sbJobName.Trim().Length == 0)
            {
                MessageBox.Show("직업 이름을 입력해주세요.");
                return;
            }

            if (tbJobTitle.Text.Contains(":") || tbJobTitle.Text.Contains("?"))
            {
                sbJobName = tbJobTitle.Text.Replace(":", "-").Replace("?", "");
            }

            string sImagePath = Properties.Settings.Default.ROOT_FILE_PATH + Properties.Settings.Default.IMAGE_FILE_PATH + sbJobName + ".png";

            try
            {
                Common.StaticUtils.SaveToPng(imgPreview.Source, sImagePath);
            }
            catch (Exception ex)
            {
                sImagePath = Properties.Settings.Default.ROOT_FILE_PATH + Properties.Settings.Default.IMAGE_FILE_PATH + "job_imagesource_imagepreparing.png";
            }

            UdtJob udtJob = new UdtJob()
            {
                TITLE = tbJobTitle.Text,
                INFO = Common.StaticUtils.StringFromRichTextBox(rtbJobDesc),
                //IMAGE = Properties.Settings.Default.IMAGE_FILE_PATH + "job_imagesource_imagepreparing.png"
                IMAGE = sImagePath
            };

            mExcuteQuery.InsertJobData(udtJob);
            // 다시 불러오기
            SetData(GetData());
        }

        private void LbAllJobList_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;

            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));
            //    parent.Items.Refresh();

            if(null == data)
            {
                return;
            }

            List<OCTSJOB> arrUdtJob = mExcuteQuery.SelectDateJobData("");

            foreach (var item in arrUdtJob)
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
            UdtJob udtJob = new UdtJob();
            foreach (var item in arrJobList)
            {
                if (item.INDEX.Equals(((UdtReservList)data).INDEX))
                {
                    udtJob = new UdtJob()
                    {
                        TITLE = item.JOB_NM,
                        INFO = item.JOB_DESC,
                        IMAGE = item.IMG_PATH,
                        jobDate = item.JOB_DT
                    };

                    arrIndex.Add(item.INDEX);

                    arrJobList.Remove(item);
                    break;
                }
            }

            // DB에서 지우기
            mExcuteQuery.DeleteJobData(arrIndex);

            // 저장된 데이터 가져오기
            List<UdtReservList> arrJobData = GetData();
            // 가져온 데이터 세팅
            SetData(arrJobData);
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
            UdtJob udtResult = new UdtJob();
            foreach (var item in arrJobList)
            {
                if (item.INDEX.Equals(((UdtReservList)data).INDEX))
                {
                    arrIndex.Add(item.INDEX);
                }
            }

            // DB에서 지우기
            mExcuteQuery.DeleteReservationData(Common.Constants.TABLE_NAME.JOB, arrIndex, DateTime.Now.ToString("yyyyMMdd"));

            // xml 데이터 다시 저장
            SaveJson();
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
