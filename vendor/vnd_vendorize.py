import os
import shutil
import subprocess


def vnd_vendorize(extra_args):
    solution = os.path.dirname(os.path.abspath(__file__ + '/../'))

    # Restore NuGet packages
    home_dir = os.environ[('USERPROFILE' if os.name == 'nt' else 'HOME')]
    deps_dir = solution + '/deps/'
    nuget_packages_dir = solution + '/nuget'
    extra_packages_dir = home_dir + '/.nuget/packages'
    print(f'Restoring NuGet packages to {nuget_packages_dir}...')
    restore_command = f'dotnet restore "{solution}/Nitrocid.slnx" ' +\
                      f'--packages "{nuget_packages_dir}"'
    result = subprocess.run(restore_command, shell=True)
    if result.returncode != 0:
        raise Exception("NuGet restore failed: %i" % (result.returncode))

    # Make a dependencies directory
    if not os.path.isdir(deps_dir):
        os.mkdir(deps_dir)

    # Get NuGet package files from the packages directory
    copy_nupkgs(nuget_packages_dir, deps_dir)
    shutil.rmtree(nuget_packages_dir)

    # Get NuGet package files from the extra packages directory
    copy_nupkgs(extra_packages_dir, deps_dir)

    # Initialize the NuGet configuration
    nuget_conf_path = solution + '/NuGet.config'
    nuget_conf_contents = """<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="nuget.org" value="./deps" />
  </packageSources>
</configuration>
"""
    with open(nuget_conf_path, 'w') as nuget_conf_file:
        nuget_conf_file.write(nuget_conf_contents)
    print('Prepared NuGet sources for offline use.')
    

def copy_nupkgs(nuget_packages_dir, deps_dir):
    binary_files = get_files(nuget_packages_dir)
    binary_files = [f.path for f in binary_files
                   if '.nupkg' in f.name and f.is_file()]
    print(f'{len(binary_files)} NuGet packages in {nuget_packages_dir}')
    for binary_file in binary_files:
        shutil.copy(binary_file, deps_dir)
    print(f'Copied {len(binary_files)} NuGet packages to {deps_dir}')


def get_files(directory):
    files = []
    for f in os.scandir(directory):
        if f.is_file():
            files.append(f)
        else:
            files.extend(get_files(f.path))
    return files
