# Module Evidence

## 1. Existing pizzas and additional pizza

Captured from the running Contoso Pizza API at `http://localhost:5207` after
creating the additional pizza, `Hawaiian` (ID 5). The response shows it with
the four pizzas already in the service.

**Request**

```http
GET /pizza HTTP/1.1
Host: localhost:5207
```

**Response — 200 OK**

```json
[
  {
    "id": 1,
    "name": "Classic Italian",
    "isGlutenFree": false
  },
  {
    "id": 2,
    "name": "Veggie",
    "isGlutenFree": true
  },
  {
    "id": 3,
    "name": "Egg n' Bacon",
    "isGlutenFree": false
  },
  {
    "id": 4,
    "name": "Frango Catupiry",
    "isGlutenFree": false
  },
  {
    "id": 5,
    "name": "Hawaiian",
    "isGlutenFree": false
  }
]
```

## 2. GET, POST, PUT, and DELETE operations

The POST, GET, PUT, and DELETE requests were sent to the same running API
instance. The GET response below was captured after creating Hawaiian.

### POST a pizza

**Request**

```http
POST /pizza HTTP/1.1
Host: localhost:5207
Content-Type: application/json

{
  "name": "Hawaiian",
  "isGlutenFree": false
}
```

**Response — 201 Created**

```http
Location: http://localhost:5207/Pizza/5
```

```json
{
  "id": 5,
  "name": "Hawaiian",
  "isGlutenFree": false
}
```

### GET all pizzas

**Request**

```http
GET /pizza HTTP/1.1
Host: localhost:5207
```

**Response — 200 OK**

The JSON response is shown in section 1.

### PUT an updated pizza

**Request**

```http
PUT /pizza/5 HTTP/1.1
Host: localhost:5207
Content-Type: application/json

{
  "id": 5,
  "name": "Hawaiian Supreme",
  "isGlutenFree": false
}
```

**Response — 204 No Content**

The response has no body. A follow-up `GET /pizza/5` returned `200 OK` with:

```json
{
  "id": 5,
  "name": "Hawaiian Supreme",
  "isGlutenFree": false
}
```

### DELETE a pizza

**Request**

```http
DELETE /pizza/5 HTTP/1.1
Host: localhost:5207
```

**Response — 204 No Content**

The response has no body. A follow-up `GET /pizza/5` returned `404 Not Found`.

## 3. Sales summary function

Text copy of `WriteSalesSummary` from
[`mslearn-dotnet-files/Program.cs`](./mslearn-dotnet-files/Program.cs):

```csharp
void WriteSalesSummary(IEnumerable<string> salesFiles, double salesTotal)
{
    File.WriteAllText(Path.Combine(salesTotalDir, "Sales Summary.txt"), $"Sales Summary{Environment.NewLine}-----------------------------{Environment.NewLine}Total Sales: {salesTotal:C}{Environment.NewLine}Details:{Environment.NewLine}");

    foreach (var file in salesFiles)
    {
        string fileData = File.ReadAllText(file);

        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(fileData);

        string fileSummaryLine = $"{Path.GetFileName(file)}: {data?.Total ?? 0:C}";

        File.AppendAllText(Path.Combine(salesTotalDir, "Sales Summary.txt"), $"{fileSummaryLine}{Environment.NewLine}");
    }
}
```