Public Class NansenDeath

    Private Sub PictureBox4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox4.Click

    End Sub

    Private Sub CONTINE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CONTINE.Click
        Me.Hide()
    End Sub

    Private Sub quitee_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles quitee.Click
        quit = True
        Me.Hide()
    End Sub
End Class