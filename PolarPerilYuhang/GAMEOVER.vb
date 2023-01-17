Public Class GAMEOVER
    Public anim As List(Of Image)
    Dim ani As animation
    Private Sub GAMEOVER_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        My.Computer.Audio.Play(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\evil.wav", AudioPlayMode.Background)
        anim = New List(Of Image)
        anim.Add(Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\BobDancing.jpg"))
        anim.Add(Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\BobDancing2.jpg"))
        ani = New animation(PictureBox2, anim)
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        ani.animate()
        PictureBox3.Visible = Not PictureBox3.Visible
    End Sub
End Class