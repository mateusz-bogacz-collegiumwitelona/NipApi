# 🧾 Biała Lista VAT

Projekt demonstracyjny do sprawdzenia działania API MF.

## Wymagania

- Docker
- Docker Compose

## Uruchomienie

1. Zbuduj kontener:

```bash
docker compose build
```

2. Uruchom kontener:

```bash
docker compose up -d
```

3. Wyłącz kontener:

```bash
docker compose down
```

## Dostęp

- **Swagger (API)**: [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)
- **Blazor (frontend)**: [http://localhost:5001](http://localhost:5001)

## Problemy

Jeżeli strona nie wyświetla się poprawnie (braki w HTML), spróbuj:

- Wyczyścić cookies w przeglądarce
- Lub wymusić odświeżenie: `Ctrl + F5`

> Typowy błąd Blazor związany z ciastkami.
