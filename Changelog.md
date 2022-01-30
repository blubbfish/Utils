# Changelog

## 1.6.2 - 2022-01-30 - ProgrammLogger improved
### New Features
* ProgrammLogger can now have a path while init, so not need to move the file
* Make it possible that two instances can use the same logfile
* IniReader GetValue can now have a default that returns if no setting is found
### Bugfixes
### Changes
* Codingstyles

## 1.6.1 - 2022-01-20 - ProgrammLogger Fixed
### New Features
### Bugfixes
* Unhandled exception. System.IO.IOException: The file '/var/log/zwaybot/debug.log' already exists.
### Changes

## 1.6.0 - 2022-01-09 - HttpEndpoint added
### New Features
* Add HttpEndpoint
### Bugfixes
### Changes
* Change Readme that its link to the normal repo
* Codingstyles
* Fixing history

## 1.5.0 - 2021-04-10 - Add GetEvent so you can call events by string; Add OwnSingeton class
### New Features
* More allowed Chars for Chapters in inifile
* Add Contribution, Licence and readme
* Adding netcore
* Add changelog
* Filedhelper to helpter
* CmdArgs improvements
* Make netcore to default
* Add OwnSingleton
### Bugfixes
* WriteLine in Programmlogger sometimes make nonesense
* Writeline event are wrong
### Changes
* Remove the binary files
* Move all Classes to subfolder
* Codingstyles

## 1.4.0 - 2018-11-27 - Add Helper to Utils
### New Features
* Add Helper
### Bugfixes
### Changes

## 1.1.3 - 2018-10-02 - Improve CmdArgs
### New Features
* CmdArgs now throw extensions if there is something wrong
### Bugfixes
### Changes

## 1.1.2 - 2018-09-11 - Tiny Codingstyles
### New Features
### Bugfixes
### Changes
* Codingstyles

## 1.1.1 - 2018-05-29 - ProgrammLogger neets to cleanup
### New Features
* Delete the Mess behind
### Bugfixes
### Changes

## 1.1.0 - 2018-05-15 - ProgrammLogger
### New Features
* Add Programmlogger
### Bugfixes
### Changes
* Codingstyles

## 1.0.7.0 - 2018-05-08 - Yet another IniReader improvemnt round again
### New Features
### Bugfixes
### Changes
* Move searchpath to its own function

## 1.0.6.0 - 2017-12-22 - Yet another IniReader improvemnt round
### New Features
* Posibillity to add complete sections to an ini file
### Bugfixes
### Changes

## 1.0.5.2 - 2017-09-26 - And Improve IniReader again
### New Features
* IniReader returns the Sections with or without Brackets now
### Bugfixes
### Changes

## 1.0.5.1 - 2017-09-24 - Improve IniReader again
### New Features
### Bugfixes
* IniReader now supports umlaute
* IniReader matches with Brackets and without
### Changes

## 1.0.5.0 - 2017-08-09 - Improve IniReader
### New Features
* IniReader can now return a whole segemt
### Bugfixes
### Changes

## 1.0.4.1 - 2017-08-08 - Cleanup OwnView
### New Features
### Bugfixes
### Changes
* remove Init from OwnView

## 1.0.4.0 - 2017-04-30 - More Updater
### New Features
* continue Development of Updater
### Bugfixes
### Changes

## 1.0.3.2 - 2017-04-26 - Next Updater
### New Features
* continue Development of Updater
### Bugfixes
### Changes

## 1.0.3.1 - 2017-04-25 - EventArgsHelper
### New Features
* Add EventArgsHelper
### Bugfixes
### Changes
* Codingstyles
* Moves LogEventArgs to EventArgsHelper
* Moves UpdaterEventArgs to EventArgsHelper

## 1.0.2.6 - 2017-04-24 - Better Updater
### New Features
* Add Updater
* Add Disposable Interface
* Using LogEventArgs now for Logging
* Make UpdaterEventArgs as Child of EventArgs
* Updater now using Ownobject as Parent
* Add VersionsInfo to Updater
* Updater spawns a Background Task to Check for Updates
### Bugfixes
### Changes

## 1.0.2.5 - 2017-04-19 - Logging in OwnObject
### New Features
* Add Logging to OwnObject
### Bugfixes
* Avoid nullpointer exception in FileMutex
### Changes
* Codingstyles

## 1.0.2.3 - 2017-04-16 - OwnModel better
### New Features
* Add Remove observer on OwnModel
### Bugfixes
### Changes
* Codingstyles

## 1.0.2.2 - 2017-03-09 - Make it nice
### New Features
### Bugfixes
### Changes
* Codingstyles

## 1.0.2.1 - 2017-03-09 - Filemutex
### New Features
* Add Filemutex
### Bugfixes
* Filelogger backet on wrong pos
### Changes

## 1.0.0.1 - 2016-12-03 - Filelogger improvements
### New Features
* Check Directory and if not exist add it
* Now logs with timestamp
* Can select logdir
* Add Abstract dispose to OwnController
### Bugfixes
### Changes

## 1.0.0.0 - 2015-11-16 - Init
### New Features
* Add CmdArgs, FileLogger, IniReader, OwnController, OwnModel, OwnObject, OwnView, 
### Bugfixes
### Changes

