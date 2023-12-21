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

namespace TrashTrackProjectV2.View
{
    /// <summary>
    /// Interaction logic for Pesan.xaml
    /// </summary>
    public partial class Pesan : UserControl
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        public static string userID = File.ReadAllText(@"jwt.json");
        private string alamat;
        private DateTime estimasiWaktu;
        public static MPoint PinCoordinate = new MPoint(0, 0);
        TrashTrackProjectV2.Model.Pesan pesan = new TrashTrackProjectV2.Model.Pesan();
        public Pesan()
        {
            InitializeComponent();
            CekPesan();
            //Create Map
            MapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
            //Map Borders
            MapControl.Map.Navigator.OverridePanBounds = new MRect(12250759.8997, -838054.2427, 12339231.2208, -924691.7367);
            MapControl.Map.Navigator.OverrideZoomBounds = new MMinMax(0, 200);
            lblbinding.Text = pesan.isPesanActive().ToString();
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
            pesan.alamat = formattedAddress;
            txtKoor.Text = formattedAddress;
            txtlatitude.Text = PinCoordinate.X.ToString();
            txtlongitude.Text = PinCoordinate.Y.ToString();
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
            subscription subscription = new subscription(-1);
            long voucher = subscription.GetVoucherValue()-1;
            if (voucher < 0)
            {
                MessageBox.Show("Voucher anda sudah habis");
            }
            else
            {
                subscription.AddVoucher();
                pesan.namaPetugas = NamaPetugas();
                txtNama.Text = pesan.namaPetugas;
                PesenBtn.Visibility = Visibility.Hidden;
                SelesaiBtn.Visibility = Visibility.Visible;
                pesan.latitude = PinCoordinate.X;
                pesan.longitude = PinCoordinate.Y;
                // Mendapatkan tanggal dan waktu saat ini
                DateTime currentTime = DateTime.Now;
                // Menambahkan satu jam ke tanggal dan waktu saat ini
                estimasiWaktu= currentTime.AddHours(1);
                pesan.estimasi = estimasiWaktu.ToString();
                txtWaktu.Text = estimasiWaktu.ToString(@"hh\:mm\:ss");
                lblVoucher.Text = voucher.ToString();
                insertPesan();
            }
        }

        
        private void SelesaiBtn_Click(object sender, RoutedEventArgs e)
        {

            //tambahkan pesanan ke history
            insertHistory();
            deletePesanan();
            txtNama.Text = "";
            txtWaktu.Text = "";
            txtKoor.Text="";
            txtlatitude.Text = "";
            txtlongitude.Text = "";
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
                    command.Parameters.AddWithValue("@alamat", pesan.alamat);
                    command.Parameters.AddWithValue("@latitude", pesan.latitude);
                    command.Parameters.AddWithValue("@longtitude", pesan.longitude);
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
            catch (Exception e)
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
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return isSuccess;
        }
    }
}