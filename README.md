[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT) [![Build Status](https://travis-ci.org/jlnovais/JN.IpFilter.svg?branch=master)](https://travis-ci.org/jlnovais/JN.IpFilter)  ![.NET Core](https://github.com/jlnovais/JN.IpFilter/workflows/.NET%20Core/badge.svg)

# JN.IpFilter
Simple IP Filter for ASP.NET Core.

Provides an IP Filter for paths exposed by the application using a white list of valid IP addresses for each path.

More details available on the [project website](https://jn-ipfilter.josenovais.com/)

## Install
Download the package from NuGet:

`Install-Package JN.IpFilter -Version [version number]`

## Usage

Use the `UseIpFilter` extension method to add the middleware inside the `Configure` method on the `Startup` class.

The `UseIpFilter` extension method needs a list of filters and an options object that can be read from configuration.

## Example

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{

    // (...)

    var filters = Configuration.GetIpFilters("IpFilters");
    var options = Configuration.GetIpFilterOptions("IpFilterMiddlewareOptions");

    app.UseIpFilter(filters, options);

    //(...)
}
```

`filters` and ` options` can be read from configuration:

```json
{

  "IpFilterMiddlewareOptions": {
    "ExactPathMatch": false,
    "LogRequests": true,
    "ApplyOnlyToHttpMethod": "" 
  },

  "IpFilters": [
    {
      "Path": "/MyController",
      "IpList": "1.1.1.1;::1"
    },
    {
      "Path": "/MyController2",
      "IpList": "*"
    },
    {
      "Path": "/MyController3",
      "IpList": "2.2.2.2"
    }
  ],
}
```
