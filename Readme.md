##РАЗВЕРТЫВАНИЕ И РАБОТА С БД

1) Запусти pgAdmin

2) открой слева вкладку Login/Group Roles

3) ПКМ по вкладке -> Create -> Login/Group Role..

4) Name = developer , password = magazinchik_forever

5) Привилегии пока проставь все.



##БЕКАП/РЕСТОР БД

1) Как настроить Postgres:

В левом верхнем углу - File -> Preferences
скроллишь вниз, ищешь в окошке раздел Paths
Кликаешь на Binary Paths
В разделе Postgresql binary path во вкладке своей версии постгреса 
настраиваешь его Binary Path. Это должен быть путь к папке bin установленного у тебя постгреса.
У меня это, например, E:\PostgreSQL\15\bin. 

Теперь у тебя будут работать backup и restore


2) Как развернуть у себя БД из файла:

создай у себя в постгресе пустую БД MagazinchikDB
ПКМ -> Restore...
В окошке следующие настройки:
Format - custom or tar
Filename - путь к файлу MagazinchikDB.sql

После этого в твою бд загрузится все из файла



3) Как сохранить изменения в файл:

MagazinchikDB -> ПКМ -> Backup...
В окошке следующие настройки:
Filename - MagazinchikDB.sql
Format - tar (чтобы все в один файл сгружалось)
Encoding лично я никакой не выставлял.

После выполнения операции рядом возникнет зеленое окошко, можно кликнуть View Processes
и откроется окно с процессами. Там кликаешь на иконку View details, и в деталях можно узнать, куда сохранился файл.


4) Важно!

Если ты попытаешься заресторить БД, то удали из нее все внутренности. Иначе при ресторе возникнет ошибка из-за попытки создать таблицу с уже существующим именем


##РАЗВЕРТЫВАНИЕ И РАБОТА С СЕРВЕРОМ

у тебя должен стоять дотнет 7 версии и си шарп 11.

1) заходишь в консоли в папку Server
2) пишешь команду dotnet restore
3) пишешь команду dotnet run Program.cs
4) Сервер запустится и все будет ок

NB: Сервер сам создаст БД и таблицу в ней, если не найдет готовую

##Гайд по работе со сваггером

При запущенном сервере набери в адресной строке http://localhost:5000/swagger/index.html

Получишь очень удобную страницу, где расписан весь имеющийся API.


