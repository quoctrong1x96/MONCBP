Public Class AppNotify
    Inherits ApplicationContext

    Private notifyIcon As NotifyIcon
    Private appActive As Boolean

    Public Sub New()
        AddHandler Application.ApplicationExit, AddressOf OnApplicationExit

        notifyIcon = New NotifyIcon()
        notifyIcon.Icon = My.Resources.Clipboard
        notifyIcon.Text = "Actived"

        AddHandler notifyIcon.MouseClick, AddressOf OnIconMouseClick

        appActive = True
        notifyIcon.Visible = True

    End Sub

    Private Sub OnApplicationExit(ByVal sender As Object, ByVal e As EventArgs)
        If notifyIcon IsNot Nothing Then
            notifyIcon.Dispose()
        End If
    End Sub

    Private Sub OnIconMouseClick(ByVal sender As Object, ByVal e As MouseEventArgs)

        If e.Button = MouseButtons.Left Then
            appActive = Not appActive
            notifyIcon.Icon = If(appActive, My.Resources.Clipboard, My.Resources.InClipboard)
            notifyIcon.Text = If(appActive, "Actived.", "Inactived.")
        Else
            If MsgBox("Do you want to Exit?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                notifyIcon.Visible = False
                ExitThread()
            End If
        End If
    End Sub
End Class
