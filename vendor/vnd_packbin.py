import os
import shutil
import zipfile


def vnd_packbin(extra_args):
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
    exec_dir = solution + '/public/Nitrocid/KSBuild/net10.0/'
    addons_dir = solution + '/public/Nitrocid/KSBuild/net10.0/Addons/'
    essentials_addons_dir = solution + '/public/Nitrocid/KSBuild/net10.0/' \
        'Addons.Essentials/'
    analyzers_dir = solution + '/public/Nitrocid/KSAnalyzer/netstandard2.0/'
    mod_analyzer_dir = solution + '/public/Nitrocid/KSAnalyzer/net10.0/'
    artifacts_dir = solution + '/artifacts'
    exec_zip_file = f'{version}-bin'
    exec_zip_path = artifacts_dir + '/' + exec_zip_file
    exec_lite_zip_path = artifacts_dir + '/' + exec_zip_file + '-lite.zip'
    addons_zip_file = f'{version}-addons'
    addons_zip_path = artifacts_dir + '/' + addons_zip_file
    analyzers_zip_file = f'{version}-analyzers'
    analyzers_zip_path = artifacts_dir + '/' + analyzers_zip_file
    mod_analyzer_zip_file = f'{version}-mod-analyzer'
    mod_analyzer_zip_path = artifacts_dir + '/' + mod_analyzer_zip_file

    # Generate the files
    zip_path = shutil.make_archive(exec_zip_path, 'zip', exec_dir)
    print(f"Written to {zip_path}")
    zip_path = shutil.make_archive(addons_zip_path, 'zip', addons_dir)
    zip_path = shutil.make_archive(analyzers_zip_path, 'zip', analyzers_dir)
    print(f"Written to {zip_path}")
    zip_path = shutil.make_archive(mod_analyzer_zip_path, 'zip', mod_analyzer_dir)
    print(f"Written to {zip_path}")

    # Make a separate lite executable zip file
    with zipfile.ZipFile(exec_zip_path + '.zip', 'r') as exec_zip:
        # Get a list of ZIP file entries
        exec_zip_infolist = exec_zip.infolist()
        with zipfile.ZipFile(exec_lite_zip_path, 'w') as exec_lite_zip:
            for zip_info in exec_zip_infolist:
                # Check to see if we are dealing with an addon
                orig_fn = zip_info.orig_filename
                is_addon = orig_fn.startswith('Addons/') or \
                           orig_fn.startswith('Addons.Essentials/')
                
                # If not an addon, then write to the lite zip file
                if not is_addon:
                    exec_lite_zip.writestr(zip_info,
                                           exec_zip.read(zip_info.filename))
    print(f"Written to {exec_lite_zip_path}")

    # Add the essential addons to the zip file
    with zipfile.ZipFile(addons_zip_path + '.zip', 'a') as addons_zip:
        for walk_tuple in os.walk(essentials_addons_dir):
            # Get the files
            root = walk_tuple[0]
            files = walk_tuple[2]
            for file in files:
                # Find the essential addons files and write them
                file_path = os.path.join(root, file)
                arcname = os.path.relpath(file_path, essentials_addons_dir)
                addons_zip.write(file_path, arcname)
    print(f"Written to {addons_zip_path}.zip")

    # Copy the changes.chg file
    source_changes_chg = solution + '/changes.chg'
    target_changes_chg = solution + f'/vendor/{version}-changes.chg'
    shutil.copy(source_changes_chg, target_changes_chg)
    print(f"Written to {target_changes_chg}")