using AngleSharp.Dom;

namespace ParsingLibrary
{
    public sealed class Product
    {
        public string? Name { get; private set; }
        public Uri Uri { get; private set; }
        public List<string?> Breadcrumbs { get; private set; }
        public List<string?> ImagesUrl { get; private set; }
        public string? Region { get; private set; }
        public double? Price { get; private set; }
        public double? OldPrice { get; private set; }
        public bool? IsAvailable { get; private set; }

        public Product(IDocument document)
        {
            Uri = new Uri(document.BaseUri);
            Region = GetRegion(document);
            Name = GetName(document);
            Price = GetPriceByClassName(document, "price");
            OldPrice = GetPriceByClassName(document, "old-price");
            IsAvailable = GetAvailableness(document);
            Breadcrumbs = GetBreadcrumbs(document);
            ImagesUrl = GetImages(document);
        }

        private static string? GetRegion(IDocument document)
        {
            var element = document.Links
                .Where(l => l.GetAttribute("data-src") == "#region")
                .FirstOrDefault();

            if (element != null)
            {
                return element
                    .TextContent
                    .Trim(new char[] { ' ', '\n', '\t' });
            }
            else
            {
                return null;
            }
        }

        private static string? GetName(IDocument document)
        {
            var element = document.GetElementsByClassName("detail-name").FirstOrDefault();
            return element?.GetAttribute("content");
        }

        private static double? GetPriceByClassName(IDocument document, string className)
        {
            var element = document.GetElementsByClassName(className).FirstOrDefault();

            if (element != null)
            {
                string textContent = element.TextContent;
                return double.Parse(textContent[0..^4]);
            }
            else return null;
        }

        private static bool? GetAvailableness(IDocument document)
        {
            var element = document.GetElementsByClassName("v");
            return element != null;
        }

        private static List<string?> GetBreadcrumbs(IDocument document)
        {
            return document.GetElementsByClassName("breadcrumb")
                .SelectMany(e => e.Children)
                .Select(e => e.GetAttribute("href"))
                .Where(href => href != null)
                .ToList();
        }

        private static List<string?> GetImages(IDocument document)
        {
            var slider = document.GetElementsByClassName("card-slider-for")
                .FirstOrDefault();
            if (slider == null)
                return new List<string?> { null };
            else 
            {
                return slider    
                    .Children
                    .SelectMany(x => x.Children)
                    .Select(e => e.GetAttribute("href"))
                    .ToList();
            }
        }
    }
}