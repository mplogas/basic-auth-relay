# basic-auth-relay

A quick and dirty basic auth ASP.NET Core sample including basic-auth-relaying to a hidden App Service on Azure. Usually, using a JWT would make more sense here, but requirements are requirements. The code is based on a sample by @dotnet-labs: https://github.com/dotnet-labs/ApiAuthDemo, but I got rid of JWT stuff and moved the Basic Auth code into a .NET Standard library.

This sample is not necessarily best practice (Basic Auth, heh) and only makes sense with the corresponding Azure PaaS architecture for this project.
