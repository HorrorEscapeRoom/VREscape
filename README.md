# Resources
- Learn Git Branching ([https://learngitbranching.js.org/])
- Head First Git Raju Gandhi

# Commands

## Commit 
```sh
git commit -m "<message>"
```

## Configure Git
```sh
git config --global user.name <name>
git config --global user.email <email>
```

## Clone repository
```sh
git clone https://github.com/<username>/<repository>
```

## Merge fetched remote’s copy of the current branch into the local repo
The following is the same as **git fetch ＜remote＞** followed by **git merge origin/＜current-branch＞**
```sh
git pull <remote>
```

## Fetch remote content without creating a merge commit
```sh
git pull --rebase <remote>
```

## Displays the content being downloaded and the merge details during a pull
```sh
git pull --verbose
```

## View commits
```sh
git status
```

## Current directory's location 
```sh
pwd
```

## List regular files and folders
```sh
ls
```

## View hidden files and folders
```sh
ls -A
```

## Return to parent folder
```sh
cd ..
```

## Add files
```sh
git add .
```

## Add file
```sh
git add <filename>
```

## Create branch
```sh
git branch <branch name>
```

## List branches
```sh
git branch
```

## Switch branch
```sh
git switch <branch name>
```

## Create and switch to new branch
```sh
git switch -c <branch name>
```

## Help with command
```sh
git <command> --help
```
