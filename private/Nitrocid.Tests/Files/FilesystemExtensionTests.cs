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

using Nitrocid.Files.Extensions;
using Nitrocid.Kernel.Debugging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Nitrocid.Tests.Files
{
    [TestClass]
    public class FilesystemExtensionTests
    {

        /// <summary>
        /// Tests registering a custom file handler
        /// </summary>
        [ClassInitialize]
#pragma warning disable IDE0060
        public static void RegisterUnitHandler(TestContext tc)
#pragma warning restore IDE0060
        {
            ExtensionHandlerTools.RegisterHandler(".ext", "ext", (path) =>
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Custom .ext handler");
                DebugWriter.WriteDebug(DebugLevel.I, path);
            }, (path) => "Just a test info");
            ExtensionHandlerTools.customHandlers.ShouldNotBeEmpty();
            ExtensionHandlerTools.customHandlers.Count.ShouldBe(1);
        }

        /// <summary>
        /// Tests getting an extension handler
        /// </summary>
        [TestMethod]
        [Description("Extension")]
        public void TestGetFirstExtensionHandler()
        {
            var handler = ExtensionHandlerTools.GetFirstExtensionHandler(".bin");
            handler.ShouldNotBeNull();
            handler.Extension.ShouldBe(".bin");
            handler.MimeType.ShouldBe("application/octet-stream");
            handler.Implementer.ShouldBe("NitrocidBin");
        }

        /// <summary>
        /// Tests getting an extension handler
        /// </summary>
        [TestMethod]
        [Description("Extension")]
        public void TestGetExtensionHandler()
        {
            var handler = ExtensionHandlerTools.GetExtensionHandler(".bin", "NitrocidBin");
            handler.ShouldNotBeNull();
            handler.Extension.ShouldBe(".bin");
            handler.MimeType.ShouldBe("application/octet-stream");
            handler.Implementer.ShouldBe("NitrocidBin");
        }

        /// <summary>
        /// Tests getting extension handlers
        /// </summary>
        [TestMethod]
        [Description("Extension")]
        public void TestGetExtensionHandlers()
        {
            var handlers = ExtensionHandlerTools.GetExtensionHandlers(".ext");
            handlers.ShouldNotBeNull();
            handlers.ShouldNotBeEmpty();
            foreach (var handler in handlers)
            {
                handler.Extension.ShouldBe(".ext");
                handler.MimeType.ShouldBe("application/octet-stream");
                handler.Implementer.ShouldBe("ext");
            }
        }

        /// <summary>
        /// Tests checking to see if the handler is built-in and registered
        /// </summary>
        [TestMethod]
        [DataRow(".ext", false, true)]
        [DataRow(".bin", true, true)]
        [DataRow(".invalidformat", false, false)]
        [Description("Extension")]
        public void TestIsHandlerBuiltinAndRegistered(string extension, bool expectedBuiltin, bool expectedRegged)
        {
            bool builtin = ExtensionHandlerTools.IsHandlerBuiltin(extension);
            bool regged = ExtensionHandlerTools.IsHandlerRegistered(extension);
            builtin.ShouldBe(expectedBuiltin);
            regged.ShouldBe(expectedRegged);
        }

        /// <summary>
        /// Tests checking to see if the handler is registered
        /// </summary>
        [TestMethod]
        [DataRow(".ext", true)]
        [DataRow(".bin", true)]
        [DataRow(".invalidformat", false)]
        [Description("Extension")]
        public void TestIsHandlerRegistered(string extension, bool expectedRegged)
        {
            bool regged = ExtensionHandlerTools.IsHandlerRegistered(extension);
            regged.ShouldBe(expectedRegged);
        }

        /// <summary>
        /// Tests checking to see if the handler is built-in
        /// </summary>
        [TestMethod]
        [DataRow(".ext", false)]
        [DataRow(".bin", true)]
        [DataRow(".invalidformat", false)]
        [Description("Extension")]
        public void TestIsHandlerBuiltin(string extension, bool expectedBuiltin)
        {
            bool builtin = ExtensionHandlerTools.IsHandlerBuiltin(extension);
            builtin.ShouldBe(expectedBuiltin);
        }

        /// <summary>
        /// Tests unregistering a custom file handler
        /// </summary>
        [ClassCleanup]
        public static void UnregisterUnitHandler()
        {
            var handler = ExtensionHandlerTools.GetFirstExtensionHandler(".ext");
            ExtensionHandlerTools.UnregisterHandler(".ext", handler);
            ExtensionHandlerTools.customHandlers.ShouldBeEmpty();
        }

    }
}
