HipstaLink
=========

HipstaLink is a tiny helper for the ASP.NET Web API 2 - it creates URIs according to the application's route configuration in a type-safe manner. It **works** both with route template configuration and *attribute routing* (new in Web API 2). The cool thing - it fits into a single [file](https://github.com/yevhen/HipstaLink/blob/master/Source/Core/RouteLinker.cs), which you can copy-paste into your own project, hook it up at start-up and you're done.

Demo
------------
The project included with the source code, provides a very simple demo of how to [wire](https://github.com/yevhen/HipstaLink/blob/master/Source/Custom/ControllerActivator.cs) and [use](https://github.com/yevhen/HipstaLink/blob/master/Source/Resources/MenuResource.cs) HipstaLink. It also contains a lot of handy Web API customizations that might be valuable when building hypermedia-driven REST APIs (HATEOAS). Teaser below.

```cs
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
```
Sending http GET to `api/menu` endpoint, with media type `application/json`, will produce the following HAL representation:

```js
{
  "items": [
    {
      "name": "Caramel Flan Latte",
      "price": 4.99,
      "_links": {
        "rate": {
          "href": "http://localhost:6788/api/menu/1"
        }
      }
    },
    {
      "name": "Mocha",
      "price": 3.99,
      "_links": {
        "rate": {
          "href": "http://localhost:6788/api/menu/2"
        }
      }
    },
  ],
  "_links": {
    "get": {
      "href": "http://localhost:6788/api/menu"
    }
  }
}
```

Nuget
-----
HipstaLink is not a framework and not even a library. The whole thing is just one [file](https://github.com/yevhen/HipstaLink/blob/master/Source/Core/RouteLinker.cs). Nothing more.

Contribute
----------
HipstaLink is OSS - do whatever you want ...

## License

Apache 2 License