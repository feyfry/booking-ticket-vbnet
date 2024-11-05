'imports data mysql dari library VB.net
Imports MySql.Data.MySqlClient 'mengimpor kelas-kelas yang dibutuhkan untuk menghubungkan dan mengakses database MySQL
Module Module1 'mendeklarasikan modul yang berisi variabel, prosedur, dan fungsi global
    Public con As MySqlConnection 'mendeklarasikan variabel con sebagai objek koneksi MySQL
    Public cmd As MySqlCommand 'mendeklarasikan variabel cmd sebagai objek perintah MySQL
    Public ds As DataSet 'mendeklarasikan variabel ds sebagai objek kumpulan data
    Public da As MySqlDataAdapter 'mendeklarasikan variabel da sebagai objek adapter data MySQL
    Public dr As MySqlDataReader 'mendeklarasikan variabel dr sebagai objek pembaca data MySQL
    Public dt As DataTable 'mendeklarasikan variabel dt sebagai objek tabel data
    Public db As String 'mendeklarasikan variabel db sebagai string yang menyimpan informasi koneksi database
    Public username As String 'mendeklarasikan variabel username sebagai string yang menyimpan nama pengguna yang sedang login
    'buat prosedur global dengan nama koneksi , yang akan dipanggil disetiap form.
    Public Sub Koneksi() 'mendefinisikan prosedur koneksi yang tidak mengembalikan nilai
        'untuk koneksi data , server localhost, user id buat aja root (default)
        ' paswword kosongin kalau default, database sesuaikan dengan yang kalian buat
        db = "Server=localhost;user id=root;password=;database=faiz_2021230006_ticketdb" 'mengisi variabel db dengan string koneksi yang sesuai dengan server, user id, password, dan nama database yang digunakan
        con = New MySqlConnection(db) 'membuat objek koneksi baru dengan parameter string koneksi db
        Try 'mencoba menjalankan blok kode berikut
            If con.State = ConnectionState.Closed Then 'jika status koneksi adalah tertutup
                con.Open() 'membuka koneksi ke database
                ' MsgBox("Koneksi ke database berhasil", MsgBoxStyle.Information, "Informasi") ' menampilkan pesan informasi bahwa koneksi berhasil
            End If
        Catch ex As Exception 'menangkap kesalahan apapun yang terjadi saat menjalankan blok kode di atas
            MsgBox(Err.Description, MsgBoxStyle.Critical, "Error") 'menampilkan pesan kritikal yang berisi deskripsi kesalahan
        End Try
    End Sub
    Public Function SQLTable(ByVal Source As String) As DataTable 'mendefinisikan fungsi SQLTable yang menerima parameter Source sebagai string dan mengembalikan nilai berupa objek tabel data
        ' ---fungsi untuk membuat nomor otomatis dengan menghubungkan pada field yang ada di table--
        Try 'mencoba menjalankan blok kode berikut
            Dim adp As New MySqlDataAdapter(Source, con) 'membuat objek adapter data baru dengan parameter Source sebagai string perintah SQL dan con sebagai objek koneksi
            Dim DT As New DataTable 'membuat objek tabel data baru
            adp.Fill(DT) 'mengisi objek tabel data dengan hasil eksekusi perintah SQL
            SQLTable = DT 'mengembalikan objek tabel data sebagai nilai fungsi
        Catch ex As SqlClient.SqlException 'menangkap kesalahan yang terjadi saat menjalankan blok kode di atas yang berkaitan dengan SQL
            MsgBox(ex.Message) 'menampilkan pesan yang berisi pesan kesalahan
            SQLTable = Nothing 'mengembalikan nilai Nothing sebagai nilai fungsi
        End Try
    End Function
End Module