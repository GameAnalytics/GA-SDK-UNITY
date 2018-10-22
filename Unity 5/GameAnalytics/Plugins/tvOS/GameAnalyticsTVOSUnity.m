//
//  GameAnalyticsUnity.m
//  GA-SDK-IOS
//
//  Copyright (c) GameAnalytics. All rights reserved.
//

#import "GameAnalyticsTVOS.h"

@interface GACommandCenterUnityDelegate : NSObject<GACommandCenterDelegate>
{
}

- (void) onCommandCenterUpdated;

@end

@implementation GACommandCenterUnityDelegate

- (void)onCommandCenterUpdated {
    UnitySendMessage("GameAnalytics", "OnCommandCenterUpdated","");
}

@end

GACommandCenterUnityDelegate* ga_cc_delegate = nil;

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

    ga_cc_delegate = [[GACommandCenterUnityDelegate alloc] init];
    [GameAnalytics setCommandCenterDelegate:ga_cc_delegate];

    [GameAnalytics initializeWithGameKey:gameKeyString gameSecret:gameSecretString];
}

void addBusinessEvent(const char *currency, int amount, const char *itemType, const char *itemId, const char *cartType, const char *receipt, const char *fields) {
    NSString *currencyString = currency != NULL ? [NSString stringWithUTF8String:currency] : nil;
    NSInteger amountInteger = (NSInteger)amount;
    NSString *itemTypeString = itemType != NULL ? [NSString stringWithUTF8String:itemType] : nil;
    NSString *itemIdString = itemId != NULL ? [NSString stringWithUTF8String:itemId] : nil;
    NSString *cartTypeString = cartType != NULL ? [NSString stringWithUTF8String:cartType] : nil;
    NSString *receiptString = receipt != NULL ? [NSString stringWithUTF8String:receipt] : nil;
    NSString *fieldsString = fields != NULL ? [NSString stringWithUTF8String:fields] : nil;
    NSDictionary *fields_dict = nil;
    if (fieldsString) {
        fields_dict = [NSJSONSerialization JSONObjectWithData:[fieldsString dataUsingEncoding:NSUTF8StringEncoding] options:kNilOptions error:nil];
    }

    [GameAnalytics addBusinessEventWithCurrency:currencyString
                                         amount:amountInteger
                                       itemType:itemTypeString
                                         itemId:itemIdString
                                       cartType:cartTypeString
                                        receipt:receiptString
                                         /*fields:fields_dict*/];
}

void addBusinessEventAndAutoFetchReceipt(const char *currency, int amount, const char *itemType, const char *itemId, const char *cartType, const char *fields) {
    NSString *currencyString = currency != NULL ? [NSString stringWithUTF8String:currency] : nil;
    NSInteger amountInteger = (NSInteger)amount;
    NSString *itemTypeString = itemType != NULL ? [NSString stringWithUTF8String:itemType] : nil;
    NSString *itemIdString = itemId != NULL ? [NSString stringWithUTF8String:itemId] : nil;
    NSString *cartTypeString = cartType != NULL ? [NSString stringWithUTF8String:cartType] : nil;
    NSString *fieldsString = fields != NULL ? [NSString stringWithUTF8String:fields] : nil;
    NSDictionary *fields_dict = nil;
    if (fieldsString) {
        fields_dict = [NSJSONSerialization JSONObjectWithData:[fieldsString dataUsingEncoding:NSUTF8StringEncoding] options:kNilOptions error:nil];
    }

    [GameAnalytics addBusinessEventWithCurrency:currencyString
                                         amount:amountInteger
                                       itemType:itemTypeString
                                         itemId:itemIdString
                                       cartType:cartTypeString
                               autoFetchReceipt:TRUE
                                         /*fields:fields_dict*/];
}

void addResourceEvent(int flowType, const char *currency, float amount, const char *itemType, const char *itemId, const char *fields) {
    NSString *currencyString = currency != NULL ? [NSString stringWithUTF8String:currency] : nil;
    NSNumber *amountNumber = [NSNumber numberWithFloat:amount];
    NSString *itemTypeString = itemType != NULL ? [NSString stringWithUTF8String:itemType] : nil;
    NSString *itemIdString = itemId != NULL ? [NSString stringWithUTF8String:itemId] : nil;
    NSString *fieldsString = fields != NULL ? [NSString stringWithUTF8String:fields] : nil;
    NSDictionary *fields_dict = nil;
    if (fieldsString) {
        fields_dict = [NSJSONSerialization JSONObjectWithData:[fieldsString dataUsingEncoding:NSUTF8StringEncoding] options:kNilOptions error:nil];
    }

    [GameAnalytics addResourceEventWithFlowType:flowType
                                       currency:currencyString
                                         amount:amountNumber
                                       itemType:itemTypeString
                                         itemId:itemIdString
                                         /*fields:fields_dict*/];
}

void addProgressionEvent(int progressionStatus, const char *progression01, const char *progression02, const char *progression03, const char *fields) {
    NSString *progression01String = progression01 != NULL ? [NSString stringWithUTF8String:progression01] : nil;
    NSString *progression02String = progression02 != NULL ? [NSString stringWithUTF8String:progression02] : nil;
    NSString *progression03String = progression03 != NULL ? [NSString stringWithUTF8String:progression03] : nil;
    NSString *fieldsString = fields != NULL ? [NSString stringWithUTF8String:fields] : nil;
    NSDictionary *fields_dict = nil;
    if (fieldsString) {
        fields_dict = [NSJSONSerialization JSONObjectWithData:[fieldsString dataUsingEncoding:NSUTF8StringEncoding] options:kNilOptions error:nil];
    }

    [GameAnalytics addProgressionEventWithProgressionStatus:progressionStatus
                                              progression01:progression01String
                                              progression02:progression02String
                                              progression03:progression03String
                                                     /*fields:fields_dict*/];
}

void addProgressionEventWithScore(int progressionStatus, const char *progression01, const char *progression02, const char *progression03, int score, const char *fields) {
    NSString *progression01String = progression01 != NULL ? [NSString stringWithUTF8String:progression01] : nil;
    NSString *progression02String = progression02 != NULL ? [NSString stringWithUTF8String:progression02] : nil;
    NSString *progression03String = progression03 != NULL ? [NSString stringWithUTF8String:progression03] : nil;
    NSString *fieldsString = fields != NULL ? [NSString stringWithUTF8String:fields] : nil;
    NSDictionary *fields_dict = nil;
    if (fieldsString) {
        fields_dict = [NSJSONSerialization JSONObjectWithData:[fieldsString dataUsingEncoding:NSUTF8StringEncoding] options:kNilOptions error:nil];
    }

    [GameAnalytics addProgressionEventWithProgressionStatus:progressionStatus
                                              progression01:progression01String
                                              progression02:progression02String
                                              progression03:progression03String
                                                      score:score
                                                     /*fields:fields_dict*/];
}

void addDesignEvent(const char *eventId, const char *fields) {
    NSString *eventIdString = eventId != NULL ? [NSString stringWithUTF8String:eventId] : nil;
    NSString *fieldsString = fields != NULL ? [NSString stringWithUTF8String:fields] : nil;
    NSDictionary *fields_dict = nil;
    if (fieldsString) {
        fields_dict = [NSJSONSerialization JSONObjectWithData:[fieldsString dataUsingEncoding:NSUTF8StringEncoding] options:kNilOptions error:nil];
    }

    [GameAnalytics addDesignEventWithEventId:eventIdString value:nil fields:fields_dict];
}

void addDesignEventWithValue(const char *eventId, float value, const char *fields) {
    NSString *eventIdString = eventId != NULL ? [NSString stringWithUTF8String:eventId] : nil;
    NSNumber *valueNumber = [NSNumber numberWithFloat:value];
    NSString *fieldsString = fields != NULL ? [NSString stringWithUTF8String:fields] : nil;
    NSDictionary *fields_dict = nil;
    if (fieldsString) {
        fields_dict = [NSJSONSerialization JSONObjectWithData:[fieldsString dataUsingEncoding:NSUTF8StringEncoding] options:kNilOptions error:nil];
    }

    [GameAnalytics addDesignEventWithEventId:eventIdString value:valueNumber /*fields:fields_dict*/];
}

void addErrorEvent(int severity, const char *message, const char *fields) {
    NSString *messageString = message != NULL ? [NSString stringWithUTF8String:message] : nil;
    NSString *fieldsString = fields != NULL ? [NSString stringWithUTF8String:fields] : nil;
    NSDictionary *fields_dict = nil;
    if (fieldsString) {
        fields_dict = [NSJSONSerialization JSONObjectWithData:[fieldsString dataUsingEncoding:NSUTF8StringEncoding] options:kNilOptions error:nil];
    }

    [GameAnalytics addErrorEventWithSeverity:severity message:messageString /*fields:fields_dict*/];
}

void setEnabledInfoLog(BOOL flag) {
    [GameAnalytics setEnabledInfoLog:flag];
}

void setEnabledVerboseLog(BOOL flag) {
    [GameAnalytics setEnabledVerboseLog:flag];
}

void setManualSessionHandling(BOOL flag) {
    [GameAnalytics setEnabledManualSessionHandling:flag];
}

void gameAnalyticsStartSession() {
    [GameAnalytics startSession];
}

void gameAnalyticsEndSession() {
    [GameAnalytics endSession];
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
    NSInteger birthYearInteger = birthYear != 0 ? (NSInteger)birthYear : 0;
    [GameAnalytics setBirthYear:birthYearInteger];
}

char* cStringCopy(const char* string)
{
    if (string == NULL)
        return NULL;

    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);

    return res;
}

char* getCommandCenterValueAsString(const char *key, const char *defaultValue) {
    NSString *keyString = key != NULL ? [NSString stringWithUTF8String:key] : nil;
    NSString *defaultValueString = defaultValue != NULL ? [NSString stringWithUTF8String:defaultValue] : nil;
    NSString *result = [GameAnalytics getCommandCenterValueAsString:keyString defaultValue:defaultValueString];

    return cStringCopy([result UTF8String]);
}

BOOL isCommandCenterReady() {
    return [GameAnalytics isCommandCenterReady];
}

char* getConfigurationsContentAsString() {
    NSString *result = [GameAnalytics getCommandCenterConfigurations];
    return cStringCopy([result UTF8String]);
}
