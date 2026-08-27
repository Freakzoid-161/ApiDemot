using QuestDemo;
    using Microsoft.EntityFrameworkCore;

    class QuestDb : DbContext
    {
        public QuestDb(DbContextOptions<QuestDb> options)
            : base(options) { }

        public DbSet<Quest> Quests => Set<Quest>();
    }

