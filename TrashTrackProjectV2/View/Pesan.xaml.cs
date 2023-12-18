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


namespace TrashTrackProjectV2.View
{
    /// <summary>
    /// Interaction logic for Pesan.xaml
    /// </summary>
    public partial class Pesan : UserControl
    {
        public Pesan()
        {
            InitializeComponent();
            //Create Map
            MapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
            //Map Borders
            MapControl.Map.Navigator.OverridePanBounds = new MRect(12250759.8997, -838054.2427, 12339231.2208, -924691.7367);
            MapControl.Map.Navigator.OverrideZoomBounds = new MMinMax(0, 200);

            // Membuat Pin lokasi TPA
            var pointFeature = new PointFeature(12250759.8997, -838054.2427)
            {
                Styles = new[] { new SymbolStyle { BitmapId = GetBitmapIdForEmbeddedResource("img/PinMap.png"), SymbolScale = 0.03 } },
            };

            // Menambahkan PointFeature ke layer pada peta
            var layer = new MemoryLayer();
            layer.Features = new List<PointFeature> { pointFeature };
            layer.Style = null;
            MapControl.Map.Layers.Add(layer);
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

        private void MapRBUp(object sender, MouseButtonEventArgs e)
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
            MessageBox.Show("Koordinat X: " + Convert.ToString(pinLonLat.X) + ", Koordinat Y: " + Convert.ToString(pinLonLat.Y));
        }
    }
}
