<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
  Inherits System.Windows.Forms.Form

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
    Me.cmdPlay = New System.Windows.Forms.Button()
    Me.lblWelcome = New System.Windows.Forms.Label()
    Me.pctWelcome = New System.Windows.Forms.PictureBox()
    Me.cmdWelcomeBrowse = New System.Windows.Forms.Button()
    Me.lblStartupSound = New System.Windows.Forms.Label()
    Me.txtStartupSound = New System.Windows.Forms.TextBox()
    Me.cmdStartupBrowse = New System.Windows.Forms.Button()
    Me.pnlButtons = New System.Windows.Forms.TableLayoutPanel()
    Me.cmdSave = New System.Windows.Forms.Button()
    Me.cmdRevert = New System.Windows.Forms.Button()
    Me.lblActivity = New System.Windows.Forms.Label()
    Me.cmdDonate = New System.Windows.Forms.Button()
    Me.TableLayoutPanel1.SuspendLayout()
    CType(Me.pctWelcome, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlButtons.SuspendLayout()
    Me.SuspendLayout()
    '
    'TableLayoutPanel1
    '
    Me.TableLayoutPanel1.ColumnCount = 4
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.TableLayoutPanel1.Controls.Add(Me.cmdPlay, 2, 1)
    Me.TableLayoutPanel1.Controls.Add(Me.lblWelcome, 0, 0)
    Me.TableLayoutPanel1.Controls.Add(Me.pctWelcome, 1, 0)
    Me.TableLayoutPanel1.Controls.Add(Me.cmdWelcomeBrowse, 3, 0)
    Me.TableLayoutPanel1.Controls.Add(Me.lblStartupSound, 0, 1)
    Me.TableLayoutPanel1.Controls.Add(Me.txtStartupSound, 1, 1)
    Me.TableLayoutPanel1.Controls.Add(Me.cmdStartupBrowse, 3, 1)
    Me.TableLayoutPanel1.Controls.Add(Me.lblActivity, 0, 2)
    Me.TableLayoutPanel1.Controls.Add(Me.pnlButtons, 1, 2)
    Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
    Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
    Me.TableLayoutPanel1.RowCount = 3
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
    Me.TableLayoutPanel1.Size = New System.Drawing.Size(389, 225)
    Me.TableLayoutPanel1.TabIndex = 0
    '
    'cmdPlay
    '
    Me.cmdPlay.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdPlay.Image = Global.WelcomeUpdater.My.Resources.Resources.play
    Me.cmdPlay.Location = New System.Drawing.Point(280, 164)
    Me.cmdPlay.Name = "cmdPlay"
    Me.cmdPlay.Size = New System.Drawing.Size(23, 23)
    Me.cmdPlay.TabIndex = 9
    Me.cmdPlay.UseVisualStyleBackColor = True
    '
    'lblWelcome
    '
    Me.lblWelcome.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblWelcome.AutoSize = True
    Me.lblWelcome.Location = New System.Drawing.Point(3, 74)
    Me.lblWelcome.Name = "lblWelcome"
    Me.lblWelcome.Size = New System.Drawing.Size(92, 13)
    Me.lblWelcome.TabIndex = 0
    Me.lblWelcome.Text = "Welcome Screen:"
    '
    'pctWelcome
    '
    Me.pctWelcome.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.TableLayoutPanel1.SetColumnSpan(Me.pctWelcome, 2)
    Me.pctWelcome.Location = New System.Drawing.Point(103, 5)
    Me.pctWelcome.Name = "pctWelcome"
    Me.pctWelcome.Size = New System.Drawing.Size(200, 150)
    Me.pctWelcome.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
    Me.pctWelcome.TabIndex = 1
    Me.pctWelcome.TabStop = False
    '
    'cmdWelcomeBrowse
    '
    Me.cmdWelcomeBrowse.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.cmdWelcomeBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdWelcomeBrowse.Location = New System.Drawing.Point(311, 69)
    Me.cmdWelcomeBrowse.Name = "cmdWelcomeBrowse"
    Me.cmdWelcomeBrowse.Size = New System.Drawing.Size(75, 23)
    Me.cmdWelcomeBrowse.TabIndex = 2
    Me.cmdWelcomeBrowse.Text = "Browse..."
    Me.cmdWelcomeBrowse.UseVisualStyleBackColor = True
    '
    'lblStartupSound
    '
    Me.lblStartupSound.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblStartupSound.AutoSize = True
    Me.lblStartupSound.Location = New System.Drawing.Point(3, 169)
    Me.lblStartupSound.Name = "lblStartupSound"
    Me.lblStartupSound.Size = New System.Drawing.Size(78, 13)
    Me.lblStartupSound.TabIndex = 3
    Me.lblStartupSound.Text = "Startup Sound:"
    '
    'txtStartupSound
    '
    Me.txtStartupSound.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtStartupSound.Location = New System.Drawing.Point(101, 165)
    Me.txtStartupSound.Name = "txtStartupSound"
    Me.txtStartupSound.Size = New System.Drawing.Size(172, 20)
    Me.txtStartupSound.TabIndex = 4
    '
    'cmdStartupBrowse
    '
    Me.cmdStartupBrowse.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.cmdStartupBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdStartupBrowse.Location = New System.Drawing.Point(311, 164)
    Me.cmdStartupBrowse.Name = "cmdStartupBrowse"
    Me.cmdStartupBrowse.Size = New System.Drawing.Size(75, 23)
    Me.cmdStartupBrowse.TabIndex = 5
    Me.cmdStartupBrowse.Text = "Browse..."
    Me.cmdStartupBrowse.UseVisualStyleBackColor = True
    '
    'pnlButtons
    '
    Me.pnlButtons.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.pnlButtons.AutoSize = True
    Me.pnlButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlButtons.ColumnCount = 3
    Me.TableLayoutPanel1.SetColumnSpan(Me.pnlButtons, 3)
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.Controls.Add(Me.cmdSave, 0, 0)
    Me.pnlButtons.Controls.Add(Me.cmdRevert, 1, 0)
    Me.pnlButtons.Controls.Add(Me.cmdDonate, 2, 0)
    Me.pnlButtons.Location = New System.Drawing.Point(104, 193)
    Me.pnlButtons.Name = "pnlButtons"
    Me.pnlButtons.RowCount = 1
    Me.pnlButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlButtons.Size = New System.Drawing.Size(278, 29)
    Me.pnlButtons.TabIndex = 8
    '
    'cmdSave
    '
    Me.cmdSave.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdSave.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdSave.Location = New System.Drawing.Point(3, 3)
    Me.cmdSave.Name = "cmdSave"
    Me.cmdSave.Size = New System.Drawing.Size(75, 23)
    Me.cmdSave.TabIndex = 6
    Me.cmdSave.Text = "Save"
    Me.cmdSave.UseVisualStyleBackColor = True
    '
    'cmdRevert
    '
    Me.cmdRevert.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdRevert.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdRevert.Location = New System.Drawing.Point(84, 3)
    Me.cmdRevert.Name = "cmdRevert"
    Me.cmdRevert.Size = New System.Drawing.Size(75, 23)
    Me.cmdRevert.TabIndex = 7
    Me.cmdRevert.Text = "Revert"
    Me.cmdRevert.UseVisualStyleBackColor = True
    '
    'lblActivity
    '
    Me.lblActivity.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblActivity.AutoSize = True
    Me.lblActivity.Location = New System.Drawing.Point(3, 201)
    Me.lblActivity.Name = "lblActivity"
    Me.lblActivity.Size = New System.Drawing.Size(0, 13)
    Me.lblActivity.TabIndex = 10
    '
    'cmdDonate
    '
    Me.cmdDonate.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdDonate.Location = New System.Drawing.Point(165, 3)
    Me.cmdDonate.Name = "cmdDonate"
    Me.cmdDonate.Size = New System.Drawing.Size(110, 23)
    Me.cmdDonate.TabIndex = 8
    Me.cmdDonate.Text = "Make a Donation"
    Me.cmdDonate.UseVisualStyleBackColor = True
    '
    'frmMain
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(389, 225)
    Me.Controls.Add(Me.TableLayoutPanel1)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
    Me.Icon = Global.WelcomeUpdater.My.Resources.Resources.windows
    Me.MaximizeBox = False
    Me.Name = "frmMain"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "Windows 7 Welcome Screen Updater"
    Me.TableLayoutPanel1.ResumeLayout(False)
    Me.TableLayoutPanel1.PerformLayout()
    CType(Me.pctWelcome, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlButtons.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblWelcome As System.Windows.Forms.Label
  Friend WithEvents pctWelcome As System.Windows.Forms.PictureBox
  Friend WithEvents cmdWelcomeBrowse As System.Windows.Forms.Button
  Friend WithEvents lblStartupSound As System.Windows.Forms.Label
  Friend WithEvents txtStartupSound As System.Windows.Forms.TextBox
  Friend WithEvents cmdStartupBrowse As System.Windows.Forms.Button
  Friend WithEvents pnlButtons As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents cmdSave As System.Windows.Forms.Button
  Friend WithEvents cmdRevert As System.Windows.Forms.Button
  Friend WithEvents cmdPlay As System.Windows.Forms.Button
  Friend WithEvents lblActivity As System.Windows.Forms.Label
  Friend WithEvents cmdDonate As System.Windows.Forms.Button

End Class
