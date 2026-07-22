# Orion-Raknet

Standalone RakNet UDP transport for Bedrock-compatible servers.

NuGet: [`Orion.RakNet`](https://www.nuget.org/packages/Orion.RakNet)

## Layout

```
Orion-Raknet/
  README.md
  src/Orion.RakNet/
    Orion.RakNet.csproj
    NetworkServer.cs
    NetworkConnection.cs
    NetworkServerConnection.cs
    RaknetServerOptions.cs
    Packets/
```

## Defaults

`RaknetServerOptions.Default` mirrors a typical Bedrock dedicated-server RakNet block:

| Field | Default |
|-------|---------|
| Address | `0.0.0.0` |
| PortIpv4 / PortIpv6 | `19132` / `19133` |
| Protocol | `1001` |
| Version | `1.26.30` |
| Message | `Raknet` |
| MaxConnections | `40` |
| MtuMaxSize / MtuMinSize | `1492` / `400` |
| ValidatePort | `true` |

## Usage

```csharp
using RakNet;

// All defaults
var server = new NetworkServer();

// Override any subset
var server = new NetworkServer(RaknetServerOptions.Default.With(
    portIpv4: 19132,
    maxConnections: 100,
    message: "My Server",
    canAcceptConnections: () => true,
    playerCountProvider: () => onlineCount));

// Or object initializer
var server = new NetworkServer(new RaknetServerOptions
{
    Address = "0.0.0.0",
    PortIpv4 = 19132,
    Motd = "Hello",
    Protocol = 1001,
});
```

## Branches

- `development` — default working branch
- `main` — release branch; merges that change `src/**` auto-bump the NuGet patch (`+0.0.1`) and publish

## Build / pack locally

```bash
dotnet build src/Orion.RakNet/Orion.RakNet.csproj -c Release
dotnet pack src/Orion.RakNet/Orion.RakNet.csproj -c Release -o ./artifacts
```
