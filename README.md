# Trash Track
C# WPF Application based on SDGS

### ⚠️⚠️ Ada bug API ⚠️⚠️  
Untuk mengatasi nya, buat folder img dalam folder:  
"...bin/Debug/net6.0-windows"

Kemudian copy file "PinMap.png" dari folder img awal, ke img yang baru (dalam folder bin)
## Execute file SQL dalam folder db
* caranya: <https://youtu.be/AJdwp0fAHf4?si=8TahMJjTVvEUagHq>
* selain execute .sql bisa juga menggunakan import .bacpac : <https://youtu.be/kcVhcYrGyXE?si=1M7vMYrxpu_Gxd-3>
* jika salah satu dari dua diatas sudah dilakukan, tapi aplikasi tidak berjalan semestinya. Update Connection String pada ```App.config```
* Jika setting SQL Server anda masih default. Kemungkinan Perubahannya hanya menyalin connection string berikut:
  
  ```
  Data Source=localhost\sqlexpress01;Initial Catalog=trashTrackProject;Integrated Security=True
  ```
