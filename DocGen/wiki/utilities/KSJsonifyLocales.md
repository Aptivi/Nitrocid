## KSJsonifyLocales

KSJsonifyLocales is the new tool that can convert the language files consisting of translations of strings to a readable JSON format. It is to ensure that there are no misses in lines of one of the languages.

### How it works

#### The normal flow

KSJsonifyLocales contains the Translations folder that has the translation text files inside. When this program is run, it reads the entire folder, parses all the files, and converts them to the JSON output, which can be found in Translations/Done folder in the same place as the executable.

If run with `--CopyToResources`, it will copy the results directly to Nitrocid KS's resources, assuming that it's run on KSBuild found inside the source code folder. This usually works with git clones of KS.

To illustrate this, KSJsonifyLocales will:

- read the Translations folder,
- parse each language file,
- add them to the list of language files,
- read both the English and the target language lines,
- save them to JSON, and
- save them to Translations/Done or (optionally) ../../Resources, assuming that the executable is found one level above the Nitrocid KS source folder

#### What else can it do?

These switches can change the way how KSJsonifyLocales runs. In ordrer to use any of the switches, you must provide one of the first three switches so that the language parsing can be modified using the switches other than them. Refer to the table below for description:

| Switch              | Description
|:--------------------|:------------
| `--CustomOnly`      | Parses the custom languages only.
| `--NormalOnly`      | Parses the normal languages only.
| `--All`             | Parses all the languages.
| `--Singular`        | Parses a single language only.
| `--CopyToResources` | Copies the result to the resources folder of Nitrocid KS. Requires a copy of source code.

### Custom languages

The CustomLanguages folder is for the custom languages which change the language of the messages found in Nitrocid KS. This folder can be used to store the text files of your custom languages, but you have to place the metadata information for each custom language to be read by the kernel as it starts up. It can be accessed in the same location as the Nitrocid KS executable.

This metadata information should be stored in a file called Metadata.json in the format below to be jsonified by this tool. It should not be removed.
```json
    "lng": {
        "name": "Language",
        "transliterable": false / true
    }
```
 
Example:
 
```json
    "afr": {
        "name": "Afrikaans",
        "transliterable": false
    }
```
 
The outputs for the custom languages can be found in the KSLanguages folder in your system profile directory (`$HOME` or `%USERPROFILE%`) stored as lng.json. For example, afr.json.
 
If there's a transliterable version of a language that you're going to add, you must implement two versions of the language:
 
- The translated version (lng-T, for e.g. arb-T)
- The transliterated version (lng, for e.g. arb)

> [!WARNING]
> The custom language shouldn't be under the same three-letter language name; it should be unique. If you're creating a fixed version of the existing language, you should append a number after it. For example, "afr-1", not "afr".

### How to use

- On Windows, you can just double-click on the KSJsonifyLocales.exe file. It's usually found on the same directory as Nitrocid KS.
- On Linux, you can run `mono KSJsonifyLocales.exe`
