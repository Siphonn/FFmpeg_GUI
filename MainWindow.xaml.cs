using FFmpeg_GUI.Properties;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace FFmpeg_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string imgName = "";
        public int imgType = 0;
        public string startFrame = "0000";
        public int fps = 1;
        public string outName = "";
        public int outType = 0;


        public MainWindow()
        {
            InitializeComponent();
            CheckForPreviousSettings();
            _windowName.Text = "Video Compiler " + Assembly.GetExecutingAssembly().GetName().Version.ToString();

            Loaded += LoadePage1; /// load Page1_Simple on startup
        }

        private void CheckForPreviousSettings()
        {
            /// Load previus setting if 'ImageName' OR 'OutputName' is not null or empty
            if (!string.IsNullOrWhiteSpace(Settings.Default.ImageName) || !string.IsNullOrWhiteSpace(Settings.Default.OutputName))
            {
                imgName = Settings.Default.ImageName;
                imgType = Settings.Default.ImageType;
                startFrame = Settings.Default.StartFrame;
                fps = Settings.Default.FPS;
                outName = Settings.Default.OutputName;
                outType = Settings.Default.OutputType;
            }
            else /// else load deafult settings
            {
                imgName = "";
                imgType = 0;
                startFrame = "0000";
                fps = 2;
                outName = "";
                outType = 0;
            }
        }

        private void LoadePage1(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new Page1_Simple(this));
        }

        private void ModeSelection_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (frame == null) { return; }

            switch (modeSelection.SelectedIndex)
            {
                case 0:
                    frame.NavigationService.Navigate(new Page1_Simple(this));
                    break;
                case 1:
                    frame.NavigationService.Navigate(new Page2_Advanced(this));
                    break;
            }
        }

        internal void StoreValues(string _imgName, string _startFrame, string _outName, int _imgType, int _fps, int _outType)
        {
            imgName = _imgName;
            imgType = _imgType;
            startFrame = _startFrame;
            fps = _fps;
            outName = _outName;
            outType = _outType;
        }

        /// <summary>
        /// Move application window on left click.
        /// </summary>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        /// <summary>
        /// Minimise application.
        /// </summary>
        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Close application.
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Save setting on Close
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            // MessageBox.Show("Values are not saved"); /// Debug
            
            Type page = frame.Content.GetType();
            if (page == typeof(Page1_Simple))
            {
                //MessageBox.Show("Page 1 was last page");

                Page1_Simple p1 = (Page1_Simple)frame.Content;

                Settings.Default.ImageName = p1._imgName.Text;
                Settings.Default.ImageType = p1._imgType.SelectedIndex;
                Settings.Default.StartFrame = string.IsNullOrEmpty(p1._startFrame.Text) ? "0000" : p1._startFrame.Text;
                Settings.Default.FPS = p1._fps.SelectedIndex;
                Settings.Default.OutputName = p1._outName.Text;
                Settings.Default.OutputType = p1._outType.SelectedIndex;
            }
            else if (page == typeof(Page2_Advanced))
            {
                //MessageBox.Show("Page 2 was last page");

                Page2_Advanced p2 = (Page2_Advanced)frame.Content;

                Settings.Default.ImageName = p2._imgName.Text;
                Settings.Default.ImageType = p2._imgType.SelectedIndex;
                Settings.Default.StartFrame = string.IsNullOrEmpty(p2._startFrame.Text) ? "0000" : p2._startFrame.Text;
                Settings.Default.FPS = p2._fps.SelectedIndex;
                Settings.Default.OutputName = p2._outName.Text;
                Settings.Default.OutputType = p2._outType.SelectedIndex;
            }


            //Settings.Default.ImageName = imgName;
            //Settings.Default.ImageType = imgType;
            //Settings.Default.StartFrame = string.IsNullOrEmpty(startFrame) ? "0000" : startFrame;
            //Settings.Default.FPS = fps;
            //Settings.Default.OutputName = outName;
            //Settings.Default.OutputType = outType;

            Settings.Default.Save();
        }
    }
}