using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IkariDoTrainingBackend.Migrations
{
    /// <inheritdoc />
    public partial class FixFingerboardExerciseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_fingerboard_exercises_exercise_base_exercise_id",
                table: "fingerboard_exercises");

            migrationBuilder.RenameColumn(
                name: "exercise_id",
                table: "fingerboard_exercises",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "exercise_id",
                table: "exercise_base",
                newName: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_fingerboard_exercises_exercise_base_id",
                table: "fingerboard_exercises",
                column: "id",
                principalTable: "exercise_base",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_fingerboard_exercises_exercise_base_id",
                table: "fingerboard_exercises");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "fingerboard_exercises",
                newName: "exercise_id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "exercise_base",
                newName: "exercise_id");

            migrationBuilder.AddForeignKey(
                name: "FK_fingerboard_exercises_exercise_base_exercise_id",
                table: "fingerboard_exercises",
                column: "exercise_id",
                principalTable: "exercise_base",
                principalColumn: "exercise_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
