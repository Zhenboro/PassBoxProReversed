Imports System.IO
Imports System.Security.AccessControl
Imports System.Security.Cryptography
Imports System.Security.Principal
Imports System.Text
Imports Microsoft.Win32
Public Class Form1
    Dim tripleDESCryptoServiceProvider_0 As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider()
    Dim md5CryptoServiceProvider_0 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider()

    Dim Directorio1 As String = "C:\Users\" & Environment.UserName & "\PassBoxData"
    Dim DB1 As String = Directorio1 & "\DB_app.dat"
    Dim DB2 As String = Directorio1 & "\DB_user.dat"

    Dim LlaveLicencias As String = "ma6KnMopqsñ5aM1GñTvnGQÑuQF63ZV"

    Dim AppLlave As String
    Dim UserLlave As String
    Dim CryptoActionsLlave As String = "GI2ttibIY56gUYED52fUfIUHo6T97rIYviUR8647td"
    Dim IsRegistered As String
    Dim Registro As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\\Worcome_Studios\\" & "WorPassBoxProEdition", True)

    Dim UserName As String
    Dim Password As String
    Dim Email As String = "zhenboro@outlook.com"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DesbloquearDirectorio()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        GetPrimeraDB()
        GetSegundaDB()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        RichTextBox1.AppendText(vbCrLf & Descifrar(RichTextBox2.Text, InputBox("Ingrese una llave criptografica", "Llave criptografica", UserLlave & " " & AppLlave)))
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim Licencia = InputBox("Modifique los valores", "Licencia", "#Activated|" & DateTime.Now.ToString("yyyyMMdd") & "|WorPassBoxProEdition|14.0.0.0|" & Email & "|QwErTy|" & DateTime.Now.ToString("dd/MM/yyyy") & "|" & DateTime.Now.ToString("dd/MM/") & DateTime.Now.ToString("yyyy") + 1 & "|True|uwuowoeweawa")
        Registro.SetValue("AppLicenser", Cifrar(Licencia, LlaveLicencias))
        RichTextBox1.AppendText(vbCrLf & "Licencia agregada")
        RichTextBox1.ScrollToCaret()
    End Sub

    Sub GetPrimeraDB()
        Try
            Dim RAWContent As String = My.Computer.FileSystem.ReadAllText(DB1)
            Dim DecryptContent As String = Descifrar(RAWContent, CryptoActionsLlave)
            Dim lines As String() = New TextBox() With {
            .Text = DecryptContent
            }.Lines
            RichTextBox1.AppendText(vbCrLf & "<--- DB Raw Content --->")
            RichTextBox1.AppendText(vbCrLf & RAWContent)
            RichTextBox1.AppendText(vbCrLf & "<--- DB Decrypted Content Loaded --->")
            RichTextBox1.AppendText(vbCrLf & DecryptContent)
            Me.IsRegistered = lines(1).Split(New Char() {">"c})(1).Trim()
            Me.AppLlave = lines(2).Split(New Char() {">"c})(1).Trim()

            TextBox5.Text = IsRegistered
            TextBox6.Text = AppLlave
            Console.WriteLine(IsRegistered & vbCrLf & AppLlave)
            RichTextBox1.ScrollToCaret()
        Catch ex As Exception
            Console.WriteLine("[Debugger@GetAppData]Error: " + ex.Message)
        End Try
    End Sub

    Sub GetSegundaDB()
        Try
            Dim RAWContent As String = My.Computer.FileSystem.ReadAllText(DB2)
            Dim DecryptContent As String = Descifrar(RAWContent, Me.AppLlave)
            Dim lines As String() = New TextBox() With {
            .Text = DecryptContent
            }.Lines
            RichTextBox1.AppendText(vbCrLf & "<--- DB Raw Content --->")
            RichTextBox1.AppendText(vbCrLf & RAWContent)
            RichTextBox1.AppendText(vbCrLf & "<--- DB Decrypted Content Loaded --->")
            RichTextBox1.AppendText(vbCrLf & DecryptContent)
            Me.UserName = lines(1).Split(New Char() {">"c})(1).Trim()
            Me.Password = lines(2).Split(New Char() {">"c})(1).Trim()
            Me.Email = lines(3).Split(New Char() {">"c})(1).Trim()
            Me.UserLlave = lines(4).Split(New Char() {">"c})(1).Trim()

            TextBox1.Text = UserName
            TextBox2.Text = Password
            TextBox3.Text = Email
            TextBox4.Text = UserLlave
            Console.WriteLine(UserName & vbCrLf & Password & vbCrLf & Email & vbCrLf & UserLlave & vbCrLf & Descifrar(UserLlave, AppLlave))
            RichTextBox1.ScrollToCaret()
        Catch ex As Exception
            Console.WriteLine("[Debugger@GetUserData]Error: " + ex.Message)
        End Try
    End Sub

    Public Sub SavePrimeraDB()
        If My.Computer.FileSystem.FileExists(Me.DB1) = True Then
            My.Computer.FileSystem.DeleteFile(Me.DB1)
        End If
        Try
            Dim FormatoDB As String = "#Wor PassBox Pro App Data Base" &
                vbCrLf & "IsRegistered>" & IsRegistered &
                vbCrLf & "CryptoKey>" & Me.AppLlave
            Dim EncryptedContent As String = Cifrar(FormatoDB, Me.CryptoActionsLlave)
            RichTextBox1.AppendText(vbCrLf & "<--- DB Raw Content --->")
            RichTextBox1.AppendText(vbCrLf & FormatoDB)
            RichTextBox1.AppendText(vbCrLf & "<--- DB Encrypted Content Saved --->")
            RichTextBox1.AppendText(vbCrLf & EncryptedContent)
            My.Computer.FileSystem.WriteAllText(Me.DB1, EncryptedContent, False)
            RichTextBox1.ScrollToCaret()
            GetPrimeraDB()
        Catch ex As Exception
            Console.WriteLine("[Debugger@SaveAppData]Error: " + ex.Message)
        End Try
    End Sub
    Public Sub SaveSegundaDB()
        If My.Computer.FileSystem.FileExists(Me.DB2) = True Then
            My.Computer.FileSystem.DeleteFile(Me.DB2)
        End If
        Try
            Dim FormatoDB As String = "#Wor PassBox Pro User Data Base" &
                vbCrLf & "Username>" & Me.UserName &
                vbCrLf & "Password>" & Me.Password &
                vbCrLf & "Email>" & Me.Email &
                vbCrLf & "CryptoKey>" & Me.UserLlave
            Dim EncryptedContent As String = Cifrar(FormatoDB, Me.AppLlave)
            RichTextBox1.AppendText(vbCrLf & "<--- DB Raw Content --->")
            RichTextBox1.AppendText(vbCrLf & FormatoDB)
            RichTextBox1.AppendText(vbCrLf & "<--- DB Encrypted Content Saved --->")
            RichTextBox1.AppendText(vbCrLf & EncryptedContent)
            My.Computer.FileSystem.WriteAllText(Me.DB2, EncryptedContent, False)
            RichTextBox1.ScrollToCaret()
            GetSegundaDB()
        Catch ex As Exception
            Console.WriteLine("[Debugger@SaveUserData]Error: " + ex.Message)
        End Try
    End Sub

    Sub DesbloquearDirectorio()
        Try
            Dim accessControl As FileSystemSecurity = File.GetAccessControl(Me.Directorio1)
            Dim identity As SecurityIdentifier = New SecurityIdentifier(WellKnownSidType.WorldSid, Nothing)
            accessControl.RemoveAccessRule(New FileSystemAccessRule(identity, FileSystemRights.FullControl, AccessControlType.Deny))
            File.SetAccessControl(Me.Directorio1, CType(accessControl, FileSecurity))
            RichTextBox1.AppendText(vbCrLf & "<--- Directorio Desbloqueado --->")
        Catch ex2 As Exception
            Console.WriteLine(ex2.Message)
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        UserName = TextBox1.Text
        Password = TextBox2.Text
        Email = TextBox3.Text
        UserLlave = TextBox4.Text
        IsRegistered = TextBox5.Text
        AppLlave = TextBox6.Text

        SaveSegundaDB()
        SavePrimeraDB()
    End Sub

    Public Function Cifrar(ByVal TextIn As String, ByVal llave As String) As String
        Dim result As String
        If TextIn = "" Then
            result = ""
        Else
            tripleDESCryptoServiceProvider_0.Key = md5CryptoServiceProvider_0.ComputeHash(New UnicodeEncoding().GetBytes(llave))
            tripleDESCryptoServiceProvider_0.Mode = CipherMode.ECB
            Dim cryptoTransform As ICryptoTransform = tripleDESCryptoServiceProvider_0.CreateEncryptor()
            Dim bytes As Byte() = Encoding.ASCII.GetBytes(TextIn)
            result = Convert.ToBase64String(cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length))
        End If
        Return result
    End Function
    Public Function Descifrar(ByVal TextIn As String, ByVal llave As String) As String
        Dim result As String
        If TextIn = "" Then
            result = ""
        Else
            tripleDESCryptoServiceProvider_0.Key = md5CryptoServiceProvider_0.ComputeHash(New UnicodeEncoding().GetBytes(llave))
            tripleDESCryptoServiceProvider_0.Mode = CipherMode.ECB
            Dim cryptoTransform As ICryptoTransform = tripleDESCryptoServiceProvider_0.CreateDecryptor()
            Dim array As Byte() = Convert.FromBase64String(TextIn)
            result = Encoding.ASCII.GetString(cryptoTransform.TransformFinalBlock(array, 0, array.Length))
        End If
        Return result
    End Function
End Class