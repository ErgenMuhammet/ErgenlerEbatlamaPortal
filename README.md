# Carpentry Connect: B2B Marketplace & ERP System

Bu proje; marangozlar, ebatlama merkezleri ve fason montajcıları tek bir platformda buluşturan, aynı zamanda işletmelerin stok ve ön muhasebe süreçlerini yönetmelerini sağlayan kapsamlı bir kurumsal çözüm portalıdır.

## 🚀 Öne Çıkan Özellikler
- **B2B İletişim Portalı:** Sektör paydaşları arasında iş emri ve fason montaj taleplerinin yönetimi.
- **Stok Takibi:** Hammadde ve ürün stoklarının gerçek zamanlı izlenmesi.
- **Ön Muhasebe:** Gelir-gider takibi, cari hesap yönetimi ve fatura süreçleri.
- **Real-Time Bildirimler:** Yeni sipariş veya durum güncellemelerinde anlık bilgilendirme.

## 🛠️ Teknik Yığın (Tech Stack)
- **Framework:** .NET 8 / ASP.NET Core Web API
- **Architecture:** Onion Architecture (Clean Architecture), CQRS Pattern
- **Libraries:** MediatR, FluentValidation, AutoMapper
- **Security:** ASP.NET Core Identity, JWT Authentication & Role-Based Authorization
- **Database:** Entity Framework Core (PostgreSQL/SQL Server)
- **Communication:** SignalR (Real-time updates), SMTP Email Service
- **UI:** React.js (veya kullandığın diğer bir framework)

## 🏗️ Mimari Yapı
Proje, sürdürülebilir ve test edilebilir bir yapı sunan **Onion Architecture** prensiplerine dayanmaktadır:
- **Domain:** Entityler ve temel kurallar.
- **Application:** CQRS (Commands & Queries) ve iş mantığı.
- **Infrastructure:** E-mail servisi ve dış entegrasyonlar.
- **Persistence:** Veritabanı konfigürasyonları ve repositoryler.
- **WebAPI:** Endpoint yönetimi ve middleware yapılandırmaları.


