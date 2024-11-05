'Form Register
Imports MySql.Data.MySqlClient 'mengimpor kelas-kelas yang dibutuhkan untuk menghubungkan dan mengakses database MySQL

Public Class Form2 'mendeklarasikan kelas Form2 yang merupakan form registrasi
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load 'mendefinisikan subrutin yang akan dijalankan saat form pertama kali dimuat
        'Me.MdiParent = Awal1  ' Menetapkan form saat ini sebagai form anak dari form induk Awal1
        Me.Text = "Register Page" 'mengatur judul form menjadi "Register Page"
        Label1.Text = "Name" 'mengatur teks label pertama menjadi "Name"
        Label2.Text = "Username" 'mengatur teks label kedua menjadi "Username"
        Label3.Text = "Password" 'mengatur teks label ketiga menjadi "Password"
        Label4.Text = "tiketIn (Register)" 'mengatur teks label ketiga menjadi "tiketIn (Register)"
        Button1.Text = "Register" 'mengatur teks tombol menjadi "Register"
        Button2.Text = "Exit" 'mengatur teks tombol menjadi "Exit"
        TextBox3.PasswordChar = "*" 'Mengatur karakter yang ditampilkan saat pengguna memasukkan password di TextBox3 menjadi "*"
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Then
            MsgBox("Lengkapi Data!", MsgBoxStyle.Exclamation, "Gagal Buat Akun!")
        Else
            Try
                Koneksi()

                ' Check if the username already exists in the database
                Dim checkUserQuery As String = "SELECT COUNT(*) FROM akun WHERE usernameAkun = @username"
                cmd = New MySqlCommand(checkUserQuery, con)
                cmd.Parameters.AddWithValue("@username", TextBox2.Text)

                Dim userCount As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                If userCount > 0 Then
                    ' Username is already in use, show a message
                    MsgBox("Username sudah dipakai!", MsgBoxStyle.Exclamation, "Gagal Buat Akun!")
                Else
                    ' Username is available, proceed to save
                    Dim simpan As String = "INSERT INTO akun VALUES ('', @name, @username, @password)"
                    cmd = New MySqlCommand(simpan, con)
                    cmd.Parameters.AddWithValue("@name", TextBox1.Text)
                    cmd.Parameters.AddWithValue("@username", TextBox2.Text)
                    cmd.Parameters.AddWithValue("@password", TextBox3.Text)
                    cmd.ExecuteNonQuery()

                    MsgBox("Registrasi Berhasil!", MsgBoxStyle.Information, "Selamat! Silahkan Login...")
                    Form1.Show()
                    Me.Close()
                End If

            Catch ex As Exception
                MsgBox("Error: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Me.Hide()
        Form1.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Application.Exit()
    End Sub
End Class