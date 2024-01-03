//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using NUnit.Framework;
using Shouldly;

namespace Nitrocid.Tests.Files
{
    [TestFixture]
    public class FilesystemExtensionTests
    {

        /// <summary>
        /// Tests registering a custom file handler
        /// </summary>
        [OneTimeSetUp]
        [Description("Extension")]
        public void TestRegisterHandler()
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
        [TestCase(".ext", false, true)]
        [TestCase(".bin", true, true)]
        [TestCase(".invalidformat", false, false)]
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
        [Test]
        [TestCase(".ext", true)]
        [TestCase(".bin", true)]
        [TestCase(".invalidformat", false)]
        [Description("Extension")]
        public void TestIsHandlerRegistered(string extension, bool expectedRegged)
        {
            bool regged = ExtensionHandlerTools.IsHandlerRegistered(extension);
            regged.ShouldBe(expectedRegged);
        }

        /// <summary>
        /// Tests checking to see if the handler is built-in
        /// </summary>
        [Test]
        [TestCase(".ext", false)]
        [TestCase(".bin", true)]
        [TestCase(".invalidformat", false)]
        [Description("Extension")]
        public void TestIsHandlerBuiltin(string extension, bool expectedBuiltin)
        {
            bool builtin = ExtensionHandlerTools.IsHandlerBuiltin(extension);
            builtin.ShouldBe(expectedBuiltin);
        }

        /// <summary>
        /// Tests unregistering a custom file handler
        /// </summary>
        [OneTimeTearDown]
        [Description("Extension")]
        public void TestUnregisterHandler()
        {
            var handler = ExtensionHandlerTools.GetFirstExtensionHandler(".ext");
            ExtensionHandlerTools.UnregisterHandler(".ext", handler);
            ExtensionHandlerTools.customHandlers.ShouldBeEmpty();
        }

    }
}
