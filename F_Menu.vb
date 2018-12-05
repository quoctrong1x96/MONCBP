Public Class F_Menu
    Private _parent As F_WordForm
    Sub New(ByRef parent As Form)

        ' This call is required by the designer.
        InitializeComponent()
        _parent = parent
        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Sub ShowMenu(ByVal point As Point)
        M_Menu.Show(point)
        Me.Left = M_Menu.Left + 1
        Me.Top = M_Menu.Top + 1
        Me.Width = M_Menu.Width
        Me.Height = M_Menu.Height
    End Sub
    Private Sub F_Main_Deactivate(sender As Object, e As EventArgs) Handles MyBase.Deactivate
        M_Menu.Visible = False
        Me.TopMost = False
        Me.Hide()
    End Sub

    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click, ToolStripMenuItem8.Click, ToolStripMenuItem7.Click, ToolStripMenuItem6.Click, ToolStripMenuItem5.Click, ToolStripMenuItem4.Click, ToolStripMenuItem3.Click
        Select Case sender.Name
            Case ToolStripMenuItem2.Name
                _parent.SetTimeTick(5)
            Case ToolStripMenuItem3.Name
                _parent.SetTimeTick(10)
            Case ToolStripMenuItem4.Name
                _parent.SetTimeTick(15)
            Case ToolStripMenuItem5.Name
                _parent.SetTimeTick(20)
            Case ToolStripMenuItem8.Name
                _parent.SetTimeTick(100)
            Case ToolStripMenuItem7.Name
                _parent.SetTimeTick(60)
            Case ToolStripMenuItem6.Name
                _parent.SetTimeTick(30)
        End Select

    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        _parent.CloseForm()
    End Sub
End Class
