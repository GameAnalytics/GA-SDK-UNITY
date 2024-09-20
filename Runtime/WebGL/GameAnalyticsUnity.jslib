var GameAnalyticsUnity = {
    $listener: {
        onRemoteConfigsUpdated: function()
        {
            SendMessage("GameAnalytics", "OnRemoteConfigsUpdated");
        }
    },
    configureAvailableCustomDimensions01: function(list)
    {
        gameanalytics.GameAnalytics.configureAvailableCustomDimensions01(JSON.parse(UTF8ToString(list)));
    },
    configureAvailableCustomDimensions02: function(list)
    {
        gameanalytics.GameAnalytics.configureAvailableCustomDimensions02(JSON.parse(UTF8ToString(list)));
    },
    configureAvailableCustomDimensions03: function(list)
    {
        gameanalytics.GameAnalytics.configureAvailableCustomDimensions03(JSON.parse(UTF8ToString(list)));
    },
    configureAvailableResourceCurrencies: function(list)
    {
        gameanalytics.GameAnalytics.configureAvailableResourceCurrencies(JSON.parse(UTF8ToString(list)));
    },
    configureAvailableResourceItemTypes: function(list)
    {
        gameanalytics.GameAnalytics.configureAvailableResourceItemTypes(JSON.parse(UTF8ToString(list)));
    },
    configureSdkGameEngineVersion: function(unitySdkVersion)
    {
        gameanalytics.GameAnalytics.configureSdkGameEngineVersion(UTF8ToString(unitySdkVersion));
    },
    configureGameEngineVersion: function(unityEngineVersion)
    {
        gameanalytics.GameAnalytics.configureGameEngineVersion(UTF8ToString(unityEngineVersion));
    },
    configureBuild: function(build)
    {
        gameanalytics.GameAnalytics.configureBuild(UTF8ToString(build));
    },
    configureUserId: function(userId)
    {
        gameanalytics.GameAnalytics.configureUserId(UTF8ToString(userId));
    },
    initialize: function(gamekey, gamesecret)
    {
        gameanalytics.GameAnalytics.addRemoteConfigsListener(listener);
        gameanalytics.GameAnalytics.initialize(UTF8ToString(gamekey), UTF8ToString(gamesecret));
    },
    setCustomDimension01: function(customDimension)
    {
        gameanalytics.GameAnalytics.setCustomDimension01(UTF8ToString(customDimension));
    },
    setCustomDimension02: function(customDimension)
    {
        gameanalytics.GameAnalytics.setCustomDimension02(UTF8ToString(customDimension));
    },
    setCustomDimension03: function(customDimension)
    {
        gameanalytics.GameAnalytics.setCustomDimension03(UTF8ToString(customDimension));
    },
    setGlobalCustomEventFields: function(customFields)
    {
        gameanalytics.GameAnalytics.setGlobalCustomEventFields(JSON.parse(customFields));
    },
    addBusinessEvent: function(currency, amount, itemType, itemId, cartType, fields, mergeFields)
    {
        var fieldsString = UTF8ToString(fields);
        fieldsString = fieldsString ? fieldsString : "{}";
        gameanalytics.GameAnalytics.addBusinessEvent(UTF8ToString(currency), amount, UTF8ToString(itemType), UTF8ToString(itemId), UTF8ToString(cartType), JSON.parse(fieldsString), mergeFields);
    },
    addResourceEvent: function(flowType, currency, amount, itemType, itemId, fields, mergeFields)
    {
        var fieldsString = UTF8ToString(fields);
        fieldsString = fieldsString ? fieldsString : "{}";
        gameanalytics.GameAnalytics.addResourceEvent(flowType, UTF8ToString(currency), amount, UTF8ToString(itemType), UTF8ToString(itemId), JSON.parse(fieldsString), mergeFields);
    },
    addProgressionEvent: function(progressionStatus, progression01, progression02, progression03, fields, mergeFields)
    {
        var fieldsString = UTF8ToString(fields);
        fieldsString = fieldsString ? fieldsString : "{}";
        gameanalytics.GameAnalytics.addProgressionEvent(progressionStatus, UTF8ToString(progression01), UTF8ToString(progression02), UTF8ToString(progression03), JSON.parse(fieldsString), mergeFields);
    },
    addProgressionEventWithScore: function(progressionStatus, progression01, progression02, progression03, score, fields, mergeFields)
    {
        var fieldsString = UTF8ToString(fields);
        fieldsString = fieldsString ? fieldsString : "{}";
        gameanalytics.GameAnalytics.addProgressionEvent(progressionStatus, UTF8ToString(progression01), UTF8ToString(progression02), UTF8ToString(progression03), score, JSON.parse(fieldsString), mergeFields);
    },
    addDesignEvent: function(eventId, fields, mergeFields)
    {
        var fieldsString = UTF8ToString(fields);
        fieldsString = fieldsString ? fieldsString : "{}";
        gameanalytics.GameAnalytics.addDesignEvent(UTF8ToString(eventId), JSON.parse(fieldsString), mergeFields);
    },
    addDesignEventWithValue: function(eventId, value, fields, mergeFields)
    {
        var fieldsString = UTF8ToString(fields);
        fieldsString = fieldsString ? fieldsString : "{}";
        gameanalytics.GameAnalytics.addDesignEvent(UTF8ToString(eventId), value, JSON.parse(fieldsString), mergeFields);
    },
    addErrorEvent: function(severity, message, fields, mergeFields)
    {
        var fieldsString = UTF8ToString(fields);
        fieldsString = fieldsString ? fieldsString : "{}";
        gameanalytics.GameAnalytics.addErrorEvent(severity, UTF8ToString(message), JSON.parse(fieldsString), mergeFields);
    },
        addAdEventWithDuration: function (adAction, adType, adSdkName, adPlacement, duration, fields, mergeFields) {
        var fieldsString = UTF8ToString(fields);
        fieldsString = fieldsString ? fieldsString : "{}";
        gameanalytics.GameAnalytics.addAdEventWithDuration(adAction, adType, UTF8ToString(adSdkName), UTF8ToString(adPlacement), duration, JSON.parse(fieldsString), mergeFields);
    },
    addAdEventWithReason: function (adAction, adType, adSdkName, adPlacement, noAdReason, fields, mergeFields) {
        var fieldsString = UTF8ToString(fields);
        fieldsString = fieldsString ? fieldsString : "{}";
        gameanalytics.GameAnalytics.addAdEventWithNoAdReason(adAction, adType, UTF8ToString(adSdkName), UTF8ToString(adPlacement), noAdReason, JSON.parse(fieldsString), mergeFields);
    },
    addAdEvent: function (adAction, adType, adSdkName, adPlacement, fields, mergeFields) {
        var fieldsString = UTF8ToString(fields);
        fieldsString = fieldsString ? fieldsString : "{}";
        gameanalytics.GameAnalytics.addAdEvent(adAction, adType, UTF8ToString(adSdkName), UTF8ToString(adPlacement), JSON.parse(fieldsString), mergeFields);
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
    setEventSubmission: function(enabled)
    {
        gameanalytics.GameAnalytics.setEnabledEventSubmission(enabled);
    },
    startSession: function()
    {
        gameanalytics.GameAnalytics.startSession();
    },
    endSession: function()
    {
        gameanalytics.GameAnalytics.endSession();
    },
    getRemoteConfigsValueAsString: function(key, defaultValue)
    {
        var returnStr = gameanalytics.GameAnalytics.getRemoteConfigsValueAsString(UTF8ToString(key), UTF8ToString(defaultValue));
        var buffer = allocateStringBuffer(returnStr);
        return buffer;
    },
    isRemoteConfigsReady: function()
    {
        return gameanalytics.GameAnalytics.isRemoteConfigsReady();
    },
    getRemoteConfigsContentAsString: function()
    {
        var returnStr = gameanalytics.GameAnalytics.getRemoteConfigsContentAsString();
        var buffer = allocateStringBuffer(returnStr);
        return buffer;
    },
    getABTestingId: function()
    {
        var returnStr = gameanalytics.GameAnalytics.getABTestingId();
        var buffer = allocateStringBuffer(returnStr);
        return buffer;
    },
    getABTestingVariantId: function()
    {
        var returnStr = gameanalytics.GameAnalytics.getABTestingVariantId();
        var buffer = allocateStringBuffer(returnStr);
        return buffer;
    }
};

autoAddDeps(GameAnalyticsUnity, '$listener');
mergeInto(LibraryManager.library, GameAnalyticsUnity);
