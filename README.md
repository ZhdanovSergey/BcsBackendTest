# BcsBackendTest

В ответе метода GET /list реализована свертка идущих подряд сообщений одного автора. Вместо примера, данного в описании ТЗ

```
{
  "status": 1,
  "items": [
    {
      "name": "John",
      "messages": [ "Hello!" ]
    },
    {
      "name": "John",
      "messages": [ "How R U?" ]
    }
  ]
}
```
сделано так:
```
{
  "status": 1,
  "items": [
    {
      "name": "John",
      "messages": [ "Hello!", "How R U?" ]
    }
  ]
}
```
В противном случае непонятно, почему поле `messages` должно быть массивом
