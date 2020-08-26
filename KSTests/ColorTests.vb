
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

Imports System.IO
Imports KS

<TestClass()> Public Class ColorTests

    ''' <summary>
    ''' Tests template setting (all templates)
    ''' </summary>
    <TestMethod()> Public Sub TestSetTemplate()
        Dim Comparison As String() = {}
        InitPaths()
        Dim ExpectedTemplates As New List(Of String)
        Dim ActualTemplates As New List(Of String)
        Dim PathToTestConfig As String = Path.GetFullPath("TestConfig.ini")
        If Not File.Exists(paths("Configuration")) Then File.Copy(PathToTestConfig, paths("Configuration"))
        ExpectedTemplates.AddRange(colorTemplates)
        For Each Template As String In colorTemplates
            TemplateSet(Template)
            ActualTemplates.Add(currentTheme)
        Next
        Comparison = ExpectedTemplates.ToArray.Except(ActualTemplates.ToArray).ToArray
        Assert.IsTrue(Comparison.Count = 0, "Themes are not properly parsed. Comparison count is {0} Check the vars for below colors, the KS > Color > ParseCurrentTheme, and the KS > Color > TemplateSet." + vbNewLine + vbNewLine +
                                            "Themes that are parsed incorrectly will be written below:" + vbNewLine + vbNewLine +
                                            "- " + Join(Comparison, vbNewLine + "- "), Comparison.Length)
    End Sub

End Class