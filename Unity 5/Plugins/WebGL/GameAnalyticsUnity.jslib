var GameAnalyticsUnity = {
    configureAvailableCustomDimensions01: function(list)
    {
        gameanalytics.GameAnalytics.configureAvailableCustomDimensions01(JSON.parse(Pointer_stringify(list)));
    },
    configureAvailableCustomDimensions02: function(list)
    {
        gameanalytics.GameAnalytics.configureAvailableCustomDimensions02(JSON.parse(Pointer_stringify(list)));
    },
    configureAvailableCustomDimensions03: function(list)
    {
        gameanalytics.GameAnalytics.configureAvailableCustomDimensions03(JSON.parse(Pointer_stringify(list)));
    },
    configureAvailableResourceCurrencies: function(list)
    {
        gameanalytics.GameAnalytics.configureAvailableResourceCurrencies(JSON.parse(Pointer_stringify(list)));
    },
    configureAvailableResourceItemTypes: function(list)
    {
        gameanalytics.GameAnalytics.configureAvailableResourceItemTypes(JSON.parse(Pointer_stringify(list)));
    },
    configureSdkGameEngineVersion: function(unitySdkVersion)
    {
        gameanalytics.GameAnalytics.configureSdkGameEngineVersion(Pointer_stringify(unitySdkVersion));
    },
    configureGameEngineVersion: function(unityEngineVersion)
    {
        gameanalytics.GameAnalytics.configureGameEngineVersion(Pointer_stringify(unityEngineVersion));
    },
    configureBuild: function(build)
    {
        gameanalytics.GameAnalytics.configureBuild(Pointer_stringify(build));
    },
    configureUserId: function(userId)
    {
        gameanalytics.GameAnalytics.configureUserId(Pointer_stringify(userId));
    },
    initialize: function(gamekey, gamesecret)
    {
        gameanalytics.GameAnalytics.initialize(Pointer_stringify(gamekey), Pointer_stringify(gamesecret));
    },
    setCustomDimension01: function(customDimension)
    {
        gameanalytics.GameAnalytics.setCustomDimension01(Pointer_stringify(customDimension));
    },
    setCustomDimension02: function(customDimension)
    {
        gameanalytics.GameAnalytics.setCustomDimension02(Pointer_stringify(customDimension));
    },
    setCustomDimension03: function(customDimension)
    {
        gameanalytics.GameAnalytics.setCustomDimension03(Pointer_stringify(customDimension));
    },
    addBusinessEvent: function(currency, amount, itemType, itemId, cartType)
    {
        gameanalytics.GameAnalytics.addBusinessEvent(Pointer_stringify(currency), amount, Pointer_stringify(itemType), Pointer_stringify(itemId), Pointer_stringify(cartType));
    },
    addResourceEvent: function(flowType, currency, amount, itemType, itemId)
    {
        gameanalytics.GameAnalytics.addResourceEvent(flowType, Pointer_stringify(currency), amount, Pointer_stringify(itemType), Pointer_stringify(itemId));
    },
    addProgressionEvent: function(progressionStatus, progression01, progression02, progression03)
    {
        gameanalytics.GameAnalytics.addProgressionEvent(progressionStatus, Pointer_stringify(progression01), Pointer_stringify(progression02), Pointer_stringify(progression03));
    },
    addProgressionEventWithScore: function(progressionStatus, progression01, progression02, progression03, score)
    {
        gameanalytics.GameAnalytics.addProgressionEvent(progressionStatus, Pointer_stringify(progression01), Pointer_stringify(progression02), Pointer_stringify(progression03), score);
    },
    addDesignEvent: function(eventId)
    {
        gameanalytics.GameAnalytics.addDesignEvent(Pointer_stringify(eventId));
    },
    addDesignEventWithValue: function(eventId, value)
    {
        gameanalytics.GameAnalytics.addDesignEvent(Pointer_stringify(eventId), value);
    },
    addErrorEvent: function(severity, message)
    {
        gameanalytics.GameAnalytics.addErrorEvent(severity, Pointer_stringify(message));
    },
    setEnabledInfoLog: function(enabled)
    {
        gameanalytics.GameAnalytics.setEnabledInfoLog(enabled);
    },
    setEnabledVerboseLog: function(enabled)
    {
        gameanalytics.GameAnalytics.setEnabledVerboseLog(enabled);
    },
    setManualSessionHandling: function(enabled)
    {
        gameanalytics.GameAnalytics.setEnabledManualSessionHandling(enabled);
    },
    startSession: function()
    {
        gameanalytics.GameAnalytics.startSession();
    },
    endSession: function()
    {
        gameanalytics.GameAnalytics.endSession();
    },
    setFacebookId: function(facebookId)
    {
        gameanalytics.GameAnalytics.setFacebookId(Pointer_stringify(facebookId));
    },
    setGender: function(gender)
    {
        gameanalytics.GameAnalytics.setGender(gender);
    },
    setBirthYear: function(birthYear)
    {
        gameanalytics.GameAnalytics.setBirthYear(birthYear);
    }
};

mergeInto(LibraryManager.library, GameAnalyticsUnity);
