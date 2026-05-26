using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WIMS.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product_categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "units_of_measure",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Abbreviation = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_units_of_measure", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "warehouses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Address = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ContactPerson = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ContactPhone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Sku = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    UomId = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false),
                    ReorderLevel = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_products_product_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "product_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_products_units_of_measure_UomId",
                        column: x => x.UomId,
                        principalTable: "units_of_measure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "goods_dispatches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GdnNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    CustomerName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CustomerAddress = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DispatchDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ConfirmedBy = table.Column<int>(type: "integer", nullable: true),
                    ConfirmedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DispatchedBy = table.Column<int>(type: "integer", nullable: true),
                    DispatchedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_goods_dispatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_goods_dispatches_warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "purchase_orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PoNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SupplierName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SupplierContact = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    OrderDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpectedDelivery = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SubmittedBy = table.Column<int>(type: "integer", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ApprovedBy = table.Column<int>(type: "integer", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancelledBy = table.Column<int>(type: "integer", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancellationReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchase_orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_purchase_orders_warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    FailedLoginAttempts = table.Column<int>(type: "integer", nullable: false),
                    LockedUntil = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    WarehouseId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "zones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_zones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_zones_warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "purchase_order_items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PoId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    OrderedQty = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false),
                    ReceivedQty = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    LineTotal = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchase_order_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_purchase_order_items_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_purchase_order_items_purchase_orders_PoId",
                        column: x => x.PoId,
                        principalTable: "purchase_orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "audit_logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActionType = table.Column<string>(type: "text", nullable: false),
                    EntityName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PreviousValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    PerformedBy = table.Column<int>(type: "integer", nullable: true),
                    PerformedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_audit_logs_users_PerformedBy",
                        column: x => x.PerformedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "goods_receipts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrnNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PoId = table.Column<int>(type: "integer", nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    ReceiptDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ReceivedBy = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_goods_receipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_goods_receipts_purchase_orders_PoId",
                        column: x => x.PoId,
                        principalTable: "purchase_orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_goods_receipts_users_ReceivedBy",
                        column: x => x.ReceivedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_goods_receipts_warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reorder_alerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    CurrentStock = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    ReorderLevel = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    AlertDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsAcknowledged = table.Column<bool>(type: "boolean", nullable: false),
                    AcknowledgedBy = table.Column<int>(type: "integer", nullable: true),
                    AcknowledgedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reorder_alerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reorder_alerts_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reorder_alerts_users_AcknowledgedBy",
                        column: x => x.AcknowledgedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reorder_alerts_warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "stock_transfers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransferNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SourceWarehouseId = table.Column<int>(type: "integer", nullable: false),
                    DestWarehouseId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    InitiatedBy = table.Column<int>(type: "integer", nullable: false),
                    InitiatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReceivedBy = table.Column<int>(type: "integer", nullable: true),
                    ReceivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancelledBy = table.Column<int>(type: "integer", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock_transfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stock_transfers_users_InitiatedBy",
                        column: x => x.InitiatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_transfers_warehouses_DestWarehouseId",
                        column: x => x.DestWarehouseId,
                        principalTable: "warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_transfers_warehouses_SourceWarehouseId",
                        column: x => x.SourceWarehouseId,
                        principalTable: "warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ZoneId = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MaxCapacity = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bins_zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "goods_dispatch_items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GdnId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    BinId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false),
                    LineTotal = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_goods_dispatch_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_goods_dispatch_items_bins_BinId",
                        column: x => x.BinId,
                        principalTable: "bins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_goods_dispatch_items_goods_dispatches_GdnId",
                        column: x => x.GdnId,
                        principalTable: "goods_dispatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_goods_dispatch_items_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "goods_receipt_items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrnId = table.Column<int>(type: "integer", nullable: false),
                    PoItemId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    BinId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    Condition = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_goods_receipt_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_goods_receipt_items_bins_BinId",
                        column: x => x.BinId,
                        principalTable: "bins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_goods_receipt_items_goods_receipts_GrnId",
                        column: x => x.GrnId,
                        principalTable: "goods_receipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_goods_receipt_items_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_goods_receipt_items_purchase_order_items_PoItemId",
                        column: x => x.PoItemId,
                        principalTable: "purchase_order_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "stock_adjustments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AdjustmentNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    BinId = table.Column<int>(type: "integer", nullable: false),
                    AdjustmentType = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    ReasonNotes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ApprovalStatus = table.Column<string>(type: "text", nullable: false),
                    ReviewedBy = table.Column<int>(type: "integer", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RejectionReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock_adjustments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stock_adjustments_bins_BinId",
                        column: x => x.BinId,
                        principalTable: "bins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_adjustments_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_adjustments_users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_adjustments_users_ReviewedBy",
                        column: x => x.ReviewedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_adjustments_warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "stock_movements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    BinId = table.Column<int>(type: "integer", nullable: false),
                    MovementType = table.Column<string>(type: "text", nullable: false),
                    QuantityChange = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    QuantityBefore = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    QuantityAfter = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    ReferenceType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ReferenceId = table.Column<int>(type: "integer", nullable: true),
                    PerformedBy = table.Column<int>(type: "integer", nullable: false),
                    PerformedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock_movements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stock_movements_bins_BinId",
                        column: x => x.BinId,
                        principalTable: "bins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_movements_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_movements_users_PerformedBy",
                        column: x => x.PerformedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_movements_warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "stock_records",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    BinId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock_records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stock_records_bins_BinId",
                        column: x => x.BinId,
                        principalTable: "bins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_records_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_records_warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "stock_transfer_items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransferId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    SourceBinId = table.Column<int>(type: "integer", nullable: false),
                    DestBinId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock_transfer_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stock_transfer_items_bins_DestBinId",
                        column: x => x.DestBinId,
                        principalTable: "bins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_transfer_items_bins_SourceBinId",
                        column: x => x.SourceBinId,
                        principalTable: "bins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_transfer_items_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_transfer_items_stock_transfers_TransferId",
                        column: x => x.TransferId,
                        principalTable: "stock_transfers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_PerformedBy",
                table: "audit_logs",
                column: "PerformedBy");

            migrationBuilder.CreateIndex(
                name: "IX_bins_Code",
                table: "bins",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bins_ZoneId",
                table: "bins",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_goods_dispatch_items_BinId",
                table: "goods_dispatch_items",
                column: "BinId");

            migrationBuilder.CreateIndex(
                name: "IX_goods_dispatch_items_GdnId",
                table: "goods_dispatch_items",
                column: "GdnId");

            migrationBuilder.CreateIndex(
                name: "IX_goods_dispatch_items_ProductId",
                table: "goods_dispatch_items",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_goods_dispatches_GdnNumber",
                table: "goods_dispatches",
                column: "GdnNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_goods_dispatches_WarehouseId",
                table: "goods_dispatches",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_goods_receipt_items_BinId",
                table: "goods_receipt_items",
                column: "BinId");

            migrationBuilder.CreateIndex(
                name: "IX_goods_receipt_items_GrnId",
                table: "goods_receipt_items",
                column: "GrnId");

            migrationBuilder.CreateIndex(
                name: "IX_goods_receipt_items_PoItemId",
                table: "goods_receipt_items",
                column: "PoItemId");

            migrationBuilder.CreateIndex(
                name: "IX_goods_receipt_items_ProductId",
                table: "goods_receipt_items",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_goods_receipts_GrnNumber",
                table: "goods_receipts",
                column: "GrnNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_goods_receipts_PoId",
                table: "goods_receipts",
                column: "PoId");

            migrationBuilder.CreateIndex(
                name: "IX_goods_receipts_ReceivedBy",
                table: "goods_receipts",
                column: "ReceivedBy");

            migrationBuilder.CreateIndex(
                name: "IX_goods_receipts_WarehouseId",
                table: "goods_receipts",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_product_categories_Name",
                table: "product_categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_products_CategoryId",
                table: "products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_products_Sku",
                table: "products",
                column: "Sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_products_UomId",
                table: "products",
                column: "UomId");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_order_items_PoId_ProductId",
                table: "purchase_order_items",
                columns: new[] { "PoId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_purchase_order_items_ProductId",
                table: "purchase_order_items",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_orders_PoNumber",
                table: "purchase_orders",
                column: "PoNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_purchase_orders_WarehouseId",
                table: "purchase_orders",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_reorder_alerts_AcknowledgedBy",
                table: "reorder_alerts",
                column: "AcknowledgedBy");

            migrationBuilder.CreateIndex(
                name: "IX_reorder_alerts_ProductId",
                table: "reorder_alerts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_reorder_alerts_WarehouseId",
                table: "reorder_alerts",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_adjustments_AdjustmentNumber",
                table: "stock_adjustments",
                column: "AdjustmentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_stock_adjustments_BinId",
                table: "stock_adjustments",
                column: "BinId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_adjustments_CreatedBy",
                table: "stock_adjustments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_stock_adjustments_ProductId",
                table: "stock_adjustments",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_adjustments_ReviewedBy",
                table: "stock_adjustments",
                column: "ReviewedBy");

            migrationBuilder.CreateIndex(
                name: "IX_stock_adjustments_WarehouseId",
                table: "stock_adjustments",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_movements_BinId",
                table: "stock_movements",
                column: "BinId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_movements_PerformedBy",
                table: "stock_movements",
                column: "PerformedBy");

            migrationBuilder.CreateIndex(
                name: "IX_stock_movements_ProductId",
                table: "stock_movements",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_movements_WarehouseId",
                table: "stock_movements",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_records_BinId",
                table: "stock_records",
                column: "BinId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_records_ProductId_BinId",
                table: "stock_records",
                columns: new[] { "ProductId", "BinId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_stock_records_WarehouseId",
                table: "stock_records",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_transfer_items_DestBinId",
                table: "stock_transfer_items",
                column: "DestBinId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_transfer_items_ProductId",
                table: "stock_transfer_items",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_transfer_items_SourceBinId",
                table: "stock_transfer_items",
                column: "SourceBinId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_transfer_items_TransferId",
                table: "stock_transfer_items",
                column: "TransferId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_transfers_DestWarehouseId",
                table: "stock_transfers",
                column: "DestWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_transfers_InitiatedBy",
                table: "stock_transfers",
                column: "InitiatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_stock_transfers_SourceWarehouseId",
                table: "stock_transfers",
                column: "SourceWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_transfers_TransferNumber",
                table: "stock_transfers",
                column: "TransferNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_units_of_measure_Abbreviation",
                table: "units_of_measure",
                column: "Abbreviation",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_units_of_measure_Name",
                table: "units_of_measure",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_WarehouseId",
                table: "users",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_warehouses_Code",
                table: "warehouses",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_zones_Code",
                table: "zones",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_zones_WarehouseId_Code",
                table: "zones",
                columns: new[] { "WarehouseId", "Code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_logs");

            migrationBuilder.DropTable(
                name: "goods_dispatch_items");

            migrationBuilder.DropTable(
                name: "goods_receipt_items");

            migrationBuilder.DropTable(
                name: "reorder_alerts");

            migrationBuilder.DropTable(
                name: "stock_adjustments");

            migrationBuilder.DropTable(
                name: "stock_movements");

            migrationBuilder.DropTable(
                name: "stock_records");

            migrationBuilder.DropTable(
                name: "stock_transfer_items");

            migrationBuilder.DropTable(
                name: "goods_dispatches");

            migrationBuilder.DropTable(
                name: "goods_receipts");

            migrationBuilder.DropTable(
                name: "purchase_order_items");

            migrationBuilder.DropTable(
                name: "bins");

            migrationBuilder.DropTable(
                name: "stock_transfers");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "purchase_orders");

            migrationBuilder.DropTable(
                name: "zones");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "product_categories");

            migrationBuilder.DropTable(
                name: "units_of_measure");

            migrationBuilder.DropTable(
                name: "warehouses");
        }
    }
}
