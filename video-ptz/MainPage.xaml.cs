using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Capture;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.System.Display;
using Windows.Graphics.Display;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace video_ptz
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        MediaCapture _mediaCapture;
        ObservableCollection<DeviceListItem> deviceList = new ObservableCollection<DeviceListItem>();
        DisplayRequest _displayRequest = new DisplayRequest();

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            this.comboBox.SelectionChanged += ComboBox_SelectionChanged;
            this.button_left.Click += Button_left_Click;
            this.button_right.Click += Button_right_Click;
        }


        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await initializeMedia();
        }


        private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ComboBox c = (ComboBox) sender;
            DeviceListItem d = (DeviceListItem) c.SelectedValue;

            await initializeCamera(d.deviceId);

        }



        private void Button_right_Click(object sender, RoutedEventArgs e)
        {
            moveRight();

            return;

        }

        private bool moveRight()
        {
            double pan, tilt, zoom;

            _mediaCapture.VideoDeviceController.Pan.TryGetValue(out pan);
            Debug.WriteLine("pan: {0}", pan);
            _mediaCapture.VideoDeviceController.Tilt.TryGetValue(out tilt);
            Debug.WriteLine("tilt: {0}", tilt);
            _mediaCapture.VideoDeviceController.Zoom.TryGetValue(out zoom);
            Debug.WriteLine("zoom: {0}", zoom);

            Debug.WriteLine("try to pan to: {0}", pan + 1000);

            var result = _mediaCapture.VideoDeviceController.Pan.TrySetValue(pan + 1000);
            Debug.WriteLine("pan right result: {0}", result);
            result = _mediaCapture.VideoDeviceController.Tilt.TrySetValue(tilt);
            Debug.WriteLine("tilt result: {0}", result);
            result = _mediaCapture.VideoDeviceController.Zoom.TrySetValue(zoom);
            Debug.WriteLine("zoom result: {0}", result);

            return true;

        }


        private void Button_left_Click(object sender, RoutedEventArgs e)
        {
            double pan, tilt, zoom;

            _mediaCapture.VideoDeviceController.Pan.TryGetValue(out pan);
            Debug.WriteLine("pan: {0}", pan);
            _mediaCapture.VideoDeviceController.Tilt.TryGetValue(out tilt);
            Debug.WriteLine("tilt: {0}", tilt);
            _mediaCapture.VideoDeviceController.Zoom.TryGetValue(out zoom);
            Debug.WriteLine("zoom: {0}", zoom);

            Debug.WriteLine("try to pan to: {0}", pan - 1000);

            var result = _mediaCapture.VideoDeviceController.Pan.TrySetValue(pan - 1000);
            Debug.WriteLine("pan left result: {0}", result);
            result = _mediaCapture.VideoDeviceController.Tilt.TrySetValue(tilt);
            Debug.WriteLine("tilt result: {0}", result);
            result = _mediaCapture.VideoDeviceController.Zoom.TrySetValue(zoom);
            Debug.WriteLine("zoom result: {0}", result);

            return;

        }



        public async Task<bool> initializeCamera(string deviceId)
        {

            var settings = new MediaCaptureInitializationSettings
            {
                VideoDeviceId = deviceId,
            };

            _mediaCapture = new MediaCapture();

            await _mediaCapture.InitializeAsync(settings);

            _displayRequest.RequestActive();
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape;

            PreviewControl.Source = _mediaCapture;
            await _mediaCapture.StartPreviewAsync();
            
            Debug.WriteLine("Pan supported: {0}", _mediaCapture.VideoDeviceController.Pan.Capabilities.Supported);
            Debug.WriteLine("Tilt supported: {0}", _mediaCapture.VideoDeviceController.Tilt.Capabilities.Supported);
            Debug.WriteLine("Zoom supported: {0}", _mediaCapture.VideoDeviceController.Zoom.Capabilities.Supported);

            double pan, tilt, zoom;
            var querySuccess = _mediaCapture.VideoDeviceController.Pan.TryGetValue(out pan);
            Debug.WriteLine("pan {0}", pan);
            Debug.WriteLine("querySuccess {0}", querySuccess);
            querySuccess = _mediaCapture.VideoDeviceController.Tilt.TryGetValue(out tilt);
            Debug.WriteLine("tilt {0}", tilt);
            Debug.WriteLine("querySuccess {0}", querySuccess);
            querySuccess = _mediaCapture.VideoDeviceController.Zoom.TryGetValue(out zoom);
            Debug.WriteLine("zoom {0}", zoom);
            Debug.WriteLine("querySuccess {0}", querySuccess);

            return true;

        }

        public async Task<string> initializeMedia()
        {
            DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

            foreach (var device in devices)
            {
                deviceList.Add(new DeviceListItem(device.Name, device.Id));
            }

            return "";

        }

    }

    class DeviceListItem
    {
        public string deviceId { get; set; }
        public string name { get; set; }
        public override string ToString()
        {
            return name;
        }
        public DeviceListItem(string n, string id)
        {
          name = n;
          deviceId = id;
        }
    }

}
