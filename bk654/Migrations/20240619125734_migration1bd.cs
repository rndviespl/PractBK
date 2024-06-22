using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bk654.Migrations
{
    /// <inheritdoc />
    public partial class migration1bd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "position_at_work",
                columns: table => new
                {
                    position_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    salary_per_hour = table.Column<decimal>(type: "decimal(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.position_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "restaurant",
                columns: table => new
                {
                    restaurant_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    restaurant_code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    town = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    address = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mall = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    employees_count = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.restaurant_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "worker",
                columns: table => new
                {
                    worker_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    position_id = table.Column<int>(type: "int", nullable: false),
                    restaurant_id = table.Column<int>(type: "int", nullable: false),
                    surname = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    name = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    patronymic = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    dismissal_reason = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.worker_id);
                    table.ForeignKey(
                        name: "fk_worker_position_at_work1",
                        column: x => x.position_id,
                        principalTable: "position_at_work",
                        principalColumn: "position_id");
                    table.ForeignKey(
                        name: "fk_worker_restaurant1",
                        column: x => x.restaurant_id,
                        principalTable: "restaurant",
                        principalColumn: "restaurant_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "deleted_workers",
                columns: table => new
                {
                    worker_id = table.Column<int>(type: "int", nullable: false),
                    position_id = table.Column<int>(type: "int", nullable: false),
                    restaurant_id = table.Column<int>(type: "int", nullable: false),
                    surname = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    name = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    patronymic = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    dismissal_reason = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    deleted_timestamp = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "fk_deleted_workers_position_at_work1",
                        column: x => x.position_id,
                        principalTable: "position_at_work",
                        principalColumn: "position_id");
                    table.ForeignKey(
                        name: "fk_deleted_workers_restaurant1",
                        column: x => x.restaurant_id,
                        principalTable: "restaurant",
                        principalColumn: "restaurant_id");
                    table.ForeignKey(
                        name: "fk_deleted_workers_worker1",
                        column: x => x.worker_id,
                        principalTable: "worker",
                        principalColumn: "worker_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "performance_reviews",
                columns: table => new
                {
                    review_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    worker_id = table.Column<int>(type: "int", nullable: false),
                    reviewer_name = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    review_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    performance_rating = table.Column<int>(type: "int", nullable: false),
                    comments = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.review_id);
                    table.ForeignKey(
                        name: "fk_performance_reviews_worker1",
                        column: x => x.worker_id,
                        principalTable: "worker",
                        principalColumn: "worker_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "work_shift",
                columns: table => new
                {
                    work_shift_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    worker_id = table.Column<int>(type: "int", nullable: false),
                    start_shift = table.Column<DateTime>(type: "datetime", nullable: false),
                    end_shift = table.Column<DateTime>(type: "datetime", nullable: false),
                    description_manual_entry = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.work_shift_id);
                    table.ForeignKey(
                        name: "fk_work_shift_worker",
                        column: x => x.worker_id,
                        principalTable: "worker",
                        principalColumn: "worker_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "fk_deleted_workers_position_at_work1_idx",
                table: "deleted_workers",
                column: "position_id");

            migrationBuilder.CreateIndex(
                name: "fk_deleted_workers_restaurant1_idx",
                table: "deleted_workers",
                column: "restaurant_id");

            migrationBuilder.CreateIndex(
                name: "fk_deleted_workers_worker1_idx",
                table: "deleted_workers",
                column: "worker_id");

            migrationBuilder.CreateIndex(
                name: "fk_performance_reviews_worker1_idx",
                table: "performance_reviews",
                column: "worker_id");

            migrationBuilder.CreateIndex(
                name: "worker_id",
                table: "restaurant",
                column: "restaurant_id");

            migrationBuilder.CreateIndex(
                name: "fk_work_shift_worker_idx",
                table: "work_shift",
                column: "worker_id");

            migrationBuilder.CreateIndex(
                name: "fk_worker_position_at_work1_idx",
                table: "worker",
                column: "position_id");

            migrationBuilder.CreateIndex(
                name: "fk_worker_restaurant1_idx",
                table: "worker",
                column: "restaurant_id");

            migrationBuilder.CreateIndex(
                name: "idx_worker_id",
                table: "worker",
                column: "worker_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "deleted_workers");

            migrationBuilder.DropTable(
                name: "performance_reviews");

            migrationBuilder.DropTable(
                name: "work_shift");

            migrationBuilder.DropTable(
                name: "worker");

            migrationBuilder.DropTable(
                name: "position_at_work");

            migrationBuilder.DropTable(
                name: "restaurant");
        }
    }
}
