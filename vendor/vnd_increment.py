import os
import datetime


class VersionInfo():
    def __init__(self, old_version, new_version, api_versions):
        # Nitrocid version
        self.old_version = old_version
        self.new_version = new_version
        self.old_version_split = self.old_version.split('.')
        self.new_version_split = self.new_version.split('.')
        self.old_major = \
            f'{self.old_version_split[0]}.' \
            f'{self.old_version_split[1]}.' \
            f'{self.old_version_split[2]}'
        self.new_major = \
            f'{self.new_version_split[0]}.' \
            f'{self.new_version_split[1]}.' \
            f'{self.new_version_split[2]}'

        # Nitrocid mod API version
        self.old_api_version = api_versions[0]
        self.new_api_version = api_versions[1]
        self.old_api_version_split = self.old_api_version.split('.')
        self.new_api_version_split = self.new_api_version.split('.')
        self.old_api_version_tmpl = self.old_api_version.replace('.', ', ')
        self.new_api_version_tmpl = self.new_api_version.replace('.', ', ')
        self.old_api_package = self.old_api_version_split[2]
        self.old_api_major = \
            f'{self.old_api_version_split[0]}.' \
            f'{self.old_api_version_split[1]}.' \
            f'{self.old_api_package}'
        self.old_api_changeset = self.old_api_version_split[3]
        self.new_api_package = self.new_api_version_split[2]
        self.new_api_major = \
            f'{self.new_api_version_split[0]}.' \
            f'{self.new_api_version_split[1]}.' \
            f'{self.new_api_package}'
        self.new_api_changeset = self.new_api_version_split[3]


def vnd_increment(old_version, new_version, api_versions):
    time = datetime.datetime.now()
    solution = os.path.dirname(os.path.abspath(__file__ + '/../'))

    # Get old and new API versions and split them
    version = VersionInfo(old_version, new_version, api_versions)

    # Replace the versions in the Directory.Build.props file
    props_file = f"{solution}/Directory.Build.props"
    props_line_idx = 0
    props_version_line_idx = 0
    props_api_major_line_idx = 0
    props_api_changeset_line_idx = 0
    props_lines = []
    with open(props_file) as props_stream:
        props_lines = props_stream.readlines()
        for line in props_lines:
            props_line_idx += 1
            if '<Version>' in line and \
               '</Version>' in line:
                props_version_line_idx = props_line_idx
            elif '<NitrocidModAPIVersionMajor>' in line and \
                 '</NitrocidModAPIVersionMajor>' in line:
                props_api_major_line_idx = props_line_idx
            elif '<NitrocidModAPIVersionChangeset>' in line and \
                 '</NitrocidModAPIVersionChangeset>' in line:
                props_api_changeset_line_idx = props_line_idx
    props_lines[props_version_line_idx - 1] = \
        props_lines[props_version_line_idx - 1] \
        .replace(old_version,
                 new_version)
    props_lines[props_api_major_line_idx - 1] = \
        props_lines[props_api_major_line_idx - 1] \
        .replace(version.old_api_major,
                 version.new_api_major)
    props_lines[props_api_changeset_line_idx - 1] = \
        props_lines[props_api_changeset_line_idx - 1] \
        .replace(version.old_api_changeset,
                 version.new_api_changeset)
    with open(props_file, "w") as props_stream:
        props_stream.writelines(props_lines)

    # Replace the version in the CHANGES.TITLE file
    changes_title_file = f"{solution}/CHANGES.TITLE"
    changes_title_lines = []
    with open(changes_title_file) as changes_title_stream:
        changes_title_lines = changes_title_stream.readlines()
    changes_title_lines[0] = changes_title_lines[0].replace(old_version,
                                                            new_version)
    with open(changes_title_file, 'w') as changes_title_stream:
        changes_title_stream.writelines(changes_title_lines)

    # Replace the version in the changes.chg file
    changes_chg_file = f"{solution}/changes.chg"
    changes_chg_lines = []
    with open(changes_chg_file) as changes_chg_stream:
        changes_chg_lines = changes_chg_stream.readlines()
    changes_chg_lines[3] = \
        changes_chg_lines[3].replace(old_version,
                                     new_version)
    changes_chg_lines[3] = \
        changes_chg_lines[3].replace(version.old_api_version,
                                     version.new_api_version)
    with open(changes_chg_file, 'w') as changes_chg_stream:
        changes_chg_stream.writelines(changes_chg_lines)
    
    # Replace the versions in the Arch Linux packaging files
    arch_rel_file = f"{solution}/PKGBUILD-REL"
    arch_rel_lite_file = f"{solution}/PKGBUILD-REL-LITE"
    arch_rel_lines = []
    arch_rel_lite_lines = []
    with open(arch_rel_file) as arch_rel_stream:
        arch_rel_lines = arch_rel_stream.readlines()
    with open(arch_rel_lite_file) as arch_rel_lite_stream:
        arch_rel_lite_lines = arch_rel_lite_stream.readlines()
    arch_rel_lines = process_arch_lines(arch_rel_lines, version)
    arch_rel_lite_lines = process_arch_lines(arch_rel_lite_lines, version)
    with open(arch_rel_file, 'w') as arch_rel_stream:
        arch_rel_stream.writelines(arch_rel_lines)
    with open(arch_rel_lite_file, 'w') as arch_rel_lite_stream:
        arch_rel_lite_stream.writelines(arch_rel_lite_lines)
    
    # Replace the versions in the GitHub Actions workflows
    workflows = {
        f"{solution}/.github/workflows/build-ppa-package-with-lintian.yml",
        f"{solution}/.github/workflows/build-ppa-package.yml",
        f"{solution}/.github/workflows/pushamend.yml",
        f"{solution}/.github/workflows/pushppa.yml",
    }
    for workflow in workflows:
        workflow_lines = []
        with open(workflow) as workflow_stream:
            workflow_lines = workflow_stream.readlines()
        workflow_lines = process_misc_lines(workflow_lines, version)
        with open(workflow, 'w') as workflow_stream:
            workflow_stream.writelines(workflow_lines)
    
    # Replace the versions in the WiX installer files
    installer_wxs = {
        f"{solution}/public/Nitrocid.Installers/Nitrocid.Installer/Package.wxs",
        f"{solution}/public/Nitrocid.Installers/Nitrocid.InstallerBundle/Bundle.wxs",
    }
    for wxs in installer_wxs:
        wxs_lines = []
        with open(wxs) as wxs_stream:
            wxs_lines = wxs_stream.readlines()
        wxs_lines = process_wxs_lines(wxs_lines, version)
        with open(wxs, 'w') as wxs_stream:
            wxs_stream.writelines(wxs_lines)
    
    # Replace the versions in the template files
    template_files = {
        f"{solution}/public/Nitrocid.Templates/templates/KSMod/KSMod.csproj",
        f"{solution}/public/Nitrocid.Templates/templates/KSMod/ModClass.cs",
        f"{solution}/public/Nitrocid.Templates/templates/KSModVB/KSModVB.vbproj",
        f"{solution}/public/Nitrocid.Templates/templates/KSModVB/ModClass.vb",
    }
    for tmpl in template_files:
        tmpl_lines = []
        with open(tmpl) as tmpl_stream:
            tmpl_lines = tmpl_stream.readlines()
        tmpl_lines = process_tmpl_lines(tmpl_lines, version)
        with open(tmpl, 'w') as tmpl_stream:
            tmpl_stream.writelines(tmpl_lines)
    
    # Add a Debian changelog entry
    debian_changes_file = f"{solution}/debian/changelog"
    debian_changes_time = time.strftime("%a, %d %b %Y %H:%M:%S") + ' ' + \
        time.now(datetime.timezone.utc).astimezone().strftime("%z")
    debian_changes_entry = f"""nitrocid-{version.new_api_package} ({version.new_api_version}-{version.new_version}-1) noble; urgency=medium

  * Please populate changelogs here

 -- Aptivi CEO <ceo@aptivi.anonaddy.com>  {debian_changes_time}
"""
    with open(debian_changes_file, 'r') as debian_changes_stream:
        debian_changes_entry = debian_changes_entry + "\n" + \
            debian_changes_stream.read()
    with open(debian_changes_file, 'w') as debian_changes_stream:
        debian_changes_stream.write(debian_changes_entry)


def process_arch_lines(arch_lines, version: VersionInfo):
    for num in range(0, len(arch_lines)):
        is_pkgname = 'pkgname=' in arch_lines[num]
        is_pkgver = 'pkgver=' in arch_lines[num]
        is_source = 'source=' in arch_lines[num]
        is_branch = 'branch=' in arch_lines[num]
        if is_pkgver or is_source:
            arch_lines[num] = \
                arch_lines[num].replace(version.old_version,
                                        version.new_version)
            if is_branch:
                arch_lines[num] = \
                    arch_lines[num].replace(version.old_major,
                                            version.new_major)
        if is_pkgver:
            arch_lines[num] = \
                arch_lines[num].replace(version.old_api_version,
                                        version.new_api_version)
        if is_pkgname:
            nitrocid_old_api_pkg = f'nitrocid-{version.old_api_package}'
            nitrocid_new_api_pkg = f'nitrocid-{version.new_api_package}'
            arch_lines[num] = \
                arch_lines[num].replace(nitrocid_old_api_pkg,
                                        nitrocid_new_api_pkg)
    return arch_lines


def process_tmpl_lines(lines, version: VersionInfo):
    for num in range(0, len(lines)):
        is_ver = f'Version="{version.old_version}"' in lines[num]
        is_cs_ver = f'new({version.old_api_version_tmpl})' in lines[num]
        is_vb_ver = f'New Version({version.old_api_version_tmpl})' in lines[num]
        if is_ver:
            lines[num] = \
                lines[num].replace(version.old_version,
                                   version.new_version)
        if is_cs_ver or is_vb_ver:
            lines[num] = \
                lines[num].replace(version.old_api_version_tmpl,
                                   version.new_api_version_tmpl)
    return lines


def process_wxs_lines(lines, version: VersionInfo):
    for num in range(0, len(lines)):
        is_ver = f'Version="{version.old_version}"' in lines[num]
        is_name = f'Name="Nitrocid {version.old_major}"' in lines[num]
        if is_ver:
            lines[num] = \
                lines[num].replace(version.old_version,
                                   version.new_version)
        if is_name:
            lines[num] = \
                lines[num].replace(version.old_major,
                                   version.new_major)
    return lines


def process_misc_lines(lines, version: VersionInfo):
    nitrocid_old_api_pkg = f'nitrocid-{version.old_api_package}'
    nitrocid_new_api_pkg = f'nitrocid-{version.new_api_package}'
    for num in range(0, len(lines)):
        is_oldver = version.old_version in lines[num]
        is_oldapiver = version.old_api_version in lines[num]
        is_package = nitrocid_old_api_pkg in lines[num]
        if is_oldver:
            lines[num] = \
                lines[num].replace(version.old_version,
                                   version.new_version)
        if is_oldapiver:
            lines[num] = \
                lines[num].replace(version.old_api_version,
                                   version.new_api_version)
        if is_package:
            lines[num] = \
                lines[num].replace(nitrocid_old_api_pkg,
                                   nitrocid_new_api_pkg)
    return lines
