using System.Windows.Controls;
using FFmpeg_GUI.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;


namespace FFmpeg_GUI
{
    /// <summary>
    /// Interaction logic for Page1_Simple.xaml
    /// </summary>
    public partial class Page1_Simple : Page
    {
        private MainWindow mainWindow;

        public Page1_Simple()
        {
            InitializeComponent();
            Loaded += Page1_Loaded;
            Unloaded += Page1_Unloaded;
        }

        public Page1_Simple(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            Loaded += Page1_Loaded;
            Unloaded += Page1_Unloaded;
        }

        private void Page1_Loaded(object sender, RoutedEventArgs e)
        {
            ApplyPreviousSettings();
            Console.WriteLine("{0} is loaded", Title);
        }

        private void Page1_Unloaded(object sender, RoutedEventArgs e)
        {
            mainWindow.StoreValues(
                _imgName.Text, _startFrame.Text, _outName.Text,
                _imgType.SelectedIndex, _fps.SelectedIndex, _outType.SelectedIndex);

            Console.WriteLine("{0} is unloaded", Title);
        }

        private void ApplyPreviousSettings()
        {
            _imgName.Text = mainWindow.imgName;
            _imgType.SelectedIndex = mainWindow.imgType;
            _startFrame.Text = mainWindow.startFrame;
            _fps.SelectedIndex = mainWindow.fps;
            _outName.Text = mainWindow.outName;
            _outType.SelectedIndex = mainWindow.outType;
        }

        /// <summary>
        /// Only a number can be entered in the 'StartFrame' input
        /// </summary>
        private void ValidateStartFrameIsANumber(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        /// <summary>
        /// Run ffmeg compile string using current input settings.
        /// EXAMPLE: //ffmpeg.exe -framerate 30 -start_number 0000 -i image.%4d.jpeg -c:v libx264 -r 30 -pix_fmt yuv420p -crf 18 -preset slower video.mp4
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

            // TODO send strings to MainWindow
            mainWindow.StoreValues(
                _imgName.Text, _startFrame.Text, _outName.Text,
                _imgType.SelectedIndex, _fps.SelectedIndex, _outType.SelectedIndex);

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
    }
}
