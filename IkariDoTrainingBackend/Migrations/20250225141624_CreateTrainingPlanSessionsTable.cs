using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IkariDoTrainingBackend.Migrations
{
    /// <inheritdoc />
    public partial class CreateTrainingPlanSessionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_exercise_base_Users_owner_id",
                table: "exercise_base");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_training_plan_sessions",
                table: "training_plan_sessions");

            migrationBuilder.DropIndex(
                name: "IX_training_plan_sessions_training_plan_id",
                table: "training_plan_sessions");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "users",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "repetitions",
                table: "exercise_base",
                newName: "timer_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_training_plan_sessions",
                table: "training_plan_sessions",
                columns: new[] { "training_plan_id", "session_id" });

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
                name: "IX_training_plan_sessions_session_id",
                table: "training_plan_sessions",
                column: "session_id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_exercise_base_users_owner_id",
                table: "exercise_base",
                column: "owner_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_exercise_base_Timers_timer_id",
                table: "exercise_base");

            migrationBuilder.DropForeignKey(
                name: "FK_exercise_base_users_owner_id",
                table: "exercise_base");

            migrationBuilder.DropTable(
                name: "Timers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_training_plan_sessions",
                table: "training_plan_sessions");

            migrationBuilder.DropIndex(
                name: "IX_training_plan_sessions_session_id",
                table: "training_plan_sessions");

            migrationBuilder.DropIndex(
                name: "IX_exercise_base_timer_id",
                table: "exercise_base");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "timer_id",
                table: "exercise_base",
                newName: "repetitions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_training_plan_sessions",
                table: "training_plan_sessions",
                columns: new[] { "session_id", "training_plan_id" });

            migrationBuilder.CreateIndex(
                name: "IX_training_plan_sessions_training_plan_id",
                table: "training_plan_sessions",
                column: "training_plan_id");

            migrationBuilder.AddForeignKey(
                name: "FK_exercise_base_Users_owner_id",
                table: "exercise_base",
                column: "owner_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
