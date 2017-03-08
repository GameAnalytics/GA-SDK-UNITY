var GameAnalyticsUnity = {
    configureAvailableCustomDimensions01: function(list)
    {
        ga.GameAnalytics.configureAvailableCustomDimensions01(JSON.parse(Pointer_stringify(list)));
    },
    configureAvailableCustomDimensions02: function(list)
    {
        ga.GameAnalytics.configureAvailableCustomDimensions02(JSON.parse(Pointer_stringify(list)));
    },
    configureAvailableCustomDimensions03: function(list)
    {
        ga.GameAnalytics.configureAvailableCustomDimensions03(JSON.parse(Pointer_stringify(list)));
    },
    configureAvailableResourceCurrencies: function(list)
    {
        ga.GameAnalytics.configureAvailableResourceCurrencies(JSON.parse(Pointer_stringify(list)));
    },
    configureAvailableResourceItemTypes: function(list)
    {
        ga.GameAnalytics.configureAvailableResourceItemTypes(JSON.parse(Pointer_stringify(list)));
    },
    configureSdkGameEngineVersion: function(unitySdkVersion)
    {
        ga.GameAnalytics.configureSdkGameEngineVersion(Pointer_stringify(unitySdkVersion));
    },
    configureGameEngineVersion: function(unityEngineVersion)
    {
        ga.GameAnalytics.configureGameEngineVersion(Pointer_stringify(unityEngineVersion));
    },
    configureBuild: function(build)
    {
        ga.GameAnalytics.configureBuild(Pointer_stringify(build));
    },
    configureUserId: function(userId)
    {
        ga.GameAnalytics.configureUserId(Pointer_stringify(userId));
    },
    initialize: function(gamekey, gamesecret)
    {
        ga.GameAnalytics.initialize(Pointer_stringify(gamekey), Pointer_stringify(gamesecret));
    },
    setCustomDimension01: function(customDimension)
    {
        ga.GameAnalytics.setCustomDimension01(Pointer_stringify(customDimension));
    },
    setCustomDimension02: function(customDimension)
    {
        ga.GameAnalytics.setCustomDimension02(Pointer_stringify(customDimension));
    },
    setCustomDimension03: function(customDimension)
    {
        ga.GameAnalytics.setCustomDimension03(Pointer_stringify(customDimension));
    },
    addBusinessEvent: function(currency, amount, itemType, itemId, cartType)
    {
        ga.GameAnalytics.addBusinessEvent(Pointer_stringify(currency), amount, Pointer_stringify(itemType), Pointer_stringify(itemId), Pointer_stringify(cartType));
    },
    addResourceEvent: function(flowType, currency, amount, itemType, itemId)
    {
        ga.GameAnalytics.addResourceEvent(flowType, Pointer_stringify(currency), amount, Pointer_stringify(itemType), Pointer_stringify(itemId));
    },
    addProgressionEvent: function(progressionStatus, progression01, progression02, progression03)
    {
        ga.GameAnalytics.addProgressionEvent(progressionStatus, Pointer_stringify(progression01), Pointer_stringify(progression02), Pointer_stringify(progression03));
    },
    addProgressionEventWithScore: function(progressionStatus, progression01, progression02, progression03, score)
    {
        ga.GameAnalytics.addProgressionEvent(progressionStatus, Pointer_stringify(progression01), Pointer_stringify(progression02), Pointer_stringify(progression03), score);
    },
    addDesignEvent: function(eventId)
    {
        ga.GameAnalytics.addDesignEvent(Pointer_stringify(eventId));
    },
    addDesignEventWithValue: function(eventId, value)
    {
        ga.GameAnalytics.addDesignEvent(Pointer_stringify(eventId), value);
    },
    addErrorEvent: function(severity, message)
    {
        ga.GameAnalytics.addErrorEvent(severity, Pointer_stringify(message));
    },
    setEnabledInfoLog: function(enabled)
    {
        ga.GameAnalytics.setEnabledInfoLog(enabled);
    },
    setEnabledVerboseLog: function(enabled)
    {
        ga.GameAnalytics.setEnabledVerboseLog(enabled);
    },
    setManualSessionHandling: function(enabled)
    {
        ga.GameAnalytics.setEnabledManualSessionHandling(enabled);
    },
    startSession: function()
    {
        ga.GameAnalytics.startSession();
    },
    endSession: function()
    {
        ga.GameAnalytics.endSession();
    },
    setFacebookId: function(facebookId)
    {
        ga.GameAnalytics.setFacebookId(Pointer_stringify(facebookId));
    },
    setGender: function(gender)
    {
        ga.GameAnalytics.setGender(gender);
    },
    setBirthYear: function(birthYear)
    {
        ga.GameAnalytics.setBirthYear(birthYear);
    }
};

mergeInto(LibraryManager.library, GameAnalyticsUnity);
