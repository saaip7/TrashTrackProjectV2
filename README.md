# Trash Track
C# WPF Application based on SDGS

Kemudian copy file "PinMap.png" dari folder img awal, ke img yang baru (dalam folder bin)
## Execute file SQL dalam folder db
* caranya: <https://youtu.be/AJdwp0fAHf4?si=8TahMJjTVvEUagHq>
* selain execute .sql bisa juga menggunakan import .bacpac : <https://youtu.be/kcVhcYrGyXE?si=1M7vMYrxpu_Gxd-3>
* jika salah satu dari dua diatas sudah dilakukan, tapi aplikasi tidak berjalan semestinya. Update Connection String pada
```App.config```
* Jika setting SQL Server anda masih default. Kemungkinan Perubahannya hanya menyalin connection string berikut:
  
  ```
  Data Source=localhost\sqlexpress01;Initial Catalog=trashTrackProject;Integrated Security=True
  ```
## Instalasi file dalam SQL Server Melalui SQL Server Management Studio
##Persyaratan
1. SQL Server Management Studio (SSMS)
2. SQL Server 2015
   
###Langkah-langkah
1. Buka SQL Server Management Studio (SSMS):
    * Jalankan SQL Server Management Studio di komputer Anda.
    * Hubungkan ke SQL Server:
2. Hubungkan ke instansi SQL Server Anda menggunakan kredensial server.
    * Buat Database Baru (Opsional):
    * Jika Anda ingin mengimpor database ke database baru, buat database baru yang kosong di SSMS.
3. Buka Jendela Kueri Baru:
    * Buka jendela kueri baru di SSMS. (Klik kanan pada database)
4. Buka Berkas SQL:
    * Klik pada File > Open > File....
    * Telusuri lokasi berkas .sql Anda dan pilihnya.
5. Periksa Skrip SQL:
    * Periksa skrip SQL untuk memastikan bahwa itu berisi pernyataan pembuatan database dan struktur tabel yang diperlukan.
7. Jalankan Skrip SQL:
    * Jalankan skrip SQL dengan mengklik tombol Execute atau menekan F5.
    * Ini akan menjalankan skrip SQL dan membuat database dan tabel.
9. Verifikasi Database:
    * Verifikasi bahwa database dan tabel telah dibuat dengan me-refresh node Database di SSMS Object Explorer.
8. Selesai:
    * Anda telah berhasil mengimpor database ke SQL Server Management Studio.
