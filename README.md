Веб-приложение для проведения опросов. Администратор сайта создавает опросы и добавляет к ним любое количество опций. Опрос может иметь дату завершения, комментарии пользователей.
Каждая опция может содержать текст, фотографию и аудиофайл (как что-то одно, так и все сразу). Администратор может досрочно завершить опрос, разрешить/запретить комментарии, изменять опции и пр.

Пользователи могут голосовать за выбранную опцию и просматривать список проголосовавших. Допускается голосовать анонимно.

На главной странице представлен список всех опросов (имеется навигация на следующую и предыдущую страницы). Реализован поиск по названию опроса с выводом автозаполнения по уже существующим опросам со схожим названием, а также фильтарция по свойствам: активный/неактивный опрос; опросы, в которых авторизованный пользователь голосовал/не голосовал.

Приложение состоит из web api и клиентского MVC-приложения.
Для аутентификации пользователей используется библиотека Identity. 
Рассылка комментариев реализуется с помощью SignalR.

Структура БД:
Стандартные таблицы библиотеки Identity (в таблице AspNetUsers добавлены дополнительные атрибуты пользователей - Avatar, Name) 

Другие таблицы:

comments: 
id	poll_id	user_id	text

poll_options: 
id	poll_id audio	photo	text

polls: 
id	title	start_date	end_date	is_active	allow_comments

votes: 
id	poll_option_id	user_id	is_anon
