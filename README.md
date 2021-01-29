# PhishDetector
A program that uses phishing page detection methods

![Alt text](https://i.imgur.com/3UWQop0.png "Скриншот программы")

Скрипт, который получает дополнительную информацию о фиш-кете, а точнее:
- получает ip (для дальнейшего использования)
- достает директории из данной ссылки
- форматирует ссылку для получения домена
- ищет “Index Of /” директории, путем перебора найденных на странице, также пробует кастомные
директории “upload” “uploads” “log” “logs”
- проверяет статус код страниц (оставляет только 200)
- проверяет на наличие формы для отправки POST запроса
- фаззер страниц с POST формами (работает только с кнопкой &lt;button&gt; и не имеет адаптацию под
поля)
- проверка страниц на наличие текстов и подбор возможных оригиналов – используется Mysearch
так как он не имеет капчи и индексация подобна google.com

- проверяет все найденные открытые директории на файлы

- Сохранение всех данных в отдельный лог (output_infos.txt) с удобными метками.
Метки :
[Domain] – домен цели
[ip] – ip адрес цели
[POSSIBLE] – возможные оригиналы
[FILE] – найденные файлы
[200] – страницы со статус кодом 200
[200-index] – найденная страница Index of/
[200-post] – найденная страница с формой для POST запроса
[TEXTS] – текста, найденные на страницах
[link] – найденные на странице ссылки

Для функции фаззера и для поиска возможных оригиналов используетcя PhantomJS. Скрипт
работает на большинстве сайтов.
Инструкция:
- Запустить, подождать до появления лого.
- Нажать цифру 1 для ввода целевой ссылки.
- Нажать 2 для начала сбора.
