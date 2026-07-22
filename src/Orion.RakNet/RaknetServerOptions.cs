namespace RakNet;

/// <summary>
/// Configuration for <see cref="NetworkServer"/>.
/// Defaults match a typical Bedrock dedicated-server RakNet block
/// (<c>Address</c>, ports, protocol, MTU, etc.). Any property can be overridden
/// via object initializer or <see cref="With"/>.
/// </summary>
public sealed class RaknetServerOptions
{
    public const string DefaultAddress = "0.0.0.0";
    public const ushort DefaultPortIpv4 = 19132;
    public const ushort DefaultPortIpv6 = 19133;
    public const int DefaultProtocol = 1001;
    public const string DefaultVersion = "1.26.30";
    public const string DefaultMessage = "Raknet";
    public const int DefaultMaxConnections = 40;
    public const int DefaultMtuMaxSize = 1492;
    public const int DefaultMtuMinSize = 400;
    public const bool DefaultValidatePort = true;
    public const string DefaultEdition = "MCPE";
    public const string DefaultMotd = "Raknet";
    public const string DefaultGamemode = "Survival";
    public const int DefaultGamemodeId = 1;

    /// <summary>Shared immutable defaults (from server.json Raknet section).</summary>
    public static RaknetServerOptions Default { get; } = new();

    public string Address { get; init; } = DefaultAddress;
    public ushort PortIpv4 { get; init; } = DefaultPortIpv4;
    public ushort PortIpv6 { get; init; } = DefaultPortIpv6;
    public int Protocol { get; init; } = DefaultProtocol;
    public string Version { get; init; } = DefaultVersion;

    /// <summary>Sub-MOTD segment in the MCPE advertisement string.</summary>
    public string Message { get; init; } = DefaultMessage;

    public int MaxConnections { get; init; } = DefaultMaxConnections;
    public int MtuMaxSize { get; init; } = DefaultMtuMaxSize;
    public int MtuMinSize { get; init; } = DefaultMtuMinSize;
    public bool ValidatePort { get; init; } = DefaultValidatePort;

    /// <summary>When true, Open Connection uses cookie challenge.</summary>
    public bool EnableCookies { get; init; } = true;

    /// <summary>0 = generate a random guid when the server starts.</summary>
    public ulong ServerGuid { get; init; }

    public string Edition { get; init; } = DefaultEdition;
    public string Motd { get; init; } = DefaultMotd;
    public string Gamemode { get; init; } = DefaultGamemode;
    public int GamemodeId { get; init; } = DefaultGamemodeId;

    /// <summary>Optional live player count for UnconnectedPong (defaults to 0).</summary>
    public Func<int>? PlayerCountProvider { get; init; }

    /// <summary>Optional gate for new connections (defaults to always accept).</summary>
    public Func<bool>? CanAcceptConnections { get; init; }

    /// <summary>
    /// Full override for the UnconnectedPong advertisement. When set, field-based
    /// <see cref="BuildAdvertisement"/> formatting is skipped.
    /// </summary>
    public Func<string>? AdvertisementProvider { get; init; }

    public ushort ResolvedMinMtu =>
        (ushort)Math.Clamp(MtuMinSize, 400, ushort.MaxValue);

    public ushort ResolvedMaxMtu
    {
        get
        {
            ushort min = ResolvedMinMtu;
            return (ushort)Math.Clamp(MtuMaxSize, min, ushort.MaxValue);
        }
    }

    public RaknetServerOptions With(
        string? address = null,
        ushort? portIpv4 = null,
        ushort? portIpv6 = null,
        int? protocol = null,
        string? version = null,
        string? message = null,
        int? maxConnections = null,
        int? mtuMaxSize = null,
        int? mtuMinSize = null,
        bool? validatePort = null,
        bool? enableCookies = null,
        ulong? serverGuid = null,
        string? edition = null,
        string? motd = null,
        string? gamemode = null,
        int? gamemodeId = null,
        Func<int>? playerCountProvider = null,
        Func<bool>? canAcceptConnections = null,
        Func<string>? advertisementProvider = null) =>
        new()
        {
            Address = address ?? Address,
            PortIpv4 = portIpv4 ?? PortIpv4,
            PortIpv6 = portIpv6 ?? PortIpv6,
            Protocol = protocol ?? Protocol,
            Version = version ?? Version,
            Message = message ?? Message,
            MaxConnections = maxConnections ?? MaxConnections,
            MtuMaxSize = mtuMaxSize ?? MtuMaxSize,
            MtuMinSize = mtuMinSize ?? MtuMinSize,
            ValidatePort = validatePort ?? ValidatePort,
            EnableCookies = enableCookies ?? EnableCookies,
            ServerGuid = serverGuid ?? ServerGuid,
            Edition = edition ?? Edition,
            Motd = motd ?? Motd,
            Gamemode = gamemode ?? Gamemode,
            GamemodeId = gamemodeId ?? GamemodeId,
            PlayerCountProvider = playerCountProvider ?? PlayerCountProvider,
            CanAcceptConnections = canAcceptConnections ?? CanAcceptConnections,
            AdvertisementProvider = advertisementProvider ?? AdvertisementProvider
        };

    /// <summary>
    /// MCPE list ping format:
    /// Edition;MOTD;Protocol;Version;PlayerCount;MaxPlayers;ServerGuid;SubMotd;Gamemode;GamemodeId;PortV4;PortV6;
    /// </summary>
    public string BuildAdvertisement(ulong serverGuid, int? playerCount = null)
    {
        if (AdvertisementProvider is not null)
        {
            return AdvertisementProvider();
        }

        int players = playerCount ?? PlayerCountProvider?.Invoke() ?? 0;
        return string.Join(';',
        [
            Edition,
            Motd,
            Protocol.ToString(),
            Version,
            players.ToString(),
            MaxConnections.ToString(),
            serverGuid.ToString(),
            Message,
            Gamemode,
            GamemodeId.ToString(),
            PortIpv4.ToString(),
            PortIpv6.ToString(),
            ""
        ]);
    }
}
