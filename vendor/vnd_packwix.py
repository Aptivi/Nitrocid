import os
import shutil


def vnd_action(args):
    solution = os.path.dirname(os.path.abspath(__file__ + '/../'))
    release = args[0] if args and args[0] else "Debug"
    architecture = args[1] if args and args[1] else "x64"

    # Create the artifacts directory if there isn't one
    artifacts_dir = solution + '/artifacts'
    if not os.path.isdir(artifacts_dir):
        os.mkdir(artifacts_dir)

    # Copy the exe file
    source_installer_file = solution + f'/public/Nitrocid.Installers/Nitrocid.InstallerBundle/bin/{architecture}/{release}/Nitrocid.InstallerBundle.exe'
    target_installer_file = solution + f'/artifacts/nitrocid-win-{architecture.lower()}-installer.exe'
    shutil.move(source_installer_file, target_installer_file)
    print(f"Moved to {target_installer_file}")
