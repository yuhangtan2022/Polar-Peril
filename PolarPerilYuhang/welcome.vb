Public Class welcome
    Dim explodes As List(Of Image)
    Dim anim As animation
    Dim anima As animation
    Dim sfs(9) As PictureBox
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Hide()
    End Sub

    Private Sub welcome_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sfs(0) = sf0
        sfs(1) = sf1
        sfs(2) = sf2
        sfs(3) = sf3
        sfs(4) = sf4
        sfs(5) = sf5
        sfs(6) = sf6
        sfs(7) = sf7
        sfs(8) = sf8
        sfs(9) = sf9
        explodes = New List(Of Image)
        explodes.Add(Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\explode1.jpg"))
        explodes.Add(Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\explode2.jpg"))
        explodes.Add(Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\explode3.jpg"))
        anim = New animation(PictureBox3, explodes)
        anima = New animation(PictureBox1, explodes)
        Timer1.Start()
        Timer2.Start()
    End Sub
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        anim.animate()
        anima.animate()
    End Sub
    Private Sub MoveDown(ByVal guy As PictureBox, ByVal speed As Integer)
        If guy.Top > 300 Then
            guy.Top = 0 - guy.Height
        Else
            guy.Top = guy.Top + speed
        End If
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        For sindex = 0 To 9
            MoveDown(sfs(sindex), 5)
        Next sindex
    End Sub
End Class