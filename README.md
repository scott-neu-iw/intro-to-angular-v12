# Introduction to Angular 12
## Software Installation
1. Install Visual Studio Code
    1. [https://code.visualstudio.com/download](https://code.visualstudio.com/download)
    2. Recommended VsCode Extensions
        1. Angular Essentials (John Papa)
        2. GitLens (Eric Amodio)
        3. Bracket Pair Colorizer (Coenraad5)
2. Install node.js (if you already have node.js, you can update it to the latest)
    1. [https://nodejs.org/en/download](https://nodejs.org/en/download)
    2. Download and install the latest LTS version (14.17.1 as of this document)
    3. If this is the first time you have installed node & npm, skip this step.  If you have already have node installed, update npm to at least 6.11
	    1. Open powershell as an Administrator
		2. Type the following commands:
```
        Set-ExecutionPolicy Unrestricted -Scope CurrentUser -Force
        npm -v

If the result is less than 6.11, execute:

        npm install -g npm-windows-upgrade
        npm-windows-upgrade
```
3. Install Angular 12
	1. Open powershell as an Administrator
	2. Type the following commands to install (12.0.5 is the version as of this document)
```
TO INSTALL
        npm install -g @angular/cli@12.0.5
```
-OR-
```
TO UPDATE
        npm uninstall -g @angular/cli
        npm cache verify
        # if npm version is < 5 then use `npm cache clean --force` 
        npm install -g @angular/cli@12.0.5
```
4. Install Git (for pulling down completed examples)
    1. [https://git-scm.com/downloads](https://git-scm.com/downloads)
    2. Install
5. Download the DotNet SDK
	1. [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download) (minimum of 5.0)
	2. Install
6. Clone the repository
	1. From a powershell prompt, in the target directory
```
        git clone https://github.com/scott-neu-iw/into-to-angular-v12.git
```