Public Class animation
    Dim current As Integer
    Dim pics As List(Of Image)
    Dim box As PictureBox
    Public Sub animate()
        If current = pics.IndexOf(pics.Last) Then
            current = 0
        Else
            current += 1
        End If
        box.Image = pics.ElementAt(current)
    End Sub
    Public Sub New(ByRef picBox As PictureBox, ByRef picsL As List(Of Image))
        current = 0
        box = picBox
        pics = picsL
    End Sub
End Class
