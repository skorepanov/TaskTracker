# TaskTracker - Планировщик задач

## Компоненты приложения

- Backend - ASP.NET Web API, Entity Framework.
- Frontend - TypeScript, React, MobX, Ant Design.
- База данных - PostgreSQL.

## Необходимые компоненты для запуска приложения

- Docker.
- Docker Compose.

## Запуск приложения

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
