Imports System.IO
Imports System.Xml

Public Class F_Main
    Private notifyIcon As NotifyIcon
    Private appActive As Boolean
    Private root As XmlDocument
    Private Const FILENAME As String = "text.xml"
    Private innerText As Dictionary(Of String, String) = New Dictionary(Of String, String)
    Private keyOfInnerTest As Integer = 0
    Private Sub F_Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
#Region "Notyfind Icon"
        notifyIcon = New NotifyIcon()
        notifyIcon.Icon = My.Resources.Clipboard
        notifyIcon.Text = "Actived"

        AddHandler notifyIcon.MouseClick, AddressOf OnIconMouseClick

        appActive = True
        notifyIcon.Visible = True
#End Region
        Me.Width = 0
        Me.Height = 0
        Me.TopMost = False
        Me.Visible = False

        '----------------------------------------
        If (IO.File.Exists(FILENAME)) Then
            Dim reader As New StreamReader(FILENAME, System.Text.Encoding.UTF8)
            Call CreateMenuStrip(reader)
        Else
            MessageBox.Show("The " & FILENAME & "you selected was not found.")
        End If
    End Sub

    Private Sub F_Main_Deactivate(sender As Object, e As EventArgs) Handles MyBase.Deactivate
        M_Menu.Visible = False
        Me.TopMost = False
        Me.Hide()
    End Sub

#Region "Notify Event"
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
            If notifyIcon.Text = "Inactived." Then
                Exit Sub
            End If
            Me.TopMost = True
            M_Menu.Show(Cursor.Position)
            Me.Left = M_Menu.Left + 1
            Me.Top = M_Menu.Top + 1
            Me.Width = M_Menu.Width
            Me.Height = M_Menu.Height
        End If
    End Sub
    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub M_Menu_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles M_Menu.ItemClicked

        Dim item As ToolStripMenuItem = e.ClickedItem
        If item IsNot Nothing Then
            If item.Text = "Exit" AndAlso item.OwnerItem Is Nothing Then
                Me.Close()
                Exit Sub
            End If
            '    Dim pointList As List(Of String) = New List(Of String)
            '    pointList.Add(item.Text)
            '    While Not IsDBNull(item.OwnerItem)
            '        item = item.OwnerItem
            '        pointList.Add(item.Text)
            '    End While
        End If
        'M_Menu.Visible = False
        'Me.TopMost = False
        'Me.Hide()
    End Sub
#End Region
    Private Sub CreateMenuStrip(ByVal reader As StreamReader)
        root = New XmlDocument
        root.Load(reader)
        If root.HasChildNodes Then
            M_Menu.Items.Clear()
            For Each item As XmlElement In root.ChildNodes
                If item.HasChildNodes AndAlso item.ChildNodes.Item(0).Name <> "#text" Then
                    Dim subitems As ToolStripMenuItem = AddChildsToContextMenuStrip(item)
                    For Each subitem As ToolStripMenuItem In subitems.DropDownItems
                        Dim subitemClone = New ClonableToolStripMenuItem().Clone(subitem)
                        M_Menu.Items.Add(subitemClone)
                        AddHandler subitemClone.Click, AddressOf ItemsMouseClick
                    Next
                Else
                    Dim subitemClone = New ToolStripMenuItem(item.Attributes("name").Value)
                    AddHandler subitemClone.Click, AddressOf ItemsMouseClick
                    keyOfInnerTest += 1
                    subitemClone.Name = New System.Text.StringBuilder("MenuItem" & keyOfInnerTest).ToString
                    innerText.Add(subitemClone.Name, item.InnerText.Clone())
                    M_Menu.Items.Add(subitemClone)
                End If
            Next
        End If
        M_Menu.Items.Add(New ToolStripMenuItem("Exit"))
    End Sub
    Private Function AddChildsToContextMenuStrip(ByVal root As XmlElement) As ToolStripMenuItem
        Dim listItemMain As List(Of ToolStripMenuItem) = New List(Of ToolStripMenuItem)
        '---------------------------
        For Each item As XmlElement In root
            Dim subtool As ToolStripMenuItem = New ToolStripMenuItem(item.Attributes("name").Value)

            If item.HasChildNodes AndAlso item.ChildNodes.Item(0).Name <> "#text" Then
                Dim subitems As ToolStripMenuItem = AddChildsToContextMenuStrip(item)
                For Each subitem As ToolStripMenuItem In subitems.DropDownItems
                    Dim subitemClone = New ClonableToolStripMenuItem().Clone(subitem)
                    subtool.DropDownItems.Add(subitemClone)
                    AddHandler subitemClone.Click, AddressOf ItemsMouseClick
                Next
            Else
                keyOfInnerTest += 1
                subtool.Name = New System.Text.StringBuilder("MenuItem" & keyOfInnerTest).ToString
                Dim xmlnode As XmlNode = item.Clone

                innerText.Add(subtool.Name, New String(xmlnode.FirstChild.InnerText))
            End If
            listItemMain.Add(subtool)

        Next
        Dim subMenu As New ToolStripMenuItem()
        For Each item As ToolStripMenuItem In listItemMain
            subMenu.DropDownItems.Add(item)
            AddHandler item.Click, AddressOf ItemsMouseClick
        Next
        Return subMenu
    End Function

    Private Sub ItemsMouseClick(ByVal sender As Object, ByVal e As EventArgs)

        Dim key As String = CType(sender, ToolStripMenuItem).Name
        If innerText.ContainsKey(key) Then
            Clipboard.Clear()
            Clipboard.SetText(innerText.Item(key))
        End If

    End Sub
End Class

Public Class ClonableToolStripMenuItem
    Inherits ToolStripMenuItem

    Public Sub New()
    End Sub

    Friend Function Clone(byval this As ToolStripMenuItem) As ToolStripMenuItem
        Dim menuItem As ClonableToolStripMenuItem = New ClonableToolStripMenuItem()
        menuItem.AccessibleName = this.AccessibleName
        menuItem.AccessibleRole = this.AccessibleRole
        menuItem.Alignment = this.Alignment
        menuItem.AllowDrop = this.AllowDrop
        menuItem.Anchor = this.Anchor
        menuItem.AutoSize = this.AutoSize
        menuItem.AutoToolTip = this.AutoToolTip
        menuItem.BackColor = this.BackColor
        menuItem.BackgroundImage = this.BackgroundImage
        menuItem.BackgroundImageLayout = this.BackgroundImageLayout
        menuItem.Checked = this.Checked
        menuItem.CheckOnClick = this.CheckOnClick
        menuItem.CheckState = this.CheckState
        menuItem.DisplayStyle = this.DisplayStyle
        menuItem.Dock = this.Dock
        menuItem.DoubleClickEnabled = this.DoubleClickEnabled
        menuItem.DropDown = this.DropDown
        menuItem.DropDownDirection = this.DropDownDirection
        menuItem.Enabled = this.Enabled
        menuItem.Font = this.Font
        menuItem.ForeColor = this.ForeColor
        menuItem.Image = this.Image
        menuItem.ImageAlign = this.ImageAlign
        menuItem.ImageScaling = this.ImageScaling
        menuItem.ImageTransparentColor = this.ImageTransparentColor
        menuItem.Margin = this.Margin
        menuItem.MergeAction = this.MergeAction
        menuItem.MergeIndex = this.MergeIndex
        menuItem.Name = this.Name
        menuItem.Overflow = this.Overflow
        menuItem.Padding = this.Padding
        menuItem.RightToLeft = this.RightToLeft
        menuItem.ShortcutKeys = this.ShortcutKeys
        menuItem.ShowShortcutKeys = this.ShowShortcutKeys
        menuItem.Tag = this.Tag
        menuItem.Text = this.Text
        menuItem.TextAlign = this.TextAlign
        menuItem.TextDirection = this.TextDirection
        menuItem.TextImageRelation = this.TextImageRelation
        menuItem.ToolTipText = this.ToolTipText
        menuItem.Available = this.Available


        If Not AutoSize Then
            menuItem.Size = this.Size
        End If

        Return menuItem
    End Function
End Class
