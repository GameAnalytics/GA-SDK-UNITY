How to setup the GameAnalytics Unity SDK for iOS:

When you build your project for iOS, XCode may complain about some missing imports. To fix this go to the Build Phases tab of your project inside XCode, and fold out the Link Binary With Libraries section. Click the (+) button and add the AdSupport.framework.

(The above setup is designed for games that show ads (either using GameAnalytics built-in Ad Support, or another ad service). If your game does not show ads please refer to our online documentation for additional information: http://support.gameanalytics.com/hc/en-us/articles/200841426-Building-for-iOS-in-Unity.)