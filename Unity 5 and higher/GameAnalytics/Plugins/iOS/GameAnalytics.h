//
//  GameAnalytics.h
//  GA-SDK-IOS
//
//  Copyright (c) 2015 GameAnalytics. All rights reserved.
//

#import <Foundation/Foundation.h>

/*!
 @enum
 @discussion
 This enum is used to specify flow in resource events
 @constant GAResourceFlowTypeSource
 Used when adding to a resource currency
 @constant GAResourceFlowTypeSink
 Used when subtracting from a resource currency
 */
typedef enum GAResourceFlowType : NSInteger {
    GAResourceFlowTypeSource = 1,
    GAResourceFlowTypeSink = 2
} GAResourceFlowType;


/*!
 @enum
 @discussion
 This enum is used to specify status for progression event
 @constant GAProgressionStatusStart
 User started progression
 @constant GAProgressionStatusComplete
 User succesfully ended a progression
 @constant GAProgressionStatusFail
 User failed a progression
 */
typedef enum GAProgressionStatus : NSInteger {
    GAProgressionStatusStart = 1,
    GAProgressionStatusComplete = 2,
    GAProgressionStatusFail = 3
} GAProgressionStatus;


/*!
 @enum
 @discussion
 This enum is used to specify severity of an error event
 @constant GAErrorSeverityDebug
 @constant GAErrorSeverityInfo
 @constant GAErrorSeverityWarning
 @constant GAErrorSeverityError
 @constant GAErrorSeverityCritical
 */
typedef enum GAErrorSeverity : NSInteger {
    GAErrorSeverityDebug = 1,
    GAErrorSeverityInfo = 2,
    GAErrorSeverityWarning = 3,
    GAErrorSeverityError = 4,
    GAErrorSeverityCritical = 5
} GAErrorSeverity;

/*!
 @enum
 @discussion
 This enum is used to specify action of an ad event
 @constant GAAdActionClicked
 @constant GAAdActionShow
 @constant GAAdActionFailedShow
 @constant GAAdActionRewardReceived
 @constant GAAdActionRequest
 @constant GAAdActionLoaded
 */
typedef enum GAAdAction : NSInteger {
    GAAdActionClicked = 1,
    GAAdActionShow = 2,
    GAAdActionFailedShow = 3,
    GAAdActionRewardReceived = 4,
    GAAdActionRequest = 5,
    GAAdActionLoaded = 6
} GAAdAction;

/*!
 @enum
 @discussion
 This enum is used to specify type of an ad event
 @constant GAAdTypeVideo
 @constant GAAdTypeRewardedVideo
 @constant GAAdTypePlayable
 @constant GAAdTypeInterstitial
 @constant GAAdTypeOfferWall
 @constant GAAdTypeBanner
 */
typedef enum GAAdType : NSInteger {
    GAAdTypeVideo = 1,
    GAAdTypeRewardedVideo = 2,
    GAAdTypePlayable = 3,
    GAAdTypeInterstitial = 4,
    GAAdTypeOfferWall = 5,
    GAAdTypeBanner = 6
} GAAdType;

/*!
 @enum
 @discussion
 This enum is used to specify error reason of an ad event
 @constant GAAdErrorUnknown
 @constant GAAdErrorOffline
 @constant GAAdErrorNoFill
 @constant GAAdErrorInternalError
 @constant GAAdErrorInvalidRequest
 @constant GAAdErrorUnableToPrecache
 */
typedef enum GAAdError : NSInteger {
    GAAdErrorUnknown = 1,
    GAAdErrorOffline = 2,
    GAAdErrorNoFill = 3,
    GAAdErrorInternalError = 4,
    GAAdErrorInvalidRequest = 5,
    GAAdErrorUnableToPrecache = 6
} GAAdError;

//Similar to IRemoteConfigsListener in the GameAnalytics Android library
@protocol GARemoteConfigsDelegate <NSObject>
@optional
- (void) onRemoteConfigsUpdated; // Updated everytime when configurations are added
@end



@class GameAnalytics;

@interface GameAnalytics : NSObject


/*!
 @method

 @abstract Define available 1st custom dimensions

 @discussion <i>Example usage:</i>
 <pre><code>
 NSArray *dimensionArray = @[@"dimA", @"dimB", @"dimC"];<br>
 [GameAnalytics configureAvailableCustomDimensions01:dimensionArray];
 </code></pre>

 @param customDimensions
   Must be an array of strings.<br>
   Array max length=20, String max length=32)

 @availability Available since 2.0.0

 @attribute Note! This method must be called before initializing the SDK
 */
+ (void)configureAvailableCustomDimensions01:(NSArray *)customDimensions;

/*!
 @method

 @abstract Set available 2nd custom dimensions

 @discussion <i>Example usage:</i>
 <pre><code>
 NSArray *available = @[@"dimD", @"dimE", @"dimF"];<br>
 [GameAnalytics configureAvailableCustomDimensions02:dimensionArray;
 </code></pre>

 @param customDimensions
   Must be an array of strings.<br>
   (Array max length=20, String max length=32)

 @availability Available since 2.0.0

 @attribute Note! This method must be called before initializing the SDK
 */
+ (void)configureAvailableCustomDimensions02:(NSArray *)customDimensions;

/*!
 @method

 @abstract Set available 3rd custom dimensions

 @discussion <i>Example usage:</i>
 <pre><code>
 NSArray *available = @[@"dimA", @"dimB", @"dimC"];<br>
 [GameAnalytics configureAvailableCustomDimensions03:dimensionArray];
 </code></pre>

 @param customDimensions
    Must be an array of strings.<br>
    (Array max length=20, String max length=32)

 @availability Available since 2.0.0

 @attribute Note! This method must be called before initializing the SDK
 */
+ (void)configureAvailableCustomDimensions03:(NSArray *)customDimensions;

/*!
 @method

 @abstract Set available resource currencies

 @discussion <i>Example usage:</i>
 <pre><code>
 NSArray *availableCurrencies = @[@"gems", @"gold"];<br>
 [GameAnalytics configureAvailableResourceCurrencies:availableCurrencies];
 </code></pre>

 @param resourceCurrencies
    Must be an array of strings.<br>
    (Array max length=20, String max length=32)

 @availability Available since 2.0.0

 @attribute Note! This method must be called before initializing the SDK
 */
+ (void)configureAvailableResourceCurrencies:(NSArray *)resourceCurrencies;

/*!
 @method

 @abstract Set available resource item types

 @discussion <i>Example usage:</i>
 <pre><code>
 NSArray *availableItemTypes = @[@"upgrades", @"powerups"];<br>
 [GameAnalytics configureAvailableResourceItemTypes:availableItemTypes];
 </code></pre>

 @param resourceItemTypes
    Must be an array of strings.<br>
    (Array max length=20, String max length=32)

 @availability Available since 2.0.0

 @attribute Note! This method must be called before initializing the SDK
 */
+ (void)configureAvailableResourceItemTypes:(NSArray *)resourceItemTypes;

/*!
 @method

 @abstract Set app build version

 @discussion <i>Example usage:</i>
 <pre><code>
 [GameAnalytics configureBuild:@"0.0.1"];
 </code></pre>

 @param build
    (String max length=32)

 @availability Available since 2.0.0

 @attribute Note! This method must be called before initializing the SDK
 */
+ (void)configureBuild:(NSString *)build;

/*!
 @method

 @abstract Set a custom unique user_id identifying the user.

 @discussion <i>Example usage:</i>
 <pre><code>
 [GameAnalytics configureUserId:@"24566"];
 </code></pre>

 @param userId
 (String max length=64)

 @availability Available since 2.2.0

 @attribute Note! This method must be called before initializing the SDK
 */
+ (void)configureUserId:(NSString *)userId;


/* @IF WRAPPER */

/*
 Used ONLY by GameAnalytics wrapper SDK's (for example Unity).
 Never call this manually!
 */
+ (void)configureSdkVersion:(NSString *)wrapperSdkVersion;
/* @ENDIF WRAPPER */

/*!
 @method

 @abstract Set app engine version

 @discussion <i>Example usage:</i>
 <pre><code>
 [GameAnalytics configureEngineVersion:@"unreal 4.8.1"];
 </code></pre>

 @param engineVersion
 (String)

 @availability Available since 2.0.0

 @attribute Note! This method must be called before initializing the SDK
 */
+ (void)configureEngineVersion:(NSString *)engineVersion;

/*!
 @method

 @abstract Enable auto detect of app version to use for build field

 @discussion <i>Example usage:</i>
 <pre><code>
 [GameAnalytics configureAutoDetectAppVersion:YES];
 </code></pre>

 @param flag
 (String)

 @availability Available since 4.1.0

 @attribute Note! This method must be called before initializing the SDK
 */
+ (void)configureAutoDetectAppVersion:(BOOL)flag;

/*!
 @method

 @abstract Configure the game key and secret key before initializing. Used by certain frameworks (like Frabric.io) needing to set the keys during configure phase.

 @discussion
 <i>Example usage:</i>
 <pre><code>
 [GameAnalytics configureGameKey:@"123456789ABCDEFGHIJKLMNOPQRSTU" gameSecret:@"123456789ABCDEFGHIJKLMNOPQRSTU12345678"];
 </code></pre>

 @param gameKey
 (String)
 @param gameSecret
 (String)

 @availability Available since 2.0.8

 */
+ (void)configureGameKey:(NSString *)gameKey
              gameSecret:(NSString *)gameSecret;

/*!
 @method

 @abstract Initialize GameAnalytics SDK

 @discussion
 <i>Example usage:</i>
 <pre><code>
 [GameAnalytics initializeWithGameKey:@"123456789ABCDEFGHIJKLMNOPQRSTU" gameSecret:@"123456789ABCDEFGHIJKLMNOPQRSTU12345678"];
 </code></pre>

 @param gameKey
    (String)
 @param gameSecret
    (String)

 @availability Available since 2.0.0
 */
+ (void)initializeWithGameKey:(NSString *)gameKey
                   gameSecret:(NSString *)gameSecret;



/*!
 @method

 @abstract Initialize GameAnalytics SDK when the game key and game secret has been configured earlier.

 @discussion <i>Example usage:</i>
 <pre><code>
 [GameAnalytics initializeWithConfiguredGameKeyAndGameSecret];
 </code></pre>

 @availability Available since 2.0.8

 @attribute Note! This method can only be used if the configureGameKey:gameSecret: method is called before.

 */
+ (void)initializeWithConfiguredGameKeyAndGameSecret;


/*!
 @method

 @abstract Add new business event with receipt

 @param currency
    Currency code in ISO 4217 format. (e.g. USD)
 @param amount
    Amount in cents (int). (e.g. 99)
 @param itemType
    Item Type bought. (e.g. Gold Pack)
 @param itemId
    Item bought. (e.g. 1000 gold)
 @param receipt
    Transaction receipt string. (Optional, can be nil)

 @availability Available since 2.0.0

 @attribute Note! This method cannot be called before initialize method has been triggered
 */
+ (void)addBusinessEventWithCurrency:(NSString *)currency
                           amount:(NSInteger)amount
                         itemType:(NSString *)itemType
                           itemId:(NSString *)itemId
                         cartType:(NSString *)cartType
                          receipt:(NSString *)receipt;

/*!
 @method

 @abstract Add new business event

 @param currency
    Currency code in ISO 4217 format. (e.g. USD)
 @param amount
    (Integer) Amount in cents. (e.g. 99)
 @param itemType
    Item Type bought. (e.g. Gold Pack)
 @param itemId
    Item bought. (e.g. 1000 gold)
 @param autoFetchReceipt
    Should the SDK automatically fetch the transaction receipt and add it to the event

 @availability Available since 1.0.0

 @attribute Note! This method cannot be called before initialize method has been triggered
 */
+ (void)addBusinessEventWithCurrency:(NSString *)currency
                              amount:(NSInteger)amount
                            itemType:(NSString *)itemType
                              itemId:(NSString *)itemId
                            cartType:(NSString *)cartType
                    autoFetchReceipt:(BOOL)autoFetchReceipt;

/*!
 @method

 @abstract Add new resource event

 @param flowType
    Add or substract resource.<br> (See. GAResourceFlowType)
 @param currency
    One of the available currencies set in configureAvailableResourceCurrencies
 @param amount
    Amount sourced or sinked
 @param itemType
    One of the available item types set in configureAvailableResourceItemTypes
 @param itemId
    Item id (string max length=32)

 @availability Available since 2.0.0

 @attribute Note! This method cannot be called before initialize method has been triggered
 */
+ (void)addResourceEventWithFlowType:(GAResourceFlowType)flowType
                            currency:(NSString *)currency
                              amount:(NSNumber *)amount
                            itemType:(NSString *)itemType
                              itemId:(NSString *)itemId;

/*!
 @method

 @abstract Add new progression event

 @param progressionStatus
    Status of added progression.<br> (See. GAProgressionStatus)
 @param progression01
    1st progression (e.g. world01)
 @param progression02
    2nd progression (e.g. level01)
 @param progression03
    3rd progression (e.g. phase01)

 @availability Available since 1.0.0

 @attribute Note! This method cannot be called before initialize method has been triggered
 */
+ (void)addProgressionEventWithProgressionStatus:(GAProgressionStatus)progressionStatus
                                   progression01:(NSString *)progression01
                                   progression02:(NSString *)progression02
                                   progression03:(NSString *)progression03;

/*!
 @method

 @abstract Add new progression event with score

 @param progressionStatus
 Status of added progression.<br> (See. GAProgressionStatus)
 @param progression01
 1st progression (e.g. world01)
 @param progression02
 2nd progression (e.g. level01)
 @param progression03
 3rd progression (e.g. phase01)

 @availability Available since 2.0.0

 @attribute Note! This method cannot be called before initialize method has been triggered
 */
+ (void)addProgressionEventWithProgressionStatus:(GAProgressionStatus)progressionStatus
                                   progression01:(NSString *)progression01
                                   progression02:(NSString *)progression02
                                   progression03:(NSString *)progression03
                                           score:(NSInteger)score;

/*!
 @method

 @abstract Add new design event without a value

 @param eventId
    String can consist of 1 to 5 segments.<br>
    Segments are seperated by ':' and segments can have a max length of 32.<br>
    (e.g. segment1:anotherSegment:gold)

 @availability Available since 2.0.0

 @attribute Note! This method cannot be called before initialize method has been triggered
 */
+ (void)addDesignEventWithEventId:(NSString *)eventId;

/*!
 @method

 @abstract Add new design event with a value

 @param eventId
    String can consist of 1 to 5 segments.<br>
    segments are seperated by ':' and segments can have a max length of 32.<br>
    (e.g. segment1:anotherSegment:gold)
 @param value
    Number value of event

 @availability Available since 2.0.0

 @attribute Note! This method cannot be called before initialize method has been triggered
 */
+ (void)addDesignEventWithEventId:(NSString *)eventId
                            value:(NSNumber *)value;


/*!
 @method

 @abstract Add new error event

 @param severity
    Severity of error (See. GAErrorSeverity)
 @param message
    Error message (Optional, can be nil)

 @availability Available since 2.0.0

 @attribute Note! This method cannot be called before initialize method has been triggered
 */
+ (void)addErrorEventWithSeverity:(GAErrorSeverity)severity
                          message:(NSString *)message;

/*!
 @method

 @abstract Add new ad event

 @param action
 Action of ad (See. GAAdAction)
 @param adType
 Type of ad (See. GAAdType)
 @param adSdkName
 Name of ad SDK
 @param adPlacement
 Placement of ad (ad identifier)
 @param duration
 Duration the user watched ad video

 @attribute Note! This method cannot be called before initialize method has been triggered
 */
+ (void)addAdEventWithAction:(GAAdAction)action
                      adType:(GAAdType)adType
                   adSdkName:(NSString *)adSdkName
                 adPlacement:(NSString *)adPlacement
                    duration:(NSInteger)duration;

/*!
 @method

 @abstract Add new ad event

 @param action
 Action of ad (See. GAAdAction)
 @param adType
 Type of ad (See. GAAdType)
 @param adSdkName
 Name of ad SDK
 @param adPlacement
 Placement of ad (ad identifier)
 @param noAdReason
 Error reason of ad

 @attribute Note! This method cannot be called before initialize method has been triggered
 */
+ (void)addAdEventWithAction:(GAAdAction)action
                      adType:(GAAdType)adType
                   adSdkName:(NSString *)adSdkName
                 adPlacement:(NSString *)adPlacement
                    noAdReason:(GAAdError)noAdReason;

/*!
 @method

 @abstract Add new ad event

 @param action
 Action of ad (See. GAAdAction)
 @param adType
 Type of ad (See. GAAdType)
 @param adSdkName
 Name of ad SDK
 @param adPlacement
 Placement of ad (ad identifier)

 @attribute Note! This method cannot be called before initialize method has been triggered
 */
+ (void)addAdEventWithAction:(GAAdAction)action
                      adType:(GAAdType)adType
                   adSdkName:(NSString *)adSdkName
                 adPlacement:(NSString *)adPlacement;

/*!
 @method

 @abstract Add new mopub impression event

 @param impressionData
 Ad impression data

 @attribute Note! This method cannot be called before initialize method has been triggered
 */
+ (void)addImpressionMopubEventWithImpressionData:(NSDictionary *)impressionData;

/*!
 @method

 @abstract Add new impression event

 @param adNetworkName
 Name of ad network
 @param impressionData
 Ad impression data

 @attribute Note! This method cannot be called before initialize method has been triggered
 */
+ (void)addImpressionEventWithAdNetworkName:(NSString *)adNetworkName
                   impressionData:(NSDictionary *)impressionData;


/*!
 @method

 @abstract Get remote configs value as string

 @param key
 The key declared in the webtool

 @availability Available since (TBD)

 @attribute Note! This method cannot be called before initialize method has been triggered
 */
+ (NSString *)getRemoteConfigsValueAsString:(NSString*) key;


/*!
 @method

 @abstract Get remote configs value as string

 @param key
 The key declared in the webtool

 @param defaultValue
 Fallback default value for when the method does not find a value under the specified key

 @availability Available since (TBD)

 @attribute Note! This method cannot be called before initialize method has been triggered
 */
+ (NSString *) getRemoteConfigsValueAsString:(NSString *) key
                                defaultValue:(NSString *)defaultValue;

/*!
 @method

 @abstract Get remote configs configurations

 @availability Available since (TBD)

 @attribute For internal use.
 */
+ (NSString *) getRemoteConfigsContentAsString;

/*!
 @method

 @abstract Use this to set the delegate for the Remote Configs to retreive information about the status of loading configurations

 @availability Available since (TBD)
 */
+ (void) setRemoteConfigsDelegate:(id)newDelegate;

/*!
 @method

 @abstract Call for checking if remote configs values are loaded and ready

 @availability Available since (TBD)

 @attribute Note! This method should not be called before initialize method has been triggered
 */
+ (BOOL) isRemoteConfigsReady;

/*!
 @method

 @abstract Get A/B testing id

 @availability Available since (TBD)
 */
+ (NSString *) getABTestingId;

/*!
 @method

 @abstract Get A/B testing variant id

 @availability Available since (TBD)
 */
+ (NSString *) getABTestingVariantId;

/*!
 @method

 @abstract Enable info logging to console

 @param flag
 Enable or disable info log mode

 @availability Available since 2.0.0

 */
+ (void)setEnabledInfoLog:(BOOL)flag;


/*!
 @method

 @abstract Enable verbose info logging of analytics. Will output event JSON data to console.

 @param flag
 Enable or disable verbose info log mode

 @availability Available since 2.0.0

 */
+ (void)setEnabledVerboseLog:(BOOL)flag;

/*!
 @method

 @abstract Enable wanrning logging of analytics.

 @param flag
 Enable or disable watning log mode

 @availability Available since 3.2.1

 */
+ (void)setEnabledWarningLog:(BOOL)flag;


/*!
 @method

 @abstract Enable manual session handling.
 This will disable the automatic session stop/start when the app goes to background/foreground and it is then needed to call endSession & startSession manually.
 Remember to call endSession when the app is going to background.
 The first session will always be started automatically when initialize is called.

 @param flag
 Enable or disable manual session handling.

 @availability Available since 2.2.2

 */
+ (void)setEnabledManualSessionHandling:(BOOL)flag;

/*!
 @method

 @abstract Enable error reporting.
 When enabled this will automatic send error events for uncaught exceptions.

 @param flag
 Enable or disable error reporting.

 @availability Available since 3.1.0

 */
+ (void)setEnabledErrorReporting:(BOOL)flag;

/*!
 @method

 @abstract Enable/disable event submission.
 When enabled this will allow events to be sent.

 @param flag
 Enable or disable event submission.

 @availability Available since 3.2.0

 */
+ (void)setEnabledEventSubmission:(BOOL)flag;

/*!
 @method

 @abstract Start a new session.
 - if sdk is initialized
 - if manual session handling is enabled
 If a current session is currently active then it will end this session and start a new.


 @availability Available since 2.2.2

 */
+ (void)startSession;


/*!
 @method

 @abstract End an active session.
 - if sdk is initialized
 - manual session handling is enabled
 - a session is active

 @availability Available since 2.2.2

 */
+ (void)endSession;


/*!
 @method

 @abstract Set 1st custom dimension

 @param dimension01
    One of the available dimension values set in configureAvailableCustomDimensions01<br>
    Will persist cross session. Set to nil to reset.

 @availability Available since 2.0.0

 @attribute Note! Must be called after setAvailableCustomDimensions01WithCustomDimensions
 */
+ (void)setCustomDimension01:(NSString *)dimension01;

/*!
 @method

 @abstract Set 2nd custom dimension

 @param dimension02
 One of the available dimension values set in configureAvailableCustomDimensions02<br>
 Will persist cross session. Set to nil to reset.

 @availability Available since 2.0.0

 @attribute Note! Must be called after setAvailableCustomDimensions02
 */
+ (void)setCustomDimension02:(NSString *)dimension02;

/*!
 @method

 @abstract Set 3rd custom dimension

 @param dimension03
 One of the available dimension values set in configureAvailableCustomDimensions03<br>
 Will persist cross session. Set to nil to reset.

 @availability Available since 2.0.0

 @attribute Note! Must be called after setAvailableCustomDimensions03W
 */
+ (void)setCustomDimension03:(NSString *)dimension03;

/*!
 @method

 @abstract Start timer for specified key

 @param key
 Key to use to relate to the timer

 */
+ (void)startTimer:(NSString *)key;

/*!
 @method

 @abstract Pause timer for specified key

 @param key
 Key to use to relate to the timer

 */
+ (void)pauseTimer:(NSString *)key;

/*!
 @method

 @abstract Resume timer for specified key

 @param key
 Key to use to relate to the timer

 */
+ (void)resumeTimer:(NSString *)key;

/*!
 @method

 @abstract Stop timer for specified key

 @param key
 Key to use to relate to the timer

 */
+ (NSInteger)stopTimer:(NSString *)key;

@end
