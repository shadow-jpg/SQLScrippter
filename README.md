# SQLScrippter
Стэк
Net 9.0
PostgreSQL
XUnit
NSubstitute(Пока не используется)
BenchMark(Пока не используется)
NLog(временно пока не переведено в библиотеку)


Основа Скриптера/ORM для языка postgresql

функционал для PostgreSQL:
Update( со словарями)
Upsert
Генерация Unique Key

Ошибка на случай не нахождения файла конфигурации пользователя.
Critical Exception на случай аналогичных критических ошибок( нет подключения к бд(рассмотреть как вариант))
различные языки для вывода ошибок и логгирования.




Требуется

PostgreSQL:

Допилить
(В процессе) Генерация Unique Constraint 

Сделать:
Обработчик ошибок по Update и по Входным данным
Создание Foreign Key
Создание View


Разбиение на виды скриптов / возможность генерировать и модифицировать код на основе C# функций
(последовательность дейстивий)
Добавлены переменные для логирывания на нескольких языках




В целом:
Генерация Таблиц
Модификация Таблиц
Отслеживание Наличие Unique Key 
Отслеживание целостности базы
Поиск файла конфигурации
Считывание файла конфигурации
Классы от которых будет наследовать ORM
Класс для скриптера или ветка
Работа с контейнером

ДОБАВИТЬ КОНФИГУРАЦИЮ ВНУТРЕННЮЮ ДЛЯ МОДФИКАЦИИ КОДА ВНУТРИ ЯЗЫКА
Добавить MySql
Добавить T-SQL
АЛГОРИТМ МИГРАЦИИ



нынешний пример вывода:
UPDATE source_temp t
SET recallid = s.ID
FROM recall s
WHERE s.lowName = t.lowname;


UPDATE source_temp t
SET recallid = s.ID
FROM recall
LEFT JOIN strat ms
        ON ms.recallId=s.id
WHERE s.lowName = t.lowname or s.Naming = t.lowname;



INSERT INTO awaw(
recallid,
gtp
)
SELECT
recallid,
gtp
FROM source_temp
ON CONFLICT ON CONSTRAINT()
DO UPDATE
        SET
gtp =EXCLUDED.gtp
recallid =EXCLUDED.recallid