namespace Exam
{
    class Program
    {
        static async Task Main()
        {
            var service = new ArticleService("http://localhost:3000/articles");

            var newArticle = new Article
            {
                Title = "Test2",
                Description = "www",
                Author = "Alex",
                Date = DateTime.Now
            };

            string? newId = await service.AddArticleAsync(newArticle);
            Console.WriteLine($"Added new articles with ID = {newId}");

            var all = await service.GetAllAsync();
            Console.WriteLine("\nAll articles:");
            all.ForEach(a => Console.WriteLine($"ID: {a.id}, Title: {a.Title}"));

            var search = await service.SearchByTitleAsync("Test");
            Console.WriteLine("\nResult:");
            search.ForEach(a => Console.WriteLine($"ID: {a.id}, Title: {a.Title}"));
        }
    }
}
