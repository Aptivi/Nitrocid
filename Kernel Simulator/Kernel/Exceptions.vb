
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
'
'    This file is part of Kernel Simulator
'
'    Kernel Simulator is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Kernel Simulator is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Public Class Exceptions

    ''' <summary>
    ''' There are no more users remaining
    ''' </summary>
    Public Class NullUsersException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when alias source and destination have the same name
    ''' </summary>
    Public Class AliasInvalidOperationException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when alias type is nonexistent
    ''' </summary>
    Public Class AliasNoSuchTypeException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when alias source command is nonexistent
    ''' </summary>
    Public Class AliasNoSuchCommandException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when alias already exists
    ''' </summary>
    Public Class AliasAlreadyExistsException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when alias is nonexistent
    ''' </summary>
    Public Class AliasNoSuchAliasException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when language is nonexistent
    ''' </summary>
    Public Class NoSuchLanguageException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when screensaver is nonexistent
    ''' </summary>
    Public Class NoSuchScreensaverException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when mod is nonexistent
    ''' </summary>
    Public Class NoSuchModException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there is a config error
    ''' </summary>
    Public Class ConfigException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there is a user creation error
    ''' </summary>
    Public Class UserCreationException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there is a user management error
    ''' </summary>
    Public Class UserManagementException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there is a user management error
    ''' </summary>
    Public Class PermissionManagementException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there is a hostname error
    ''' </summary>
    Public Class HostnameException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there is a color setting error
    ''' </summary>
    Public Class ColorException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there is a remote debugger device not found error
    ''' </summary>
    Public Class RemoteDebugDeviceNotFoundException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there is an FTP filesystem error
    ''' </summary>
    Public Class FTPFilesystemException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there is an SFTP filesystem error
    ''' </summary>
    Public Class SFTPFilesystemException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there is a mail error
    ''' </summary>
    Public Class MailException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there is no such mail directory
    ''' </summary>
    Public Class NoSuchMailDirectoryException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there is already a device
    ''' </summary>
    Public Class RemoteDebugDeviceAlreadyExistsException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there is an error in FTP shell
    ''' </summary>
    Public Class FTPShellException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there is an error in SFTP shell
    ''' </summary>
    Public Class SFTPShellException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there is an error in RSS shell
    ''' </summary>
    Public Class RSSShellException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there is an error in FTP network
    ''' </summary>
    Public Class FTPNetworkException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there is an error in SFTP network
    ''' </summary>
    Public Class SFTPNetworkException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there is an error in RSS network
    ''' </summary>
    Public Class RSSNetworkException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when the RSS feed type is invalid
    ''' </summary>
    Public Class InvalidFeedTypeException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when the specified hash is invalid
    ''' </summary>
    Public Class InvalidHashException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when the specified hash algorithm is invalid
    ''' </summary>
    Public Class InvalidHashAlgorithmException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when the general filesystem error occurs
    ''' </summary>
    Public Class FilesystemException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when the console read timeout occurs
    ''' </summary>
    Public Class ConsoleReadTimeoutException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when the screensaver management fails
    ''' </summary>
    Public Class ScreensaverManagementException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when the mod has zero parts
    ''' </summary>
    Public Class ModNoPartsException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when the manpage was invalid
    ''' </summary>
    Public Class InvalidManpageException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Invalid kernel path specified
    ''' </summary>
    Public Class InvalidKernelPathException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Invalid path specified
    ''' </summary>
    Public Class InvalidPathException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there was a kernel error. Used for <see cref="KernelError(KernelErrorLevel, Boolean, Long, String, Exception, Object())"/>
    ''' </summary>
    Public Class KernelErrorException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there was an installation failure for the mod.
    ''' </summary>
    Public Class ModInstallException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there was a uninstallation failure for the mod.
    ''' </summary>
    Public Class ModUninstallException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there is an error in HTTP shell
    ''' </summary>
    Public Class HTTPShellException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there was an installation failure for the language.
    ''' </summary>
    Public Class LanguageInstallException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there was a uninstallation failure for the language.
    ''' </summary>
    Public Class LanguageUninstallException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

    ''' <summary>
    ''' Thrown when there was a parse failure for the language.
    ''' </summary>
    Public Class LanguageParseException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(String.Format(message, vars), e)
        End Sub
    End Class

End Class
