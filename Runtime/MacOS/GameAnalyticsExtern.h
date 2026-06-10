#pragma once
#ifndef _GA_EXTERNAL_H_
#define _GA_EXTERNAL_H_

#ifdef __cplusplus
extern "C" {
#endif

#ifdef GA_SHARED_LIB
	#if defined(_WIN32)
		#define GA_API __declspec(dllexport)
	#else
		#define GA_API __attribute__((visibility("default")))
	#endif
#else
	#if defined (_WIN32)
		#define GA_API __declspec(dllimport)
	#else
		#define GA_API
	#endif
#endif

enum GAErrorCode
{
	EGANoError = 0,
	EGAFailure,
	EGABufferError
};

enum GAStatus: char
{
	EGADisabled = 0,
	EGAEnabled
};

enum GAResourceFlowType
{
    EGASource = 1,
    EGASink = 2
};

enum GAProgressionStatus
{
    EGAStart = 1,
    EGAComplete = 2,
    EGAFail = 3
};

enum GAErrorSeverity
{
    EGADebug       = 1,
    EGAInfo        = 2,
    EGAWarning     = 3,
    EGAError       = 4,
    EGACritical    = 5
};

enum GALoggerMessageType
{
    EGALogError   = 0,
    EGALogWarning = 1,
    EGALogInfo    = 2,
    EGALogDebug   = 3,
    EGALogVerbose = 4
};

typedef float(*GAFpsTracker)(void);
typedef void(*GALogHandler)(const char* message, GALoggerMessageType messageType);

GA_API void gameAnalytics_freeString(const char* ptr);

GA_API void gameAnalytics_configureAvailableCustomDimensions01(const char **customDimensions, int size);
GA_API void gameAnalytics_configureAvailableCustomDimensions02(const char **customDimensions, int size);
GA_API void gameAnalytics_configureAvailableCustomDimensions03(const char **customDimensions, int size);
GA_API void gameAnalytics_configureAvailableResourceCurrencies(const char **resourceCurrencies, int size);
GA_API void gameAnalytics_configureAvailableResourceItemTypes(const char **resourceItemTypes, int size);
GA_API void gameAnalytics_configureBuild(const char *build);
GA_API void gameAnalytics_configureWritablePath(const char *writablePath);
GA_API void gameAnalytics_configureBuildPlatform(const char *platform);
GA_API void gameAnalytics_configureCustomLogHandler(GALogHandler handler);
GA_API void gameAnalytics_disableDeviceInfo();
GA_API void gameAnalytics_configureDeviceModel(const char *deviceModel);
GA_API void gameAnalytics_configureDeviceManufacturer(const char *deviceManufacturer);

// the version of SDK code used in an engine. Used for sdk_version field.
// !! if set then it will override the SdkWrapperVersion.
// example "unity 4.6.9"
GA_API void gameAnalytics_configureSdkGameEngineVersion(const char *sdkGameEngineVersion);

// the version of the game engine (if used and version is available)
GA_API void gameAnalytics_configureGameEngineVersion(const char *engineVersion);

GA_API void gameAnalytics_configureUserId(const char *uId);

GA_API void gameAnalytics_configureExternalUserId(const char* extId);

// initialize - starting SDK (need configuration before starting)
GA_API void gameAnalytics_initialize(const char *gameKey, const char *gameSecret);

// add events
GA_API void gameAnalytics_addBusinessEvent(const char *currency, double amount, const char *itemType, const char *itemId, const char *cartType, const char *customFields, GAStatus mergeFields);
GA_API void gameAnalytics_addResourceEvent(GAResourceFlowType flowType, const char *currency, double amount, const char *itemType, const char *itemId, const char *customFields, GAStatus mergeFields);
GA_API void gameAnalytics_addProgressionEvent(GAProgressionStatus progressionStatus, const char *progression01, const char *progression02, const char *progression03, const char *customFields, GAStatus mergeFields);
GA_API void gameAnalytics_addProgressionEventWithScore(GAProgressionStatus progressionStatus, const char *progression01, const char *progression02, const char *progression03, int score, const char *customFields, GAStatus mergeFields);
GA_API void gameAnalytics_addDesignEvent(const char *eventId, const char *customFields, GAStatus mergeFields);
GA_API void gameAnalytics_addDesignEventWithValue(const char *eventId, double value, const char *customFields, GAStatus mergeFields);
GA_API void gameAnalytics_addErrorEvent(GAErrorSeverity severity, const char *message, const char *customFields, GAStatus mergeFields);

// set calls can be changed at any time (pre- and post-initialize)
// some calls only work after a configure is called (setCustomDimension)
GA_API void gameAnalytics_setEnabledInfoLog(GAStatus flag);
GA_API void gameAnalytics_setEnabledVerboseLog(GAStatus flag);
GA_API void gameAnalytics_setEnabledManualSessionHandling(GAStatus flag);
GA_API void gameAnalytics_setEnabledErrorReporting(GAStatus flag);
GA_API void gameAnalytics_setEnabledEventSubmission(GAStatus flag);
GA_API void gameAnalytics_setCustomDimension01(const char *dimension01);
GA_API void gameAnalytics_setCustomDimension02(const char *dimension02);
GA_API void gameAnalytics_setCustomDimension03(const char *dimension03);

GA_API void gameAnalytics_setGlobalCustomEventFields(const char *customFields);

GA_API void gameAnalytics_startSession();
GA_API void gameAnalytics_endSession();

// game state changes
// will affect how session is started / ended
GA_API void gameAnalytics_onResume();
GA_API void gameAnalytics_onSuspend();
GA_API void gameAnalytics_onQuit();

GA_API const char* gameAnalytics_getRemoteConfigsValueAsString(const char *key);
GA_API const char* gameAnalytics_getRemoteConfigsValueAsStringWithDefaultValue(const char *key, const char *defaultValue);
GA_API const char* gameAnalytics_getRemoteConfigsValueAsJson(const char* key);

GA_API const char* gameAnalytics_getUserId();
GA_API const char* gameAnalytics_getExternalUserId();

GA_API GAStatus    gameAnalytics_isRemoteConfigsReady();
GA_API const char* gameAnalytics_getRemoteConfigsContentAsString();

GA_API const char* gameAnalytics_getABTestingId();
GA_API const char* gameAnalytics_getABTestingVariantId();

GA_API long long gameAnalytics_getElapsedSessionTime();
GA_API long long gameAnalytics_getElapsedTimeFromAllSessions();
GA_API long long gameAnalytics_getElapsedTimeForPreviousSession();

GA_API void gameAnalytics_enableSDKInitEvent(GAStatus status);
GA_API void gameAnalytics_enableMemoryHistogram(GAStatus status);
GA_API void gameAnalytics_enableFPSHistogram(GAFpsTracker tracker, GAStatus status);
GA_API void gameAnalytics_enableHardwareTracking(GAStatus status);

#ifdef __cplusplus
}
#endif

#endif // _GA_EXTERNAL_H_
