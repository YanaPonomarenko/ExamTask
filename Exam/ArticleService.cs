using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Exam;

public class ArticleService : IDisposable
{
    private readonly HttpClient _client;
    private readonly string _baseUrl;

    public ArticleService(string baseUrl)
    {
        _client = new HttpClient();
        _baseUrl = baseUrl;
    }
    public async Task<List<Article>> GetAllAsync() =>
        await _client.GetFromJsonAsync<List<Article>>(_baseUrl) ?? new List<Article>();

    public async Task<Article?> GetByIdAsync(string id) =>
        await _client.GetFromJsonAsync<Article>($"{_baseUrl}/{id}");
    public async Task<string?> AddArticleAsync(Article article)
    {
        var response = await _client.PostAsJsonAsync(_baseUrl, article);
        if (response.IsSuccessStatusCode)
        {
            var added = await response.Content.ReadFromJsonAsync<Article>();
            return added?.id;
        }
        return null;
    }
    public async Task<bool> DeleteArticleAsync(string id)
    {
        var response = await _client.DeleteAsync($"{_baseUrl}/{id}");
        return response.IsSuccessStatusCode;
    }
    public async Task<Article?> PatchArticleAsync(string id, Dictionary<string, object> updates)
    {
        var json = JsonSerializer.Serialize(updates);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PatchAsync($"{_baseUrl}/{id}", data);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Article>();
        }
        return null;
    }
    public async Task<List<Article>> SearchByTitleAsync(string title)
    {
        var all = await GetAllAsync();
        return all.Where(a => a.Title?.Contains(title, StringComparison.OrdinalIgnoreCase) == true).ToList();
    }

    public void Dispose()
    {
        _client?.Dispose();
    }
}