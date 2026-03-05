import os
import subprocess


def vnd_gendocs(extra_args):
    solution = os.path.dirname(os.path.abspath(__file__ + '/../'))
    command = f"docfx {solution}/DocGen/docfx.json"
    result = subprocess.run(command, shell=True)
    if result.returncode != 0:
        raise Exception("Generation failed with code %i" % (result.returncode))
