using KS.Misc.Reflection;
using Microsoft.VisualBasic.CompilerServices;
using NUnit.Framework;
using Shouldly;

namespace KSTests
{

    [TestFixture]
    public class FieldManagementTests
    {

        /// <summary>
    /// Tests checking field
    /// </summary>
        [Test]
        [Description("Management")]
        public void TestCheckField()
        {
            FieldManager.CheckField("HiddenFiles").ShouldBeTrue();
        }

        /// <summary>
    /// Tests getting value
    /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetValue()
        {
            string Value = Conversions.ToString(FieldManager.GetValue("HiddenFiles"));
            Value.ShouldNotBeNullOrEmpty();
        }

        /// <summary>
    /// Tests setting value
    /// </summary>
        [Test]
        [Description("Management")]
        public void TestSetValue()
        {
            FieldManager.SetValue("HiddenFiles", false);
            string Value = Conversions.ToString(FieldManager.GetValue("HiddenFiles"));
            Value.ShouldBe("False");
        }

        /// <summary>
    /// Tests getting variable
    /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetField()
        {
            var Field = FieldManager.GetField("HiddenFiles");
            Field.Name.ShouldBe("HiddenFiles");
        }

    }
}