//
//  GameAnalyticsWrapper.h
//
//  Copyright (c) 2026 GameAnalytics.
//  All rights reserved.
//
//  Only for use by the GameAnalytics wrapper SDKs (Unity, Unreal, Flutter,
//  GameMaker). Don't import this from app code.
//

#if __has_include(<GameAnalytics/GameAnalytics.h>)
#import <GameAnalytics/GameAnalytics.h>
#elif __has_include(<GameAnalyticsTVOS/GameAnalytics.h>)
#import <GameAnalyticsTVOS/GameAnalytics.h>
#elif __has_include("GameAnalytics.h")
#import "GameAnalytics.h"
#else
// tvOS static drop ships the header as GameAnalyticsTVOS.h
#import "GameAnalyticsTVOS.h"
#endif

@interface GameAnalytics (Wrapper)

/*!
 @method

 @abstract Set the wrapper SDK version

 @discussion Called by a GameAnalytics wrapper SDK during its own
 initialisation, so that events carry the wrapper's version alongside the
 native one. Never call this from application code.

 <i>Example usage:</i>
 <pre><code>
 [GameAnalytics configureSdkVersion:@"unity 7.10.0"];
 </code></pre>

 @param wrapperSdkVersion
 (String)

 @attribute Note! This method must be called before initializing the SDK
 */
+ (void)configureSdkVersion:(NSString *)wrapperSdkVersion;

@end
