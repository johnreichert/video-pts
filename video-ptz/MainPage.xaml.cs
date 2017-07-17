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

        int pan_step = 20;
        int tilt_step = 20;
        int zoom_step = 20;


        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            this.comboBox.SelectionChanged += ComboBox_SelectionChanged;
            this.button_left.Click += Button_left_Click;
            this.button_right.Click += Button_right_Click;
            this.button_zoom_in.Click += Button_zoom_in_Click;
            this.button_zoom_out.Click += Button_zoom_out_Click;
            this.button_tilt_up.Click += Button_tilt_up_Click;
            this.button_tilt_down.Click += Button_tilt_down_Click;
            this.Unloaded += MainPage_Unloaded;
        }


        private async void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            await disposeMedia();
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

            Debug.WriteLine("try to pan to: {0}", pan + pan_step);

            var result = _mediaCapture.VideoDeviceController.Pan.TrySetValue(pan + pan_step);
            Debug.WriteLine("pan right result: {0}", result);

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

            Debug.WriteLine("try to pan to: {0}", pan - pan_step);

            var result = _mediaCapture.VideoDeviceController.Pan.TrySetValue(pan - pan_step);
            Debug.WriteLine("pan left result: {0}", result);

            return;

        }



        private void Button_zoom_in_Click(object sender, RoutedEventArgs e)
        {
            double pan, tilt, zoom;

            _mediaCapture.VideoDeviceController.Pan.TryGetValue(out pan);
            Debug.WriteLine("pan: {0}", pan);
            _mediaCapture.VideoDeviceController.Tilt.TryGetValue(out tilt);
            Debug.WriteLine("tilt: {0}", tilt);
            _mediaCapture.VideoDeviceController.Zoom.TryGetValue(out zoom);
            Debug.WriteLine("zoom: {0}", zoom);

            Debug.WriteLine("try to zoom to: {0}", zoom + zoom_step);

            var result = _mediaCapture.VideoDeviceController.Zoom.TrySetValue(zoom + zoom_step);
            Debug.WriteLine("zoom result: {0}", result);

            return;

        }


        private void Button_zoom_out_Click(object sender, RoutedEventArgs e)
        {
            double pan, tilt, zoom;

            _mediaCapture.VideoDeviceController.Pan.TryGetValue(out pan);
            Debug.WriteLine("pan: {0}", pan);
            _mediaCapture.VideoDeviceController.Tilt.TryGetValue(out tilt);
            Debug.WriteLine("tilt: {0}", tilt);
            _mediaCapture.VideoDeviceController.Zoom.TryGetValue(out zoom);
            Debug.WriteLine("zoom: {0}", zoom);

            Debug.WriteLine("try to zoom to: {0}", zoom - zoom_step);

            var result = _mediaCapture.VideoDeviceController.Zoom.TrySetValue(zoom - zoom_step);
            Debug.WriteLine("zoom result: {0}", result);

            return;

        }


        private void Button_tilt_up_Click(object sender, RoutedEventArgs e)
        {
            double pan, tilt, zoom;

            _mediaCapture.VideoDeviceController.Pan.TryGetValue(out pan);
            Debug.WriteLine("pan: {0}", pan);
            _mediaCapture.VideoDeviceController.Tilt.TryGetValue(out tilt);
            Debug.WriteLine("tilt: {0}", tilt);
            _mediaCapture.VideoDeviceController.Zoom.TryGetValue(out zoom);
            Debug.WriteLine("zoom: {0}", zoom);

            Debug.WriteLine("try to tilt to: {0}", tilt + tilt_step);

            var result = _mediaCapture.VideoDeviceController.Tilt.TrySetValue(tilt + tilt_step);
            Debug.WriteLine("tilt result: {0}", result);

            return;

        }


        private void Button_tilt_down_Click(object sender, RoutedEventArgs e)
        {
            double pan, tilt, zoom;

            _mediaCapture.VideoDeviceController.Pan.TryGetValue(out pan);
            Debug.WriteLine("pan: {0}", pan);
            _mediaCapture.VideoDeviceController.Tilt.TryGetValue(out tilt);
            Debug.WriteLine("tilt: {0}", tilt);
            _mediaCapture.VideoDeviceController.Zoom.TryGetValue(out zoom);
            Debug.WriteLine("zoom: {0}", zoom);

            Debug.WriteLine("try to tilt to: {0}", tilt - tilt_step);

            var result = _mediaCapture.VideoDeviceController.Tilt.TrySetValue(tilt - tilt_step);
            Debug.WriteLine("tilt result: {0}", result);

            return;

        }


        public async Task<bool> initializeCamera(string deviceId)
        {

            await disposeMedia();

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
            

            if (_mediaCapture.VideoDeviceController.Pan.Capabilities.Supported)
            {
                Debug.WriteLine("Pan supported: min={0}, max={1}",
                  _mediaCapture.VideoDeviceController.Pan.Capabilities.Min,
                  _mediaCapture.VideoDeviceController.Pan.Capabilities.Max);
                pan_step = (int)((_mediaCapture.VideoDeviceController.Pan.Capabilities.Max - _mediaCapture.VideoDeviceController.Pan.Capabilities.Min) / 10);
                Debug.WriteLine("Pan step: {0}", pan_step);
            }
            else
            {
                Debug.WriteLine("Pan NOT supported");
            }


            if (_mediaCapture.VideoDeviceController.Tilt.Capabilities.Supported)
            {
                Debug.WriteLine("Tilt supported: min={0}, max={1}",
                  _mediaCapture.VideoDeviceController.Tilt.Capabilities.Min,
                  _mediaCapture.VideoDeviceController.Tilt.Capabilities.Max);
                tilt_step = (int) ((_mediaCapture.VideoDeviceController.Tilt.Capabilities.Max - _mediaCapture.VideoDeviceController.Tilt.Capabilities.Min) / 10);
                Debug.WriteLine("Tilt step: {0}", tilt_step);
            }
            else
            {
                Debug.WriteLine("Tilt NOT supported");
            }


            if (_mediaCapture.VideoDeviceController.Zoom.Capabilities.Supported)
            {
                Debug.WriteLine("Zoom supported: min={0}, max={1}",
                  _mediaCapture.VideoDeviceController.Zoom.Capabilities.Min,
                  _mediaCapture.VideoDeviceController.Zoom.Capabilities.Max);
                zoom_step = (int)((_mediaCapture.VideoDeviceController.Zoom.Capabilities.Max - _mediaCapture.VideoDeviceController.Zoom.Capabilities.Min) / 10);
                Debug.WriteLine("Zoom step: {0}", zoom_step);
            }
            else
            {
                Debug.WriteLine("Zoom NOT supported");
            }


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

        public async Task<bool> initializeMedia()
        {
            DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

            foreach (var device in devices)
            {
                deviceList.Add(new DeviceListItem(device.Name, device.Id));
            }

            return true;

        }

        private async Task<bool> disposeMedia()
        {
            if (_mediaCapture == null)
            {
                return true;
            }

            await _mediaCapture.StopPreviewAsync();
            _mediaCapture.Dispose();
            _mediaCapture = null;

            return true;

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
