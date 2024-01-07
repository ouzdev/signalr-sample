SignalR Sample
SignalR Nedir ?

Real time communication

Yaptığı iş -> Abstraction Over Transport -> RPC

Browser HTML5 deskteliyorsa Web Socket değil ise Long Polling yapılır ( Internet Exploler 8 ve öncesi)
Connection sırasında jsonp parametresi true gönbderilmiş ise Long Polling yapılır.
Cross domain SignalR endpoint ile host edilen sayfanın domaini aynı değil ise :

```Server, Client Websocket’i desteklemez ise ve client CORS (Cross-Origin Resource Sharing) desteklemez ise Long Polling yapılır.```

Long polling -> Sunucuya isteği atar fakat sunucu tarafından cevap dönmesi için yeni bir istek yapılmasını bekler.




