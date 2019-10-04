using Microsoft.EntityFrameworkCore.Migrations;

namespace IFNMUSiteCore.Migrations
{
    public partial class migrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    QuestionText = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleDays",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    DayNumber = table.Column<byte>(nullable: false),
                    LessonCount = table.Column<int>(nullable: false),
                    Week = table.Column<byte>(nullable: false),
                    Courses = table.Column<byte>(nullable: false),
                    Group = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleDays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThematicPlans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    NameChair = table.Column<string>(nullable: true),
                    NameLesson = table.Column<string>(nullable: true),
                    Courses = table.Column<string>(nullable: true),
                    NameSpecialty = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThematicPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Text = table.Column<string>(nullable: true),
                    TrueAnswer = table.Column<short>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Name = table.Column<string>(nullable: true),
                    LessonNumber = table.Column<byte>(nullable: false),
                    ThematicPlanId = table.Column<int>(nullable: true),
                    ScheduleDayId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lessons_ScheduleDays_ScheduleDayId",
                        column: x => x.ScheduleDayId,
                        principalTable: "ScheduleDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lessons_ThematicPlans_ThematicPlanId",
                        column: x => x.ThematicPlanId,
                        principalTable: "ThematicPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Name = table.Column<string>(nullable: true),
                    NumberHour = table.Column<byte>(nullable: false),
                    Date = table.Column<string>(nullable: true),
                    ThematicPlanId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Themes_ThematicPlans_ThematicPlanId",
                        column: x => x.ThematicPlanId,
                        principalTable: "ThematicPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ThematicPlans",
                columns: new[] { "Id", "Courses", "NameChair", "NameLesson", "NameSpecialty" },
                values: new object[] { -1, "None", "None", "None", "None" });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ScheduleDayId",
                table: "Lessons",
                column: "ScheduleDayId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ThematicPlanId",
                table: "Lessons",
                column: "ThematicPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Themes_ThematicPlanId",
                table: "Themes",
                column: "ThematicPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "Themes");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "ScheduleDays");

            migrationBuilder.DropTable(
                name: "ThematicPlans");
        }
    }
}
