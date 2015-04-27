using System;
using System.Linq;
using System.Web.Http;

namespace HipstaLink.Resources
{
    [RoutePrefix("api/menu")]
    public class MenuResource : ControllerBase
    {
        [HttpGet, Route]
        public dynamic Get()
        {
            return new
            {
                items = Menu.Items.Select(item => new
                {
                    item.Name,
                    item.Price,

                    _links = new Links
                    {
                      LinkTo<MenuResource>(x => x.Rate(item.Id, null))  
                    }
                }),

                _links = new Links
                {
                    LinkSelf<MenuResource>(x => x.Get())
                }
            };
        }

        [HttpPost, Route("{id}")]
        public dynamic Rate(int id, [FromBody] string comment)
        {
            return Ok(string.Format("Comment for menu item {0} was accepted", id));
        }
    }

    static class Menu
    {
        public static readonly Item[] Items = new[]
        {
            new Item(1, "Caramel Flan Latte", 4.99m),
            new Item(2, "Mocha", 3.99m),
            new Item(3, "Whole Milk", 0.39m),
            new Item(4, "Whipped Cream", 0.59m),
            new Item(5, "Tiramisu", 5.39m),
        };

        public struct Item
        {
            public readonly int Id;
            public readonly string Name;
            public readonly decimal Price;

            public Item(int id, string name, decimal price)
            {
                Id = id;
                Name = name;
                Price = price;
            }
        }
    }
}