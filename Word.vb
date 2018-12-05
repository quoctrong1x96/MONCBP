
Imports System.IO
Imports System.Xml

Public Class F_WordForm
    Structure Word
        Dim text As String
        Dim read As String
        Dim meaning As String
    End Structure
    Private Const FILENAME As String = "Source.xml"
    Dim Counter As Integer = 0
    Dim IndexOfWords As Integer = 0
    Private Words As New List(Of Word)
    Private RandomIndex As New List(Of Integer)
    Private RandomSource As New List(Of Integer)
    Private RandomSourceConst As New List(Of Integer)
    Dim flagLoad As Boolean = False
    Private RightMenu As F_Menu
    Private timeTick As Integer = 10
    Public Sub SetTimeTick(ByVal time As Integer)
        timeTick = time
    End Sub
    Public Sub CloseForm()
        If RightMenu IsNot Nothing Then
            RightMenu.Close()
            RightMenu.Dispose()
        End If
        Me.Close()
    End Sub
    Private Sub F_WordForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        L_Meaning.Visible = False
        L_Read.Visible = False
        L_Word.Visible = False
        Dim monitorWidth As Integer = My.Computer.Screen.WorkingArea.Size.Width
        Dim monitorHeight As Integer = My.Computer.Screen.WorkingArea.Size.Height

        Me.Width = monitorWidth
        Me.Height = monitorHeight
        NotifyIconWord.Icon = My.Resources.Word
        NotifyIconWord.Text = "Learn word with " & FILENAME & "."

        AddHandler NotifyIconWord.MouseClick, AddressOf NotifyIconWordMouseClick

        NotifyIconWord.Visible = True
        LoadDataFromXML()
        TimerWord.Interval = 1000
        TimerWord.Start()
        flagLoad = True
    End Sub
    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or &H80
            Return cp
        End Get
    End Property
    Private Sub LoadDataFromXML()
        Dim reader As New StreamReader(FILENAME, System.Text.Encoding.UTF8)
        Dim root = New XmlDocument
        Dim word As Word = New Word()
        Dim index As Integer = 0
        root.Load(reader)
        If root.HasChildNodes Then
            For Each node As XmlNode In root.FirstChild.ChildNodes
                word.text = node.Attributes("text").Value
                word.read = node.Attributes("read").Value
                word.meaning = node.Attributes("meaning").Value
                Words.Add(word)
                RandomSourceConst.Add(index)
                index += 1
            Next
        End If
        If Words.Count <= 0 Then
            Me.Close()
        End If
        RandomIndex.Clear()
        RandomSource = CopyList(RandomSourceConst)
    End Sub
    Private Sub OnApplicationExit(ByVal sender As Object, ByVal e As EventArgs)
        If NotifyIconWord IsNot Nothing Then
            NotifyIconWord.Dispose()
        End If
    End Sub
    Private Sub NotifyIconWordMouseClick(ByVal sender As Object, ByVal e As MouseEventArgs)
        If e.Button = MouseButtons.Left Then
            'Dim word As Word = Words.ElementAt(IndexOfWords)
            'L_Word.Text = word.text
            'L_Word.Location = New Point(Convert.ToInt32((Me.Width - L_Word.Width) / 2), Convert.ToInt32((Me.Height - L_Word.Height) / 2 + Me.Height / 4))
            'L_Read.Text = word.read
            'L_Read.Location = New Point(Convert.ToInt32((Me.Width - L_Word.Width) / 2), L_Word.Location.Y + L_Word.Height + 20)
            'L_Meaning.Text = word.meaning
            'L_Meaning.Location = New Point(Convert.ToInt32((Me.Width - L_Word.Width) / 2), L_Read.Location.Y + L_Read.Height + 20)
            'Counter = 0
            'IndexOfWords += 1
            'If IndexOfWords >= Words.Count Then
            '    IndexOfWords = 0
            'End If
            Dim r As Random = New Random
            Dim index As Integer
            If RandomSource.Count = 1 Then
                index = 0
            Else
                index = r.Next(0, RandomSource.Count - 1)
            End If
            Dim word As Word = Words.ElementAt(RandomSource.ElementAt(index))
            RandomSource.RemoveAt(index)
            L_Word.Text = word.text
            L_Word.Location = New Point(Convert.ToInt32((Me.Width - L_Word.Width) / 2), Convert.ToInt32((Me.Height - L_Word.Height) / 2 - Me.Height / 7))
            L_Read.Text = word.read
            L_Read.Location = New Point(Convert.ToInt32((Me.Width - L_Word.Width) / 2), L_Word.Location.Y + L_Word.Height)
            L_Meaning.Text = word.meaning
            L_Meaning.Location = New Point(Convert.ToInt32((Me.Width - L_Word.Width) / 2), L_Read.Location.Y + L_Read.Height + 20)
            If RandomSource.Count <= 0 Then
                RandomSource = CopyList(RandomSourceConst)
            End If
            L_Meaning.Visible = True
            L_Read.Visible = True
            L_Word.Visible = True
        ElseIf e.Button = MouseButtons.Right Then
            If RightMenu Is Nothing Then
                RightMenu = New F_Menu(Me)
            End If
            Me.TopMost = True
            RightMenu.ShowMenu(Cursor.Position)
            'Me.Close()
        End If
    End Sub
    Private Sub TimeToChange(sender As Object, e As EventArgs) Handles TimerWord.Tick
        Counter += 1
        If Counter >= timeTick OrElse flagLoad Then
            Dim r As Random = New Random
            Dim index As Integer
            If RandomSource.Count = 1 Then
                index = 0
            Else
                index = r.Next(0, RandomSource.Count - 1)
            End If
            Dim word As Word = Words.ElementAt(RandomSource.ElementAt(index))
            RandomSource.RemoveAt(index)
            L_Word.Text = word.text
            L_Word.Location = New Point(Convert.ToInt32((Me.Width - L_Word.Width) / 2), Convert.ToInt32((Me.Height - L_Word.Height) / 2 - Me.Height / 7))
            L_Read.Text = word.read
            L_Read.Location = New Point(Convert.ToInt32((Me.Width - L_Word.Width) / 2), L_Word.Location.Y + L_Word.Height)
            L_Meaning.Text = word.meaning
            L_Meaning.Location = New Point(Convert.ToInt32((Me.Width - L_Word.Width) / 2), L_Read.Location.Y + L_Read.Height + 20)
            If RandomSource.Count <= 0 Then
                RandomSource = CopyList(RandomSourceConst)
            End If
            flagLoad = False
            Counter = 0
            '------------------------------------------
            'Dim word As Word = Words.ElementAt(IndexOfWords)
            'L_Word.Text = word.text
            'L_Word.Location = New Point(Convert.ToInt32((Me.Width - L_Word.Width) / 2), Convert.ToInt32((Me.Height - L_Word.Height) / 2 + Me.Height / 4))
            'L_Read.Text = word.read
            'L_Read.Location = New Point(Convert.ToInt32((Me.Width - L_Word.Width) / 2), L_Word.Location.Y + L_Word.Height + 20)
            'L_Meaning.Text = word.meaning
            'L_Meaning.Location = New Point(Convert.ToInt32((Me.Width - L_Word.Width) / 2), L_Read.Location.Y + L_Read.Height + 20)
            'Counter = 0
            'IndexOfWords += 1
            'If IndexOfWords >= Words.Count Then
            '    IndexOfWords = 0
            'End If
            L_Meaning.Visible = True
            L_Read.Visible = True
            L_Word.Visible = True
        End If
    End Sub
    Private Function CopyList(ByVal list As List(Of Integer)) As List(Of Integer)
        Dim result As New List(Of Integer)
        For Each number As Integer In list
            result.Add(number)
        Next
        Return result
    End Function
End Class
