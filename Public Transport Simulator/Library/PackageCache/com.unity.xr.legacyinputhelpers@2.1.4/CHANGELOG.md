# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [2.1.4] - 2020-04-23
Fix for URP and HDRP project migrating.

## [2.1.3] - 2020-04-08
Fixes incorrect documentation merge

## [2.1.2] - 2020-04-06
Better backcompat with 1.3.X streams 

## [2.1.1] - 2020-03-27
Better backcompat with 1.3.X streams 

## [2.1.0] - 2020-03-25
Roll up of 1.4.0, 1.4.1, 1.4.2 XR Management changes to the 2.X stream

## [2.0.8] - 2020-02-13
Roll up of fixes from 1.3.9->1.3.11
fix for UnityEngine.XR usage outside of ifdefs.
fix for programatically added TPD's not saving
Fixes for 2020.1 deprecation of features.

## [2.0.7] - 2019-12-18
Fixes for 2020.1 deprecation of features. (from 1.3.9)
Fixes switch compile errors. (from 1.3.8)
Fix for PS4 compile error. (from 1.3.7)
## [2.0.6] - 2019-07-25
Fix for incorrect selection logic (from 1.3.6)

## [2.0.5] - 2019-07-23
merges 1.3.5 into the mainline stream

## [2.0.4] - 2019-06-07
Update version number for Yamato.

## [2.0.3] - 2019-06-03
More fixes to compiler error on non XR platforms (thanks again @zilys!)
fixes unit test. removes knuckles finger input bindings

## [2.0.2] - 2019-03-12
fixes compiler error on non XR platforms (thanks @zilys!)

## [2.0.1] - 2019-01-02
fixes standalone compile error / forward port of 1.3.2 fix

## [2.0.0] - 2019-01-02
breaking changes to API to allow pose data queries to indiciate what data was actually valid. this fixes the bug where position was being set to identity if only rotation was provided by the input system.

Pose provider API has now changed to return a PoseDataFlags bitflag, the bitflag will indiciate what pieces of data was set on the output pose parameter.
```csharp
public abstract bool TryGetPoseFromProvider(out Pose output)
```
is now
```csharp
public virtual PoseDataFlags GetPoseFromProvider(out Pose output)
```
All pose providers in this package have been updated, as has the tracked pose driver code to correctly handle the returned bitflags. Any user derived users of this API will need to also update their code accordingly.

New unit tests added for this case in the tracked pose driver

## [1.4.2] - 2020-03-24
removes incorrect comment.

## [1.4.1] - 2020-03-23
minor tweaks to version

## [1.4.0] - 2020-02-11
Updates for XR Management UX Flow changes, the correct version to use for XR mgt is 2.1.0

## [1.3.11] - 2020-02-05
fix for UnityEngine.XR usage outside of ifdefs.

## [1.3.10] - 2019-12-05
fix for programatically added TPD's not saving

## [1.3.9] - 2019-11-12
Fixes for 2020.1 deprecation of features.

## [1.3.8] - 2019-10-21
Fixes switch compile errors.

## [1.3.7] - 2019-08-02
Fix for PS4 compile error.

## [1.3.6] - 2019-07-25
Fix for incorrect selection logic

## [1.3.5] - 2019-07-17
Minor changes to the TPD and its associated editor
- TPD defaults to "Center Eye - HMD Reference" and Reference Tracking off.
- Warnings for when center eye is not picked, if the TPD is on a camera
- "Center Eye renamed" to "Center Eye - HMD Reference" for clarity

Adds XR Settings page when using com.unity.xr.management that allows automatic TPD attachment to cameras.

## [1.3.4] - 2019-07-07
Verified package for 2019.3

## [1.3.2] - 2019-01-18
fixes standalone only compile error

## [1.3.1] - 2018-12-17
merges 1.0.3 into mainline branch.

## [1.3.0] - 2018-12-05
Adds arm model support for 3dof controllers.

## [1.2.1] - 2018-10-25
Makes the input settings menu pop up when you click the menu item

## [1.2.0] - 2018-10-15
Adds the XR Binding Input Asset Seeder and associated documentation and tests

## [1.1.0] - 2018-10-11
Moved some classes internal for cleaner docs.

## [1.0.3] - 2018-11-09
fixes enums erroneously removed

## [1.0.2] - 2018-10-25
fix for inconsistent line endings in trackedposedriver.cs

## [1.0.1] - 2018-10-10
ported API documentation from the engine

## [1.0.0] - 2018-10-09
release prep

## [0.0.4] - 2018-10-09
ci fixes

## [0.0.3] - 2018-10-08
fix for changelog values.

## [0.0.2] - 2018-10-08
updated to latest upm package template

## [0.0.1] - 2018-10-08

### This is the first release of *Unity Package XR Tools*
Initial move from XR Tools to Legacy Input Helper package

