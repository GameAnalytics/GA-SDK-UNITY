//
//  GameAnalyticsWrapper.m
//  GA-SDK-IOS
//
//  Copyright (c) GameAnalytics. All rights reserved.
//

#import "GameAnalytics.h"


void configureAvailableCustomDimensions01(const char *list) {
    NSString *list_string = list != NULL ? [NSString stringWithUTF8String:list] : nil;
    NSArray *list_array = nil;
    if (list_string) {
        list_array = [NSJSONSerialization JSONObjectWithData:[list_string dataUsingEncoding:NSUTF8StringEncoding]
                                                     options:kNilOptions
                                                       error:nil];
    }
    [GameAnalytics configureAvailableCustomDimensions01:list_array];
}

void configureAvailableCustomDimensions02(const char *list) {
    NSString *list_string = list != NULL ? [NSString stringWithUTF8String:list] : nil;
    NSArray *list_array = nil;
    if (list_string) {
        list_array = [NSJSONSerialization JSONObjectWithData:[list_string dataUsingEncoding:NSUTF8StringEncoding]
                                                     options:kNilOptions
                                                       error:nil];
    }
    [GameAnalytics configureAvailableCustomDimensions02:list_array];
}

void configureAvailableCustomDimensions03(const char *list) {
    NSString *list_string = list != NULL ? [NSString stringWithUTF8String:list] : nil;
    NSArray *list_array = nil;
    if (list_string) {
        list_array = [NSJSONSerialization JSONObjectWithData:[list_string dataUsingEncoding:NSUTF8StringEncoding]
                                                     options:kNilOptions
                                                       error:nil];
    }
    [GameAnalytics configureAvailableCustomDimensions03:list_array];
}

void configureAvailableResourceCurrencies(const char *list) {
    NSString *list_string = list != NULL ? [NSString stringWithUTF8String:list] : nil;
    NSArray *list_array = nil;
    if (list_string) {
        list_array = [NSJSONSerialization JSONObjectWithData:[list_string dataUsingEncoding:NSUTF8StringEncoding]
                                                     options:kNilOptions
                                                       error:nil];
    }
    [GameAnalytics configureAvailableResourceCurrencies:list_array];
}

void configureAvailableResourceItemTypes(const char *list) {
    NSString *list_string = list != NULL ? [NSString stringWithUTF8String:list] : nil;
    NSArray *list_array = nil;
    if (list_string) {
        list_array = [NSJSONSerialization JSONObjectWithData:[list_string dataUsingEncoding:NSUTF8StringEncoding]
                                                     options:kNilOptions
                                                       error:nil];
    }
    [GameAnalytics configureAvailableResourceItemTypes:list_array];
}

void configureSdkGameEngineVersion(const char *gameEngineSdkVersion) {
    NSString *gameEngineSdkVersionString = gameEngineSdkVersion != NULL ? [NSString stringWithUTF8String:gameEngineSdkVersion] : nil;
    [GameAnalytics configureSdkVersion:gameEngineSdkVersionString];
}

void configureGameEngineVersion(const char *gameEngineVersion) {
    NSString *gameEngineVersionString = gameEngineVersion != NULL ? [NSString stringWithUTF8String:gameEngineVersion] : nil;
    [GameAnalytics configureEngineVersion:gameEngineVersionString];
}

void configureBuild(const char *build) {
    NSString *buildString = build != NULL ? [NSString stringWithUTF8String:build] : nil;
    [GameAnalytics configureBuild:buildString];
}

void configureUserId(const char *userId) {
    NSString *userIdString = userId != NULL ? [NSString stringWithUTF8String:userId] : nil;
    [GameAnalytics configureUserId:userIdString];
}

void initialize(const char *gameKey, const char *gameSecret) {
    NSString *gameKeyString = gameKey != NULL ? [NSString stringWithUTF8String:gameKey] : nil;
    NSString *gameSecretString = gameSecret != NULL ? [NSString stringWithUTF8String:gameSecret] : nil;
    [GameAnalytics initializeWithGameKey:gameKeyString gameSecret:gameSecretString];
}

void addBusinessEvent(const char *currency, int amount, const char *itemType, const char *itemId, const char *cartType, const char *receipt) {
    NSString *currencyString = currency != NULL ? [NSString stringWithUTF8String:currency] : nil;
    NSInteger amountInteger = (NSInteger)amount;
    NSString *itemTypeString = itemType != NULL ? [NSString stringWithUTF8String:itemType] : nil;
    NSString *itemIdString = itemId != NULL ? [NSString stringWithUTF8String:itemId] : nil;
    NSString *cartTypeString = cartType != NULL ? [NSString stringWithUTF8String:cartType] : nil;
    NSString *receiptString = receipt != NULL ? [NSString stringWithUTF8String:receipt] : nil;

    [GameAnalytics addBusinessEventWithCurrency:currencyString
                                         amount:amountInteger
                                       itemType:itemTypeString
                                         itemId:itemIdString
                                       cartType:cartTypeString
                                        receipt:receiptString];
}

void addBusinessEventAndAutoFetchReceipt(const char *currency, int amount, const char *itemType, const char *itemId, const char *cartType) {
    NSString *currencyString = currency != NULL ? [NSString stringWithUTF8String:currency] : nil;
    NSInteger amountInteger = (NSInteger)amount;
    NSString *itemTypeString = itemType != NULL ? [NSString stringWithUTF8String:itemType] : nil;
    NSString *itemIdString = itemId != NULL ? [NSString stringWithUTF8String:itemId] : nil;
    NSString *cartTypeString = cartType != NULL ? [NSString stringWithUTF8String:cartType] : nil;

    [GameAnalytics addBusinessEventWithCurrency:currencyString
                                         amount:amountInteger
                                       itemType:itemTypeString
                                         itemId:itemIdString
                                       cartType:cartTypeString
                               autoFetchReceipt:TRUE];
}

void addResourceEvent(int flowType, const char *currency, float amount, const char *itemType, const char *itemId) {
    NSString *currencyString = currency != NULL ? [NSString stringWithUTF8String:currency] : nil;
    NSNumber *amountNumber = [NSNumber numberWithFloat:amount];
    NSString *itemTypeString = itemType != NULL ? [NSString stringWithUTF8String:itemType] : nil;
    NSString *itemIdString = itemId != NULL ? [NSString stringWithUTF8String:itemId] : nil;
    
    [GameAnalytics addResourceEventWithFlowType:flowType
                                       currency:currencyString
                                         amount:amountNumber
                                       itemType:itemTypeString
                                         itemId:itemIdString];
}

void addProgressionEvent(int progressionStatus, const char *progression01, const char *progression02, const char *progression03) {
    NSString *progression01String = progression01 != NULL ? [NSString stringWithUTF8String:progression01] : nil;
    NSString *progression02String = progression02 != NULL ? [NSString stringWithUTF8String:progression02] : nil;
    NSString *progression03String = progression03 != NULL ? [NSString stringWithUTF8String:progression03] : nil;
    
    [GameAnalytics addProgressionEventWithProgressionStatus:progressionStatus
                                              progression01:progression01String
                                              progression02:progression02String
                                              progression03:progression03String];
}

void addProgressionEventWithScore(int progressionStatus, const char *progression01, const char *progression02, const char *progression03, int score) {
    NSString *progression01String = progression01 != NULL ? [NSString stringWithUTF8String:progression01] : nil;
    NSString *progression02String = progression02 != NULL ? [NSString stringWithUTF8String:progression02] : nil;
    NSString *progression03String = progression03 != NULL ? [NSString stringWithUTF8String:progression03] : nil;

    [GameAnalytics addProgressionEventWithProgressionStatus:progressionStatus
                                              progression01:progression01String
                                              progression02:progression02String
                                              progression03:progression03String
                                                      score:score];
}

void addDesignEvent(const char *eventId) {
    NSString *eventIdString = eventId != NULL ? [NSString stringWithUTF8String:eventId] : nil;

    [GameAnalytics addDesignEventWithEventId:eventIdString value:nil];
}

void addDesignEventWithValue(const char *eventId, float value) {
    NSString *eventIdString = eventId != NULL ? [NSString stringWithUTF8String:eventId] : nil;
    NSNumber *valueNumber = [NSNumber numberWithFloat:value];

    [GameAnalytics addDesignEventWithEventId:eventIdString value:valueNumber];
}

void addErrorEvent(int severity, const char *message) {
    NSString *messageString = message != NULL ? [NSString stringWithUTF8String:message] : nil;
    
    [GameAnalytics addErrorEventWithSeverity:severity message:messageString];
}

void setEnabledInfoLog(BOOL flag) {
    [GameAnalytics setEnabledInfoLog:flag];
}

void setEnabledVerboseLog(BOOL flag) {
    [GameAnalytics setEnabledVerboseLog:flag];
}

void setCustomDimension01(const char *customDimension) {
    NSString *customDimensionString = customDimension != NULL ? [NSString stringWithUTF8String:customDimension] : nil;
    [GameAnalytics setCustomDimension01:customDimensionString];
}

void setCustomDimension02(const char *customDimension) {
    NSString *customDimensionString = customDimension != NULL ? [NSString stringWithUTF8String:customDimension] : nil;
    [GameAnalytics setCustomDimension02:customDimensionString];
}

void setCustomDimension03(const char *customDimension) {
    NSString *customDimensionString = customDimension != NULL ? [NSString stringWithUTF8String:customDimension] : nil;
    [GameAnalytics setCustomDimension03:customDimensionString];
}

void setFacebookId(const char *facebookId) {
    NSString *facebookIdString = facebookId != NULL ? [NSString stringWithUTF8String:facebookId] : nil;
    [GameAnalytics setFacebookId:facebookIdString];
}

void setGender(const char *gender) {
    NSString *genderString = gender != NULL ? [NSString stringWithUTF8String:gender] : nil;
    [GameAnalytics setGender:genderString];
}

void setBirthYear(int birthYear) {
    NSInteger birthYearInteger = birthYear != NULL ? (NSInteger)birthYear : nil;
    [GameAnalytics setBirthYear:birthYearInteger];
}
