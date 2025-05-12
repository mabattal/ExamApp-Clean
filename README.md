# 📝 ExamApp - Online Sınav Sistemi

**ExamApp**, modern yazılım mimarileri ve güvenlik standartları kullanılarak geliştirilmiş, çok rollü (Admin, Instructor, Student) bir çevrim içi sınav uygulamasıdır. Kullanıcılar sınav oluşturabilir, katılabilir ve sonuçlarını görüntüleyebilir. Proje backend tarafında **.NET 8 Web API** ile önce N-Tier achitecture ile geliştirilmiş daha sonra clean architecture'a dönüştürülmüştür.
N-Tier Architecture reposuna [buradan](https://github.com/mabattal/ExamApp-N-Tier) ulaşabilirsiniz.

---

## 🚀 Özellikler

- ✅ JWT tabanlı kimlik doğrulama ve yetkilendirme
- ✅ Role-based erişim kontrolü
- ✅ Kullanıcı, sınav, soru ve sonuç yönetimi
- ✅ UTC zaman desteği ve zamanlama doğruluğu
- ✅ Mobil ve web uyumlu frontend (Angular ile geliştirilecek)
- ✅ Geliştirici dostu `.http` test dosyası

---

## 🧰 Kullanılan Teknolojiler

| Katman | Teknoloji |
|-------|-----------|
| Backend | .NET 8 Web API |
| ORM | Entity Framework Core |
| Veritabanı | Microsoft SQL Server |
| Kimlik Doğrulama | JWT (Bearer Tokens) |
| Test | Postman, HTTP Client (`.http` dosyası) |
| Logging | Console + (Genişletilebilir) |
| Mapping | Auto Mapper |
| Validasyon | Fluent Validation |
| Yapı | RESTful API, SOLID, N-Tier Architecture prensipleri |

---

## 💡 Tasarım Yaklaşımları

- 🧱 **SOLID Prensipleri**: Esnek, test edilebilir ve sürdürülebilir kod.
- 🔄 **Dependency Injection**: Tüm servisler ve repository'ler DI container aracılığıyla yönetilir.
- ♻️ **Clean Code**: Anlaşılır isimlendirme, tek sorumluluk ilkesi ve modüler yapı.
- 🌐 **DTO ve Mapping**: Veri taşımak için DTO sınıfları, dönüşüm için AutoMapper kullanımı.

---

## ⚙️ Kurulum Adımları

### 1. Veritabanı Ayarı

`appsettings.json` dosyasındaki bağlantı cümlesini kendi sisteminize göre düzenleyin:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ExamAppDb;Trusted_Connection=True;"
}
 ``` 

### 2. Migration ve Veritabanı Oluşturma
```json
dotnet ef database update
``` 

### 3. Projeyi Çalıştırma
```json
dotnet run
``` 

Varsayılan çalışma adresi: https://localhost:7091

🔐 Kimlik Doğrulama
Giriş Örneği
```json
POST /api/Auth/Login
Content-Type: application/json

{
  "email": "admin@admin.com",
  "password": "Admin123"
}
``` 

JWT Token Kullanımı
```json
Authorization: Bearer {token}
``` 
🧪 HTTP Test Dosyası
ExamApp.http dosyasını kullanarak VS Code gibi editörlerde test yapılabilir.

👤 Roller ve Yetkiler
| Rol        | Yetkiler                          |
| ---------- | --------------------------------- |
| Admin      | Kullanıcı yönetimi, tüm işlemler  |
| Instructor | Sınav oluşturma, değerlendirme    |
| Student    | Sınava katılım, sonuç görüntüleme |


📌 Notlar:

⏱️ Tüm saatler UTC formatında kaydedilir, gerektiğinde Türkiye saatine çevrilir.

🌐 API'ler REST mimarisine uygun olarak tasarlanmıştır.

⚠️ CORS ayarları Program.cs içinde yapılandırılmıştır.



🌐 Frontend (Angular) uygulamasına [buradan](https://github.com/mabattal/ExamApp-UI-A)
 ulaşabilirsiniz.


💡 Katkı Sağlamak
Proje herkese açıktır. Pull request'ler ve issue bildirimleri memnuniyetle karşılanır.


