import os
import shutil


def vnd_clean(extra_args):
    print("Populating directories to clean...")
    solution = os.path.dirname(os.path.abspath(__file__ + '/../'))
    outputs = {"bin", "api", "artifacts", "obj", "tmp",
               "KSBuild", "KSAnalyzer", "KSTest",
               "nitrocid-25", "nitrocid-25-lite", "docs"}
    directories = get_dirs(solution)
    final_directories = [d.path for d in directories
                         if d.name in outputs]
    for d in final_directories:
        print("Cleaning directory %s..." % (d))
        shutil.rmtree(d, ignore_errors=True)


def get_dirs(directory):
    directories = [d for d in os.scandir(directory) if d.is_dir()]
    for d in list(directories):
        directories.extend(get_dirs(d.path))
    return directories
