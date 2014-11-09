Imports Vestris.ResourceLib

Public Class frmMain
  Private imageresDLL As String = Environment.GetFolderPath(Environment.SpecialFolder.System) & "\imageres.dll"
  Private TempDir As String = My.Computer.FileSystem.SpecialDirectories.Temp & "\welcomeUP\"

  Private Sub frmMain_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
    If My.Computer.FileSystem.FileExists(imageresDLL & ".bak") Then
      cmdRevert.Enabled = True
    Else
      cmdRevert.Enabled = False
    End If
    If My.Computer.FileSystem.FileExists(imageresDLL & ".del") Then
      Try
        My.Computer.FileSystem.DeleteFile(imageresDLL & ".del")
      Catch ex As Exception
      End Try
    End If
    If My.Computer.FileSystem.FileExists(imageresDLL & ".del") Then cmdRevert.Enabled = False
    If My.Computer.FileSystem.DirectoryExists(TempDir) Then My.Computer.FileSystem.DeleteDirectory(TempDir, FileIO.DeleteDirectoryOption.DeleteAllContents)
    Do While My.Computer.FileSystem.DirectoryExists(TempDir)
      Application.DoEvents()
      Threading.Thread.Sleep(1)
    Loop
    My.Computer.FileSystem.CreateDirectory(TempDir)
    Try
      txtStartupSound.Text = "Loading..."
      Application.DoEvents()
      Using rI As New ResourceInfo
        rI.Load(imageresDLL)
        For Each rID As ResourceId In rI.ResourceTypes
          For Each res As Resource In rI.Resources(rID)
            If res.TypeName = "IMAGE" And res.Name.Name = "5033" Then
              My.Computer.FileSystem.WriteAllBytes(TempDir & "welcome.jpg", res.WriteAndGetBytes, False)
              pctWelcome.Image = Image.FromFile(TempDir & "welcome.jpg")
              pctWelcome.Tag = "EMBEDDED IMAGE"
            End If
            If res.TypeName = "WAVE" And res.Name.Name = "5080" Then
              My.Computer.FileSystem.WriteAllBytes(TempDir & "welcome.wav", res.WriteAndGetBytes, False)
              txtStartupSound.Text = "EMBEDDED SOUND"
            End If
          Next
        Next
        rI.Unload()
      End Using
      If txtStartupSound.Text = "Loading..." Then txtStartupSound.Text = ""
    Catch ex As Exception
      MsgBox("Unable to read ImageRes DLL File. Please make sure it's not in use!" & vbNewLine & ex.Message, MsgBoxStyle.Critical, "DLL Read Error!")
      HideUI()
      Exit Sub
    End Try
  End Sub

  Private Sub cmdPlay_Click(sender As System.Object, e As System.EventArgs) Handles cmdPlay.Click
    If txtStartupSound.Text = "EMBEDDED SOUND" Then
      If My.Computer.FileSystem.FileExists(TempDir & "\welcome.wav") Then
        Try
          My.Computer.Audio.Play(TempDir & "\welcome.wav", AudioPlayMode.Background)
        Catch ex As Exception
          MsgBox("The Alert sound file failed to play!" & vbNewLine & ex.Message, MsgBoxStyle.Exclamation, "Alert File Error")
        End Try
      Else
        MsgBox("The Alert sound file was not found!" & vbNewLine & "It may not have been extracted during startup.", MsgBoxStyle.Exclamation, "Alert File Missing")
      End If
    Else
      Try
        My.Computer.Audio.Play(txtStartupSound.Text, AudioPlayMode.Background)
      Catch ex As Exception
        MsgBox("The Alert sound file failed to play!" & vbNewLine & ex.Message, MsgBoxStyle.Exclamation, "Alert File Error")
      End Try
    End If
  End Sub

  Private Sub cmdWelcomeBrowse_Click(sender As System.Object, e As System.EventArgs) Handles cmdWelcomeBrowse.Click
    Using cdlWelcome As New OpenFileDialog
      cdlWelcome.Filter = "Welcome Screen Image|*.jpg"
      cdlWelcome.Title = "Select Welcome Screen"
      If cdlWelcome.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        pctWelcome.Tag = cdlWelcome.FileName
        pctWelcome.Image = Image.FromFile(pctWelcome.Tag)
      End If
    End Using
  End Sub

  Private Sub cmdStartupBrowse_Click(sender As System.Object, e As System.EventArgs) Handles cmdStartupBrowse.Click
    Using cdlWelcome As New OpenFileDialog
      cdlWelcome.Filter = "Welcome Sound Effect|*.wav"
      cdlWelcome.Title = "Select Welcome Sound"
      If cdlWelcome.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        txtStartupSound.Text = cdlWelcome.FileName
      End If
    End Using
  End Sub

  Private Sub cmdRevert_Click(sender As System.Object, e As System.EventArgs) Handles cmdRevert.Click
    If My.Computer.FileSystem.FileExists(imageresDLL & ".del") Then
      MsgBox("ImageRes DLL to delete is still in use. Unable to delete another one!" & vbNewLine & "Please Restart your computer before making any further changes.", MsgBoxStyle.Critical, "Restart Your Computer")
    Else
      If MsgBox("Are you sure you want to revert your Welcome Screen and Startup Sound?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "Revert Welcome?") = MsgBoxResult.Yes Then
        IO.File.Move(imageresDLL, imageresDLL & ".del")
        IO.File.Move(imageresDLL & ".bak", imageresDLL)
        cmdRevert.Enabled = False
      End If
    End If
  End Sub

  Private Sub cmdSave_Click(sender As System.Object, e As System.EventArgs) Handles cmdSave.Click
    Dim Changed As Boolean = False
    Dim DoChange As Boolean = False
    HideUI()
    If Not pctWelcome.Tag = "EMBEDDED IMAGE" Then DoChange = True
    If Not txtStartupSound.Text = "EMBEDDED SOUND" Then DoChange = True
    If DoChange Then
      GrantFullControlToEveryone(imageresDLL)
      If Not My.Computer.FileSystem.FileExists(imageresDLL & ".bak") Then
        lblActivity.Text = "Backing up DLL"
        Application.DoEvents()
        Try
          IO.File.Move(imageresDLL, imageresDLL & ".bak")
          IO.File.Copy(imageresDLL & ".bak", imageresDLL, True)
        Catch ex As Exception
          MsgBox("Unable to backup ImageRes DLL File. Please make sure it's not in use!" & vbNewLine & ex.Message, MsgBoxStyle.Critical, "DLL Backup Error!")
          ShowUI()
          Exit Sub
        End Try
      End If
      If My.Computer.FileSystem.FileExists(imageresDLL & ".active") Then
        Try
          IO.File.Delete(imageresDLL & ".active")
        Catch ex As Exception

        End Try
      End If
      If Not pctWelcome.Tag = "EMBEDDED IMAGE" Then
        lblActivity.Text = "Loading Images"
        Application.DoEvents()
        Dim gRes As New List(Of GenericResource)
        Try
          Using rI As New ResourceInfo
            rI.Load(imageresDLL)
            For Each rID As ResourceId In rI.ResourceTypes
              If rID.Name = "IMAGE" Then
                For Each res As Resource In rI.Resources(rID)
                  gRes.Add(DirectCast(res, GenericResource))
                Next
                Exit For
              End If
            Next
            rI.Unload()
          End Using
        Catch ex As Exception
          MsgBox("Unable to read ImageRes DLL File. Please make sure it's not in use!" & vbNewLine & ex.Message, MsgBoxStyle.Critical, "DLL Read Error!")
          ShowUI()
          Exit Sub
        End Try
        Application.DoEvents()

        Try
          IO.File.Move(imageresDLL, imageresDLL & ".active")
          IO.File.Copy(imageresDLL & ".active", imageresDLL, True)
        Catch ex As Exception
          MsgBox("Unable to backup ImageRes DLL File. Please make sure it's not in use!" & vbNewLine & ex.Message, MsgBoxStyle.Critical, "DLL Backup Error!")
          ShowUI()
          Exit Sub
        End Try

        For I As Integer = 0 To gRes.Count - 1
          lblActivity.Text = "Scaling Image " & (I + 1) & "/" & gRes.Count
          Application.DoEvents()
          Dim myDataPath As String = TempDir & "Data_" & (I + 1) & ".jpg"
          My.Computer.FileSystem.WriteAllBytes(myDataPath, gRes(I).Data, False)
          Dim resOut As Size = Nothing
          Using rData As Image = Image.FromFile(myDataPath)
            resOut = rData.Size
          End Using
          Dim imgIn As Image = Image.FromFile(pctWelcome.Tag)
          Dim pFMT As Imaging.PixelFormat = Imaging.PixelFormat.Format24bppRgb
          If I > 12 Then pFMT = Imaging.PixelFormat.Format1bppIndexed
          Dim bmpOut As New Bitmap(resOut.Width, resOut.Height, Imaging.PixelFormat.Format24bppRgb)
          Dim newX As Integer
          Dim newW As Integer
          newW = imgIn.Width / imgIn.Height * resOut.Height
          newX = -1 * ((newW - resOut.Width) / 2)
          Using g As Graphics = Graphics.FromImage(bmpOut)
            g.DrawImage(imgIn, newX, 0, newW, resOut.Height)
          End Using
          My.Computer.FileSystem.DeleteFile(myDataPath)
          If I > 12 Then
            GrayBMP_File.CreateGrayBitmapFile(bmpOut, myDataPath)
          Else
            bmpOut.Save(myDataPath)
          End If
          gRes(I).Data = My.Computer.FileSystem.ReadAllBytes(myDataPath)
          lblActivity.Text = "Saving Image " & (I + 1) & "/" & gRes.Count
          Application.DoEvents()
          Try
            gRes(I).SaveTo(imageresDLL)
          Catch ex As Exception
            MsgBox("Unable to save ImageRes DLL File. Please make sure it's not in use!" & vbNewLine & ex.Message, MsgBoxStyle.Critical, "DLL Save Error!")
            ShowUI()
            Exit Sub
          End Try
        Next
        Try
          IO.File.Delete(imageresDLL & ".active")
        Catch ex As Exception

        End Try
        Changed = True
      End If
      If Not txtStartupSound.Text = "EMBEDDED SOUND" Then
        lblActivity.Text = "Loading Sound"
        Application.DoEvents()
        Dim gRes As GenericResource = Nothing
        Try
          Using rI As New ResourceInfo
            rI.Load(imageresDLL)
            For Each rID As ResourceId In rI.ResourceTypes
              If rID.Name = "WAVE" Then
                'wavRES = rID
                For Each res As Resource In rI.Resources(rID)
                  If res.Name.Name = "5080" Then
                    gRes = res
                    gRes.Data = My.Computer.FileSystem.ReadAllBytes(txtStartupSound.Text)
                    Exit For
                  End If
                Next
                Exit For
              End If
            Next
            rI.Unload()
          End Using
        Catch ex As Exception
          MsgBox("Unable to read ImageRes DLL File. Please make sure it's not in use!" & vbNewLine & ex.Message, MsgBoxStyle.Critical, "DLL Read Error!")
          ShowUI()
          Exit Sub
        End Try

        Try
          IO.File.Move(imageresDLL, imageresDLL & ".active")
          IO.File.Copy(imageresDLL & ".active", imageresDLL, True)
        Catch ex As Exception
          MsgBox("Unable to backup ImageRes DLL File. Please make sure it's not in use!" & vbNewLine & ex.Message, MsgBoxStyle.Critical, "DLL Backup Error!")
          ShowUI()
          Exit Sub
        End Try

        lblActivity.Text = "Saving Sound"
        Application.DoEvents()
        Try
          gRes.SaveTo(imageresDLL)
        Catch ex As Exception
          MsgBox("Unable to save ImageRes DLL File. Please make sure it's not in use!" & vbNewLine & ex.Message, MsgBoxStyle.Critical, "DLL Save Error!")
          ShowUI()
          Exit Sub
        End Try
        Try
          IO.File.Delete(imageresDLL & ".active")
        Catch ex As Exception

        End Try
        Changed = True
      End If
    End If
    ShowUI()
    Application.DoEvents()
    If Changed Then
      If MsgBox("Changes have been made to your ImageRes DLL. Please Restart your computer before making any other changes." & vbNewLine & vbNewLine & "Would you like to restart now?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "Welcome Updated - Restart?") = MsgBoxResult.Yes Then
        Process.Start("shutdown", "/r /t 0 /d p:0:0 /f")
      End If
    End If
  End Sub

  Private Sub cmdDonate_Click(sender As System.Object, e As System.EventArgs) Handles cmdDonate.Click
    Process.Start("http://realityripple.com/donate.php?itm=Welcome+Updater")
  End Sub

  Private Sub ShowUI()
    lblActivity.Text = ""
    TableLayoutPanel1.SetColumnSpan(lblActivity, 1)
    pnlButtons.Visible = True
    cmdSave.Enabled = True
    cmdRevert.Enabled = True
    cmdPlay.Enabled = True
    cmdStartupBrowse.Enabled = True
    cmdWelcomeBrowse.Enabled = True
    txtStartupSound.Enabled = True
    pctWelcome.Enabled = True
  End Sub

  Private Sub HideUI()
    cmdSave.Enabled = False
    cmdRevert.Enabled = False
    pnlButtons.Visible = False
    TableLayoutPanel1.SetColumnSpan(lblActivity, 2)
    cmdPlay.Enabled = False
    cmdStartupBrowse.Enabled = False
    cmdWelcomeBrowse.Enabled = False
    txtStartupSound.Enabled = False
    pctWelcome.Enabled = False
  End Sub

  Friend Function GrantFullControlToEveryone(ByVal Folder As String) As Boolean
    Try
      Dim Security As System.Security.AccessControl.DirectorySecurity = IO.Directory.GetAccessControl(Folder)
      Dim Sid As New System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, Nothing)
      Dim Account As System.Security.Principal.NTAccount = TryCast(Sid.Translate(GetType(System.Security.Principal.NTAccount)), System.Security.Principal.NTAccount)
      Dim Grant As New System.Security.AccessControl.FileSystemAccessRule(Account, System.Security.AccessControl.FileSystemRights.FullControl, System.Security.AccessControl.InheritanceFlags.ContainerInherit Or System.Security.AccessControl.InheritanceFlags.ObjectInherit, System.Security.AccessControl.PropagationFlags.None, System.Security.AccessControl.AccessControlType.Allow)
      Security.AddAccessRule(Grant)
      IO.Directory.SetAccessControl(Folder, Security)
      Return True
    Catch ex As Exception
      Return False
    End Try
  End Function
End Class
