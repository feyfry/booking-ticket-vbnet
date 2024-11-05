Public Class Awal1 ' Mendefinisikan kelas Awal1
    Private Sub Awal1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.IsMdiContainer = True ' Mengatur form ini sebagai container untuk multiple document interface
        ' Mengatur judul form dengan teks "Selamat Datang!"
        Me.Text = "Muhammad Faiz Aqil Fathoni (2021230006)"
        Form3.Show()
    End Sub

    ' Menampilkan form Login saat item menu LoginToolStripMenuItem diklik
    Private Sub AppToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AppToolStripMenuItem.Click
        Form3.Show() ' Menampilkan form Login saat item menu LoginToolStripMenuItem diklik
    End Sub

    '  Menampilkan form Register saat item menu RegisterToolStripMenuItem diklik
    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Application.Exit() ' Menampilkan form Register saat item menu RegisterToolStripMenuItem diklik
    End Sub
End Class