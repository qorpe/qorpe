# Contributing

Teşekkürler! Küçük bir rehber:

## Branch adı
`feature|fix|chore|docs|refactor/<kısa-konu-kebab>`

## PR başlığı — Conventional Commits
Örn: `feat: pricing sayfası`

## PR içeriği
- PR açıklamasında **Fixes #<issueId>** bulunmalı (Linked issue zorunludur)
- En az 1 reviewer onayı
- CI (build+lint+test) geçmeli
- Yorumlar çözümlenmeli

## Geliştirme
- **qorpe.com (Next.js)**: Node 20+, `npm ci`, `npm run dev`
- **qorpe (.NET)**: .NET 8/9, `dotnet restore`, `dotnet build`

## Kod standartları
- Web: ESLint + Prettier
- C#: analyzers + nullable enable
- EditorConfig kullanın
