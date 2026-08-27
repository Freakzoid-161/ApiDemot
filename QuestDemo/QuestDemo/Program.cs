using QuestDemo;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<QuestDb>(opt => opt.UseInMemoryDatabase("QuestList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

RouteGroupBuilder quests = app.MapGroup("/quests");

quests.MapGet("/", GetAllQuests);
quests.MapGet("/{id}", GetQuest);
quests.MapPost("/", CreateQuest);
quests.MapPut("/{id}", UpdateQuest);
quests.MapDelete("/{id}", DeleteQuest);

app.Run();

static async Task<IResult> GetAllQuests(QuestDb db)
{
    return TypedResults.Ok(await db.Quests.Select(x => new QuestItemDto(x)).ToArrayAsync());
}


static async Task<IResult> GetQuest(int id, QuestDb db)
{
    return await db.Quests.FindAsync(id)
        is Quest quest
            ? TypedResults.Ok(new QuestItemDto(quest))
            : TypedResults.NotFound();
}

static async Task<IResult> CreateQuest(QuestItemDto questItemDTO, QuestDb db)
{
    var questItem = new Quest
    {
        Reward = questItemDTO.Reward,
        Name = questItemDTO.Name,
        Description = questItemDTO.Description,
    };

    db.Quests.Add(questItem);
    await db.SaveChangesAsync();

    questItemDTO = new QuestItemDto(questItem);

    return TypedResults.Created($"/quests/{questItem.Id}", questItemDTO);
}

static async Task<IResult> UpdateQuest(int id, QuestItemDto questItemDTO, QuestDb db)
{
    var quest = await db.Quests.FindAsync(id);

    if (quest is null) return TypedResults.NotFound();

    quest.Name = questItemDTO.Name;
    quest.Reward = questItemDTO.Reward;
    quest.Description = questItemDTO.Description;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteQuest(int id, QuestDb db)
{
    if (await db.Quests.FindAsync(id) is Quest quest)
    {
        db.Quests.Remove(quest);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}