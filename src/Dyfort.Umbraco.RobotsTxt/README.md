# Dyfort Umbraco RobotsTxt #

Umbraco plugin return robots.txt by environment settings



## Versions ##
This package is designed to work with Umbraco 10+. [View all available versions](https://www.nuget.org/packages/Dyfort.Umbraco.RobotsTxt/#Versions).

## Installation ##
[![Nuget Downloads](https://www.nuget.org/packages/Dyfort.Umbraco.RobotsTxt)](https://www.nuget.org/packages/Dyfort.Umbraco.RobotsTxt)


```
    PM > Install-Package Dyfort.Umbraco.RobotsTxt

```

## Settings ## optional
Following settings in your appsettings.json (or similar configuration file) to dynamically generate the /robots.txt file content directly from the Umbraco CMS backoffice.

|Parameter|	Type|	Description|
|---|---|---|
|ContentKey|	Guid|	The Unique Identifier (GUID) of the Umbraco Content Node (page) that contains the robots.txt text. The package uses this ID to locate the source content.|
|FieldName|	string|	The Property Alias on the identified Content Node where the actual crawl instructions (e.g., User-agent: *, Disallow: /admin/) are stored|



```
 "RobotsTxt": {
   "ContentKey": "e8c8510a-3941-4232-bf0c-ae4f3409873f",
   "FieldName": "crawlInstructions"
  }
```