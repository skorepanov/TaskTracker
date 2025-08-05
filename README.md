# TaskTracker

Приложение для планирования задач.

## Компоненты приложения

- Backend - ASP.NET Web API, Entity Framework.
- Frontend - TypeScript, React, MobX, Ant Design.
- База данных - PostgreSQL.

## Необходимые компоненты для запуска приложения

- Docker.
- Docker Compose.

## Запуск приложения с помощью Docker

1. Склонировать репозиторий.
2. Перейти в директорию `docker`.
3. Выполнить команду для запуска приложения:

```
docker-compose up --build
```

- Эта команда соберёт Docker-образы и запустит контейнеры для backend, frontend и БД.
- При запуске backend будет создана БД и будут применены миграции.

## Доступ к приложению

- Backend будет доступен по адресу: `http://localhost:7265`.
- Frontend будет доступен по адресу: `http://localhost:3000`.
- База данных будет доступна по адресу: `http://localhost:5433`.

## Документация API

Подключается только в Development-окружении, поэтому недоступна при запуске через Docker.

- Scalar: `https://localhost:7265/scalar/v1`.
- Swagger: `https://localhost:7265/swagger/index.html`.

## Тестирование

- Модульные тесты.
    - Проект: `TaskTracker.UnitTests`.
    - Технологии: xUnit, FleuntAssertions.
    - Тестируют только бизнес-модели, в изоляции от бизнес-сервисов и внешних зависимостей.
- Интеграционные тесты.
    - Проект: `TaskTracker.IntegrationTests`.
    - Технологии: xUnit, Moq, FluentAssertions, Respawn.
    - Тестируют backend от Web API до БД включительно.
    - При первом запуске, при необходимости, создают БД и применяют миграции.
    - Тестовые данные удаляются из БД между тестами с помощью Respawn.
