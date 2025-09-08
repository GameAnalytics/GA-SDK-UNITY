Changelog
---------
<!--(CHANGELOG_TOP)-->
**7.10.6**
* Fixed a bug where GetUserID() could be called before SDK was initialized.
* GetUserID() now returns empty string if called before SDK is not done initializing.

**7.10.5**
* Android: fixed a bug regarding OAID logic
* Android: improved internal user id logic

**7.10.4**
* added fix for bug with getting user id on win32/mac/android
* added fix for tvOS platform
* fixed warning for unsupported platform
* removed unused variables
* repair Max SDK integration

**7.10.3**
* removed user sign-up form
* updated GATool links
* updated documentation links
* updated login requests
* update game and organization requests

**7.10.2**
* fixed legacy FPS warning if events were sent before sdk was initialized:fixed webgl string marshaling bug

**7.10.1**
* fixed error event validation on iOS

**7.10.0**
* added checks to safely upgrade from 7.6.0 , 7.6.1, 7.7.0 without having any impact on metrics for Android

* fixed impression callback json for Digital Turbine
* removed deprecated functions from webgl
* fixed warnings for android

**7.10.0-beta**
* added checks to safely upgrade from 7.6.0 , 7.6.1, 7.7.0 without having any impact on metrics for Android
* fixed impression callback json for Digital Turbine

**7.9.1**
* consent status is now tracked correctly on iOS
* bug fix for fps tracking on iOS
* updated ironsource impression listener

**7.9.0**
* added the iOS Privacy Manifest
* fixed app boot time measurement for Android & iOS

**7.8.0**
* added optional session performance metrics collection
* added optional app boot-time metric collection
* fixed a warning when submitting FPS before initializing the SDK
* added ad event support for webgl

**7.8.0-beta**
* beta versionadded optional session performance metrics collection
* added optional app boot-time metric collection

**7.7.2**
* added optional external user id:increased resource currency limit from 20 to 50

**7.7.1**
* fixed a bug in the android user-id generation

**7.7.0**
* updated impression listeners for AdMob 8.0.0 (please check out the migration guide: https://developers.google.com/admob/unity/migration)
* added functionality to opt out of GAID & IDFV tracking
* optional local event caching if event submission is disabled:support for AppOpen ads
* support for custom initialization callbacks

**7.6.2b**
** beta version **
* added functionality for opting out of GAID/IDFV tracking on request
* option to enable local storing of events with submission disabled
* updated impression listener for AdMob 8.0.0, please check the migration guide: https://developers.google.com/admob/unity/migration
* beta support for app open ads

**7.6.1**
* fixed openupm version

**7.6.0**
* deprecated armv7 and i386 for iOS
* added option to opt-out of GAID tracking for android

**7.5.1**
* reintroduced the health event
* added method to retrieve the user id

**7.5.0**
* Temporary removal of FPS tracking due to rare issues observed. FPS tracking will be reintroduced in the future once the cause is identified and fixed.

**7.4.4**
* dependency fix for openupm package

**7.4.2**
* bugfix for android health event error reporting

**7.4.1**
* fixed a crash with the health event on android if not on the main thread:added session_num for the init event on iOS

**7.4.0**
* Added Health event
* Includes FPS data measurement automatically sent at the end of the session


**7.3.24**
* fixed fps script for when pausing game

**7.3.23**
* fixed bug with custom fields (locale related bug)

**7.3.22**
* added external dependency manager for .unitypackage release as well to solve issues with appset id on android

**7.3.21**
* fixed bug in internal error reporting

**7.3.20**
* added event uuid to events sent

**7.3.19**
* updated dependencies

**7.3.18**
* fixed fps event script to still run coroutines when Time.timeScale = 0 by using WaitForSecondsRealtime instead of WaitForSeconds

**7.3.17**
* added method manually update gamekey and secretkey

**7.3.16**
* fixed uwp build errors

**7.3.15**
* changed frequency logic for fps events

**7.3.14**
* small fix

**7.3.13**
* fixed crash for android builds

**7.3.12**
* removed imei identifiers and other alternative identifiers from user identifier logic (android)

**7.3.11**
* updated dependency to external dependency resolver

**7.3.10**
* adding missing .meta file for upm release
* switched to using openupm for scoped registry when using upm, please update the upm setup for the unity sdk

**7.3.9**
* fixed upm package.json

**7.3.8**
* fixed upm dependecy

**7.3.7**
* added dependencies.xml for upm release

**7.3.6**
* fixed playmaker bugs

**7.3.5**
* changed settings to have FPS events turned off by default

**7.3.4**
* added error events to be sent for invalid custom event fields used
* added optional mergeFields argument to event methods to merge with global custom fields instead of overwrite them

**7.3.3**
* playmaker fixes

**7.3.2**
* fixed missing custom event fields for when trying to fix missing session end events

**7.3.1**
* fixed editor ui bug with games with the same name

**7.3.0**
* added global custom event fields function to allow to add custom fields to events sent automatically by the SDK

**7.2.1**
* added functionality to force a new user in a/b testing without having to uninstall app first, simply use custom user id function to set a new user id which hasn't been used yet

**7.2.0**
* added support for admob impression events

**7.1.1**
* fixed build errors for desktop platforms

**7.1.0**
* added custom event fields feature

**7.0.5**
* updated hyperbid ilrd integration

**7.0.4**
* added ios part for aequus ilrd integration

**7.0.3**
* renamed ATT method name to avoid dupplicate symbols

**7.0.2**
* fixes to ILRD integrations

**7.0.1**
* updated ILRD integrations

**7.0.0**
* Changed user identifier logic in preparation for Google changes to GAID. User id for a new install is now a randomised GUID. Existing installs that update SDK will continue using previous identifier logic. It is recommended to update as soon as possible to reduce impact on calculated metrics.

**6.7.1**
* fixes to max irld integration

**6.7.0**
* added option to enable native exceptions error reporting for android and ios

**6.6.4**
* prepared for google advertising identifier changes (will not use google advertising identifier when user has opted out) (android)
* it should now be possible to not show idfa consent dialog if you don't have any third party code that needs to use idfa

**6.6.3**
* removed unnecessary append of stacktrace when stacktrace is missing from warning/error messages sent to GA backend

**6.6.2**
* fixed ios compile errors
* fixed ILRD related compile error
* added ios_testflight to events coming from testflight builds

**6.6.1**
* added ILRD support for Aequus SDK
* fixed ILRD related bug

**6.6.0**
* more fixes for using ILRD when using SDK from UPM
* OBS now you need to use GameAnalyticsILRD class to subscribe to ILRD impression events (also download latest GA ILRD .unitypackage to get it to work if using SDK from UPM, see docs for more info)
* prepared for google advertising identifier changes (will not use google advertising identifier when user has opted out) (android)

**6.5.8**
* fixed compile issues when using ILRD with SDK from UPM (now you have to download a seperate .unitypackage when using ILRD with SDK from UPM)

**6.5.7**
* added missing architectures of ios libraries

**6.5.6**
* fixed crash bug for ios

**6.5.5**
* reverted back to v6.5.2 as crashes happen for ios which have not been narrowed down to what is causing this yet

**6.5.4**
* fixed typo error

**6.5.3**
* added support for max in impression events
* fixed dupplicate symbol on ios

**6.5.2**
* fixed crashes for android devices without google play API (android)

**6.5.1**
* added support for OAID (android)
* added missing export function for UWP lib (uwp)

**6.5.0**
* added app tracking transparency request function (ios)

**6.4.1**
* added idfa consent status field to events (ios)

**6.4.0**
* added support for unity package manager

**6.3.14**
* fixed playmaker scripts

**6.3.13**
* fixed compiler errors for 2017.1
* updated client ts validator

**6.3.12**
* changed AppTrackingTransparency dependency to be optional for iOS

**6.3.11**
* fixed dependencies for iOS (min. XCode 12 required)

**6.3.10**
* fix to mopub impression events, require min. mopub v5.14.0
* REMEMBER to update to this version of the SDK if you use MoPub impression events and you have min. v5.14.0 of MoPub SDK inatalled or else the SDK will not send MoPub impression event for you any more

**6.3.9**
* fixed ironsource impression event compile error

**6.3.8**
* removed memory info from automatic crash reports
* updated sqlite lib

**6.3.7**
* corrected ad event annotation

**6.3.6**
* updated validator for impression events (android, ios)

**6.3.5**
* fixed user log in function in editor for unity 2017

**6.3.4**
* fixed null exception happening some times on desktop platforms (mac, linux, windows)

**6.3.3**
* updated impression events

**6.3.2**
* fixed www build errors on unity 2017

**6.3.1**
* improved user identifer flow for ios (ios)

**6.3.0**
* updated user identifier flow to prepare for iOS 14 IDFA changes (ios)

**6.2.4**
* fixed progression events with scores (android)

**6.2.3**
* editor ui error fix

**6.2.2**
* tvos compile fix

**6.2.1**
* added better error messages for API calls potentially being removed in the future

**6.2.0**
* exposed functions to get AB testing id and variant id

**6.1.5**
* fixed bug in ui for settings.asset

**6.1.4**
* fixed instant app bug(android)
* remember to update your proguard file (https://gameanalytics.com/docs/item/unity-sdk#proguard-only-android)

**6.1.3**
* fixed sign up function
* added organizations to games overview
* you might need to delete the settings.asset file and set the keys again for the different platforms

**6.1.2**
* fixed ios compile error (ios)

**6.1.1**
* added missing instant app dependency (android)

**6.1.0**
* added new impression event, see documentation page for more info (android, ios)

**6.0.15**
* added support for android instant apps (android)

**6.0.14**
* added session_num to init requests

**6.0.13**
* logo updated

**6.0.12**
* hiding archived games bug fix in editor

**6.0.11**
* removed facebook, gender and birthyear methods

**6.0.10**
* settings namespace fix

**6.0.9**
* A/B testing fixes

**6.0.8**
* namespace bug fix
* batchmode bug fix

**6.0.7**
* fixed getRemoteConfigsValueAsString bug (ios)

**6.0.6**
* remote configs fixes

**6.0.5**
* fixed javascript events bug

**6.0.4**
* fixed ios events bug

**6.0.3**
* fixed ios build error

**6.0.2**
* corrected naming of function

**6.0.1**
* small fixes

**6.0.0**
* Remote Config calls have been updated and the old calls have deprecated. Please see GA documentation for the new SDK calls and migration guide
* A/B testing support added
* ad event added (ios, android only)

**5.2.2**
* compile error fixes for windows builds

**5.2.1**
* fixed uwp compile errors

**5.2.0**
* improved device identifier flow (android)
* removed google play services dependency
* OPS refactored IMEI code out into a seperate library, please check the documentation page for how to use it now (android, only relevant for apps using IMEI ids)

**5.1.11**
* fixed import bug for 2019.2

**5.1.10**
* fixed compile error in GA_SettingsInspector script

**5.1.9**
* fixed typo in editor script

**5.1.8**
* session fixes for desktop platforms

**5.1.7**
* bug fix for special events script: changed script to `Time.unscaledTime` instead of `Time.time` to calculate FPS. This might affect your FPS metrics if your game is using fast motion or slow motion effects in your game in other words if you change time scale frequently during your game.

**5.1.6**
* thread bug fix for desktop

**5.1.5**
* session length fixed for desktop platforms

**5.1.4**
* small correction for logging for desktop

**5.1.3**
* small timing fix for initialization

**5.1.2**
* fixes to events not being sent for desktop platforms

**5.1.1**
* ios and tvos wrapper fixes

**5.1.0**
* added enable/disable event submission function

**5.0.12**
* UnityWebRequest fixes

**5.0.11**
* fixed 2018.3 www deprecated warnings

**5.0.10**
* fixed business event validation

**5.0.9**
* fixed json deserializing for desktop platforms

**5.0.8**
* fixed freeze bug on exit for desktop platforms

**5.0.7**
* added gameanalytics android aar library file instead of jar file

**5.0.6**
* tvOS bug fixes

**5.0.5**
* added play services resolver

**5.0.4**
* added missing playmaker actions

**5.0.3**
* fixed playmaker errors

**5.0.2**
* changed log warnings to log errors for when notifying the user about not having initialized the SDK before trying to send events
* updated android google play libraries (android)

**5.0.1**
* fixed compile error for ios

**5.0.0**
* added command center functionality
* fixed various bugs

**4.0.6**
* removed mac sqlite library so local installed one is used (mac)

**4.0.5**
* fixes to not trying to send events if SDK has not been initialized yet

**4.0.4**
* fixes to android session handling
* added custom dimensions to design and error events
* fixes to sqlite db path for windows and linux platforms

**4.0.3**
* small uwp bug fix (uwp)

**4.0.2**
* small editor bug fixes for stop showing log warnings after calling initialize function

**4.0.1**
* small update for wehn signing up in editor

**4.0.0**
* dumped major version to emphasize the need to use manual initialization of the SDK now
* added log warnings to warn if the SDK has not been manually initialized before sending events

**3.11.3**
* fixed session length bug
* fixed not allowing to send event when session is not started

**3.11.2**
* updated android google play libraries (android)

**3.11.1**
* various bug fixes in android native library (android)

**3.11.0**
* ui updated for sign up dialog
* PLEASE NOTE: initialization is now manual and not automatic anymore

**3.10.5**
* 'install' field added to session start events when called for the first time (android, ios)

**3.10.4**
* fixed javascript library (webgl)

**3.10.3**
* bug fix to webgl platform (webgl)

**3.10.2**
* Android manifest file fix for Android SDK Tools v26.0.2 (android)
* Send build number option fixed for Unity 5.6 and up (ios, android)
* Added stack traces for non-development builds

**3.10.1**
* Advanced option Send Player Settings Build changed to work without refreshing the Settings object. Player Settings Build number is sent for iOS and identification Version number for Android when option is used (Unity 5)

**3.10.0**
* changed the behaviour of using IMEI with the 'READ_PHONE_STATE' permission to guarantee precise analytics for certain regions (android)

**3.9.12**
* fixed webgl wrapper to use correct namespace (webgl)

**3.9.11**
* updated to v10.2.1 of Google Play Services libraries (android)

**3.9.10**
* prevent session_num and transaction_num from resetting if app is killed (ios)

**3.9.9**
* added option to exclude Google Play libraries when building (android)

**3.9.8**
* bug fix for end session when using manual session handling

**3.9.7**
* bug fix for sending events straight after initializing sdk (webgl)

**3.9.6**
* minor fix for app version validation (android)

**3.9.5**
* removed debug log messages (webgl)

**3.9.4**
* session length precision improvement

**3.9.3**
* option added in Advanced for automatically assigning the bundle version from Player Settings as the build number (ios and android)
* fixed error at launch for builds using manual session handling (webgl)

**3.9.2**
* added app signature and channel id (which app store was the app installed from)(android)
* added IMEI as fallback option for identifier when Google AID and Android ID is not available on the device (requires to add optional READ_PHONE_STATE permission)(android)

**3.9.1**
* fixed webgl compile hang/freeze bug

**3.9.0**
* native tizen library added

**3.8.9**
* added native javascript library for WebGL (webgl)

**3.8.8**
* added bundle_id, app version and app build tracking (iOS and Android)

**3.8.7**
* possible to set custom dimensions and demographics before initialize

**3.8.6**
* session length bug fix
* fixed bug when using manual session handling (android)

**3.8.5**
* bug fix to design events sent without value

**3.8.4**
* fix to Windows Store App certification test (Windows Phone) caused by GameAnalytics plugin (wsa)

**3.8.3**
* removed x86_64h architecture from sqlite3.bundle (mac)

**3.8.2**
* fix to Windows Store App certification test caused by GameAnalytics plugin (wsa)

**3.8.1**
* fix to Windows Store App certification test caused by GameAnalytics plugin (uwp, wsa)

**3.8.0**
* added parameters validation for custom methods called during editor runtime

**3.7.2**
* updated guide
* fixed editor dynamic path finding issue on Windows machines

**3.7.1**
* added support for Tizen

**3.7.0**
* added support for Universal Windows 8 (Windows 8 and Windows Phone 8)

**3.6.3**
* fixed user_id tracking for iOS 10 (ios, tvos)
* added MetroLog to use for logging in UWP library (uwp)
* small fix related to manual session handling (android)

**3.6.2**
* fix isAppStoreReceiptSandbox bug on iOS 6 devices and lower (iOS)

**3.6.1**
* renamed methods to avoid duplicate symbols (iOS)

**3.6.0**
* Google Play Services libraries updated to version 9.4.0 (android)

**3.5.3**
* fixed manual session handling compiling issue (iOS, tvOS)

**3.5.2**
* fixed support for UWP with IL2CPP backend bug

**3.5.1**
* added manual session handling
* fixed bug for client timestamp handling and session length in certain edge cases

**3.5.0**
* added support for UWP (uwp)

**3.4.3**
* changed to use https to send events on windows, Mac, Windows, Linux and WebGL (mac, windows, linux, webgl)

**3.4.2**
* fixed playmaker bug

**3.4.1**
* external storage read and write permissions are now optional (android)

**3.4.0**
* added support for Standalone platforms (Windows, Mac, Linux) and WebGL
* reworked settings inspector UI to support new platforms
* moved Unity 4.6.x support to a separate package (see [here](https://github.com/GameAnalytics/GA-SDK-UNITY/wiki/Download%20and%20Installation) for more details)

**3.3.3**
* fixed bug related to network changes for Android API level 23 and above (android)

**3.3.2**
* Added max cap (20) for custom dimensions, resource currencies and resource item types
* tvOS library Unity asset importer bug fix

**3.3.1**
* Google Play Services libraries updated to version 8.4.0 (android)

**3.3.0**
* feature for using a custom user id
* fix testflight issue with user id generation (ios)

**3.2.3**
* fixed disappearing GameAnalytics prefab bug (happening since Unity 5.3.0)

**3.2.2**
* postprocess script fixed when ios library is not installed
* Unity 4.7.x support fix

**3.2.1**
* error events fixed (android)

**3.2.0**
* added support for tvOS (tvos)

**3.1.1**
* fixed inclusion of faulty library in 3.1.0 (ios)

**3.1.0**
* altered jailbreak check causing ios9 warning (ios)
* library / framework now compiled with bitcode (ios)
* alternative non-bitcode library (Xcode6) added (ios)
* restructuring to prepare for tvOS (ios)
* fix issue for offline initialization (android)

**3.0.0b**
* new java-only implementation (android)
* install size reduced (android)

**2.4.3**
* Google Play Services libraries updated to version 8.3.0 (android)

**2.4.2**
* Postprocessor script for XCode fixed for Unity 4.6.x (ios)

**2.4.1**
* android.permission.WRITE_EXTERNAL_STORAGE not needed anymore (android)

**2.4.0**
* fixed tracking bug with ad opt out enabled (android)

**2.3.2**
* no network connection bug fix (android)

**2.3.1**
* fix related to connection changes (android)


**2.3.0**
* 'Receiver not registered' bug fix (android)

**2.2.0**
* improved code structure (android)
* built library with Xcode7 (iOS 9.0)
* fixed missing links in setup guide
* minor tweaks

**2.1.4**
* increased allowed character count to 64 for many parameters (android)

**2.1.3**
* fix for session length (android)

**2.1.2**
* fixed enable/disable submit errors in unity settings
* improved session handling (android)
* minor bug fixes (android)

**2.1.1**
* fixed rare editor bug causing settings object replication
* minor bug fixes (android)

**2.1.0**
* Android support added
* Google Play purchase validation
* support for individual game keys for each supported platform

**2.0.4**
* fixed an issue with going-to-background on iOS 6
* fixed submit of birthyear value

**2.0.3**
* fixed an iOS 6 issue
* added a post process build script for setting up Xcode (Unity 5 only)

**2.0.2**
* fix for PlayMaker events
* fix for Critical FPS events

**2.0.1**
* fix for business event receipt rejection in some cases
* tweaked local db size trimming

**2.0.0**
* redesigned Unity SDK
* initially with **only iOS** support
* progression event
* business event validation
* resource event
* custom dimensions
