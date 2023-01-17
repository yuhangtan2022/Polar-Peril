Public Class MainForm
    Dim nStuff As spriteType
    Dim ladders(8) As PictureBox
    Dim fStuff(6) As floorType
    Dim sBalls(3) As PictureBox
    Dim bStuff(3) As spriteType
    Dim throwTimer As Integer
    Dim bobDancing As Boolean
    Dim helpTimer As Integer
    Dim lives(2) As PictureBox
    Dim livesLeft As Integer
    Dim HasHammer As Boolean
    Dim hammerTick As Integer
    Dim useHammer As Boolean
    Dim explode As PictureBox
    Dim explodes As List(Of Image)
    Dim explodeAnimate As animation
    Private Sub MainForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.H Then
            If useHammer = True And nStuff.onLadder = False Then
                HasHammer = True
                useHammer = False
            End If
        End If
        If e.KeyCode = Keys.Left Or e.KeyCode = Keys.A Then
            If Nansen.Left > 80 Then
                nStuff.speed.X = -5
            Else
                nStuff.speed.X = 0
            End If
            nStuff.faceRight = False
        End If
        If e.KeyCode = Keys.Right Or e.KeyCode = Keys.D Then
            If Nansen.Left < 560 Then
                nStuff.speed.X = 5
            Else
                nStuff.speed.X = 0
            End If
            nStuff.faceRight = True
        End If
        If HasHammer = False Then
            If e.KeyCode = Keys.Space And nStuff.speed.Y = 0 And nStuff.onFloor = True Then
                nStuff.onFloor = False
                nStuff.floatTime = 0
                nStuff.speed.Y = -5
                nStuff.floating = True
            End If
            If (e.KeyCode = Keys.Up Or e.KeyCode = Keys.W) And nStuff.floating = False Then
                nStuff.speed.Y = -5
            ElseIf (e.KeyCode = Keys.Down Or e.KeyCode = Keys.S) And nStuff.floating = False Then
                nStuff.speed.Y = 5
            End If
        End If
    End Sub

    Private Sub MainForm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Left Or e.KeyCode = Keys.A Or e.KeyCode = Keys.Right Or e.KeyCode = Keys.D Then
            nStuff.speed.X = 0
        End If
        If e.KeyCode = Keys.Up Or e.KeyCode = Keys.W Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.S Then
            If nStuff.floating = False Then
                nStuff.speed.Y = 0
            End If
        End If
    End Sub

    Private Sub MainForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        welcome.ShowDialog()
        quit = False
        resetEvery()
        explodes = New List(Of Image)
        explodes.Add(Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\explode1.jpg"))
        explodes.Add(Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\explode2.jpg"))
        explodes.Add(Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\explode3.jpg"))
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        If quit = True Then
            quit = False
            Timer1.Stop()
            welcome.ShowDialog()
            resetEvery()
            Timer1.Start()
        Else
            If touching(Nansen, penguin) Then
                If HasHammer = False Then
                    Timer1.Stop()
                    winForm.ShowDialog()
                    resetEvery()
                    Timer1.Start()
                End If
            End If
            hammerr.Visible = useHammer
            If HasHammer Then
                hammerTick += 1
                If hammerTick = 80 Then
                    Timer2.Start()
                    HasHammer = False
                    hammerTick = 0
                End If
            End If
            If touching(Nansen, bob) Then
                If HasHammer = True Then
                    Timer1.Stop()
                    bob.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\BobDancing.jpg")
                    bobExplode.Visible = True
                    explodeAnimate = New animation(bobExplode, explodes)
                    Timer4.Start()
                    Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenLeftSmash2.jpg")
                    winForm.ShowDialog()
                    resetEvery()
                    Timer1.Start()
                Else
                    livesLeft = 0
                    killNansen(0)
                End If
            End If
            MoveNansen()
            animateNansen()
            animateBob()
            throwBall()
            flashHelp()
            For bbindex = 0 To 3
                If sBalls(bbindex).Visible = True Then
                    moveSball(bbindex)
                    animateSball(bbindex)
                    If touching(Nansen, sBalls(bbindex)) Then
                        If HasHammer Then
                            explode = sBalls(bbindex)
                            explodeAnimate = New animation(sBalls(bbindex), explodes)
                            Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenSpin.jpg")
                            Timer1.Stop()
                            Timer3.Start()
                            Timer4.Start()
                        Else
                            killNansen(bbindex)
                        End If
                    End If
                End If
            Next bbindex
        End If

    End Sub
    Private Sub MoveNansen()
        nStuff.floorNum = getFloorNum(Nansen)
        If nStuff.floating = True Then
            nansenJumping()
        Else
            nStuff.onLadder = checkLadder()
            If nStuff.onLadder = False Then
                moveAlongFloor(nStuff, Nansen)
            Else
                nStuff.speed.X = 0
            End If
        End If
        If (nStuff.speed.X < 0 And Nansen.Left < 85) Or (nStuff.speed.X > 0 And Nansen.Left > 540) Then
            nStuff.speed.X = 0
        End If
        Nansen.Left += nStuff.speed.X
        Nansen.Top += nStuff.speed.Y
    End Sub
    Private Sub animateNansen()
        If nStuff.speed.Y <> 0 Then
            If nStuff.floating = True Then
                animateJump()
            Else
                animateClimb()
            End If
        ElseIf nStuff.onLadder = False Then
            If nStuff.speed.X < 0 Then
                animateLeft()
            ElseIf nStuff.speed.X > 0 Then
                animateRight()
            Else
                animateStanding()
            End If
        End If
    End Sub
    Private Sub animateLeft()
        Select Case nStuff.picNum
            Case 0
                If HasHammer Then
                    nStuff.picNum = 1
                    Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenLeftSmash1.jpg")
                Else
                    nStuff.picNum = 1
                    Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenLeftMove1.jpg")
                End If
            Case 1
                If HasHammer Then
                    nStuff.picNum = 0
                    Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenLeftSmash2.jpg")
                Else
                    nStuff.picNum = 2
                    Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenLeftMove2.jpg")
                End If
            Case 2
                nStuff.picNum = 0
                Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenLeftMove3.jpg")
        End Select
    End Sub
    Private Sub animateRight()
        Select Case nStuff.picNum
            Case 0
                If HasHammer Then
                    nStuff.picNum = 1
                    Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenrightSmash1.jpg")
                Else
                    nStuff.picNum = 1
                    Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenRightMove1.jpg")
                End If
            Case 1
                If HasHammer Then
                    nStuff.picNum = 0
                    Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenrightSmash2.jpg")
                Else
                    nStuff.picNum = 2
                    Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenRightMove2.jpg")
                End If
            Case 2
                nStuff.picNum = 0
                Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenRightMove3.jpg")
        End Select
    End Sub
    Private Sub animateJump()
        If nStuff.speed.X > 0 And nStuff.faceRight = True Then
            Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenRightFloat.jpg")
        ElseIf nStuff.speed.X < 0 And nStuff.faceRight = False Then
            Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenLeftFloat.jpg")
        End If
    End Sub
    Private Sub animateStanding()
        If nStuff.faceRight = True Then
            If HasHammer Then
                Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenrightSmash1.jpg")
            Else
                Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenRightMove1.jpg")
            End If
        Else
            If HasHammer Then
                Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenleftSmash1.jpg")
            Else
                Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenLeftMove1.jpg")
            End If
        End If
    End Sub
    Private Sub animateClimb()
        Select Case nStuff.picNum
            Case 0
                nStuff.picNum = 1
                Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenClimb1.jpg")
            Case 1
                nStuff.picNum = 2
                Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenClimb2.jpg")
            Case 2
                nStuff.picNum = 0
                Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenClimb3.jpg")
        End Select
    End Sub
    Private Function checkLadder()
        If HasHammer Then
            Return False
        End If
        Dim offSet As Integer
        offSet = 15
        For lindex = 0 To 8
            If nStuff.speed.Y < 0 Then
                If Nansen.Left > ladders(lindex).Left - offSet And Nansen.Right < ladders(lindex).Right + offSet Then
                    If Nansen.Top < ladders(lindex).Bottom And Nansen.Bottom > ladders(lindex).Top Then
                        nStuff.ladderNum = lindex
                        Return True
                    End If
                End If
            ElseIf nStuff.speed.Y > 0 Then
                If Nansen.Left > ladders(lindex).Left - offSet And Nansen.Right < ladders(lindex).Right + offSet Then
                    If Nansen.Bottom < ladders(lindex).Bottom And Nansen.Bottom > ladders(lindex).Top - offSet Then
                        nStuff.ladderNum = lindex
                        Return True
                    End If
                End If
            ElseIf nStuff.speed.Y = 0 Then
                If Nansen.Left > ladders(lindex).Left - offSet And Nansen.Right < ladders(lindex).Right + offSet Then
                    If Nansen.Bottom < ladders(lindex).Bottom - offSet And Nansen.Bottom > ladders(lindex).Top + offSet Then
                        nStuff.ladderNum = lindex
                        Return True
                    End If
                End If
            End If
        Next lindex
        Return False
    End Function
    Private Function touching(ByVal object1 As PictureBox, ByVal object2 As PictureBox)
        If object1.Right > object2.Left And object1.Left < object2.Right Then
            If object1.Top < object2.Bottom And object1.Bottom > object2.Top Then
                Return True
            End If
        End If
        Return False
    End Function
    Public Sub FloorSet()
        fStuff(0).slope = -0.00001
        fStuff(1).slope = 0.078
        fStuff(2).slope = -0.078
        fStuff(3).slope = 0.078
        fStuff(4).slope = -0.078
        fStuff(5).slope = 0
        fStuff(6).slope = 0


        fStuff(0).x = 137
        fStuff(1).x = 137
        fStuff(2).x = 137
        fStuff(3).x = 137
        fStuff(4).x = 137
        fStuff(5).x = 137
        fStuff(6).x = 137

        fStuff(0).y = 465
        fStuff(1).y = 377
        fStuff(2).y = 327
        fStuff(3).y = 226
        fStuff(4).y = 176
        fStuff(5).y = 92
        fStuff(6).y = 42

        fStuff(0).leftEdge = 0
        fStuff(1).leftEdge = 0
        fStuff(2).leftEdge = 133
        fStuff(3).leftEdge = 0
        fStuff(4).leftEdge = 133
        fStuff(5).leftEdge = 0
        fStuff(6).leftEdge = 200

        fStuff(0).rightEdge = 570
        fStuff(1).rightEdge = 507
        fStuff(2).rightEdge = 570
        fStuff(3).rightEdge = 507
        fStuff(4).rightEdge = 570
        fStuff(5).rightEdge = 507
        fStuff(6).rightEdge = 312
    End Sub
    Private Function getFloorNum(ByVal stuff As PictureBox)
        If stuff.Top > 393 Then
            Return 0
        ElseIf stuff.Top > 320 Then
            Return 1
        ElseIf stuff.Top > 240 Then
            Return 2
        ElseIf stuff.Top > 160 Then
            Return 3
        ElseIf stuff.Top > 73 Then
            Return 4
        ElseIf stuff.Top > 20 Then
            Return 5
        Else
            Return 6
        End If
    End Function
    Private Sub jumping(ByRef stuff As spriteType)
        If stuff.floatTime = 7 Then
            If stuff.speed.Y = -5 Then
                stuff.speed.Y = 5
            Else
                stuff.speed.Y = 0
                stuff.floating = False
            End If
            stuff.floatTime = 1
        Else
            stuff.floatTime += 1
        End If
    End Sub
    Private Sub moveAlongFloor(ByRef stuff As spriteType, ByRef pBox As PictureBox)
        stuff.speed.Y = 0
        stuff.onFloor = True
        pBox.Top = fStuff(stuff.floorNum).slope * (pBox.Left - fStuff(stuff.floorNum).x) + fStuff(stuff.floorNum).y - pBox.Height
        If pBox.Left > fStuff(stuff.floorNum).rightEdge Then
            stuff.floating = True
            stuff.floatTime = 1
            stuff.speed.Y = 5
        ElseIf pBox.Right < fStuff(stuff.floorNum).leftEdge Then
            stuff.floating = True
            stuff.floatTime = 1
            stuff.speed.Y = 5
        End If
    End Sub
    Private Sub sBallSet()
        sBalls(0) = sBall0
        sBalls(1) = sBall1
        sBalls(2) = sBall2
        sBalls(3) = sBall3
        For bindex = 0 To 3
            sBalls(bindex).Visible = False
            sBalls(bindex).Top = 70
            sBalls(bindex).Left = 203
            bStuff(bindex).floating = False
            bStuff(bindex).onLadder = False
            bStuff(bindex).picNum = 0
            bStuff(bindex).speed.X = 10
            bStuff(bindex).speed.Y = 0
        Next bindex
    End Sub
    Private Sub moveSball(ByVal bindex As Integer)
        bStuff(bindex).floorNum = getFloorNum(sBalls(bindex))
        If bStuff(bindex).floating = True Then
            jumping(bStuff(bindex))
        Else
            bStuff(bindex).onLadder = ballCheckLadder(bindex)
            If bStuff(bindex).onLadder = False Then
                If fStuff(bStuff(bindex).floorNum).slope < 0 Then
                    bStuff(bindex).speed.X = -10
                Else
                    bStuff(bindex).speed.X = 10
                End If
                moveAlongFloor(bStuff(bindex), sBalls(bindex))
            Else
                bStuff(bindex).speed.X = 0
                bStuff(bindex).speed.Y = 10
                bStuff(bindex).floating = True
            End If
        End If
        If (bStuff(bindex).speed.X < 0 And sBalls(bindex).Left < 85) Or (bStuff(bindex).speed.X > 0 And sBalls(bindex).Left > 540) Then
            bStuff(bindex).speed.X = 0
        End If
        If bStuff(bindex).floorNum = 0 And sBalls(bindex).Left < 88 Then
            sBalls(bindex).Visible = False
            If bobDancing = True Then
                bobDancing = False
                throwTimer = 0
                bob.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\BobStanding.jpg")
            End If
        End If
        sBalls(bindex).Left += bStuff(bindex).speed.X
        sBalls(bindex).Top += bStuff(bindex).speed.Y
    End Sub
    Private Function ballCheckLadder(ByVal bindex As Integer)
        Randomize()
        Dim offset = 13
        If CInt(Math.Floor(Rnd() * 6)) = 0 Then
            For llindex = 0 To 8
                If sBalls(bindex).Left > ladders(llindex).Left And sBalls(bindex).Right < ladders(llindex).Right Then
                    If sBalls(bindex).Bottom + offset > ladders(llindex).Top And sBalls(bindex).Bottom - offset < ladders(llindex).Top Then
                        Return True
                    End If
                End If
            Next llindex
        End If
        Return False
    End Function
    Private Sub animateSball(ByVal bindex As Integer)
        Select Case bStuff(bindex).picNum
            Case 0
                bStuff(bindex).picNum = 1
                sBalls(bindex).Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\ball1.jpg")
            Case 1
                bStuff(bindex).picNum = 2
                sBalls(bindex).Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\ball2.jpg")
            Case 2
                bStuff(bindex).picNum = 3
                sBalls(bindex).Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\ball3.jpg")
            Case 3
                bStuff(bindex).picNum = 0
                sBalls(bindex).Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\ball4.jpg")
        End Select
    End Sub
    Private Sub resetLevel()
        Timer4.Stop()
        bobExplode.Visible = False
        hammerTick = 0
        useHammer = True
        HasHammer = False
        bobDancing = False
        nansenSet()
        ladderSet()
        FloorSet()
        sBallSet()
    End Sub
    Private Sub nansenSet()
        lives(0) = life0
        lives(1) = life1
        lives(2) = life2
        nStuff.picNum = 0
        nStuff.faceRight = True
        nStuff.speed.X = 0
        nStuff.speed.Y = 0
        nStuff.onFloor = True
        nStuff.onLadder = False
        Nansen.Top = 430
        Nansen.Left = 104
        Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenRightMove1.jpg")
    End Sub
    Private Sub ladderSet()
        ladders(0) = ladder0
        ladders(1) = ladder1
        ladders(2) = ladder2
        ladders(3) = ladder3
        ladders(4) = ladder4
        ladders(5) = ladder5
        ladders(6) = ladder6
        ladders(7) = ladder7
        ladders(8) = ladder8
    End Sub
    Private Sub throwBall()
        Dim done As Boolean
        Dim index As Integer
        throwTimer += 1
        If throwTimer = 32 Then
            throwTimer = 0
            done = False
            Do While done = False
                If sBalls(index).Visible = False Then
                    done = True
                    sBalls(index).Visible = True
                    sBalls(index).Top = 70
                    sBalls(index).Left = 203
                    bStuff(index).picNum = 0
                    bStuff(index).floating = False
                    bStuff(index).onLadder = False
                    bStuff(index).speed.X = 10
                    bStuff(index).speed.Y = 0
                    bobDancing = True
                    For sindex = 0 To 3
                        If sBalls(sindex).Visible = False Then
                            bobDancing = False
                        End If
                    Next sindex
                End If
                index += 1
                If index = 4 Then
                    done = True
                End If
            Loop
        End If
    End Sub
    Private Sub animateBob()
        If bobDancing = True Then
            Select Case throwTimer
                Case Is > 30
                    bob.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\BobSmiling.jpg")
                Case Is > 22
                    bob.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\BobDancing2.jpg")
                Case Is > 14
                    bob.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\BobDancing.jpg")
                Case Is > 8
                    bob.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\BobStanding.jpg")
            End Select
        Else
            Select Case throwTimer
                Case Is > 30
                    bob.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\BobRollingBall.jpg")
                Case Is > 15
                    bob.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\BobHoldingBall.jpg")
                Case Is > 7
                    bob.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\BobGettingBall.jpg")
            End Select
        End If
    End Sub
    Private Sub flashHelp()
        If helpTimer > 3 Then
            helpSign.Visible = Not helpSign.Visible
            helpTimer = 0
        End If
        helpTimer += 1
    End Sub
    Private Sub resetEvery()
        resetLevel()
        c.Visible = False
        r.Visible = False
        q.Visible = False
        livesLeft = 2
        For vindex = 0 To 2
            lives(vindex).Visible = True
        Next vindex
    End Sub
    Private Sub killNansen(ByVal bbindex As Integer)
        If livesLeft > 0 Then
            Timer1.Stop()
            sBalls(bbindex).Visible = False
            lives(livesLeft).Visible = False
            livesLeft -= 1
            If nStuff.onLadder = True Then
                Nansen.Top = ladders(nStuff.ladderNum).Bottom - Nansen.Height
            Else
                Nansen.Top = fStuff(nStuff.floorNum).slope * (Nansen.Left - fStuff(nStuff.floorNum).x) + fStuff(nStuff.floorNum).y - Nansen.Height
            End If
            Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenDead.jpg")
            NansenDeath.ShowDialog()
            resetLevel()
            Timer1.Start()
        Else
            Timer1.Stop()
            sBalls(bbindex).Visible = False
            If nStuff.onLadder = True Then
                Nansen.Top = ladders(nStuff.ladderNum).Bottom - Nansen.Height
            Else
                Nansen.Top = fStuff(nStuff.floorNum).slope * (Nansen.Left - fStuff(nStuff.floorNum).x) + fStuff(nStuff.floorNum).y - Nansen.Height
            End If
            Nansen.Image = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\nansenDead.jpg")
            GAMEOVER.ShowDialog()
            resetEvery()
            Timer1.Start()
        End If
    End Sub
    Private Sub hammer()
        
    End Sub
    Private Sub nansenJumping()
        If nStuff.floatTime = 6 Then
            If nStuff.speed.Y = -5 Then
                nStuff.speed.Y = 5
            Else
                nStuff.speed.Y = 0
                nStuff.floating = False
            End If
            nStuff.floatTime = 1
        Else
            nStuff.floatTime += 1
        End If
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        useHammer = True
        Timer2.Stop()
    End Sub

    Private Sub Timer3_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer3.Tick
        Timer3.Stop()
        Timer4.Stop()
        Timer1.Start()
        explode.Visible = False
    End Sub

    Private Sub Timer4_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer4.Tick
        explodeAnimate.animate()
    End Sub

    Private Sub PictureBox43_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pb.Click
        Timer1.Enabled = Not Timer1.Enabled
        If Timer1.Enabled = False Then
            c.Visible = True
            r.Visible = True
            q.Visible = True
        Else
            c.Visible = False
            r.Visible = False
            q.Visible = False
        End If
    End Sub

    Private Sub c_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles c.Click
        Timer1.Start()
    End Sub

    Private Sub r_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles r.Click
        resetEvery()
        Timer1.Start()
    End Sub

    Private Sub q_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles q.Click
        quit = True
        Timer1.Start()
    End Sub
End Class