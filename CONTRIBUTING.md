## Important warnings

On 2020, GitHub renamed the default main branch from 'master' to 'main' because of growing social unrest, the word being offensive to some people, and racism regarding the use of the words 'master' and 'slave', so we renamed the default branch to 'main'.

To update your environment so that it uses the new branch name, run the below commands on the root directory of the project:

```sh
git branch -m master main
git fetch origin
git branch -u origin/main main
git remote set-head origin -a
```

For mroe information, visit [this article](https://sfconservancy.org/news/2020/jun/23/gitbranchname/).

## General Contributions

This document needs to be re-written. TODO.
