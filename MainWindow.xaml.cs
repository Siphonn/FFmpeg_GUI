using FFmpeg_GUI.Properties;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace FFmpeg_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _windowName.Text = "Video Compiler " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            CheckForPreviousSettings();
        }

        /// <summary>
        /// Check if there are any settings save from a previous session.
        /// Set defaults if no previous settings found.
        /// </summary>
        private void CheckForPreviousSettings()
        {
            /// Load previus setting if 'ImageName' OR 'OutputName' is not null or empty
            if (!string.IsNullOrEmpty(Settings.Default.ImageName) || !string.IsNullOrEmpty(Settings.Default.OutputName))
            {
                _imgName.Text = Settings.Default.ImageName;
                _imgType.SelectedIndex = Settings.Default.ImageType;
                _startFrame.Text = Settings.Default.StartFrame;
                _fps.SelectedIndex = Settings.Default.FPS;
                _outName.Text = Settings.Default.OutputName;
                _outType.SelectedIndex = Settings.Default.OutputType;
            }
            else /// else load deafult settings
            {
                _imgName.Text = "";
                _imgType.SelectedIndex = 0;
                _startFrame.Text = "0000";
                _fps.SelectedIndex = 1;
                _outName.Text = "";
                _outType.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Run ffmeg compile string using current input settings.
        /// EXAMPLE: //ffmpeg.exe -framerate 30 -i InputImage.% 4d.jpeg -c:v libx264 -r 30 -pix_fmt yuv420p -crf 18 -preset slower _OutputVideo.mp4
        /// </summary>
        private void RunCompiler(object sender, RoutedEventArgs e)
        {
            string inputCommand;

            string inputFPS = " -framerate " + _fps.Text;
            string startFrame = " -start_number " + _startFrame.Text;
            string inputShotName = " -i " + _imgName.Text + "." + "%" + _startFrame.Text.Length + "d." + _imgType.Text;
            string encoding = " -c:v libx264";
            string outputFPS = " -r " + _fps.Text;
            string pixelFormat = " -pix_fmt yuv420p";
            string bitrate = " -crf 18";
            string preset = " -preset slower ";
            string outputVideoName = _outName.Text + "." + _outType.Text;

            /// '/k' will keep the window open and '/c' will close the window the command has finished
            /// Compile entered values to a single string.
            inputCommand = "/c ffmpeg.exe" +
                inputFPS +
                startFrame +
                inputShotName +
                encoding +
                outputFPS +
                pixelFormat +
                bitrate +
                preset +
                outputVideoName;

            Process.Start("CMD.exe", inputCommand);
            /// Is FPS output needed? (-r 30)
            //MessageBox.Show(inputCommand); ///Debug inputs 
        }

        /// <summary>
        /// Close application.
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Minimise application.
        /// </summary>
        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
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
        /// Only a number can be entered in the 'StartFrame' input
        /// </summary>
        private void ValidateStartFrameIsANumber(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        /// <summary>
        /// Save setting on Close
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            Settings.Default.ImageName = _imgName.Text;
            Settings.Default.ImageType = _imgType.SelectedIndex;
            Settings.Default.StartFrame = string.IsNullOrEmpty(_startFrame.Text) ? "0000" : _startFrame.Text;
            Settings.Default.FPS = _fps.SelectedIndex;
            Settings.Default.OutputName = _outName.Text;
            Settings.Default.OutputType = _outType.SelectedIndex;
            Settings.Default.Save();
        }
    }
}
