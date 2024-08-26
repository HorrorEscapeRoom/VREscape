# Git Commands

## Commit Changes
```sh
git commit -m "<message>"
```

## Configure
```sh
git config --global user.name <name>
git config --global user.email <email>
```

## Repository Operations

### Clone a Repository
```sh
git clone https://github.com/<username>/<repository>
```

### Add Files to Staging Area

#### All Files
```sh
git add .
```

#### Single File
```sh
git add <filename>
```

### View Commit Status
```sh
git status
```

## Branch Management

### Create a New Branch
```sh
git branch <branch-name>
```

### List All Branches
```sh
git branch
```

### Switch Branch
```sh
git switch <branch-name>
```

### Create and Switch to a New Branch
```sh
git switch -c <branch-name>
```

## Synchronization

### Pull and Merge Remote Branch into Local Repository
```sh
git pull <remote>
```

### Fetch Remote Content Without Creating a Merge Commit
```sh
git pull --rebase <remote>
```

### Display Verbose Output During Pull
```sh
git pull --verbose
```

## Help with Git Commands
```sh
git <command> --help
```

# Shell Commands

### View Current Directory's Location
```sh
pwd
```

### List Files and Folders
```sh
ls
```

### View Hidden Files and Folders
```sh
ls -A
```

### Navigate to Parent Directory
```sh
cd ..
```

### Make Directory
```sh
mkdir <name>
```

### Make Directory Inside Another
```sh
mkdir ../mydir
```

# Resources
- [Learn Git Branching](https://learngitbranching.js.org/)
- [Head First Git](https://www.oreilly.com/library/view/head-first-git/9781492092506/)
