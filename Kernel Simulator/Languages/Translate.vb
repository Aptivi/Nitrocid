
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

Public Module Translate

    'This is not final! This is here only for preparation for 0.0.5.9's features.
    'Files like ger.txt, spa.txt, etc. are only stubs, and are unfinished. 
    'It does not contain all of the strings, but contains only some strings.
    'If anyone want to contribute, please feel free to do so.

    'FOR INDIA: (EN) Please be aware that the terminal won't be able to use Hindi letters, so we will use alternative way.
    '           (IN) कृपया ध्यान रखें कि टर्मिनल हिंदी अक्षरों का उपयोग करने में सक्षम नहीं होगा, इसलिए हम वैकल्पिक तरीके से उपयोग करेंगे।

    ''' <summary>
    ''' We haven't implemented anything yet.
    ''' </summary>
    ''' <param name="text">Any string that exists in Kernel Simulator's translation files or mod files</param>
    ''' <param name="lang">3 letter language</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DoTranslation(ByVal text As String, ByVal lang As String) As String
        Throw New NotImplementedException
    End Function

End Module
