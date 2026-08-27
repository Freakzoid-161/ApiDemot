using QuestDemo;

public class QuestItemDto
{
    public int Id { set; get; }
    public string Name { set; get; }
    public string Description { set; get; }
    public int Reward { set; get; }

    public QuestItemDto() { }
    public QuestItemDto(Quest QuestItem) =>
    (Id, Name, Description, Reward) = (QuestItem.Id, QuestItem.Name, QuestItem.Description, QuestItem.Reward);
}
