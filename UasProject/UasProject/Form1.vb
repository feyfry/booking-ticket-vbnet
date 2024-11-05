'Form Login
Imports MySql.Data.MySqlClient 'mengimpor kelas-kelas yang dibutuhkan untuk menghubungkan dan mengakses database MySQL

Public Class Form1 'mendeklarasikan kelas Form1 yang merupakan form login
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load 'mendefinisikan subrutin yang akan dijalankan saat form pertama kali dimuat
        Koneksi() 'memanggil prosedur koneksi yang telah didefinisikan di modul
        'Me.MdiParent = Awal1  ' Menetapkan form saat ini sebagai form anak dari form induk Awal1
        Me.Text = "Login Page" 'mengatur judul form menjadi "Login Page"
        Label1.Text = "Username" 'mengatur teks label pertama menjadi "Username"
        Label2.Text = "Password" 'mengatur teks label kedua menjadi "Password"
        Label3.Text = "tiketIn (Login)" 'mengatur teks label kedua menjadi "tiketIn (Login)"
        Button1.Text = "Login" 'mengatur teks tombol menjadi "Login"
        Button2.Text = "Exit" 'mengatur teks tombol menjadi "Exit'
        LinkLabel1.Text = "Don't Have an Account? Register Here!" 'mengatur teks link label menjadi "Don't Have an Account? Register Here!"
        TextBox2.PasswordChar = "*" 'Mengatur karakter yang ditampilkan saat pengguna memasukkan password di TextBox2 menjadi "*"
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click 'mendefinisikan subrutin yang akan dijalankan saat tombol login diklik
        cmd = New MySqlCommand("SELECT * FROM akun WHERE usernameAkun='" & TextBox1.Text & "' AND passwordAkun='" & TextBox2.Text & "'", con) 'membuat objek perintah MySQL dengan query untuk memeriksa apakah ada akun yang sesuai dengan username dan password yang dimasukkan pengguna
        dr = cmd.ExecuteReader() 'membuat objek pembaca data yang akan mengeksekusi perintah dan mengembalikan hasilnya
        dr.Read() 'membaca baris pertama dari hasil query
        If dr.HasRows Then 'jika ada baris yang dikembalikan, berarti ada akun yang cocok
            MsgBox("Login Sukses", MsgBoxStyle.Information, "Selamat Datang!") 'menampilkan pesan kotak dialog dengan informasi bahwa login sukses
            username = TextBox1.Text 'menyimpan username pengguna ke dalam variabel username
            Me.Hide() 'menyembunyikan form login
            Awal1.Show() 'menampilkan form ketiga yang merupakan form menu MDI
        Else 'jika tidak ada baris yang dikembalikan, berarti tidak ada akun yang cocok
            MsgBox("Login Gagal", MsgBoxStyle.Exclamation, "Coba Lagi!") 'menampilkan pesan kotak dialog dengan peringatan bahwa login gagal
        End If

        ' Menutup DataReader setelah menggunakannya
        dr.Close()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked 'mendefinisikan subrutin yang akan dijalankan saat link label diklik
        Me.Hide() 'menutup form login
        Form2.Show() 'menampilkan form kedua yang merupakan form registrasi
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Application.Exit()
    End Sub
End Class