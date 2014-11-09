Imports Vestris.ResourceLib
Imports System.IO

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
        GC.Collect()
        Dim iStart As Long = TickCount()
        lblActivity.Text = "Waiting for DLL"
        Application.DoEvents()
        Do Until InUseChecker(imageresDLL, FileAccess.Write)
          Application.DoEvents()
          Threading.Thread.Sleep(1)
          If TickCount() - iStart > 15000 Then Exit Do
        Loop
        'If Not InUseChecker(imageresDLL, FileAccess.Write) Then
        '  MsgBox("Unable to modify ImageRes DLL File. Please make sure it's not in use!", MsgBoxStyle.Critical, "DLL Modify Error!")
        '  ShowUI()
        '  Exit Sub
        'End If
        Application.DoEvents()
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
        GC.Collect()
        Dim iStart As Long = TickCount()
        lblActivity.Text = "Waiting for DLL"
        Application.DoEvents()
        Do Until InUseChecker(imageresDLL, FileAccess.Write)
          Application.DoEvents()
          Threading.Thread.Sleep(1)
          If TickCount() - iStart > 15000 Then Exit Do
        Loop
        lblActivity.Text = "Saving Sound"
        Application.DoEvents()
        Try
          gRes.SaveTo(imageresDLL)
        Catch ex As Exception
          MsgBox("Unable to save ImageRes DLL File. Please make sure it's not in use!" & vbNewLine & ex.Message, MsgBoxStyle.Critical, "DLL Save Error!")
          ShowUI()
          Exit Sub
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


  ''' <summary>
  ''' Attempts to see if a file is in use, waiting up to five seconds for it to be freed.
  ''' </summary>
  ''' <param name="Filename">The exact path to the file which needs to be checked.</param>
  ''' <param name="access">Write permissions required for checking.</param>
  ''' <returns>True on available, false on in use.</returns>
  ''' <remarks></remarks>
  Public Function InUseChecker(Filename As String, access As IO.FileAccess) As Boolean
    If Not IO.File.Exists(Filename) Then Return True
    Dim iStart As Long = TickCount()
    Do
      Try
        Select Case access
          Case FileAccess.Read
            'only check for ability to read
            Using fs As FileStream = IO.File.Open(Filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite Or FileShare.Delete)
              If fs.CanRead Then
                Return True
                Exit Do
              End If
            End Using
          Case FileAccess.Write, FileAccess.ReadWrite
            'check for ability to write
            Using fs As FileStream = IO.File.Open(Filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite Or FileShare.Delete)
              If fs.CanWrite Then
                Return True
                Exit Do
              End If
            End Using
        End Select
      Catch ex As Exception
      End Try
      Threading.Thread.Sleep(100)
      Threading.Thread.Sleep(0)
      Threading.Thread.Sleep(100)
    Loop While TickCount() - iStart < 5000
    Return False
  End Function
  Public Function TickCount() As Long
    Return (Stopwatch.GetTimestamp / Stopwatch.Frequency) * 1000
  End Function
  Friend Function GrantFullControlToEveryone(ByVal Folder As String) As Boolean
    Try
      Dim Security As System.Security.AccessControl.DirectorySecurity = Directory.GetAccessControl(Folder)
      Dim Sid As New System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, Nothing)
      Dim Account As System.Security.Principal.NTAccount = TryCast(Sid.Translate(GetType(System.Security.Principal.NTAccount)), System.Security.Principal.NTAccount)
      Dim Grant As New System.Security.AccessControl.FileSystemAccessRule(Account, System.Security.AccessControl.FileSystemRights.FullControl, System.Security.AccessControl.InheritanceFlags.ContainerInherit Or System.Security.AccessControl.InheritanceFlags.ObjectInherit, System.Security.AccessControl.PropagationFlags.None, System.Security.AccessControl.AccessControlType.Allow)
      Security.AddAccessRule(Grant)
      Directory.SetAccessControl(Folder, Security)
      Return True
    Catch ex As Exception
      Return False
    End Try
  End Function

  Private Sub cmdDonate_Click(sender As System.Object, e As System.EventArgs) Handles cmdDonate.Click
    Process.Start("http://realityripple.com/donate.php?itm=Welcome+Updater")
  End Sub
End Class



NotInheritable Class GrayBMP_File
  Private Sub New()
  End Sub
  Shared BMP_File_Header As Byte() = New Byte(13) {}
  Shared DIB_header As Byte() = New Byte(39) {}
  Shared Color_palette As Byte() = New Byte(1023) {}
  Shared Bitmap_Data As Byte() = Nothing
  Private Shared Function create_palette() As Byte()
    Dim color_palette As Byte() = New Byte(1023) {}
    For i As Integer = 0 To 255
      color_palette(i * 4 + 0) = CByte(i)
      color_palette(i * 4 + 1) = CByte(i)
      color_palette(i * 4 + 2) = CByte(i)
      color_palette(i * 4 + 3) = CByte(0)
    Next
    Return color_palette
  End Function
  Private Shared Sub create_parts(img As Image)
    Bitmap_Data = ConvertToGrayscale(img)
    Copy_to_Index(BMP_File_Header, New Byte() {CByte(AscW("B"c)), CByte(AscW("M"c))}, 0)
    Copy_to_Index(BMP_File_Header, BitConverter.GetBytes(BMP_File_Header.Length + DIB_header.Length + Color_palette.Length + Bitmap_Data.Length), 2)
    Copy_to_Index(BMP_File_Header, New Byte() {CByte(AscW("M"c)), CByte(AscW("C"c)), CByte(AscW("A"c)), CByte(AscW("T"c))}, 6)
    Copy_to_Index(BMP_File_Header, BitConverter.GetBytes(BMP_File_Header.Length + DIB_header.Length + Color_palette.Length), 10)
    Copy_to_Index(DIB_header, BitConverter.GetBytes(DIB_header.Length), 0)
    Copy_to_Index(DIB_header, BitConverter.GetBytes(DirectCast(img, Bitmap).Width), 4)
    Copy_to_Index(DIB_header, BitConverter.GetBytes(DirectCast(img, Bitmap).Height), 8)
    Copy_to_Index(DIB_header, New Byte() {CByte(1), CByte(0)}, 12)
    Copy_to_Index(DIB_header, New Byte() {CByte(8), CByte(0)}, 14)
    Copy_to_Index(DIB_header, BitConverter.GetBytes(0), 16)
    Copy_to_Index(DIB_header, BitConverter.GetBytes(Bitmap_Data.Length), 20)
    Copy_to_Index(DIB_header, BitConverter.GetBytes(1000), 24)
    'horizontal reselution N.B. not important
    Copy_to_Index(DIB_header, BitConverter.GetBytes(1000), 28)
    'vertical reselution N.B. not important
    Copy_to_Index(DIB_header, BitConverter.GetBytes(256), 32)
    Copy_to_Index(DIB_header, BitConverter.GetBytes(0), 36)
    Color_palette = create_palette()
  End Sub
  Private Shared Function ConvertToGrayscale(inImg As Image) As Byte()
    Dim inBMP As Bitmap = DirectCast(inImg, Bitmap)
    Dim padding As Integer = If((inBMP.Width Mod 4) <> 0, 4 - (inBMP.Width Mod 4), 0)
    Dim bytes As Byte() = New Byte(inBMP.Width * inBMP.Height + (padding * inBMP.Height - 1)) {}
    For y As Integer = 0 To inBMP.Height - 1
      For x As Integer = 0 To inBMP.Width - 1
        Dim c As Color = inBMP.GetPixel(x, y)
        Dim g As Integer = Convert.ToInt32(0.3 * c.R + 0.59 * c.G + 0.11 * c.B)
        bytes((inBMP.Height - 1 - y) * inBMP.Width + (inBMP.Height - 1 - y) * padding + x) = CByte(g)
      Next
      For i As Integer = 0 To padding - 1
        bytes((inBMP.Height - y) * inBMP.Width + (inBMP.Height - 1 - y) * padding + i) = CByte(0)
      Next
    Next
    Return bytes
  End Function
  Public Shared Function CreateGrayBitmapFile(Image As Image, Path As String) As Boolean
    Try
      create_parts(Image)
      Dim oFileStream As FileStream
      oFileStream = New FileStream(Path, FileMode.OpenOrCreate)
      oFileStream.Write(BMP_File_Header, 0, BMP_File_Header.Length)
      oFileStream.Write(DIB_header, 0, DIB_header.Length)
      oFileStream.Write(Color_palette, 0, Color_palette.Length)
      oFileStream.Write(Bitmap_Data, 0, Bitmap_Data.Length)
      oFileStream.Close()
      Return True
    Catch
      Return False
    End Try
  End Function
  Private Shared Function Copy_to_Index(destination As Byte(), source As Byte(), index As Integer) As Boolean
    Try
      For i As Integer = 0 To source.Length - 1
        destination(i + index) = source(i)
      Next
      Return True
    Catch
      Return False
    End Try
  End Function
End Class