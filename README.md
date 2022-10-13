# AttachAndSnap prototype

Unity prototype using Oculus Integration on Oculus Quest headset to manipulate letters with the hands, attach them to form a word.

[<img src="https://user-images.githubusercontent.com/541021/193836529-29d7c694-2fbc-403a-9a50-84fd9c9bf644.jpg" width="600" />](https://user-images.githubusercontent.com/541021/193836052-59d14e1f-1ab6-4264-9228-f736e49884dc.mp4)

[https://github.com/prossel/AttachAndSnap](https://github.com/prossel/AttachAndSnap)

## Installation

* Create a Unity project (ex 2021.3)
* Switch to Android platform (File | Build Settings)
* Install Oculus Integration v44.0 from [https://developer.oculus.com/downloads/package/unity-integration/44.0/](https://developer.oculus.com/downloads/package/unity-integration/44.0/)
* Configure it for Android, Quest, etc. Detailed instructions here: https://abstractmachine.net/en/tutorials/unity-vr/installation
* Add this repository to the project, see below for various options (choose one).

### Add this repository in a folder

* In the "Assets" folder, create a new folder named "Prototypes"
* Clone or download this repo in the "Prototypes" folder

### Add this repository as a submodule

* If the project is itself init as a repository, you can add this repository as a submodule:

  `git submodule add git@github.com:prossel/AttachAndSnap.git Assets/Prototypes/AttachAndSnap`

* More infos on submodules:
  * https://sweetcode.io/git-submodules/
  * https://www.git-scm.com/book/en/v2/Git-Tools-Submodules

### Add this repository as a subtree

* If the project is itself init as a repository, you can add this repository as a subtree. There a several ways, choose one: 

  1. Using SourceTree (preferred)
     * Right click the side bar and "Add/Link subtree..."
     * Source: git@github.com:prossel/AttachAndSnap.git
     * Branch: master
     * Local relative path: Assets/Prototypes/AttachAndSnap

  2. Using a direct link (not visible in SourceTree, but the link can be added afterwards from SourceTree, also by someone who pulled the main project and also wand to pull or push the subtree)

     `git subtree add -- prefix Assets/Prototypes/AttachAndSnap git@github.com:prossel/AttachAndSnap.git master --squash`

  3. Using a branch (branch is visible in SourceTree, along with all commits, but subtree is not and it's not easy to manage in SourceTree, like pushing to uplink)

     `git remote add -f AttachAndSnap git@github.com:prossel/AttachAndSnap.git`

     `git subtree add --prefix Assets/Prototypes/AttachAndSnap AttachAndSnap master --squash`

* More infos on subtree:
  * https://www.atlassian.com/git/tutorials/git-subtree

## History

See [CHANGELOG.md](CHANGELOG.md)
