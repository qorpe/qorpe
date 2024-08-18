using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qorpe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClusterConfigs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClusterId = table.Column<string>(type: "TEXT", nullable: false),
                    LoadBalancingPolicy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClusterConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RouteConfigs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RouteId = table.Column<string>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: true),
                    ClusterId = table.Column<string>(type: "TEXT", nullable: true),
                    AuthorizationPolicy = table.Column<string>(type: "TEXT", nullable: true),
                    RateLimiterPolicy = table.Column<string>(type: "TEXT", nullable: true),
                    OutputCachePolicy = table.Column<string>(type: "TEXT", nullable: true),
                    TimeoutPolicy = table.Column<string>(type: "TEXT", nullable: true),
                    Timeout = table.Column<string>(type: "TEXT", nullable: true),
                    CorsPolicy = table.Column<string>(type: "TEXT", nullable: true),
                    MaxRequestBodySize = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParentId = table.Column<long>(type: "INTEGER", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Destinations_ClusterConfigs_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ClusterConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForwarderRequestConfigs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActivityTimeout = table.Column<string>(type: "TEXT", nullable: true),
                    Version = table.Column<string>(type: "TEXT", nullable: true),
                    VersionPolicy = table.Column<int>(type: "INTEGER", nullable: true),
                    AllowResponseBuffering = table.Column<bool>(type: "INTEGER", nullable: true),
                    ClusterConfigId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForwarderRequestConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForwarderRequestConfigs_ClusterConfigs_ClusterConfigId",
                        column: x => x.ClusterConfigId,
                        principalTable: "ClusterConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HealthCheckConfigs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AvailableDestinationsPolicy = table.Column<string>(type: "TEXT", nullable: true),
                    ClusterConfigId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthCheckConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthCheckConfigs_ClusterConfigs_ClusterConfigId",
                        column: x => x.ClusterConfigId,
                        principalTable: "ClusterConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HttpClientConfigs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SslProtocols = table.Column<int>(type: "INTEGER", nullable: true),
                    DangerousAcceptAnyServerCertificate = table.Column<bool>(type: "INTEGER", nullable: true),
                    MaxConnectionsPerServer = table.Column<int>(type: "INTEGER", nullable: true),
                    EnableMultipleHttp2Connections = table.Column<bool>(type: "INTEGER", nullable: true),
                    RequestHeaderEncoding = table.Column<string>(type: "TEXT", nullable: true),
                    ResponseHeaderEncoding = table.Column<string>(type: "TEXT", nullable: true),
                    ClusterConfigId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HttpClientConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HttpClientConfigs_ClusterConfigs_ClusterConfigId",
                        column: x => x.ClusterConfigId,
                        principalTable: "ClusterConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionAffinityConfigs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: true),
                    Policy = table.Column<string>(type: "TEXT", nullable: true),
                    FailurePolicy = table.Column<string>(type: "TEXT", nullable: true),
                    AffinityKeyName = table.Column<string>(type: "TEXT", nullable: false),
                    ClusterConfigId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionAffinityConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionAffinityConfigs_ClusterConfigs_ClusterConfigId",
                        column: x => x.ClusterConfigId,
                        principalTable: "ClusterConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RouteMatches",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Methods = table.Column<string>(type: "TEXT", nullable: true),
                    Hosts = table.Column<string>(type: "TEXT", nullable: true),
                    Path = table.Column<string>(type: "TEXT", nullable: true),
                    RouteConfigId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteMatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteMatches_RouteConfigs_RouteConfigId",
                        column: x => x.RouteConfigId,
                        principalTable: "RouteConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transforms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RouteConfigId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transforms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transforms_RouteConfigs_RouteConfigId",
                        column: x => x.RouteConfigId,
                        principalTable: "RouteConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DestinationConfigs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Health = table.Column<string>(type: "TEXT", nullable: true),
                    Host = table.Column<string>(type: "TEXT", nullable: true),
                    DestinationId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DestinationConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DestinationConfigs_Destinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActiveHealthCheckConfigs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: true),
                    Interval = table.Column<string>(type: "TEXT", nullable: true),
                    Timeout = table.Column<string>(type: "TEXT", nullable: true),
                    Policy = table.Column<string>(type: "TEXT", nullable: true),
                    Path = table.Column<string>(type: "TEXT", nullable: true),
                    Query = table.Column<string>(type: "TEXT", nullable: true),
                    HealthCheckConfigId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveHealthCheckConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActiveHealthCheckConfigs_HealthCheckConfigs_HealthCheckConfigId",
                        column: x => x.HealthCheckConfigId,
                        principalTable: "HealthCheckConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PassiveHealthCheckConfigs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: true),
                    Policy = table.Column<string>(type: "TEXT", nullable: true),
                    ReactivationPeriod = table.Column<string>(type: "TEXT", nullable: true),
                    HealthCheckConfigId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassiveHealthCheckConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PassiveHealthCheckConfigs_HealthCheckConfigs_HealthCheckConfigId",
                        column: x => x.HealthCheckConfigId,
                        principalTable: "HealthCheckConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebProxyConfigs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    BypassOnLocal = table.Column<bool>(type: "INTEGER", nullable: true),
                    UseDefaultCredentials = table.Column<bool>(type: "INTEGER", nullable: true),
                    HttpClientConfigId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebProxyConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebProxyConfigs_HttpClientConfigs_HttpClientConfigId",
                        column: x => x.HttpClientConfigId,
                        principalTable: "HttpClientConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionAffinityCookieConfigs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Path = table.Column<string>(type: "TEXT", nullable: true),
                    Domain = table.Column<string>(type: "TEXT", nullable: true),
                    HttpOnly = table.Column<bool>(type: "INTEGER", nullable: true),
                    SecurePolicy = table.Column<int>(type: "INTEGER", nullable: true),
                    SameSite = table.Column<int>(type: "INTEGER", nullable: true),
                    Expiration = table.Column<string>(type: "TEXT", nullable: true),
                    MaxAge = table.Column<string>(type: "TEXT", nullable: true),
                    IsEssential = table.Column<bool>(type: "INTEGER", nullable: true),
                    SessionAffinityConfigId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionAffinityCookieConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionAffinityCookieConfigs_SessionAffinityConfigs_SessionAffinityConfigId",
                        column: x => x.SessionAffinityConfigId,
                        principalTable: "SessionAffinityConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RouteHeaders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Values = table.Column<string>(type: "TEXT", nullable: true),
                    Mode = table.Column<int>(type: "INTEGER", nullable: false),
                    IsCaseSensitive = table.Column<bool>(type: "INTEGER", nullable: false),
                    RouteMatchId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteHeaders_RouteMatches_RouteMatchId",
                        column: x => x.RouteMatchId,
                        principalTable: "RouteMatches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RouteQueryParameters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Values = table.Column<string>(type: "TEXT", nullable: true),
                    Mode = table.Column<int>(type: "INTEGER", nullable: false),
                    IsCaseSensitive = table.Column<bool>(type: "INTEGER", nullable: false),
                    RouteMatchId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteQueryParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteQueryParameters_RouteMatches_RouteMatchId",
                        column: x => x.RouteMatchId,
                        principalTable: "RouteMatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Metadata",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParentId = table.Column<long>(type: "INTEGER", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metadata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Metadata_ClusterConfigs_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ClusterConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Metadata_DestinationConfigs_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DestinationConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Metadata_RouteConfigs_ParentId",
                        column: x => x.ParentId,
                        principalTable: "RouteConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Metadata_Transforms_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Transforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveHealthCheckConfigs_HealthCheckConfigId",
                table: "ActiveHealthCheckConfigs",
                column: "HealthCheckConfigId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DestinationConfigs_DestinationId",
                table: "DestinationConfigs",
                column: "DestinationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_ParentId",
                table: "Destinations",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ForwarderRequestConfigs_ClusterConfigId",
                table: "ForwarderRequestConfigs",
                column: "ClusterConfigId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckConfigs_ClusterConfigId",
                table: "HealthCheckConfigs",
                column: "ClusterConfigId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HttpClientConfigs_ClusterConfigId",
                table: "HttpClientConfigs",
                column: "ClusterConfigId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Metadata_ParentId",
                table: "Metadata",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PassiveHealthCheckConfigs_HealthCheckConfigId",
                table: "PassiveHealthCheckConfigs",
                column: "HealthCheckConfigId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RouteHeaders_RouteMatchId",
                table: "RouteHeaders",
                column: "RouteMatchId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteMatches_RouteConfigId",
                table: "RouteMatches",
                column: "RouteConfigId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RouteQueryParameters_RouteMatchId",
                table: "RouteQueryParameters",
                column: "RouteMatchId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionAffinityConfigs_ClusterConfigId",
                table: "SessionAffinityConfigs",
                column: "ClusterConfigId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessionAffinityCookieConfigs_SessionAffinityConfigId",
                table: "SessionAffinityCookieConfigs",
                column: "SessionAffinityConfigId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transforms_RouteConfigId",
                table: "Transforms",
                column: "RouteConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_WebProxyConfigs_HttpClientConfigId",
                table: "WebProxyConfigs",
                column: "HttpClientConfigId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveHealthCheckConfigs");

            migrationBuilder.DropTable(
                name: "ForwarderRequestConfigs");

            migrationBuilder.DropTable(
                name: "Metadata");

            migrationBuilder.DropTable(
                name: "PassiveHealthCheckConfigs");

            migrationBuilder.DropTable(
                name: "RouteHeaders");

            migrationBuilder.DropTable(
                name: "RouteQueryParameters");

            migrationBuilder.DropTable(
                name: "SessionAffinityCookieConfigs");

            migrationBuilder.DropTable(
                name: "WebProxyConfigs");

            migrationBuilder.DropTable(
                name: "DestinationConfigs");

            migrationBuilder.DropTable(
                name: "Transforms");

            migrationBuilder.DropTable(
                name: "HealthCheckConfigs");

            migrationBuilder.DropTable(
                name: "RouteMatches");

            migrationBuilder.DropTable(
                name: "SessionAffinityConfigs");

            migrationBuilder.DropTable(
                name: "HttpClientConfigs");

            migrationBuilder.DropTable(
                name: "Destinations");

            migrationBuilder.DropTable(
                name: "RouteConfigs");

            migrationBuilder.DropTable(
                name: "ClusterConfigs");
        }
    }
}
