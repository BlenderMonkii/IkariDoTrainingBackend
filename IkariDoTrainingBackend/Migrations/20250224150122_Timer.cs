using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IkariDoTrainingBackend.Migrations
{
    /// <inheritdoc />
    public partial class Timer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "repetitions",
                table: "exercise_base",
                newName: "timer_id");

            migrationBuilder.CreateTable(
                name: "Timers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    rest_time = table.Column<int>(type: "int", nullable: true),
                    active_time = table.Column<int>(type: "int", nullable: true),
                    pause_time = table.Column<int>(type: "int", nullable: true),
                    sets = table.Column<int>(type: "int", nullable: true),
                    reps = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timers", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_exercise_base_timer_id",
                table: "exercise_base",
                column: "timer_id");

            migrationBuilder.AddForeignKey(
                name: "FK_exercise_base_Timers_timer_id",
                table: "exercise_base",
                column: "timer_id",
                principalTable: "Timers",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_exercise_base_Timers_timer_id",
                table: "exercise_base");

            migrationBuilder.DropTable(
                name: "Timers");

            migrationBuilder.DropIndex(
                name: "IX_exercise_base_timer_id",
                table: "exercise_base");

            migrationBuilder.RenameColumn(
                name: "timer_id",
                table: "exercise_base",
                newName: "repetitions");
        }
    }
}
