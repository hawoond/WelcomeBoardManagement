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

namespace DisplayAdmin.View.Control
{
    /// <summary>
    /// UcOpenFileDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UcOpenFileDialog : UserControl
    {
        private List<string> mArrFile;
        private OpenFileDialog mOpenFileDialog;
        private string mDefaultFilter;

        /// <summary>
        /// 생성자
        /// </summary>
        public UcOpenFileDialog()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// 초기화 함수
        /// </summary>
        private void Init()
        {
            mArrFile = new List<string>();
            mOpenFileDialog = new OpenFileDialog();
            mOpenFileDialog.Filter = mDefaultFilter;
            btnOpenFileDialog.Click += BtnOpenFileDialog_Click;
        }

        /// <summary>
        /// 필터 설정
        /// default : 동영상 파일(*.mp4)|*.mp4|동영상 파일(*.mpeg)|*.mpeg|모든 파일|*.*
        /// </summary>
        /// <param name="sFilter">파일명(*.확장자)|*.확장자</param>
        public void SetOpenFileDialogFilter(string sFilter)
        {
            if (sFilter.Equals("") || sFilter == null)
            {
                mOpenFileDialog.Filter = mDefaultFilter;
            }
            else
            {
                mOpenFileDialog.Filter = sFilter;
            }
        }

        /// <summary>
        /// OpenFileDialog 호출 이벤트
        /// 파일 경로 저장 로직 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenFileDialog_Click(object sender, RoutedEventArgs e)
        {


            if (null != mArrFile && mArrFile.Count == 3)
            {
                MessageBox.Show("선택 할 수 있는 영상 갯수를 초과했습니다.");
                return;
            }
            else
            {
                mOpenFileDialog.ShowDialog();

                if (mOpenFileDialog.FileNames.Length > 0)
                {
                    if (mArrFile.Count > 3)
                    {
                        MessageBox.Show("동영상은 세개 이하로 선택 가능합니다.");
                        return;
                    }
                    for (int i = 0; i < mOpenFileDialog.FileNames.Length; i++)
                    {
                        //if (arrFile.Count == 3)
                        //{
                        //    break;
                        //}
                        for (int j = 0; j < mArrFile.Count; j++)
                        {
                            if (mArrFile[j].Equals(mOpenFileDialog.FileNames[i].ToString()))
                            {
                                MessageBox.Show("이미 선택한 동영상입니다.");
                                return;
                            }
                        }

                        mArrFile.Add(mOpenFileDialog.FileNames[i].ToString());
                    }
                    AddFile(mArrFile);
                }
            }
        }

        /// <summary>
        /// 선택된 파일 경로를 List에 추가
        /// </summary>
        /// <param name="arrOpenFile"></param>
        private void AddFile(List<string> arrOpenFile)
        {
            StackPanel spItem = new StackPanel();
            TextBlock tbFile = new TextBlock();
            Button btnClear = new Button();
            string[] arrTempPath = arrOpenFile[arrOpenFile.Count - 1].Split(new string[] { "\\" }, StringSplitOptions.None);

            tbFile.Text = arrTempPath[arrTempPath.Length - 1];
            tbFile.Height = 30;
            tbFile.VerticalAlignment = VerticalAlignment.Center;
            tbFile.Foreground = new SolidColorBrush(Colors.Black);

            btnClear.Width = 20;
            btnClear.Height = 20;
            btnClear.Content = "X";
            btnClear.FontSize = 12;
            btnClear.Background = new SolidColorBrush(Colors.Transparent);
            btnClear.Foreground = new SolidColorBrush(Colors.Black);
            btnClear.Click += BtnClear_Click;

            spItem.Background = new SolidColorBrush(Colors.White);
            spItem.Orientation = Orientation.Horizontal;
            spItem.HorizontalAlignment = HorizontalAlignment.Left;

            spItem.Children.Add(tbFile);
            spItem.Children.Add(btnClear);

            this.spOpenFileBackView.Children.Add(spItem);

            //btnClear.Tag = spOpenFileBackView.Children.Count - 1;
            btnClear.Tag = spItem;
        }

        /// <summary>
        /// 제거 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            //this.spOpenFileBackView.Children.RemoveAt((int)(((Button)sender).Tag));
            this.spOpenFileBackView.Children.Remove((UIElement)((Button)sender).Tag);

            StackPanel spTempPanel = (StackPanel)(((Button)sender).Tag);

            for (int i = 0; i < spTempPanel.Children.Count; i++)
            {
                if (spTempPanel.Children[i].GetType().Name.Equals("TextBlock"))
                {
                    for (int j = 0; j < mArrFile.Count; j++)
                    {
                        if (mArrFile[j].Contains(((TextBlock)spTempPanel.Children[i]).Text))
                        {
                            mArrFile.RemoveAt(j);
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 파일 경로 리스트 반환 함수
        /// </summary>
        /// <returns></returns>
        public List<string> GetMovieList()
        {
            if (null == mArrFile)
            {
                mArrFile = new List<string>();
            }

            return mArrFile;
        }

        /// <summary>
        /// 바인딩용
        /// </summary>
        public static readonly DependencyProperty FilepathProperty =
        DependencyProperty.Register("Filepath", typeof(string),
        typeof(UcOpenFileDialog), new PropertyMetadata(null));

        public string Filepath
        {
            get
            {
                return (string)GetValue(FilepathProperty);
            }
            set
            {
                SetValue(FilepathProperty, value);
            }
        }
    }
}
