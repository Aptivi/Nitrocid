Custom languages
================

This folder is for the custom languages which change the language of the
messages found in Kernel Simulator. This folder can be used to store the text
files of your custom languages, but you have to place the metadata
information for each custom language to be read by the kernel as it starts up.
This metadata information should be stored in a file called Metadata.json in
the format below to be jsonified by this tool. It should not be removed.

    "lng": {
        "name": "Language",
        "transliterable": false / true
    }

Example:

    "afr": {
        "name": "Afrikaans",
        "transliterable": false
    }

The outputs for the custom languages can be found in the KSLanguages folder in
your system profile directory ($HOME or %USERPROFILE%) stored as lng.json. For
example, afr.json.

WARNING: The custom language shouldn't be under the same three-letter language
         name; it should be unique. If you're creating a fixed version of the
         existing language, you should append a number after it. For example,
         "afr-1", not "afr".

If there's a transliterable version of a language that you're going to add,
you must implement two versions of the language:

  - The translated version     (lng-T, for e.g. arb-T)
  - The transliterated version (lng,   for e.g. arb)
