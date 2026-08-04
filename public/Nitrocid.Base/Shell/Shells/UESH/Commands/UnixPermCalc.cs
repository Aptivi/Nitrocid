//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Text;
using Nitrocid.Base.Files.Unix;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Unix permissions calculator
    /// </summary>
    class UnixPermCalcCommand : BaseCommand, ICommand
    {

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string mode = parameters.ArgumentsList[0];
            switch (mode)
            {
                case "tonum":
                    // Get the specifiers
                    string userSpecifier = parameters.ArgumentsList[1];
                    string groupSpecifier = parameters.ArgumentsList[2];
                    string otherSpecifier = parameters.ArgumentsList[3];

                    // Get the types from the specifiers
                    UnixPermissionType userType = UnixPermissionManager.GetTypeFrom(userSpecifier);
                    UnixPermissionType groupType = UnixPermissionManager.GetTypeFrom(groupSpecifier);
                    UnixPermissionType otherType = UnixPermissionManager.GetTypeFrom(otherSpecifier);

                    // Convert them to numbers, and make a compound permissions number
                    int userTypeNum = UnixPermissionManager.Calculate(userType) * 100;
                    int groupTypeNum = UnixPermissionManager.Calculate(groupType) * 10;
                    int otherTypeNum = UnixPermissionManager.Calculate(otherType);
                    int permissions = userTypeNum + groupTypeNum + otherTypeNum;
                    
                    // Print it
                    variableValue = $"{permissions}";
                    TextWriterColor.Write(variableValue);
                    break;
                case "torep":
                    // Parse the permissions number and get the descriptors
                    int chmodNum = int.Parse(parameters.ArgumentsList[1]);
                    var descriptors = UnixPermissionManager.GetDescriptors(chmodNum);

                    // Build the representation string
                    StringBuilder representationBuilder = new();
                    for (int i = 0; i < descriptors.Length; i++)
                    {
                        UnixPermissionDescriptor descriptor = descriptors[i];
                        UnixPermissionType types = descriptor.Types;

                        // Process all permission types
                        representationBuilder.Append(UnixPermissionManager.BuildPermissionRepresentation(types));
                        
                        // Add a space if desired
                        if (i < descriptors.Length - 1)
                            representationBuilder.Append(' ');
                    }

                    // Print it
                    variableValue = $"{representationBuilder}";
                    TextWriterColor.Write(variableValue);
                    break;
            }
            return 0;
        }

    }
}
