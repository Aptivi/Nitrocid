Imports System.Threading

Module ShellStartThreads

    ''' <summary>
    ''' Master start command thread
    ''' </summary>
    Public StartCommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "Shell Command Thread"}
    ''' <summary>
    ''' Text editor start command thread
    ''' </summary>
    Public TextEdit_CommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "Text Edit Command Thread"}
    ''' <summary>
    ''' Zip start command thread
    ''' </summary>
    Public ZipShell_CommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "ZIP Shell Command Thread"}
    ''' <summary>
    ''' FTP start command thread
    ''' </summary>
    Public FTPStartCommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "FTP Command Thread"}
    ''' <summary>
    ''' RSS start command thread
    ''' </summary>
    Public RSSCommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "RSS Shell Command Thread"}
    ''' <summary>
    ''' Mail start command thread
    ''' </summary>
    Public MailStartCommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "Mail Command Thread"}
    ''' <summary>
    ''' SFTP start command thread
    ''' </summary>
    Public SFTPStartCommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "SFTP Command Thread"}
    ''' <summary>
    ''' Test start command thread
    ''' </summary>
    Public TStartCommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "Test Shell Command Thread"}

End Module
