using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maersk.SCM.DeliveryPlanning.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryOrderEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TraceId = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryOrderEvents", x => new { x.Id, x.Version });
                });

            migrationBuilder.CreateTable(
                name: "DeliveryOrderReferences",
                columns: table => new
                {
                    DeliveryOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveryOrderNumberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryOrderReferences", x => x.DeliveryOrderId);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryPlanEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TraceId = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryPlanEvents", x => new { x.Id, x.Version });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryOrderEvents");

            migrationBuilder.DropTable(
                name: "DeliveryOrderReferences");

            migrationBuilder.DropTable(
                name: "DeliveryPlanEvents");
        }
    }
}
