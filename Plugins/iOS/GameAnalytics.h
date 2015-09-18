//
//  GameAnalytics.h
//  GA-SDK-IOS
//
//  Copyright (c) 2015 GameAnalytics. All rights reserved.
//

/*!
 @header GameAnalytics BETA iOS SDK
 @discussion
 <h2>What is GameAnalytics?</h2>
 <p>GameAnalytics is a cloud-hosted solution for tracking, analysis and reporting of game metrics. You can use the services provided by GameAnalytics to store your game-related data directly in the cloud and process, visualize, and analyze it on the fly.</p>

 <h3>Event types</h3>
 <p>We provide a wide range of event types that you can instrument in your game to track the data you're interested in building your analysis from. Here's a brief overview of their uses:</p>
 <p><b>Business</b></p>
 <p>Track any real money transaction in-game. Additionally fetch and attach the in-app purchase receipt.</p>
 <p><b>Resource</b></p>
 <p>Analyze your in-game economy (virtual currencies). You will be able to see the flow (sink/source) for each virtual currency.</p>
 <p><b>Progression</b></p>
 <p>Measure player progression in the game. It follows a 3 hierarchy structure (world, level and phase) to indicate a player's path or place.</p>
 <p><b>Error</b></p>
 <p>Set up custom error events in the game. You can group the events by severity level and attach a message.</p>
 <p><b>Design</b></p>
 <p>Track any type of design event that you want to measure i.e. GUI elements or tutorial steps. <i>Custom dimensions are not supported.</i></p>
  
 <h3>Download and installation</h3>
 <p>Download the latest version of the GameAnalytics SDK. Add these files to the Xcode project folder:</p>
 <ul>
 <li>GameAnalytics.h - The required header file containing methods for GameAnalytics</li>
 <li>libGameAnalytics.a - The required static library for GameAnalytics</li>
 </ul>

 <h2>Setup</h2>
 <p>In your project "Build Phases" add the following to "Link Binary With Libraries":</p>
 <ul>
 <li>AdSupport.framework</li>
 <li>SystemConfiguration.framework</li>
 <li>libsqlite3.dylib</li>
 <li>libz.dylib</li>
 <li>libGameAnalytics.a (if not already present)</li>
 </ul>

 <h2>Usage</h2>
 <p>You can now use the SDK by adding this in your precompiled header or other places where it's needed:<p>
 <code>#import "GameAnalytics.h"</code>

 <h4>SDK initialize flow</h4>
 <p>GameAnalytics will be activated once initialization method is called. Some methods can only be called <b>before</b> and others <b>after</b> the initialize call.</p>
 <ul>
 <li>Call configure methods - specifying build, valid dimensions...</li>
 <li>Call initialize with the game key and secret key</li>
 <li>Call post-initialize methods - adding events or changing current dimensions.</li>
 </ul>

 <h3>Sessions</h3>
 <p>A new session is started when:</p>
 <ul>
 <li>The SDK is being <b>initialized</b> (game launch)</li>
 <li>When the app is <b>resuming from background</b></li>
 </ul>
 When the app is <b>going to background</b> it will attempt to send a "session_end" event. Sometimes the "session_end" submission is interrupted if the device is offline or there is not enough time to send it. The next time the game launches it will detect this missing "session_end" and send it for the previous session.</p>
 
 <p><b>Please note:</b> When developing locally the simulator/device will stop the app instantly and the "session_end" is often not sent. Therefore when developing locally there might be a "session_end" being submitted almost each time the game is started.</p>

 <h3>Logging</h3>
 <p>When implementing the SDK in Xcode the console will output information about what is going on.</p>

 <p>These log types will always be available in the log:</p>
 <ul>
 <li><b>Warning</b> - non-critical unexpected behavior like parameter validation fail.</li>
 <li><b>Error</b> - critical errors within the SDK that should never happen and will cause it to disable.</li>
 </ul>

 <p>These log types need to be activated:</p>
 <ul>
 <li><b>InfoLog</b> - Compact information output when actions are performed by the SDK, i.e. adding/sending events. Use this when implementing the SDK.</li>
 <li><b>VerboseLog</b> - This will print the full JSON data that will be submitted to our servers.</li>
 </ul>

 <h3>Performance and threading</h3>
 <p>Every action performed by the SDK (like adding an event) is added into a low-priority thread queue.</p>
 <p>This will prevent the SDK blocking the main thread. Each added task will be queued for processing in the same order it was added.</p>

 <h2>Example</h2>
 <p>This is a short example of a typical instrumentation.</p>
 
 <h3>Initialize the SDK</h3>
 <p>You can place this code in the AppDelegate class in the method "applicationDidFinishLaunchingWithOptions".</p>

 <pre><code>
 // Enable log
 [GameAnalytics setEnabledInfoLog:YES];
 [GameAnalytics setEnabledVerboseLog:NO];

 // Configure available virtual currencies and item types
 [GameAnalytics configureAvailableResourceCurrencies:@[@"gems", @"gold"]];
 [GameAnalytics configureAvailableResourceItemTypes:@[@"boost", @"lives", @"weapon"]];

 // Configure available custom dimensions
 [GameAnalytics configureAvailableCustomDimensions01:@[@"ninja", @"samurai"]];
 [GameAnalytics configureAvailableCustomDimensions02:@[@"whale", @"dolphin"]];
 [GameAnalytics configureAvailableCustomDimensions03:@[@"horde", @"alliance"]];

 // Configure build version
 [GameAnalytics configureBuild:@"0.1.0"];

 // Initialize
 [GameAnalytics initializeWithGameKey:@"[game key]" gameSecret:@"[secret key]"];
 </code></pre>

 <p><b>Start instrumentation</b></p>
 <p>The methods for adding events become active once the SDK initialization has been called.</p>
 <p>The following is a list of one-liner examples. When implemented in a game they should be called at different parts of the code where the specific action is happening.</p>

 <pre><code>
  // Set dimension (will persist cross session)
 [GameAnalytics setCustomDimension01:@"ninja"];

 // Set progression start (e.g. level start)
 [GameAnalytics addProgressionEventWithProgressionStatus:GAProgressionStatusStart progression01:@"world_01" progression02:@"level_01" progression03:@"wave_01"];

 // Submit virtual currency event
 [GameAnalytics addResourceEventWithFlowType:GAResourceFlowTypeSource currency:@"gold" amount:@999 itemType:@"weapon" itemId:@"sword_of_justice"];
 // Submit custom design event
 [GameAnalytics addDesignEventWithEventId:@"killed:metal_robot" value:@200];
 // Submit error event
 [GameAnalytics addErrorEventWithSeverity:GAErrorSeverityWarning message:@"warning detected in user object code. line 606."];

 // Set progression complete (e.g. level end / score screen)
 [GameAnalytics addProgressionEventWithProgressionStatus:GAProgressionStatusComplete progression01:@"world_01" progression02:@"level_01" progression03:@"wave_01" score:20000];

 </code></pre>

 */

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
 his enum is used to specify status for progression event
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
 his enum is used to specify severity of an error event
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
    (Array max length=20, String max length=32)
 
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

/* @IF WRAPPER */
/* 
 Used ONLY by GameAnalytics wrapper SDK's (for example Unity).
 Never call this manually!
 */
+ (void)configureSdkVersion:(NSString *)wrapperSdkVersion;
/* @ENDIF UNITY */

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

 @abstract Initialize GameAnalytics SDK
 
 @discussion <i>Example usage:</i>
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
    One of the available currencies set in configureAvailableResourceItemTypes
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
 
 @abstract Enable info logging to console
 
 @param flag
    Enable or disable info log mode
 
 @availability Available since 2.0.0
 
*/
+ (void)setEnabledInfoLog:(BOOL)flag;

/*!
 @method
 
 @abstract Enable verbose info logging of event JSON data to console
 
 @param flag
 Enable or disable verbose info log mode
 
 @availability Available since 2.0.0
 
 */
+ (void)setEnabledVerboseLog:(BOOL)flag;

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
 
 @abstract Set user facebook id
 
 @param facebookId
    Facebook id of user (Persists cross session)
 
 @availability Available since 2.0.0
 */
+ (void)setFacebookId:(NSString *)facebookId;

/*!
 @method
 
 @abstract Set user gender
 
 @param gender
    Gender of user (Persists cross session)<br>
    Must be one of (male / female)
 
 @availability Available since 2.0.0
 */
+ (void)setGender:(NSString *)gender;

/*!
 @method
 
 @abstract Set user birth year
 
 @param birthYear
    Birth year of user (Persists cross session)
 
 @availability Available since 2.0.0
 */
+ (void)setBirthYear:(NSInteger)birthYear;

@end
