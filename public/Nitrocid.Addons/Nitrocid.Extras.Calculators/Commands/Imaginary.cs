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

using Terminaux.Colors.Themes.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using System;

namespace Nitrocid.Extras.Calculators.Commands
{
    /// <summary>
    /// This command will show information about the imaginary number formula specified by a specified real and imaginary number.
    /// </summary>
    /// <remarks>
    /// This command can be used to get information about the imaginary number formula specified by a specified real and imaginary number. It shows the formula in
    /// the following format:<code>Z = Re + Im i</code>
    /// </remarks>
    class ImaginaryCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check both the real and the imaginary numbers for verification
            if (!double.TryParse(parameters.ArgumentsList[0], out double Real))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_CALCULATORS_REALINVALID"), true, ThemeColorType.Error);
                return 2;
            }
            if (!double.TryParse(parameters.ArgumentsList[1], out double Imaginary))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_CALCULATORS_IMAGINARYINVALID"), true, ThemeColorType.Error);
                return 2;
            }

            // Print the Z formula, first of all.
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_CALCULATORS_FORMULA") + " Z = {0} + {1}i", Real, Imaginary);

            // Process the radius
            double Radius = Math.Sqrt(Math.Pow(Real, 2) + Math.Pow(Imaginary, 2));
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_CALCULATORS_RADIUS") + " {0}", Radius);

            // Determine the cosine (Real to Radius) and the sine (Imaginary to Radius)
            double RealRadius = Math.Cos(Real / Radius);
            double ImaginaryRadius = Math.Sin(Imaginary / Radius);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_CALCULATORS_REALTORAD") + " {0} rad", RealRadius);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_CALCULATORS_IMAGTORAD") + " {0} rad", ImaginaryRadius);

            // Try to find out the angle from the given cos/sin values
            double AngleCos = Math.Acos(RealRadius);
            double AngleSin = Math.Asin(ImaginaryRadius);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_CALCULATORS_ANGLE") + " {0} rad, {1} rad", AngleCos, AngleSin);

            // Now, write the result in both the exponentional format (Z = r * (e)^{angle}i)
            //                           and the triangular format    (Z = r (cos {angle} + i sin {angle})
            TextWriters.Write(LanguageTools.GetLocalized("NKS_CALCULATORS_EXPONENTIAL") + " Z = {0} * (e)^{1}i", true, ThemeColorType.Success, Radius, AngleSin);
            TextWriters.Write(LanguageTools.GetLocalized("NKS_CALCULATORS_TRIANGULAR") + " Z = {0} * (cos ({1}) + i sin ({2}))", true, ThemeColorType.Success, Radius, AngleCos, AngleSin);
            return 0;
        }
    }
}
