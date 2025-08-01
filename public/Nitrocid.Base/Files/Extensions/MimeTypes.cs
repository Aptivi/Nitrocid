//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Magico.Files;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using System.Collections.Generic;

namespace Nitrocid.Base.Files.Extensions
{
    /// <summary>
    /// Simple MIME management class
    /// </summary>
    public static class MimeTypes
    {
        private static readonly Dictionary<string, string> mimeTypes = new()
        {
            { ".323", "text/h323" },
            { ".3g2", "video/3gpp2" },
            { ".3gp", "video/3gpp" },
            { ".7z", "application/x-7z-compressed" },
            { ".aab", "application/x-authorware-bin" },
            { ".aac", "audio/aac" },
            { ".aam", "application/x-authorware-map" },
            { ".aas", "application/x-authorware-seg" },
            { ".abc", "text/vnd.abc" },
            { ".acgi", "text/html" },
            { ".acx", "application/internet-property-stream" },
            { ".afl", "video/animaflex" },
            { ".ai", "application/postscript" },
            { ".aif", "audio/aiff" },
            { ".aifc", "audio/aiff" },
            { ".aiff", "audio/aiff" },
            { ".aim", "application/x-aim" },
            { ".aip", "text/x-audiosoft-intra" },
            { ".ani", "application/x-navi-animation" },
            { ".aos", "application/x-nokia-9000-communicator-add-on-software" },
            { ".appcache", "text/cache-manifest" },
            { ".application", "application/x-ms-application" },
            { ".aps", "application/mime" },
            { ".art", "image/x-jg" },
            { ".asf", "video/x-ms-asf" },
            { ".asm", "text/x-asm" },
            { ".asp", "text/asp" },
            { ".asr", "video/x-ms-asf" },
            { ".asx", "application/x-mplayer2" },
            { ".atom", "application/atom+xml" },
            { ".au", "audio/x-au" },
            { ".avi", "video/avi" },
            { ".avs", "video/avs-video" },
            { ".axs", "application/olescript" },
            { ".azw", "application/vnd.amazon.ebook" },
            { ".bas", "text/plain" },
            { ".bcpio", "application/x-bcpio" },
            { ".bin", "application/octet-stream" },
            { ".bm", "image/bmp" },
            { ".bmp", "image/bmp" },
            { ".boo", "application/book" },
            { ".book", "application/book" },
            { ".boz", "application/x-bzip2" },
            { ".bsh", "application/x-bsh" },
            { ".bz2", "application/x-bzip2" },
            { ".bz", "application/x-bzip" },
            { ".cat", "application/vnd.ms-pki.seccat" },
            { ".ccad", "application/clariscad" },
            { ".cco", "application/x-cocoa" },
            { ".cc", "text/plain" },
            { ".cdf", "application/cdf" },
            { ".cer", "application/pkix-cert" },
            { ".cha", "application/x-chat" },
            { ".chat", "application/x-chat" },
            { ".chm", "application/vnd.ms-htmlhelp" },
            { ".class", "application/x-java-applet" },
            { ".clp", "application/x-msclip" },
            { ".cmx", "image/x-cmx" },
            { ".cod", "image/cis-cod" },
            { ".coffee", "text/x-coffeescript" },
            { ".conf", "text/plain" },
            { ".cpio", "application/x-cpio" },
            { ".cpp", "text/plain" },
            { ".cpt", "application/x-cpt" },
            { ".crd", "application/x-mscardfile" },
            { ".crl", "application/pkix-crl" },
            { ".crt", "application/pkix-cert" },
            { ".csh", "application/x-csh" },
            { ".css", "text/css" },
            { ".csv", "text/csv" },
            { ".cs", "text/plain" },
            { ".c", "text/plain" },
            { ".c++", "text/plain" },
            { ".cxx", "text/plain" },
            { ".dart", "application/dart" },
            { ".dcr", "application/x-director" },
            { ".deb", "application/x-deb" },
            { ".deepv", "application/x-deepv" },
            { ".def", "text/plain" },
            { ".deploy", "application/octet-stream" },
            { ".der", "application/x-x509-ca-cert" },
            { ".dib", "image/bmp" },
            { ".dif", "video/x-dv" },
            { ".dir", "application/x-director" },
            { ".disco", "text/xml" },
            { ".dll", "application/x-msdownload" },
            { ".dl", "video/dl" },
            { ".doc", "application/msword" },
            { ".docm", "application/vnd.ms-word.document.macroEnabled.12" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ".dot", "application/msword" },
            { ".dotm", "application/vnd.ms-word.template.macroEnabled.12" },
            { ".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template" },
            { ".dp", "application/commonground" },
            { ".drw", "application/drafting" },
            { ".dtd", "application/xml-dtd" },
            { ".dvi", "application/x-dvi" },
            { ".dv", "video/x-dv" },
            { ".dwg", "application/acad" },
            { ".dxf", "application/dxf" },
            { ".dxr", "application/x-director" },
            { ".el", "text/x-script.elisp" },
            { ".elc", "application/x-elc" },
            { ".emf", "image/emf" },
            { ".eml", "message/rfc822" },
            { ".eot", "application/vnd.bw-fontobject" },
            { ".eps", "application/postscript" },
            { ".epub", "application/epub+zip" },
            { ".es", "application/x-esrehber" },
            { ".etx", "text/x-setext" },
            { ".evy", "application/envoy" },
            { ".exe", "application/octet-stream" },
            { ".f77", "text/plain" },
            { ".f90", "text/plain" },
            { ".fdf", "application/vnd.fdf" },
            { ".fif", "image/fif" },
            { ".flac", "audio/x-flac" },
            { ".fli", "video/fli" },
            { ".flx", "text/vnd.fmi.flexstor" },
            { ".fmf", "video/x-atomic3d-feature" },
            { ".for", "text/plain" },
            { ".fpx", "image/vnd.fpx" },
            { ".frl", "application/freeloader" },
            { ".fsx", "application/fsharp-script" },
            { ".g3", "image/g3fax" },
            { ".gif", "image/gif" },
            { ".gl", "video/gl" },
            { ".gsd", "audio/x-gsm" },
            { ".gsm", "audio/x-gsm" },
            { ".gsp", "application/x-gsp" },
            { ".gss", "application/x-gss" },
            { ".gtar", "application/x-gtar" },
            { ".g", "text/plain" },
            { ".gz", "application/x-gzip" },
            { ".gzip", "application/x-gzip" },
            { ".hdf", "application/x-hdf" },
            { ".help", "application/x-helpfile" },
            { ".hgl", "application/vnd.hp-HPGL" },
            { ".hh", "text/plain" },
            { ".hlb", "text/x-script" },
            { ".hlp", "application/x-helpfile" },
            { ".hpg", "application/vnd.hp-HPGL" },
            { ".hpgl", "application/vnd.hp-HPGL" },
            { ".hqx", "application/binhex" },
            { ".hta", "application/hta" },
            { ".htc", "text/x-component" },
            { ".h", "text/plain" },
            { ".htmls", "text/html" },
            { ".html", "text/html" },
            { ".htm", "text/html" },
            { ".htt", "text/webviewhtml" },
            { ".htx", "text/html" },
            { ".ico", "image/vnd.microsoft.icon" },
            { ".ics", "text/calendar" },
            { ".idc", "text/plain" },
            { ".ief", "image/ief" },
            { ".iefs", "image/ief" },
            { ".iges", "model/iges" },
            { ".igs", "model/iges" },
            { ".iii", "application/x-iphone" },
            { ".ima", "application/x-ima" },
            { ".imap", "application/x-httpd-imap" },
            { ".inf", "application/inf" },
            { ".ins", "application/x-internett-signup" },
            { ".ip", "application/x-ip2" },
            { ".isp", "application/x-internet-signup" },
            { ".isu", "video/x-isvideo" },
            { ".it", "audio/it" },
            { ".iv", "application/x-inventor" },
            { ".ivf", "video/x-ivf" },
            { ".ivy", "application/x-livescreen" },
            { ".jam", "audio/x-jam" },
            { ".jar", "application/java-archive" },
            { ".java", "text/plain" },
            { ".jav", "text/plain" },
            { ".jcm", "application/x-java-commerce" },
            { ".jfif", "image/jpeg" },
            { ".jfif-tbnl", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".jpe", "image/jpeg" },
            { ".jpg", "image/jpeg" },
            { ".jps", "image/x-jps" },
            { ".js", "text/javascript" },
            { ".json", "application/json" },
            { ".jsonld", "application/ld+json" },
            { ".jut", "image/jutvision" },
            { ".kar", "audio/midi" },
            { ".ksh", "text/x-script.ksh" },
            { ".la", "audio/nspaudio" },
            { ".lam", "audio/x-liveaudio" },
            { ".latex", "application/x-latex" },
            { ".list", "text/plain" },
            { ".lma", "audio/nspaudio" },
            { ".log", "text/plain" },
            { ".lsp", "application/x-lisp" },
            { ".lst", "text/plain" },
            { ".lsx", "text/x-la-asf" },
            { ".ltx", "application/x-latex" },
            { ".m13", "application/x-msmediaview" },
            { ".m14", "application/x-msmediaview" },
            { ".m1v", "video/mpeg" },
            { ".m2a", "audio/mpeg" },
            { ".m2v", "video/mpeg" },
            { ".m3u", "audio/x-mpequrl" },
            { ".m4a", "audio/mp4" },
            { ".m4v", "video/mp4" },
            { ".man", "application/x-troff-man" },
            { ".manifest", "application/x-ms-manifest" },
            { ".map", "application/x-navimap" },
            { ".mar", "text/plain" },
            { ".markdown", "text/markdown" },
            { ".mbd", "application/mbedlet" },
            { ".mc$", "application/x-magic-cap-package-1.0" },
            { ".mcd", "application/mcad" },
            { ".mcf", "image/vasa" },
            { ".mcp", "application/netmc" },
            { ".md", "text/markdown" },
            { ".mdb", "application/x-msaccess" },
            { ".mesh", "model/mesh" },
            { ".me", "application/x-troff-me" },
            { ".mid", "audio/midi" },
            { ".midi", "audio/midi" },
            { ".mif", "application/x-mif" },
            { ".mjf", "audio/x-vnd.AudioExplosion.MjuiceMediaFile" },
            { ".mjpg", "video/x-motion-jpeg" },
            { ".mjs", "text/javascript" },
            { ".mkv", "video/x-matroska" },
            { ".mm", "application/base64" },
            { ".mme", "application/base64" },
            { ".mny", "application/x-msmoney" },
            { ".mod", "audio/mod" },
            { ".mov", "video/quicktime" },
            { ".movie", "video/x-sgi-movie" },
            { ".mp2", "video/mpeg" },
            { ".mp3", "audio/mpeg" },
            { ".mp4", "video/mp4" },
            { ".mp4a", "audio/mp4" },
            { ".mp4v", "video/mp4" },
            { ".mpa", "audio/mpeg" },
            { ".mpc", "application/x-project" },
            { ".mpeg", "video/mpeg" },
            { ".mpe", "video/mpeg" },
            { ".mpga", "audio/mpeg" },
            { ".mpg", "video/mpeg" },
            { ".mpp", "application/vnd.ms-project" },
            { ".mpt", "application/x-project" },
            { ".mpv2", "video/mpeg" },
            { ".mpv", "application/x-project" },
            { ".mpx", "application/x-project" },
            { ".mrc", "application/marc" },
            { ".ms", "application/x-troff-ms" },
            { ".msg", "application/vnd.ms-outlook" },
            { ".msh", "model/mesh" },
            { ".m", "text/plain" },
            { ".mvb", "application/x-msmediaview" },
            { ".mv", "video/x-sgi-movie" },
            { ".mzz", "application/x-vnd.AudioExplosion.mzz" },
            { ".nap", "image/naplps" },
            { ".naplps", "image/naplps" },
            { ".nc", "application/x-netcdf" },
            { ".ncm", "application/vnd.nokia.configuration-message" },
            { ".niff", "image/x-niff" },
            { ".nif", "image/x-niff" },
            { ".nix", "application/x-mix-transfer" },
            { ".nsc", "application/x-conference" },
            { ".nvd", "application/x-navidoc" },
            { ".nws", "message/rfc822" },
            { ".oda", "application/oda" },
            { ".odp", "application/vnd.oasis.opendocument.presentation" },
            { ".ods", "application/vnd.oasis.opendocument.spreadsheet" },
            { ".odt", "application/vnd.oasis.opendocument.text" },
            { ".oga", "audio/ogg" },
            { ".ogg", "audio/ogg" },
            { ".ogv", "video/ogg" },
            { ".ogx", "application/ogg" },
            { ".omc", "application/x-omc" },
            { ".omcd", "application/x-omcdatamaker" },
            { ".omcr", "application/x-omcregerator" },
            { ".opus", "audio/opus" },
            { ".otf", "font/otf" },
            { ".oxps", "application/oxps" },
            { ".p10", "application/pkcs10" },
            { ".p12", "application/pkcs-12" },
            { ".p7a", "application/x-pkcs7-signature" },
            { ".p7b", "application/x-pkcs7-certificates" },
            { ".p7c", "application/pkcs7-mime" },
            { ".p7m", "application/pkcs7-mime" },
            { ".p7r", "application/x-pkcs7-certreqresp" },
            { ".p7s", "application/pkcs7-signature" },
            { ".part", "application/pro_eng" },
            { ".pas", "text/pascal" },
            { ".pbm", "image/x-portable-bitmap" },
            { ".pcl", "application/x-pcl" },
            { ".pct", "image/x-pict" },
            { ".pcx", "image/x-pcx" },
            { ".pdf", "application/pdf" },
            { ".pfx", "application/x-pkcs12" },
            { ".pgm", "image/x-portable-graymap" },
            { ".pic", "image/pict" },
            { ".pict", "image/pict" },
            { ".pkg", "application/x-newton-compatible-pkg" },
            { ".pko", "application/vnd.ms-pki.pko" },
            { ".pl", "text/plain" },
            { ".plx", "application/x-PiXCLscript" },
            { ".pm4", "application/x-pagemaker" },
            { ".pm5", "application/x-pagemaker" },
            { ".pma", "application/x-perfmon" },
            { ".pmc", "application/x-perfmon" },
            { ".pm", "image/x-xpixmap" },
            { ".pml", "application/x-perfmon" },
            { ".pmr", "application/x-perfmon" },
            { ".pmw", "application/x-perfmon" },
            { ".png", "image/png" },
            { ".pnm", "application/x-portable-anymap" },
            { ".pot", "application/vnd.ms-powerpoint" },
            { ".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12" },
            { ".potx", "application/vnd.openxmlformats-officedocument.presentationml.template" },
            { ".pov", "model/x-pov" },
            { ".ppa", "application/vnd.ms-powerpoint" },
            { ".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12" },
            { ".ppm", "image/x-portable-pixmap" },
            { ".pps", "application/vnd.ms-powerpoint" },
            { ".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12" },
            { ".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow" },
            { ".ppt", "application/vnd.ms-powerpoint" },
            { ".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12" },
            { ".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
            { ".ppz", "application/mspowerpoint" },
            { ".pre", "application/x-freelance" },
            { ".prf", "application/pics-rules" },
            { ".prt", "application/pro_eng" },
            { ".ps", "application/postscript" },
            { ".p", "text/x-pascal" },
            { ".pub", "application/x-mspublisher" },
            { ".pwz", "application/vnd.ms-powerpoint" },
            { ".pyc", "application/x-bytecode.python" },
            { ".py", "text/x-script.phyton" },
            { ".qcp", "audio/vnd.qcelp" },
            { ".qif", "image/x-quicktime" },
            { ".qtc", "video/x-qtc" },
            { ".qtif", "image/x-quicktime" },
            { ".qti", "image/x-quicktime" },
            { ".qt", "video/quicktime" },
            { ".ra", "audio/x-pn-realaudio" },
            { ".ram", "audio/x-pn-realaudio" },
            { ".rar", "application/vnd.rar" },
            { ".ras", "application/x-cmu-raster" },
            { ".rast", "image/cmu-raster" },
            { ".rexx", "text/x-script.rexx" },
            { ".rf", "image/vnd.rn-realflash" },
            { ".rgb", "image/x-rgb" },
            { ".rm", "application/vnd.rn-realmedia" },
            { ".rmi", "audio/mid" },
            { ".rmm", "audio/x-pn-realaudio" },
            { ".rmp", "audio/x-pn-realaudio" },
            { ".rng", "application/ringing-tones" },
            { ".rnx", "application/vnd.rn-realplayer" },
            { ".roff", "application/x-troff" },
            { ".rp", "image/vnd.rn-realpix" },
            { ".rpm", "audio/x-pn-realaudio-plugin" },
            { ".rss", "application/rss+xml" },
            { ".rtf", "text/rtf" },
            { ".rt", "text/richtext" },
            { ".rtx", "text/richtext" },
            { ".rv", "video/vnd.rn-realvideo" },
            { ".s3m", "audio/s3m" },
            { ".sbk", "application/x-tbook" },
            { ".scd", "application/x-msschedule" },
            { ".scm", "application/x-lotusscreencam" },
            { ".sct", "text/scriptlet" },
            { ".sdml", "text/plain" },
            { ".sdp", "application/sdp" },
            { ".sdr", "application/sounder" },
            { ".sea", "application/sea" },
            { ".set", "application/set" },
            { ".setpay", "application/set-payment-initiation" },
            { ".setreg", "application/set-registration-initiation" },
            { ".sgml", "text/sgml" },
            { ".sgm", "text/sgml" },
            { ".shar", "application/x-bsh" },
            { ".sh", "text/x-script.sh" },
            { ".shtml", "text/html" },
            { ".sid", "audio/x-psid" },
            { ".silo", "model/mesh" },
            { ".sit", "application/x-sit" },
            { ".skd", "application/x-koan" },
            { ".skm", "application/x-koan" },
            { ".skp", "application/x-koan" },
            { ".skt", "application/x-koan" },
            { ".sl", "application/x-seelogo" },
            { ".smi", "application/smil" },
            { ".smil", "application/smil" },
            { ".snd", "audio/basic" },
            { ".sol", "application/solids" },
            { ".spc", "application/x-pkcs7-certificates" },
            { ".spl", "application/futuresplash" },
            { ".spr", "application/x-sprite" },
            { ".sprite", "application/x-sprite" },
            { ".spx", "audio/ogg" },
            { ".src", "application/x-wais-source" },
            { ".ssi", "text/x-server-parsed-html" },
            { ".ssm", "application/streamingmedia" },
            { ".sst", "application/vnd.ms-pki.certstore" },
            { ".step", "application/step" },
            { ".s", "text/x-asm" },
            { ".stl", "application/sla" },
            { ".stm", "text/html" },
            { ".stp", "application/step" },
            { ".sv4cpio", "application/x-sv4cpio" },
            { ".sv4crc", "application/x-sv4crc" },
            { ".svf", "image/x-dwg" },
            { ".svg", "image/svg+xml" },
            { ".svr", "application/x-world" },
            { ".swf", "application/x-shockwave-flash" },
            { ".talk", "text/x-speech" },
            { ".t", "application/x-troff" },
            { ".tar", "application/x-tar" },
            { ".tbk", "application/toolbook" },
            { ".tcl", "text/x-script.tcl" },
            { ".tcsh", "text/x-script.tcsh" },
            { ".tex", "application/x-tex" },
            { ".texi", "application/x-texinfo" },
            { ".texinfo", "application/x-texinfo" },
            { ".text", "text/plain" },
            { ".tgz", "application/x-compressed" },
            { ".tiff", "image/tiff" },
            { ".tif", "image/tiff" },
            { ".tr", "application/x-troff" },
            { ".trm", "application/x-msterminal" },
            { ".ts", "application/typescript" },
            { ".tsi", "audio/tsp-audio" },
            { ".tsp", "audio/tsplayer" },
            { ".tsv", "text/tab-separated-values" },
            { ".ttc", "font/collection" },
            { ".ttf", "font/ttf" },
            { ".txt", "text/plain" },
            { ".uil", "text/x-uil" },
            { ".uls", "text/iuls" },
            { ".unis", "text/uri-list" },
            { ".uni", "text/uri-list" },
            { ".unv", "application/i-deas" },
            { ".uris", "text/uri-list" },
            { ".uri", "text/uri-list" },
            { ".ustar", "multipart/x-ustar" },
            { ".uue", "text/x-uuencode" },
            { ".uu", "text/x-uuencode" },
            { ".vcd", "application/x-cdlink" },
            { ".vcf", "text/vcard" },
            { ".vcard", "text/vcard" },
            { ".vcs", "text/x-vcalendar" },
            { ".vda", "application/vda" },
            { ".vdo", "video/vdo" },
            { ".vew", "application/groupwise" },
            { ".vivo", "video/vnd.vivo" },
            { ".viv", "video/vnd.vivo" },
            { ".vmd", "application/vocaltec-media-desc" },
            { ".vmf", "application/vocaltec-media-file" },
            { ".voc", "audio/voc" },
            { ".vos", "video/vosaic" },
            { ".vox", "audio/voxware" },
            { ".vqe", "audio/x-twinvq-plugin" },
            { ".vqf", "audio/x-twinvq" },
            { ".vql", "audio/x-twinvq-plugin" },
            { ".vrml", "application/x-vrml" },
            { ".vsd", "application/x-visio" },
            { ".vst", "application/x-visio" },
            { ".vsw", "application/x-visio" },
            { ".w60", "application/wordperfect6.0" },
            { ".w61", "application/wordperfect6.1" },
            { ".w6w", "application/msword" },
            { ".wav", "audio/wav" },
            { ".wb1", "application/x-qpro" },
            { ".wbmp", "image/vnd.wap.wbmp" },
            { ".wcm", "application/vnd.ms-works" },
            { ".wdb", "application/vnd.ms-works" },
            { ".web", "application/vnd.xara" },
            { ".weba", "audio/webm" },
            { ".webm", "video/webm" },
            { ".webp", "image/webp" },
            { ".wiz", "application/msword" },
            { ".wk1", "application/x-123" },
            { ".wks", "application/vnd.ms-works" },
            { ".wmf", "image/wmf" },
            { ".wmlc", "application/vnd.wap.wmlc" },
            { ".wmlsc", "application/vnd.wap.wmlscriptc" },
            { ".wmls", "text/vnd.wap.wmlscript" },
            { ".wml", "text/vnd.wap.wml" },
            { ".wmp", "video/x-ms-wmp" },
            { ".wmv", "video/x-ms-wmv" },
            { ".wmx", "video/x-ms-wmx" },
            { ".woff", "font/woff" },
            { ".woff2", "font/woff2" },
            { ".word", "application/msword" },
            { ".wp5", "application/wordperfect" },
            { ".wp6", "application/wordperfect" },
            { ".wp", "application/wordperfect" },
            { ".wpd", "application/wordperfect" },
            { ".wps", "application/vnd.ms-works" },
            { ".wq1", "application/x-lotus" },
            { ".wri", "application/mswrite" },
            { ".wrl", "application/x-world" },
            { ".wrz", "model/vrml" },
            { ".wsc", "text/scriplet" },
            { ".wsdl", "text/xml" },
            { ".wsrc", "application/x-wais-source" },
            { ".wtk", "application/x-wintalk" },
            { ".wvx", "video/x-ms-wvx" },
            { ".x3d", "model/x3d+xml" },
            { ".x3db", "model/x3d+fastinfoset" },
            { ".x3dv", "model/x3d-vrml" },
            { ".xaml", "application/xaml+xml" },
            { ".xap", "application/x-silverlight-app" },
            { ".xbap", "application/x-ms-xbap" },
            { ".xbm", "image/x-xbitmap" },
            { ".xdr", "video/x-amt-demorun" },
            { ".xht", "application/xhtml+xml" },
            { ".xhtml", "application/xhtml+xml" },
            { ".xif", "image/vnd.xiff" },
            { ".xla", "application/vnd.ms-excel" },
            { ".xlam", "application/vnd.ms-excel.addin.macroEnabled.12" },
            { ".xl", "application/excel" },
            { ".xlb", "application/excel" },
            { ".xlc", "application/excel" },
            { ".xld", "application/excel" },
            { ".xlk", "application/excel" },
            { ".xll", "application/excel" },
            { ".xlm", "application/excel" },
            { ".xls", "application/vnd.ms-excel" },
            { ".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12" },
            { ".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12" },
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { ".xlt", "application/vnd.ms-excel" },
            { ".xltm", "application/vnd.ms-excel.template.macroEnabled.12" },
            { ".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template" },
            { ".xlv", "application/excel" },
            { ".xlw", "application/excel" },
            { ".xm", "audio/xm" },
            { ".xml", "text/xml" },
            { ".xpi", "application/x-xpinstall" },
            { ".xpix", "application/x-vnd.ls-xpix" },
            { ".xpm", "image/xpm" },
            { ".xps", "application/vnd.ms-xpsdocument" },
            { ".x-png", "image/png" },
            { ".xsd", "text/xml" },
            { ".xsl", "text/xml" },
            { ".xslt", "text/xml" },
            { ".xsr", "video/x-amt-showrun" },
            { ".xwd", "image/x-xwd" },
            { ".z", "application/x-compressed" },
            { ".zip", "application/zip" },
            { ".zsh", "text/x-script.zsh" }
        };

        /// <summary>
        /// Gets the MIME type from the extension
        /// </summary>
        /// <param name="extension">An extension that starts with the dot</param>
        /// <returns>The MIME type for an extension</returns>
        /// <exception cref="KernelException"></exception>
        public static string GetMimeType(string extension)
        {
            // If the extension is nothing, assume that it's a binary file
            if (string.IsNullOrEmpty(extension))
                return mimeTypes[".bin"];

            // Extensions must start with a dot
            if (!extension.StartsWith("."))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_EXCEPTION_NEEDSADOT") + $" .{extension}");

            // Now, check to see if we have this extension
            if (mimeTypes.TryGetValue(extension, out string? mimeType))
                return mimeType;
            return mimeTypes[".bin"];
        }

        /// <summary>
        /// Gets the extended MIME type based on file data
        /// </summary>
        /// <param name="path">A file that contains either text or binary data</param>
        /// <returns>The MIME type for this file</returns>
        public static string GetExtendedMimeType(string path)
        {
            path = FilesystemTools.NeutralizePath(path);
            return MagicHandler.GetMagicMimeInfo(path);
        }

        /// <summary>
        /// Gets the file magic info based on file data
        /// </summary>
        /// <param name="path">A file that contains either text or binary data</param>
        /// <returns>Magic info for this file</returns>
        public static string GetMagicInfo(string path)
        {
            path = FilesystemTools.NeutralizePath(path);
            return MagicHandler.GetMagicInfo(path);
        }
    }
}
