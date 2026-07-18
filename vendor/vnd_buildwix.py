import os
import subprocess


def vnd_action(args):
    release = args[0] if args and args[0] else "Debug"
    architecture = args[1] if args and args[1] else "x64"
    solution = os.path.dirname(os.path.abspath(__file__ + '/../'))
    solution = solution + "/public/Nitrocid.Installers/Nitrocid.Installer.slnx"
    command = f"dotnet build \"{solution}\" -p:Configuration={release} -p:Platform={architecture}"
    result = subprocess.run(command, shell=True)
    if result.returncode != 0:
        raise Exception("WiX build failed with code %i" % (result.returncode))
