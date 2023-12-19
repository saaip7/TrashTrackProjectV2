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
using Mapsui.UI.Wpf;
using Mapsui;
using Mapsui.Styles;
using System.IO;
using Mapsui.Layers;
using Mapsui.Extensions;
using Mapsui.Projections;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using TrashTrackProjectV2.Model;


namespace TrashTrackProjectV2.View
{
    /// <summary>
    /// Interaction logic for Pesan.xaml
    /// </summary>
    public partial class Pesan : UserControl
    {
        public static MPoint PinCoordinate = new MPoint(0, 0);
        public Pesan()
        {
            InitializeComponent();
            //Create Map
            MapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
            //Map Borders
            MapControl.Map.Navigator.OverridePanBounds = new MRect(12250759.8997, -838054.2427, 12339231.2208, -924691.7367);
            MapControl.Map.Navigator.OverrideZoomBounds = new MMinMax(0, 200);
        }

        //mendapat bitmap id dari gambar
        private static int GetBitmapIdForEmbeddedResource(string imagePath)
        {
            using (FileStream fs = new FileStream(imagePath, FileMode.Open))
            {
                var memoryStream = new MemoryStream();
                fs.CopyTo(memoryStream);
                var bitmapId = BitmapRegistry.Instance.Register(memoryStream);
                return bitmapId;
            }

        }

        private async void MapRBUp(object sender, MouseButtonEventArgs e)
        {
            //menghapus pin yang sudah ada
            var pinLayer = MapControl.Map.Layers.FirstOrDefault(l => l.Name == "PinLayer");
            if (pinLayer != null)
            {
                MapControl.Map.Layers.Remove(pinLayer);
            }
            var screenPosition = e.GetPosition(MapControl);//lokasi relatif pada layar
            var worldPosition = MapControl.Map.Navigator.Viewport.ScreenToWorld(screenPosition.X, screenPosition.Y);//lokasi pada map(EPSG 3857)
            //deklarasi pin
            var pointFeature = new PointFeature(worldPosition.X, worldPosition.Y)
            {
                Styles = new[] { new SymbolStyle { BitmapId = GetBitmapIdForEmbeddedResource("img/PinMap.png"), SymbolScale = 0.03 } },
            };
            //penambahan pin ke map
            var layer = new MemoryLayer() { Name = "PinLayer" };
            layer.Features = new List<PointFeature> { pointFeature };
            layer.Style = null;
            MapControl.Map.Layers.Add(layer);
            var pinLonLat = SphericalMercator.ToLonLat(worldPosition);
            PinCoordinate = pinLonLat;
            string address = await (GetAddressFromCoordinates(pinLonLat.Y, pinLonLat.X, "a77f4e85a7714142b456302043856fe7"));
            string formattedAddress = ExtractFormattedAddress(address);
            MessageBox.Show(formattedAddress);
        }
        public async Task<string> GetAddressFromCoordinates(double latitude, double longitude, string apiKey)
        {
            using (var client = new HttpClient())
            {
                string latitudeString = latitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
                string longitudeString = longitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
                string url = "https://geocode.maps.co/reverse?lat=" + latitudeString + "&lon=" + longitudeString;
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                string jsonString = JsonConvert.SerializeObject(result);

                return jsonString;
            }
        }
        public string ExtractFormattedAddress(string json)
        {
            int startIndex = json.IndexOf("\\\"display_name\\\"") + 19; // cari kata "formatted", tambahkan 13 untuk melewati ":"
            int endIndex = json.IndexOf("\\\"", startIndex); // cari tanda kutip pertama setelah startIndex
            if (startIndex > 12 && endIndex > startIndex)
            {
                return json.Substring(startIndex, endIndex - startIndex); // ekstrak substring antara startIndex dan endIndex
            }
            return null;
        }

        private void PesenBtn_Click(object sender, RoutedEventArgs e)
        {
            PesenBtn.Visibility = Visibility.Hidden;
            SelesaiBtn.Visibility = Visibility.Visible;
        }

        private void SelesaiBtn_Click(object sender, RoutedEventArgs e)
        {
            SelesaiBtn.Visibility = Visibility.Hidden;
            PesenBtn.Visibility = Visibility.Visible;
        }
    }
}
