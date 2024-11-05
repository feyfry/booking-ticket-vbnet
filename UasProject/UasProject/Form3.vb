'Form pemesanan tiket
Imports System.Drawing.Printing 'mengimpor kelas yang dibutuhkan untuk mencetak dokumen grafis
Imports System.Reflection.Emit 'mengimpor kelas yang dibutuhkan untuk menghasilkan kode MSIL secara dinamis
Imports System.Windows.Forms.VisualStyles.VisualStyleElement 'mengimpor kelas yang dibutuhkan untuk mengakses elemen gaya visual
Imports MySql.Data.MySqlClient 'mengimpor kelas-kelas yang dibutuhkan untuk menghubungkan dan mengakses database MySQL

Public Class Form3 'mendeklarasikan kelas Form3 yang merupakan form pemesanan tiket

    'Deklarasi variabel
    Dim da As New MySqlDataAdapter 'mendeklarasikan objek adapter data yang akan mengisi dataset dengan data dari database
    Dim ds As New DataSet 'mendeklarasikan objek kumpulan data yang akan menyimpan data dalam memori
    Dim dt As New DataTable 'mendeklarasikan objek tabel data yang akan menyimpan data dalam bentuk tabel
    Private countdownSeconds As Integer = 3 'mendeklarasikan variabel bertipe integer yang akan menyimpan jumlah detik yang dihitung mundur
    Private PaymentCodes As String() = {"LOKJY12I", "ONLPH31J"} 'mendeklarasikan array bertipe string yang akan menyimpan kode pembayaran statis

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load 'mendefinisikan subrutin yang akan dijalankan saat form ketiga pertama kali dimuat
        'Panggil prosedur koneksi ke database
        Koneksi() 'memanggil prosedur koneksi yang telah didefinisikan di modul

        Me.MdiParent = Awal1  ' Menetapkan form saat ini sebagai form anak dari form induk Awal1
        Me.Text = "Aplikasi Booking Tiket Bus 3rd Party Murah Se-Indonesia" 'mengatur judul form menjadi "Aplikasi Booking Tiket Bus 3rd Party Murah Se-Indonesia"

        'Isi label nama user
        Label1.Text = "Halo " & username & "!" 'mengatur teks label pertama menjadi "Halo " dan nama pengguna yang telah disimpan di variabel username
        Label2.Text = "Jumlah" 'mengatur teks label kedua menjadi "Jumlah"
        Label3.Text = "Total Harga" 'mengatur teks label ketiga menjadi "Total Harga"
        Label4.Text = "Harga" 'mengatur teks label keempat menjadi "Harga"
        Button1.Text = "Pesan" 'mengatur teks tombol pertama menjadi "Pesan"
        Button2.Text = "Hapus" 'mengatur teks tombol kedua menjadi "Hapus"
        Button3.Text = "Selesai" 'mengatur teks tombol ketiga menjadi "Selesai"
        Label5.Text = "Tanggal" 'mengatur teks label kelima menjadi "Tanggal"
        Label6.Text = "Tiket" 'mengatur teks label keenam menjadi "Tiket"
        Label7.Text = "Dari" 'mengatur teks label ketujuh menjadi "Dari"
        Label8.Text = "Ke" 'mengatur teks label kedelapan menjadi "Ke"
        Label9.Text = "Tiket" 'mengatur teks label kesembilan menjadi "Tiket"
        Label10.Text = "Pembayaran" 'mengatur teks label kesepuluh menjadi "Pembayaran"
        Label11.Text = "Aplikasi Booking Tiket Termurah!" ''mengatur teks label kesepuluh menjadi "Aplikasi Booking Tiket Termurah!"
        GroupBox1.Text = "Cari dan Book Tiket" 'mengatur teks group box pertama menjadi "Cari dan Book Tiket"
        GroupBox2.Text = "Metode Pembayaran dan Cetak Tiket" 'mengatur teks group box kedua menjadi "Metode Pembayaran dan Cetak Tiket"
        Me.ContextMenuStrip = ContextMenuStrip1 ' Mengatur menu konteks untuk list view 1
        ListView1.View = View.Details 'mengatur tampilan list view 1 menjadi detail
        ' Mengatur lebar kolom secara otomatis berdasarkan konten
        ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent) 'mengatur lebar kolom list view 1 secara otomatis berdasarkan konten
        ' Atau mengatur lebar kolom secara otomatis berdasarkan judul
        ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize) 'mengatur lebar kolom list view 1 secara otomatis berdasarkan judul


        'Buat check box dan radio button untuk fitur lain
        CheckBox1.Text = "Cetak Tiket" 'mengatur teks check box menjadi "Cetak Tiket"
        RadioButton1.Text = "Bayar Tunai" 'mengatur teks radio button pertama menjadi "Bayar Tunai"
        RadioButton2.Text = "Bayar Online" 'mengatur teks radio button kedua menjadi "Bayar Online"

        ' Mengatur ComboBox1 dan ComboBox2 agar tidak dapat diedit
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList ' Mengubah gaya ComboBox1 menjadi drop-down list

        ' Isi combo box daftar destinasi
        With Me.ComboBox1 'menggunakan objek ComboBox1 sebagai konteks
            .DataSource = SQLTable("SELECT * FROM ticket") 'mengatur sumber data menjadi hasil query SQL yang mengambil semua data dari tabel ticket
            .DisplayMember = "namaTicket" 'mengatur properti yang akan ditampilkan menjadi namaTicket
            .ValueMember = "idTicket" 'mengatur properti yang akan digunakan sebagai nilai menjadi idTicket
        End With 'mengakhiri konteks objek ComboBox1

        ' Atur properti Timer
        Timer1.Interval = 1000 ' Ubah interval menjadi 1000 milidetik (1 detik)
        Timer1.Enabled = False ' Mulai dalam kondisi nonaktif

        Button3.Enabled = False 'mengatur tombol ketiga menjadi tidak aktif
        CheckBox1.Enabled = False 'mengatur check box menjadi tidak aktif
        TextBox2.ReadOnly = True 'mengatur text box kedua menjadi tidak dapat diedit
        TextBox3.ReadOnly = True 'mengatur text box ketiga menjadi tidak dapat diedit
        TextBox4.ReadOnly = True 'mengatur text box keempat menjadi tidak dapat diedit
        TextBox5.ReadOnly = True 'mengatur text box kelima menjadi tidak dapat diedit

        TextBox3.Clear() 'menghapus isi text box ketiga
        TextBox4.Clear() 'menghapus isi text box keempat
        TextBox5.Clear() 'menghapus isi text box kelima
    End Sub

    'Event saat combo box tiket diubah
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged 'mendefinisikan subrutin yang akan dijalankan saat nilai ComboBox1 berubah
        'Ambil data harga tiket
        Try 'mencoba menjalankan blok kode berikut
            'Tentukan kolom mana yang akan digunakan sebagai ValueMember
            ComboBox1.ValueMember = "idTicket" 'mengatur properti ValueMember menjadi idTicket
            'Gunakan parameter dalam query SQL
            Dim cmd As New MySqlCommand("Select hargaTicket FROM ticket WHERE idTicket=@id", con) 'membuat objek perintah MySQL dengan query untuk mengambil harga tiket berdasarkan id tiket
            'Tambahkan nilai parameter
            cmd.Parameters.AddWithValue("@id", ComboBox1.SelectedValue.ToString()) 'menambahkan nilai parameter @id dengan mengkonversi nilai yang dipilih di ComboBox1 ke string
            Dim result = cmd.ExecuteScalar() 'mengeksekusi perintah dan menyimpan hasilnya ke dalam variabel result
            TextBox3.Text = result 'mengatur teks TextBox3 menjadi result
        Catch ex As Exception 'jika terjadi kesalahan saat menjalankan blok kode di atas
            MsgBox(ex.Message) 'menampilkan pesan kesalahan
        End Try 'mengakhiri blok try-catch

        'Isi text box destinasi dan tujuan berdasarkan tiket yang dipilih
        Try 'mencoba menjalankan blok kode berikut
            'Gunakan parameter dalam query SQL
            Dim cmd2 As New MySqlCommand("Select fromDest, toDest FROM ticket WHERE idTicket=@id", con) 'membuat objek perintah MySQL dengan query untuk mengambil destinasi asal dan tujuan berdasarkan id tiket
            'Tambahkan nilai parameter
            cmd2.Parameters.AddWithValue("@id", ComboBox1.SelectedValue.ToString()) 'menambahkan nilai parameter @id dengan mengkonversi nilai yang dipilih di ComboBox1 ke string
            Dim dr As MySqlDataReader = cmd2.ExecuteReader() 'membuat objek pembaca data yang akan mengeksekusi perintah dan mengembalikan hasilnya
            If dr.Read() Then 'jika ada baris yang dibaca
                TextBox4.Text = dr("fromDest") 'mengatur teks TextBox4 menjadi nilai kolom fromDest
                TextBox5.Text = dr("toDest") 'mengatur teks TextBox5 menjadi nilai kolom toDest
            End If 'mengakhiri blok if
            dr.Close() 'menutup objek pembaca data
        Catch ex As Exception 'jika terjadi kesalahan saat menjalankan blok kode di atas
            MsgBox(ex.Message) 'menampilkan pesan kesalahan
        End Try 'mengakhiri blok try-catch
    End Sub 'mengakhiri subrutin

    Private Function Hitung() As Integer 'mendeklarasikan fungsi bernama Hitung yang mengembalikan nilai bertipe integer
        Dim input As String = TextBox1.Text 'mendeklarasikan variabel input yang menyimpan teks dari TextBox1
        Dim jumlah As Integer 'mendeklarasikan variabel jumlah yang akan menyimpan nilai yang dikonversi dari input
        If Not Integer.TryParse(input, jumlah) Then 'mencoba mengkonversi input ke integer dan menyimpannya ke jumlah, jika gagal maka masuk ke blok if
            MsgBox("Input tidak boleh kosong", MsgBoxStyle.Exclamation, "Peringatan") 'menampilkan pesan peringatan
        End If 'mengakhiri blok if

        Return jumlah 'mengembalikan nilai jumlah
    End Function 'mengakhiri fungsi


    'Event saat text box jumlah diubah
    Dim alreadyShown As Boolean = False 'mendeklarasikan variabel alreadyShown yang menyimpan nilai boolean (benar atau salah) dan mengaturnya menjadi salah
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged 'mendefinisikan subrutin yang akan dijalankan saat teks TextBox1 berubah
        If String.IsNullOrEmpty(TextBox3.Text) Then 'jika teks TextBox3 kosong atau null
            If Not alreadyShown Then 'jika variabel alreadyShown masih bernilai salah
                MessageBox.Show("Pilih tiket terlebih dahulu") 'menampilkan pesan bahwa pengguna harus memilih tiket dulu
                alreadyShown = True 'mengubah nilai alreadyShown menjadi benar
            End If 'mengakhiri blok if
            TextBox1.Clear() 'menghapus teks TextBox1
            Exit Sub 'keluar dari subrutin
        End If 'mengakhiri blok if

        'Hitung total harga jika ada nilai pada TextBox1 
        If Not String.IsNullOrEmpty(TextBox1.Text) Then 'jika teks TextBox1 tidak kosong atau null
            Dim jumlah As Integer 'mendeklarasikan variabel jumlah
            If Integer.TryParse(TextBox1.Text, jumlah) Then 'mencoba mengkonversi teks TextBox1 ke integer dan menyimpannya ke jumlah, jika berhasil maka masuk ke blok if
                Dim harga As Double = CDbl(TextBox3.Text) 'mendeklarasikan variabel harga yang menyimpan nilai yang dikonversi dari teks TextBox3 ke double (bilangan pecahan)
                Dim total As Double = harga * jumlah 'mendeklarasikan variabel total yang menyimpan hasil perkalian harga dan jumlah
                TextBox2.Text = total.ToString() 'mengatur teks TextBox2 menjadi total yang dikonversi ke string
            End If 'mengakhiri blok if
        Else 'jika teks TextBox1 kosong atau null
            TextBox2.Text = "0" 'mengatur teks TextBox2 menjadi "0"
        End If 'mengakhiri blok if
    End Sub 'mengakhiri subrutin

    ' Event saat tombol pesan diklik
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click 'mendefinisikan subrutin yang akan dijalankan saat tombol pertama diklik
        Call Hitung() 'memanggil fungsi Hitung yang telah didefinisikan sebelumnya

        Dim jumlahTiket As Integer
        If Not Integer.TryParse(TextBox1.Text, jumlahTiket) OrElse jumlahTiket <= 0 Then
            MsgBox("Jumlah tiket tidak boleh kosong atau kurang dari 1", MsgBoxStyle.Exclamation, "Peringatan")
            Exit Sub
        End If

        If TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Or TextBox5.Text = "" Then 'jika salah satu dari text box kedua, ketiga, keempat, atau kelima kosong
            MessageBox.Show("Pastikan semua kolom sudah terisi dengan benar.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning) 'menampilkan pesan peringatan
            Exit Sub 'keluar dari subrutin
        End If 'mengakhiri blok if

        Select Case True 'memilih kasus berdasarkan kondisi yang benar
            Case RadioButton1.Checked 'jika radio button pertama dicentang (Bayar Tunai)
                MsgBox("Ini adalah kode unik pembayaran: LOKJY12I, Silakan bayar tunai di loket dengan kode tersebut.", MsgBoxStyle.Information) 'menampilkan pesan informasi dengan kode pembayaran

            Case RadioButton2.Checked 'jika radio button kedua dicentang (Bayar Online)
                Dim inputCode As String = InputBox("Ini Adalah Kode Pembayaran Anda" & vbCrLf & PaymentCodes(1), "Pembayaran Online") 'mendeklarasikan variabel inputCode yang menyimpan kode pembayaran yang dimasukkan pengguna
                If Array.Exists(PaymentCodes, Function(code) code = inputCode) Then 'jika kode pembayaran yang dimasukkan ada di dalam array PaymentCodes
                    MsgBox("Pembayaran berhasil. Kode pembayaran valid.", MsgBoxStyle.Information) 'menampilkan pesan informasi bahwa pembayaran berhasil
                Else 'jika kode pembayaran yang dimasukkan tidak ada di dalam array PaymentCodes
                    MsgBox("Kode pembayaran salah. Silakan coba lagi.", MsgBoxStyle.Exclamation) 'menampilkan pesan peringatan bahwa pembayaran gagal
                    Exit Sub 'keluar dari subrutin
                End If 'mengakhiri blok if

            Case Else 'jika tidak ada radio button yang dicentang
                ' Jika tidak ada yang dipilih
                MsgBox("Pilih salah satu metode pembayaran!", MsgBoxStyle.Exclamation, "Peringatan") 'menampilkan pesan peringatan
                Exit Sub 'keluar dari subrutin
        End Select 'mengakhiri blok select case

        ' Simpan data pemesanan ke database
        Try 'mencoba menjalankan blok kode berikut
            ' Cek apakah data sudah ada sebelum insert
            Dim cekSQL = "SELECT COUNT(*) FROM transaction WHERE akun_idAkun = " &
             "(SELECT idAkun FROM akun WHERE usernameAkun = '" & Module1.username & "') AND " &
             "ticket_idTicket = '" & ComboBox1.SelectedValue & "'" 'mendeklarasikan variabel cekSQL yang menyimpan query SQL untuk menghitung jumlah data transaksi yang sesuai dengan id akun dan id tiket

            Dim cmdCheck As New MySqlCommand(cekSQL, Module1.con) 'membuat objek perintah MySQL dengan query cekSQL dan koneksi yang telah didefinisikan di modul
            Dim count = cmdCheck.ExecuteScalar() 'mengeksekusi perintah dan menyimpan hasilnya ke dalam variabel count

            If count > 0 Then 'jika count lebih dari nol, berarti data sudah ada
                MsgBox("Data pemesanan sudah ada", MsgBoxStyle.Information) 'menampilkan pesan informasi
                Exit Sub 'keluar dari subrutin
            End If 'mengakhiri blok if

            ' Menginisialisasikan variabel tanggal sebagai DateTime
            Dim tanggal As DateTime = DateTimePicker1.Value
            'mendeklarasikan variabel tanggalString yang menyimpan tanggal dan waktu saat ini dalam format string
            Dim tanggalString As String = tanggal.ToString("yyyy-MM-dd HH:mm:ss")

            Dim sql As String = "INSERT INTO transaction (akun_idAkun, ticket_idTicket, jumlah, total_harga, tanggal) VALUES (" &
                    "(SELECT idAkun FROM akun WHERE usernameAkun = '" & Module1.username & "'), " &
                    "'" & ComboBox1.SelectedValue & "', " &
                    "'" & TextBox1.Text & "', " &
                    "'" & TextBox2.Text & "', " &
                    "'" & tanggalString & "')" 'mendeklarasikan variabel sql yang menyimpan query SQL untuk memasukkan data transaksi ke dalam tabel

            Dim cmd As New MySqlCommand(sql, Module1.con) 'membuat objek perintah MySQL dengan query sql dan koneksi yang telah didefinisikan di modul
            cmd.ExecuteNonQuery() 'mengeksekusi perintah

            MsgBox("Pemesanan tiket berhasil", MsgBoxStyle.Information, "Hore!...") 'menampilkan pesan informasi bahwa pemesanan tiket berhasil
        Catch ex As Exception 'jika terjadi kesalahan saat menjalankan blok kode di atas
            MsgBox("Gagal memesan tiket. " & ex.Message) 'menampilkan pesan kesalahan
        End Try 'mengakhiri blok try-catch

        TampilData() 'memanggil prosedur TampilData yang telah didefinisikan sebelumnya
        ClearForm() 'memanggil prosedur ClearForm yang telah didefinisikan sebelumnya
        Button3.Enabled = True 'mengatur tombol ketiga menjadi aktif
        CheckBox1.Enabled = True 'mengatur check box menjadi aktif
    End Sub 'mengakhiri subrutin

    'Event saat tombol hapus diklik
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click 'mendefinisikan subrutin yang akan dijalankan saat tombol kedua diklik
        'Hapus semua data dari list view
        ListView1.Items.Clear() 'menghapus semua item dari list view 1
        CheckBox1.Enabled = False 'mengatur check box menjadi tidak aktif

        If CheckBox1.Checked = True Then 'jika check box dicentang
            CheckBox1.Checked = False 'mengatur check box menjadi tidak dicentang
        End If 'mengakhiri blok if
    End Sub 'mengakhiri subrutin

    'Event saat tombol selesai diklik
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click 'mendefinisikan subrutin yang akan dijalankan saat tombol ketiga diklik
        Timer1.Start() 'memulai timer 1

        ' Tampilkan pesan konfirmasi dalam popup dengan hitungan mundur
        ShowCountdownPopup() 'memanggil prosedur ShowCountdownPopup yang telah didefinisikan sebelumnya
    End Sub 'mengakhiri subrutin

    ' Method untuk menampilkan pesan dengan hitungan mundur
    Private Sub ShowCountdownPopup() 'mendeklarasikan subrutin yang akan menampilkan pesan konfirmasi dengan hitungan mundur
        MessageBox.Show("Pemesanan Selesai! Akan keluar dalam " & countdownSeconds & " detik.", "Konfirmasi", MessageBoxButtons.OK, MessageBoxIcon.Information) 'menampilkan kotak dialog dengan teks yang menggabungkan variabel countdownSeconds dan ikon informasi
    End Sub 'mengakhiri subrutin

    Private WithEvents PrintDoc As New Printing.PrintDocument 'mendeklarasikan objek PrintDoc yang akan digunakan untuk mencetak dokumen
    Private CurrentItemIndex As Integer = 0 'mendeklarasikan variabel CurrentItemIndex yang akan menyimpan indeks item yang sedang dicetak
    Private Sub PrintDocoment1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage 'mendefinisikan subrutin yang akan dijalankan saat PrintDocument1 mencetak halaman
        Dim PrintFont As Font = New Font("Arial", 12) 'mendeklarasikan objek PrintFont yang akan digunakan sebagai font untuk mencetak
        Dim lineHeight As Integer = PrintFont.Height + 2 'mendeklarasikan variabel lineHeight yang akan menyimpan tinggi baris untuk spasi antarbaris

        Dim startX As Integer = 50 'mendeklarasikan variabel startX yang akan menyimpan posisi horizontal awal untuk mencetak
        Dim startY As Integer = 100 'mendeklarasikan variabel startY yang akan menyimpan posisi vertikal awal untuk mencetak

        Dim columnPosition As Integer = startX 'mendeklarasikan variabel columnPosition yang akan menyimpan posisi horizontal untuk setiap kolom
        Dim yPos As Integer = startY 'mendeklarasikan variabel yPos yang akan menyimpan posisi vertikal untuk mencetak

        Dim columnWidths As New List(Of Integer) 'mendeklarasikan objek columnWidths yang akan menyimpan lebar kolom dalam bentuk list

        ' Menghitung lebar kolom
        For Each column As ColumnHeader In ListView1.Columns 'melakukan perulangan untuk setiap kolom di list view 1
            columnWidths.Add(column.Width) 'menambahkan lebar kolom ke dalam list columnWidths
        Next 'mengakhiri perulangan

        ' Membatasi lebar kolom
        For i As Integer = 0 To ListView1.Columns.Count - 1 'melakukan perulangan untuk setiap indeks kolom dari nol sampai jumlah kolom dikurangi satu
            ' Mengatur lebar kolom berdasarkan header
            ListView1.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize) 'mengatur lebar kolom secara otomatis berdasarkan ukuran header
            ' Mengatur lebar maksimum
            ListView1.Columns(i).Width = Math.Min(ListView1.Columns(i).Width, columnWidths(i)) 'mengatur lebar kolom menjadi nilai minimum antara lebar kolom saat ini dan lebar kolom yang disimpan di list
        Next 'mengakhiri perulangan

        ' Mencetak header kolom
        For i As Integer = 0 To ListView1.Columns.Count - 1 'melakukan perulangan untuk setiap indeks kolom dari nol sampai jumlah kolom dikurangi satu
            e.Graphics.DrawString(ListView1.Columns(i).Text, PrintFont, Brushes.Black, columnPosition, yPos) 'mencetak teks header kolom dengan font, warna, dan posisi yang ditentukan
            columnPosition += columnWidths(i) 'menambahkan posisi horizontal dengan lebar kolom
        Next 'mengakhiri perulangan

        yPos += lineHeight 'menambahkan posisi vertikal dengan tinggi baris

        ' Lakukan pencetakan untuk setiap item pada ListView
        For i As Integer = CurrentItemIndex To ListView1.Items.Count - 1 'melakukan perulangan untuk setiap indeks item dari CurrentItemIndex sampai jumlah item dikurangi satu
            Dim item As ListViewItem = ListView1.Items(i) 'mendeklarasikan objek item yang menyimpan item list view pada indeks i
            columnPosition = startX 'mengatur ulang posisi horizontal untuk setiap item

            ' Mencetak setiap kolom dari item ListView
            For j As Integer = 0 To item.SubItems.Count - 1 'melakukan perulangan untuk setiap indeks subitem dari nol sampai jumlah subitem dikurangi satu
                e.Graphics.DrawString(item.SubItems(j).Text, PrintFont, Brushes.Black, columnPosition, yPos) 'mencetak teks subitem dengan font, warna, dan posisi yang ditentukan
                columnPosition += columnWidths(j) 'menambahkan posisi horizontal dengan lebar kolom
            Next 'mengakhiri perulangan

            yPos += lineHeight 'menambahkan posisi vertikal dengan tinggi baris

            ' Ubah indeks item yang sedang dicetak untuk halaman berikutnya
            CurrentItemIndex = i 'mengatur CurrentItemIndex menjadi i

            ' Jika masih ada item untuk dicetak, atur HasMorePages menjadi True untuk mencetak halaman berikutnya
            If yPos > e.MarginBounds.Bottom - 50 Then 'jika posisi vertikal melebihi batas bawah margin dikurangi 50
                e.HasMorePages = True 'mengatur properti HasMorePages menjadi benar
                Return 'keluar dari subrutin
            End If 'mengakhiri blok if
        Next 'mengakhiri perulangan

        ' Jika tidak ada lagi item yang akan dicetak, atur HasMorePages menjadi False
        e.HasMorePages = False 'mengatur properti HasMorePages menjadi salah
        CurrentItemIndex = 0 'mengatur ulang CurrentItemIndex menjadi nol
    End Sub 'mengakhiri subrutin

    'Event saat check box cetak tiket dicentang
    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged 'mendefinisikan subrutin yang akan dijalankan saat nilai CheckBox1 berubah
        'Cetak tiket jika dicentang
        If CheckBox1.Checked Then 'jika CheckBox1 dicentang
            CurrentItemIndex = 0 'mengatur ulang CurrentItemIndex menjadi nol
            PrintDocument1.Print() 'memanggil metode Print pada objek PrintDocument1
        End If 'mengakhiri blok if
    End Sub 'mengakhiri subrutin

    ' Hapus sejarah isian input form pengguna
    Private Sub ClearForm() 'mendeklarasikan subrutin yang akan menghapus semua data dari form
        If Not IsNothing(TextBox1) Or Not IsNothing(TextBox2) Or Not IsNothing(TextBox3) Or Not IsNothing(TextBox4) Or Not IsNothing(TextBox5) Or Not IsNothing(RadioButton1) Or Not IsNothing(RadioButton2) Or Not IsNothing(CheckBox1) Then 'jika tidak ada objek yang bernilai null
            TextBox1.Clear() 'menghapus semua teks dalam TextBox1
            TextBox2.Clear() 'menghapus semua teks dalam TextBox2
            TextBox3.Clear() 'menghapus semua teks dalam TextBox3
            TextBox4.Clear() 'menghapus semua teks dalam TextBox4
            TextBox5.Clear() 'menghapus semua teks dalam TextBox5
            RadioButton1.Checked = False 'mengatur RadioButton1 menjadi tidak dicentang
            RadioButton2.Checked = False 'mengatur RadioButton2 menjadi tidak dicentang
            CheckBox1.Checked = False 'mengatur CheckBox1 menjadi tidak dicentang
        End If 'mengakhiri blok if
    End Sub 'mengakhiri subrutin

    Private Sub UpdateColumnWidths() 'mendeklarasikan subrutin yang akan mengatur lebar kolom berdasarkan konten
        ' Atur lebar kolom berdasarkan konten
        For Each column As ColumnHeader In ListView1.Columns 'melakukan perulangan untuk setiap kolom di list view 1
            column.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent) 'mengatur lebar kolom secara otomatis berdasarkan konten

            ' Atur lebar minimum untuk kolom
            If column.Width < 100 Then 'jika lebar kolom kurang dari 100 (sesuaikan dengan lebar minimal yang Anda inginkan)
                column.Width = 100 'mengatur lebar kolom menjadi 100 (ganti dengan lebar minimal yang Anda inginkan jika lebar saat ini kurang dari lebar minimal)
            End If 'mengakhiri blok if
        Next 'mengakhiri perulangan
    End Sub 'mengakhiri subrutin

    'Tampilkan data pemesanan ke list view
    Private Sub TampilData() 'mendeklarasikan subrutin yang akan menampilkan data pemesanan ke list view 1
        ' Ambil data dari database dengan menggunakan JOIN antara tabel transaction, ticket, dan akun
        da = New MySqlDataAdapter("SELECT t.ticket_idTicket, ti.namaTicket, ti.hargaTicket, ti.fromDest, ti.toDest, t.jumlah, t.total_harga, t.tanggal, a.usernameAkun FROM transaction t JOIN ticket ti ON ti.idTicket = t.ticket_idTicket JOIN akun a ON a.idAkun = t.akun_idAkun WHERE a.usernameAkun ='" & Module1.username & "'", Module1.con) 'membuat objek adapter data yang akan mengambil data dari database dengan query SQL yang menggabungkan tiga tabel berdasarkan id akun dan id tiket
        ds = New DataSet 'membuat objek kumpulan data yang akan menyimpan data dalam memori
        da.Fill(ds, "transaction") 'mengisi kumpulan data dengan data yang diambil dari database dengan nama tabel "transaction"
        dt = ds.Tables("transaction") 'mendeklarasikan objek tabel data yang akan menyimpan data dalam bentuk tabel

        Try 'mencoba menjalankan blok kode berikut
            ' Hapus semua item pada list view sebelum menambahkan yang baru
            ListView1.Items.Clear() 'menghapus semua item dari list view 1

            ' Tambahkan data ke list view
            For Each dr As DataRow In dt.Rows 'melakukan perulangan untuk setiap baris data di tabel data
                ' Membuat item baru di list view
                Dim item As New ListViewItem 'mendeklarasikan objek item yang akan menyimpan item list view

                ' Tambahkan kolom sesuai dengan urutan kolom di ListView
                item = ListView1.Items.Add(dr("ticket_idTicket").ToString()) 'menambahkan item ke list view 1 dengan teks yang diambil dari kolom ticket_idTicket dan dikonversi ke string
                item.SubItems.Add(dr("namaTicket").ToString()) 'menambahkan subitem dengan teks yang diambil dari kolom namaTicket dan dikonversi ke string
                item.SubItems.Add(dr("jumlah").ToString()) 'menambahkan subitem dengan teks yang diambil dari kolom jumlah dan dikonversi ke string
                item.SubItems.Add(dr("hargaTicket").ToString()) 'menambahkan subitem dengan teks yang diambil dari kolom hargaTicket dan dikonversi ke string
                item.SubItems.Add(dr("tanggal").ToString()) 'menambahkan subitem dengan teks yang diambil dari kolom tanggal dan dikonversi ke string
                item.SubItems.Add(dr("fromDest").ToString()) 'menambahkan subitem dengan teks yang diambil dari kolom fromDest dan dikonversi ke string
                item.SubItems.Add(dr("toDest").ToString()) 'menambahkan subitem dengan teks yang diambil dari kolom toDest dan dikonversi ke string
                item.SubItems.Add(dr("usernameAkun").ToString()) 'menambahkan subitem dengan teks yang diambil dari kolom usernameAkun dan dikonversi ke string
                item.SubItems.Add(dr("total_harga").ToString()) 'menambahkan subitem dengan teks yang diambil dari kolom hargaTicket dan dikonversi ke string

                ' Setelah menambahkan data, panggil fungsi untuk memperbarui lebar kolom
                UpdateColumnWidths() 'memanggil subrutin UpdateColumnWidths yang telah didefinisikan sebelumnya
            Next 'mengakhiri perulangan
        Catch ex As Exception 'jika terjadi kesalahan saat menjalankan blok kode di atas
            MsgBox(ex.Message) 'menampilkan pesan kesalahan
        End Try 'mengakhiri blok try-catch
    End Sub 'mengakhiri subrutin

    'Hapus data pemesanan dari list view dan database
    Private Sub HapusItem() 'mendeklarasikan subrutin yang akan menghapus data pemesanan dari list view 1 dan database
        If ListView1.SelectedItems.Count > 0 Then 'jika ada item yang dipilih di list view 1
            'Ambil data dari list view
            Dim id As String = ListView1.SelectedItems(0).Text 'mendeklarasikan variabel id yang menyimpan teks dari item yang dipilih (id tiket)

            'Hapus data dari database
            Try 'mencoba menjalankan blok kode berikut
                Dim sql As String = "DELETE FROM transaction WHERE " &
                    "akun_idAkun = (SELECT idAkun FROM akun WHERE usernameAkun = @username) AND " &
                    "ticket_idTicket = @id" 'mendeklarasikan variabel sql yang menyimpan query SQL untuk menghapus data transaksi berdasarkan id akun dan id tiket

                Dim cmd As New MySqlCommand(sql, Module1.con) 'membuat objek perintah MySQL dengan query sql dan koneksi yang telah didefinisikan di modul
                cmd.Parameters.AddWithValue("@id", id) 'menambahkan nilai parameter @id dengan variabel id
                cmd.Parameters.AddWithValue("@username", Module1.username) 'menambahkan nilai parameter @username dengan variabel username yang telah didefinisikan di modul

                cmd.ExecuteNonQuery() 'mengeksekusi perintah

                MsgBox("Data pemesanan berhasil dihapus") 'menampilkan pesan informasi bahwa data pemesanan berhasil dihapus
                ListView1.Items.Remove(ListView1.SelectedItems(0)) 'menghapus item yang dipilih dari list view 1

            Catch ex As Exception 'jika terjadi kesalahan saat menjalankan blok kode di atas
                MsgBox("Gagal menghapus data pemesanan. " & ex.Message) 'menampilkan pesan kesalahan
            End Try 'mengakhiri blok try-catch
        Else 'jika tidak ada item yang dipilih di list view 1
            MsgBox("Pilih item yang ingin dihapus.") 'menampilkan pesan peringatan
        End If 'mengakhiri blok if
    End Sub 'mengakhiri subrutin

    Private Sub HapusToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HapusToolStripMenuItem.Click 'mendefinisikan subrutin yang akan dijalankan saat context menu Hapus diklik
        If ListView1.SelectedItems.Count > 0 Then 'jika ada item yang dipilih di list view 1

            Try 'mencoba menjalankan blok kode berikut
                HapusItem() 'memanggil subrutin HapusItem yang telah didefinisikan sebelumnya
            Catch ex As Exception 'jika terjadi kesalahan saat menjalankan blok kode di atas
                MsgBox(ex.Message) 'menampilkan pesan kesalahan
            End Try 'mengakhiri blok try-catch

            TextBox1.Clear() 'menghapus semua teks dalam TextBox1
            TextBox2.Clear() 'menghapus semua teks dalam TextBox2
            TextBox3.Clear() 'menghapus semua teks dalam TextBox3
            TextBox4.Clear() 'menghapus semua teks dalam TextBox4
            TextBox5.Clear() 'menghapus semua teks dalam TextBox5
            RadioButton1.Checked = False 'mengatur RadioButton1 menjadi tidak dicentang
            RadioButton2.Checked = False 'mengatur RadioButton2 menjadi tidak dicentang
            CheckBox1.Checked = False 'mengatur CheckBox1 menjadi tidak dicentang
        Else 'jika tidak ada item yang dipilih di list view 1
            MessageBox.Show("Pilih Kolom ID Tiket yang ingin dihapus pada ListView.") 'menampilkan pesan peringatan
        End If 'mengakhiri blok if
    End Sub 'mengakhiri subrutin

    ' Event handler untuk TimerPopup (menampilkan pesan dengan hitungan mundur)
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick 'mendefinisikan subrutin yang akan dijalankan saat Timer1 berdetak
        countdownSeconds -= 1 'mengurangi variabel countdownSeconds dengan satu

        If countdownSeconds = 0 Then 'jika countdownSeconds sama dengan nol
            Timer1.Stop() 'menghentikan Timer1
            Timer1.Enabled = False 'mengatur Timer1 menjadi tidak aktif
            Me.Close() 'menutup form
        End If 'mengakhiri blok if
    End Sub 'mengakhiri subrutin
End Class 'mengakhiri kelas