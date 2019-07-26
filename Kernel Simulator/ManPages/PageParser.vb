
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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

Module PageParser

    'Variables
    Public Pages As New Dictionary(Of String, Manual)
    Public AvailablePages() As String = { 'English manuals below
                                         "Introduction to the Kernel", "Available manual pages", "Available commands", "History of Kernel Simulator",
                                         "Available FTP commands", "Modding guide", "Screensaver modding guide", "adduser", "alias", "arginj",
                                         "calc", "chdir", "chhostname", "chmal", "chmotd", "chpwd", "chusrname", "cls", "debuglog", "FTP changelocaldir", "FTP cdl",
                                         "FTP changeremotedir", "FTP cdr", "FTP connect", "FTP currlocaldir", "FTP pwdl", "FTP currremotedir", "FTP pwdr", "FTP delete",
                                         "FTP del", "FTP disconnect", "FTP download", "FTP get", "FTP listlocal", "FTP lsl", "FTP listremote", "FTP lsr", "FTP rename",
                                         "FTP ren", "FTP upload", "FTP put", "ftp", "list", "reloadsaver", "lockscreen", "logout", "lscomp", "lsnet", "lsnettree",
                                         "md", "netinfo", "noaliases", "perm", "ping", "rd", "read", "reboot", "reloadconfig", "rmuser", "savescreen", "scical", "setcolors",
                                         "setsaver", "setthemes", "showmotd", "showtd", "showtdzone", "shutdown", "sysinfo", "unitconv", "useddeps", "Available command-line arguments",
                                         "Available kernel arguments", "Configuration for your Kernel", "cdbglog", "chlang", "sses", 'Chinese manuals below
                                         "chiIntroduction to the Kernel", "chiAvailable manual pages", "chiAvailable commands", "chiHistory of Kernel Simulator",
                                         "chiAvailable FTP commands", "chiModding guide", "chiScreensaver modding guide", "chiadduser", "chialias", "chiarginj",
                                         "chicalc", "chichdir", "chichhostname", "chichmal", "chichmotd", "chichpwd", "chichusrname", "chicls", "chidebuglog", "chiFTP changelocaldir", "chiFTP cdl",
                                         "chiFTP changeremotedir", "chiFTP cdr", "chiFTP connect", "chiFTP currlocaldir", "chiFTP pwdl", "chiFTP currremotedir", "chiFTP pwdr", "chiFTP delete",
                                         "chiFTP del", "chiFTP disconnect", "chiFTP download", "chiFTP get", "chiFTP listlocal", "chiFTP lsl", "chiFTP listremote", "chiFTP lsr", "chiFTP rename",
                                         "chiFTP ren", "chiFTP upload", "chiFTP put", "chiftp", "chilist", "chireloadsaver", "chilockscreen", "chilogout", "chilscomp", "chilsnet", "chilsnettree",
                                         "chimd", "chinetinfo", "chinoaliases", "chiperm", "chiping", "chird", "chiread", "chireboot", "chireloadconfig", "chirmuser", "chisavescreen", "chiscical", "chisetcolors",
                                         "chisetsaver", "chisetthemes", "chishowmotd", "chishowtd", "chishowtdzone", "chishutdown", "chisysinfo", "chiunitconv", "chiuseddeps", "chiAvailable command-line arguments",
                                         "chiAvailable kernel arguments", "chiConfiguration for your Kernel", "chicdbglog", "chichlang", "chisses", 'Czech manuals below
                                         "czeIntroduction to the Kernel", "czeAvailable manual pages", "czeAvailable commands", "czeHistory of Kernel Simulator",
                                         "czeAvailable FTP commands", "czeModding guide", "czeScreensaver modding guide", "czeadduser", "czealias", "czearginj",
                                         "czecalc", "czechdir", "czechhostname", "czechmal", "czechmotd", "czechpwd", "czechusrname", "czecls", "czedebuglog", "czeFTP changelocaldir", "czeFTP cdl",
                                         "czeFTP changeremotedir", "czeFTP cdr", "czeFTP connect", "czeFTP currlocaldir", "czeFTP pwdl", "czeFTP currremotedir", "czeFTP pwdr", "czeFTP delete",
                                         "czeFTP del", "czeFTP disconnect", "czeFTP download", "czeFTP get", "czeFTP listlocal", "czeFTP lsl", "czeFTP listremote", "czeFTP lsr", "czeFTP rename",
                                         "czeFTP ren", "czeFTP upload", "czeFTP put", "czeftp", "czelist", "czereloadsaver", "czelockscreen", "czelogout", "czelscomp", "czelsnet", "czelsnettree",
                                         "czemd", "czenetinfo", "czenoaliases", "czeperm", "czeping", "czerd", "czeread", "czereboot", "czereloadconfig", "czermuser", "czesavescreen", "czescical", "czesetcolors",
                                         "czesetsaver", "czesetthemes", "czeshowmotd", "czeshowtd", "czeshowtdzone", "czeshutdown", "czesysinfo", "czeunitconv", "czeuseddeps", "czeAvailable command-line arguments",
                                         "czeAvailable kernel arguments", "czeConfiguration for your Kernel", "czecdbglog", "czechlang", "czesses", 'Dutch manuals below
                                         "dtcIntroduction to the Kernel", "dtcAvailable manual pages", "dtcAvailable commands", "dtcHistory of Kernel Simulator",
                                         "dtcAvailable FTP commands", "dtcModding guide", "dtcScreensaver modding guide", "dtcadduser", "dtcalias", "dtcarginj",
                                         "dtccalc", "dtcchdir", "dtcchhostname", "dtcchmal", "dtcchmotd", "dtcchpwd", "dtcchusrname", "dtccls", "dtcdebuglog", "dtcFTP changelocaldir", "dtcFTP cdl",
                                         "dtcFTP changeremotedir", "dtcFTP cdr", "dtcFTP connect", "dtcFTP currlocaldir", "dtcFTP pwdl", "dtcFTP currremotedir", "dtcFTP pwdr", "dtcFTP delete",
                                         "dtcFTP del", "dtcFTP disconnect", "dtcFTP download", "dtcFTP get", "dtcFTP listlocal", "dtcFTP lsl", "dtcFTP listremote", "dtcFTP lsr", "dtcFTP rename",
                                         "dtcFTP ren", "dtcFTP upload", "dtcFTP put", "dtcftp", "dtclist", "dtcreloadsaver", "dtclockscreen", "dtclogout", "dtclscomp", "dtclsnet", "dtclsnettree",
                                         "dtcmd", "dtcnetinfo", "dtcnoaliases", "dtcperm", "dtcping", "dtcrd", "dtcread", "dtcreboot", "dtcreloadconfig", "dtcrmuser", "dtcsavescreen", "dtcscical", "dtcsetcolors",
                                         "dtcsetsaver", "dtcsetthemes", "dtcshowmotd", "dtcshowtd", "dtcshowtdzone", "dtcshutdown", "dtcsysinfo", "dtcunitconv", "dtcuseddeps", "dtcAvailable command-line arguments",
                                         "dtcAvailable kernel arguments", "dtcConfiguration for your Kernel", "dtccdbglog", "dtcchlang", "dtcsses", 'Finnish manuals below
                                         "finIntroduction to the Kernel", "finAvailable manual pages", "finAvailable commands", "finHistory of Kernel Simulator",
                                         "finAvailable FTP commands", "finModding guide", "finScreensaver modding guide", "finadduser", "finalias", "finarginj",
                                         "fincalc", "finchdir", "finchhostname", "finchmal", "finchmotd", "finchpwd", "finchusrname", "fincls", "findebuglog", "finFTP changelocaldir", "finFTP cdl",
                                         "finFTP changeremotedir", "finFTP cdr", "finFTP connect", "finFTP currlocaldir", "finFTP pwdl", "finFTP currremotedir", "finFTP pwdr", "finFTP delete",
                                         "finFTP del", "finFTP disconnect", "finFTP download", "finFTP get", "finFTP listlocal", "finFTP lsl", "finFTP listremote", "finFTP lsr", "finFTP rename",
                                         "finFTP ren", "finFTP upload", "finFTP put", "finftp", "finlist", "finreloadsaver", "finlockscreen", "finlogout", "finlscomp", "finlsnet", "finlsnettree",
                                         "finmd", "finnetinfo", "finnoaliases", "finperm", "finping", "finrd", "finread", "finreboot", "finreloadconfig", "finrmuser", "finsavescreen", "finscical", "finsetcolors",
                                         "finsetsaver", "finsetthemes", "finshowmotd", "finshowtd", "finshowtdzone", "finshutdown", "finsysinfo", "finunitconv", "finuseddeps", "finAvailable command-line arguments",
                                         "finAvailable kernel arguments", "finConfiguration for your Kernel", "fincdbglog", "finchlang", "finsses", 'French manuals below
                                         "freIntroduction to the Kernel", "freAvailable manual pages", "freAvailable commands", "freHistory of Kernel Simulator",
                                         "freAvailable FTP commands", "freModding guide", "freScreensaver modding guide", "freadduser", "frealias", "frearginj",
                                         "frecalc", "frechdir", "frechhostname", "frechmal", "frechmotd", "frechpwd", "frechusrname", "frecls", "fredebuglog", "freFTP changelocaldir", "freFTP cdl",
                                         "freFTP changeremotedir", "freFTP cdr", "freFTP connect", "freFTP currlocaldir", "freFTP pwdl", "freFTP currremotedir", "freFTP pwdr", "freFTP delete",
                                         "freFTP del", "freFTP disconnect", "freFTP download", "freFTP get", "freFTP listlocal", "freFTP lsl", "freFTP listremote", "freFTP lsr", "freFTP rename",
                                         "freFTP ren", "freFTP upload", "freFTP put", "freftp", "frelist", "frereloadsaver", "frelockscreen", "frelogout", "frelscomp", "frelsnet", "frelsnettree",
                                         "fremd", "frenetinfo", "frenoaliases", "freperm", "freping", "frerd", "freread", "frereboot", "frereloadconfig", "frermuser", "fresavescreen", "frescical", "fresetcolors",
                                         "fresetsaver", "fresetthemes", "freshowmotd", "freshowtd", "freshowtdzone", "freshutdown", "fresysinfo", "freunitconv", "freuseddeps", "freAvailable command-line arguments",
                                         "freAvailable kernel arguments", "freConfiguration for your Kernel", "frecdbglog", "frechlang", "fresses", 'German manuals below
                                         "gerIntroduction to the Kernel", "gerAvailable manual pages", "gerAvailable commands", "gerHistory of Kernel Simulator",
                                         "gerAvailable FTP commands", "gerModding guide", "gerScreensaver modding guide", "geradduser", "geralias", "gerarginj",
                                         "gercalc", "gerchdir", "gerchhostname", "gerchmal", "gerchmotd", "gerchpwd", "gerchusrname", "gercls", "gerdebuglog", "gerFTP changelocaldir", "gerFTP cdl",
                                         "gerFTP changeremotedir", "gerFTP cdr", "gerFTP connect", "gerFTP currlocaldir", "gerFTP pwdl", "gerFTP currremotedir", "gerFTP pwdr", "gerFTP delete",
                                         "gerFTP del", "gerFTP disconnect", "gerFTP download", "gerFTP get", "gerFTP listlocal", "gerFTP lsl", "gerFTP listremote", "gerFTP lsr", "gerFTP rename",
                                         "gerFTP ren", "gerFTP upload", "gerFTP put", "gerftp", "gerlist", "gerreloadsaver", "gerlockscreen", "gerlogout", "gerlscomp", "gerlsnet", "gerlsnettree",
                                         "germd", "gernetinfo", "gernoaliases", "gerperm", "gerping", "gerrd", "gerread", "gerreboot", "gerreloadconfig", "gerrmuser", "gersavescreen", "gerscical", "gersetcolors",
                                         "gersetsaver", "gersetthemes", "gershowmotd", "gershowtd", "gershowtdzone", "gershutdown", "gersysinfo", "gerunitconv", "geruseddeps", "gerAvailable command-line arguments",
                                         "gerAvailable kernel arguments", "gerConfiguration for your Kernel", "gercdbglog", "gerchlang", "gersses", 'Hindi manuals below
                                         "indIntroduction to the Kernel", "indAvailable manual pages", "indAvailable commands", "indHistory of Kernel Simulator",
                                         "indAvailable FTP commands", "indModding guide", "indScreensaver modding guide", "indadduser", "indalias", "indarginj",
                                         "indcalc", "indchdir", "indchhostname", "indchmal", "indchmotd", "indchpwd", "indchusrname", "indcls", "inddebuglog", "indFTP changelocaldir", "indFTP cdl",
                                         "indFTP changeremotedir", "indFTP cdr", "indFTP connect", "indFTP currlocaldir", "indFTP pwdl", "indFTP currremotedir", "indFTP pwdr", "indFTP delete",
                                         "indFTP del", "indFTP disconnect", "indFTP download", "indFTP get", "indFTP listlocal", "indFTP lsl", "indFTP listremote", "indFTP lsr", "indFTP rename",
                                         "indFTP ren", "indFTP upload", "indFTP put", "indftp", "indlist", "indreloadsaver", "indlockscreen", "indlogout", "indlscomp", "indlsnet", "indlsnettree",
                                         "indmd", "indnetinfo", "indnoaliases", "indperm", "indping", "indrd", "indread", "indreboot", "indreloadconfig", "indrmuser", "indsavescreen", "indscical", "indsetcolors",
                                         "indsetsaver", "indsetthemes", "indshowmotd", "indshowtd", "indshowtdzone", "indshutdown", "indsysinfo", "indunitconv", "induseddeps", "indAvailable command-line arguments",
                                         "indAvailable kernel arguments", "indConfiguration for your Kernel", "indcdbglog", "indchlang", "indsses", 'Italian manuals below
                                         "itaIntroduction to the Kernel", "itaAvailable manual pages", "itaAvailable commands", "itaHistory of Kernel Simulator",
                                         "itaAvailable FTP commands", "itaModding guide", "itaScreensaver modding guide", "itaadduser", "itaalias", "itaarginj",
                                         "itacalc", "itachdir", "itachhostname", "itachmal", "itachmotd", "itachpwd", "itachusrname", "itacls", "itadebuglog", "itaFTP changelocaldir", "itaFTP cdl",
                                         "itaFTP changeremotedir", "itaFTP cdr", "itaFTP connect", "itaFTP currlocaldir", "itaFTP pwdl", "itaFTP currremotedir", "itaFTP pwdr", "itaFTP delete",
                                         "itaFTP del", "itaFTP disconnect", "itaFTP download", "itaFTP get", "itaFTP listlocal", "itaFTP lsl", "itaFTP listremote", "itaFTP lsr", "itaFTP rename",
                                         "itaFTP ren", "itaFTP upload", "itaFTP put", "itaftp", "italist", "itareloadsaver", "italockscreen", "italogout", "italscomp", "italsnet", "italsnettree",
                                         "itamd", "itanetinfo", "itanoaliases", "itaperm", "itaping", "itard", "itaread", "itareboot", "itareloadconfig", "itarmuser", "itasavescreen", "itascical", "itasetcolors",
                                         "itasetsaver", "itasetthemes", "itashowmotd", "itashowtd", "itashowtdzone", "itashutdown", "itasysinfo", "itaunitconv", "itauseddeps", "itaAvailable command-line arguments",
                                         "itaAvailable kernel arguments", "itaConfiguration for your Kernel", "itacdbglog", "itachlang", "itasses", 'Malay manuals below
                                         "malIntroduction to the Kernel", "malAvailable manual pages", "malAvailable commands", "malHistory of Kernel Simulator",
                                         "malAvailable FTP commands", "malModding guide", "malScreensaver modding guide", "maladduser", "malalias", "malarginj",
                                         "malcalc", "malchdir", "malchhostname", "malchmal", "malchmotd", "malchpwd", "malchusrname", "malcls", "maldebuglog", "malFTP changelocaldir", "malFTP cdl",
                                         "malFTP changeremotedir", "malFTP cdr", "malFTP connect", "malFTP currlocaldir", "malFTP pwdl", "malFTP currremotedir", "malFTP pwdr", "malFTP delete",
                                         "malFTP del", "malFTP disconnect", "malFTP download", "malFTP get", "malFTP listlocal", "malFTP lsl", "malFTP listremote", "malFTP lsr", "malFTP rename",
                                         "malFTP ren", "malFTP upload", "malFTP put", "malftp", "mallist", "malreloadsaver", "mallockscreen", "mallogout", "mallscomp", "mallsnet", "mallsnettree",
                                         "malmd", "malnetinfo", "malnoaliases", "malperm", "malping", "malrd", "malread", "malreboot", "malreloadconfig", "malrmuser", "malsavescreen", "malscical", "malsetcolors",
                                         "malsetsaver", "malsetthemes", "malshowmotd", "malshowtd", "malshowtdzone", "malshutdown", "malsysinfo", "malunitconv", "maluseddeps", "malAvailable command-line arguments",
                                         "malAvailable kernel arguments", "malConfiguration for your Kernel", "malcdbglog", "malchlang", "malsses", 'Portuguese manuals below
                                         "ptgIntroduction to the Kernel", "ptgAvailable manual pages", "ptgAvailable commands", "ptgHistory of Kernel Simulator",
                                         "ptgAvailable FTP commands", "ptgModding guide", "ptgScreensaver modding guide", "ptgadduser", "ptgalias", "ptgarginj",
                                         "ptgcalc", "ptgchdir", "ptgchhostname", "ptgchmal", "ptgchmotd", "ptgchpwd", "ptgchusrname", "ptgcls", "ptgdebuglog", "ptgFTP changelocaldir", "ptgFTP cdl",
                                         "ptgFTP changeremotedir", "ptgFTP cdr", "ptgFTP connect", "ptgFTP currlocaldir", "ptgFTP pwdl", "ptgFTP currremotedir", "ptgFTP pwdr", "ptgFTP delete",
                                         "ptgFTP del", "ptgFTP disconnect", "ptgFTP download", "ptgFTP get", "ptgFTP listlocal", "ptgFTP lsl", "ptgFTP listremote", "ptgFTP lsr", "ptgFTP rename",
                                         "ptgFTP ren", "ptgFTP upload", "ptgFTP put", "ptgftp", "ptglist", "ptgreloadsaver", "ptglockscreen", "ptglogout", "ptglscomp", "ptglsnet", "ptglsnettree",
                                         "ptgmd", "ptgnetinfo", "ptgnoaliases", "ptgperm", "ptgping", "ptgrd", "ptgread", "ptgreboot", "ptgreloadconfig", "ptgrmuser", "ptgsavescreen", "ptgscical", "ptgsetcolors",
                                         "ptgsetsaver", "ptgsetthemes", "ptgshowmotd", "ptgshowtd", "ptgshowtdzone", "ptgshutdown", "ptgsysinfo", "ptgunitconv", "ptguseddeps", "ptgAvailable command-line arguments",
                                         "ptgAvailable kernel arguments", "ptgConfiguration for your Kernel", "ptgcdbglog", "ptgchlang", "ptgsses", 'Spanish manuals below
                                         "spaIntroduction to the Kernel", "spaAvailable manual pages", "spaAvailable commands", "spaHistory of Kernel Simulator",
                                         "spaAvailable FTP commands", "spaModding guide", "spaScreensaver modding guide", "spaadduser", "spaalias", "spaarginj",
                                         "spacalc", "spachdir", "spachhostname", "spachmal", "spachmotd", "spachpwd", "spachusrname", "spacls", "spadebuglog", "spaFTP changelocaldir", "spaFTP cdl",
                                         "spaFTP changeremotedir", "spaFTP cdr", "spaFTP connect", "spaFTP currlocaldir", "spaFTP pwdl", "spaFTP currremotedir", "spaFTP pwdr", "spaFTP delete",
                                         "spaFTP del", "spaFTP disconnect", "spaFTP download", "spaFTP get", "spaFTP listlocal", "spaFTP lsl", "spaFTP listremote", "spaFTP lsr", "spaFTP rename",
                                         "spaFTP ren", "spaFTP upload", "spaFTP put", "spaftp", "spalist", "spareloadsaver", "spalockscreen", "spalogout", "spalscomp", "spalsnet", "spalsnettree",
                                         "spamd", "spanetinfo", "spanoaliases", "spaperm", "spaping", "spard", "sparead", "spareboot", "spareloadconfig", "sparmuser", "spasavescreen", "spascical", "spasetcolors",
                                         "spasetsaver", "spasetthemes", "spashowmotd", "spashowtd", "spashowtdzone", "spashutdown", "spasysinfo", "spaunitconv", "spauseddeps", "spaAvailable command-line arguments",
                                         "spaAvailable kernel arguments", "spaConfiguration for your Kernel", "spacdbglog", "spachlang", "spasses", 'Swedish manuals below
                                         "sweIntroduction to the Kernel", "sweAvailable manual pages", "sweAvailable commands", "sweHistory of Kernel Simulator",
                                         "sweAvailable FTP commands", "sweModding guide", "sweScreensaver modding guide", "sweadduser", "swealias", "swearginj",
                                         "swecalc", "swechdir", "swechhostname", "swechmal", "swechmotd", "swechpwd", "swechusrname", "swecls", "swedebuglog", "sweFTP changelocaldir", "sweFTP cdl",
                                         "sweFTP changeremotedir", "sweFTP cdr", "sweFTP connect", "sweFTP currlocaldir", "sweFTP pwdl", "sweFTP currremotedir", "sweFTP pwdr", "sweFTP delete",
                                         "sweFTP del", "sweFTP disconnect", "sweFTP download", "sweFTP get", "sweFTP listlocal", "sweFTP lsl", "sweFTP listremote", "sweFTP lsr", "sweFTP rename",
                                         "sweFTP ren", "sweFTP upload", "sweFTP put", "sweftp", "swelist", "swereloadsaver", "swelockscreen", "swelogout", "swelscomp", "swelsnet", "swelsnettree",
                                         "swemd", "swenetinfo", "swenoaliases", "sweperm", "sweping", "swerd", "sweread", "swereboot", "swereloadconfig", "swermuser", "swesavescreen", "swescical", "swesetcolors",
                                         "swesetsaver", "swesetthemes", "sweshowmotd", "sweshowtd", "sweshowtdzone", "sweshutdown", "swesysinfo", "sweunitconv", "sweuseddeps", "sweAvailable command-line arguments",
                                         "sweAvailable kernel arguments", "sweConfiguration for your Kernel", "swecdbglog", "swechlang", "swesses", 'Turkish manuals below
                                         "tkyIntroduction to the Kernel", "tkyAvailable manual pages", "tkyAvailable commands", "tkyHistory of Kernel Simulator",
                                         "tkyAvailable FTP commands", "tkyModding guide", "tkyScreensaver modding guide", "tkyadduser", "tkyalias", "tkyarginj",
                                         "tkycalc", "tkychdir", "tkychhostname", "tkychmal", "tkychmotd", "tkychpwd", "tkychusrname", "tkycls", "tkydebuglog", "tkyFTP changelocaldir", "tkyFTP cdl",
                                         "tkyFTP changeremotedir", "tkyFTP cdr", "tkyFTP connect", "tkyFTP currlocaldir", "tkyFTP pwdl", "tkyFTP currremotedir", "tkyFTP pwdr", "tkyFTP delete",
                                         "tkyFTP del", "tkyFTP disconnect", "tkyFTP download", "tkyFTP get", "tkyFTP listlocal", "tkyFTP lsl", "tkyFTP listremote", "tkyFTP lsr", "tkyFTP rename",
                                         "tkyFTP ren", "tkyFTP upload", "tkyFTP put", "tkyftp", "tkylist", "tkyreloadsaver", "tkylockscreen", "tkylogout", "tkylscomp", "tkylsnet", "tkylsnettree",
                                         "tkymd", "tkynetinfo", "tkynoaliases", "tkyperm", "tkyping", "tkyrd", "tkyread", "tkyreboot", "tkyreloadconfig", "tkyrmuser", "tkysavescreen", "tkyscical", "tkysetcolors",
                                         "tkysetsaver", "tkysetthemes", "tkyshowmotd", "tkyshowtd", "tkyshowtdzone", "tkyshutdown", "tkysysinfo", "tkyunitconv", "tkyuseddeps", "tkyAvailable command-line arguments",
                                         "tkyAvailable kernel arguments", "tkyConfiguration for your Kernel", "tkycdbglog", "tkychlang", "tkysses"}
    Public AvailableLayouts() As String = {"0.0.5.9-OR-ABOVE"}
    Private InternalParseDone As Boolean = False
    Private ManTitle As String
    Private BodyParsing As Boolean = False
    Private ColorParsing As Boolean = False
    Private SectionParsing As Boolean = False
    Private UnknownTitleCount As Integer = 0

    'Initializes manual pages
    Sub InitMan()
        For Each titleMan As String In AvailablePages
            If Not Pages.ContainsKey(titleMan) Then
                Pages.Add(titleMan, New Manual(titleMan))
                CheckManual(titleMan)
            End If
        Next
    End Sub

    'Checks for manual page if it's valid
    Public Sub CheckManual(ByVal Title As String)
        Try
            ManTitle = Title
            If AvailablePages.Contains(ManTitle) Then
                Dim manLines As String() = {}
                Select Case ManTitle
                    'English manuals
                    Case "Introduction to the Kernel"
                        manLines = My.Resources.Introduction_to_the_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    Case "Available manual pages"
                        manLines = My.Resources.Available_manual_pages.Replace(Chr(13), "").Split(Chr(10))
                    Case "Available commands"
                        manLines = My.Resources.Available_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "History of Kernel Simulator"
                        manLines = My.Resources.History_of_Kernel_Simulator.Replace(Chr(13), "").Split(Chr(10))
                    Case "Available FTP commands"
                        manLines = My.Resources.Available_FTP_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "Modding guide"
                        manLines = My.Resources.Modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "Screensaver modding guide"
                        manLines = My.Resources.Screensaver_modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "adduser"
                        manLines = My.Resources.adduser.Replace(Chr(13), "").Split(Chr(10))
                    Case "alias"
                        manLines = My.Resources._alias.Replace(Chr(13), "").Split(Chr(10))
                    Case "arginj"
                        manLines = My.Resources.arginj.Replace(Chr(13), "").Split(Chr(10))
                    Case "calc"
                        manLines = My.Resources.calc.Replace(Chr(13), "").Split(Chr(10))
                    Case "chdir"
                        manLines = My.Resources.chdir.Replace(Chr(13), "").Split(Chr(10))
                    Case "chhostname"
                        manLines = My.Resources.chhostname.Replace(Chr(13), "").Split(Chr(10))
                    Case "chmal"
                        manLines = My.Resources.chmal.Replace(Chr(13), "").Split(Chr(10))
                    Case "chmotd"
                        manLines = My.Resources.chmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "chpwd"
                        manLines = My.Resources.chpwd.Replace(Chr(13), "").Split(Chr(10))
                    Case "chusrname"
                        manLines = My.Resources.chusrname.Replace(Chr(13), "").Split(Chr(10))
                    Case "cls"
                        manLines = My.Resources.cls.Replace(Chr(13), "").Split(Chr(10))
                    Case "debuglog"
                        manLines = My.Resources.debuglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP changelocaldir", "FTP cdl"
                        manLines = My.Resources.FTP_changelocaldir_or_cdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP changeremotedir", "FTP cdr"
                        manLines = My.Resources.FTP_changeremotedir_or_cdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP connect"
                        manLines = My.Resources.FTP_connect.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP currlocaldir", "FTP pwdl"
                        manLines = My.Resources.FTP_currlocaldir_or_pwdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP currremotedir", "FTP pwdr"
                        manLines = My.Resources.FTP_currremotedir_or_pwdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP delete", "FTP del"
                        manLines = My.Resources.FTP_delete_or_del.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP disconnect"
                        manLines = My.Resources.FTP_disconnect.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP download", "FTP get"
                        manLines = My.Resources.FTP_download_or_get.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP listlocal", "FTP lsl"
                        manLines = My.Resources.FTP_listlocal_or_lsl.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP listremote", "FTP lsr"
                        manLines = My.Resources.FTP_listremote_or_lsr.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP rename", "FTP ren"
                        manLines = My.Resources.FTP_rename_or_ren.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP upload", "FTP put"
                        manLines = My.Resources.FTP_upload_or_put.Replace(Chr(13), "").Split(Chr(10))
                    Case "ftp"
                        manLines = My.Resources.ftp.Replace(Chr(13), "").Split(Chr(10))
                    Case "list"
                        manLines = My.Resources.list.Replace(Chr(13), "").Split(Chr(10))
                    Case "reloadsaver"
                        manLines = My.Resources.reloadsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "lockscreen"
                        manLines = My.Resources.lockscreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "logout"
                        manLines = My.Resources.logout.Replace(Chr(13), "").Split(Chr(10))
                    Case "lscomp"
                        manLines = My.Resources.lscomp.Replace(Chr(13), "").Split(Chr(10))
                    Case "lsnet"
                        manLines = My.Resources.lsnet.Replace(Chr(13), "").Split(Chr(10))
                    Case "lsnettree"
                        manLines = My.Resources.lsnettree.Replace(Chr(13), "").Split(Chr(10))
                    Case "md"
                        manLines = My.Resources.md.Replace(Chr(13), "").Split(Chr(10))
                    Case "netinfo"
                        manLines = My.Resources.netinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "noaliases"
                        manLines = My.Resources.noaliases.Replace(Chr(13), "").Split(Chr(10))
                    Case "perm"
                        manLines = My.Resources.perm.Replace(Chr(13), "").Split(Chr(10))
                    Case "ping"
                        manLines = My.Resources.ping.Replace(Chr(13), "").Split(Chr(10))
                    Case "rd"
                        manLines = My.Resources.rd.Replace(Chr(13), "").Split(Chr(10))
                    Case "read"
                        manLines = My.Resources.read.Replace(Chr(13), "").Split(Chr(10))
                    Case "reboot"
                        manLines = My.Resources.reboot.Replace(Chr(13), "").Split(Chr(10))
                    Case "reloadconfig"
                        manLines = My.Resources.reloadconfig.Replace(Chr(13), "").Split(Chr(10))
                    Case "rmuser"
                        manLines = My.Resources.rmuser.Replace(Chr(13), "").Split(Chr(10))
                    Case "savescreen"
                        manLines = My.Resources.savescreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "scical"
                        manLines = My.Resources.scical.Replace(Chr(13), "").Split(Chr(10))
                    Case "setcolors"
                        manLines = My.Resources.setcolors.Replace(Chr(13), "").Split(Chr(10))
                    Case "setsaver"
                        manLines = My.Resources.setsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "setthemes"
                        manLines = My.Resources.setthemes.Replace(Chr(13), "").Split(Chr(10))
                    Case "showmotd"
                        manLines = My.Resources.showmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "showtd"
                        manLines = My.Resources.showtd.Replace(Chr(13), "").Split(Chr(10))
                    Case "showtdzone"
                        manLines = My.Resources.showtdzone.Replace(Chr(13), "").Split(Chr(10))
                    Case "shutdown"
                        manLines = My.Resources.shutdown.Replace(Chr(13), "").Split(Chr(10))
                    Case "sysinfo"
                        manLines = My.Resources.sysinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "unitconv"
                        manLines = My.Resources.unitconv.Replace(Chr(13), "").Split(Chr(10))
                    Case "useddeps"
                        manLines = My.Resources.useddeps.Replace(Chr(13), "").Split(Chr(10))
                    Case "Available command-line arguments"
                        manLines = My.Resources.Available_command_line_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "Available kernel arguments"
                        manLines = My.Resources.Available_kernel_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "Configuration for your Kernel"
                        manLines = My.Resources.Configuration_for_your_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    'Newly added manuals
                    Case "chlang"
                        manLines = My.Resources.chlang.Replace(Chr(13), "").Split(Chr(10))
                    Case "cdbglog"
                        manLines = My.Resources.cdbglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "sses"
                        manLines = My.Resources.sses.Replace(Chr(13), "").Split(Chr(10))

                    'Chinese manuals
                    Case "chiIntroduction to the Kernel"
                        manLines = My.Resources.zh_Introduction_to_the_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiAvailable manual pages"
                        manLines = My.Resources.zh_Available_manual_pages.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiAvailable commands"
                        manLines = My.Resources.zh_Available_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiHistory of Kernel Simulator"
                        manLines = My.Resources.zh_History_of_Kernel_Simulator.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiAvailable FTP commands"
                        manLines = My.Resources.zh_Available_FTP_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiModding guide"
                        manLines = My.Resources.zh_Modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiScreensaver modding guide"
                        manLines = My.Resources.zh_Screensaver_modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiadduser"
                        manLines = My.Resources.zh_adduser.Replace(Chr(13), "").Split(Chr(10))
                    Case "chialias"
                        manLines = My.Resources.zh_alias.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiarginj"
                        manLines = My.Resources.zh_arginj.Replace(Chr(13), "").Split(Chr(10))
                    Case "chicalc"
                        manLines = My.Resources.zh_calc.Replace(Chr(13), "").Split(Chr(10))
                    Case "chichdir"
                        manLines = My.Resources.zh_chdir.Replace(Chr(13), "").Split(Chr(10))
                    Case "chichhostname"
                        manLines = My.Resources.zh_chhostname.Replace(Chr(13), "").Split(Chr(10))
                    Case "chichmal"
                        manLines = My.Resources.zh_chmal.Replace(Chr(13), "").Split(Chr(10))
                    Case "chichmotd"
                        manLines = My.Resources.zh_chmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "chichpwd"
                        manLines = My.Resources.zh_chpwd.Replace(Chr(13), "").Split(Chr(10))
                    Case "chichusrname"
                        manLines = My.Resources.zh_chusrname.Replace(Chr(13), "").Split(Chr(10))
                    Case "chicls"
                        manLines = My.Resources.zh_cls.Replace(Chr(13), "").Split(Chr(10))
                    Case "chidebuglog"
                        manLines = My.Resources.zh_debuglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiFTP changelocaldir", "chiFTP cdl"
                        manLines = My.Resources.zh_FTP_changelocaldir_or_cdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiFTP changeremotedir", "chiFTP cdr"
                        manLines = My.Resources.zh_FTP_changeremotedir_or_cdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiFTP connect"
                        manLines = My.Resources.zh_FTP_connect.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiFTP currlocaldir", "chiFTP pwdl"
                        manLines = My.Resources.zh_FTP_currlocaldir_or_pwdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiFTP currremotedir", "chiFTP pwdr"
                        manLines = My.Resources.zh_FTP_currremotedir_or_pwdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiFTP delete", "chiFTP del"
                        manLines = My.Resources.zh_FTP_delete_or_del.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiFTP disconnect"
                        manLines = My.Resources.zh_FTP_disconnect.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiFTP download", "chiFTP get"
                        manLines = My.Resources.zh_FTP_download_or_get.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiFTP listlocal", "chiFTP lsl"
                        manLines = My.Resources.zh_FTP_listlocal_or_lsl.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiFTP listremote", "chiFTP lsr"
                        manLines = My.Resources.zh_FTP_listremote_or_lsr.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiFTP rename", "chiFTP ren"
                        manLines = My.Resources.zh_FTP_rename_or_ren.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiFTP upload", "chiFTP put"
                        manLines = My.Resources.zh_FTP_upload_or_put.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiftp"
                        manLines = My.Resources.zh_ftp.Replace(Chr(13), "").Split(Chr(10))
                    Case "chilist"
                        manLines = My.Resources.zh_list.Replace(Chr(13), "").Split(Chr(10))
                    Case "chireloadsaver"
                        manLines = My.Resources.zh_reloadsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "chilockscreen"
                        manLines = My.Resources.zh_lockscreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "chilogout"
                        manLines = My.Resources.zh_logout.Replace(Chr(13), "").Split(Chr(10))
                    Case "chilscomp"
                        manLines = My.Resources.zh_lscomp.Replace(Chr(13), "").Split(Chr(10))
                    Case "chilsnet"
                        manLines = My.Resources.zh_lsnet.Replace(Chr(13), "").Split(Chr(10))
                    Case "chilsnettree"
                        manLines = My.Resources.zh_lsnettree.Replace(Chr(13), "").Split(Chr(10))
                    Case "chimd"
                        manLines = My.Resources.zh_md.Replace(Chr(13), "").Split(Chr(10))
                    Case "chinetinfo"
                        manLines = My.Resources.zh_netinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "chinoaliases"
                        manLines = My.Resources.zh_noaliases.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiperm"
                        manLines = My.Resources.zh_perm.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiping"
                        manLines = My.Resources.zh_ping.Replace(Chr(13), "").Split(Chr(10))
                    Case "chird"
                        manLines = My.Resources.zh_rd.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiread"
                        manLines = My.Resources.zh_read.Replace(Chr(13), "").Split(Chr(10))
                    Case "chireboot"
                        manLines = My.Resources.zh_reboot.Replace(Chr(13), "").Split(Chr(10))
                    Case "chireloadconfig"
                        manLines = My.Resources.zh_reloadconfig.Replace(Chr(13), "").Split(Chr(10))
                    Case "chirmuser"
                        manLines = My.Resources.zh_rmuser.Replace(Chr(13), "").Split(Chr(10))
                    Case "chisavescreen"
                        manLines = My.Resources.zh_savescreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiscical"
                        manLines = My.Resources.zh_scical.Replace(Chr(13), "").Split(Chr(10))
                    Case "chisetcolors"
                        manLines = My.Resources.zh_setcolors.Replace(Chr(13), "").Split(Chr(10))
                    Case "chisetsaver"
                        manLines = My.Resources.zh_setsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "chisetthemes"
                        manLines = My.Resources.zh_setthemes.Replace(Chr(13), "").Split(Chr(10))
                    Case "chishowmotd"
                        manLines = My.Resources.zh_showmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "chishowtd"
                        manLines = My.Resources.zh_showtd.Replace(Chr(13), "").Split(Chr(10))
                    Case "chishowtdzone"
                        manLines = My.Resources.zh_showtdzone.Replace(Chr(13), "").Split(Chr(10))
                    Case "chishutdown"
                        manLines = My.Resources.zh_shutdown.Replace(Chr(13), "").Split(Chr(10))
                    Case "chisysinfo"
                        manLines = My.Resources.zh_sysinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiunitconv"
                        manLines = My.Resources.zh_unitconv.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiuseddeps"
                        manLines = My.Resources.zh_useddeps.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiAvailable command-line arguments"
                        manLines = My.Resources.zh_Available_command_line_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiAvailable kernel arguments"
                        manLines = My.Resources.zh_Available_kernel_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "chiConfiguration for your Kernel"
                        manLines = My.Resources.zh_Configuration_for_your_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    'Newly added manuals
                    Case "chichlang"
                        manLines = My.Resources.zh_chlang.Replace(Chr(13), "").Split(Chr(10))
                    Case "chicdbglog"
                        manLines = My.Resources.zh_cdbglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "chisses"
                        manLines = My.Resources.zh_sses.Replace(Chr(13), "").Split(Chr(10))

                    'Czech manuals
                    Case "czeIntroduction to the Kernel"
                        manLines = My.Resources.cs_Introduction_to_the_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeAvailable manual pages"
                        manLines = My.Resources.cs_Available_manual_pages.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeAvailable commands"
                        manLines = My.Resources.cs_Available_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeHistory of Kernel Simulator"
                        manLines = My.Resources.cs_History_of_Kernel_Simulator.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeAvailable FTP commands"
                        manLines = My.Resources.cs_Available_FTP_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeModding guide"
                        manLines = My.Resources.cs_Modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeScreensaver modding guide"
                        manLines = My.Resources.cs_Screensaver_modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeadduser"
                        manLines = My.Resources.cs_adduser.Replace(Chr(13), "").Split(Chr(10))
                    Case "czealias"
                        manLines = My.Resources.cs_alias.Replace(Chr(13), "").Split(Chr(10))
                    Case "czearginj"
                        manLines = My.Resources.cs_arginj.Replace(Chr(13), "").Split(Chr(10))
                    Case "czecalc"
                        manLines = My.Resources.cs_calc.Replace(Chr(13), "").Split(Chr(10))
                    Case "czechdir"
                        manLines = My.Resources.cs_chdir.Replace(Chr(13), "").Split(Chr(10))
                    Case "czechhostname"
                        manLines = My.Resources.cs_chhostname.Replace(Chr(13), "").Split(Chr(10))
                    Case "czechmal"
                        manLines = My.Resources.cs_chmal.Replace(Chr(13), "").Split(Chr(10))
                    Case "czechmotd"
                        manLines = My.Resources.cs_chmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "czechpwd"
                        manLines = My.Resources.cs_chpwd.Replace(Chr(13), "").Split(Chr(10))
                    Case "czechusrname"
                        manLines = My.Resources.cs_chusrname.Replace(Chr(13), "").Split(Chr(10))
                    Case "czecls"
                        manLines = My.Resources.cs_cls.Replace(Chr(13), "").Split(Chr(10))
                    Case "czedebuglog"
                        manLines = My.Resources.cs_debuglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeFTP changelocaldir", "czeFTP cdl"
                        manLines = My.Resources.cs_FTP_changelocaldir_or_cdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeFTP changeremotedir", "czeFTP cdr"
                        manLines = My.Resources.cs_FTP_changeremotedir_or_cdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeFTP connect"
                        manLines = My.Resources.cs_FTP_connect.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeFTP currlocaldir", "czeFTP pwdl"
                        manLines = My.Resources.cs_FTP_currlocaldir_or_pwdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeFTP currremotedir", "czeFTP pwdr"
                        manLines = My.Resources.cs_FTP_currremotedir_or_pwdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeFTP delete", "czeFTP del"
                        manLines = My.Resources.cs_FTP_delete_or_del.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeFTP disconnect"
                        manLines = My.Resources.cs_FTP_disconnect.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeFTP download", "czeFTP get"
                        manLines = My.Resources.cs_FTP_download_or_get.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeFTP listlocal", "czeFTP lsl"
                        manLines = My.Resources.cs_FTP_listlocal_or_lsl.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeFTP listremote", "czeFTP lsr"
                        manLines = My.Resources.cs_FTP_listremote_or_lsr.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeFTP rename", "czeFTP ren"
                        manLines = My.Resources.cs_FTP_rename_or_ren.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeFTP upload", "czeFTP put"
                        manLines = My.Resources.cs_FTP_upload_or_put.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeftp"
                        manLines = My.Resources.cs_ftp.Replace(Chr(13), "").Split(Chr(10))
                    Case "czelist"
                        manLines = My.Resources.cs_list.Replace(Chr(13), "").Split(Chr(10))
                    Case "czereloadsaver"
                        manLines = My.Resources.cs_reloadsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "czelockscreen"
                        manLines = My.Resources.cs_lockscreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "czelogout"
                        manLines = My.Resources.cs_logout.Replace(Chr(13), "").Split(Chr(10))
                    Case "czelscomp"
                        manLines = My.Resources.cs_lscomp.Replace(Chr(13), "").Split(Chr(10))
                    Case "czelsnet"
                        manLines = My.Resources.cs_lsnet.Replace(Chr(13), "").Split(Chr(10))
                    Case "czelsnettree"
                        manLines = My.Resources.cs_lsnettree.Replace(Chr(13), "").Split(Chr(10))
                    Case "czemd"
                        manLines = My.Resources.cs_md.Replace(Chr(13), "").Split(Chr(10))
                    Case "czenetinfo"
                        manLines = My.Resources.cs_netinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "czenoaliases"
                        manLines = My.Resources.cs_noaliases.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeperm"
                        manLines = My.Resources.cs_perm.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeping"
                        manLines = My.Resources.cs_ping.Replace(Chr(13), "").Split(Chr(10))
                    Case "czerd"
                        manLines = My.Resources.cs_rd.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeread"
                        manLines = My.Resources.cs_read.Replace(Chr(13), "").Split(Chr(10))
                    Case "czereboot"
                        manLines = My.Resources.cs_reboot.Replace(Chr(13), "").Split(Chr(10))
                    Case "czereloadconfig"
                        manLines = My.Resources.cs_reloadconfig.Replace(Chr(13), "").Split(Chr(10))
                    Case "czermuser"
                        manLines = My.Resources.cs_rmuser.Replace(Chr(13), "").Split(Chr(10))
                    Case "czesavescreen"
                        manLines = My.Resources.cs_savescreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "czescical"
                        manLines = My.Resources.cs_scical.Replace(Chr(13), "").Split(Chr(10))
                    Case "czesetcolors"
                        manLines = My.Resources.cs_setcolors.Replace(Chr(13), "").Split(Chr(10))
                    Case "czesetsaver"
                        manLines = My.Resources.cs_setsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "czesetthemes"
                        manLines = My.Resources.cs_setthemes.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeshowmotd"
                        manLines = My.Resources.cs_showmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeshowtd"
                        manLines = My.Resources.cs_showtd.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeshowtdzone"
                        manLines = My.Resources.cs_showtdzone.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeshutdown"
                        manLines = My.Resources.cs_shutdown.Replace(Chr(13), "").Split(Chr(10))
                    Case "czesysinfo"
                        manLines = My.Resources.cs_sysinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeunitconv"
                        manLines = My.Resources.cs_unitconv.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeuseddeps"
                        manLines = My.Resources.cs_useddeps.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeAvailable command-line arguments"
                        manLines = My.Resources.cs_Available_command_line_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeAvailable kernel arguments"
                        manLines = My.Resources.cs_Available_kernel_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "czeConfiguration for your Kernel"
                        manLines = My.Resources.cs_Configuration_for_your_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    'Newly added manuals
                    Case "czechlang"
                        manLines = My.Resources.cs_chlang.Replace(Chr(13), "").Split(Chr(10))
                    Case "czecdbglog"
                        manLines = My.Resources.cs_cdbglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "czesses"
                        manLines = My.Resources.cs_sses.Replace(Chr(13), "").Split(Chr(10))

                    'Dutch manuals
                    Case "dtcIntroduction to the Kernel"
                        manLines = My.Resources.nl_Introduction_to_the_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcAvailable manual pages"
                        manLines = My.Resources.nl_Available_manual_pages.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcAvailable commands"
                        manLines = My.Resources.nl_Available_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcHistory of Kernel Simulator"
                        manLines = My.Resources.nl_History_of_Kernel_Simulator.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcAvailable FTP commands"
                        manLines = My.Resources.nl_Available_FTP_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcModding guide"
                        manLines = My.Resources.nl_Modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcScreensaver modding guide"
                        manLines = My.Resources.nl_Screensaver_modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcadduser"
                        manLines = My.Resources.nl_adduser.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcalias"
                        manLines = My.Resources.nl_alias.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcarginj"
                        manLines = My.Resources.nl_arginj.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtccalc"
                        manLines = My.Resources.nl_calc.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcchdir"
                        manLines = My.Resources.nl_chdir.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcchhostname"
                        manLines = My.Resources.nl_chhostname.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcchmal"
                        manLines = My.Resources.nl_chmal.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcchmotd"
                        manLines = My.Resources.nl_chmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcchpwd"
                        manLines = My.Resources.nl_chpwd.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcchusrname"
                        manLines = My.Resources.nl_chusrname.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtccls"
                        manLines = My.Resources.nl_cls.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcdebuglog"
                        manLines = My.Resources.nl_debuglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcFTP changelocaldir", "dtcFTP cdl"
                        manLines = My.Resources.nl_FTP_changelocaldir_or_cdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcFTP changeremotedir", "dtcFTP cdr"
                        manLines = My.Resources.nl_FTP_changeremotedir_or_cdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcFTP connect"
                        manLines = My.Resources.nl_FTP_connect.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcFTP currlocaldir", "dtcFTP pwdl"
                        manLines = My.Resources.nl_FTP_currlocaldir_or_pwdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcFTP currremotedir", "dtcFTP pwdr"
                        manLines = My.Resources.nl_FTP_currremotedir_or_pwdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcFTP delete", "dtcFTP del"
                        manLines = My.Resources.nl_FTP_delete_or_del.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcFTP disconnect"
                        manLines = My.Resources.nl_FTP_disconnect.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcFTP download", "dtcFTP get"
                        manLines = My.Resources.nl_FTP_download_or_get.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcFTP listlocal", "dtcFTP lsl"
                        manLines = My.Resources.nl_FTP_listlocal_or_lsl.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcFTP listremote", "dtcFTP lsr"
                        manLines = My.Resources.nl_FTP_listremote_or_lsr.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcFTP rename", "dtcFTP ren"
                        manLines = My.Resources.nl_FTP_rename_or_ren.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcFTP upload", "dtcFTP put"
                        manLines = My.Resources.nl_FTP_upload_or_put.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcftp"
                        manLines = My.Resources.nl_ftp.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtclist"
                        manLines = My.Resources.nl_list.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcreloadsaver"
                        manLines = My.Resources.nl_reloadsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtclockscreen"
                        manLines = My.Resources.nl_lockscreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtclogout"
                        manLines = My.Resources.nl_logout.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtclscomp"
                        manLines = My.Resources.nl_lscomp.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtclsnet"
                        manLines = My.Resources.nl_lsnet.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtclsnettree"
                        manLines = My.Resources.nl_lsnettree.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcmd"
                        manLines = My.Resources.nl_md.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcnetinfo"
                        manLines = My.Resources.nl_netinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcnoaliases"
                        manLines = My.Resources.nl_noaliases.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcperm"
                        manLines = My.Resources.nl_perm.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcping"
                        manLines = My.Resources.nl_ping.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcrd"
                        manLines = My.Resources.nl_rd.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcread"
                        manLines = My.Resources.nl_read.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcreboot"
                        manLines = My.Resources.nl_reboot.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcreloadconfig"
                        manLines = My.Resources.nl_reloadconfig.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcrmuser"
                        manLines = My.Resources.nl_rmuser.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcsavescreen"
                        manLines = My.Resources.nl_savescreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcscical"
                        manLines = My.Resources.nl_scical.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcsetcolors"
                        manLines = My.Resources.nl_setcolors.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcsetsaver"
                        manLines = My.Resources.nl_setsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcsetthemes"
                        manLines = My.Resources.nl_setthemes.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcshowmotd"
                        manLines = My.Resources.nl_showmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcshowtd"
                        manLines = My.Resources.nl_showtd.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcshowtdzone"
                        manLines = My.Resources.nl_showtdzone.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcshutdown"
                        manLines = My.Resources.nl_shutdown.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcsysinfo"
                        manLines = My.Resources.nl_sysinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcunitconv"
                        manLines = My.Resources.nl_unitconv.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcuseddeps"
                        manLines = My.Resources.nl_useddeps.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcAvailable command-line arguments"
                        manLines = My.Resources.nl_Available_command_line_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcAvailable kernel arguments"
                        manLines = My.Resources.nl_Available_kernel_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcConfiguration for your Kernel"
                        manLines = My.Resources.nl_Configuration_for_your_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    'Newly added manuals
                    Case "dtcchlang"
                        manLines = My.Resources.nl_chlang.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtccdbglog"
                        manLines = My.Resources.nl_cdbglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "dtcsses"
                        manLines = My.Resources.nl_sses.Replace(Chr(13), "").Split(Chr(10))

                    'Finnish manuals
                    Case "finIntroduction to the Kernel"
                        manLines = My.Resources.fi_Introduction_to_the_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    Case "finAvailable manual pages"
                        manLines = My.Resources.fi_Available_manual_pages.Replace(Chr(13), "").Split(Chr(10))
                    Case "finAvailable commands"
                        manLines = My.Resources.fi_Available_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "finHistory of Kernel Simulator"
                        manLines = My.Resources.fi_History_of_Kernel_Simulator.Replace(Chr(13), "").Split(Chr(10))
                    Case "finAvailable FTP commands"
                        manLines = My.Resources.fi_Available_FTP_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "finModding guide"
                        manLines = My.Resources.fi_Modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "finScreensaver modding guide"
                        manLines = My.Resources.fi_Screensaver_modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "finadduser"
                        manLines = My.Resources.fi_adduser.Replace(Chr(13), "").Split(Chr(10))
                    Case "finalias"
                        manLines = My.Resources.fi_alias.Replace(Chr(13), "").Split(Chr(10))
                    Case "finarginj"
                        manLines = My.Resources.fi_arginj.Replace(Chr(13), "").Split(Chr(10))
                    Case "fincalc"
                        manLines = My.Resources.fi_calc.Replace(Chr(13), "").Split(Chr(10))
                    Case "finchdir"
                        manLines = My.Resources.fi_chdir.Replace(Chr(13), "").Split(Chr(10))
                    Case "finchhostname"
                        manLines = My.Resources.fi_chhostname.Replace(Chr(13), "").Split(Chr(10))
                    Case "finchmal"
                        manLines = My.Resources.fi_chmal.Replace(Chr(13), "").Split(Chr(10))
                    Case "finchmotd"
                        manLines = My.Resources.fi_chmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "finchpwd"
                        manLines = My.Resources.fi_chpwd.Replace(Chr(13), "").Split(Chr(10))
                    Case "finchusrname"
                        manLines = My.Resources.fi_chusrname.Replace(Chr(13), "").Split(Chr(10))
                    Case "fincls"
                        manLines = My.Resources.fi_cls.Replace(Chr(13), "").Split(Chr(10))
                    Case "findebuglog"
                        manLines = My.Resources.fi_debuglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "finFTP changelocaldir", "finFTP cdl"
                        manLines = My.Resources.fi_FTP_changelocaldir_or_cdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "finFTP changeremotedir", "finFTP cdr"
                        manLines = My.Resources.fi_FTP_changeremotedir_or_cdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "finFTP connect"
                        manLines = My.Resources.fi_FTP_connect.Replace(Chr(13), "").Split(Chr(10))
                    Case "finFTP currlocaldir", "finFTP pwdl"
                        manLines = My.Resources.fi_FTP_currlocaldir_or_pwdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "finFTP currremotedir", "finFTP pwdr"
                        manLines = My.Resources.fi_FTP_currremotedir_or_pwdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "finFTP delete", "finFTP del"
                        manLines = My.Resources.fi_FTP_delete_or_del.Replace(Chr(13), "").Split(Chr(10))
                    Case "finFTP disconnect"
                        manLines = My.Resources.fi_FTP_disconnect.Replace(Chr(13), "").Split(Chr(10))
                    Case "finFTP download", "finFTP get"
                        manLines = My.Resources.fi_FTP_download_or_get.Replace(Chr(13), "").Split(Chr(10))
                    Case "finFTP listlocal", "finFTP lsl"
                        manLines = My.Resources.fi_FTP_listlocal_or_lsl.Replace(Chr(13), "").Split(Chr(10))
                    Case "finFTP listremote", "finFTP lsr"
                        manLines = My.Resources.fi_FTP_listremote_or_lsr.Replace(Chr(13), "").Split(Chr(10))
                    Case "finFTP rename", "finFTP ren"
                        manLines = My.Resources.fi_FTP_rename_or_ren.Replace(Chr(13), "").Split(Chr(10))
                    Case "finFTP upload", "finFTP put"
                        manLines = My.Resources.fi_FTP_upload_or_put.Replace(Chr(13), "").Split(Chr(10))
                    Case "finftp"
                        manLines = My.Resources.fi_ftp.Replace(Chr(13), "").Split(Chr(10))
                    Case "finlist"
                        manLines = My.Resources.fi_list.Replace(Chr(13), "").Split(Chr(10))
                    Case "finreloadsaver"
                        manLines = My.Resources.fi_reloadsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "finlockscreen"
                        manLines = My.Resources.fi_lockscreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "finlogout"
                        manLines = My.Resources.fi_logout.Replace(Chr(13), "").Split(Chr(10))
                    Case "finlscomp"
                        manLines = My.Resources.fi_lscomp.Replace(Chr(13), "").Split(Chr(10))
                    Case "finlsnet"
                        manLines = My.Resources.fi_lsnet.Replace(Chr(13), "").Split(Chr(10))
                    Case "finlsnettree"
                        manLines = My.Resources.fi_lsnettree.Replace(Chr(13), "").Split(Chr(10))
                    Case "finmd"
                        manLines = My.Resources.fi_md.Replace(Chr(13), "").Split(Chr(10))
                    Case "finnetinfo"
                        manLines = My.Resources.fi_netinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "finnoaliases"
                        manLines = My.Resources.fi_noaliases.Replace(Chr(13), "").Split(Chr(10))
                    Case "finperm"
                        manLines = My.Resources.fi_perm.Replace(Chr(13), "").Split(Chr(10))
                    Case "finping"
                        manLines = My.Resources.fi_ping.Replace(Chr(13), "").Split(Chr(10))
                    Case "finrd"
                        manLines = My.Resources.fi_rd.Replace(Chr(13), "").Split(Chr(10))
                    Case "finread"
                        manLines = My.Resources.fi_read.Replace(Chr(13), "").Split(Chr(10))
                    Case "finreboot"
                        manLines = My.Resources.fi_reboot.Replace(Chr(13), "").Split(Chr(10))
                    Case "finreloadconfig"
                        manLines = My.Resources.fi_reloadconfig.Replace(Chr(13), "").Split(Chr(10))
                    Case "finrmuser"
                        manLines = My.Resources.fi_rmuser.Replace(Chr(13), "").Split(Chr(10))
                    Case "finsavescreen"
                        manLines = My.Resources.fi_savescreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "finscical"
                        manLines = My.Resources.fi_scical.Replace(Chr(13), "").Split(Chr(10))
                    Case "finsetcolors"
                        manLines = My.Resources.fi_setcolors.Replace(Chr(13), "").Split(Chr(10))
                    Case "finsetsaver"
                        manLines = My.Resources.fi_setsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "finsetthemes"
                        manLines = My.Resources.fi_setthemes.Replace(Chr(13), "").Split(Chr(10))
                    Case "finshowmotd"
                        manLines = My.Resources.fi_showmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "finshowtd"
                        manLines = My.Resources.fi_showtd.Replace(Chr(13), "").Split(Chr(10))
                    Case "finshowtdzone"
                        manLines = My.Resources.fi_showtdzone.Replace(Chr(13), "").Split(Chr(10))
                    Case "finshutdown"
                        manLines = My.Resources.fi_shutdown.Replace(Chr(13), "").Split(Chr(10))
                    Case "finsysinfo"
                        manLines = My.Resources.fi_sysinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "finunitconv"
                        manLines = My.Resources.fi_unitconv.Replace(Chr(13), "").Split(Chr(10))
                    Case "finuseddeps"
                        manLines = My.Resources.fi_useddeps.Replace(Chr(13), "").Split(Chr(10))
                    Case "finAvailable command-line arguments"
                        manLines = My.Resources.fi_Available_command_line_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "finAvailable kernel arguments"
                        manLines = My.Resources.fi_Available_kernel_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "finConfiguration for your Kernel"
                        manLines = My.Resources.fi_Configuration_for_your_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    'Newly added manuals
                    Case "finchlang"
                        manLines = My.Resources.fi_chlang.Replace(Chr(13), "").Split(Chr(10))
                    Case "fincdbglog"
                        manLines = My.Resources.fi_cdbglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "finsses"
                        manLines = My.Resources.fi_sses.Replace(Chr(13), "").Split(Chr(10))

                    'French manuals
                    Case "freIntroduction to the Kernel"
                        manLines = My.Resources.fr_Introduction_to_the_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    Case "freAvailable manual pages"
                        manLines = My.Resources.fr_Available_manual_pages.Replace(Chr(13), "").Split(Chr(10))
                    Case "freAvailable commands"
                        manLines = My.Resources.fr_Available_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "freHistory of Kernel Simulator"
                        manLines = My.Resources.fr_History_of_Kernel_Simulator.Replace(Chr(13), "").Split(Chr(10))
                    Case "freAvailable FTP commands"
                        manLines = My.Resources.fr_Available_FTP_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "freModding guide"
                        manLines = My.Resources.fr_Modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "freScreensaver modding guide"
                        manLines = My.Resources.fr_Screensaver_modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "freadduser"
                        manLines = My.Resources.fr_adduser.Replace(Chr(13), "").Split(Chr(10))
                    Case "frealias"
                        manLines = My.Resources.fr_alias.Replace(Chr(13), "").Split(Chr(10))
                    Case "frearginj"
                        manLines = My.Resources.fr_arginj.Replace(Chr(13), "").Split(Chr(10))
                    Case "frecalc"
                        manLines = My.Resources.fr_calc.Replace(Chr(13), "").Split(Chr(10))
                    Case "frechdir"
                        manLines = My.Resources.fr_chdir.Replace(Chr(13), "").Split(Chr(10))
                    Case "frechhostname"
                        manLines = My.Resources.fr_chhostname.Replace(Chr(13), "").Split(Chr(10))
                    Case "frechmal"
                        manLines = My.Resources.fr_chmal.Replace(Chr(13), "").Split(Chr(10))
                    Case "frechmotd"
                        manLines = My.Resources.fr_chmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "frechpwd"
                        manLines = My.Resources.fr_chpwd.Replace(Chr(13), "").Split(Chr(10))
                    Case "frechusrname"
                        manLines = My.Resources.fr_chusrname.Replace(Chr(13), "").Split(Chr(10))
                    Case "frecls"
                        manLines = My.Resources.fr_cls.Replace(Chr(13), "").Split(Chr(10))
                    Case "fredebuglog"
                        manLines = My.Resources.fr_debuglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "freFTP changelocaldir", "freFTP cdl"
                        manLines = My.Resources.fr_FTP_changelocaldir_or_cdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "freFTP changeremotedir", "freFTP cdr"
                        manLines = My.Resources.fr_FTP_changeremotedir_or_cdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "freFTP connect"
                        manLines = My.Resources.fr_FTP_connect.Replace(Chr(13), "").Split(Chr(10))
                    Case "freFTP currlocaldir", "freFTP pwdl"
                        manLines = My.Resources.fr_FTP_currlocaldir_or_pwdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "freFTP currremotedir", "freFTP pwdr"
                        manLines = My.Resources.fr_FTP_currremotedir_or_pwdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "freFTP delete", "freFTP del"
                        manLines = My.Resources.fr_FTP_delete_or_del.Replace(Chr(13), "").Split(Chr(10))
                    Case "freFTP disconnect"
                        manLines = My.Resources.fr_FTP_disconnect.Replace(Chr(13), "").Split(Chr(10))
                    Case "freFTP download", "freFTP get"
                        manLines = My.Resources.fr_FTP_download_or_get.Replace(Chr(13), "").Split(Chr(10))
                    Case "freFTP listlocal", "freFTP lsl"
                        manLines = My.Resources.fr_FTP_listlocal_or_lsl.Replace(Chr(13), "").Split(Chr(10))
                    Case "freFTP listremote", "freFTP lsr"
                        manLines = My.Resources.fr_FTP_listremote_or_lsr.Replace(Chr(13), "").Split(Chr(10))
                    Case "freFTP rename", "freFTP ren"
                        manLines = My.Resources.fr_FTP_rename_or_ren.Replace(Chr(13), "").Split(Chr(10))
                    Case "freFTP upload", "freFTP put"
                        manLines = My.Resources.fr_FTP_upload_or_put.Replace(Chr(13), "").Split(Chr(10))
                    Case "freftp"
                        manLines = My.Resources.fr_ftp.Replace(Chr(13), "").Split(Chr(10))
                    Case "frelist"
                        manLines = My.Resources.fr_list.Replace(Chr(13), "").Split(Chr(10))
                    Case "frereloadsaver"
                        manLines = My.Resources.fr_reloadsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "frelockscreen"
                        manLines = My.Resources.fr_lockscreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "frelogout"
                        manLines = My.Resources.fr_logout.Replace(Chr(13), "").Split(Chr(10))
                    Case "frelscomp"
                        manLines = My.Resources.fr_lscomp.Replace(Chr(13), "").Split(Chr(10))
                    Case "frelsnet"
                        manLines = My.Resources.fr_lsnet.Replace(Chr(13), "").Split(Chr(10))
                    Case "frelsnettree"
                        manLines = My.Resources.fr_lsnettree.Replace(Chr(13), "").Split(Chr(10))
                    Case "fremd"
                        manLines = My.Resources.fr_md.Replace(Chr(13), "").Split(Chr(10))
                    Case "frenetinfo"
                        manLines = My.Resources.fr_netinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "frenoaliases"
                        manLines = My.Resources.fr_noaliases.Replace(Chr(13), "").Split(Chr(10))
                    Case "freperm"
                        manLines = My.Resources.fr_perm.Replace(Chr(13), "").Split(Chr(10))
                    Case "freping"
                        manLines = My.Resources.fr_ping.Replace(Chr(13), "").Split(Chr(10))
                    Case "frerd"
                        manLines = My.Resources.fr_rd.Replace(Chr(13), "").Split(Chr(10))
                    Case "freread"
                        manLines = My.Resources.fr_read.Replace(Chr(13), "").Split(Chr(10))
                    Case "frereboot"
                        manLines = My.Resources.fr_reboot.Replace(Chr(13), "").Split(Chr(10))
                    Case "frereloadconfig"
                        manLines = My.Resources.fr_reloadconfig.Replace(Chr(13), "").Split(Chr(10))
                    Case "frermuser"
                        manLines = My.Resources.fr_rmuser.Replace(Chr(13), "").Split(Chr(10))
                    Case "fresavescreen"
                        manLines = My.Resources.fr_savescreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "frescical"
                        manLines = My.Resources.fr_scical.Replace(Chr(13), "").Split(Chr(10))
                    Case "fresetcolors"
                        manLines = My.Resources.fr_setcolors.Replace(Chr(13), "").Split(Chr(10))
                    Case "fresetsaver"
                        manLines = My.Resources.fr_setsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "fresetthemes"
                        manLines = My.Resources.fr_setthemes.Replace(Chr(13), "").Split(Chr(10))
                    Case "freshowmotd"
                        manLines = My.Resources.fr_showmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "freshowtd"
                        manLines = My.Resources.fr_showtd.Replace(Chr(13), "").Split(Chr(10))
                    Case "freshowtdzone"
                        manLines = My.Resources.fr_showtdzone.Replace(Chr(13), "").Split(Chr(10))
                    Case "freshutdown"
                        manLines = My.Resources.fr_shutdown.Replace(Chr(13), "").Split(Chr(10))
                    Case "fresysinfo"
                        manLines = My.Resources.fr_sysinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "freunitconv"
                        manLines = My.Resources.fr_unitconv.Replace(Chr(13), "").Split(Chr(10))
                    Case "freuseddeps"
                        manLines = My.Resources.fr_useddeps.Replace(Chr(13), "").Split(Chr(10))
                    Case "freAvailable command-line arguments"
                        manLines = My.Resources.fr_Available_command_line_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "freAvailable kernel arguments"
                        manLines = My.Resources.fr_Available_kernel_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "freConfiguration for your Kernel"
                        manLines = My.Resources.fr_Configuration_for_your_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    'Newly added manuals
                    Case "frechlang"
                        manLines = My.Resources.fr_chlang.Replace(Chr(13), "").Split(Chr(10))
                    Case "frecdbglog"
                        manLines = My.Resources.fr_cdbglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "fresses"
                        manLines = My.Resources.fr_sses.Replace(Chr(13), "").Split(Chr(10))

                    'German manuals
                    Case "gerIntroduction to the Kernel"
                        manLines = My.Resources.de_Introduction_to_the_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerAvailable manual pages"
                        manLines = My.Resources.de_Available_manual_pages.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerAvailable commands"
                        manLines = My.Resources.de_Available_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerHistory of Kernel Simulator"
                        manLines = My.Resources.de_History_of_Kernel_Simulator.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerAvailable FTP commands"
                        manLines = My.Resources.de_Available_FTP_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerModding guide"
                        manLines = My.Resources.de_Modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerScreensaver modding guide"
                        manLines = My.Resources.de_Screensaver_modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "geradduser"
                        manLines = My.Resources.de_adduser.Replace(Chr(13), "").Split(Chr(10))
                    Case "geralias"
                        manLines = My.Resources.de_alias.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerarginj"
                        manLines = My.Resources.de_arginj.Replace(Chr(13), "").Split(Chr(10))
                    Case "gercalc"
                        manLines = My.Resources.de_calc.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerchdir"
                        manLines = My.Resources.de_chdir.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerchhostname"
                        manLines = My.Resources.de_chhostname.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerchmal"
                        manLines = My.Resources.de_chmal.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerchmotd"
                        manLines = My.Resources.de_chmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerchpwd"
                        manLines = My.Resources.de_chpwd.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerchusrname"
                        manLines = My.Resources.de_chusrname.Replace(Chr(13), "").Split(Chr(10))
                    Case "gercls"
                        manLines = My.Resources.de_cls.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerdebuglog"
                        manLines = My.Resources.de_debuglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerFTP changelocaldir", "gerFTP cdl"
                        manLines = My.Resources.de_FTP_changelocaldir_or_cdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerFTP changeremotedir", "gerFTP cdr"
                        manLines = My.Resources.de_FTP_changeremotedir_or_cdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerFTP connect"
                        manLines = My.Resources.de_FTP_connect.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerFTP currlocaldir", "gerFTP pwdl"
                        manLines = My.Resources.de_FTP_currlocaldir_or_pwdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerFTP currremotedir", "gerFTP pwdr"
                        manLines = My.Resources.de_FTP_currremotedir_or_pwdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerFTP delete", "gerFTP del"
                        manLines = My.Resources.de_FTP_delete_or_del.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerFTP disconnect"
                        manLines = My.Resources.de_FTP_disconnect.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerFTP download", "gerFTP get"
                        manLines = My.Resources.de_FTP_download_or_get.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerFTP listlocal", "gerFTP lsl"
                        manLines = My.Resources.de_FTP_listlocal_or_lsl.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerFTP listremote", "gerFTP lsr"
                        manLines = My.Resources.de_FTP_listremote_or_lsr.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerFTP rename", "gerFTP ren"
                        manLines = My.Resources.de_FTP_rename_or_ren.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerFTP upload", "gerFTP put"
                        manLines = My.Resources.de_FTP_upload_or_put.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerftp"
                        manLines = My.Resources.de_ftp.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerlist"
                        manLines = My.Resources.de_list.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerreloadsaver"
                        manLines = My.Resources.de_reloadsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerlockscreen"
                        manLines = My.Resources.de_lockscreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerlogout"
                        manLines = My.Resources.de_logout.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerlscomp"
                        manLines = My.Resources.de_lscomp.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerlsnet"
                        manLines = My.Resources.de_lsnet.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerlsnettree"
                        manLines = My.Resources.de_lsnettree.Replace(Chr(13), "").Split(Chr(10))
                    Case "germd"
                        manLines = My.Resources.de_md.Replace(Chr(13), "").Split(Chr(10))
                    Case "gernetinfo"
                        manLines = My.Resources.de_netinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "gernoaliases"
                        manLines = My.Resources.de_noaliases.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerperm"
                        manLines = My.Resources.de_perm.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerping"
                        manLines = My.Resources.de_ping.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerrd"
                        manLines = My.Resources.de_rd.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerread"
                        manLines = My.Resources.de_read.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerreboot"
                        manLines = My.Resources.de_reboot.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerreloadconfig"
                        manLines = My.Resources.de_reloadconfig.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerrmuser"
                        manLines = My.Resources.de_rmuser.Replace(Chr(13), "").Split(Chr(10))
                    Case "gersavescreen"
                        manLines = My.Resources.de_savescreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerscical"
                        manLines = My.Resources.de_scical.Replace(Chr(13), "").Split(Chr(10))
                    Case "gersetcolors"
                        manLines = My.Resources.de_setcolors.Replace(Chr(13), "").Split(Chr(10))
                    Case "gersetsaver"
                        manLines = My.Resources.de_setsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "gersetthemes"
                        manLines = My.Resources.de_setthemes.Replace(Chr(13), "").Split(Chr(10))
                    Case "gershowmotd"
                        manLines = My.Resources.de_showmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "gershowtd"
                        manLines = My.Resources.de_showtd.Replace(Chr(13), "").Split(Chr(10))
                    Case "gershowtdzone"
                        manLines = My.Resources.de_showtdzone.Replace(Chr(13), "").Split(Chr(10))
                    Case "gershutdown"
                        manLines = My.Resources.de_shutdown.Replace(Chr(13), "").Split(Chr(10))
                    Case "gersysinfo"
                        manLines = My.Resources.de_sysinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerunitconv"
                        manLines = My.Resources.de_unitconv.Replace(Chr(13), "").Split(Chr(10))
                    Case "geruseddeps"
                        manLines = My.Resources.de_useddeps.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerAvailable command-line arguments"
                        manLines = My.Resources.de_Available_command_line_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerAvailable kernel arguments"
                        manLines = My.Resources.de_Available_kernel_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "gerConfiguration for your Kernel"
                        manLines = My.Resources.de_Configuration_for_your_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    'Newly added manuals
                    Case "gerchlang"
                        manLines = My.Resources.de_chlang.Replace(Chr(13), "").Split(Chr(10))
                    Case "gercdbglog"
                        manLines = My.Resources.de_cdbglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "gersses"
                        manLines = My.Resources.de_sses.Replace(Chr(13), "").Split(Chr(10))

                    'Hindi manuals
                    Case "indIntroduction to the Kernel"
                        manLines = My.Resources.hi_Introduction_to_the_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    Case "indAvailable manual pages"
                        manLines = My.Resources.hi_Available_manual_pages.Replace(Chr(13), "").Split(Chr(10))
                    Case "indAvailable commands"
                        manLines = My.Resources.hi_Available_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "indHistory of Kernel Simulator"
                        manLines = My.Resources.hi_History_of_Kernel_Simulator.Replace(Chr(13), "").Split(Chr(10))
                    Case "indAvailable FTP commands"
                        manLines = My.Resources.hi_Available_FTP_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "indModding guide"
                        manLines = My.Resources.hi_Modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "indScreensaver modding guide"
                        manLines = My.Resources.hi_Screensaver_modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "indadduser"
                        manLines = My.Resources.hi_adduser.Replace(Chr(13), "").Split(Chr(10))
                    Case "indalias"
                        manLines = My.Resources.hi_alias.Replace(Chr(13), "").Split(Chr(10))
                    Case "indarginj"
                        manLines = My.Resources.hi_arginj.Replace(Chr(13), "").Split(Chr(10))
                    Case "indcalc"
                        manLines = My.Resources.hi_calc.Replace(Chr(13), "").Split(Chr(10))
                    Case "indchdir"
                        manLines = My.Resources.hi_chdir.Replace(Chr(13), "").Split(Chr(10))
                    Case "indchhostname"
                        manLines = My.Resources.hi_chhostname.Replace(Chr(13), "").Split(Chr(10))
                    Case "indchmal"
                        manLines = My.Resources.hi_chmal.Replace(Chr(13), "").Split(Chr(10))
                    Case "indchmotd"
                        manLines = My.Resources.hi_chmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "indchpwd"
                        manLines = My.Resources.hi_chpwd.Replace(Chr(13), "").Split(Chr(10))
                    Case "indchusrname"
                        manLines = My.Resources.hi_chusrname.Replace(Chr(13), "").Split(Chr(10))
                    Case "indcls"
                        manLines = My.Resources.hi_cls.Replace(Chr(13), "").Split(Chr(10))
                    Case "inddebuglog"
                        manLines = My.Resources.hi_debuglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "indFTP changelocaldir", "indFTP cdl"
                        manLines = My.Resources.hi_FTP_changelocaldir_or_cdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "indFTP changeremotedir", "indFTP cdr"
                        manLines = My.Resources.hi_FTP_changeremotedir_or_cdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "indFTP connect"
                        manLines = My.Resources.hi_FTP_connect.Replace(Chr(13), "").Split(Chr(10))
                    Case "indFTP currlocaldir", "indFTP pwdl"
                        manLines = My.Resources.hi_FTP_currlocaldir_or_pwdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "indFTP currremotedir", "indFTP pwdr"
                        manLines = My.Resources.hi_FTP_currremotedir_or_pwdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "indFTP delete", "indFTP del"
                        manLines = My.Resources.hi_FTP_delete_or_del.Replace(Chr(13), "").Split(Chr(10))
                    Case "indFTP disconnect"
                        manLines = My.Resources.hi_FTP_disconnect.Replace(Chr(13), "").Split(Chr(10))
                    Case "indFTP download", "indFTP get"
                        manLines = My.Resources.hi_FTP_download_or_get.Replace(Chr(13), "").Split(Chr(10))
                    Case "indFTP listlocal", "indFTP lsl"
                        manLines = My.Resources.hi_FTP_listlocal_or_lsl.Replace(Chr(13), "").Split(Chr(10))
                    Case "indFTP listremote", "indFTP lsr"
                        manLines = My.Resources.hi_FTP_listremote_or_lsr.Replace(Chr(13), "").Split(Chr(10))
                    Case "indFTP rename", "indFTP ren"
                        manLines = My.Resources.hi_FTP_rename_or_ren.Replace(Chr(13), "").Split(Chr(10))
                    Case "indFTP upload", "indFTP put"
                        manLines = My.Resources.hi_FTP_upload_or_put.Replace(Chr(13), "").Split(Chr(10))
                    Case "indftp"
                        manLines = My.Resources.hi_ftp.Replace(Chr(13), "").Split(Chr(10))
                    Case "indlist"
                        manLines = My.Resources.hi_list.Replace(Chr(13), "").Split(Chr(10))
                    Case "indreloadsaver"
                        manLines = My.Resources.hi_reloadsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "indlockscreen"
                        manLines = My.Resources.hi_lockscreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "indlogout"
                        manLines = My.Resources.hi_logout.Replace(Chr(13), "").Split(Chr(10))
                    Case "indlscomp"
                        manLines = My.Resources.hi_lscomp.Replace(Chr(13), "").Split(Chr(10))
                    Case "indlsnet"
                        manLines = My.Resources.hi_lsnet.Replace(Chr(13), "").Split(Chr(10))
                    Case "indlsnettree"
                        manLines = My.Resources.hi_lsnettree.Replace(Chr(13), "").Split(Chr(10))
                    Case "indmd"
                        manLines = My.Resources.hi_md.Replace(Chr(13), "").Split(Chr(10))
                    Case "indnetinfo"
                        manLines = My.Resources.hi_netinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "indnoaliases"
                        manLines = My.Resources.hi_noaliases.Replace(Chr(13), "").Split(Chr(10))
                    Case "indperm"
                        manLines = My.Resources.hi_perm.Replace(Chr(13), "").Split(Chr(10))
                    Case "indping"
                        manLines = My.Resources.hi_ping.Replace(Chr(13), "").Split(Chr(10))
                    Case "indrd"
                        manLines = My.Resources.hi_rd.Replace(Chr(13), "").Split(Chr(10))
                    Case "indread"
                        manLines = My.Resources.hi_read.Replace(Chr(13), "").Split(Chr(10))
                    Case "indreboot"
                        manLines = My.Resources.hi_reboot.Replace(Chr(13), "").Split(Chr(10))
                    Case "indreloadconfig"
                        manLines = My.Resources.hi_reloadconfig.Replace(Chr(13), "").Split(Chr(10))
                    Case "indrmuser"
                        manLines = My.Resources.hi_rmuser.Replace(Chr(13), "").Split(Chr(10))
                    Case "indsavescreen"
                        manLines = My.Resources.hi_savescreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "indscical"
                        manLines = My.Resources.hi_scical.Replace(Chr(13), "").Split(Chr(10))
                    Case "indsetcolors"
                        manLines = My.Resources.hi_setcolors.Replace(Chr(13), "").Split(Chr(10))
                    Case "indsetsaver"
                        manLines = My.Resources.hi_setsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "indsetthemes"
                        manLines = My.Resources.hi_setthemes.Replace(Chr(13), "").Split(Chr(10))
                    Case "indshowmotd"
                        manLines = My.Resources.hi_showmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "indshowtd"
                        manLines = My.Resources.hi_showtd.Replace(Chr(13), "").Split(Chr(10))
                    Case "indshowtdzone"
                        manLines = My.Resources.hi_showtdzone.Replace(Chr(13), "").Split(Chr(10))
                    Case "indshutdown"
                        manLines = My.Resources.hi_shutdown.Replace(Chr(13), "").Split(Chr(10))
                    Case "indsysinfo"
                        manLines = My.Resources.hi_sysinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "indunitconv"
                        manLines = My.Resources.hi_unitconv.Replace(Chr(13), "").Split(Chr(10))
                    Case "induseddeps"
                        manLines = My.Resources.hi_useddeps.Replace(Chr(13), "").Split(Chr(10))
                    Case "indAvailable command-line arguments"
                        manLines = My.Resources.hi_Available_command_line_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "indAvailable kernel arguments"
                        manLines = My.Resources.hi_Available_kernel_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "indConfiguration for your Kernel"
                        manLines = My.Resources.hi_Configuration_for_your_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    'Newly added manuals
                    Case "indchlang"
                        manLines = My.Resources.hi_chlang.Replace(Chr(13), "").Split(Chr(10))
                    Case "indcdbglog"
                        manLines = My.Resources.hi_cdbglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "indsses"
                        manLines = My.Resources.hi_sses.Replace(Chr(13), "").Split(Chr(10))

                    'Italian manuals
                    Case "itaIntroduction to the Kernel"
                        manLines = My.Resources.it_Introduction_to_the_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaAvailable manual pages"
                        manLines = My.Resources.it_Available_manual_pages.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaAvailable commands"
                        manLines = My.Resources.it_Available_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaHistory of Kernel Simulator"
                        manLines = My.Resources.it_History_of_Kernel_Simulator.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaAvailable FTP commands"
                        manLines = My.Resources.it_Available_FTP_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaModding guide"
                        manLines = My.Resources.it_Modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaScreensaver modding guide"
                        manLines = My.Resources.it_Screensaver_modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaadduser"
                        manLines = My.Resources.it_adduser.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaalias"
                        manLines = My.Resources.it_alias.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaarginj"
                        manLines = My.Resources.it_arginj.Replace(Chr(13), "").Split(Chr(10))
                    Case "itacalc"
                        manLines = My.Resources.it_calc.Replace(Chr(13), "").Split(Chr(10))
                    Case "itachdir"
                        manLines = My.Resources.it_chdir.Replace(Chr(13), "").Split(Chr(10))
                    Case "itachhostname"
                        manLines = My.Resources.it_chhostname.Replace(Chr(13), "").Split(Chr(10))
                    Case "itachmal"
                        manLines = My.Resources.it_chmal.Replace(Chr(13), "").Split(Chr(10))
                    Case "itachmotd"
                        manLines = My.Resources.it_chmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "itachpwd"
                        manLines = My.Resources.it_chpwd.Replace(Chr(13), "").Split(Chr(10))
                    Case "itachusrname"
                        manLines = My.Resources.it_chusrname.Replace(Chr(13), "").Split(Chr(10))
                    Case "itacls"
                        manLines = My.Resources.it_cls.Replace(Chr(13), "").Split(Chr(10))
                    Case "itadebuglog"
                        manLines = My.Resources.it_debuglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaFTP changelocaldir", "itaFTP cdl"
                        manLines = My.Resources.it_FTP_changelocaldir_or_cdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaFTP changeremotedir", "itaFTP cdr"
                        manLines = My.Resources.it_FTP_changeremotedir_or_cdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaFTP connect"
                        manLines = My.Resources.it_FTP_connect.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaFTP currlocaldir", "itaFTP pwdl"
                        manLines = My.Resources.it_FTP_currlocaldir_or_pwdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaFTP currremotedir", "itaFTP pwdr"
                        manLines = My.Resources.it_FTP_currremotedir_or_pwdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaFTP delete", "itaFTP del"
                        manLines = My.Resources.it_FTP_delete_or_del.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaFTP disconnect"
                        manLines = My.Resources.it_FTP_disconnect.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaFTP download", "itaFTP get"
                        manLines = My.Resources.it_FTP_download_or_get.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaFTP listlocal", "itaFTP lsl"
                        manLines = My.Resources.it_FTP_listlocal_or_lsl.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaFTP listremote", "itaFTP lsr"
                        manLines = My.Resources.it_FTP_listremote_or_lsr.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaFTP rename", "itaFTP ren"
                        manLines = My.Resources.it_FTP_rename_or_ren.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaFTP upload", "itaFTP put"
                        manLines = My.Resources.it_FTP_upload_or_put.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaftp"
                        manLines = My.Resources.it_ftp.Replace(Chr(13), "").Split(Chr(10))
                    Case "italist"
                        manLines = My.Resources.it_list.Replace(Chr(13), "").Split(Chr(10))
                    Case "itareloadsaver"
                        manLines = My.Resources.it_reloadsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "italockscreen"
                        manLines = My.Resources.it_lockscreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "italogout"
                        manLines = My.Resources.it_logout.Replace(Chr(13), "").Split(Chr(10))
                    Case "italscomp"
                        manLines = My.Resources.it_lscomp.Replace(Chr(13), "").Split(Chr(10))
                    Case "italsnet"
                        manLines = My.Resources.it_lsnet.Replace(Chr(13), "").Split(Chr(10))
                    Case "italsnettree"
                        manLines = My.Resources.it_lsnettree.Replace(Chr(13), "").Split(Chr(10))
                    Case "itamd"
                        manLines = My.Resources.it_md.Replace(Chr(13), "").Split(Chr(10))
                    Case "itanetinfo"
                        manLines = My.Resources.it_netinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "itanoaliases"
                        manLines = My.Resources.it_noaliases.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaperm"
                        manLines = My.Resources.it_perm.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaping"
                        manLines = My.Resources.it_ping.Replace(Chr(13), "").Split(Chr(10))
                    Case "itard"
                        manLines = My.Resources.it_rd.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaread"
                        manLines = My.Resources.it_read.Replace(Chr(13), "").Split(Chr(10))
                    Case "itareboot"
                        manLines = My.Resources.it_reboot.Replace(Chr(13), "").Split(Chr(10))
                    Case "itareloadconfig"
                        manLines = My.Resources.it_reloadconfig.Replace(Chr(13), "").Split(Chr(10))
                    Case "itarmuser"
                        manLines = My.Resources.it_rmuser.Replace(Chr(13), "").Split(Chr(10))
                    Case "itasavescreen"
                        manLines = My.Resources.it_savescreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "itascical"
                        manLines = My.Resources.it_scical.Replace(Chr(13), "").Split(Chr(10))
                    Case "itasetcolors"
                        manLines = My.Resources.it_setcolors.Replace(Chr(13), "").Split(Chr(10))
                    Case "itasetsaver"
                        manLines = My.Resources.it_setsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "itasetthemes"
                        manLines = My.Resources.it_setthemes.Replace(Chr(13), "").Split(Chr(10))
                    Case "itashowmotd"
                        manLines = My.Resources.it_showmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "itashowtd"
                        manLines = My.Resources.it_showtd.Replace(Chr(13), "").Split(Chr(10))
                    Case "itashowtdzone"
                        manLines = My.Resources.it_showtdzone.Replace(Chr(13), "").Split(Chr(10))
                    Case "itashutdown"
                        manLines = My.Resources.it_shutdown.Replace(Chr(13), "").Split(Chr(10))
                    Case "itasysinfo"
                        manLines = My.Resources.it_sysinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaunitconv"
                        manLines = My.Resources.it_unitconv.Replace(Chr(13), "").Split(Chr(10))
                    Case "itauseddeps"
                        manLines = My.Resources.it_useddeps.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaAvailable command-line arguments"
                        manLines = My.Resources.it_Available_command_line_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaAvailable kernel arguments"
                        manLines = My.Resources.it_Available_kernel_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "itaConfiguration for your Kernel"
                        manLines = My.Resources.it_Configuration_for_your_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    'Newly added manuals
                    Case "itachlang"
                        manLines = My.Resources.it_chlang.Replace(Chr(13), "").Split(Chr(10))
                    Case "itacdbglog"
                        manLines = My.Resources.it_cdbglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "itasses"
                        manLines = My.Resources.it_sses.Replace(Chr(13), "").Split(Chr(10))

                    'Malay manuals
                    Case "malIntroduction to the Kernel"
                        manLines = My.Resources.ms_Introduction_to_the_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    Case "malAvailable manual pages"
                        manLines = My.Resources.ms_Available_manual_pages.Replace(Chr(13), "").Split(Chr(10))
                    Case "malAvailable commands"
                        manLines = My.Resources.ms_Available_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "malHistory of Kernel Simulator"
                        manLines = My.Resources.ms_History_of_Kernel_Simulator.Replace(Chr(13), "").Split(Chr(10))
                    Case "malAvailable FTP commands"
                        manLines = My.Resources.ms_Available_FTP_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "malModding guide"
                        manLines = My.Resources.ms_Modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "malScreensaver modding guide"
                        manLines = My.Resources.ms_Screensaver_modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "maladduser"
                        manLines = My.Resources.ms_adduser.Replace(Chr(13), "").Split(Chr(10))
                    Case "malalias"
                        manLines = My.Resources.ms_alias.Replace(Chr(13), "").Split(Chr(10))
                    Case "malarginj"
                        manLines = My.Resources.ms_arginj.Replace(Chr(13), "").Split(Chr(10))
                    Case "malcalc"
                        manLines = My.Resources.ms_calc.Replace(Chr(13), "").Split(Chr(10))
                    Case "malchdir"
                        manLines = My.Resources.ms_chdir.Replace(Chr(13), "").Split(Chr(10))
                    Case "malchhostname"
                        manLines = My.Resources.ms_chhostname.Replace(Chr(13), "").Split(Chr(10))
                    Case "malchmal"
                        manLines = My.Resources.ms_chmal.Replace(Chr(13), "").Split(Chr(10))
                    Case "malchmotd"
                        manLines = My.Resources.ms_chmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "malchpwd"
                        manLines = My.Resources.ms_chpwd.Replace(Chr(13), "").Split(Chr(10))
                    Case "malchusrname"
                        manLines = My.Resources.ms_chusrname.Replace(Chr(13), "").Split(Chr(10))
                    Case "malcls"
                        manLines = My.Resources.ms_cls.Replace(Chr(13), "").Split(Chr(10))
                    Case "maldebuglog"
                        manLines = My.Resources.ms_debuglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "malFTP changelocaldir", "malFTP cdl"
                        manLines = My.Resources.ms_FTP_changelocaldir_or_cdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "malFTP changeremotedir", "malFTP cdr"
                        manLines = My.Resources.ms_FTP_changeremotedir_or_cdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "malFTP connect"
                        manLines = My.Resources.ms_FTP_connect.Replace(Chr(13), "").Split(Chr(10))
                    Case "malFTP currlocaldir", "malFTP pwdl"
                        manLines = My.Resources.ms_FTP_currlocaldir_or_pwdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "malFTP currremotedir", "malFTP pwdr"
                        manLines = My.Resources.ms_FTP_currremotedir_or_pwdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "malFTP delete", "malFTP del"
                        manLines = My.Resources.ms_FTP_delete_or_del.Replace(Chr(13), "").Split(Chr(10))
                    Case "malFTP disconnect"
                        manLines = My.Resources.ms_FTP_disconnect.Replace(Chr(13), "").Split(Chr(10))
                    Case "malFTP download", "malFTP get"
                        manLines = My.Resources.ms_FTP_download_or_get.Replace(Chr(13), "").Split(Chr(10))
                    Case "malFTP listlocal", "malFTP lsl"
                        manLines = My.Resources.ms_FTP_listlocal_or_lsl.Replace(Chr(13), "").Split(Chr(10))
                    Case "malFTP listremote", "malFTP lsr"
                        manLines = My.Resources.ms_FTP_listremote_or_lsr.Replace(Chr(13), "").Split(Chr(10))
                    Case "malFTP rename", "malFTP ren"
                        manLines = My.Resources.ms_FTP_rename_or_ren.Replace(Chr(13), "").Split(Chr(10))
                    Case "malFTP upload", "malFTP put"
                        manLines = My.Resources.ms_FTP_upload_or_put.Replace(Chr(13), "").Split(Chr(10))
                    Case "malftp"
                        manLines = My.Resources.ms_ftp.Replace(Chr(13), "").Split(Chr(10))
                    Case "mallist"
                        manLines = My.Resources.ms_list.Replace(Chr(13), "").Split(Chr(10))
                    Case "malreloadsaver"
                        manLines = My.Resources.ms_reloadsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "mallockscreen"
                        manLines = My.Resources.ms_lockscreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "mallogout"
                        manLines = My.Resources.ms_logout.Replace(Chr(13), "").Split(Chr(10))
                    Case "mallscomp"
                        manLines = My.Resources.ms_lscomp.Replace(Chr(13), "").Split(Chr(10))
                    Case "mallsnet"
                        manLines = My.Resources.ms_lsnet.Replace(Chr(13), "").Split(Chr(10))
                    Case "mallsnettree"
                        manLines = My.Resources.ms_lsnettree.Replace(Chr(13), "").Split(Chr(10))
                    Case "malmd"
                        manLines = My.Resources.ms_md.Replace(Chr(13), "").Split(Chr(10))
                    Case "malnetinfo"
                        manLines = My.Resources.ms_netinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "malnoaliases"
                        manLines = My.Resources.ms_noaliases.Replace(Chr(13), "").Split(Chr(10))
                    Case "malperm"
                        manLines = My.Resources.ms_perm.Replace(Chr(13), "").Split(Chr(10))
                    Case "malping"
                        manLines = My.Resources.ms_ping.Replace(Chr(13), "").Split(Chr(10))
                    Case "malrd"
                        manLines = My.Resources.ms_rd.Replace(Chr(13), "").Split(Chr(10))
                    Case "malread"
                        manLines = My.Resources.ms_read.Replace(Chr(13), "").Split(Chr(10))
                    Case "malreboot"
                        manLines = My.Resources.ms_reboot.Replace(Chr(13), "").Split(Chr(10))
                    Case "malreloadconfig"
                        manLines = My.Resources.ms_reloadconfig.Replace(Chr(13), "").Split(Chr(10))
                    Case "malrmuser"
                        manLines = My.Resources.ms_rmuser.Replace(Chr(13), "").Split(Chr(10))
                    Case "malsavescreen"
                        manLines = My.Resources.ms_savescreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "malscical"
                        manLines = My.Resources.ms_scical.Replace(Chr(13), "").Split(Chr(10))
                    Case "malsetcolors"
                        manLines = My.Resources.ms_setcolors.Replace(Chr(13), "").Split(Chr(10))
                    Case "malsetsaver"
                        manLines = My.Resources.ms_setsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "malsetthemes"
                        manLines = My.Resources.ms_setthemes.Replace(Chr(13), "").Split(Chr(10))
                    Case "malshowmotd"
                        manLines = My.Resources.ms_showmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "malshowtd"
                        manLines = My.Resources.ms_showtd.Replace(Chr(13), "").Split(Chr(10))
                    Case "malshowtdzone"
                        manLines = My.Resources.ms_showtdzone.Replace(Chr(13), "").Split(Chr(10))
                    Case "malshutdown"
                        manLines = My.Resources.ms_shutdown.Replace(Chr(13), "").Split(Chr(10))
                    Case "malsysinfo"
                        manLines = My.Resources.ms_sysinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "malunitconv"
                        manLines = My.Resources.ms_unitconv.Replace(Chr(13), "").Split(Chr(10))
                    Case "maluseddeps"
                        manLines = My.Resources.ms_useddeps.Replace(Chr(13), "").Split(Chr(10))
                    Case "malAvailable command-line arguments"
                        manLines = My.Resources.ms_Available_command_line_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "malAvailable kernel arguments"
                        manLines = My.Resources.ms_Available_kernel_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "malConfiguration for your Kernel"
                        manLines = My.Resources.ms_Configuration_for_your_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    'Newly added manuals
                    Case "malchlang"
                        manLines = My.Resources.ms_chlang.Replace(Chr(13), "").Split(Chr(10))
                    Case "malcdbglog"
                        manLines = My.Resources.ms_cdbglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "malsses"
                        manLines = My.Resources.ms_sses.Replace(Chr(13), "").Split(Chr(10))

                    'Portuguese manuals
                    Case "ptgIntroduction to the Kernel"
                        manLines = My.Resources.pt_Introduction_to_the_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgAvailable manual pages"
                        manLines = My.Resources.pt_Available_manual_pages.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgAvailable commands"
                        manLines = My.Resources.pt_Available_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgHistory of Kernel Simulator"
                        manLines = My.Resources.pt_History_of_Kernel_Simulator.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgAvailable FTP commands"
                        manLines = My.Resources.pt_Available_FTP_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgModding guide"
                        manLines = My.Resources.pt_Modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgScreensaver modding guide"
                        manLines = My.Resources.pt_Screensaver_modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgadduser"
                        manLines = My.Resources.pt_adduser.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgalias"
                        manLines = My.Resources.pt_alias.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgarginj"
                        manLines = My.Resources.pt_arginj.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgcalc"
                        manLines = My.Resources.pt_calc.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgchdir"
                        manLines = My.Resources.pt_chdir.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgchhostname"
                        manLines = My.Resources.pt_chhostname.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgchmal"
                        manLines = My.Resources.pt_chmal.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgchmotd"
                        manLines = My.Resources.pt_chmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgchpwd"
                        manLines = My.Resources.pt_chpwd.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgchusrname"
                        manLines = My.Resources.pt_chusrname.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgcls"
                        manLines = My.Resources.pt_cls.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgdebuglog"
                        manLines = My.Resources.pt_debuglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgFTP changelocaldir", "ptgFTP cdl"
                        manLines = My.Resources.pt_FTP_changelocaldir_or_cdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgFTP changeremotedir", "ptgFTP cdr"
                        manLines = My.Resources.pt_FTP_changeremotedir_or_cdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgFTP connect"
                        manLines = My.Resources.pt_FTP_connect.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgFTP currlocaldir", "ptgFTP pwdl"
                        manLines = My.Resources.pt_FTP_currlocaldir_or_pwdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgFTP currremotedir", "ptgFTP pwdr"
                        manLines = My.Resources.pt_FTP_currremotedir_or_pwdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgFTP delete", "ptgFTP del"
                        manLines = My.Resources.pt_FTP_delete_or_del.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgFTP disconnect"
                        manLines = My.Resources.pt_FTP_disconnect.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgFTP download", "ptgFTP get"
                        manLines = My.Resources.pt_FTP_download_or_get.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgFTP listlocal", "ptgFTP lsl"
                        manLines = My.Resources.pt_FTP_listlocal_or_lsl.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgFTP listremote", "ptgFTP lsr"
                        manLines = My.Resources.pt_FTP_listremote_or_lsr.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgFTP rename", "ptgFTP ren"
                        manLines = My.Resources.pt_FTP_rename_or_ren.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgFTP upload", "ptgFTP put"
                        manLines = My.Resources.pt_FTP_upload_or_put.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgftp"
                        manLines = My.Resources.pt_ftp.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptglist"
                        manLines = My.Resources.pt_list.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgreloadsaver"
                        manLines = My.Resources.pt_reloadsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptglockscreen"
                        manLines = My.Resources.pt_lockscreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptglogout"
                        manLines = My.Resources.pt_logout.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptglscomp"
                        manLines = My.Resources.pt_lscomp.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptglsnet"
                        manLines = My.Resources.pt_lsnet.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptglsnettree"
                        manLines = My.Resources.pt_lsnettree.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgmd"
                        manLines = My.Resources.pt_md.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgnetinfo"
                        manLines = My.Resources.pt_netinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgnoaliases"
                        manLines = My.Resources.pt_noaliases.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgperm"
                        manLines = My.Resources.pt_perm.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgping"
                        manLines = My.Resources.pt_ping.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgrd"
                        manLines = My.Resources.pt_rd.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgread"
                        manLines = My.Resources.pt_read.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgreboot"
                        manLines = My.Resources.pt_reboot.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgreloadconfig"
                        manLines = My.Resources.pt_reloadconfig.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgrmuser"
                        manLines = My.Resources.pt_rmuser.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgsavescreen"
                        manLines = My.Resources.pt_savescreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgscical"
                        manLines = My.Resources.pt_scical.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgsetcolors"
                        manLines = My.Resources.pt_setcolors.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgsetsaver"
                        manLines = My.Resources.pt_setsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgsetthemes"
                        manLines = My.Resources.pt_setthemes.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgshowmotd"
                        manLines = My.Resources.pt_showmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgshowtd"
                        manLines = My.Resources.pt_showtd.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgshowtdzone"
                        manLines = My.Resources.pt_showtdzone.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgshutdown"
                        manLines = My.Resources.pt_shutdown.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgsysinfo"
                        manLines = My.Resources.pt_sysinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgunitconv"
                        manLines = My.Resources.pt_unitconv.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptguseddeps"
                        manLines = My.Resources.pt_useddeps.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgAvailable command-line arguments"
                        manLines = My.Resources.pt_Available_command_line_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgAvailable kernel arguments"
                        manLines = My.Resources.pt_Available_kernel_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgConfiguration for your Kernel"
                        manLines = My.Resources.pt_Configuration_for_your_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    'Newly added manuals
                    Case "ptgchlang"
                        manLines = My.Resources.pt_chlang.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgcdbglog"
                        manLines = My.Resources.pt_cdbglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "ptgsses"
                        manLines = My.Resources.pt_sses.Replace(Chr(13), "").Split(Chr(10))

                    'Spanish manuals
                    Case "spaIntroduction to the Kernel"
                        manLines = My.Resources.es_Introduction_to_the_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaAvailable manual pages"
                        manLines = My.Resources.es_Available_manual_pages.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaAvailable commands"
                        manLines = My.Resources.es_Available_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaHistory of Kernel Simulator"
                        manLines = My.Resources.es_History_of_Kernel_Simulator.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaAvailable FTP commands"
                        manLines = My.Resources.es_Available_FTP_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaModding guide"
                        manLines = My.Resources.es_Modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaScreensaver modding guide"
                        manLines = My.Resources.es_Screensaver_modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaadduser"
                        manLines = My.Resources.es_adduser.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaalias"
                        manLines = My.Resources.es_alias.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaarginj"
                        manLines = My.Resources.es_arginj.Replace(Chr(13), "").Split(Chr(10))
                    Case "spacalc"
                        manLines = My.Resources.es_calc.Replace(Chr(13), "").Split(Chr(10))
                    Case "spachdir"
                        manLines = My.Resources.es_chdir.Replace(Chr(13), "").Split(Chr(10))
                    Case "spachhostname"
                        manLines = My.Resources.es_chhostname.Replace(Chr(13), "").Split(Chr(10))
                    Case "spachmal"
                        manLines = My.Resources.es_chmal.Replace(Chr(13), "").Split(Chr(10))
                    Case "spachmotd"
                        manLines = My.Resources.es_chmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "spachpwd"
                        manLines = My.Resources.es_chpwd.Replace(Chr(13), "").Split(Chr(10))
                    Case "spachusrname"
                        manLines = My.Resources.es_chusrname.Replace(Chr(13), "").Split(Chr(10))
                    Case "spacls"
                        manLines = My.Resources.es_cls.Replace(Chr(13), "").Split(Chr(10))
                    Case "spadebuglog"
                        manLines = My.Resources.es_debuglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaFTP changelocaldir", "spaFTP cdl"
                        manLines = My.Resources.es_FTP_changelocaldir_or_cdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaFTP changeremotedir", "spaFTP cdr"
                        manLines = My.Resources.es_FTP_changeremotedir_or_cdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaFTP connect"
                        manLines = My.Resources.es_FTP_connect.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaFTP currlocaldir", "spaFTP pwdl"
                        manLines = My.Resources.es_FTP_currlocaldir_or_pwdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaFTP currremotedir", "spaFTP pwdr"
                        manLines = My.Resources.es_FTP_currremotedir_or_pwdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaFTP delete", "spaFTP del"
                        manLines = My.Resources.es_FTP_delete_or_del.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaFTP disconnect"
                        manLines = My.Resources.es_FTP_disconnect.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaFTP download", "spaFTP get"
                        manLines = My.Resources.es_FTP_download_or_get.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaFTP listlocal", "spaFTP lsl"
                        manLines = My.Resources.es_FTP_listlocal_or_lsl.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaFTP listremote", "spaFTP lsr"
                        manLines = My.Resources.es_FTP_listremote_or_lsr.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaFTP rename", "spaFTP ren"
                        manLines = My.Resources.es_FTP_rename_or_ren.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaFTP upload", "spaFTP put"
                        manLines = My.Resources.es_FTP_upload_or_put.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaftp"
                        manLines = My.Resources.es_ftp.Replace(Chr(13), "").Split(Chr(10))
                    Case "spalist"
                        manLines = My.Resources.es_list.Replace(Chr(13), "").Split(Chr(10))
                    Case "spareloadsaver"
                        manLines = My.Resources.es_reloadsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "spalockscreen"
                        manLines = My.Resources.es_lockscreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "spalogout"
                        manLines = My.Resources.es_logout.Replace(Chr(13), "").Split(Chr(10))
                    Case "spalscomp"
                        manLines = My.Resources.es_lscomp.Replace(Chr(13), "").Split(Chr(10))
                    Case "spalsnet"
                        manLines = My.Resources.es_lsnet.Replace(Chr(13), "").Split(Chr(10))
                    Case "spalsnettree"
                        manLines = My.Resources.es_lsnettree.Replace(Chr(13), "").Split(Chr(10))
                    Case "spamd"
                        manLines = My.Resources.es_md.Replace(Chr(13), "").Split(Chr(10))
                    Case "spanetinfo"
                        manLines = My.Resources.es_netinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "spanoaliases"
                        manLines = My.Resources.es_noaliases.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaperm"
                        manLines = My.Resources.es_perm.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaping"
                        manLines = My.Resources.es_ping.Replace(Chr(13), "").Split(Chr(10))
                    Case "spard"
                        manLines = My.Resources.es_rd.Replace(Chr(13), "").Split(Chr(10))
                    Case "sparead"
                        manLines = My.Resources.es_read.Replace(Chr(13), "").Split(Chr(10))
                    Case "spareboot"
                        manLines = My.Resources.es_reboot.Replace(Chr(13), "").Split(Chr(10))
                    Case "spareloadconfig"
                        manLines = My.Resources.es_reloadconfig.Replace(Chr(13), "").Split(Chr(10))
                    Case "sparmuser"
                        manLines = My.Resources.es_rmuser.Replace(Chr(13), "").Split(Chr(10))
                    Case "spasavescreen"
                        manLines = My.Resources.es_savescreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "spascical"
                        manLines = My.Resources.es_scical.Replace(Chr(13), "").Split(Chr(10))
                    Case "spasetcolors"
                        manLines = My.Resources.es_setcolors.Replace(Chr(13), "").Split(Chr(10))
                    Case "spasetsaver"
                        manLines = My.Resources.es_setsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "spasetthemes"
                        manLines = My.Resources.es_setthemes.Replace(Chr(13), "").Split(Chr(10))
                    Case "spashowmotd"
                        manLines = My.Resources.es_showmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "spashowtd"
                        manLines = My.Resources.es_showtd.Replace(Chr(13), "").Split(Chr(10))
                    Case "spashowtdzone"
                        manLines = My.Resources.es_showtdzone.Replace(Chr(13), "").Split(Chr(10))
                    Case "spashutdown"
                        manLines = My.Resources.es_shutdown.Replace(Chr(13), "").Split(Chr(10))
                    Case "spasysinfo"
                        manLines = My.Resources.es_sysinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaunitconv"
                        manLines = My.Resources.es_unitconv.Replace(Chr(13), "").Split(Chr(10))
                    Case "spauseddeps"
                        manLines = My.Resources.es_useddeps.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaAvailable command-line arguments"
                        manLines = My.Resources.es_Available_command_line_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaAvailable kernel arguments"
                        manLines = My.Resources.es_Available_kernel_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "spaConfiguration for your Kernel"
                        manLines = My.Resources.es_Configuration_for_your_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    'Newly added manuals
                    Case "spachlang"
                        manLines = My.Resources.es_chlang.Replace(Chr(13), "").Split(Chr(10))
                    Case "spacdbglog"
                        manLines = My.Resources.es_cdbglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "spasses"
                        manLines = My.Resources.es_sses.Replace(Chr(13), "").Split(Chr(10))

                    'Swedish manuals
                    Case "sweIntroduction to the Kernel"
                        manLines = My.Resources.sv_Introduction_to_the_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweAvailable manual pages"
                        manLines = My.Resources.sv_Available_manual_pages.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweAvailable commands"
                        manLines = My.Resources.sv_Available_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweHistory of Kernel Simulator"
                        manLines = My.Resources.sv_History_of_Kernel_Simulator.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweAvailable FTP commands"
                        manLines = My.Resources.sv_Available_FTP_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweModding guide"
                        manLines = My.Resources.sv_Modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweScreensaver modding guide"
                        manLines = My.Resources.sv_Screensaver_modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweadduser"
                        manLines = My.Resources.sv_adduser.Replace(Chr(13), "").Split(Chr(10))
                    Case "swealias"
                        manLines = My.Resources.sv_alias.Replace(Chr(13), "").Split(Chr(10))
                    Case "swearginj"
                        manLines = My.Resources.sv_arginj.Replace(Chr(13), "").Split(Chr(10))
                    Case "swecalc"
                        manLines = My.Resources.sv_calc.Replace(Chr(13), "").Split(Chr(10))
                    Case "swechdir"
                        manLines = My.Resources.sv_chdir.Replace(Chr(13), "").Split(Chr(10))
                    Case "swechhostname"
                        manLines = My.Resources.sv_chhostname.Replace(Chr(13), "").Split(Chr(10))
                    Case "swechmal"
                        manLines = My.Resources.sv_chmal.Replace(Chr(13), "").Split(Chr(10))
                    Case "swechmotd"
                        manLines = My.Resources.sv_chmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "swechpwd"
                        manLines = My.Resources.sv_chpwd.Replace(Chr(13), "").Split(Chr(10))
                    Case "swechusrname"
                        manLines = My.Resources.sv_chusrname.Replace(Chr(13), "").Split(Chr(10))
                    Case "swecls"
                        manLines = My.Resources.sv_cls.Replace(Chr(13), "").Split(Chr(10))
                    Case "swedebuglog"
                        manLines = My.Resources.sv_debuglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweFTP changelocaldir", "sweFTP cdl"
                        manLines = My.Resources.sv_FTP_changelocaldir_or_cdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweFTP changeremotedir", "sweFTP cdr"
                        manLines = My.Resources.sv_FTP_changeremotedir_or_cdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweFTP connect"
                        manLines = My.Resources.sv_FTP_connect.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweFTP currlocaldir", "sweFTP pwdl"
                        manLines = My.Resources.sv_FTP_currlocaldir_or_pwdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweFTP currremotedir", "sweFTP pwdr"
                        manLines = My.Resources.sv_FTP_currremotedir_or_pwdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweFTP delete", "sweFTP del"
                        manLines = My.Resources.sv_FTP_delete_or_del.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweFTP disconnect"
                        manLines = My.Resources.sv_FTP_disconnect.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweFTP download", "sweFTP get"
                        manLines = My.Resources.sv_FTP_download_or_get.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweFTP listlocal", "sweFTP lsl"
                        manLines = My.Resources.sv_FTP_listlocal_or_lsl.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweFTP listremote", "sweFTP lsr"
                        manLines = My.Resources.sv_FTP_listremote_or_lsr.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweFTP rename", "sweFTP ren"
                        manLines = My.Resources.sv_FTP_rename_or_ren.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweFTP upload", "sweFTP put"
                        manLines = My.Resources.sv_FTP_upload_or_put.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweftp"
                        manLines = My.Resources.sv_ftp.Replace(Chr(13), "").Split(Chr(10))
                    Case "swelist"
                        manLines = My.Resources.sv_list.Replace(Chr(13), "").Split(Chr(10))
                    Case "swereloadsaver"
                        manLines = My.Resources.sv_reloadsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "swelockscreen"
                        manLines = My.Resources.sv_lockscreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "swelogout"
                        manLines = My.Resources.sv_logout.Replace(Chr(13), "").Split(Chr(10))
                    Case "swelscomp"
                        manLines = My.Resources.sv_lscomp.Replace(Chr(13), "").Split(Chr(10))
                    Case "swelsnet"
                        manLines = My.Resources.sv_lsnet.Replace(Chr(13), "").Split(Chr(10))
                    Case "swelsnettree"
                        manLines = My.Resources.sv_lsnettree.Replace(Chr(13), "").Split(Chr(10))
                    Case "swemd"
                        manLines = My.Resources.sv_md.Replace(Chr(13), "").Split(Chr(10))
                    Case "swenetinfo"
                        manLines = My.Resources.sv_netinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "swenoaliases"
                        manLines = My.Resources.sv_noaliases.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweperm"
                        manLines = My.Resources.sv_perm.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweping"
                        manLines = My.Resources.sv_ping.Replace(Chr(13), "").Split(Chr(10))
                    Case "swerd"
                        manLines = My.Resources.sv_rd.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweread"
                        manLines = My.Resources.sv_read.Replace(Chr(13), "").Split(Chr(10))
                    Case "swereboot"
                        manLines = My.Resources.sv_reboot.Replace(Chr(13), "").Split(Chr(10))
                    Case "swereloadconfig"
                        manLines = My.Resources.sv_reloadconfig.Replace(Chr(13), "").Split(Chr(10))
                    Case "swermuser"
                        manLines = My.Resources.sv_rmuser.Replace(Chr(13), "").Split(Chr(10))
                    Case "swesavescreen"
                        manLines = My.Resources.sv_savescreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "swescical"
                        manLines = My.Resources.sv_scical.Replace(Chr(13), "").Split(Chr(10))
                    Case "swesetcolors"
                        manLines = My.Resources.sv_setcolors.Replace(Chr(13), "").Split(Chr(10))
                    Case "swesetsaver"
                        manLines = My.Resources.sv_setsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "swesetthemes"
                        manLines = My.Resources.sv_setthemes.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweshowmotd"
                        manLines = My.Resources.sv_showmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweshowtd"
                        manLines = My.Resources.sv_showtd.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweshowtdzone"
                        manLines = My.Resources.sv_showtdzone.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweshutdown"
                        manLines = My.Resources.sv_shutdown.Replace(Chr(13), "").Split(Chr(10))
                    Case "swesysinfo"
                        manLines = My.Resources.sv_sysinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweunitconv"
                        manLines = My.Resources.sv_unitconv.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweuseddeps"
                        manLines = My.Resources.sv_useddeps.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweAvailable command-line arguments"
                        manLines = My.Resources.sv_Available_command_line_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweAvailable kernel arguments"
                        manLines = My.Resources.sv_Available_kernel_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "sweConfiguration for your Kernel"
                        manLines = My.Resources.sv_Configuration_for_your_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    'Newly added manuals
                    Case "swechlang"
                        manLines = My.Resources.sv_chlang.Replace(Chr(13), "").Split(Chr(10))
                    Case "swecdbglog"
                        manLines = My.Resources.sv_cdbglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "swesses"
                        manLines = My.Resources.sv_sses.Replace(Chr(13), "").Split(Chr(10))

                    'Turkish manuals
                    Case "tkyIntroduction to the Kernel"
                        manLines = My.Resources.tr_Introduction_to_the_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyAvailable manual pages"
                        manLines = My.Resources.tr_Available_manual_pages.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyAvailable commands"
                        manLines = My.Resources.tr_Available_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyHistory of Kernel Simulator"
                        manLines = My.Resources.tr_History_of_Kernel_Simulator.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyAvailable FTP commands"
                        manLines = My.Resources.tr_Available_FTP_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyModding guide"
                        manLines = My.Resources.tr_Modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyScreensaver modding guide"
                        manLines = My.Resources.tr_Screensaver_modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyadduser"
                        manLines = My.Resources.tr_adduser.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyalias"
                        manLines = My.Resources.tr_alias.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyarginj"
                        manLines = My.Resources.tr_arginj.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkycalc"
                        manLines = My.Resources.tr_calc.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkychdir"
                        manLines = My.Resources.tr_chdir.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkychhostname"
                        manLines = My.Resources.tr_chhostname.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkychmal"
                        manLines = My.Resources.tr_chmal.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkychmotd"
                        manLines = My.Resources.tr_chmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkychpwd"
                        manLines = My.Resources.tr_chpwd.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkychusrname"
                        manLines = My.Resources.tr_chusrname.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkycls"
                        manLines = My.Resources.tr_cls.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkydebuglog"
                        manLines = My.Resources.tr_debuglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyFTP changelocaldir", "tkyFTP cdl"
                        manLines = My.Resources.tr_FTP_changelocaldir_or_cdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyFTP changeremotedir", "tkyFTP cdr"
                        manLines = My.Resources.tr_FTP_changeremotedir_or_cdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyFTP connect"
                        manLines = My.Resources.tr_FTP_connect.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyFTP currlocaldir", "tkyFTP pwdl"
                        manLines = My.Resources.tr_FTP_currlocaldir_or_pwdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyFTP currremotedir", "tkyFTP pwdr"
                        manLines = My.Resources.tr_FTP_currremotedir_or_pwdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyFTP delete", "tkyFTP del"
                        manLines = My.Resources.tr_FTP_delete_or_del.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyFTP disconnect"
                        manLines = My.Resources.tr_FTP_disconnect.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyFTP download", "tkyFTP get"
                        manLines = My.Resources.tr_FTP_download_or_get.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyFTP listlocal", "tkyFTP lsl"
                        manLines = My.Resources.tr_FTP_listlocal_or_lsl.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyFTP listremote", "tkyFTP lsr"
                        manLines = My.Resources.tr_FTP_listremote_or_lsr.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyFTP rename", "tkyFTP ren"
                        manLines = My.Resources.tr_FTP_rename_or_ren.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyFTP upload", "tkyFTP put"
                        manLines = My.Resources.tr_FTP_upload_or_put.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyftp"
                        manLines = My.Resources.tr_ftp.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkylist"
                        manLines = My.Resources.tr_list.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyreloadsaver"
                        manLines = My.Resources.tr_reloadsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkylockscreen"
                        manLines = My.Resources.tr_lockscreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkylogout"
                        manLines = My.Resources.tr_logout.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkylscomp"
                        manLines = My.Resources.tr_lscomp.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkylsnet"
                        manLines = My.Resources.tr_lsnet.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkylsnettree"
                        manLines = My.Resources.tr_lsnettree.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkymd"
                        manLines = My.Resources.tr_md.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkynetinfo"
                        manLines = My.Resources.tr_netinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkynoaliases"
                        manLines = My.Resources.tr_noaliases.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyperm"
                        manLines = My.Resources.tr_perm.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyping"
                        manLines = My.Resources.tr_ping.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyrd"
                        manLines = My.Resources.tr_rd.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyread"
                        manLines = My.Resources.tr_read.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyreboot"
                        manLines = My.Resources.tr_reboot.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyreloadconfig"
                        manLines = My.Resources.tr_reloadconfig.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyrmuser"
                        manLines = My.Resources.tr_rmuser.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkysavescreen"
                        manLines = My.Resources.tr_savescreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyscical"
                        manLines = My.Resources.tr_scical.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkysetcolors"
                        manLines = My.Resources.tr_setcolors.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkysetsaver"
                        manLines = My.Resources.tr_setsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkysetthemes"
                        manLines = My.Resources.tr_setthemes.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyshowmotd"
                        manLines = My.Resources.tr_showmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyshowtd"
                        manLines = My.Resources.tr_showtd.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyshowtdzone"
                        manLines = My.Resources.tr_showtdzone.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyshutdown"
                        manLines = My.Resources.tr_shutdown.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkysysinfo"
                        manLines = My.Resources.tr_sysinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyunitconv"
                        manLines = My.Resources.tr_unitconv.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyuseddeps"
                        manLines = My.Resources.tr_useddeps.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyAvailable command-line arguments"
                        manLines = My.Resources.tr_Available_command_line_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyAvailable kernel arguments"
                        manLines = My.Resources.tr_Available_kernel_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkyConfiguration for your Kernel"
                        manLines = My.Resources.tr_Configuration_for_your_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    'Newly added manuals
                    Case "tkychlang"
                        manLines = My.Resources.tr_chlang.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkycdbglog"
                        manLines = My.Resources.tr_cdbglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "tkysses"
                        manLines = My.Resources.tr_sses.Replace(Chr(13), "").Split(Chr(10))
                End Select
                Wdbg("Checking manual {0}", ManTitle)
                For Each manLine As String In manLines
                    If InternalParseDone = True Then 'Check for the rest if the manpage has MAN START section
                        CheckTODO(manLine)
                        If BodyParsing = True Then
                            ParseBody(manLine)
                        ElseIf ColorParsing = True Then
                            ParseColor(manLine)
                        ElseIf SectionParsing = True Then
                            ParseSection(manLine)
                        Else
                            ParseMan_INTERNAL(manLine)
                        End If
                    ElseIf InternalParseDone = False Then 'Check for the MAN START section
                        If manLine = "(*MAN START*)" Then
                            Wdbg("Successfully found (*MAN START*) in manpage {0}.", ManTitle)
                            InternalParseDone = True
                        End If
                    End If
                Next
                If InternalParseDone = True Then
                    Wdbg("Valid manual page! ({0})", ManTitle)
                    Sanity_INTERNAL(ManTitle)
                Else
                    Throw New EventsAndExceptions.TruncatedManpageException(DoTranslation("The manual page {0} is somehow truncated.", currentLang))
                End If
            End If
        Catch ex As Exception
            Wdbg("The manual page {0} is somehow truncated. {1}", ManTitle, ex.Message)
            Wdbg(ex.StackTrace)
            Wln(DoTranslation("There is an error when trying to load the manual page {0} becuase {1}.", currentLang), "neutralText", ManTitle, ex.Message)
            If DebugMode = True Then
                Wln(ex.StackTrace, "neutralText")
            End If
        End Try
    End Sub

    'Check for any TODO
    Public Sub CheckTODO(ByVal line As String)
        If line.Contains("TODO") Then
            Wdbg("TODO found on this line: {0}", line)
            Dim TODOindex As Integer = InStr(line, "TODO")
            Pages(ManTitle).Todos.Add(line.Substring(TODOindex + "TODO".Length + 1))
        End If
    End Sub

    'Parse manual file from KS, not mods
    Public Sub ParseMan_INTERNAL(ByVal line As String)
        If line.StartsWith("-REVISION:") Then
            Wdbg("Revision found on this line: {0}", line)
            Dim Rev As String = line.Substring(line.IndexOf(":") + 1)
            If Rev = "" Then
                Wdbg("Revision not defined. Assuming v1...")
                Rev = "1"
            End If
            Pages(ManTitle).ManualRevision = Rev
        ElseIf line.StartsWith("-KSLAYOUT:") Then
            Dim Lay As String = line.Substring(line.IndexOf(":") + 1)
            If Not AvailableLayouts.Contains(Lay) Then
                Wdbg("Layout {0} not found in the available layouts. Assuming 0.0.5.9-OR-ABOVE...", Lay)
                Lay = "0.0.5.9-OR-ABOVE"
            End If
            Pages(ManTitle).ManualLayoutVersion = Lay
        ElseIf line = "-BODY START-" Then
            BodyParsing = True
        ElseIf line = "-COLOR CONFIGURATION-" Then
            ColorParsing = True
        ElseIf line = "-SECTIONS-" Then
            SectionParsing = True
        End If
    End Sub

    'Parse manual file from mods (Not implemented yet)
#Disable Warning IDE0060
    Public Sub ParseMan_EXTERNAL(ByVal line As String, ByVal ManFile As String)
#Enable Warning IDE0060
        Throw New NotImplementedException
    End Sub

    'Get strings until end of body
    Public Sub ParseBody(ByVal line As String)
        If line <> "-BODY END-" Then
            'If line <> "" Then Wdbg("Appending {0} to builder", line) (Causes spam - Don't uncomment until further notice)
            Pages(ManTitle).Body.Append(line + vbNewLine)
        ElseIf line.StartsWith("~~-") = False Then 'If the line does not start with the comment
            BodyParsing = False
        End If
    End Sub

    'The colors on the manpage will be parsed
    Public Sub ParseColor(ByVal line As String)
        If line <> "-COLOR CONFIG END-" Then
            Dim colors_MAN() As String = line.Split("=>".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
            If Not colors_MAN.Length = 0 Then
                If Not Pages(ManTitle).Colors.ContainsKey(colors_MAN(0)) Then
                    Pages(ManTitle).Colors.Add(colors_MAN(0), [Enum].Parse(GetType(ConsoleColor), colors_MAN(1)))
                    Wdbg("The color {0} is being assigned to {1}, according to: {2}", colors_MAN(1).ToString, colors_MAN(0), line)
                End If
            End If
        ElseIf line.StartsWith("~~-") = False Then
            ColorParsing = False
        End If
    End Sub

    'Parse sections
    Public Sub ParseSection(ByVal line As String)
        If line <> "-SECTIONS END-" Then
            Dim sections() As String = line.Split("=>".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
            Pages(ManTitle).Sections.Add(sections(0), sections(1))
            Wdbg("The section {0} is being assigned to {1}, according to {2}", sections(1), sections(0), line)
        ElseIf line.StartsWith("~~-") = False Then
            SectionParsing = False
        End If
    End Sub

    'Perform a sanity check on internal manpages
    Public Sub Sanity_INTERNAL(ByVal title As String)
        If title = "" Then
            UnknownTitleCount += 1
            Wdbg("The manual page #{0} seems to have no title.", UnknownTitleCount)
            Dim originalValue As Manual = Pages(title)
            Pages.Remove(title)
            title = "Manual page #" + UnknownTitleCount
            Pages.Add(title, originalValue)
            Pages(title).ManualTitle = title
            Wdbg("Title has changed to ""{0}""", title)
            Wln(DoTranslation("This manual page title is not written", currentLang), "neutralText")
        ElseIf Pages(title).Body.ToString = "" Then
            Wdbg("Body for ""{0}"" does not contain anything.", title)
            Wln(DoTranslation("This manual page ({0}) does not contain any body text. Deleting page...", currentLang), "neutralText", title)
            Pages.Remove(title)
        ElseIf Pages(title).Sections.Count = 0 Then
            Wdbg("No sections for ""{0}""", title)
            Wln(DoTranslation("This manual page ({0}) does not contain any section. Deleting page...", currentLang), "neutralText", title)
            Pages.Remove(title)
        End If
    End Sub

    'Perform a sanity check on mod manpages (Not implemented)
#Disable Warning IDE0060
    Public Sub Sanity_EXTERNAL(ByVal title As String)
#Enable Warning IDE0060
        Throw New NotImplementedException
    End Sub

End Module
