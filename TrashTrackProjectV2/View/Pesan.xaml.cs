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
using System.Text.RegularExpressions;
using TrashTrackProjectV2.Model;
using System.Data.SqlClient;
using System.Configuration;
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
        public static string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        public string userID = File.ReadAllText(@"jwt.json");
        private string alamat;
        private DateTime estimasiWaktu;
        public static MPoint PinCoordinate = new MPoint(0, 0);
        TrashTrackProjectV2.Model.Pesan pesan = new TrashTrackProjectV2.Model.Pesan();
        public static string AlamatMap = new string(string.Empty);
        string[] LatData = new string[7];
        string[] LonData = new string[7];
        string[] AddressData = new string[7];
        public Pesan()
        {
            InitializeComponent();
            CekPesan();
            //Create Map
            MapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
            //Map Borders
            MapControl.Map.Navigator.OverridePanBounds = new MRect(12250759.8997, -838054.2427, 12339231.2208, -924691.7367);
            MapControl.Map.Navigator.OverrideZoomBounds = new MMinMax(0, 200);
            if (pesan.isPesanActive())
            {
                PinCoordinate.X = pesan.GetPesanActive().latitude;
                PinCoordinate.Y = pesan.GetPesanActive().longitude;
            }
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
            MapControl.Map.Refresh();
            txtKoor.Text = AlamatMap;
            if (txtKoor.Text.Length > 51)
            {
                BtnLocationExpand.Visibility = Visibility.Visible;
            }
            else
            {
                BtnLocationExpand.Visibility = Visibility.Collapsed;
            }
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
            if (txtNama.Text == "")
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
                MRect bbox = new MRect(12250759.8997, -838054.2427, 12339231.2208, -924691.7367);
                if (bbox.Contains(worldPosition))
                {
                    var pinLonLat = SphericalMercator.ToLonLat(worldPosition);
                    PinCoordinate = pinLonLat;
                    string address = await (GetAddressFromCoordinates(pinLonLat.Y, pinLonLat.X, "a77f4e85a7714142b456302043856fe7"));
                    string formattedAddress = ExtractFormattedAddress(address);
                    AlamatMap = formattedAddress;
                    txtKoor.Text = AlamatMap;
                    if (txtKoor.Text.Length > 51)
                    {
                        BtnLocationExpand.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        BtnLocationExpand.Visibility = Visibility.Collapsed;
                    }
                    MessageBox.Show(formattedAddress);
                    canvas.Children.Clear();
                }
                else
                {
                    MessageBox.Show("Lokasi berada di luar jangkauan");
                    MapControl.Map.Layers.Remove(layer);
                    AlamatMap = new string(string.Empty);
                    txtKoor.Text = AlamatMap;
                }
            }
            else
            {
                MessageBox.Show("Pesanan sedang berlangsung. Mohon tunggu hingga pesanan selesai");
            }
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
            if (startIndex > 12 && endIndex > startIndex && json.IndexOf("\\\"display_name\\\"", start) != -1)
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
                return json.Substring(startIndex, endIndex - startIndex); // ekstrak substring antara startIndex dan endIndex
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
            subscription subscription = new subscription(-1);
            long voucher = subscription.GetVoucherValue() - 1;
            if (voucher < 0)
            {
                MessageBox.Show("Voucher anda sudah habis");
            }
            else
            {
                if (txtKoor.Text == null || txtKoor.Text == "")
                {
                    MessageBox.Show("Lokasi belum ditentukan");
                    return;
                }
                else
                {
                    subscription.AddVoucher();
                    pesan.namaPetugas = NamaPetugas();
                    txtNama.Text = pesan.namaPetugas;
                    PesenBtn.Visibility = Visibility.Hidden;
                    SelesaiBtn.Visibility = Visibility.Visible;
                    //mendapatkan koordinat dari pin
                    pesan.latitude = PinCoordinate.X;
                    pesan.longitude = PinCoordinate.Y;
                    // Mendapatkan tanggal dan waktu saat ini
                    DateTime currentTime = DateTime.Now;
                    // Menambahkan satu jam ke tanggal dan waktu saat ini
                    estimasiWaktu = currentTime.AddHours(1);
                    pesan.estimasi = estimasiWaktu.ToString();
                    txtWaktu.Text = estimasiWaktu.ToString(@"HH\:mm\:ss");
                    lblVoucher.Text = voucher.ToString();
                    insertPesan();    
                }
            }
        }


        private void SelesaiBtn_Click(object sender, RoutedEventArgs e)
        {

            //tambahkan pesanan ke history
            insertHistory();
            deletePesanan();
            txtNama.Text = "";
            txtWaktu.Text = "";
            txtKoor.Text = "";
            AlamatMap = new string(string.Empty);
            BtnLocationExpand.Visibility = Visibility.Collapsed;
            var pinLayer = MapControl.Map.Layers.FirstOrDefault(l => l.Name == "PinLayer");
            if (pinLayer != null)
            {
                MapControl.Map.Layers.Remove(pinLayer);
            }
            MessageBox.Show("Terimakasih sudah menggunakan jasa kami :D");
            SelesaiBtn.Visibility = Visibility.Hidden;
            PesenBtn.Visibility = Visibility.Visible;
        }

        private string NamaPetugas()
        {
            // Array nama petugas
            string[] nama = { "Budi", "Siti", "Dewi", "Rudi", "Andi", "Lina", "Hadi", "Nina", "Eko", "Linda", "Ari", "Maya", "Doni", "Dina", "Hery", "Rina", "Adi", "Anita", "Joko", "Yuni" };
            Random random = new Random();
            // Mendapatkan indeks acak
            int randomIndex = random.Next(nama.Length);
            // Mengambil nilai dari array pada indeks acak
            string petugasRandom = nama[randomIndex];

            return petugasRandom;
        }
        private void CekPesan()
        {
            if (pesan.isPesanActive())
            {
                /*txtNama.Text = pesan.GetPesanActive().namaPetugas;
                txtWaktu.Text = pesan.GetPesanActive().estimasi;*/
                AlamatMap = pesan.GetPesanActive().alamat;
                txtKoor.Text = pesan.GetPesanActive().alamat;
                PesenBtn.Visibility = Visibility.Hidden;
                SelesaiBtn.Visibility = Visibility.Visible;
            }
   
        }

        private bool insertPesan()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            bool isSuccess = false;
            try
            {
                string query = "INSERT INTO tb_pesanan (user_id, namaPetugas, alamat, latitude, longitude, EstimasiPengambilan, Status) VALUES (@user_id, @Nama, @alamat, @latitude, @longtitude, @estimasi, @status)";

                // Membuka koneksi ke database
                connection.Open();

                // Membuat objek SqlCommand
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Menambahkan parameter ke query
                    command.Parameters.AddWithValue("@user_id", userID);
                    command.Parameters.AddWithValue("@Nama", pesan.namaPetugas);
                    command.Parameters.AddWithValue("@alamat", txtKoor.Text);
                    command.Parameters.AddWithValue("@latitude", PinCoordinate.X);
                    command.Parameters.AddWithValue("@longtitude", PinCoordinate.Y);
                    command.Parameters.AddWithValue("@estimasi", pesan.estimasi);
                    command.Parameters.AddWithValue("@status", "Belum diambil");

                    // Eksekusi perintah SQL
                    int rows = command.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        isSuccess = false;
                    }
                }

            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return isSuccess;
        }
        private void deletePesanan()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("DELETE FROM tb_pesanan WHERE user_id = @UserId", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userID);

                    command.ExecuteNonQuery();

                }
            }
        }
        private bool insertHistory()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            bool isSuccess = false;
            try
            {
                string query = "INSERT INTO tb_history (user_id, NamaPetugas, Alamat, Latitude, Longitude, EstimasiPengambilan, Status) SELECT user_id, NamaPetugas, Alamat, Latitude, Longitude, EstimasiPengambilan, 'Selesai' AS Status FROM tb_pesanan WHERE user_id=@user_id; DELETE FROM tb_pesanan WHERE user_id=@user_id";
                //string query = "INSERT INTO tb_history (user_id, namaPetugas, alamat, latitude, longitude, EstimasiPengambilan, Status) VALUES (@user_id, @Nama, @alamat, @latitude, @longtitude, @estimasi, @status)";

                // Membuka koneksi ke database
                connection.Open();

                // Membuat objek SqlCommand
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Menambahkan parameter ke query
                    command.Parameters.AddWithValue("@user_id", userID);


                    // Eksekusi perintah SQL
                    int rows = command.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        isSuccess = false;
                    }
                }

            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return isSuccess;
        }
        private async void LocationKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && txtNama.Text == "")
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
            else if (e.Key == Key.Enter && txtNama.Text != "")
            {

                MessageBox.Show("Pesanan sedang berlangsung. Mohon tunggu hingga pesanan selesai");
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
            AlamatMap = AddressData[index];
            txtKoor.Text = AlamatMap;
            if (txtKoor.Text.Length > 51)
            {
                BtnLocationExpand.Visibility = Visibility.Visible;
            }
            else
            {
                BtnLocationExpand.Visibility = Visibility.Collapsed;
            }
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
                MessageBox.Show("Lokasi berada di luar jangkauan");
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
            if (txtNama.Text == "")
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
            else
            {
                MessageBox.Show("Pesanan sedang berlangsung. Mohon tunggu hingga pesanan selesai");
            }
        }

        private void LocationKeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtLocationQuery.Text) && txtLocationQuery.Text.Length > 0)
            {
                ImgX.Visibility = Visibility.Visible;
                txtCariLokasi.Visibility = Visibility.Collapsed;
            }
            else
            {
                ImgX.Visibility = Visibility.Hidden;
                txtCariLokasi.Visibility = Visibility.Visible;
                canvas.Children.Clear();
            }
        }

        private void txtLocationQueryChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtLocationQuery.Text) && txtLocationQuery.Text.Length > 0)
            {
                txtCariLokasi.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtCariLokasi.Visibility = Visibility.Visible;
            }
        }

        private void ClickLocationexpand(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(AlamatMap);
        }
    } 
}