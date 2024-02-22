# Dokumentacja aplikacji komisu samochodowego

## Lista technologii wykorzystanych w projekcie

- **ASP.NET Core 6**: Framework do tworzenia aplikacji webowych i API.
- **Entity Framework Core**: ORMapper do obsługi bazy danych.
- **Microsoft SQL Server**: System zarządzania bazą danych.
- **Razor Pages**: Technologia do budowania interfejsu użytkownika.
- **Bootstrap 4**: Framework CSS do tworzenia responsywnych layoutów.
- **xUnit**: Framework do testów jednostkowych.
- **Moq**: Biblioteka do mockowania w testach jednostkowych.
- **Visual Studio 2019/2022**: Zintegrowane środowisko programistyczne (IDE) do tworzenia, debugowania i testowania aplikacji.

## Dane przykładowych użytkowników

| Rola        | Email             | Hasło     |
|-------------|-------------------|-----------|
| Admin       | admin@admin.com   | Admin123! |
| Użytkownik  | user@user.com     | User123!  |

## Jak uruchomić aplikację

### Przygotowanie bazy danych

1. Upewnij się, że masz zainstalowany SQL Server.
2. Użyj migracji EF Core do utworzenia bazy danych: Otwórz konsolę menedżera pakietów (PMC) w Visual Studio i wykonaj `Update-Database`.

### Konfiguracja projektu

1. Otwórz rozwiązanie w Visual Studio.
2. Upewnij się, że projekty są poprawnie skonfigurowane do korzystania z lokalnej bazy danych SQL Server (sprawdź `appsettings.json`).

### Uruchomienie aplikacji

1. Wybierz projekt startowy w Visual Studio.
2. Naciśnij F5 lub kliknij Start w Visual Studio, aby uruchomić aplikację.

## Opis funkcji

### Przeglądanie samochodów

- Użytkownicy mogą przeglądać dostępne samochody w komisie.

### Szczegóły samochodu

- Możliwość wyświetlenia szczegółowych informacji o samochodzie.

### Dodawanie nowego samochodu (tylko dla Admina)

- Administrator może dodać nowy samochód do bazy danych.

### Edycja samochodu (tylko dla Admina)

- Administrator może edytować informacje o samochodach w bazie danych.

### Usuwanie samochodu (tylko dla Admina)

- Administrator ma możliwość usunięcia samochodu z bazy danych.

### Zarządzanie klientami (tylko dla Admina)

- Administrator może zarządzać listą klientów: dodawać nowych, edytować istniejących, usuwać.

### Zarządzanie sprzedażami (tylko dla Admina)

- Administrator może zarządzać sprzedażami, łącznie z dodawaniem nowych transakcji, edytowaniem i usuwaniem istniejących.

## Testowanie

Aby przeprowadzić testy jednostkowe, użyj xUnit i Moq do symulacji interakcji z bazą danych. Testy można uruchomić z poziomu Visual Studio, używając Test Explorera.
