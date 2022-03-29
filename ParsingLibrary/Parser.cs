using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System.Diagnostics;

namespace ParsingLibrary
{
    public static class Parser
    {
        public static async Task<IDocument> GetDocumentAsync(string input)
        {
            IConfiguration config = Configuration.Default.WithDefaultLoader();
            IBrowsingContext context = BrowsingContext.New(config);
            Url url = new(input);

            return await context.OpenAsync(url);
        }

        public static async IAsyncEnumerable<IDocument> GetShopPagesAsync(string input)
        {
            var current = await GetDocumentAsync(input);
            yield return current;

            var nextPageLinkElement = current.Links
                .Where(link => link.ClassName == "page-link" && link.InnerHtml == "След.")
                .FirstOrDefault();

            while (nextPageLinkElement != null)
            {
                string href = ((IHtmlAnchorElement)nextPageLinkElement).Href;
                current = await GetDocumentAsync(href);
                yield return current;

                nextPageLinkElement = current.Links
                    .Where(link => link.ClassName == "page-link" && link.InnerHtml == "След.")
                    .FirstOrDefault();
            }
        }

        public static async IAsyncEnumerable<string> GetProductLinks(this IAsyncEnumerable<IDocument> pages)
        {
            await foreach (var page in pages)
            {
                var cards = page.GetElementsByClassName("d-block img-link text-center gtm-click");
                foreach (var card in cards)
                {
                    string href = ((IHtmlAnchorElement)card).Href;
                    Debug.WriteLine($"New product on {href}");
                    yield return href;
                }
            }
        }

        public static async IAsyncEnumerable<Product> GetProducts(this IEnumerable<string> links)
        {
            foreach (var link in links)
            {
                using IDocument document = await Parser.GetDocumentAsync(link);
                Product product = new(document);
                Debug.WriteLine($"Parsed {product.Name} (thread {Environment.CurrentManagedThreadId})");
                yield return new Product(document);
            }
        }
    }
}
