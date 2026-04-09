namespace Exam
{
    class Program
    {
            static async Task Main()
            {
                var service = new ArticleService("http://localhost:3000/articles");

                var newArticle = new Article
                {
                    Title = "Test3",
                    Description = "rrr",
                    Author = "Alex",
                    Date = DateTime.Now
                };

                string? newId = await service.AddArticleAsync(newArticle);
                Console.WriteLine($"Added new article with ID = {newId}");

                var all = await service.GetAllAsync();
                Console.WriteLine("\nAll articles:");
                all.ForEach(a => Console.WriteLine($"ID: {a.id}, Title: {a.Title}"));

                var search = await service.SearchByTitleAsync("Test");
                Console.WriteLine("\nSearch result:");
                search.ForEach(a => Console.WriteLine($"ID: {a.id}, Title: {a.Title}"));

                if (!string.IsNullOrEmpty(newId))
                {
                    var updates = new Dictionary<string, object>
                {
                    { "title", "Updated Test3" },
                    { "description", "Updated description via PATCH" }
                };

                    var updated = await service.PatchArticleAsync(newId, updates);
                    if (updated != null)
                    {
                        Console.WriteLine($"\nArticle updated successfully via PATCH!");
                        Console.WriteLine($"New title: {updated.Title}");
                        Console.WriteLine($"New description: {updated.Description}");
                    }
                    else
                    {
                        Console.WriteLine("\nFailed to update article");
                    }
                }
                if (!string.IsNullOrEmpty(newId))
                {
                    var updatedArticle = await service.GetByIdAsync(newId);
                    if (updatedArticle != null)
                    {
                        Console.WriteLine($"\nVerified: Article {newId} now has title: {updatedArticle.Title}");
                    }
                }
                //if (!string.IsNullOrEmpty(newId))
                //{
                //    var deleted = await service.DeleteArticleAsync(newId);
                //    if (deleted)
                //    {
                //        Console.WriteLine($"\nArticle with ID {newId} deleted successfully");
                //    }
                //    else
                //    {
                //        Console.WriteLine($"\nFailed to delete article with ID {newId}");
                //    }
                //}

                Console.WriteLine("\nAll articles after deletion:");
                var afterDelete = await service.GetAllAsync();
                if (afterDelete.Count == 0)
                {
                    Console.WriteLine("   No articles left");
                }
                else
                {
                    afterDelete.ForEach(a => Console.WriteLine($"ID: {a.id}, Title: {a.Title}"));
                }
            }
        }
    }
