import os
import subprocess


def vnd_test(args, extra_args):
    solution = os.path.dirname(os.path.abspath(__file__ + '/../'))
    solution = solution + "/Nitrocid.slnx"
    command = f"dotnet test \"{solution}\" {args if args else ''}"
    result = subprocess.run(command, shell=True)
    if result.returncode != 0:
        raise Exception("Test failed with code %i" % (result.returncode))
