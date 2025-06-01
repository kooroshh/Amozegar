using Microsoft.AspNetCore.Mvc;
namespace Amozegar.Areas.Shared.Components.Controllers
{
    public abstract class BaseViewComponent : ViewComponent
    {
        public static string DefaultPath { get; set; } = "/Areas/Shared/Components/Views";


        protected string setViewPath(params string[] paths)
        {
            var route = DefaultPath;
            foreach(string path in paths)
            {
                route = Path.Combine(route, path);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(route);
            return route;
        }
    }
}
