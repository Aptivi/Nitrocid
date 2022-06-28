## chlang command

### Summary

Changes system language

### Description

The system language can be changed either by manually editing configuration files or by using this command. Restart is not required, since printing text, viewing user manual, and updating help list relies on the current language field.

| Switches | Description
|:----------------------|:------------
| -alwaystransliterated | Always use the transliterated version of the language. Must be transliterable.
| -alwaystranslated     | Always use the translated version of the language. Must be transliterable.
| -force                | Forces the language to be set.
| -list                 | Lists the installed languages.

### Command usage

* `chlang [-alwaystransliterated|-alwaystranslated|-force|-list] <language>`

### Examples

* `chlang fre`: Changes system language to French