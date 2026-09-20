import os
import json
import re
from pathlib import Path

vendor_prefix = 'NKS_'
id_finder = rf'"({vendor_prefix}[^"]+)"'
explicit_finder = rf'LanguageTools\.GetLocalized\s*\(\s*{id_finder}'
implicit_finder = rf'/\*\s*Localizable\s*\*/\s*{id_finder}'

def vnd_action(args):
    root_dir = os.path.dirname(os.path.abspath(__file__ + '/../'))
    project_files = [p.resolve() for p in
                    Path(root_dir).rglob('*')
                    if p.is_file() and p.name.endswith('.csproj')]
    for p in project_files:
        find_locs(root_dir, p)


def find_locs(root_dir: str, project_file: Path):
    project_dir = project_file.parent
    project_name = project_file.name
    project_languages = Path(f'{project_dir}/Resources/Languages')
    if not project_languages.exists():
        return
    
    source_codes = [p.resolve() for p in
                    Path(project_dir).rglob('*')
                    if p.is_file() and p.name.endswith('.cs')]
    loc_jsons = [p.resolve() for p in
                 project_languages.iterdir()
                 if p.is_file() and p.name.endswith('.json')]
    
    source_locs = get_locs_from_jsons(loc_jsons)
    locs_sources = get_loc_ids_from_sources(source_codes)
    locs_others = get_loc_ids_from_others(project_dir)
    target_locs = [loc for loc in locs_sources] + [loc for loc in locs_others]

    targets_dirty = check_for_existence_in_target(source_locs, target_locs)
    targets_dirty_count = len(targets_dirty)
    if (targets_dirty_count > 0):
        print(f'Please localize {targets_dirty_count} listed strings in '
              f'{project_name}.')
        with open(f"{root_dir}/dirty-t-{project_name}.txt", 'x') as file:
            for target_dirty in targets_dirty:
                file.write(target_dirty + "\n")
        
    sources_dirty = check_for_existence_in_source(source_locs, target_locs)
    sources_dirty_count = len(sources_dirty)
    if (sources_dirty_count > 0):
        print(f'Please remove {sources_dirty_count} listed strings in '
              f'{project_name}.')
        with open(f"{root_dir}/dirty-s-{project_name}.txt", 'x') as file:
            for source_dirty in sources_dirty:
                file.write(source_dirty + "\n")


def get_locs_from_jsons(loc_jsons: list[Path]):
    final_locs = []
    for loc_json in loc_jsons:
        with open(loc_json, 'r', encoding='utf-8-sig') as file:
            loc_json_data = json.load(file)
        loc_lang = loc_json_data['lang']
        loc_name = loc_json_data['name']
        loc_locs = [loc['loc'] for loc in loc_json_data['locs']]
        final_locs.append((loc_json.resolve(), loc_lang, loc_name, loc_locs))
    return final_locs


def get_loc_ids_from_sources(source_codes: list[Path]):
    final_ids = []
    for source_code in source_codes:
        lines = get_file_lines(source_code)
        found_locs = []
        line_count = 1
        for line in lines:
            explicit_matches = re.finditer(explicit_finder, line)
            implicit_matches = re.finditer(implicit_finder, line)
            for matches in explicit_matches, implicit_matches:
                for match in matches:
                    found_locs.append((line_count,
                                       match.start(), match.end(),
                                       match.group(1)))
            line_count += 1
        if len(found_locs) > 0:
            final_ids.append((source_code.resolve(), found_locs))
    return final_ids


def get_loc_ids_from_others(project_dir: Path):
    final_others = []
    resources = [p.resolve() for p in
                 Path(f"{project_dir}/Resources").rglob('*')
                 if p.is_file()
                 and p.name.endswith('.json')
                 and p.parent.name != 'Languages']
    for resource in resources:
        lines = get_file_lines(resource)
        found_locs = []
        line_count = 1
        for line in lines:
            matches = re.finditer(id_finder, line)
            for match in matches:
                found_locs.append((line_count,
                                   match.start(), match.end(),
                                   match.group(1)))
            line_count += 1
        if len(found_locs) > 0:
            final_others.append((resource.resolve(), found_locs))
    return final_others


def check_for_existence_in_target(source_locs: list, target_locs: list):
    dirty_locs = []
    for target_loc in target_locs:
        target_loc_path = target_loc[0]
        target_loc_tuples = target_loc[1]
        for loc_id in target_loc_tuples:
            target_loc_line = loc_id[0]
            target_loc_start = loc_id[1]
            target_loc_end = loc_id[2]
            target_loc_str = loc_id[3]
            for source_loc in source_locs:
                source_loc_lang = source_loc[1]
                source_loc_name = source_loc[2]
                source_loc_locs = source_loc[3]
                exists = target_loc_str in source_loc_locs
                if not exists:
                    if not target_loc_str in dirty_locs:
                        dirty_locs.append(target_loc_str)
                    print(f'WARNING: {target_loc_str} is unlocalized for '
                          f'{source_loc_lang} {source_loc_name} at line '
                          f'{target_loc_line} '
                          f'({target_loc_start}:{target_loc_end}) '
                          f'from source code file {target_loc_path}')
    return dirty_locs


def check_for_existence_in_source(source_locs: list, target_locs: list):
    dirty_locs = []
    for source_loc in source_locs:
        source_loc_path = source_loc[0]
        source_loc_lang = source_loc[1]
        source_loc_name = source_loc[2]
        source_loc_locs = source_loc[3]
        target_loc_ids = []
        for target_loc in target_locs:
            target_loc_tuples = target_loc[1]
            for target_loc_tuple in target_loc_tuples:
                target_loc_str = target_loc_tuple[3]
                target_loc_ids.append(target_loc_str)
        for source_loc_id in source_loc_locs:
            exists = source_loc_id in target_loc_ids
            if not exists:
                if not source_loc_id in dirty_locs:
                    dirty_locs.append(source_loc_id)
                print(f'WARNING: {source_loc_id} is an extra loc for '
                      f'{source_loc_lang} {source_loc_name} '
                      f'from source code file {source_loc_path}')
    return dirty_locs



def get_file_lines(file: Path):
    with open(file, 'r', encoding='utf-8') as f:
        return f.readlines()
