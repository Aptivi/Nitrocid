import os
import argparse
import subprocess


def vnd_pushbin(extra_args):
    solution = os.path.dirname(os.path.abspath(__file__ + '/../'))

    # Get NuGet binary files
    binary_files = get_files(solution)
    binary_files = [f.path for f in binary_files
                   if '.nupkg' in f.name and f.is_file()]
    print(f'{len(binary_files)} NuGet binaries to push')

    # Get extra arguments to parse for API key and source
    parser = argparse.ArgumentParser(prog='adt pushbin',
                                     add_help=False)
    parser.add_argument('-a', '--api_key')
    parser.add_argument('-s', '--source', default='NuGet.org')
    parsed = parser.parse_args(extra_args)

    # Push package one by one
    api_key = parsed.api_key
    source = parsed.source
    for binary_file in binary_files:
        print(f'\nPushing NuGet package: {binary_file}')
        command = f'dotnet nuget push {binary_file} ' + \
                  f'--api-key "{api_key}" ' + \
                  f'--source "{source}" ' + \
                  '--skip-duplicate'
        result = subprocess.run(command, shell=True)
        if result.returncode != 0:
            raise Exception("Push failed with code %i" % (result.returncode))
    

def get_files(directory):
    files = []
    for f in os.scandir(directory):
        if f.is_file():
            files.append(f)
        else:
            files.extend(get_files(f.path))
    return files
