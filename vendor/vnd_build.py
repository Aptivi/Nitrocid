import os
import subprocess


def vnd_build(args, extra_args):
    solution = os.path.dirname(os.path.abspath(__file__ + '/../'))
    solution = solution + "/Nitrocid.slnx"
    command = f"dotnet build {solution} {args if args else ''}"
    result = subprocess.run(command, shell=True)
    if result.returncode != 0:
        raise Exception("Build failed with code %i" % (result.returncode))
