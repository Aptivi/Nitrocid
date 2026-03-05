import os
import shutil


def vnd_packdocs(extra_args):
    solution = os.path.dirname(os.path.abspath(__file__ + '/../'))

    # Get the project version
    build_props = solution + '/Directory.Build.props'
    version = ""
    with open(build_props) as props_file:
        lines = props_file.readlines()
        version_found = False
        for line in lines:
            if '<Version>' in line and '</Version>' in line:
                version_found = True
                substr_idx = line.index('<') + 9
                substr_idx_end = line[substr_idx:].index('<') + substr_idx
                version = line[substr_idx:substr_idx_end]
                break
        if not version_found:
            raise Exception("Version not found in Directory.Build.props.")
    
    # Make a zip archive file path
    docs_dir = solution + '/docs/'
    artifacts_dir = solution + '/artifacts'
    zip_archive_file = f'{version}-doc'
    zip_archive_path = artifacts_dir + '/' + zip_archive_file

    # Generate the file
    zip_path = shutil.make_archive(zip_archive_path, 'zip', docs_dir)
    print(f"Written to {zip_path}")