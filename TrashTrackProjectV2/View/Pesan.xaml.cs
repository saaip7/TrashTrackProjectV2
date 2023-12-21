﻿using System;
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
using NetTopologySuite.Algorithm;
using System.Collections;
using System.Configuration;
using System.Globalization;
using BruTile.Wms;


namespace TrashTrackProjectV2.View
{
    /// <summary>
    /// Interaction logic for Pesan.xaml
    /// </summary>
    public partial class Pesan : UserControl
    {
        public static MPoint PinCoordinate = new MPoint(0, 0);
        string[] LatData = new string[7];
        string[] LonData = new string[7];
        string[] AddressData = new string[7];
        public Pesan()
        {
            InitializeComponent();
            //Create Map
            MapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
            //Map Borders
            MapControl.Map.Navigator.OverridePanBounds = new MRect(12250759.8997, -838054.2427, 12339231.2208, -924691.7367);
            MapControl.Map.Navigator.OverrideZoomBounds = new MMinMax(0, 200);
            var coordinate = SphericalMercator.FromLonLat(PinCoordinate.X, PinCoordinate.Y);
            var pointFeature = new PointFeature(coordinate.x, coordinate.y)
            {
                Styles = new[] { new SymbolStyle { BitmapId = GetBitmapIdForEmbeddedResource("img/PinMap.png"), SymbolScale = 0.03 } },
            };
            //penambahan pin ke map
            var layer = new MemoryLayer() { Name = "PinLayer" };
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
        public async Task<string> GetCoordinatesFromAddress(string input)
        {
            using (var client = new HttpClient())
            {
                if (input != null)
                {
                    string formattedInput = input.Replace(' ', '+');
                    string url = "https://geocode.maps.co/search?q=" + formattedInput;
                    var response = await client.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();

                    string jsonString = JsonConvert.SerializeObject(result);

                    return jsonString;
                }
                else return null;
            }
        }
        public string ExtractFormattedAddress(string json)
        {
            int startIndex = json.IndexOf("\\\"display_name\\\"") + 19; // cari kata "display_name", tambahkan 13 untuk melewati ":"
            int endIndex = json.IndexOf("\\\"", startIndex); // cari tanda kutip pertama setelah startIndex
            if (startIndex > 12 && endIndex > startIndex)
            {
                return json.Substring(startIndex, endIndex - startIndex); // ekstrak substring antara startIndex dan endIndex
            }
            return null;
        }
        public string ExtractFormattedAddress(string json, int start)
        {
            if (json.IndexOf("\\\"display_name\\\"") == -1)
            {
                return null;
            }
            int startIndex = json.IndexOf("\\\"display_name\\\"", start) + 19; // cari kata "display_name", tambahkan 13 untuk melewati ":"
            int endIndex = json.IndexOf("\\\"", startIndex); // cari tanda kutip pertama setelah startIndex
            if (startIndex > 12 && endIndex > startIndex && json.IndexOf("\\\"display_name\\\"", start)!=-1)
            {
                return json.Substring(startIndex, endIndex - startIndex); // ekstrak substring antara startIndex dan endIndex
            }
            return null;
        }
        public string ExtractLatitude(string json)
        {
            int startIndex = json.IndexOf("\\\"lat\\\"") + 10; // cari kata "display_name", tambahkan 13 untuk melewati ":"
            int endIndex = json.IndexOf("\\\"", startIndex); // cari tanda kutip pertama setelah startIndex
            if (startIndex > 12 && endIndex > startIndex)
            {
                return  json.Substring(startIndex, endIndex - startIndex); // ekstrak substring antara startIndex dan endIndex
            }
            return null;
        }
        public string ExtractLatitude(string json, int start)
        {
            if (json.IndexOf("\\\"lat\\\"") == -1)
            {
                return null;
            }
            int startIndex = json.IndexOf("\\\"lat\\\"", start) + 10; // cari kata "display_name", tambahkan 13 untuk melewati ":"
            int endIndex = json.IndexOf("\\\"", startIndex); // cari tanda kutip pertama setelah startIndex
            if (startIndex > 12 && endIndex > startIndex)
            {
                return json.Substring(startIndex, endIndex - startIndex); // ekstrak substring antara startIndex dan endIndex
            }
            return null;
        }
        public string ExtractLongitude(string json)
        {
            int startIndex = json.IndexOf("\\\"lon\\\"") + 10; // cari kata "display_name", tambahkan 13 untuk melewati ":"
            int endIndex = json.IndexOf("\\\"", startIndex); // cari tanda kutip pertama setelah startIndex
            if (startIndex > 12 && endIndex > startIndex)
            {
                return json.Substring(startIndex, endIndex - startIndex); // ekstrak substring antara startIndex dan endIndex
            }
            return null;
        }
        public string ExtractLongitude(string json, int start)
        {
            if (json.IndexOf("\\\"lon\\\"") == -1)
            {
                return null;
            }
            int startIndex = json.IndexOf("\\\"lon\\\"", start) + 10;
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

        private async void LocationKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                canvas.Children.Clear();
                if (txtLocationQuery.Text != null)
                {
                    var pinLayer = MapControl.Map.Layers.FirstOrDefault(l => l.Name == "PinLayer");
                    if (pinLayer != null)
                    {
                        MapControl.Map.Layers.Remove(pinLayer);
                    }
                    string result = await (GetCoordinatesFromAddress(txtLocationQuery.Text));
                    if (result == "\"[]\"" && txtLocationQuery.Text != "")
                    {
                        Button newButton = new Button();
                        newButton.Content = "Lokasi tidak ditemukan, letakkan pin pada peta dengan klik kanan";
                        newButton.HorizontalContentAlignment = HorizontalAlignment.Left;
                        newButton.Width = 498;
                        newButton.Height = 30;
                        newButton.Background = new SolidColorBrush(Colors.White);
                        Canvas.SetTop(newButton, 0);
                        Canvas.SetLeft(newButton, 0);
                        canvas.Children.Add(newButton);
                    }
                    string[] formattedLat = new string[7];
                    string[] formattedLon = new string[7];
                    string[] formattedAddress = new string[7];
                    int i = 0, addressStartingIndex = 0, LonStartIndex = 0, LatStartIndex = 0;
                    do
                    {
                        formattedLon[i] = ExtractLongitude(result, LonStartIndex);
                        LonData[i] = formattedLon[i];
                        formattedLat[i] = ExtractLatitude(result, LatStartIndex);
                        LatData[i] = formattedLat[i];
                        formattedAddress[i] = ExtractFormattedAddress(result, addressStartingIndex);
                        AddressData[i] = formattedAddress[i];
                        i++;
                        LonStartIndex = result.IndexOf("\\\"lon\\\"", LonStartIndex) + 10;
                        LatStartIndex = result.IndexOf("\\\"lat\\\"", LatStartIndex) + 10;
                        addressStartingIndex = result.IndexOf("\\\"display_name\\\"", addressStartingIndex) + 19;

                    } while (formattedAddress[i - 1] != null && i < 6);
                    double top = 0;
                    for (i = 0; i < formattedAddress.Length; i++)
                    {
                        string buttontext = formattedAddress[i];
                        if (buttontext != null)
                        {
                            Button newButton = new Button();
                            newButton.Content = buttontext;
                            newButton.HorizontalContentAlignment = HorizontalAlignment.Left;
                            newButton.Width = 498;
                            newButton.Height = 30;
                            newButton.Background = new SolidColorBrush(Colors.White);

                            // Menambahkan event handler untuk button
                            int index = i; // Membuat salinan variabel indeks
                            newButton.Click += (sender, e) => Button_Click(sender, e, index);

                            // Menentukan posisi button
                            Canvas.SetTop(newButton, top);
                            Canvas.SetLeft(newButton, 0);

                            // Menambahkan button ke dalam canvas
                            canvas.Children.Add(newButton);

                            // Mengatur posisi untuk button selanjutnya
                            top += newButton.Height;
                        }
                    }
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e, int index)
        {
            var pinLayer = MapControl.Map.Layers.FirstOrDefault(l => l.Name == "PinLayer");
            if (pinLayer != null)
            {
                MapControl.Map.Layers.Remove(pinLayer);
            }
            Button clickedButton = (Button)sender;
            double Lat = double.Parse(LatData[index], CultureInfo.InvariantCulture);
            double Lon = double.Parse(LonData[index], CultureInfo.InvariantCulture);
            PinCoordinate = new MPoint(Lon, Lat);
            var coordinate = SphericalMercator.FromLonLat(Lon, Lat);
            MPoint FlyCoordinate = new MPoint(coordinate.x, coordinate.y);
            MRect bbox = new MRect(12250759.8997, -838054.2427, 12339231.2208, -924691.7367);
            if (bbox.Contains(FlyCoordinate))
            {
                var pointFeature = new PointFeature(coordinate.x, coordinate.y)
                {
                    Styles = new[] { new SymbolStyle { BitmapId = GetBitmapIdForEmbeddedResource("img/PinMap.png"), SymbolScale = 0.03 } },
                };
                //penambahan pin ke map
                var layer = new MemoryLayer() { Name = "PinLayer" };
                layer.Features = new List<PointFeature> { pointFeature };
                layer.Style = null;
                MapControl.Map.Layers.Add(layer);
                MapControl.Map.Navigator.FlyTo(FlyCoordinate, 1, 1L);
                canvas.Children.Clear();
            }
            else
            {
                MessageBox.Show("Lokasi berada diluar jangkauan");
                canvas.Children.Clear();
                txtLocationQuery.Clear();
            }
        }

        private void ClickX(object sender, MouseButtonEventArgs e)
        {
            txtLocationQuery.Clear();
            canvas.Children.Clear();
            ImgX.Visibility = Visibility.Hidden;
        }

        private async void ClickSearch(object sender, MouseButtonEventArgs e)
        {
            canvas.Children.Clear();
            if (txtLocationQuery.Text != null)
            {
                var pinLayer = MapControl.Map.Layers.FirstOrDefault(l => l.Name == "PinLayer");
                if (pinLayer != null)
                {
                    MapControl.Map.Layers.Remove(pinLayer);
                }
                string result = await (GetCoordinatesFromAddress(txtLocationQuery.Text));
                if (result == "\"[]\"" && txtLocationQuery.Text != "")
                {
                    Button newButton = new Button();
                    newButton.Content = "Lokasi tidak ditemukan, letakkan pin pada peta dengan klik kanan";
                    newButton.HorizontalContentAlignment = HorizontalAlignment.Left;
                    newButton.Width = 498;
                    newButton.Height = 30;
                    newButton.Background = new SolidColorBrush(Colors.White);
                    Canvas.SetTop(newButton, 0);
                    Canvas.SetLeft(newButton, 0);
                    canvas.Children.Add(newButton);
                }
                string[] formattedLat = new string[7];
                string[] formattedLon = new string[7];
                string[] formattedAddress = new string[7];
                int i = 0, addressStartingIndex = 0, LonStartIndex = 0, LatStartIndex = 0;
                do
                {
                    formattedLon[i] = ExtractLongitude(result, LonStartIndex);
                    LonData[i] = formattedLon[i];
                    formattedLat[i] = ExtractLatitude(result, LatStartIndex);
                    LatData[i] = formattedLat[i];
                    formattedAddress[i] = ExtractFormattedAddress(result, addressStartingIndex);
                    AddressData[i] = formattedAddress[i];
                    i++;
                    LonStartIndex = result.IndexOf("\\\"lon\\\"", LonStartIndex) + 10;
                    LatStartIndex = result.IndexOf("\\\"lat\\\"", LatStartIndex) + 10;
                    addressStartingIndex = result.IndexOf("\\\"display_name\\\"", addressStartingIndex) + 19;

                } while (formattedAddress[i - 1] != null && i < 6);
                double top = 0;
                for (i = 0; i < formattedAddress.Length; i++)
                {
                    string buttontext = formattedAddress[i];
                    if (buttontext != null)
                    {
                        Button newButton = new Button();
                        newButton.Content = buttontext;
                        newButton.HorizontalContentAlignment = HorizontalAlignment.Left;
                        newButton.Width = 498;
                        newButton.Height = 30;
                        newButton.Background = new SolidColorBrush(Colors.White);

                        // Menambahkan event handler untuk button
                        int index = i; // Membuat salinan variabel indeks
                        newButton.Click += (sender, e) => Button_Click(sender, e, index);

                        // Menentukan posisi button
                        Canvas.SetTop(newButton, top);
                        Canvas.SetLeft(newButton, 0);

                        // Menambahkan button ke dalam canvas
                        canvas.Children.Add(newButton);

                        // Mengatur posisi untuk button selanjutnya
                        top += newButton.Height;
                    }
                }
            }
        }

        private void LocationKeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtLocationQuery.Text) && txtLocationQuery.Text.Length > 0)
            {
                ImgX.Visibility = Visibility.Visible;
            }
            else
            {
                ImgX.Visibility = Visibility.Hidden;
                canvas.Children.Clear();
            }
        }
    }
}
