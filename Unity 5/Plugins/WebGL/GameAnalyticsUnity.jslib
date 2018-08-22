var GameAnalyticsUnity = {
    $listener: {
        onCommandCenterUpdated: function()
        {
            SendMessage("GameAnalytics", "OnCommandCenterUpdated");
        }
    },
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
        gameanalytics.GameAnalytics.addCommandCenterListener(listener);
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
    addBusinessEvent: function(currency, amount, itemType, itemId, cartType, fields)
    {
        gameanalytics.GameAnalytics.addBusinessEvent(Pointer_stringify(currency), amount, Pointer_stringify(itemType), Pointer_stringify(itemId), Pointer_stringify(cartType)/*, JSON.parse(Pointer_stringify(fields))*/);
    },
    addResourceEvent: function(flowType, currency, amount, itemType, itemId, fields)
    {
        gameanalytics.GameAnalytics.addResourceEvent(flowType, Pointer_stringify(currency), amount, Pointer_stringify(itemType), Pointer_stringify(itemId)/*, JSON.parse(Pointer_stringify(fields))*/);
    },
    addProgressionEvent: function(progressionStatus, progression01, progression02, progression03, fields)
    {
        gameanalytics.GameAnalytics.addProgressionEvent(progressionStatus, Pointer_stringify(progression01), Pointer_stringify(progression02), Pointer_stringify(progression03)/*, JSON.parse(Pointer_stringify(fields))*/);
    },
    addProgressionEventWithScore: function(progressionStatus, progression01, progression02, progression03, score, fields)
    {
        gameanalytics.GameAnalytics.addProgressionEvent(progressionStatus, Pointer_stringify(progression01), Pointer_stringify(progression02), Pointer_stringify(progression03), score/*, JSON.parse(Pointer_stringify(fields))*/);
    },
    addDesignEvent: function(eventId, fields)
    {
        gameanalytics.GameAnalytics.addDesignEvent(Pointer_stringify(eventId)/*, JSON.parse(Pointer_stringify(fields))*/);
    },
    addDesignEventWithValue: function(eventId, value, fields)
    {
        gameanalytics.GameAnalytics.addDesignEvent(Pointer_stringify(eventId), value/*, JSON.parse(Pointer_stringify(fields))*/);
    },
    addErrorEvent: function(severity, message, fields)
    {
        gameanalytics.GameAnalytics.addErrorEvent(severity, Pointer_stringify(message)/*, JSON.parse(Pointer_stringify(fields))*/);
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
    },
    getCommandCenterValueAsString: function(key, defaultValue)
    {
        var returnStr = gameanalytics.GameAnalytics.getCommandCenterValueAsString(Pointer_stringify(key), Pointer_stringify(defaultValue));
        var buffer = _malloc(lengthBytesUTF8(returnStr) + 1);
        writeStringToMemory(returnStr, buffer);
        return buffer;
    },
    isCommandCenterReady: function()
    {
        return gameanalytics.GameAnalytics.isCommandCenterReady();
    },
    getConfigurationsContentAsString: function()
    {
        var returnStr = gameanalytics.GameAnalytics.getConfigurationsContentAsString();
        var buffer = _malloc(lengthBytesUTF8(returnStr) + 1);
        writeStringToMemory(returnStr, buffer);
        return buffer;
    }
};

autoAddDeps(GameAnalyticsUnity, '$listener');
mergeInto(LibraryManager.library, GameAnalyticsUnity);
