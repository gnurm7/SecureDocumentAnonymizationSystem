# 📄 Secure Document Anonymization API

Akademik makaleleri yüklemek, anonimleştirmek ve değerlendirme sürecini yönetmek için geliştirilmiş .NET Web API.

## 🚀 Özellikler

- Makale yükleme ve revize etme
- Anonimleştirilmiş PDF yönetimi
- Editör-hakem atama ve değerlendirme
- Yazar-editör mesajlaşma
- Takip numarası ile durum sorgulama

## 🔧 Endpoint Örnekleri

### Makale Yükle  
`POST /api/FileUpload`  
**FormData:** `file`, `email`

### Makale Durumu  
`GET /api/FileUpload/status/{trackingNumber}?email={email}`

### Mesaj Gönder  
`POST /api/Message`  
```json
{
  "TrackingNumber": "abc123",
  "Text": "Merhaba",
  "Sender": "author"
}
```

## ⚙️ Kurulum

```bash
dotnet restore
dotnet ef database update
dotnet run
```

## 📂 Not
Yüklenen dosyalar `upload/` klasöründe saklanır.

